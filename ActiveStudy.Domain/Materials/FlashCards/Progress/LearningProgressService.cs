using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Materials.FlashCards.Progress;

public class LearningProgressService
{
    private readonly Random random = new Random(DateTime.UtcNow.Millisecond);
    private readonly FlashCardsService flashCardsService;
    private readonly IProgressStorage progressStorage;

    public LearningProgressService(FlashCardsService flashCardsService, IProgressStorage progressStorage)
    {
        this.flashCardsService = flashCardsService;
        this.progressStorage = progressStorage;
    }

    public Task<SetLearningProgress> GetProgressAsync(string userId, FlashCardSetDetails set)
    {
        return progressStorage.GetSetProgressAsync(userId, set);
    }

    public async Task<LearningRound> GetNextRoundAsync(string userId, string cardSetId)
    {
        var set = await flashCardsService.GetByIdAsync(cardSetId);
        var process = await GetProgressAsync(userId, set);
        var learning = process.CardsProgress
            .Where(cardProgress => !cardProgress.Learned)
            .OrderBy(cardProgress => cardProgress.Progress)
            .ThenBy(progress => random.Next())
            .Take(5)
            .Select(cardProgress => new LearningRoundItem(cardProgress.Progress, cardProgress.Card))
            .ToList();

        return new LearningRound(set, learning);
    }

    public async Task<bool> IsFinishedAsync(string userId, string cardSetId)
    {
        var set = await flashCardsService.GetByIdAsync(cardSetId);
        var process = await GetProgressAsync(userId, set);

        return process.CardsProgress.All(cardProgress => cardProgress.Learned);
    }
    
    public async Task<IEnumerable<AnswerResult>> UpdateProgressAsync(string userId, string cardSetId, IEnumerable<NewAnswer> answers)
    {
        var set = await flashCardsService.GetByIdAsync(cardSetId);

        var result = new List<AnswerResult>(answers.Count());
        foreach (var answer in answers)
        {
            var card = set.Cards.FirstOrDefault(c => c.Id == answer.TermId);
            if (card == null)
            {
                continue;
            }

            var a = new AnswerResult(answer, card);
            if (a.IsCorrect)
            {
                await progressStorage.IncreaseProgressAsync(userId, card, 5);
            }
            else
            {
                await progressStorage.DecreaseProgressAsync(userId, card, 3);
            }

            result.Add(a);
        }

        return result;
    }

    public async Task ResetProgressAsync(string userId, string cardSetId)
    {
        var set = await flashCardsService.GetByIdAsync(cardSetId);
        await progressStorage.ClearSetProgressAsync(userId, set);
    }
}