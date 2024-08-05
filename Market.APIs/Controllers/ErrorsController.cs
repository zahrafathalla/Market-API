using Market.APIs.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Market.APIs.Controllers
{
    [Route("error/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] 
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return code switch
            {
                404 => NotFound(new ApiResponse(404)),
                401 => Unauthorized (new ApiResponse(401)),
                _ => StatusCode(code)
            };
        }
    }
}
