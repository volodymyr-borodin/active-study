using System;
using System.Threading.Tasks;
using ActiveStudy.Storage.Mongo.Identity;
using ActiveStudy.Web.Models.Account;
using ActiveStudy.Web.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ActiveStudyUserEntity> signInManager;
        private readonly UserManager<ActiveStudyUserEntity> userManager;
        private readonly IEmailService emailService;

        public AccountController(SignInManager<ActiveStudyUserEntity> signInManager,
            UserManager<ActiveStudyUserEntity> userManager,
            IEmailService emailService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailService = emailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await userManager.FindByIdAsync(userId);
            // TODO: user not found

            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                // email confirmed
                return View();
            }

            // return page with errors
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
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            
            // TODO: Validation error

            return View(new LoginInputModel
            {
                Username = model.Username,
                RememberLogin = model.RememberLogin
            });
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
                
                return RedirectToAction("PostRegistration");
            }

            // TODO: return error on !Succeeded
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
        public IActionResult PostRegistration()
        {
            return View();
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
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = Url.Action(
                "ConfirmEmail", "Account",
                new { userId = user.Id, code }, 
                Request.Scheme);

            var subject = "Confirm your email";
            var body = $"Please confirm your account by clicking this link: <a href=\"{url}\">link</a>";

            await emailService.SendEmailAsync(new EmailRecipient($"{user.FirstName} {user.LastName}", user.Email), subject, body);
        }

        private async Task SendPasswordRecoveryEmail(ActiveStudyUserEntity user)
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action(
                "PasswordRecovery", "Account", 
                new { userId = user.Id, code }, 
                Request.Scheme);

            var subject = "Recovery your password";
            var body = $"Follow the link below to the account recovery form: <a href=\"{url}\">link</a>";
            
            await emailService.SendEmailAsync(new EmailRecipient($"{user.FirstName} {user.LastName}", user.Email), subject, body);
        }
    }
}