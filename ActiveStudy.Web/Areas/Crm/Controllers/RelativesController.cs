using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Crm.Relatives;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Web.Areas.Crm.Models.Relatives;
using ActiveStudy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Areas.Crm.Controllers;

[Authorize]
[Area("Crm"), Route("school/{schoolId}/relatives")]
public class RelativesController : Controller
{
    private readonly ISchoolStorage schoolStorage;
    private readonly IRelativesStorage relativesStorage;
    private readonly IStudentStorage studentStorage;
    private readonly IAuditStorage auditStorage;
    private readonly CurrentUserProvider currentUserProvider;

    public RelativesController(ISchoolStorage schoolStorage,
        IRelativesStorage relativesStorage,
        IStudentStorage studentStorage,
        IAuditStorage auditStorage,
        CurrentUserProvider currentUserProvider)
    {
        this.schoolStorage = schoolStorage;
        this.relativesStorage = relativesStorage;
        this.studentStorage = studentStorage;
        this.auditStorage = auditStorage;
        this.currentUserProvider = currentUserProvider;
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

        var school = await schoolStorage.GetByIdAsync(schoolId);
        var student = await studentStorage.GetByIdAsync(model.StudentId);

        var relative = new Relative(string.Empty, model.FirstName, model.LastName, model.Email, model.Phone);
        var relativeId = await relativesStorage.InsertAsync(relative);

        await auditStorage.LogRelativeCreatedAsync(
            school.Id, school.Title,
            relativeId, relative.FullName,
            currentUserProvider.User.AsUser());
            
        await relativesStorage.AddStudentAsync(relativeId, model.StudentId);

        await auditStorage.LogRelativeAddedToStudentAsync(
            school.Id, school.Title,
            student.Id, student.FullName,
            relativeId, relative.FullName,
            currentUserProvider.User.AsUser());

        return RedirectToAction("Details", "School", new { id = schoolId });
    }
    
    private async Task<CreateRelativeViewModel> Build(string schoolId, string studentId, CreateRelativeInputModel input = null)
    {
        var student = string.IsNullOrEmpty(studentId)
            ? default
            : await studentStorage.GetByIdAsync(studentId);

        return new CreateRelativeViewModel(schoolId,
            student,
            input?.FirstName,
            input?.LastName,
            input?.Email,
            input?.Phone,
            studentId);
    }
}