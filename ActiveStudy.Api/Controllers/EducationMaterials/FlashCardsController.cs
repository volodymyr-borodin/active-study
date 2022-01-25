using System.Security.Claims;
using ActiveStudy.Domain.Materials.FlashCards;
using ActiveStudy.Domain.Materials.FlashCards.Progress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Api.Controllers.EducationMaterials;

[ApiController]
[Route("education-materials/flash-cards")]
public class FlashCardsController : ControllerBase
{
    private readonly FlashCardsService flashCardsService;
    private readonly LearningProgressService learningProgressService;

    public FlashCardsController(
        FlashCardsService flashCardsService,
        LearningProgressService learningProgressService)
    {
        this.flashCardsService = flashCardsService;
        this.learningProgressService = learningProgressService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var items = await flashCardsService.FindAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(string id)
    {
        var item = await flashCardsService.GetByIdAsync(id);

        return Ok(item);
    }

    [Authorize(Policy = "Education/FlashCards")]
    [HttpGet("{id}/learn")]
    public async Task<IActionResult> Learn(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var item = await learningProgressService.GetNextRoundAsync(userId, id);
        if (item == null)
        {
            return NotFound();
        }

        return Ok(new LearningRound(item.Items));
    }

    [Authorize(Policy = "Education/FlashCards")]
    [HttpPost("{id}/learn")]
    public async Task<IActionResult> Learn(
        [FromRoute] string id,
        [FromBody] LearnInput model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await learningProgressService.UpdateProgressAsync(userId, id, model.Answers);

        return Ok(new
        {
            Results = result,
            IsFinished = await learningProgressService.IsFinishedAsync(userId, id)
        });
    }

    public record LearningRound(IEnumerable<LearningRoundItem> CardsToLearn);
    public record LearnInput(IEnumerable<NewAnswer> Answers);
}