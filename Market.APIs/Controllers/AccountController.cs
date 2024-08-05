using AutoMapper;
using Market.APIs.Dtos;
using Market.APIs.Errors;
using Market.APIs.Extenstions;
using Market.Core.Entities.Identity;
using Market.Core.ServiceContracts;
using Market.Service.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Loader;
using System.Security.Claims;

namespace Market.APIs.Controllers
{

    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuthService authService,
            IMapper mapper) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(loginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new ApiResponse(401, "Login NotValid "));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ApiResponse(401, "Login NotValid "));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token =  await _authService.CreateTokenAsync(user,_userManager)
            });        

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new AppUser()
            {
                 DisplayName = model.DisplayName,
                 Email = model.Email,
                 UserName = model.Email.Split("@")[0],
                 PhoneNumber = model.Phone
            };

            var result = await _userManager.CreateAsync(user,model.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse() { Errors= result.Errors.Select(E => E.Description)});

            return Ok(new UserDto()
            {
                DisplayName = model.DisplayName,
                Email= user.Email,
                Token = await _authService.CreateTokenAsync(user,_userManager)
            });

            
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize]
        [HttpGet("address")]

        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            
            var user = await _userManager.FindUserWithAddressAsync(User);

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<Address>> UpdateAddress(AddressDto address)
        {
            var updatedAddress = _mapper.Map<Address>(address);

            var user = await _userManager.FindUserWithAddressAsync(User);

            updatedAddress.Id = user.Address.Id ;

            user.Address = updatedAddress;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new ApiValidationErrorResponse() { Errors = result.Errors.Select(e => e.Description) });

            return Ok(address);
        }
        [HttpGet("emailexists")]

        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

    }
}
