using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Materials.FlashCards;

public class FlashCardsService
{
    private readonly IFlashCardsStorage flashCardsStorage;

    public FlashCardsService(IFlashCardsStorage flashCardsStorage)
    {
        this.flashCardsStorage = flashCardsStorage;
    }

    public Task<FlashCardSetDetails> GetByIdAsync(string id)
    {
        return flashCardsStorage.GetByIdAsync(id);
    }

    public Task<IEnumerable<FlashCardSet>> FindAsync()
    {
        return flashCardsStorage.FindAsync();
    }

    public Task CreateAsync(FlashCardSetDetails set)
    {
        return flashCardsStorage.InsertAsync(set);
    }
}