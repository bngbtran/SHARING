using VoNguyenHoangKim.Common;
using VoNguyenHoangKim.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoNguyenHoangKim.Common.Validate;
using VoNguyenHoangKim.Data;

namespace VoNguyenHoangKim.Service.ValidateReceive
{
    public class CategoryResponse
    {
        public CategoryDTO Category { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }

    }
}