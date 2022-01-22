using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveStudy.Storage.Mongo.Identity;
using Microsoft.AspNetCore.Http;

namespace ActiveStudy.Web.Models
{
    public class CurrentUserProvider
    {
        private readonly UserManager userManager;
        private readonly RoleManager roleManager;

        public CurrentUserProvider(UserManager userManager,
            RoleManager roleManager,
            IHttpContextAccessor contextAccessor)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;

            if (contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false)
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

            var claims = (await userManager.GetClaimsAsync(User)).ToList();
            foreach (var roleName in await userManager.GetRolesAsync(User))
            {
                var role = await roleManager.FindByNameAsync(roleName);
                claims.AddRange(await roleManager.GetClaimsAsync(role));
            }

            return claims
                .Where(c => c.Type == CustomClaimTypes.School)
                .Select(c => c.Value)
                .ToList();
        }
    }
}