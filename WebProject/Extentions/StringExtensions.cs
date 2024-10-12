using System.Globalization;
using System.Text;

namespace WebProject.Extentions
{
    public static class StringExtensions
    {
        public static bool ContainsAccentSensitive(this string source, string toCheck)
        {
            return source.IndexOf(toCheck, StringComparison.Ordinal) >= 0;
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
