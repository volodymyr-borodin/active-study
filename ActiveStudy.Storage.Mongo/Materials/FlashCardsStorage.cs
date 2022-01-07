using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveStudy.Domain.Materials.FlashCards;

namespace ActiveStudy.Storage.Mongo.Materials;

public class FlashCardsStorage : IFlashCardsStorage
{
    public async Task<FlashCardSetDetails> GetByIdAsync(string id)
    {
        return new FlashCardSetDetails("1", "Set 1", new []
        {
            new FlashCard("1", "Negotiate", "to have formal discussions with someone in order to reach an agreement with them", new []
            {
                new Clue("In my previous job, I ______ client contracts to find a price point that worked best for their needs")
            }),
            new FlashCard("2", "Supervise", "to watch a person or activity to make certain that everything is done correctly, safely, etc", new []
            {
                new Clue("I ______ a team of 10 product engineers throughout the design and testing phases for prototypes")
            }),
            new FlashCard("3", "Coordinate", "to make many different things work effectively as a whole", new []
            {
                new Clue("I was responsible for ______ department activities in the absence of my direct superior"),
                new Clue("We need someone to ______ the whole campaign")
            })
        });
    }

    public async Task<IEnumerable<FlashCardSet>> FindAsync()
    {
        return new[]
        {
            new FlashCardSet("1", "Set 1"),
            new FlashCardSet("2", "Set 2"),
            new FlashCardSet("3", "Set 3"),
            new FlashCardSet("4", "Set 4"),
            new FlashCardSet("5", "Set 5")
        };
    }
}