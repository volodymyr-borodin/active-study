using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Classes;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Web.Models.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Areas.Schools.Controllers
{
    [Route("school/{schoolId}/classes"), Authorize]
    public class ClassesController : Controller
    {
        private readonly IClassStorage classStorage;
        private readonly ISchoolStorage schoolStorage;
        private readonly IStudentStorage studentStorage;

        public ClassesController(IClassStorage classStorage,
            ISchoolStorage schoolStorage,
            IStudentStorage studentStorage)
        {
            this.classStorage = classStorage;
            this.studentStorage = studentStorage;
            this.schoolStorage = schoolStorage;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([Required] string schoolId, [Required] string id)
        {
            var @class = await classStorage.GetByIdAsync(id);
            var students = await studentStorage.FindAsync(StudentFilter.ByClass(id));
            var school = await schoolStorage.GetByIdAsync(@class.SchoolId);
            
            var model = new ClassViewModel(@class.Id, school, @class.Title, students.Select(s => new StudentListModel(s.Id, s.FullName)));
            
            return View(model);
        }

        [HttpGet("create")]
        public IActionResult Create(string schoolId)
        {
            return View(new CreateClassViewModel());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required]string schoolId, CreateClassInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new CreateClassViewModel
                {
                    Label = model.Label,
                    Grade = model.Grade
                });
            }

            // TODO: Validate create class access to school
            var @class = new Class(string.Empty, model.Grade, model.Label, schoolId);
            var classId = await classStorage.InsertAsync(@class);

            return RedirectToAction("Details", new { schoolId, id = classId });
        }
    }
}
