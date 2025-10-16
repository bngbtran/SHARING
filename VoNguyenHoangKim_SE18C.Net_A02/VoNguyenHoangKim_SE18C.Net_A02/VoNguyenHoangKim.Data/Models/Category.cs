namespace VoNguyenHoangKim.Data.Models
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public virtual ICollection<NewsArticle> NewsArticles { get; set; }
    }
}
