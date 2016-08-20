using System;
using System.Collections.Generic;
using System.Linq;
using Photosphere.ServiceLocatorGeneration.Analysis.Metadata;
using Photosphere.ServiceLocatorGeneration.Extensions;
using static Photosphere.ServiceLocatorGeneration.Templates.ServiceLocatorTemplate;

namespace Photosphere.ServiceLocatorGeneration.Generation
{
    internal class ConstructorBodyGenerator
    {
        private const string DictionaryName = "_map";
        private readonly IReadOnlyCollection<string> _servicesTypes;
        private readonly IReadOnlyCollection<string> _parametersNames;
        private readonly VariablesGenerator _variablesGenerator;

        public ConstructorBodyGenerator(ServiceLocatorConfiguration configuration)
        {
            _servicesTypes = configuration.ServicesTypes.Select(t => t.Name).ToArray();
            _parametersNames = configuration.Parameters.Keys.Select(t => t.Name).ToArray();
            _variablesGenerator = new VariablesGenerator();
        }

        public string Generate(IReadOnlyCollection<ClassMetadata> metadatas)
        {
            var result = new List<string>();
            var alreadyActivated = new List<string>();
            alreadyActivated.AddRange(_parametersNames);
            foreach (var metadata in metadatas.Where(x => x.BaseTypesNames != null && _servicesTypes.Any(x.BaseTypesNames.Contains)))
            {
                var serviceName = _servicesTypes.First(x => metadata.BaseTypesNames.Contains(x));
                if (metadata.CtorParametersTypesNames != null)
                {
                    var readOnlyList = _variablesGenerator.Generate(
                        metadatas,metadata.ClassName, metadata.CtorParametersTypesNames, alreadyActivated);
                    result.AddRange(readOnlyList);
                }
                else
                {
                    var varName = metadata.ClassName.ToLowerCamelCase();
                    if (!alreadyActivated.Contains(varName))
                    {
                        var newInstanceStatement = NewInstanceStatement(metadata.ClassName, string.Empty);
                        var variableStatement = VariableStatement(varName, newInstanceStatement);
                        result.Add(variableStatement);

                        alreadyActivated.Add(varName);
                    }
                }
                var typeofExpression = TypeofExpression(serviceName);
                var addToDictionaryStatement = AddToDictionaryStatement(DictionaryName, typeofExpression, metadata.ClassName.ToLowerCamelCase());
                result.Add(addToDictionaryStatement);
            }
            return result.JoinByNewLineAndTabs(3);
        }
    }
}