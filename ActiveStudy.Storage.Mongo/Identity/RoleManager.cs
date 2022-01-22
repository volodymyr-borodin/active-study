using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ActiveStudy.Storage.Mongo.Identity;

public class RoleManager : RoleManager<ActiveStudyRoleEntity>
{
    public RoleManager(IRoleStore<ActiveStudyRoleEntity> store, IEnumerable<IRoleValidator<ActiveStudyRoleEntity>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<ActiveStudyRoleEntity>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
    {
    }

    public async Task<IdentityResult> AddDefaultAsync(string schoolId)
    {
        await CreateAsync(schoolId, Role.Principal.Name);
        await CreateAsync(schoolId, Role.Teacher.Name);
        await CreateAsync(schoolId, Role.Student.Name);
        await CreateAsync(schoolId, Role.Relative.Name);

        return IdentityResult.Success;
    }

    private async Task<IdentityResult> CreateAsync(string schoolId, string role)
    {
        var studyRole = new ActiveStudyRoleEntity
        {
            Name = $"{role}_{schoolId}"
        };

        var result = await base.CreateAsync(studyRole);
        if (!result.Succeeded)
        {
            return result;
        }

        return await base.AddClaimAsync(studyRole, new Claim(CustomClaimTypes.School, schoolId));
    }
}