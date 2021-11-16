using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Models.TestWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Controllers;

[Route("materials/test-works"), Authorize]
public class TestWorksController : Controller
{
    private readonly TestWorksService testWorksService;
    private readonly ISubjectStorage subjectStorage;
    private readonly CurrentUserProvider currentUserProvider;

    public TestWorksController(TestWorksService testWorksService,
        ISubjectStorage subjectStorage,
        CurrentUserProvider currentUserProvider)
    {
        this.testWorksService = testWorksService;
        this.subjectStorage = subjectStorage;
        this.currentUserProvider = currentUserProvider;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var items = await testWorksService.FindPublishedAsync();
        return View(new TestWorksListViewModel(items));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(string id)
    {
        var item = await testWorksService.GetByIdAsync(id);
        return View(item);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        var subjects = await subjectStorage.SearchAsync("UA");
        var subjectsSelect = subjects
            .Select(subject => new SelectListItem(subject.Title, subject.Id))
            .ToList();

        return View(CreateTestWorkViewModel.Empty(subjectsSelect));
    }

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
                var options = question.Options
                    .Select(o => new SingleAnswerOption(Guid.NewGuid().ToString(), o.Text))
                    .ToList();

                questions.Add(new SingleAnswerQuestion(Guid.NewGuid().ToString(), question.Text, string.Empty, 1, options, options[question.CorrectOptionIndex].Id));
            }
            
            variants.Add(new TestWorkVariant(Guid.NewGuid().ToString(), questions));
        }

        var testWork = new TestWorkDetails(model.Title, model.Description, subject, author, variants, TestWorkStatus.Published);

        await testWorksService.CreateAsync(testWork);

        return View(model);
    }
}