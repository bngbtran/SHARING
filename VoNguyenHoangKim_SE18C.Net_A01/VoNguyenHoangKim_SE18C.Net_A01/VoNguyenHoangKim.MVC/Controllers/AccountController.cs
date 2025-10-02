using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(VoNguyenHoangKim.Service.ValidateSend.LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var response = await _accountService.GetByEmailAsync(request.Email);
            if (!response.Success || response.Account.Password != request.Password)
            {
                ModelState.AddModelError("", "Error Email or Password. Please try again !!!");
                return View(request);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, response.Account.Id),
                new Claim(ClaimTypes.Email, response.Account.Email),
                new Claim(ClaimTypes.Role, ((Role)response.Account.Role).ToString()),
                new Claim(ClaimTypes.Name, response.Account.FullName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAsync();
            return View(accounts);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _accountService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }

            var request = new AccountRequest
            {
                Email = response.Account.Email,
                FullName = response.Account.FullName,
                Role = response.Account.Role
            };
            return View(request);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _accountService.DeleteAsync(id);
            if (!response.Success)
            {
                TempData["Error"] = "Delete Failed !!!";
            }
            else
            {
                TempData["Success"] = "Delete Successfully !!!";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}