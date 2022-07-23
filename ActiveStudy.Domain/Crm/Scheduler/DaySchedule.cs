using System.Collections.Generic;

namespace ActiveStudy.Domain.Crm.Scheduler;

public class DaySchedule : Dictionary<int, ScheduleItem>
{
    public DaySchedule(Dictionary<int, ScheduleItem> dictionary) : base(dictionary)
    {
    }

    public static DaySchedule Init(IEnumerable<int> durations, Dictionary<int, ScheduleItem> dictionary)
    {
        var dict = new Dictionary<int, ScheduleItem>();

        foreach (var (dayOfWeek, daySchedule) in dictionary)
        {
            dict[dayOfWeek] = daySchedule;
        }

        return new DaySchedule(dict);
    }

    public static DaySchedule Empty => new DaySchedule(new Dictionary<int, ScheduleItem>());
}