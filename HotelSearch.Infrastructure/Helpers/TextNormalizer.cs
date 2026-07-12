using System.Globalization;
using System.Text;

namespace HotelSearch.Infrastructure.Helpers
{
    public static class TextNormalizer
    {
        // Strips diacritics from text ("Đakovo" -> "Dakovo") for more resilient string matching
        public static string RemoveDiacritics(string text)
        {
            var decomposed = text.Normalize(NormalizationForm.FormD);
            var result = new StringBuilder();

            foreach (var character in decomposed)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(character);

                if (category != UnicodeCategory.NonSpacingMark)
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }
    }
}
