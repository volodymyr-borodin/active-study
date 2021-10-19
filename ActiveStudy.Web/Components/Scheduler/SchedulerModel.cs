using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Scheduler;

namespace ActiveStudy.Web.Components.Scheduler
{
    public class SchedulerModel
    {
        public Schedule Schedule { get; set; }
        public string SchoolId { get; set; }
        public string ClassId { get; set; }
        public string TeacherId { get; set; }
        public IEnumerable<Lesson> Lessons { get; set; }
    }
}