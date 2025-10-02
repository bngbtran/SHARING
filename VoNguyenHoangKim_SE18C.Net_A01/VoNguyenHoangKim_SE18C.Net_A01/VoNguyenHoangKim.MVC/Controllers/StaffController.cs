using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateReceive;
using VoNguyenHoangKim.Service.ValidateSend;
using VoNguyenHoangKim.Data;

namespace VoNguyenHoangKim.MVC.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private readonly IAccountService _accountService;

        public StaffController(
            ICategoryService categoryService,
            INewsArticleService newsArticleService,
            ITagService tagService,
            IAccountService accountService)
        {
            _categoryService = categoryService;
            _newsArticleService = newsArticleService;
            _tagService = tagService;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            var categoriesDto = await _categoryService.GetAllAsync();
            var categories = categoriesDto.Select(c => new CategoryResponse
            {
                Success = true,
                Category = c,
                Error = null
            });
            if (!string.IsNullOrEmpty(search))
            {
                categories = categories.Where(c => c.Category.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            ViewData["CurrentSearch"] = search;
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var response = await _categoryService.CreateAsync(request);
            if (!response.Success)
            {
                ModelState.AddModelError("", "Error !!!");
                return View(request);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(string id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }

            var request = new CategoryRequest
            {
                Name = response.Category.Name,
                Description = response.Category.Description,
                Status = response.Category.Status
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(string id, CategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var response = await _categoryService.UpdateAsync(id, request);
            if (!response.Success)
            {
                ModelState.AddModelError("", "Error !!!");
                return View(request);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var newsCountResponse = await _newsArticleService.GetCountByCategoryAsync(id);
            if (newsCountResponse.Success && newsCountResponse.Data > 0)
            {
                TempData["Error"] = "Category Is In Use !!!";
                return RedirectToAction("Index");
            }

            var response = await _categoryService.DeleteAsync(id);
            if (!response.Success)
            {
                TempData["Error"] = "Error !!!";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ManageNews(string search)
        {
            var articlesDto = await _newsArticleService.GetAllAsync();
            var articles = articlesDto.Select(a => new NewsArticleResponse
            {
                Success = true,
                NewsArticle = a,
                Error = null
            });
            if (!string.IsNullOrEmpty(search))
            {
                articles = articles.Where(a => a.NewsArticle.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> CreateNews()
        {
            ViewBag.Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name");
            ViewBag.Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews(NewsArticleRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("AccountId", "Not Found !!!");
                ViewBag.Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name");
                ViewBag.Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name");
                ViewBag.AccountId = userId;
                return View(request);
            }
            request.AccountId = userId;
            request.UpdatedById = null;

            ModelState.Remove("AccountId");
            ModelState.Remove("UpdatedById");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name");
                ViewBag.Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name");
                ViewBag.AccountId = userId;
                return View(request);
            }

            var response = await _newsArticleService.CreateAsync(request);
            if (!response.Success)
            {
                ModelState.AddModelError("", "Created Failed !!!");
                ViewBag.Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name");
                ViewBag.Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name");
                return View(request);
            }

            TempData["Success"] = "Created Successfully !!!";
            return RedirectToAction("ManageNews");
        }

        [HttpGet]
        public async Task<IActionResult> EditNews(string id)
        {
            var response = await _newsArticleService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }

            var request = new NewsArticleRequest
            {
                Title = response.NewsArticle.Title,
                Headline = response.NewsArticle.Headline,
                Content = response.NewsArticle.Content,
                NewsSource = response.NewsArticle.NewsSource,
                CreatedDate = response.NewsArticle.CreatedDate,
                ModifiedDate = response.NewsArticle.ModifiedDate,
                Status = response.NewsArticle.Status,
                CategoryId = response.NewsArticle.CategoryId,
                AccountId = response.NewsArticle.AccountId,
                UpdatedById = response.NewsArticle.UpdatedById,
                TagIds = response.NewsArticle.TagIds
            };

            ViewBag.Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name", request.CategoryId);
            ViewBag.Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name", request.TagIds);
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> EditNews(string id, NewsArticleRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("AccountId", "Not Found !!!");
                ViewBag.Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name");
                ViewBag.Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name");
                ViewBag.AccountId = userId;
                return View(request);
            }
            request.AccountId = userId;
            request.UpdatedById = userId;
            request.ModifiedDate = DateTime.UtcNow;

            ModelState.Remove("AccountId");
            ModelState.Remove("UpdatedById");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name", request.CategoryId);
                ViewBag.Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name", request.TagIds);
                ViewBag.AccountId = userId;
                return View(request);
            }

            var articleResponse = await _newsArticleService.GetByIdAsync(id);
            if (!articleResponse.Success)
            {
                return NotFound();
            }

            var response = await _newsArticleService.UpdateAsync(id, request);
            if (!response.Success)
            {
                ModelState.AddModelError("", "Update Failed !!!");
                ViewBag.Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name", request.CategoryId);
                ViewBag.Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name", request.TagIds);
                return View(request);
            }

            TempData["Success"] = "Updated Successfully !!!";
            return RedirectToAction("ManageNews");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNews(string id)
        {
            var response = await _newsArticleService.DeleteAsync(id);
            if (!response.Success)
            {
                TempData["Error"] = "Error !!!";
            }

            return RedirectToAction("ManageNews");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _accountService.GetByIdAsync(userId);
            if (!response.Success)
            {
                return NotFound();
            }

            var model = new AccountEditRequest
            {
                Id = userId,
                Email = response.Account.Email,
                FullName = response.Account.FullName,
                Role = response.Account.Role
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(AccountEditRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            request.Id = userId;

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var response = await _accountService.UpdateAsync(userId, request);
            if (!response.Success)
            {
                ModelState.AddModelError("", "Update Failed !!!");
                return View(request);
            }

            TempData["Success"] = "Updated Successfully !!!";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var articlesDto = await _newsArticleService.GetAllAsync();
            var articles = articlesDto
                .Where(a => a.AccountId == userId)
                .Select(a => new NewsArticleResponse
                {
                    Success = true,
                    NewsArticle = a,
                    Error = null
                });
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> ManageTags()
        {
            var tagsDto = await _tagService.GetAllAsync();
            var tags = tagsDto.Select(t => new TagResponse
            {
                Success = true,
                Tag = t,
                Error = null
            });
            return View(tags);
        }

        [HttpGet]
        public IActionResult CreateTag()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag(TagRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var response = await _tagService.CreateAsync(request);
            if (!response.Success)
            {
                ModelState.AddModelError("", "Error !!!");
                return View(request);
            }

            return RedirectToAction("ManageTags");
        }

        [HttpGet]
        public async Task<IActionResult> EditTag(string id)
        {
            var response = await _tagService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }

            var request = new TagRequest
            {
                Name = response.Tag.Name
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> EditTag(string id, TagRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var response = await _tagService.UpdateAsync(id, request);
            if (!response.Success)
            {
                ModelState.AddModelError("", "Error !!!");
                return View(request);
            }

            return RedirectToAction("ManageTags");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTag(string id)
        {
            var newsCountResponse = await _newsArticleService.GetCountByTagAsync(id);
            if (newsCountResponse.Success && newsCountResponse.Data > 0)
            {
                TempData["Error"] = "Tag Is In Use !!!";
                return RedirectToAction("ManageTags");
            }

            var response = await _tagService.DeleteAsync(id);
            if (!response.Success)
            {
                TempData["Error"] = "Error !!!";
            }

            return RedirectToAction("ManageTags");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var response = await _newsArticleService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }

            return View(response);
        }
    }
}