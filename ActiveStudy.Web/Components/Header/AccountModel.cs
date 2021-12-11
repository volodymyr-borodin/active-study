namespace ActiveStudy.Web.Components.Header
{
    public class AccountModel
    {
        public AccountModel(string fullName, string username)
        {
            FullName = fullName;
            Username = username;
        }

        public string FullName { get; }
        public string Username { get; }
    }
}
