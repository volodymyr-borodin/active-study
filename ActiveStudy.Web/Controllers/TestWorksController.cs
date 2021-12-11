using System;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Domain.Materials.TestWorks.Questions.MultiAnswer;
using ActiveStudy.Domain.Materials.TestWorks.Questions.SingleAnswer;
using ActiveStudy.Domain.Materials.TestWorks.Results;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Models.TestWorks;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Controllers;

[Route("materials/test-works")]
public class TestWorksController : Controller
{
    private readonly TestWorksService testWorksService;
    private readonly ISubjectStorage subjectStorage;
    private readonly ITestWorkResultsStorage testWorkResultsStorage;
    private readonly CurrentUserProvider currentUserProvider;

    public TestWorksController(TestWorksService testWorksService,
        ISubjectStorage subjectStorage,
        ITestWorkResultsStorage testWorkResultsStorage,
        CurrentUserProvider currentUserProvider)
    {
        this.testWorksService = testWorksService;
        this.subjectStorage = subjectStorage;
        this.testWorkResultsStorage = testWorkResultsStorage;
        this.currentUserProvider = currentUserProvider;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var items = await testWorksService.FindPublishedAsync(null);

        return View(new TestWorksListViewModel(items, currentUserProvider.IsAuthenticated));
    }
    
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> Category(string categoryId)
    {
        var items = await testWorksService.FindPublishedAsync(categoryId);
        var subject = await subjectStorage.GetByIdAsync(categoryId);

        return View(new TestWorksCategoryViewModel(items, subject, currentUserProvider.IsAuthenticated));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(string id)
    {
        var item = await testWorksService.GetByIdAsync(id);
        return View(item);
    }

    [HttpGet("{id}/submit")]
    public async Task<IActionResult> Submit(string id)
    {
        var item = await testWorksService.GetByIdAsync(id);
        return View(new TestFormViewModel(item, item.Variants.First().Id));
    }

    [HttpPost("{id}/submit")]
    public async Task<IActionResult> Submit(SubmitTestViewModel model)
    {
        var item = await testWorksService.GetByIdAsync(model.TestWorkId);

        var answersMap = model.Answers.ToDictionary(a => a.QuestionId);

        var answers = item.Variants.First(v => v.Id == model.VariantId).Questions.Select(question => question switch
        {
            SingleAnswerQuestion singleAnswerQuestion => new TestWorkSingleQuestionResult(singleAnswerQuestion, answersMap[question.Id].OptionId),
            MultiAnswerQuestion multiAnswerQuestion => new TestWorkMultiQuestionResult(multiAnswerQuestion, answersMap[question.Id].SelectedOptionIds),
            // QuestionType.Text => new TestWorkTextQuestionResult(tQuestions[a.QuestionId], a.Text),
            // QuestionType.Number => new TestWorkNumberQuestionResult(nQuestions[a.QuestionId], a.Number),
            // QuestionType.SequenceRecovery => new TestWorkSequenceRecoveryResult(srQuestions[a.QuestionId], a.SequencePairs.Select(p => new SequenceRecoveryResultPair(p.QuestionId, p.AnswerId))),
            _ => default(TestWorkQuestionResult)
        });

        var author = new TestWorkResultAuthor(model.Author.FirstName ?? string.Empty, model.Author.LastName ?? string.Empty, model.Author.Email);
        var result = new TestWorkResult(model.TestWorkId, model.VariantId, answers, author, DateTimeOffset.UtcNow);

        await testWorkResultsStorage.InsertAsync(result);

        return View(new TestFormViewModel(item, item.Variants.First().Id));
    }
}
