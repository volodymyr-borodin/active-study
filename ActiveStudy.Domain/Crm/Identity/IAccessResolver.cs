using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Identity;

public interface IAccessResolver
{
    Task<bool> HasFullAccessAsync(string userId, string schoolId, Guid section);
    Task<bool> HasFullAccessAsync(ClaimsPrincipal user, string schoolId, Guid section);

    Task<bool> HasReadAccessAsync(string userId, string schoolId, Guid section);
    Task<bool> HasReadAccessAsync(ClaimsPrincipal user, string schoolId, Guid section);
}