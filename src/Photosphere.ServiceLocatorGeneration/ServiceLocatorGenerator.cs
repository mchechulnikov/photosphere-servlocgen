using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photosphere.ServiceLocatorGeneration.Extensions;
using Photosphere.ServiceLocatorGeneration.FileSystem;
using Photosphere.ServiceLocatorGeneration.Generation;
using Photosphere.ServiceLocatorGeneration.Metadata;
using Photosphere.ServiceLocatorGeneration.Parsing;
using Photosphere.ServiceLocatorGeneration.Templates;

namespace Photosphere.ServiceLocatorGeneration
{
    public class ServiceLocatorGenerator
    {
        private readonly string[] _dependencies;
        private readonly SourceFilesContentReader _sourceFilesContentReader;

        public ServiceLocatorGenerator(string hostProvidedPath, params string[] dependencies)
        {
            _dependencies = dependencies;
            _sourceFilesContentReader = new SourceFilesContentReader(hostProvidedPath, SourceFilesExtension.CSharp);
        }

        public string Generate()
        {
            var filesContents = _sourceFilesContentReader.Read();
            var classesMetadata = GetClassesMetadata(filesContents);
            var variablesGenerator = new VariablesGenerator(classesMetadata);

            var generateUsingsDirectives = GenerateUsingsDirectives(classesMetadata);
            return string.Format(
                TemplatesResource.ServiceLocator,
                generateUsingsDirectives,
                string.Empty,
                "IContainerConfiguration containerConfiguration",
                GenerateConstructor(classesMetadata, variablesGenerator)
            );
        }

        private static string GenerateUsingsDirectives(IEnumerable<ClassMetadata> classesMetadata) =>
            classesMetadata
                .Select(t => t.Namespace)
                .Where(n => n != null)
                .Distinct()
                .Select(ns => string.Format(TemplatesResource.UsingDirective, ns))
                .JoinByNewLine();

        private string GenerateConstructor(IEnumerable<ClassMetadata> classesMetadata, VariablesGenerator variablesGenerator)
        {
            var result = new List<string>();
            var alreadyActivated = new HashSet<string>
            {
                "containerConfiguration"
            };
            foreach (var type in classesMetadata.Where(x => x.BaseTypesNames != null && Contains(x.BaseTypesNames, _dependencies)))
            {
                var serviceName = _dependencies.First(x => type.BaseTypesNames.Contains(x));
                if (type.CtorParametersTypesNames != null)
                {
                    result.AddRange(variablesGenerator.Generate(type.ClassName, type.CtorParametersTypesNames, alreadyActivated));
                }
                else
                {
                    var varName = type.ClassName.ToLowerCamelCase();
                    if (!alreadyActivated.Contains(varName))
                    {
                        result.Add(string.Format(
                            TemplatesResource.VariableStatement,
                            varName,
                            string.Format(TemplatesResource.NewInstanceStatement, type.ClassName, string.Empty)
                        ));
                        alreadyActivated.Add(varName);                        
                    }
                }
                result.Add(string.Format(
                    TemplatesResource.AddToDictinaryStatement,
                    "_map",
                    string.Format(TemplatesResource.TypeofExpression, serviceName),
                    type.ClassName.ToLowerCamelCase()
                ));
            }
            return result.JoinByNewLineAndTabs(3);
        }

        private static IReadOnlyCollection<ClassMetadata> GetClassesMetadata(IEnumerable<string> filesContents) =>
            filesContents
                .Select(File.ReadAllText)
                .Select(CSharpFileParser.Parse)
                .Where(info => info != null).ToList();

        private static bool Contains(IEnumerable<string> source, IEnumerable<string> target) => target.Any(source.Contains);
    }
}