using System.Threading.Tasks;

namespace ActiveStudy.Domain.Materials.FlashCards.Progress;

public interface IProgressStorage
{
    Task<CardLearningProgress> GetCardProgressAsync(string userId, FlashCard card);
    Task IncreaseProgressAsync(string userId, FlashCard card);
    Task DecreaseProgressAsync(string userId, FlashCard card);
}
