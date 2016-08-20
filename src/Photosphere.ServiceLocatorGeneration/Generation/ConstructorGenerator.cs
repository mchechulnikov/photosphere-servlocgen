using System.Collections.Generic;
using System.Linq;
using Photosphere.ServiceLocatorGeneration.Extensions;
using Photosphere.ServiceLocatorGeneration.Metadata;
using static Photosphere.ServiceLocatorGeneration.Templates.ServiceLocatorTemplate;

namespace Photosphere.ServiceLocatorGeneration.Generation
{
    internal class ConstructorGenerator
    {
        private readonly VariablesGenerator _variablesGenerator;
        private readonly string[] _dependencies;

        public ConstructorGenerator(string[] dependencies)
        {
            _variablesGenerator = new VariablesGenerator();
            _dependencies = dependencies;
        }

        public string Generate(IReadOnlyCollection<ClassMetadata> metadatas)
        {
            var result = new List<string>();
            var alreadyActivated = new HashSet<string>
            {
                "containerConfiguration"
            };
            foreach (var type in metadatas.Where(x => x.BaseTypesNames != null && _dependencies.Any(x.BaseTypesNames.Contains)))
            {
                var serviceName = _dependencies.First(x => type.BaseTypesNames.Contains(x));
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