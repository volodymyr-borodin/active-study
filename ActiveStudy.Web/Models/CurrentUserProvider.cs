using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Storage.Mongo.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ActiveStudy.Web.Models
{
    public class CurrentUserProvider
    {
        private readonly UserManager<ActiveStudyUserEntity> userManager;

        public CurrentUserProvider(UserManager<ActiveStudyUserEntity> userManager, IHttpContextAccessor contextAccessor)
        {
            this.userManager = userManager;

            if (contextAccessor.HttpContext.User?.Identity?.IsAuthenticated ?? false)
            {
                User = userManager.GetUserAsync(contextAccessor.HttpContext.User).GetAwaiter().GetResult();
            }
        }

        public ActiveStudyUserEntity User { get; }
        public bool IsAuthenticated => User != null;

        public async Task<IEnumerable<string>> GetAssignedSchoolAsync()
        {
            if (!IsAuthenticated)
            {
                return Enumerable.Empty<string>();
            }
            
            var schoolClaims = await userManager.GetSchoolClaimsAsync(User);
            return schoolClaims.Select(c => c.Value).ToList();
        }
    }
}