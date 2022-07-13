using System.Security.Claims;
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

        public static AuthContext GetAuthContext(this ClaimsPrincipal user)
        {
            return user.Identity?.IsAuthenticated ?? false
                ? new AuthContext("123")//user.FindFirstValue(JwtRegisteredClaimNames.Sub)!)
                : null;
        }
    }
}