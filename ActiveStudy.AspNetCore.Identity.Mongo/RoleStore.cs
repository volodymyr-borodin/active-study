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
    internal class RoleStore<TRole, TKey, TUserRole, TRoleClaim> :
        RoleStoreBase<TRole, TKey, TUserRole, TRoleClaim>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserRole : IdentityUserRole<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        private readonly IdentityDbContext<TKey> context;

        public RoleStore(IdentityDbContext<TKey> context) : base(new IdentityErrorDescriber())
        {
            this.context = context;
        }

        public async override Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            await roles.InsertOneAsync(role, null, cancellationToken);

            return IdentityResult.Success;
        }

        public async override Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            await RemoveClaimsAsync(role);
            await roles.DeleteOneAsync(roleFilter.Eq(r => r.Id, role.Id), cancellationToken);

            return IdentityResult.Success;
        }

        public override Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            return roles.Find(roleFilter.Eq(r => r.Id, ConvertIdFromString(id))).FirstOrDefaultAsync(cancellationToken);
        }

        public override Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken)
        {
            return roles.Find(roleFilter.Eq(r => r.NormalizedName, normalizedName)).FirstOrDefaultAsync(cancellationToken);
        }

        public async override Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            await roles.ReplaceOneAsync(roleFilter.Eq(r => r.Id, role.Id), role, cancellationToken: cancellationToken);

            return IdentityResult.Success;
        }

        public async override Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            await roleClaims.InsertOneAsync(new TRoleClaim
            {
                RoleId = role.Id,
                ClaimValue = claim.Value,
                ClaimType = claim.Type
            });
        }

        public async override Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            var rcs = await roleClaims
                .Find(roleClaimFilter.Eq(rc => rc.RoleId, role.Id))
                .ToListAsync();

            return rcs.Select(rc => rc.ToClaim()).ToList();
        }

        public async override Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            var filter = roleClaimFilter.And(
                roleClaimFilter.Eq(rc => rc.RoleId, role.Id),
                roleClaimFilter.Eq(rc => rc.ClaimType, claim.Type),
                roleClaimFilter.Eq(rc => rc.ClaimValue, claim.Value));

            await roleClaims.DeleteOneAsync(filter);
        }

        public async Task RemoveClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            var filter = roleClaimFilter.Eq(rc => rc.RoleId, role.Id);

            await roleClaims.DeleteOneAsync(filter, cancellationToken);
        }

        public override IQueryable<TRole> Roles => roles.AsQueryable();

        private IMongoCollection<TRole> roles => context.Collection<TRole>("roles");
        private FilterDefinitionBuilder<TRole> roleFilter = Builders<TRole>.Filter;

        private IMongoCollection<TRoleClaim> roleClaims => context.Collection<TRoleClaim>("roleClaims");
        private FilterDefinitionBuilder<TRoleClaim> roleClaimFilter = Builders<TRoleClaim>.Filter;
    }
}
