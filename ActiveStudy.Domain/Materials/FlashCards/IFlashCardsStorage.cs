using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Materials.FlashCards;

public interface IFlashCardsStorage
{
    Task<FlashCardSetDetails> GetByIdAsync(string id, AuthContext authContext = null);
    Task<IEnumerable<FlashCardSet>> FindAsync(AuthContext authContext = null);
    Task InsertAsync(FlashCardSetDetails set);
}