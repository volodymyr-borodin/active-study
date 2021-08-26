using System;

namespace ActiveStudy.Web.Components.Scheduler
{
    public class Lesson
    {
        public Lesson(TimeSpan from, TimeSpan to)
        {
            From = from;
            To = to;
        }

        public TimeSpan From { get; }
        public TimeSpan To { get; }
    }
}