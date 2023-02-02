using System.ComponentModel.DataAnnotations;

namespace JWT_AspCoreApi.Model
{
    public class Login
    {
        [Required(ErrorMessage = "User name is requierd")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "User password is requierd")]
        public string? Password { get; set; }
    }
}
