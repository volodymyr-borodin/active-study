namespace ActiveStudy.Web.Models.Shared
{
    public class MainInfoModel
    {
        public MainInfoModel(string title, string icon)
        {
            Title = title;
            Icon = icon;
        }

        public string Title { get; }
        public string Icon { get; }
    }
}