using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class CrmContext
    {
        private readonly IMongoDatabase database;
        public IMongoCollection<SchoolEntity> Schools => database.GetCollection<SchoolEntity>("schools");
        public IMongoCollection<ClassEntity> Classes => database.GetCollection<ClassEntity>("classes");
        public IMongoCollection<TeacherEntity> Teachers => database.GetCollection<TeacherEntity>("teachers");
        public IMongoCollection<StudentEntity> Students => database.GetCollection<StudentEntity>("students");
        public IMongoCollection<RelativeEntity> Relatives => database.GetCollection<RelativeEntity>("relatives");
        public IMongoCollection<EventEntity> Events => database.GetCollection<EventEntity>("events");
        public IMongoCollection<ScheduleTemplateEntity> ScheduleTemplates => database.GetCollection<ScheduleTemplateEntity>("scheduleTemplates");

        public CrmContext(MongoUrl url)
        {
            var client = new MongoClient(MongoClientSettings.FromUrl(url));
            database = client.GetDatabase(url.DatabaseName);
        }
    }
}
