using VoNguyenHoangKim.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace VoNguyenHoangKim.Service.ValidateSend
{
    public class NewsArticleRequest
    {
        [Required(ErrorMessage = "Title is required !!!")]
        public string Title { get; set; }
        
        public string Headline { get; set; }

        [Required(ErrorMessage = "Content is required !!!")]
        public string Content { get; set; }
        
        public string NewsSource { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "Status is required !!!")]
        [EnumDataType(typeof(Status), ErrorMessage = "Invalid status !!!")]
        public Status Status { get; set; }

        [Required(ErrorMessage = "Category is required !!!")]
        public string CategoryId { get; set; }
        public string AccountId { get; set; }

        public string UpdatedById { get; set; }

        public List<string> TagIds { get; set; } = new List<string>();
    }
}