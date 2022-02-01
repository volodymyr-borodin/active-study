using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Identity;
using ActiveStudy.Storage.Mongo.Identity;

namespace ActiveStudy.Storage.Mongo.Crm;

public class AccessResolver : IAccessResolver
{
    private readonly UserManager userManager;
    private readonly RoleManager roleManager;

    public AccessResolver(UserManager userManager, RoleManager roleManager)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task<bool> HasFullAccessAsync(ClaimsPrincipal user, string schoolId, Guid section)
    {
        var userRoles = await GetUserRoleInSchoolAsync(user, schoolId);

        return userRoles.Any(role => role.Access[section] == AccessLevel.Full);
    }

    public async Task<bool> HasReadAccessAsync(ClaimsPrincipal user, string schoolId, Guid section)
    {
        var userRoles = await GetUserRoleInSchoolAsync(user, schoolId);

        return userRoles.Any(role => role.Access[section] == AccessLevel.Readonly
                                     || role.Access[section] == AccessLevel.Full);
    }

    private async Task<IEnumerable<Role>> GetUserRoleInSchoolAsync(ClaimsPrincipal user, string schoolId)
    {
        return await GetUserRoleInSchoolAsync(await userManager.GetUserAsync(user), schoolId);
    }

    private async Task<IEnumerable<Role>> GetUserRoleInSchoolAsync(ActiveStudyUserEntity user, string schoolId)
    {
        var rolesName = await userManager.GetRolesAsync(user);
        var roles = new List<Role>(rolesName.Count);
        foreach (var roleName in await userManager.GetRolesAsync(user))
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var claims = await roleManager.GetClaimsAsync(role);
            if (claims.Any(c => c.Type == CustomClaimTypes.School && c.Value == schoolId))
            {
                roles.Add(RoleNameToRole(role.Name[..role.Name.LastIndexOf('_')]));
            }
        }

        return roles;
    }

    private static Role RoleNameToRole(string roleName)
    {
        return roleName switch
        {
            Role.PrincipalName => Role.Principal,
            Role.TeacherName => Role.Teacher,
            Role.StudentName => Role.Student,
            Role.RelativeName => Role.Relative,
            _ => throw new ArgumentOutOfRangeException(nameof(roleName), roleName, null)
        };
    }
}