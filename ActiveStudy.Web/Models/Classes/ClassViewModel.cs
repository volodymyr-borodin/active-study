using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Web.Models.Shared;

namespace ActiveStudy.Web.Models.Classes
{
    public class ClassViewModel
    {
        public ClassViewModel(string id, School school, string title, IEnumerable<StudentListModel> students)
        {
            Id = id;
            School = school;
            Title = title;
            Students = students;
        }

        public string Id { get; }
        public School School { get; }
        public string Title { get; }
        public IEnumerable<StudentListModel> Students { get; }
        
        public MainInfoModel MainInfo => new MainInfoModel(Title, "fa-university");
    }
}