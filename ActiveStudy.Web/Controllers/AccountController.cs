using System;
using System.Threading.Tasks;
using ActiveStudy.Storage.Mongo.Identity;
using ActiveStudy.Web.Models.Account;
using ActiveStudy.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly SignInManager<ActiveStudyUserEntity> signInManager;
    private readonly UserManager<ActiveStudyUserEntity> userManager;
    private readonly NotificationManager notificationManager;

    public AccountController(SignInManager<ActiveStudyUserEntity> signInManager,
        UserManager<ActiveStudyUserEntity> userManager,
        NotificationManager notificationManager)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.notificationManager = notificationManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found");
            return View();
        }

        var result = await userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            // email confirmed
            return View();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> CompleteInvitation(string userId, string code)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found");
            return View();
        }

        var result = await userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            // email confirmed
            return View(new CompleteInvitationInputModel
            {
                UserId = userId
            });
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CompleteInvitation(CompleteInvitationInputModel model)
    {
        var user = await userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found");
            return View();
        }

        var result = await userManager.AddPasswordAsync(user, model.Password);
        if (result.Succeeded)
        {
            var signInResult = await signInManager.PasswordSignInAsync(user.UserName, model.Password, false, lockoutOnFailure: true);
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
            
        var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid login or password");

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        if (User?.Identity.IsAuthenticated == true)
        {
            await signInManager.SignOutAsync();
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registration(RegistrationInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ActiveStudyUserEntity
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
            
        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await SendConfirmationEmail(user);

            return View("PostRegistration");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await userManager.FindByEmailAsync(model.Email);
        await SendPasswordRecoveryEmail(user);

        return RedirectToAction("PostForgotPassword");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult PasswordRecovery(string userId, string code)
    {
        return View(new PasswordRecoveryInputModel
        {
            UserId = userId,
            Code = code
        });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> PasswordRecovery(PasswordRecoveryInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await userManager.FindByIdAsync(model.UserId);
        var result = await userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);
        if (result.Succeeded)
        {
            return RedirectToAction("PostPasswordRecovery");
        }

        // TODO: return error on !Succeeded
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult PostPasswordRecovery()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult PostForgotPassword()
    {
        return View();
    }

    private async Task SendConfirmationEmail(ActiveStudyUserEntity user)
    {
        var url = Url.Action(
            "ConfirmEmail",
            "Account",
            new
            {
                userId = user.Id,
                code = await userManager.GenerateEmailConfirmationTokenAsync(user)
            }, 
            Request.Scheme);

        await notificationManager.SendEmailConfirmationAsync(new EmailConfirmationTemplateInfo(user, url));
    }

    private async Task SendPasswordRecoveryEmail(ActiveStudyUserEntity user)
    {
        var url = Url.Action(
            "PasswordRecovery",
            "Account", 
            new
            {
                userId = user.Id, code = await userManager.GeneratePasswordResetTokenAsync(user)
            }, 
            Request.Scheme);

        await notificationManager.SendPasswordRecoveryAsync(new PasswordRecoveryTemplateInfo(user, url));
    }
}