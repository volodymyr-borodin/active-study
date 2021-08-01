using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Web.Models.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Controllers
{
    [Route("school/{schoolId}/students"), Authorize]
    public class StudentsController : Controller
    {
        private readonly IStudentStorage studentStorage;
        private readonly IClassStorage classStorage;

        public StudentsController(IStudentStorage studentStorage, IClassStorage classStorage)
        {
            this.studentStorage = studentStorage;
            this.classStorage = classStorage;
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create(string classId = null)
        {
            return View(await Build(classId));
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required]string schoolId, CreateStudentInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(await Build(model?.ClassId, model));
            }
                
            // TODO: Validate create class access to school
            ClassShortInfo classInfo = null;
            if (!string.IsNullOrEmpty(model.ClassId))
            {
                var @class = await classStorage.GetByIdAsync(model.ClassId);
                classInfo = new ClassShortInfo(@class.Id, @class.Title);
            }
            
            var student = new Student(string.Empty, model.FirstName, model.LastName, model.Email, schoolId, classInfo == null ? Enumerable.Empty<ClassShortInfo>() : new [] { classInfo });
            await studentStorage.InsertAsync(student);

            return RedirectToAction("Details", "Classes", new { schoolId, id = model.ClassId });
        }
    
        private async Task<CreateStudentViewModel> Build(string classId = null, CreateStudentInputModel input = null)
        {
            ClassShortInfo classInfo = null;
            if (!string.IsNullOrEmpty(classId))
            {
                var @class = await classStorage.GetByIdAsync(classId);
                classInfo = new ClassShortInfo(@class.Id, @class.Title);
            }

            return new CreateStudentViewModel(classInfo)
            {
                FirstName = input?.FirstName,
                LastName = input?.LastName,
                Email = input?.Email
            };
        }
    }
}