using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models
{
    public class UserRegister
    {
        [Required,EmailAddress]
        public string email { get; set; } = string.Empty;
        [Required,MinLength(5,ErrorMessage="Please enter more than 5 letters")]
        public string password { get; set; } = string.Empty;
        [Required,Compare("password")]
        public string confirmPassword { get; set; } = string.Empty;
        [Required,Phone]
        public string mobile_no { get; set; } = string.Empty;
        [Required]
        public string first_name { get; set; } = string.Empty;
        [Required]
        public string last_name { get; set; } = string.Empty;
        //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
