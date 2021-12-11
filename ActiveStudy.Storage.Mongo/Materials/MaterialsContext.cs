using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Materials
{
    public class MaterialsContext
    {
        private readonly IMongoDatabase database;
        public IMongoCollection<TestWorkEntity> TestWorks => database.GetCollection<TestWorkEntity>("testWorks");
        public IMongoCollection<TestWorkResultEntity> TestWorkResults => database.GetCollection<TestWorkResultEntity>("testWorkResults");

        public MaterialsContext(MongoUrl url)
        {
            var client = new MongoClient(MongoClientSettings.FromUrl(url));
            database = client.GetDatabase(url.DatabaseName);
        }
    }
}
