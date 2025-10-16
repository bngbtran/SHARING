using VoNguyenHoangKim.Common.Constants;
using VoNguyenHoangKim.Common.Validate;
using VoNguyenHoangKim.Data.Models;
using VoNguyenHoangKim.Data.Repositories;
using VoNguyenHoangKim.Service.DTOs;
using VoNguyenHoangKim.Service.ValidateReceive;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.Service.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IdGenerator _idGenerator;

        public TagService(ITagRepository tagRepository, IdGenerator idGenerator)
        {
            _tagRepository = tagRepository;
            _idGenerator = idGenerator;
        }

        public async Task<TagResponse> GetByIdAsync(string id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                return new TagResponse { Success = false, Error = "Not Found !!!" };
            }

            var tagDTO = new TagDTO
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return new TagResponse { Success = true, Tag = tagDTO };
        }

        public async Task<IEnumerable<TagDTO>> GetAllAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags.Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name
            });
        }

        public async Task<TagResponse> CreateAsync(TagRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return new TagResponse { Success = false, Error = "Invalid Input" };
            }

            var lastId = await _tagRepository.GetLastIdAsync();
            var newId = _idGenerator.GenerateId(IdPrefix.Tag, lastId);

            var tag = new Tag
            {
                Id = newId,
                Name = request.Name
            };

            await _tagRepository.AddAsync(tag);

            var tagDTO = new TagDTO
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return new TagResponse { Success = true, Tag = tagDTO };
        }

        public async Task<TagResponse> UpdateAsync(string id, TagRequest request)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                return new TagResponse { Success = false, Error = "Not Found !!!" };
            }

            tag.Name = request.Name;
            await _tagRepository.UpdateAsync(tag);

            var tagDTO = new TagDTO
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return new TagResponse { Success = true, Tag = tagDTO };
        }

        public async Task<TagResponse> DeleteAsync(string id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                return new TagResponse { Success = false, Error = "Not Found !!!" };
            }

            await _tagRepository.DeleteAsync(id);
            return new TagResponse { Success = true };
        }
    }
}