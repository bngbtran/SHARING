using VoNguyenHoangKim.Service.DTOs;
using VoNguyenHoangKim.Service.ValidateReceive;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.Service.Services
{
    public interface ITagService
    {
        Task<TagResponse> GetByIdAsync(string id);
        Task<IEnumerable<TagDTO>> GetAllAsync();
        Task<TagResponse> CreateAsync(TagRequest request);
        Task<TagResponse> UpdateAsync(string id, TagRequest request);
        Task<TagResponse> DeleteAsync(string id);
    }
}