using VoNguyenHoangKim.Service.DTOs;

namespace VoNguyenHoangKim.Service.ValidateReceive
{
    public class TagResponse
    {
        public TagDTO Tag { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }

    }
}