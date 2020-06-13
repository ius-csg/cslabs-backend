using System.Text.RegularExpressions;

namespace CSLabs.Api.Util
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
        
        public static string ToSafeId(this string str)
        {
            return Regex.Replace(str, @"[^a-zA-Z0-9 -]|\s+", "");
        }
    }
}