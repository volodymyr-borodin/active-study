using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Materials.FlashCards;

public interface IFlashCardsStorage
{
    Task<FlashCardSetDetails> GetByIdAsync(string id);
    Task<IEnumerable<FlashCardSet>> FindAsync();
    Task InsertAsync(FlashCardSetDetails set);
}