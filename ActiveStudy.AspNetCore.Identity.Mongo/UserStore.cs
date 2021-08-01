using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace ActiveStudy.AspNetCore.Identity.Mongo
{
    internal class UserStore<TUser, TRole, TKey, TUserRole, TUserClaim, TUserLogin, TUserToken> :
        UserStoreBase<TUser, TKey, TUserClaim, TUserLogin, TUserToken>,
        IUserRoleStore<TUser>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
    {
        private readonly IdentityDbContext<TKey> context;

        public UserStore(IdentityDbContext<TKey> context)
            : base(new IdentityErrorDescriber())
        {
            this.context = context;
        }

        public override IQueryable<TUser> Users => users.AsQueryable();

        public async override Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            await userClaims.InsertManyAsync(claims.Select(c => new TUserClaim
            {
                UserId = user.Id,
                ClaimType = c.Type,
                ClaimValue = c.Value
            }));
        }

        public async override Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            await userLogins.InsertOneAsync(new TUserLogin
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
                ProviderKey = login.ProviderKey
            });
        }

        public async override Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            await users.InsertOneAsync(user, cancellationToken: cancellationToken);

            return IdentityResult.Success;
        }

        public async override Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
        {
            await users.DeleteOneAsync(userFilter.Eq(u => u.Id, user.Id), cancellationToken);

            return IdentityResult.Success;
        }

        public async override Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            return await users
                .Find(userFilter.Eq(u => u.NormalizedEmail, normalizedEmail))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async override Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await users
                .Find(userFilter.Eq(u => u.Id, ConvertIdFromString(userId)))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async override Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            return await users
                .Find(userFilter.Eq(u => u.NormalizedUserName, normalizedUserName))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async override Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            var ucs = await userClaims
                .Find(userClaimFilter.Eq(u => u.UserId, user.Id))
                .ToListAsync(cancellationToken);

            return ucs.Select(cs => cs.ToClaim()).ToList();
        }

        public async override Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            var uls = await userLogins
                .Find(userLoginFilter.Eq(u => u.UserId, user.Id))
                .ToListAsync(cancellationToken);

            return uls.Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey, ul.ProviderDisplayName)).ToList();
        }

        public async override Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            var claimFilter = userClaimFilter.And(
                userClaimFilter.Eq(uc => uc.ClaimValue, claim.Value),
                userClaimFilter.Eq(uc => uc.ClaimType, claim.Type));

            var ucs = await userClaims
                .Find(claimFilter)
                .ToListAsync(cancellationToken);

            return await users
                .Find(userFilter.In(u => u.Id, ucs.Select(uc => uc.UserId)))
                .ToListAsync();
        }

        public async override Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            var claimsFilter = userClaimFilter.Or(claims.Select(c => userClaimFilter.And(
                userClaimFilter.Eq(uc => uc.ClaimValue, c.Value),
                userClaimFilter.Eq(uc => uc.ClaimType, c.Type))));

            var filter = userClaimFilter.And(
                userClaimFilter.Eq(uc => uc.UserId, user.Id),
                claimsFilter);

            await userClaims.DeleteManyAsync(filter, cancellationToken);
        }

        public async override Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            var filter = userLoginFilter.And(
                userLoginFilter.Eq(ul => ul.UserId, user.Id),
                userLoginFilter.Eq(ul => ul.LoginProvider, loginProvider),
                userLoginFilter.Eq(ul => ul.ProviderKey, providerKey));

            await userLogins.DeleteOneAsync(filter, cancellationToken);
        }

        public async override Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            var filter = userClaimFilter.And(
                userClaimFilter.Eq(uc => uc.UserId, user.Id),
                userClaimFilter.Eq(uc => uc.ClaimValue, claim.Value),
                userClaimFilter.Eq(uc => uc.ClaimType, claim.Type));

            await userClaims.ReplaceOneAsync(filter, new TUserClaim
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            }, cancellationToken: cancellationToken);
        }

        public async override Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            var filter = userFilter.Eq(u => u.Id, user.Id);

            await users.ReplaceOneAsync(filter, user, cancellationToken: cancellationToken);

            return IdentityResult.Success;
        }

        protected async override Task AddUserTokenAsync(TUserToken token)
        {
            await userTokens.InsertOneAsync(token);
        }

        protected async override Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            var filter = userTokenFilter.And(
                userTokenFilter.Eq(u => u.LoginProvider, loginProvider),
                userTokenFilter.Eq(u => u.Name, name),
                userTokenFilter.Eq(u => u.UserId, user.Id));

            return await userTokens.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        protected async override Task<TUser> FindUserAsync(TKey userId, CancellationToken cancellationToken)
        {
            var filter = userFilter.Eq(u => u.Id, userId);

            return await users.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        protected async override Task<TUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var filter = userLoginFilter.And(
                userLoginFilter.Eq(u => u.LoginProvider, loginProvider),
                userLoginFilter.Eq(u => u.ProviderKey, providerKey));

            return await userLogins.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        protected async override Task<TUserLogin> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var filter = userLoginFilter.And(
                userLoginFilter.Eq(u => u.UserId, userId),
                userLoginFilter.Eq(u => u.LoginProvider, loginProvider),
                userLoginFilter.Eq(u => u.ProviderKey, providerKey));

            return await userLogins.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        protected async override Task RemoveUserTokenAsync(TUserToken token)
        {
            var filter = userTokenFilter.And(
                userTokenFilter.Eq(ul => ul.UserId, token.UserId),
                userTokenFilter.Eq(ul => ul.LoginProvider, token.LoginProvider));

            await userTokens.DeleteOneAsync(filter);
        }

        public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var role = await roles.Find(roleFilter.Eq(r => r.NormalizedName, roleName)).FirstAsync();

            await userRoles.InsertOneAsync(new TUserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });
        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            var urs = await userRoles.Find(userRoleFilter.Eq(ur => ur.UserId, user.Id)).ToListAsync();
            var rs = await roles.Find(roleFilter.In(r => r.Id, urs.Select(ur => ur.RoleId))).ToListAsync();

            return rs.Select(r => r.Name).ToList();
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var role = await roles.Find(roleFilter.Eq(r => r.NormalizedName, roleName)).FirstAsync();
            var urs = await userRoles
                .Find(userRoleFilter.Eq(ur => ur.RoleId, role.Id))
                .ToListAsync();

            return await users.Find(userFilter.In(r => r.Id, urs.Select(ur => ur.UserId))).ToListAsync();
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var role = await roles.Find(roleFilter.Eq(r => r.NormalizedName, roleName))
                .FirstAsync();

            var filter = userRoleFilter.And(
                userRoleFilter.Eq(ur => ur.UserId, user.Id),
                userRoleFilter.Eq(ur => ur.RoleId, role.Id));

            return await userRoles
                .Find(filter)
                .AnyAsync();
        }

        public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var role = await roles.Find(roleFilter.Eq(r => r.NormalizedName, roleName)).FirstAsync();

            var filter = userRoleFilter.And(
                userRoleFilter.Eq(ur => ur.UserId, user.Id),
                userRoleFilter.Eq(ur => ur.RoleId, role.Id));
            await userRoles.DeleteOneAsync(filter);
        }

        private IMongoCollection<TUser> users => context.Collection<TUser>("users");
        private readonly FilterDefinitionBuilder<TUser> userFilter = Builders<TUser>.Filter;

        private IMongoCollection<TRole> roles => context.Collection<TRole>("roles");
        private readonly FilterDefinitionBuilder<TRole> roleFilter = Builders<TRole>.Filter;

        private IMongoCollection<TUserRole> userRoles => context.Collection<TUserRole>("userRoles");
        private readonly FilterDefinitionBuilder<TUserRole> userRoleFilter = Builders<TUserRole>.Filter;

        private IMongoCollection<TUserClaim> userClaims => context.Collection<TUserClaim>("userClaims");
        private readonly FilterDefinitionBuilder<TUserClaim> userClaimFilter = Builders<TUserClaim>.Filter;

        private IMongoCollection<TUserLogin> userLogins => context.Collection<TUserLogin>("userLogins");
        private readonly FilterDefinitionBuilder<TUserLogin> userLoginFilter = Builders<TUserLogin>.Filter;

        private IMongoCollection<TUserToken> userTokens => context.Collection<TUserToken>("userTokens");
        private readonly FilterDefinitionBuilder<TUserToken> userTokenFilter = Builders<TUserToken>.Filter;
    }
}
