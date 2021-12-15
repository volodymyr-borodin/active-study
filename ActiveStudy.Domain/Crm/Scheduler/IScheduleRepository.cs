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

    public class Schedule : Dictionary<DateOnly, IReadOnlyCollection<Event>>
    {
        public Schedule(Dictionary<DateOnly, IReadOnlyCollection<Event>> schedule) : base(schedule)
        { }
    }
}