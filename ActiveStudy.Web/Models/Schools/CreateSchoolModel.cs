using System.Collections.Generic;
using ActiveStudy.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Models.Schools
{
    public class CreateSchoolModel : CreateSchoolInputModel
    {
        public CreateSchoolModel(IEnumerable<Country> countries)
        {
            Countries = new SelectList(countries, "Code", "Name");
        }

        public SelectList Countries { get; }
    }
}
