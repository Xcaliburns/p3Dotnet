using Microsoft.Extensions.Localization;
using System.Globalization;

namespace P3AddNewFunctionalityDotNetCore.Extensions
{
    public static class StringLocalizerExtensions
    {
        public static IStringLocalizer WithCulture(this IStringLocalizer localizer, CultureInfo culture)
        {
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            return localizer;
        }
    }
}
