using System.Collections.Generic;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Scheduler;
using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Models.Classes
{
    public class ClassViewModel
    {
        public ClassViewModel(string id, string title, School school, TeacherShortInfo teacher, IEnumerable<StudentViewModel> students, Schedule schedule)
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
        public IEnumerable<StudentViewModel> Students { get; }
        public Schedule Schedule { get; }
    }

    public class StudentViewModel
    {
        public string Id { get; }
        public string FullName { get; }
        public IEnumerable<RelativeViewModel> Relatives { get; }

        public StudentViewModel(string id, string fullName, IEnumerable<RelativeViewModel> relatives)
        {
            Id = id;
            FullName = fullName;
            Relatives = relatives;
        }
    }

    public class RelativeViewModel
    {
        public string Id { get; }
        public string FullName { get; }
        public string Phone { get; }

        public RelativeViewModel(string id, string fullName, string phone)
        {
            Id = id;
            FullName = fullName;
            Phone = phone;
        }
    }
}