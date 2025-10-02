using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoNguyenHoangKim.Data.Models
{
    public class Tag
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<NewsTag> NewsTags { get; set; }
    }
}
