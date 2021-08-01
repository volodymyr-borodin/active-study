using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Components.Header
{
    public class Header : ViewComponent
    {
        private readonly HttpContext _context;
        
        public Header(IHttpContextAccessor contextAccessor)
        {
            _context = contextAccessor.HttpContext;
        }

        public IViewComponentResult Invoke()
        {
            return View(new HeaderModel(_context.User?.Identity?.IsAuthenticated ?? false));
        }
    }
}