using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Materials.TestWorks;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Models.TestWorks;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Controllers;

[Route("materials/test-works")]
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
}
