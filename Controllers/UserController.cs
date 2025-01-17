using Hein.Data;
using Hein.Data.Models;
using Hein.Helpers;
using Hein.Models.PersonalCenter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hein.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(
            ILogger<UserController> logger,
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string view = "Index")
        {
            // Validate view name to prevent security issues
            var allowedViews = new[] { "Index", "Profile", "ManageAccount" };
            view = allowedViews.Contains(view) ? view : "Index";

            ViewBag.ActiveView = view;

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Auth");

            // Load appropriate model based on view
            switch (view)
            {
                case "Profile":
                    var profileModel = new PersonalCenterProfileViewModel
                    {
                        FirstName = user.FirstName ?? "",
                        LastName = user.LastName ?? "",
                        Email = user.Email ?? "",
                        Phone = user.Phone ?? "",
                        Dob = user.Dob,
                        Gender = user.Gender ?? "",
                        StylePreference = user.StylePreference?.Split(',').ToList() ?? new List<string>(),
                        ShoppingPreference = user.ShoppingPreference?.Split(',').ToList() ?? new List<string>(),
                        FashionStylePreference = user.FashionStylePreference?.Split(',').ToList() ?? new List<string>(),
                        IsEmailConfirmed = user.EmailConfirmed
                    };
                    ViewBag.Model = profileModel;
                    break;

                case "ManageAccount":
                    var manageModel = new ManageAccountViewModel
                    {
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        TwoFactorEnabled = user.TwoFactorEnabled,
                        Phone = user.PhoneNumber
                    };
                    ViewBag.Model = manageModel;
                    break;
            }

            return View();
        }

        [HttpGet("GetPartialView")]
        public async Task<IActionResult> GetPartialView(string view)
        {
            var allowedViews = new[] { "Index", "Profile", "ManageAccount" };
            view = allowedViews.Contains(view) ? view : "Index";

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Auth");

            switch (view)
            {
                case "Profile":
                    var profileModel = new PersonalCenterProfileViewModel
                    {
                        FirstName = user.FirstName ?? "",
                        LastName = user.LastName ?? "",
                        Email = user.Email ?? "",
                        Phone = user.Phone ?? "",
                        Dob = user.Dob,
                        Gender = user.Gender ?? "",
                        StylePreference = user.StylePreference?.Split(',').ToList() ?? new List<string>(),
                        ShoppingPreference = user.ShoppingPreference?.Split(',').ToList() ?? new List<string>(),
                        FashionStylePreference = user.FashionStylePreference?.Split(',').ToList() ?? new List<string>(),
                        IsEmailConfirmed = user.EmailConfirmed
                    };
                    return PartialView($"Partials/_PersonalCenter{view}Partial", profileModel);

                case "ManageAccount":
                    var manageModel = new ManageAccountViewModel
                    {
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        TwoFactorEnabled = user.TwoFactorEnabled,
                        Phone = user.PhoneNumber
                    };
                    return PartialView($"Partials/_PersonalCenter{view}Partial", manageModel);

                default:
                    return PartialView($"Partials/_PersonalCenter{view}Partial");
            }
        }

        [HttpPost("UpdatePreference")]
        public async Task<IActionResult> UpdatePreference(PersonalCenterProfileViewModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return RedirectToAction("Index", "Auth");

                user.StylePreference = model.StylePreference != null ? string.Join(",", model.StylePreference) : null;
                user.ShoppingPreference = model.ShoppingPreference != null ? string.Join(",", model.ShoppingPreference) : null;
                user.FashionStylePreference = model.FashionStylePreference != null ? string.Join(",", model.FashionStylePreference) : null;

                var result = await _userManager.UpdateAsync(user);
                
                if (result.Succeeded)
                {
                    SonnerHelper.Success(TempData, "Your preferences have been updated successfully!");
                    return RedirectToAction("Index", new { view = "Profile" });
                }
                
                SonnerHelper.Error(TempData, "Failed to update preferences. Please try again.");
                return RedirectToAction("Index", new { view = "Profile" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user preferences");
                SonnerHelper.Error(TempData, "An unexpected error occurred. Please try again later.");
                return RedirectToAction("Index", new { view = "Profile" });
            }
        }
    }
}