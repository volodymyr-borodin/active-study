using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Storage.Mongo.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Components.SchoolsNav
{
    public class SchoolsNav : ViewComponent
    {
        private readonly ISchoolStorage _schoolStorage;
        private readonly UserManager<ActiveStudyUserEntity> _userManager;
        private readonly HttpContext _context;

        public SchoolsNav(ISchoolStorage schoolStorage,
            IHttpContextAccessor contextAccessor,
            UserManager<ActiveStudyUserEntity> userManager)
        {
            _schoolStorage = schoolStorage;
            _userManager = userManager;
            _context = contextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(_context.User);
            

            if (user == null)
            {
                return View(new SchoolsNavModel(Enumerable.Empty<UserSchoolModel>()));
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var schoolIds = claims
                .Where(c => c.Type == CustomClaimTypes.School)
                .Select(c => c.Value)
                .ToList();

            var schools = await _schoolStorage.SearchByIdsAsync(schoolIds);
            
            // TODO: show only 3 test work in nav
            var model = new SchoolsNavModel(schools.Select(s => new UserSchoolModel(s.Id, s.Title)).ToList());
            return View(model);
        }
    }
}