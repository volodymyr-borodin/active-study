using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Models.Classes
{
    public class CreateClassViewModel : CreateClassInputModel
    {
        public IEnumerable<SelectListItem> Grades => new[] { new SelectListItem(null, string.Empty) }.Concat(Enumerable.Range(1, 12).Select(i => new SelectListItem(i.ToString(), i.ToString())));
    }
}