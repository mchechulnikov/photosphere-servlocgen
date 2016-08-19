using System.Text.RegularExpressions;

namespace Photosphere.ServiceLocatorGeneration.Parsing
{
    internal static class ParsingRegex
    {
        public static readonly Regex ClassName = new Regex("class [a-zA-Z0-9]+");
        public static readonly Regex StaticClass = new Regex("static class");
        public static readonly Regex AbstractClass = new Regex("abstract class");
        public static readonly Regex Namespace = new Regex("namespace [a-zA-Z0-9.]+");
        public static readonly Regex BaseTypes = new Regex(":\\s*([a-zA-Z0-9]+,*\\s*)+");

        public static Regex GetCtorParameters(string className)
        {
            return new Regex(className + "\\s*\\((\\s*[(this) \\w=]+,*\\s*)+");
        }
    }
}