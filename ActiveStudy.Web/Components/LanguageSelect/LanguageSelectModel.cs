using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActiveStudy.Web.Components.LanguageSelect
{
    public class LanguageSelectModel
    {
        private readonly CultureInfo selectedLanguage;

        public LanguageSelectModel(string returnUrl, CultureInfo selectedLanguage, IEnumerable<SelectListItem> availableCultures)
        {
            this.selectedLanguage = selectedLanguage;
            ReturnUrl = returnUrl;
            AvailableCultures = availableCultures;
        }

        public string ReturnUrl { get; }
        public string SelectedLanguage => selectedLanguage.NativeName;

        public string SelectedLanguageIcon => selectedLanguage.Name switch
        {
            "uk-UA" => "ua.svg",
            _ => "us.svg"
        };
        public IEnumerable<SelectListItem> AvailableCultures { get; }
    }
}