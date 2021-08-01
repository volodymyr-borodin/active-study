using System.Collections.Generic;
using ActiveStudy.Domain.Crm.Schools;

namespace ActiveStudy.Web.Models.Schools
{
    public class SchoolListPageModel
    {
        public SchoolListPageModel(IEnumerable<School> schools)
        {
            Schools = schools;
        }

        public IEnumerable<School> Schools { get; }
    }
}