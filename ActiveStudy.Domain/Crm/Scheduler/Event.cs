using System;

namespace ActiveStudy.Domain.Crm.Scheduler
{
    public class Event
    {
        public Event(string id, string schoolId, string description, TeacherShortInfo teacher, Subject subject, ClassShortInfo @class, DateTime date, TimeSpan from, TimeSpan to)
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
        public DateTime Date { get; }
        public TimeSpan From { get; }
        public TimeSpan To { get; }
    }
}