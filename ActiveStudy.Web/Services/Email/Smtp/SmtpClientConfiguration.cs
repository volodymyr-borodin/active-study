namespace ActiveStudy.Web.Services.Email.Smtp
{
    public class SmtpClientConfiguration
    {
        public SmtpClientConfiguration(string host, int port, bool enableSsl, string userName,
            string password, string displayName, string email)
        {
            Host = host;
            Port = port;
            EnableSsl = enableSsl;
            
            UserName = userName;
            Password = password;
            
            DisplayName = displayName;
            Email = email;
        }

        public string Host { get; }
        public int Port { get; }
        public bool EnableSsl { get; }
        public string UserName { get; }
        public string Password { get; }
        public string DisplayName { get; }
        public string Email { get; }
    }
}