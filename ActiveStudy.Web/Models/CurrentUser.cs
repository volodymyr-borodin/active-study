using ActiveStudy.Domain;
using ActiveStudy.Storage.Mongo.Identity;

namespace ActiveStudy.Web.Models
{
    public static class CurrentUserExtension
    {
        public static User AsUser(this ActiveStudyUserEntity currentUser)
        {
            return new User(currentUser.Id, $"{currentUser.FirstName} {currentUser.LastName}");
        }
    }
}