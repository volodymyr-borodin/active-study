using System;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Identity;

public interface IAccessResolver
{
    Task<bool> HasFullAccessAsync(string userId, string schoolId, Guid section);

    Task<bool> HasReadAccessAsync(string userId, string schoolId, Guid section);
}