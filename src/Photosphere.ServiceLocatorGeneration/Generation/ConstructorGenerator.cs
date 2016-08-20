using System;
using System.Collections.Generic;
using System.Linq;
using Photosphere.ServiceLocatorGeneration.Extensions;
using Photosphere.ServiceLocatorGeneration.Metadata;
using static Photosphere.ServiceLocatorGeneration.Templates.ServiceLocatorTemplate;

namespace Photosphere.ServiceLocatorGeneration.Generation
{
    internal class ConstructorGenerator
    {
        private readonly IReadOnlyCollection<string> _servicesTypes;
        private readonly IReadOnlyCollection<string> _parametersNames;
        private readonly VariablesGenerator _variablesGenerator;

        public ConstructorGenerator(IEnumerable<Type> servicesTypes, IEnumerable<Type> parameters)
        {
            _servicesTypes = servicesTypes.Select(t => t.Name).ToArray();
            _parametersNames = parameters.Select(t => t.Name).ToArray();
            _variablesGenerator = new VariablesGenerator();
        }

        public string Generate(IReadOnlyCollection<ClassMetadata> metadatas)
        {
            var result = new List<string>();
            var alreadyActivated = new List<string>();
            alreadyActivated.AddRange(_parametersNames);
            foreach (var type in metadatas.Where(x => x.BaseTypesNames != null && _servicesTypes.Any(x.BaseTypesNames.Contains)))
            {
                var serviceName = _servicesTypes.First(x => type.BaseTypesNames.Contains(x));
                if (type.CtorParametersTypesNames != null)
                {
                    var readOnlyList = _variablesGenerator.Generate(
                        metadatas,type.ClassName, type.CtorParametersTypesNames, alreadyActivated);
                    result.AddRange(readOnlyList);
                }
                else
                {
                    var varName = type.ClassName.ToLowerCamelCase();
                    if (!alreadyActivated.Contains(varName))
                    {
                        var newInstanceStatement = NewInstanceStatement(type.ClassName, string.Empty);
                        var variableStatement = VariableStatement(varName, newInstanceStatement);
                        result.Add(variableStatement);

                        alreadyActivated.Add(varName);
                    }
                }
                var typeofExpression = TypeofExpression(serviceName);
                var addToDictionaryStatement = AddToDictionaryStatement("_map", typeofExpression, type.ClassName.ToLowerCamelCase());
                result.Add(addToDictionaryStatement);
            }
            return result.JoinByNewLineAndTabs(3);
        }
    }
}