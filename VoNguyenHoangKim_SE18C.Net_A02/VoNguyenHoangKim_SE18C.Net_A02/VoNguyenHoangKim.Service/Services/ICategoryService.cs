using VoNguyenHoangKim.Service.DTOs;
using VoNguyenHoangKim.Service.ValidateReceive;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.Service.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponse> GetByIdAsync(string id);
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryResponse> CreateAsync(CategoryRequest request);
        Task<CategoryResponse> UpdateAsync(string id, CategoryRequest request);
        Task<CategoryResponse> DeleteAsync(string id);
    }
}