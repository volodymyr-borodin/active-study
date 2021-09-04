using ActiveStudy.Domain.Crm.Schools;
using ActiveStudy.Storage.Mongo.Identity;

namespace ActiveStudy.Web.Services
{
    public class InvitationEmailTemplateInfo
    {
        public ActiveStudyUserEntity User { get; }
        public School School { get; }
        public string CompleteInvitationUrl { get; }
        public ActiveStudyUserEntity Invitor { get; }

        public InvitationEmailTemplateInfo(ActiveStudyUserEntity user, School school, string completeInvitationUrl, ActiveStudyUserEntity invitor)
        {
            User = user;
            School = school;
            CompleteInvitationUrl = completeInvitationUrl;
            Invitor = invitor;
        }
    }
}