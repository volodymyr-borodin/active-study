namespace ActiveStudy.Web.Components.Breadcrumb
{
    public class BreadcrumbItem
    {
        public string Text { get; }
        public string Link { get; }
        public bool IsActive { get; }

        public BreadcrumbItem(string text, string link, bool isActive)
        {
            Text = text;
            Link = link;
            IsActive = isActive;
        }

        public static BreadcrumbItem Active(string text, string link)
        {
            return new BreadcrumbItem(text, link, true);
        }

        public static BreadcrumbItem InActive(string text)
        {
            return new BreadcrumbItem(text, string.Empty, false);
        }
    }
}