using VoNguyenHoangKim.Common.Constants;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Common.Validate;
using VoNguyenHoangKim.Data.Models;
using VoNguyenHoangKim.Data.Repositories;
using VoNguyenHoangKim.Service.DTOs;
using VoNguyenHoangKim.Service.ValidateReceive;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IdGenerator _idGenerator;

        public CategoryService(ICategoryRepository categoryRepository, IdGenerator idGenerator)
        {
            _categoryRepository = categoryRepository;
            _idGenerator = idGenerator;
        }

        public async Task<CategoryResponse> GetByIdAsync(string id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new CategoryResponse { Success = false, Error = "Not Found !!!" };
            }

            var categoryDTO = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Status = (Status)category.Status
            };

            return new CategoryResponse { Success = true, Category = categoryDTO };
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Description = c.Description,
                Name = c.Name,
                Status = (Status)c.Status
            });
        }

        public async Task<CategoryResponse> CreateAsync(CategoryRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return new CategoryResponse { Success = false, Error = "Invalid Input !!!" };
            }

            var lastId = await _categoryRepository.GetLastIdAsync();
            var newId = _idGenerator.GenerateId(IdPrefix.Category, lastId);

            var category = new Category
            {
                Id = newId,
                Name = request.Name,
                Description = request.Description,
                Status = (int)request.Status
            };

            await _categoryRepository.AddAsync(category);

            var categoryDTO = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description= category.Description,
                Status = (Status)category.Status
            };

            return new CategoryResponse { Success = true, Category = categoryDTO };
        }

        public async Task<CategoryResponse> UpdateAsync(string id, CategoryRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new CategoryResponse { Success = false, Error = "Not Found !!!" };
            }

            category.Name = request.Name;
            category.Description = request.Description;
            category.Status = (int)request.Status;

            await _categoryRepository.UpdateAsync(category);

            var categoryDTO = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = request.Description,
                Status = (Status)category.Status
            };

            return new CategoryResponse { Success = true, Category = categoryDTO };
        }

        public async Task<CategoryResponse> DeleteAsync(string id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new CategoryResponse { Success = false, Error = "Not Found !!!" };
            }

            if (await _categoryRepository.HasNewsArticlesAsync(id))
            {
                return new CategoryResponse { Success = false, Error = "Category Is In Use !!!" };
            }

            await _categoryRepository.DeleteAsync(id);
            return new CategoryResponse { Success = true };
        }
    }
}