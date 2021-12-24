using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActiveStudy.Domain.Crm.Scheduler
{
    public interface ISchedulerStorage
    {
        Task CreateAsync(Event @event);
        Task<Schedule> GetByClassAsync(string classId, DateTime from, DateTime to);
        Task<Schedule> GetByTeacherAsync(string teacherId, DateTime from, DateTime to);
    }

    public record SchedulePeriod(TimeOnly Start, TimeOnly End);
    
    public class Schedule : Dictionary<DateOnly, IReadOnlyCollection<Event>>
    {
        public IEnumerable<SchedulePeriod> Periods { get; }

        public Schedule(Dictionary<DateOnly, IReadOnlyCollection<Event>> schedule,
            IEnumerable<SchedulePeriod> periods) : base(schedule)
        {
            Periods = periods;
        }

        public static Schedule Empty(DateOnly from, DateOnly to)
        {
            return new Schedule(DaysRange(@from, to)
                .ToDictionary(r => r, d => new List<Event>() as IReadOnlyCollection<Event>),
                new List<SchedulePeriod>());
        }

        private static IEnumerable<DateOnly> DaysRange(DateOnly from, DateOnly to)
        {
            while (from < to)
            {
                yield return from;
                from = from.AddDays(1);
            }
        }
    }
}