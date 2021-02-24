using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FortCode.Model.Request
{
    public class AddUserRequest
    {
        [JsonProperty("username")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 4)]
        public string Username { get; set; }

        [JsonProperty("password")]
        [Required(ErrorMessage = "Password is required")]        
        public string Password { get; set; }

        [JsonProperty("email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
