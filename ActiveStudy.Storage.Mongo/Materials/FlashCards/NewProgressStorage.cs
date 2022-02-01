using System;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain.Materials.FlashCards;
using ActiveStudy.Domain.Materials.FlashCards.Progress;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Materials.FlashCards;

public class NewProgressStorage : IProgressStorage
{
    private readonly MaterialsContext context;

    public NewProgressStorage(MaterialsContext context)
    {
        this.context = context;
    }

    public async Task<SetLearningProgress> GetSetProgressAsync(string userId, FlashCardSetDetails set)
    {
        var filter = Builders<UserCardProgressEntity>.Filter.Eq(progress => progress.UserId, userId)
            & Builders<UserCardProgressEntity>.Filter.In(progress => progress.TermId, set.Cards.Select(c => c.Id));

        var setProgress = (await context.UserCardProgress.Find(filter).ToListAsync())
            .ToDictionary(p => p.TermId);

        return new SetLearningProgress(set.Cards
            .Select(card => setProgress.ContainsKey(card.Id)
                ? new CardLearningProgress(setProgress[card.Id].Progress, card)
                : new CardLearningProgress(0, card)));
    }

    public async Task IncreaseProgressAsync(string userId, FlashCard card, int amount)
    {
        var filter = Builders<UserCardProgressEntity>.Filter.Eq(progress => progress.UserId, userId)
                     & Builders<UserCardProgressEntity>.Filter.Eq(progress => progress.TermId, card.Id);

        var progress = await context.UserCardProgress.Find(filter).FirstOrDefaultAsync();
        if (progress == null)
        {
            await context.UserCardProgress.ReplaceOneAsync(filter, new UserCardProgressEntity
                {
                    UserId = userId,
                    TermId = card.Id,
                    Progress = amount,
                    UpdatedOn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                },
                new ReplaceOptions
                {
                    IsUpsert = true
                });
        }
        else
        {
            var update = Builders<UserCardProgressEntity>.Update
                .Set(p => p.Progress, Math.Min(progress.Progress + amount, 10));

            var transactionFilter = filter & Builders<UserCardProgressEntity>.Filter
                .Eq(p => p.Progress, progress.Progress);

            await context.UserCardProgress.UpdateOneAsync(transactionFilter, update);
        }
    }

    public async Task DecreaseProgressAsync(string userId, FlashCard card, int amount)
    {
        var filter = Builders<UserCardProgressEntity>.Filter.Eq(progress => progress.UserId, userId)
                     & Builders<UserCardProgressEntity>.Filter.Eq(progress => progress.TermId, card.Id);
        var progress = await context.UserCardProgress.Find(filter).FirstOrDefaultAsync();

        if (progress != null)
        {
            var update = Builders<UserCardProgressEntity>.Update
                .Set(p => p.Progress, Math.Max(progress.Progress - amount, 0));

            var transactionFilter = filter & Builders<UserCardProgressEntity>.Filter
                .Eq(p => p.Progress, progress.Progress);

            await context.UserCardProgress.UpdateOneAsync(transactionFilter, update);
        }
    }

    public async Task ClearProgressAsync(string userId, FlashCard card)
    {
        var filter = Builders<UserCardProgressEntity>.Filter.Eq(progress => progress.UserId, userId)
                     & Builders<UserCardProgressEntity>.Filter.Eq(progress => progress.TermId, card.Id);

        await context.UserCardProgress.DeleteOneAsync(filter);
    }
}