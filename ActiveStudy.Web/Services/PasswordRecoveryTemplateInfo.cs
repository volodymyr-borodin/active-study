using ActiveStudy.Storage.Mongo.Identity;

namespace ActiveStudy.Web.Services
{
    public class PasswordRecoveryTemplateInfo
    {
        public PasswordRecoveryTemplateInfo(ActiveStudyUserEntity user, string resetPasswordUrl)
        {
            User = user;
            ResetPasswordUrl = resetPasswordUrl;
        }

        public ActiveStudyUserEntity User { get; }
        public string ResetPasswordUrl { get; }
    }
}