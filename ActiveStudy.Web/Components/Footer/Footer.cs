using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Components.Footer
{
    public class Footer : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}