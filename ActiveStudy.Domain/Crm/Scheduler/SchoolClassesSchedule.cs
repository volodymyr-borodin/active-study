using System.Collections.Generic;

namespace ActiveStudy.Domain.Crm.Scheduler;

public class SchoolClassesSchedule : Dictionary<ClassShortInfo, ClassSchedule>
{
    public EducationPeriod EducationPeriod { get; }

    public SchoolClassesSchedule(EducationPeriod educationPeriod, Dictionary<ClassShortInfo, ClassSchedule> dictionary) : base(dictionary)
    {
        EducationPeriod = educationPeriod;
    }
}