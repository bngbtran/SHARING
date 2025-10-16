using VoNguyenHoangKim.Common.Enums;

namespace VoNguyenHoangKim.Service.DTOs
{
    public class NewsArticleDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string NewsSource { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Status Status { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string AccountId { get; set; }
        public string AccountFullName { get; set; }
        public string UpdatedById { get; set; }
        public string UpdatedByFullName { get; set; }
        public List<string> TagIds { get; set; }
        public List<string> TagNames { get; set; }
    }
}