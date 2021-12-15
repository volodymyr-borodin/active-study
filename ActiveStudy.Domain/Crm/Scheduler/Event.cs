using System;

namespace ActiveStudy.Domain.Crm.Scheduler
{
    public class Event
    {
        public Event(string id, string schoolId, string description, TeacherShortInfo teacher, Subject subject, ClassShortInfo @class, DateOnly date, TimeOnly from, TimeOnly to)
        {
            Id = id;
            SchoolId = schoolId;
            Description = description;
            Teacher = teacher;
            Subject = subject;
            Class = @class;
            Date = date;
            From = from;
            To = to;
        }

        public string Id { get; }
        public string SchoolId { get; }
        public string Description { get; }
        public TeacherShortInfo Teacher { get; }
        public Subject Subject { get; }
        public ClassShortInfo Class { get; }
        public DateOnly Date { get; }
        public TimeOnly From { get; }
        public TimeOnly To { get; }
    }
}