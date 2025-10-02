using System.ComponentModel.DataAnnotations;

namespace VoNguyenHoangKim.Service.ValidateSend
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required !!!")]
        [EmailAddress(ErrorMessage = "Invalid Email !!!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required !!!")]
        public string Password { get; set; }
    }
}