using System.ComponentModel.DataAnnotations;

namespace VoNguyenHoangKim.Service.ValidateSend
{
    public class AccountRequest
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [Range(1, 3)]
        public int Role { get; set; }
    }
}