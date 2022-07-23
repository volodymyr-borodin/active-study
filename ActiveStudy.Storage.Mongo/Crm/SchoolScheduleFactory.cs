using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveStudy.Domain.Crm;
using ActiveStudy.Domain.Crm.Scheduler;

namespace ActiveStudy.Storage.Mongo.Crm;

public class SchoolScheduleFactory
{
    private readonly ClassStorage classStorage;
    
    public SchoolScheduleFactory(CrmContext context)
    {
        classStorage = new ClassStorage(context);
    }

    public async Task<SchoolClassesSchedule> BuildAsync(
        EducationPeriod educationPeriod,
        Dictionary<ClassShortInfo, ClassSchedule> dictionary)
    {
        var classes = await classStorage.FindAsync(educationPeriod.SchoolId);
        foreach (var @class in classes)
        {
            var c = (ClassShortInfo) @class;
            if (!dictionary.ContainsKey(c))
            {
                dictionary[c] = ClassSchedule.Init(educationPeriod, new Dictionary<DayOfWeek, DaySchedule>());
            }
        }
        
        return new SchoolClassesSchedule(educationPeriod, dictionary);
    }
}