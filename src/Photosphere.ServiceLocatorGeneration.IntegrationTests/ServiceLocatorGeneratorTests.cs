using Xunit;

namespace Photosphere.ServiceLocatorGeneration.IntegrationTests
{
    public class ServiceLocatorGeneratorTests
    {
        private const string TestAssemblyPath = @"..\..\..\Photosphere.ServiceLocatorGeneration.TestAssembly";

        [Fact]
        internal void Generate_ValidSourcesFiles_NotNull()
        {
            var generator = new ServiceLocatorGenerator(TestAssemblyPath, "IFoo", "IBar");
            var result = generator.Generate();
            Assert.NotNull(result);
        }

        [Fact]
        internal void Generate_ValidSourcesFiles_ValidUsingDirectves()
        {
            var generator = new ServiceLocatorGenerator(TestAssemblyPath, "IFoo", "IBar");
            var result = generator.Generate();
            var expected = new[]
            {
                "using Photosphere.ServiceLocatorGeneration.TestAssembly;",
                "using Photosphere.ServiceLocatorGeneration.TestAssembly.Bars;"
            };
            foreach (var expectedSubstring in expected)
            {
                Assert.Contains(expectedSubstring, result);
            }
        }

        [Fact]
        internal void Generate_ValidSourcesFiles_ValidVarsStatements()
        {
            var generator = new ServiceLocatorGenerator(TestAssemblyPath, "IFoo", "IBar");
            var result = generator.Generate();
            var expected = new[]
            {
                "var foo = new Foo();",
                "var buz = new Buz();",
                "var bar = new Bar(buz);"
            };
            foreach (var expectedSubstring in expected)
            {
                Assert.Contains(expectedSubstring, result);
            }
        }

        [Fact]
        internal void Generate_ValidSourcesFiles_ValidAddToDictionaryStatements()
        {
            var generator = new ServiceLocatorGenerator(TestAssemblyPath, "IFoo", "IBar");
            var result = generator.Generate();
            var expected = new[]
            {
                ".Add(typeof (IFoo), foo)",
                ".Add(typeof (IBar), bar)"
            };
            foreach (var expectedSubstring in expected)
            {
                Assert.Contains(expectedSubstring, result);
            }
        }
    }
}