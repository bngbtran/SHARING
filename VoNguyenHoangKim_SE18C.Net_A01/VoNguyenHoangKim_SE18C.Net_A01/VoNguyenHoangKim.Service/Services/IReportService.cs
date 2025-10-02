using VoNguyenHoangKim.Service.DTOs;
using VoNguyenHoangKim.Service.ValidateReceive;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.Service.Services
{
    public interface IReportService
    {
        Task<IEnumerable<NewsArticleDTO>> GetReportAsync(DateTime startDate, DateTime endDate);
    }
}