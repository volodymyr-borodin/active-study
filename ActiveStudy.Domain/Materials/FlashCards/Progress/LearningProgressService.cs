using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Materials.FlashCards.Progress;

public class LearningProgressService
{
    private readonly FlashCardsService flashCardsService;
    private readonly IProgressStorage progressStorage;

    public LearningProgressService(FlashCardsService flashCardsService, IProgressStorage progressStorage)
    {
        this.flashCardsService = flashCardsService;
        this.progressStorage = progressStorage;
    }

    public async Task<SetLearningProgress> GetProgressAsync(string userId, FlashCardSetDetails set)
    {
        var progresses = new List<CardLearningProgress>(set.Cards.Count());
        foreach (var card in set.Cards)
        {
            progresses.Add(await progressStorage.GetCardProgressAsync(userId, card));
        }

        return new SetLearningProgress(set.Id, set.Title, progresses);
    }

    public async Task<LearningRound> GetNextRoundAsync(string userId, string cardSetId)
    {
        var set = await flashCardsService.GetByIdAsync(cardSetId);
        var process = await GetProgressAsync(userId, set);
        var learning = process.CardsProgress
            .Where(cardProgress => !cardProgress.Learned)
            .OrderBy(cardProgress => cardProgress.Progress)
            .Take(5)
            .Select(cardProgress => new LearningRoundItem(cardProgress.Progress, cardProgress.Card))
            .ToList();

        return new LearningRound(set, learning);
    }

    public async Task UpdateProgressAsync(string userId, string cardSetId, IEnumerable<NewAnswer> answers)
    {
        var set = await flashCardsService.GetByIdAsync(cardSetId);

        foreach (var answer in answers)
        {
            var card = set.Cards.FirstOrDefault(c => c.Id == answer.TermId);
            if (card == null)
            {
                continue;
            }

            if (card.Term == answer.Answer)
            {
                await progressStorage.IncreaseProgressAsync(userId, card);
            }
            else
            {
                await progressStorage.DecreaseProgressAsync(userId, card);
            }
        }
    }
}