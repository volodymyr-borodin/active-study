using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Components.LanguageSelect
{
    public class LanguageSelectModel
    {
        public LanguageSelectModel(string returnUrl, string selectedLanguage, IEnumerable<SelectListItem> availableCultures)
        {
            ReturnUrl = returnUrl;
            SelectedLanguage = selectedLanguage;
            AvailableCultures = availableCultures;
        }

        public string ReturnUrl { get; }
        public string SelectedLanguage { get; }
        public IEnumerable<SelectListItem> AvailableCultures { get; }
    }
}