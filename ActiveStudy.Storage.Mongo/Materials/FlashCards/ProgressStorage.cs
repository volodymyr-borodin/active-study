using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveStudy.Domain.Materials.FlashCards;
using ActiveStudy.Domain.Materials.FlashCards.Progress;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Materials.FlashCards;

public class ProgressStorage : IProgressStorage
{
    private readonly MaterialsContext context;

    public ProgressStorage(MaterialsContext context)
    {
        this.context = context;
    }
    
    public async Task<CardLearningProgress> GetCardProgressAsync(string userId, FlashCard card)
    {
        var userProgress = await context.FlashCardsProgress
            .Find(Builders<UserFlashCardsProgressEntity>.Filter.Eq(progress => progress.UserId, userId))
            .FirstOrDefaultAsync();

        var progress = 0;
        if (userProgress != null && userProgress.Progress.ContainsKey(card.Id))
        {
            progress = userProgress.Progress[card.Id];
        }

        return new CardLearningProgress(progress, card);
    }

    public async Task IncreaseProgressAsync(string userId, FlashCard card)
    {
        var userProgress = await context.FlashCardsProgress
            .Find(Builders<UserFlashCardsProgressEntity>.Filter.Eq(progress => progress.UserId, userId))
            .FirstOrDefaultAsync();

        if (userProgress == null)
        {
            await context.FlashCardsProgress.InsertOneAsync(new UserFlashCardsProgressEntity
            {
                UserId = userId,
                Progress = new Dictionary<string, int>
                {
                    [card.Id] = 5
                }
            });
        }
        else
        {
            if (userProgress.Progress.ContainsKey(card.Id))
            {
                userProgress.Progress[card.Id] = Math.Min(userProgress.Progress[card.Id] + 5, 10);
            }
            else
            {
                userProgress.Progress[card.Id] = 5;
            }
            await context.FlashCardsProgress
                .ReplaceOneAsync(Builders<UserFlashCardsProgressEntity>.Filter.Eq(progress => progress.UserId, userId), userProgress);
        }
    }

    public async Task DecreaseProgressAsync(string userId, FlashCard card)
    {
        var userProgress = await context.FlashCardsProgress
            .Find(Builders<UserFlashCardsProgressEntity>.Filter.Eq(progress => progress.UserId, userId))
            .FirstOrDefaultAsync();

        if (userProgress != null)
        {
            if (userProgress.Progress.ContainsKey(card.Id))
            {
                userProgress.Progress[card.Id] = Math.Max(userProgress.Progress[card.Id] - 5, 0);
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
