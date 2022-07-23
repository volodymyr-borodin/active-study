using System;
using System.Collections.Generic;

namespace ActiveStudy.Domain.Crm.Scheduler;

public class TeacherSchedule : Dictionary<DayOfWeek, DaySchedule>
{
    public EducationPeriod EducationPeriod { get; }
    
    public TeacherSchedule(EducationPeriod educationPeriod, Dictionary<DayOfWeek, DaySchedule> dictionary) : base(dictionary)
    {
        EducationPeriod = educationPeriod;
    }

    public static TeacherSchedule Init(EducationPeriod educationPeriod, Dictionary<DayOfWeek, DaySchedule> dictionary)
    {
        var dict = new Dictionary<DayOfWeek, DaySchedule>
        {
            [DayOfWeek.Monday] = DaySchedule.Empty,
            [DayOfWeek.Tuesday] = DaySchedule.Empty,
            [DayOfWeek.Wednesday] = DaySchedule.Empty,
            [DayOfWeek.Thursday] = DaySchedule.Empty,
            [DayOfWeek.Friday] = DaySchedule.Empty,
            [DayOfWeek.Saturday] = DaySchedule.Empty,
            [DayOfWeek.Sunday] = DaySchedule.Empty
        };

        foreach (var (dayOfWeek, daySchedule) in dictionary)
        {
            dict[dayOfWeek] = daySchedule;
        }

        return new TeacherSchedule(educationPeriod, dict);
    }
}