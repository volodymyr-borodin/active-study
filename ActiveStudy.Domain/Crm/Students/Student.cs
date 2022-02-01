using System.Collections.Generic;

namespace ActiveStudy.Domain.Crm.Students
{
    public class Student
    {
        public string Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        
        public string Email { get; }
        public string Phone { get; }

        // Access related fields
        public string SchoolId { get; }
        public string UserId { get; }
        public IEnumerable<ClassShortInfo> Classes { get; }

        public string FullName => $"{FirstName} {LastName}";

        public Student(string id, string firstName, string lastName, string email, string phone, string schoolId, string userId, IEnumerable<ClassShortInfo> classes)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            SchoolId = schoolId;
            UserId = userId;
            Classes = classes;
        }
    }
}