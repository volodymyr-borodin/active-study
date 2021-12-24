using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Scheduler;

public interface IEventsStorage
{
    Task<IEnumerable<Event>> GetByClassAsync(string classId, DateOnly from, DateOnly to);
}