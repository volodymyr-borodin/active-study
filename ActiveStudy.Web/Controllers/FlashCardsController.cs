using System.Collections.Generic;
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
        await learningProgressService.UpdateProgressAsync(currentUserProvider.User.Id, id, model.Answers);

        return RedirectToAction("Details", new {id});
    }
}
