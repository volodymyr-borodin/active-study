using ActiveStudy.Storage.Mongo.Materials.FlashCards;
using ActiveStudy.Storage.Mongo.Materials.TestWorks;
using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Materials
{
    public class MaterialsContext
    {
        private readonly IMongoDatabase database;
        public IMongoCollection<TestWorkEntity> TestWorks => database.GetCollection<TestWorkEntity>("testWorks");
        public IMongoCollection<TestWorkResultEntity> TestWorkResults => database.GetCollection<TestWorkResultEntity>("testWorkResults");

        public IMongoCollection<FlashCardsSetEntity> FlashCardsSets => database.GetCollection<FlashCardsSetEntity>("flashCardsSets");
        public IMongoCollection<UserFlashCardsProgressEntity> FlashCardsProgress => database.GetCollection<UserFlashCardsProgressEntity>("flashCardsProgress");

        public MaterialsContext(MongoUrl url)
        {
            var client = new MongoClient(MongoClientSettings.FromUrl(url));
            database = client.GetDatabase(url.DatabaseName);
        }
    }
}
