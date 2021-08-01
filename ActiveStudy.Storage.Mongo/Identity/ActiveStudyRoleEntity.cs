using Microsoft.AspNetCore.Identity;

namespace ActiveStudy.Storage.Mongo.Identity
{
    public class ActiveStudyRoleEntity : IdentityRole
    {
        public string SchoolId { get; set; }
    }
}
