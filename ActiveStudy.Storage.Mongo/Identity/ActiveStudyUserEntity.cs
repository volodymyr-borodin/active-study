using Microsoft.AspNetCore.Identity;

namespace ActiveStudy.Storage.Mongo.Identity
{
    public class ActiveStudyUserEntity : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}