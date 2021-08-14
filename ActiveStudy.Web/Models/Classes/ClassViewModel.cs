using System.Collections.Generic;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Domain.Crm.Students;
using ActiveStudy.Web.Models.Shared;

namespace ActiveStudy.Web.Models.Classes
{
    public class ClassViewModel
    {
        public ClassViewModel(string id, string title, School school, TeacherShortInfo teacher, IEnumerable<Student> students)
        {
            Id = id;
            Title = title;
            School = school;
            Teacher = teacher;
            Students = students;
        }

        public string Id { get; }
        public string Title { get; }
        public School School { get; }
        public TeacherShortInfo Teacher { get; }
        public IEnumerable<Student> Students { get; }
        
        public MainInfoModel MainInfo => new MainInfoModel(Title, "fa-university");
    }
}