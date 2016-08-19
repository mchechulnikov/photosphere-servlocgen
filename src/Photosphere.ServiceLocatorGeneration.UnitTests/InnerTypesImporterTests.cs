using System.Linq;
using Photosphere.ServiceLocatorGeneration.Parsing;
using Xunit;

namespace Photosphere.ServiceLocatorGeneration.UnitTests
{
    public class ParsingRegexTests
    {
        private const string TypeDefinitionExample = "public class Foo : FooBase, IFoo, IBar";

        [Fact]
        internal void ClassName_ValidString()
        {
            var value = ParsingRegex.ClassName.Match(TypeDefinitionExample).Value;
            var actualString = value.Substring(6);
            Assert.Contains("Foo", actualString);
        }

        [Fact]
        internal void BaseTypesRegexp()
        {
            var expected = new[] { "FooBase", "IFoo", "IBar" };
            var actual = ParsingRegex.BaseTypes
                    .Match(TypeDefinitionExample).Value
                    .TrimStart(':').TrimStart().Replace(",", string.Empty).Split(' ');
            foreach (var s in expected)
            {
                Assert.Contains(s, actual);
            }
        }

        [Fact]
        internal void CtorParametersTypesCtorRegexp()
        {
            const string str = @"
                public class Foo : FooBase, IFoo, IBar
                {
                  public Foo(
                    this IBuz buz,
                    IQiz qiz = null)
                  {
                  }
                }";
            var expected = new [] { "IBuz", "IQiz" };
            var result = ParsingRegex.GetCtorParameters("Foo")
                .Match(str).Value
                .Replace("Foo(", string.Empty)
                .Replace("this ", string.Empty)
                .Split(',')
                .Select(x => x.Trim())
                .Select(x => x.Split(' ').First());
            foreach (var s in expected)
            {
                Assert.Contains(s, result);
            }
        }

        [Fact]
        internal void CtorParametersTypesCtorRegexp_EmptyCtor()
        {
            const string str = @"
                public class Foo : FooBase, IFoo, IBar
                {
                  public Foo() {}
                }";
            var result = ParsingRegex.GetCtorParameters("Foo")
                .Match(str).Value
                .Replace("Foo(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("this ", string.Empty)
                .Split(',')
                .Select(x => x.Trim())
                .Select(x => x.Split(' ').First()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            Assert.Empty(result);
        }
    }
}