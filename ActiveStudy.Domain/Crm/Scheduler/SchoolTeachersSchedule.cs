using System.Collections.Generic;

namespace ActiveStudy.Domain.Crm.Scheduler;

public class SchoolTeachersSchedule : Dictionary<TeacherShortInfo, TeacherSchedule>
{
    public EducationPeriod EducationPeriod { get; }

    public SchoolTeachersSchedule(EducationPeriod educationPeriod, Dictionary<TeacherShortInfo, TeacherSchedule> dictionary) : base(dictionary)
    {
        EducationPeriod = educationPeriod;
    }
}