using VoNguyenHoangKim.Service.DTOs;
using VoNguyenHoangKim.Service.ValidateReceive;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.Service.Services
{
    public interface INewsTagService
    {
        Task<NewsTagResponse> GetByIdAsync(string id);
        Task<IEnumerable<NewsTagDTO>> GetByNewsArticleIdAsync(string newsArticleId);
        Task<NewsTagResponse> CreateAsync(NewsTagRequest request);
        Task<NewsTagResponse> UpdateAsync(string id, NewsTagRequest request);
        Task<NewsTagResponse> DeleteAsync(string id);
    }
}