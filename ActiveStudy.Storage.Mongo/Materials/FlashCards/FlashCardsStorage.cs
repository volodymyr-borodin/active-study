using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using ActiveStudy.Domain.Materials.FlashCards;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Materials.FlashCards;

public class FlashCardsStorage : IFlashCardsStorage
{
    private readonly MaterialsContext context;

    public FlashCardsStorage(MaterialsContext context)
    {
        this.context = context;
    }
    
    public async Task<FlashCardSetDetails> GetByIdAsync(string id, AuthContext authContext = null)
    {
        var filter = Builders<FlashCardsSetEntity>.Filter.Eq(set => set.Id, ObjectId.Parse(id))
                     & AuthFilter(authContext);
        
        var entity = await context.FlashCardsSets
            .Find(filter)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            return null;
        }

        var cards = entity.Cards.Select(card =>
            new FlashCard(card.Id.ToString(), card.Term, card.Definition, card.Clues?.Select(clue => new Clue(clue.Text)) ?? Enumerable.Empty<Clue>()));

        return new FlashCardSetDetails(
            entity.Id.ToString(),
            entity.Title,
            entity.Description ?? string.Empty,
            entity.Status,
            entity.Author,
            cards);
    }

    public async Task<IEnumerable<FlashCardSet>> FindAsync(AuthContext authContext = null)
    {
        var filter = Builders<FlashCardsSetEntity>.Filter.Eq(set => set.Status, FlashCardSetStatus.Public);

        if (authContext != null)
        {
            filter |= Builders<FlashCardsSetEntity>.Filter.Eq(set => set.Status, FlashCardSetStatus.Private)
                      & Builders<FlashCardsSetEntity>.Filter.Eq(set => set.Author.UserId, authContext.OwnerId);
        }

        var entities = await context.FlashCardsSets
            .Find(filter)
            .ToListAsync();

        return entities.Select(e => new FlashCardSet(e.Id.ToString(), e.Title, e.Author));
    }

    public async Task InsertAsync(FlashCardSetDetails set)
    {
        await context.FlashCardsSets.InsertOneAsync(new FlashCardsSetEntity
        {
            Title = set.Title,
            Description = set.Description,
            Status = set.Status,
            Author = set.Author,
            Cards = set.Cards.Select(c => new FlashCardEntity
            {
                Id = ObjectId.GenerateNewId(),
                Term = c.Term,
                Definition = c.Definition,
                Clues = c.Clues.Select(cl => new ClueEntity
                {
                    Text = cl.Text
                })
            })
        });
    }

    private FilterDefinition<FlashCardsSetEntity> AuthFilter(AuthContext authContext)
    {
        var filter = Builders<FlashCardsSetEntity>.Filter.Eq(set => set.Status, FlashCardSetStatus.Public);

        if (authContext != null)
        {
            filter |= Builders<FlashCardsSetEntity>.Filter.Eq(set => set.Status, FlashCardSetStatus.Private)
                & Builders<FlashCardsSetEntity>.Filter.Eq(set => set.Author.UserId, authContext.OwnerId);
        }

        return filter;
    }
}