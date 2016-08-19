using System.Linq;
using Photosphere.ServiceLocatorGeneration.Metadata;

namespace Photosphere.ServiceLocatorGeneration.Parsing
{
    internal static class CSharpFileParser
    {
        public static ClassMetadata Parse(string content)
        {
            var className = GetClassName(content);
            if (className == null)
            {
                return null;
            }
            if (IsStaticClass(content))
            {
                return null;
            }
            if (IsAbstractClass(content))
            {
                return null;
            }
            var baseTypesNames = GetBaseTypesNames(content);
            if (baseTypesNames != null && baseTypesNames.Contains("Attribute"))
            {
                return null;
            }
            return new ClassMetadata(
                className,
                GetNamespace(content),
                baseTypesNames,
                GetCtorParametersTypes(content, className)
            );
        }

        private static string GetClassName(string content)
        {
            var value = ParsingRegex.ClassName.Match(content).Value;
            return value == string.Empty ? null : value.Substring(6);
        }

        private static bool IsStaticClass(string content)
        {
            var value = ParsingRegex.StaticClass.Match(content).Value;
            return value != string.Empty;
        }

        private static bool IsAbstractClass(string content)
        {
            var value = ParsingRegex.AbstractClass.Match(content).Value;
            return value != string.Empty;
        }

        private static string GetNamespace(string content)
        {
            var value = ParsingRegex.Namespace.Match(content).Value;
            return value == string.Empty ? null : value.Substring(10);
        }

        private static string[] GetBaseTypesNames(string content)
        {
            var value = ParsingRegex.BaseTypes.Match(content).Value;
            if (value == string.Empty)
            {
                return null;
            }
            var result = value.TrimStart(':').Trim().Replace(",", string.Empty);
            return !result.Contains(" ") ? new[] { result } : result.Split(' ');
        }

        private static string[] GetCtorParametersTypes(string content, string className)
        {
            var value = ParsingRegex.GetCtorParameters(className).Match(content).Value;
            if (value == string.Empty)
            {
                return null;
            }
            var result = value
                .Trim()
                .Replace(className + "(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("this ", string.Empty)
                .Split(',')
                .Select(x => x.Trim())
                .Select(x => x.Split(' ').First())
                .Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return result.Any() ? result : null;
        }
    }
}