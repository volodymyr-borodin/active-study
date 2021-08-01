using System.Threading.Tasks;
using ActiveStudy.Storage.Mongo.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Components.Account
{
    public class Account : ViewComponent
    {   
        private readonly UserManager<ActiveStudyUserEntity> _userManager;
        private readonly HttpContext _context;

        public Account(UserManager<ActiveStudyUserEntity> userManager, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _context = contextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(_context.User);
            
            return View(new AccountModel("/img/100x100/img1.jpg", $"{user.FirstName} {user.LastName}", user.UserName));
        }
    }
}