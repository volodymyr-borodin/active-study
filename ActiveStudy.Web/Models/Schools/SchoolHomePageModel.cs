using System.Collections.Generic;
using ActiveStudy.Web.Models.Shared;

namespace ActiveStudy.Web.Models.Schools
{
    public class SchoolHomePageModel
    {
        public SchoolHomePageModel(string id, string title,
            IEnumerable<ClassListModel> classes,
            IEnumerable<TeacherListModel> teachers)
        {
            Id = id;
            Title = title;
            Classes = classes;
            Teachers = teachers;
        }

        public string Id { get; }
        public string Title { get; }
        public IEnumerable<ClassListModel> Classes { get; }
        public IEnumerable<TeacherListModel> Teachers { get; }

        public MainInfoModel MainInfo => new MainInfoModel(Title, "fa-university");
    }
}