using System.Threading.Tasks;
using ActiveStudy.Domain.Materials.FlashCards;
using ActiveStudy.Web.Models.FlashCards;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Controllers;

[Route("materials/flash-cards")]
public class FlashCardsController : Controller
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

        return View(new FlashCardsViewModel(items));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(string id)
    {
        var item = await flashCardsService.GetByIdAsync(id);

        return View(item);
    }
}
