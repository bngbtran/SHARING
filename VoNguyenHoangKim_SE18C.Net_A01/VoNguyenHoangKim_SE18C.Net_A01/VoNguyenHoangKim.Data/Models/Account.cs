namespace VoNguyenHoangKim.Data.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public virtual ICollection<NewsArticle> NewsArticles { get; set; }
    }
}
