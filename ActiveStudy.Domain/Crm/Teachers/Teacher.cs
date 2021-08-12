using System.Collections.Generic;

namespace ActiveStudy.Domain.Crm.Teachers
{
    public class Teacher
    {
        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public string Email { get; }
        public IEnumerable<Subject> Subjects { get; }

        public string SchoolId { get; }
        public string UserId { get; }

        public string FullName => $"{FirstName} {LastName}";

        public Teacher(string id, string firstName, string lastName, string email, IEnumerable<Subject> subjects, string schoolId, string userId)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;

            Email = email;
            Subjects = subjects;

            SchoolId = schoolId;
            UserId = userId;
        }

        public static explicit operator TeacherShortInfo(Teacher teacher)
        {
            if (teacher == null)
            {
                return null;
            }
            
            return new TeacherShortInfo(teacher.Id, teacher.FullName, teacher.UserId);
        }
    }
}