using MongoDB.Driver;

namespace ActiveStudy.Storage.Mongo.Crm
{
    public class CrmContext
    {
        private readonly IMongoDatabase database;
        public IMongoCollection<SchoolEntity> Schools => database.GetCollection<SchoolEntity>("schools");
        public IMongoCollection<SchoolSubjectEntity> SchoolSubjects => database.GetCollection<SchoolSubjectEntity>("schoolSubjects");
        public IMongoCollection<ClassEntity> Classes => database.GetCollection<ClassEntity>("classes");
        public IMongoCollection<TeacherEntity> Teachers => database.GetCollection<TeacherEntity>("teachers");
        public IMongoCollection<StudentEntity> Students => database.GetCollection<StudentEntity>("students");
        public IMongoCollection<RelativeEntity> Relatives => database.GetCollection<RelativeEntity>("relatives");
        public IMongoCollection<ScheduleLessonEntity> ScheduleLessons => database.GetCollection<ScheduleLessonEntity>("scheduleLessons");
        public IMongoCollection<SchedulePeriodEntity> SchedulePeriods => database.GetCollection<SchedulePeriodEntity>("schedulePeriods");

        public CrmContext(MongoUrl url)
        {
            var client = new MongoClient(MongoClientSettings.FromUrl(url));
            database = client.GetDatabase(url.DatabaseName);
        }
    }
}
