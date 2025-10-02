using VoNguyenHoangKim.Common.Constants;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Data.Repositories;
using VoNguyenHoangKim.Service.DTOs;
using VoNguyenHoangKim.Common.Validate;
using VoNguyenHoangKim.Service.ValidateReceive;
using VoNguyenHoangKim.Service.ValidateSend;
using VoNguyenHoangKim.Data.Models;

namespace VoNguyenHoangKim.Service.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly INewsTagRepository _newsTagRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IdGenerator _idGenerator;

        public NewsArticleService(
            INewsArticleRepository newsArticleRepository,
            ICategoryRepository categoryRepository,
            IAccountRepository accountRepository,
            INewsTagRepository newsTagRepository,
            ITagRepository tagRepository,
            IdGenerator idGenerator)
        {
            _newsArticleRepository = newsArticleRepository;
            _categoryRepository = categoryRepository;
            _accountRepository = accountRepository;
            _newsTagRepository = newsTagRepository;
            _tagRepository = tagRepository;
            _idGenerator = idGenerator;
        }

        public async Task<NewsArticleResponse> GetByIdAsync(string id)
        {
            var article = await _newsArticleRepository.GetByIdAsync(id);
            if (article == null)
            {
                return new NewsArticleResponse { Success = false, Error = "Not Found !!!" };
            }

            var newsTags = await _newsTagRepository.GetByNewsArticleIdAsync(id);
            var tags = await _tagRepository.GetAllAsync();

            var articleDTO = new NewsArticleDTO
            {
                Id = article.Id,
                Title = article.Title,
                Headline = article.Headline,
                Content = article.Content,
                NewsSource = article.NewsSource,
                CreatedDate = article.CreatedDate,
                ModifiedDate = article.ModifiedDate,
                Status = (Status)article.Status,
                CategoryId = article.CategoryId,
                CategoryName = article.Category?.Name,
                AccountId = article.AccountId,
                AccountFullName = article.Account?.FullName,
                UpdatedById = article.UpdatedById,
                UpdatedByFullName = article.UpdatedBy?.FullName,
                TagIds = newsTags.Select(nt => nt.TagId).ToList(),
                TagNames = newsTags.Select(nt => tags.FirstOrDefault(t => t.Id == nt.TagId)?.Name).ToList()
            };

            return new NewsArticleResponse { Success = true, NewsArticle = articleDTO };
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetAllAsync()
        {
            var articles = await _newsArticleRepository.GetAllAsync();
            var tags = await _tagRepository.GetAllAsync();

            return articles.Select(a =>
            {
                var newsTags = a.NewsTags.ToList();
                return new NewsArticleDTO
                {
                    Id = a.Id,
                    Title = a.Title,
                    Headline = a.Headline,
                    Content = a.Content,
                    NewsSource = a.NewsSource,
                    CreatedDate = a.CreatedDate,
                    ModifiedDate = a.ModifiedDate,
                    Status = (Status)a.Status,
                    CategoryId = a.CategoryId,
                    CategoryName = a.Category?.Name,
                    AccountId = a.AccountId,
                    AccountFullName = a.Account?.FullName,
                    UpdatedById = a.UpdatedById,
                    UpdatedByFullName = a.UpdatedBy?.FullName,
                    TagIds = newsTags.Select(nt => nt.TagId).ToList(),
                    TagNames = newsTags.Select(nt => tags.FirstOrDefault(t => t.Id == nt.TagId)?.Name).ToList()
                };
            });
        }

        public async Task<NewsArticleResponse> CreateAsync(NewsArticleRequest request)
        {
            if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Content) ||
                string.IsNullOrEmpty(request.CategoryId) || string.IsNullOrEmpty(request.AccountId))
            {
                return new NewsArticleResponse { Success = false, Error = "Invalid Input !!!" };
            }

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            var account = await _accountRepository.GetByIdAsync(request.AccountId);
            if (category == null || account == null)
            {
                return new NewsArticleResponse { Success = false, Error = "Not Found !!!" };
            }

            var lastId = await _newsArticleRepository.GetLastIdAsync();
            var newId = _idGenerator.GenerateId(IdPrefix.NewsArticle, lastId);

            var article = new NewsArticle
            {
                Id = newId,
                Title = request.Title,
                Headline = request.Headline,
                Content = request.Content,
                NewsSource = request.NewsSource,
                CreatedDate = request.CreatedDate,
                ModifiedDate = null,
                Status = (int)request.Status,
                CategoryId = request.CategoryId,
                AccountId = request.AccountId,
                UpdatedById = null
            };

            await _newsArticleRepository.AddAsync(article);

            foreach (var tagId in request.TagIds)
            {
                var tag = await _tagRepository.GetByIdAsync(tagId);
                if (tag != null)
                {
                    var newsTagId = _idGenerator.GenerateId(IdPrefix.NewsTag, await _newsTagRepository.GetLastIdAsync());
                    var newsTag = new NewsTag
                    {
                        Id = newsTagId,
                        NewsArticleId = newId,
                        TagId = tagId
                    };
                    await _newsTagRepository.AddAsync(newsTag);
                }
            }

            var articleDTO = new NewsArticleDTO
            {
                Id = article.Id,
                Title = article.Title,
                Headline = article.Headline,
                Content = article.Content,
                NewsSource = article.NewsSource,
                CreatedDate = article.CreatedDate,
                ModifiedDate = article.ModifiedDate,
                Status = (Status)article.Status,
                CategoryId = article.CategoryId,
                CategoryName = category.Name,
                AccountId = article.AccountId,
                AccountFullName = account.FullName,
                UpdatedById = article.UpdatedById,
                UpdatedByFullName = null,
                TagIds = request.TagIds,
                TagNames = (await _tagRepository.GetAllAsync()).Where(t => request.TagIds.Contains(t.Id)).Select(t => t.Name).ToList()
            };

            return new NewsArticleResponse { Success = true, NewsArticle = articleDTO };
        }

        public async Task<NewsArticleResponse> UpdateAsync(string id, NewsArticleRequest request)
        {
            var article = await _newsArticleRepository.GetByIdAsync(id);
            if (article == null)
            {
                return new NewsArticleResponse { Success = false, Error = "Not Found !!!" };
            }

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            var account = await _accountRepository.GetByIdAsync(request.AccountId);
            var updatedBy = await _accountRepository.GetByIdAsync(request.UpdatedById);
            if (category == null || account == null || (request.UpdatedById != null && updatedBy == null))
            {
                return new NewsArticleResponse { Success = false, Error = "Not Found !!!" };
            }

            article.Title = request.Title;
            article.Headline = request.Headline;
            article.Content = request.Content;
            article.NewsSource = request.NewsSource;
            article.CreatedDate = request.CreatedDate;
            article.ModifiedDate = DateTime.Now;
            article.Status = (int)request.Status;
            article.CategoryId = request.CategoryId;
            article.AccountId = request.AccountId;
            article.UpdatedById = request.UpdatedById;

            await _newsArticleRepository.UpdateAsync(article);

            var existingNewsTags = await _newsTagRepository.GetByNewsArticleIdAsync(id);
            foreach (var newsTag in existingNewsTags)
            {
                await _newsTagRepository.DeleteAsync(newsTag.Id);
            }
            foreach (var tagId in request.TagIds)
            {
                var tag = await _tagRepository.GetByIdAsync(tagId);
                if (tag != null)
                {
                    var newsTagId = _idGenerator.GenerateId(IdPrefix.NewsTag, await _newsTagRepository.GetLastIdAsync());
                    var newsTag = new NewsTag
                    {
                        Id = newsTagId,
                        NewsArticleId = id,
                        TagId = tagId
                    };
                    await _newsTagRepository.AddAsync(newsTag);
                }
            }

            var articleDTO = new NewsArticleDTO
            {
                Id = article.Id,
                Title = article.Title,
                Headline = article.Headline,
                Content = article.Content,
                NewsSource = article.NewsSource,
                CreatedDate = article.CreatedDate,
                ModifiedDate = article.ModifiedDate,
                Status = (Status)article.Status,
                CategoryId = article.CategoryId,
                CategoryName = category.Name,
                AccountId = article.AccountId,
                AccountFullName = account.FullName,
                UpdatedById = article.UpdatedById,
                UpdatedByFullName = updatedBy?.FullName,
                TagIds = request.TagIds,
                TagNames = (await _tagRepository.GetAllAsync()).Where(t => request.TagIds.Contains(t.Id)).Select(t => t.Name).ToList()
            };

            return new NewsArticleResponse { Success = true, NewsArticle = articleDTO };
        }

        public async Task<NewsArticleResponse> DeleteAsync(string id)
        {
            var article = await _newsArticleRepository.GetByIdAsync(id);
            if (article == null)
            {
                return new NewsArticleResponse { Success = false, Error = "Not Found !!!" };
            }

            var newsTags = await _newsTagRepository.GetByNewsArticleIdAsync(id);
            foreach (var newsTag in newsTags)
            {
                await _newsTagRepository.DeleteAsync(newsTag.Id);
            }

            await _newsArticleRepository.DeleteAsync(id);
            return new NewsArticleResponse { Success = true };
        }

        public async Task<ServiceResponse<int>> GetCountByCategoryAsync(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                return new ServiceResponse<int> { Success = false, Error = "Invalid Input !!!" };
            }

            var count = await _newsArticleRepository.GetCountByCategoryAsync(categoryId);
            return new ServiceResponse<int> { Success = true, Data = count };
        }

        public async Task<ServiceResponse<int>> GetCountByTagAsync(string tagId)
        {
            if (string.IsNullOrEmpty(tagId))
            {
                return new ServiceResponse<int> { Success = false, Error = "Invalid Input !!!" };
            }

            var newsTags = await _newsTagRepository.GetAllAsync();
            var count = newsTags.Count(nt => nt.TagId == tagId);
            return new ServiceResponse<int> { Success = true, Data = count };
        }
    }
}