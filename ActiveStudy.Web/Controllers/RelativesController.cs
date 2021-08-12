using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Relatives;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Web.Models.Relatives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Controllers
{
    [Route("school/{schoolId}/relatives"), Authorize]
    public class RelativesController : Controller
    {
        private readonly IRelativesStorage relativesStorage;
        private readonly IStudentStorage studentStorage;

        public RelativesController(IRelativesStorage relativesStorage, IStudentStorage studentStorage)
        {
            this.relativesStorage = relativesStorage;
            this.studentStorage = studentStorage;
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create([Required]string schoolId, string studentId)
        {
            return View(await Build(schoolId, studentId));
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required]string schoolId, CreateRelativeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(await Build(schoolId, model.StudentId, model));
            }

            var relative = new Relative(string.Empty, model.FirstName, model.LastName, model.Email, model.Phone);
            var relativeId = await relativesStorage.InsertAsync(relative);
            await relativesStorage.AddStudentAsync(relativeId, model.StudentId);

            return RedirectToAction("Details", "School", new { id = schoolId });
        }
    
        private async Task<CreateRelativeViewModel> Build(string schoolId, string studentId, CreateRelativeInputModel input = null)
        {
            var student = string.IsNullOrEmpty(studentId)
                ? default
                : await studentStorage.GetByIdAsync(studentId);

            return new CreateRelativeViewModel(schoolId, student)
            {
                FirstName = input?.FirstName,
                LastName = input?.LastName,
                Email = input?.Email
            };
        }
    }
}