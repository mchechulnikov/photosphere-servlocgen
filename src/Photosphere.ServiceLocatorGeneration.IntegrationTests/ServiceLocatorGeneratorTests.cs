using Photosphere.ServiceLocatorGeneration.TestAssembly;
using Photosphere.ServiceLocatorGeneration.TestAssembly.Bars;
using Xunit;

namespace Photosphere.ServiceLocatorGeneration.IntegrationTests
{
    public class ServiceLocatorGeneratorTests
    {
        private const string TestAssemblyPath = @"..\..\..\Photosphere.ServiceLocatorGeneration.TestAssembly";

        private static ServiceLocatorGenerator NewServiceLocator
            => new ServiceLocatorGenerator(new ServiceLocatorConfiguration
            {
                HostProvidedPath = TestAssemblyPath,
                ServicesTypes = new[] { typeof(IFoo), typeof(IBar) }
            });

        [Fact]
        internal void Generate_ValidSourcesFiles_NotNull()
        {
            var result = NewServiceLocator.Generate();
            Assert.NotNull(result);
        }

        [Fact]
        internal void Generate_ValidSourcesFiles_ValidUsingDirectves()
        {
            var result = NewServiceLocator.Generate();
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
            var result = NewServiceLocator.Generate();
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
            var result = NewServiceLocator.Generate();
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