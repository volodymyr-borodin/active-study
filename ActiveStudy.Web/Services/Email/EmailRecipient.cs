namespace ActiveStudy.Web.Services.Email
{
    public class EmailRecipient
    {
        public EmailRecipient(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }
        public string Email { get; }
    }
}