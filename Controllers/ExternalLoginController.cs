using System.Security.Claims;
using Hein.Data;
using Hein.Data.Models;
using Hein.Helpers;
using Hein.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hein.Controllers;

[Route("[controller]")]
public class ExternalLoginController : Controller
{
    private readonly ILogger<ExternalLoginController> _logger;

    private readonly ApplicationDbContext _context;


    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public ExternalLoginController(
        ILogger<ExternalLoginController> logger,
        ApplicationDbContext context,
        SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    /// <summary>
    /// Handles the redirect to the external OAuth provider
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpGet("Login")]
    public IActionResult Login(string provider, string returnUrl = null)
    {
        var redirectUrl = Url.Action("LoginCallback", "ExternalLogin", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }


    /// <summary>
    /// Handles the callback from external OAuth providers
    /// </summary>
    [HttpGet("LoginCallback")]
    public async Task<IActionResult> LoginCallback(string returnUrl = null, string remoteError = null)
    {
        if (!string.IsNullOrEmpty(remoteError))
        {
            SonnerHelper.Error(TempData, $"Error from external provider: {remoteError}");
            return RedirectToAction("Index", "Auth");
        }

        var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo == null)
        {
            return RedirectToAction("Index", "Auth");
        }

        var userClaims = GetUserClaimsFromExternalLogin(externalLoginInfo);

        TempDataHelper.SetTempData(this, "Email", userClaims.Email);
        TempDataHelper.SetTempData(this, "FirstName", userClaims.FirstName);
        TempDataHelper.SetTempData(this, "LastName", userClaims.LastName);

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userClaims.Email);

        if (existingUser == null)
        {
            // Create new user
            var newUser = new User
            {
                Email = userClaims.Email,
                UserName = userClaims.Email,
                FirstName = userClaims.FirstName,
                LastName = userClaims.LastName,
                IsOnboarded = false,
                EmailConfirmed = true // Set email as confirmed since it's from external provider
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
            {
                SonnerHelper.Error(TempData, "Failed to create user account.");
                return RedirectToAction("Index", "Auth");
            }

            // Add the external login to the new user
            var addLoginResult = await _userManager.AddLoginAsync(newUser, externalLoginInfo);
            if (!addLoginResult.Succeeded)
            {
                SonnerHelper.Error(TempData, "Failed to add external login.");
                return RedirectToAction("Index", "Auth");
            }

            // Sign in the new user
            await _signInManager.SignInAsync(newUser, isPersistent: false);
            return RedirectToAction("Onboarding", "Auth");
        }

        var userOAuthProvider = await _context.UserLogins
            .FirstOrDefaultAsync(u =>
                u.UserId == existingUser.Id && u.LoginProvider == externalLoginInfo.LoginProvider);

        if (userOAuthProvider == null)
        {
            // Add new login provider to existing user
            await _userManager.AddLoginAsync(existingUser, externalLoginInfo);
        }

        return await SignInExistingUser(externalLoginInfo, returnUrl, existingUser.IsOnboarded);
    }

    private UserClaimsModel GetUserClaimsFromExternalLogin(ExternalLoginInfo loginInfo)
    {
        return new UserClaimsModel
        {
            Email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email),
            FirstName = loginInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
            LastName = loginInfo.Principal.FindFirstValue(ClaimTypes.Surname)
        };
    }

    private async Task<IActionResult> SignInExistingUser(ExternalLoginInfo loginInfo, string returnUrl,
        bool isOnboarded)
    {
        var result = await _signInManager.ExternalLoginSignInAsync(
            loginInfo.LoginProvider,
            loginInfo.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true
        );

        if (result.IsLockedOut)
        {
            return RedirectToAction("Lockout", "Auth");
        }

        if (!result.Succeeded)
        {
            SonnerHelper.Error(TempData, "Invalid login attempt. Please try again or register a new account.");
            return RedirectToAction("Index", "Auth");
        }

        if (!isOnboarded)
        {
            return RedirectToAction("Onboarding", "Auth");
        }

        var updatedUser = new User
        {
            EmailConfirmed = true
        };

        await _userManager.UpdateAsync(updatedUser);
        return LocalRedirect(returnUrl ?? "/");
    }

    private class UserClaimsModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}