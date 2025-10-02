using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoNguyenHoangKim.Data.Models
{
    public class NewsTag
    {
        public string Id { get; set; }
        public string NewsArticleId { get; set; }
        public string TagId { get; set; }
        public virtual NewsArticle NewsArticle { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
