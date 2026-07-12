using HotelSearch.Application.Interfaces;
using HotelSearch.Application.ValueObjects;
using HotelSearch.Domain.ValueObjects;
using HotelSearch.Infrastructure.Data;
using HotelSearch.Infrastructure.Helpers;
using System.Text.RegularExpressions;

namespace HotelSearch.Infrastructure.Parsers
{
    public class RegexHotelSearchPromptParser : IHotelSearchPromptParser
    {
        // "between 50 and 100"
        private static readonly Regex PriceRangePattern = new(@"\bbetween\s+(?:€|\$)?\s*(\d+)\s+and\s+(?:€|\$)?\s*(\d+)", RegexOptions.IgnoreCase);
        // "100-150"
        private static readonly Regex HyphenRangePattern = new(@"(?:€|\$)?\s*(\d+)\s*-\s*(?:€|\$)?\s*(\d+)", RegexOptions.IgnoreCase);
        // "under 100", "max price of 250", "up to 100"
        private static readonly Regex MaxPricePattern = new(@"\b(?:under|budget|max|below|up to)\b(?:\s+price)?(?:\s+of\s+around|\s+is|\s+of|\s+around)?\s+(?:€|\$)?\s*(\d+)", RegexOptions.IgnoreCase);
        // "over 75", "above 75", "at least 75"
        private static readonly Regex MinPricePattern = new(@"\b(?:over|above|at least)\b(?:\s+is|\s+of|\s+around)?\s+(?:€|\$)?\s*(\d+)", RegexOptions.IgnoreCase);

        public Task<HotelSearchParameters> ParseSearchPromptAsync(string prompt)
        {
            decimal? minPrice = null;
            decimal? maxPrice = null;
            GeoLocation? location = null;

            var rangeMatch = PriceRangePattern.Match(prompt);
            var hyphenRangeMatch = HyphenRangePattern.Match(prompt);

            if (rangeMatch.Success)
            {
                minPrice = decimal.Parse(rangeMatch.Groups[1].Value);
                maxPrice = decimal.Parse(rangeMatch.Groups[2].Value);
            }
            else if (hyphenRangeMatch.Success)
            {
                minPrice = decimal.Parse(hyphenRangeMatch.Groups[1].Value);
                maxPrice = decimal.Parse(hyphenRangeMatch.Groups[2].Value);
            }
            else
            {
                var maxPriceMatch = MaxPricePattern.Match(prompt);
                var minPriceMatch = MinPricePattern.Match(prompt);

                if (maxPriceMatch.Success)
                {
                    maxPrice = decimal.Parse(maxPriceMatch.Groups[1].Value);
                }

                if (minPriceMatch.Success)
                {
                    minPrice = decimal.Parse(minPriceMatch.Groups[1].Value);
                }
            }

            var normalizedPrompt = TextNormalizer.RemoveDiacritics(prompt);

            foreach (var cityName in KnownCityLocations.GetAllCityNames())
            {
                if (normalizedPrompt.Contains(cityName, StringComparison.OrdinalIgnoreCase))
                {
                    location = KnownCityLocations.GetCityLocation(cityName);

                    break;
                }
            }

            var parameters = new HotelSearchParameters(minPrice, maxPrice, location);

            return Task.FromResult(parameters);
        }
    }
}