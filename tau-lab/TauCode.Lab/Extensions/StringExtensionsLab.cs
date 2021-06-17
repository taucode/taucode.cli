using System.Text.RegularExpressions;

namespace TauCode.Lab.Extensions
{
    public static class StringExtensionsLab
    {
        public static bool IsInt32(this string s)
        {
            // todo checks
            return int.TryParse(s, out var dummy);
        }

        public static bool IsRegexMatch(this string s, string pattern)
        {
            // todo checks
            return Regex.IsMatch(s, pattern);
        }
    }
}
