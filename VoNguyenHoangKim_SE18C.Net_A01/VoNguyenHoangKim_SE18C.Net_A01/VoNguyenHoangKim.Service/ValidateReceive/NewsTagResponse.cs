using VoNguyenHoangKim.Common;
using VoNguyenHoangKim.Common.Validate;
using VoNguyenHoangKim.Data;
using VoNguyenHoangKim.Service.DTOs;

namespace VoNguyenHoangKim.Service.ValidateReceive
{
    public class NewsTagResponse
    {
        public NewsTagDTO NewsTag { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }

    }
}