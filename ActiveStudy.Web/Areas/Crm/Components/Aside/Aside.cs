using System.Threading.Tasks;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Areas.Crm.Components.Aside;

public class Aside : ViewComponent
{
    private readonly ISchoolStorage schoolStorage;
    private readonly CurrentUserProvider currentUserProvider;

    public Aside(ISchoolStorage schoolStorage,
        CurrentUserProvider currentUserProvider)
    {
        this.schoolStorage = schoolStorage;
        this.currentUserProvider = currentUserProvider;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var schoolIds = await currentUserProvider.GetAssignedSchoolAsync();
        var schools = await schoolStorage.SearchByIdsAsync(schoolIds);

        return View(schools);
    }
}