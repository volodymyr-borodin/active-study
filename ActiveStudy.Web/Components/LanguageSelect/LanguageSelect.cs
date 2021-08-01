using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace ActiveStudy.Web.Components.LanguageSelect
{
    public class LanguageSelect : ViewComponent
    {
        private readonly IRequestCultureFeature _requestCultureFeature;
        private readonly RequestLocalizationOptions _localizationOptions;
        private readonly string _returnUrl;
        
        public LanguageSelect(IHttpContextAccessor contextAccessor,
            IOptions<RequestLocalizationOptions> localizationOptions)
        {
            var context = contextAccessor.HttpContext;
            
            _requestCultureFeature = context.Features.Get<IRequestCultureFeature>();
            _localizationOptions = localizationOptions.Value;
            _returnUrl = string.IsNullOrEmpty(context.Request.Path) ? "~/" : $"~{context.Request.Path.Value}{context.Request.QueryString}";
        }

        public IViewComponentResult Invoke()
        {
            var requestCulture = _requestCultureFeature?.RequestCulture?.UICulture;
            var cultures = _localizationOptions.SupportedUICultures
                .Select(c => new SelectListItem(c.NativeName, c.Name, c.Name == requestCulture.Name))
                .ToList();
            
            return View(new LanguageSelectModel(_returnUrl, requestCulture.NativeName, cultures));
        }
    }
}