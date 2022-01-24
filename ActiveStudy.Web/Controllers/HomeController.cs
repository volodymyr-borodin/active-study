using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Models.Home;
using Microsoft.AspNetCore.Mvc;
using static ActiveStudy.Web.Consts;

namespace ActiveStudy.Web.Controllers;

public class HomeController : Controller
{
    private readonly CurrentUserProvider currentUserProvider;

    public HomeController(CurrentUserProvider currentUserProvider)
    {
        this.currentUserProvider = currentUserProvider;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!currentUserProvider.IsAuthenticated)
        {
            return View(new HomePageModel());
        }

        var userSchoolIds = await currentUserProvider.GetAssignedSchoolAsync();
        return userSchoolIds.Count() == 1
            ? RedirectToAction("Details", "School", new { area = Area.Crm, Id = userSchoolIds.First()})
            : RedirectToAction("List", "School", new { area = Area.Crm });
    }
}
