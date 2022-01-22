using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ActiveStudy.Storage.Mongo.Identity;

public class UserManager : UserManager<ActiveStudyUserEntity>
{
    public UserManager(IUserStore<ActiveStudyUserEntity> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ActiveStudyUserEntity> passwordHasher, IEnumerable<IUserValidator<ActiveStudyUserEntity>> userValidators, IEnumerable<IPasswordValidator<ActiveStudyUserEntity>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ActiveStudyUserEntity>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    public async Task<IdentityResult> AddToRoleAsync(ActiveStudyUserEntity user, Role role, string schoolId)
    {
        return await base.AddToRoleAsync(user, $"{role.Name}_{schoolId}");
    }

}