using ActiveStudy.Storage.Mongo.Identity;

namespace ActiveStudy.Web.Services
{
    public class EmailConfirmationTemplateInfo
    {
        public EmailConfirmationTemplateInfo(ActiveStudyUserEntity user, string confirmEmailUrl)
        {
            User = user;
            ConfirmEmailUrl = confirmEmailUrl;
        }

        public ActiveStudyUserEntity User { get; }
        public string ConfirmEmailUrl { get; }
    }
}