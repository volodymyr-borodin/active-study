using ActiveStudy.Web.Models.Shared;

namespace ActiveStudy.Web.Models.Schools
{
    public class SchoolHomePageModel
    {
        public SchoolHomePageModel(string id, string title)
        {
            Id = id;
            Title = title;
        }

        public string Id { get; }
        public string Title { get; }

        public MainInfoModel MainInfo => new MainInfoModel(Title, "fa-university");
    }
}