using Microsoft.EntityFrameworkCore;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Data.Context;
using VoNguyenHoangKim.Service.DTOs;

namespace VoNguyenHoangKim.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly FUNewsManagementDbContext _context;

        public ReportService(FUNewsManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetReportAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.NewsArticles
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate && n.Status == (int)Status.Active)
                .OrderByDescending(n => n.CreatedDate)
                .Select(n => new NewsArticleDTO
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    CreatedDate = n.CreatedDate,
                    Status = (Status)n.Status,
                    CategoryId = n.CategoryId,
                    CategoryName = n.Category.Name,
                    AccountId = n.AccountId,
                    AccountFullName = n.Account.FullName,
                    TagIds = n.NewsTags.Select(nt => nt.TagId).ToList(),
                    TagNames = n.NewsTags.Select(nt => nt.Tag.Name).ToList()
                })
                .ToListAsync();
        }
    }
}