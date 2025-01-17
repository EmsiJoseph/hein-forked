using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Hein.Models;
using Hein.Data;
using Hein.Data.Models;
using Hein.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hein.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(
            ILogger<AuthController> logger,
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Helper Methods
        private async Task<User> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user;
        }

        private IActionResult RedirectToHomeOrOnboarding(User user)
        {
            return user.IsOnboarded
                ? RedirectToAction("Index", "Home")
                : RedirectToAction("Onboarding");
        }

        // Action Methods
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                return RedirectToHomeOrOnboarding(user);
            }

            var model = new SignInOrRegisterViewModel
            {
                Email = TempDataHelper.GetTempData(this, "Email")
            };


            return View(model);
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SignInOrRegisterViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Email))
            {
                TempDataHelper.SetTempData(this, "Email", model.Email);
                var userExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
                return RedirectToAction(userExists ? "SignIn" : "Register");
            }

            if (!string.IsNullOrEmpty(model.Phone))
            {
                var fullPhoneNumber = model.CountryCode + model.Phone;
                var userExists = await _context.Users.AnyAsync(u => u.Phone == fullPhoneNumber);

                if (!userExists)
                {
                    TempDataHelper.SetTempData(this, "Phone", model.Phone);
                    return RedirectToAction("Register");
                }

                return RedirectToAction("VerifyPhoneSignIn");
            }

            ModelState.AddModelError("", "Please enter an email or phone number");
            return View(model);
        }

        [HttpGet("Register")]
        public async Task<IActionResult> Register()
        {
            var email = TempDataHelper.GetTempData(this, "Email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                TempDataHelper.SetTempData(this, "ErrorMessageRegisterRedirect",
                    "This email is already registered. Please sign in or use a different email.", false);
                return RedirectToAction("SignIn");
            }

            var fromOAuth = TempDataHelper.GetTempData(this, "FromOAuth") == "true";
            return View(new RegisterViewModel
            {
                Email = email,
                FromOAuth = fromOAuth
            });
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = false,
                CreatedAt = DateTime.UtcNow,
                EmailSubscription = model.EmailSubscription
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                SonnerHelper.Error(TempData, "An unexpected error occurred. Please try again later.");
                return View(model);
            }


            await _signInManager.SignInAsync(user, isPersistent: false);
            SonnerHelper.Success(TempData, $"Welcome to HEIN, {user.Email}! Let's get started with your onboarding.");
            return RedirectToAction("Onboarding");
        }

        [HttpGet("SignIn")]
        public IActionResult SignIn()
        {
            var email = TempDataHelper.GetTempData(this, "Email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index");
            }

            var model = new SignInViewModel { Email = email };

            if (TempData.ContainsKey("ErrorMessageRegisterRedirect"))
            {
                ModelState.AddModelError("Email",
                    TempDataHelper.GetTempData(this, "ErrorMessageRegisterRedirect", false));
            }

            return View(model);
        }

        [HttpPost("SignIn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid email or password");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                user.EmailSubscription = model.EmailSubscription;
                await _userManager.UpdateAsync(user);
                return RedirectToHomeOrOnboarding(user);
            }

            if (result.RequiresTwoFactor)
                return RedirectToAction("LoginWith2fa", new { RememberMe = model.RememberMe });

            if (result.IsLockedOut)
                return RedirectToAction("Lockout");

            ModelState.AddModelError("Email", "Invalid email or password");
            return View(model);
        }

        [HttpGet("Onboarding")]
        public async Task<IActionResult> Onboarding()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return RedirectToAction("Index");
                }

                var model = new OnboardingViewModel
                {
                    FirstName = TempDataHelper.GetTempData(this, "FirstName"),
                    LastName = TempDataHelper.GetTempData(this, "LastName")
                };
                return View(model);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost("Onboarding")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Onboarding(OnboardingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await GetCurrentUserAsync();
                await UpdateUserOnboardingInfo(user, model);
                TempDataHelper.RemoveTempData(this, "FirstName");
                TempDataHelper.RemoveTempData(this, "LastName");
                SonnerHelper.Success(TempData, "Register success! Your account is now set up.");
                return RedirectToAction("Index", "Home");
            }
            catch (InvalidOperationException)
            {
                SonnerHelper.Error(TempData, "An unexpected error occurred. Please try again later.");
                return RedirectToAction("Index");
            }
        }

        // Other existing action methods remain unchanged...
        [HttpGet("VerifyPhoneSignIn")]
        public IActionResult VerifyPhoneSignIn()
        {
            return View();
        }

        [HttpPost("VerifyPhoneSignIn")]
        public IActionResult VerifyPhoneSignIn(string code)
        {
            return View();
        }

        [HttpGet("Lockout")]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet("LoginWith2fa")]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe)
        {
            // Ensure the user has gone through password auth first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException("Unable to load two-factor authentication user.");
            }

            return View(new LoginWith2faViewModel { RememberMe = rememberMe });
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        // Private helper methods for user updates
        private async Task UpdateUserOnboardingInfo(User user, OnboardingViewModel model)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.FullName = $"{model.FirstName} {model.LastName}";
            user.Age = (DateTime.UtcNow.Year - model.Dob.Value.Year).ToString();
            user.Dob = model.Dob;
            user.StylePreference = model.StylePreference;
            user.Latitude = model.Latitude;
            user.Longitude = model.Longitude;
            user.IsOnboarded = true;
            user.Gender = model.Gender;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to update user onboarding information");
            }
        }
    }
}