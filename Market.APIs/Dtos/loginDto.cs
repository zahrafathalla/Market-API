using System.ComponentModel.DataAnnotations;

namespace Market.APIs.Dtos
{
    public class loginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
