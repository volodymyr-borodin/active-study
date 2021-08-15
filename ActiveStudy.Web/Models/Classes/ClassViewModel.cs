using System.Collections.Generic;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;

namespace ActiveStudy.Web.Models.Classes
{
    public class ClassViewModel
    {
        public ClassViewModel(string id, string title, School school, TeacherShortInfo teacher, IEnumerable<Student> students, Schedule schedule)
        {
            Id = id;
            Title = title;
            School = school;
            Teacher = teacher;
            Students = students;
            Schedule = schedule;
        }

        public string Id { get; }
        public string Title { get; }
        public School School { get; }
        public TeacherShortInfo Teacher { get; }
        public IEnumerable<Student> Students { get; }
        public Schedule Schedule { get; }
    }
}