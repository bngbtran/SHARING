using System.ComponentModel.DataAnnotations;

namespace VoNguyenHoangKim.Service.ValidateSend
{
    public class NewsTagRequest
    {
        [Required(ErrorMessage = "Article is required !!!")]
        public string NewsArticleId { get; set; }

        [Required(ErrorMessage = "Tag is required !!!")]
        public string TagId { get; set; }
    }
}