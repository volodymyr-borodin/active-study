using System.Collections.Generic;

namespace ActiveStudy.Web.Components.SchoolsNav
{
    public class SchoolsNavModel
    {
        public SchoolsNavModel(IEnumerable<UserSchoolModel> schools)
        {
            Schools = schools;
        }

        public IEnumerable<UserSchoolModel> Schools { get; }
    }
}