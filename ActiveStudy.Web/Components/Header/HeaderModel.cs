namespace ActiveStudy.Web.Components.Header
{
    public class HeaderModel
    {
        public HeaderModel(bool isAuthenticated, AccountModel accountModel)
        {
            IsAuthenticated = isAuthenticated;
            AccountModel = accountModel;
        }

        public bool IsAuthenticated { get; }
        public AccountModel AccountModel { get; }
    }
}