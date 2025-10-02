using VoNguyenHoangKim.Common;
using VoNguyenHoangKim.Common.Validate;
using VoNguyenHoangKim.Data;

namespace VoNguyenHoangKim.Service.ValidateReceive
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }

    }
}
