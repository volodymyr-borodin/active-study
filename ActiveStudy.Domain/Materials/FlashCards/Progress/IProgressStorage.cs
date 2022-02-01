using System.Threading.Tasks;

namespace ActiveStudy.Domain.Materials.FlashCards.Progress;

public interface IProgressStorage
{
    Task<SetLearningProgress> GetSetProgressAsync(string userId, FlashCardSetDetails set);
    Task IncreaseProgressAsync(string userId, FlashCard card, int amount);
    Task DecreaseProgressAsync(string userId, FlashCard card, int amount);
    Task ClearSetProgressAsync(string userId, FlashCardSetDetails set);
}
