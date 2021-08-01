using System.Collections.Generic;
using System.Linq;
using ActiveStudy.Domain;

namespace ActiveStudy.Web.Models.Schools
{
    public class TeacherListModel
    {
        public TeacherListModel(string id, string fullName, string schoolId, IEnumerable<Subject> subjects)
        {
            Id = id;
            FullName = fullName;
            SchoolId = schoolId;
            Subjects = subjects;
        }

        public string Id { get; }
        public string FullName { get; }
        public string SchoolId { get; }
        public IEnumerable<Subject> Subjects { get; }

        public bool AnySubject => Subjects.Any();
    }
}