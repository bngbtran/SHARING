using VoNguyenHoangKim.Service.DTOs;

namespace VoNguyenHoangKim.Service.Services
{
    public interface IReportService
    {
        Task<IEnumerable<NewsArticleDTO>> GetReportAsync(DateTime startDate, DateTime endDate);
    }
}