namespace ActiveStudy.Web.Components.Header
{
    public class HeaderModel
    {
        public HeaderModel(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }

        public bool IsAuthenticated { get; }
    }
}