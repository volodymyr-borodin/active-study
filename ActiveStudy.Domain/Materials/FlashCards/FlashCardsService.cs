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

    public Task<FlashCardSetDetails> GetByIdAsync(string id, AuthContext authContext = null)
    {
        return flashCardsStorage.GetByIdAsync(id, authContext);
    }

    public Task<IEnumerable<FlashCardSet>> FindAsync(AuthContext authContext = null)
    {
        return flashCardsStorage.FindAsync(authContext);
    }

    public Task CreateAsync(FlashCardSetDetails set)
    {
        return flashCardsStorage.InsertAsync(set);
    }
}