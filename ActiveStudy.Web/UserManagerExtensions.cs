using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ActiveStudy.Storage.Mongo.Identity;
using Microsoft.AspNetCore.Identity;

namespace ActiveStudy.Web
{
    public static class UserManagerExtensions
    {
        public static async Task AddSchoolClaimAsync(this UserManager<ActiveStudyUserEntity> userManager,
            ActiveStudyUserEntity user, string schoolId)
        {
            await userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.School, schoolId));
        }
        
        public static async Task<IEnumerable<Claim>> GetSchoolClaimsAsync(this UserManager<ActiveStudyUserEntity> userManager, ActiveStudyUserEntity user)
        {
            return await userManager.GetClaimsAsync(user);
        }
    }
}
