namespace VoNguyenHoangKim.Data.Models
{
    public class NewsArticle
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string NewsSource { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Status { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public string? UpdatedById { get; set; }
        public virtual Category Category { get; set; }
        public virtual Account Account { get; set; }
        public virtual Account UpdatedBy { get; set; }
        public virtual ICollection<NewsTag> NewsTags { get; set; }
    }
}
