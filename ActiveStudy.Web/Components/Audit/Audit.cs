using System.Collections.Generic;
using System.Threading.Tasks;
using ActiveStudy.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ActiveStudy.Web.Components.Audit
{
    public class Audit : ViewComponent
    {   
        private readonly IAuditStorage auditStorage;

        public Audit(IAuditStorage auditStorage)
        {
            this.auditStorage = auditStorage;
        }

        public async Task<IViewComponentResult> InvokeAsync(Dictionary<string, string> filter)
        {
            var history = await auditStorage.SearchAnyAsync(filter);
            
            return View(history);
        }
    }
}