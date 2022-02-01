using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Materials.FlashCards;
using ActiveStudy.Domain.Materials.FlashCards.Progress;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Materials.FlashCards;

public class ProgressStorage : IProgressStorage
{
    private readonly MaterialsContext context;

    public ProgressStorage(MaterialsContext context)
    {
        this.context = context;
    }

    public async Task<SetLearningProgress> GetSetProgressAsync(string userId, FlashCardSetDetails set)
    {
        var userProgress = await context.FlashCardsProgress
            .Find(Builders<UserFlashCardsProgressEntity>.Filter.Eq(progress => progress.UserId, userId))
            .FirstOrDefaultAsync();

        return new SetLearningProgress(set.Cards
            .Select(c => new CardLearningProgress(
                userProgress?.Progress.ContainsKey(c.Id) ?? false
                    ? userProgress.Progress[c.Id]
                    : 0, c)));
    }

    public async Task IncreaseProgressAsync(string userId, FlashCard card, int amount)
    {
        var userProgress = await context.FlashCardsProgress
            .Find(Builders<UserFlashCardsProgressEntity>.Filter.Eq(progress => progress.UserId, userId))
            .FirstOrDefaultAsync();

        if (userProgress == null)
        {
            await context.FlashCardsProgress.InsertOneAsync(new UserFlashCardsProgressEntity
            {
                Id = ObjectId.GenerateNewId(),
                UserId = userId,
                Progress = new Dictionary<string, int>
                {
                    [card.Id] = amount
                }
            });
        }
        else
        {
            if (userProgress.Progress.ContainsKey(card.Id))
            {
                userProgress.Progress[card.Id] = Math.Min(userProgress.Progress[card.Id] + amount, 10);
            }
            else
            {
                userProgress.Progress[card.Id] = amount;
            }
            await context.FlashCardsProgress
                .ReplaceOneAsync(Builders<UserFlashCardsProgressEntity>.Filter.Eq(progress => progress.UserId, userId), userProgress);
        }
    }

    public async Task DecreaseProgressAsync(string userId, FlashCard card, int amount)
    {
        var userProgress = await context.FlashCardsProgress
            .Find(Builders<UserFlashCardsProgressEntity>.Filter.Eq(progress => progress.UserId, userId))
            .FirstOrDefaultAsync();

        if (userProgress != null)
        {
            if (userProgress.Progress.ContainsKey(card.Id))
            {
                userProgress.Progress[card.Id] = Math.Max(userProgress.Progress[card.Id] - amount, 0);
                await context.FlashCardsProgress
                    .ReplaceOneAsync(Builders<UserFlashCardsProgressEntity>.Filter.Eq(progress => progress.UserId, userId), userProgress);
            }
        }
    }

    public async Task ClearProgressAsync(string userId, FlashCard card)
    {
        var userProgress = await context.FlashCardsProgress
            .Find(Builders<UserFlashCardsProgressEntity>.Filter.Eq(progress => progress.UserId, userId))
            .FirstOrDefaultAsync();

        if (userProgress != null)
        {
            if (userProgress.Progress.ContainsKey(card.Id))
            {
                userProgress.Progress.Remove(card.Id);
                await context.FlashCardsProgress
                    .ReplaceOneAsync(Builders<UserFlashCardsProgressEntity>.Filter.Eq(progress => progress.UserId, userId), userProgress);
            }
        }
    }
}
