using System.ComponentModel.DataAnnotations;

namespace VoNguyenHoangKim.Service.ValidateSend
{
    public class AccountCreateRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [Range(1, 3)]
        public int Role { get; set; }
    }
}