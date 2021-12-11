using System.Threading.Tasks;
using ActiveStudy.Storage.Mongo.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Components.Header
{
    public class Header : ViewComponent
    {
        private readonly UserManager<ActiveStudyUserEntity> userManager;
        private readonly HttpContext context;
        
        public Header(UserManager<ActiveStudyUserEntity> userManager,
            IHttpContextAccessor contextAccessor)
        {
            this.userManager = userManager;
            context = contextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var isAuthenticated = context.User?.Identity?.IsAuthenticated ?? false;
            var account = default(AccountModel);

            if (isAuthenticated)
            {
                var user = await userManager.GetUserAsync(context.User);
                account = new AccountModel($"{user.FirstName} {user.LastName}", user.UserName);
            }

            return View(new HeaderModel(isAuthenticated, account));
        }
    }
}