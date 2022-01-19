using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Materials.FlashCards;
using ActiveStudy.Domain.Materials.FlashCards.Progress;
using ActiveStudy.Web.Models;
using ActiveStudy.Web.Models.FlashCards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Controllers;

[Route("materials/flash-cards")]
public class FlashCardsController : Controller
{
    private readonly FlashCardsService flashCardsService;
    private readonly LearningProgressService learningProgressService;
    private readonly CurrentUserProvider currentUserProvider;

    public FlashCardsController(FlashCardsService flashCardsService,
        LearningProgressService learningProgressService,
        CurrentUserProvider currentUserProvider)
    {
        this.flashCardsService = flashCardsService;
        this.learningProgressService = learningProgressService;
        this.currentUserProvider = currentUserProvider;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var items = await flashCardsService.FindAsync();

        return View(new FlashCardsViewModel(items));
    }

    [Authorize]
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> Create(FlashCardSetCreateInputModel input)
    {
        var cards = input.Cards.Select(c => new FlashCard(string.Empty, c.Term, c.Definition, Enumerable.Empty<Clue>()));
        await flashCardsService.CreateAsync(new FlashCardSetDetails(string.Empty, input.Title, cards));

        return RedirectToAction("Index");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(string id)
    {
        var item = await flashCardsService.GetByIdAsync(id);
        var progress = Enumerable.Empty<CardLearningProgress>();
        if (currentUserProvider.IsAuthenticated)
        {
            progress = (await learningProgressService.GetProgressAsync(currentUserProvider.User.Id, item)).CardsProgress;
        }

        return View(new FlashCardsDetailsViewModel(item, progress));
    }

    [Authorize]
    [HttpGet("{id}/learn")]
    public async Task<IActionResult> Learn(string id)
    {
        var item = await learningProgressService.GetNextRoundAsync(currentUserProvider.User.Id, id);

        return View(item);
    }
    
    [Authorize]
    [HttpPost("{id}/learn")]
    public async Task<IActionResult> Learn(string id, LearnInput model)
    {
        var result = await learningProgressService.UpdateProgressAsync(currentUserProvider.User.Id, id, model.Answers);

        var item = await flashCardsService.GetByIdAsync(id);
        var finished = await learningProgressService.IsFinishedAsync(currentUserProvider.User.Id, id);
        return View("LearnResult", new LearningRoundResult(item, result, finished));
    }

    [Authorize]
    [HttpPost("{id}/reset")]
    public async Task<IActionResult> Reset(string id)
    {
        await learningProgressService.ResetProgressAsync(currentUserProvider.User.Id, id);

        return RedirectToAction("Details", new {id});
    }
}
