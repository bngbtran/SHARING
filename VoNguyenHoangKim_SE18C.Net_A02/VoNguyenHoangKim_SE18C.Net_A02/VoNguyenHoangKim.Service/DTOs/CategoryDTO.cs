using VoNguyenHoangKim.Common.Enums;

namespace VoNguyenHoangKim.Service.DTOs
{
    public class CategoryDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
    }
}