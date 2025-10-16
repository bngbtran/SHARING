using System.ComponentModel.DataAnnotations;

namespace VoNguyenHoangKim.Service.ValidateSend
{
    public class TagRequest
    {
        [Required(ErrorMessage = "Tag Name is required !!!")]        
        public string Name { get; set; }
    }
}