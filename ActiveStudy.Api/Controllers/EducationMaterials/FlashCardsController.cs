using ActiveStudy.Domain.Materials.FlashCards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Api.Controllers.EducationMaterials;

[ApiController]
[Route("education-materials/flash-cards")]
public class FlashCardsController : ControllerBase
{
    private readonly FlashCardsService flashCardsService;

    public FlashCardsController(FlashCardsService flashCardsService)
    {
        this.flashCardsService = flashCardsService;
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
}