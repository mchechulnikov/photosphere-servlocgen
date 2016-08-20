using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photosphere.ServiceLocatorGeneration.Analysis.Metadata;
using Photosphere.ServiceLocatorGeneration.FileSystem;
using Photosphere.ServiceLocatorGeneration.Generation;
using Photosphere.ServiceLocatorGeneration.Parsing;
using Photosphere.ServiceLocatorGeneration.Templates;

namespace Photosphere.ServiceLocatorGeneration
{
    public class ServiceLocatorGenerator
    {
        private readonly SourceFilesContentReader _sourceFilesContentReader;
        private readonly UsingDirectivesGenerator _usingDirectivesGenerator;
        private readonly ParametersGenerator _parametersGenerator;
        private readonly ConstructorBodyGenerator _constructorBodyGenerator;
        private readonly string _serviceLocatorPrefix;

        public ServiceLocatorGenerator(ServiceLocatorConfiguration configuration)
        {
            _sourceFilesContentReader = new SourceFilesContentReader(configuration, SourceFilesExtension.CSharp);
            _usingDirectivesGenerator = new UsingDirectivesGenerator();
            _constructorBodyGenerator = new ConstructorBodyGenerator(configuration);
            _parametersGenerator = new ParametersGenerator(configuration);
            _serviceLocatorPrefix = configuration.ServiceLocatorPrefix;
        }

        public string Generate()
        {
            var filesContents = _sourceFilesContentReader.Read();
            var metadatas = GetClassesMetadata(filesContents);

            return ServiceLocatorTemplate.ServiceLocator(
                _usingDirectivesGenerator.Generate(metadatas),
                _serviceLocatorPrefix,
                _parametersGenerator.Generate(),
                _constructorBodyGenerator.Generate(metadatas)
            );
        }

        private static IReadOnlyCollection<ClassMetadata> GetClassesMetadata(IEnumerable<string> filesContents)
            => filesContents
                .Select(File.ReadAllText)
                .Select(CSharpFileParser.Parse)
                .Where(info => info != null).ToList();
    }
}