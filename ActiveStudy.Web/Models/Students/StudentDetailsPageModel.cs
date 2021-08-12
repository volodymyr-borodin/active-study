using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Relatives;

namespace ActiveStudy.Web.Models.Students
{
    public class StudentDetailsPageModel
    {
        public StudentDetailsPageModel(string fullName, IEnumerable<Relative> relatives)
        {
            FullName = fullName;
            Relatives = relatives;
        }

        public string FullName { get; }
        public IEnumerable<Relative> Relatives { get; }
    }
}