namespace ActiveStudy.Web.Components.Account
{
    public class AccountModel
    {
        public AccountModel(string profileImageLink, string fullName, string username)
        {
            ProfileImageLink = profileImageLink;
            FullName = fullName;
            Username = username;
        }

        public string ProfileImageLink { get; }
        public string FullName { get; }
        public string Username { get; }
    }
}