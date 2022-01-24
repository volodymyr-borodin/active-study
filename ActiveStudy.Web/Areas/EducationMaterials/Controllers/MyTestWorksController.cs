using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.MultiAnswer;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;
using ActiveStudy.Domain.Materials.TestWorks.Results;
using ActiveStudy.Web.Areas.EducationMaterials.Models.MyTestWorks;
using ActiveStudy.Web.Areas.EducationMaterials.Models.TestWorks;
using ActiveStudy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Areas.EducationMaterials.Controllers;

[Area("EducationMaterials"), Route("materials/test-works/my"), Authorize]
public class MyTestWorksController : Controller
{
    private readonly TestWorksService testWorksService;
    private readonly ITestWorkResultsStorage testWorkResultsStorage;
    private readonly ISubjectStorage subjectStorage;
    private readonly CurrentUserProvider currentUserProvider;

    public MyTestWorksController(TestWorksService testWorksService, ISubjectStorage subjectStorage, CurrentUserProvider currentUserProvider, ITestWorkResultsStorage testWorkResultsStorage)
    {
        this.testWorksService = testWorksService;
        this.subjectStorage = subjectStorage;
        this.currentUserProvider = currentUserProvider;
        this.testWorkResultsStorage = testWorkResultsStorage;
    }

    public async Task<IActionResult> Index()
    {
        var items = await testWorksService.FindByAuthorAsync(currentUserProvider.User.Id);

        return View(new TestWorksListViewModel(items, currentUserProvider.IsAuthenticated));
    }

    [Authorize]
    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        var subjects = await subjectStorage.SearchAsync("UA");
        var subjectsList = subjects
            .Select(subject => new SelectListItem(subject.Title, subject.Id))
            .ToList();

        var statusesList = ((TestWorkStatus[]) Enum.GetValues(typeof(TestWorkStatus)))
            .Select(e => new SelectListItem(e.ToString(), ((int)e).ToString()))
            .ToList();

        return View(CreateTestWorkViewModel.Empty(subjectsList, statusesList));
    }

    [Authorize]
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTestWorkViewModel model)
    {
        var subject = await subjectStorage.GetByIdAsync(model.SubjectId);
        var author = currentUserProvider.User.AsUser();

        var variants = new List<TestWorkVariant>();
        foreach (var variant in model.Variants)
        {
            var questions = new List<Question>();
            foreach (var question in variant.Questions)
            {
                if (question.Type == nameof(SingleAnswerQuestion))
                {
                    var options = question.Options
                        .Select(o => new SingleAnswerOption(Guid.NewGuid().ToString(), o.Text))
                        .ToList();

                    questions.Add(new SingleAnswerQuestion(Guid.NewGuid().ToString(),
                        question.Text,
                        string.Empty,
                        1,
                        options,
                        options[question.CorrectOptionIndex].Id));
                }
                else if (question.Type == nameof(MultiAnswerQuestion))
                {
                    var options = question.Options
                        .Select(o => new MultiAnswerOption(Guid.NewGuid().ToString(), o.Text, o.IsCorrect))
                        .ToList();

                    questions.Add(new MultiAnswerQuestion(Guid.NewGuid().ToString(),
                        question.Text,
                        string.Empty,
                        1,
                        options));
                }

            }
            
            variants.Add(new TestWorkVariant(Guid.NewGuid().ToString(), questions));
        }

        var testWork = new TestWorkDetails(model.Title, model.Description, subject, author, variants, model.Status);

        await testWorksService.CreateAsync(testWork);

        return RedirectToAction("Index", "TestWorks");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(string id)
    {
        var testWork = await testWorksService.GetByIdAsync(id);
        var results = await testWorkResultsStorage.FindAsync(testWork.Id);
        return View(new MyTestWorkDetailsViewModel(testWork, results));
    }
}