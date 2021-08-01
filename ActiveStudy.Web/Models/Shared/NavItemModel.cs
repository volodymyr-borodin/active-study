namespace ActiveStudy.Web.Models.Shared
{
    public class NavItemModel
    {
        public NavItemModel(string title, string link, string icon = "", bool isSelected = false)
        {
            Title = title;
            Link = link;
            Icon = icon;
            IsSelected = isSelected;
        }

        public string Title { get; }
        public string Link { get; }
        public string Icon { get; }
        public bool IsSelected { get; }

        public bool HasIcon => !string.IsNullOrEmpty(Icon);
    }
}