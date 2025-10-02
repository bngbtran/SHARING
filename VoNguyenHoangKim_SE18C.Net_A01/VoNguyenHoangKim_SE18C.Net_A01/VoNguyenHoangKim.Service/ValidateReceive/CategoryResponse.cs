using VoNguyenHoangKim.Service.DTOs;

namespace VoNguyenHoangKim.Service.ValidateReceive
{
    public class CategoryResponse
    {
        public CategoryDTO Category { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }

    }
}