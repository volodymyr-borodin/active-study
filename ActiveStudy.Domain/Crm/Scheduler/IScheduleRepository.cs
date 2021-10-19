using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Scheduler
{
    public interface ISchedulerStorage
    {
        Task CreateAsync(Event @event);
        Task<Schedule> GetByClassAsync(string classId, DateTime from, DateTime to);
        Task<Schedule> GetByTeacherAsync(string teacherId, DateTime from, DateTime to);
    }

    public class Schedule : Dictionary<DateTime, IEnumerable<Event>>
    {
        public Schedule(IDictionary<DateTime, IEnumerable<Event>> schedule) : base(schedule)
        { }
    }
}