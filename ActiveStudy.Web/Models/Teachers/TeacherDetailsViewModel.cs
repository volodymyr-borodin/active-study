using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Models.Teachers
{
    public class TeacherDetailsViewModel
    {
        public TeacherDetailsViewModel(string id, string fullName, School school, Schedule schedule)
        {
            Id = id;
            FullName = fullName;
            School = school;
            Schedule = schedule;
        }

        public string Id { get; }
        public string FullName { get; }
        public School School { get; }
        public Schedule Schedule { get; }
    }
}