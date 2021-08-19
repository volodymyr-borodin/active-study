using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Components.Breadcrumb
{
    public class Breadcrumb : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<BreadcrumbItem> items)
        {
            return View(items);
        }
    }
}