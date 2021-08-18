using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Teachers;
using ActiveStudy.Web.Models.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Areas.Schools.Controllers
{
    [Route("school/{schoolId}/teachers"), Authorize]
    public class TeachersController : Controller
    {
        private readonly ITeacherStorage teacherStorage;
        private readonly ISchoolStorage schoolStorage;
        private readonly ISubjectStorage subjectStorage;
    
        public TeachersController(ITeacherStorage teacherStorage, ISchoolStorage schoolStorage, ISubjectStorage subjectStorage)
        {
            this.teacherStorage = teacherStorage;
            this.schoolStorage = schoolStorage;
            this.subjectStorage = subjectStorage;
        }
    
        [HttpGet]
        public async Task<IActionResult> List([Required]string schoolId)
        {
            var school = await schoolStorage.GetByIdAsync(schoolId);
            var teachers = await teacherStorage.FindAsync(schoolId);

            return View(new TeachersListPageModel(school, teachers));
        }
    
        [HttpGet("create")]
        public async Task<IActionResult> Create([Required]string schoolId)
        {
            return View(await Build(schoolId));
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required]string schoolId, CreateTeacherInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(await Build(schoolId));
            }

            // TODO: Validate create class access to school
            var subjects = await subjectStorage.SearchAsync(model.SubjectIds);
            
            var teacher = new Teacher(string.Empty, model.FirstName, model.LastName, model.MiddleName, model.Email, subjects, schoolId, string.Empty);
            var teacherId = await teacherStorage.InsertAsync(teacher);
    
            return RedirectToAction("List", "Teachers", new { schoolId });
        }

        [HttpPost("{id}/delete")]
        public async Task<IActionResult> Delete([Required] string schoolId, [Required] string id)
        {
            var teacher = await teacherStorage.GetByIdAsync(id);

            // TODO: Add validation. Teacher can be assigned to class
            await teacherStorage.DeleteAsync(teacher);

            return RedirectToAction("List", "Teachers", new {schoolId});
        }

        private async Task<CreateTeacherViewModel> Build(string schoolId, CreateTeacherInputModel input = null)
        {
            var school = await schoolStorage.GetByIdAsync(schoolId);
            var subjects = await subjectStorage.SearchAsync(school.Country.Code);

            return new CreateTeacherViewModel(school, subjects)
            {
                FirstName = input?.FirstName,
                LastName = input?.LastName,
                Email = input?.Email,
                SubjectIds = input?.SubjectIds
            };
        }
    }
}
