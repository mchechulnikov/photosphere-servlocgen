using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Photosphere.ServiceLocatorGeneration.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsWord(this string str) => new Regex("\\w").IsMatch(str);

        public static string ToLowerCamelCase(this string className) =>
            char.ToLowerInvariant(className[0]) + className.Substring(1);

        public static string JoinByCommaAndSpace(this IEnumerable<string> strs) => string.Join(", ", strs);

        public static string JoinByNewLine(this IEnumerable<string> strs) => string.Join("\n", strs);

        public static string JoinByNewLineAndTabs(this IEnumerable<string> strs, int tabsCount)
        {
            var separator = new StringBuilder("\r\n");

            var i = 0;
            while (i < tabsCount)
            {
                separator.Append("\t");
                i++;
            }
            return string.Join(separator.ToString(), strs);
        }
    }
}