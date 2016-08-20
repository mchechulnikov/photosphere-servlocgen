using System.Collections.Generic;
using System.Linq;
using Photosphere.ServiceLocatorGeneration.Extensions;
using Photosphere.ServiceLocatorGeneration.Metadata;
using Photosphere.ServiceLocatorGeneration.Templates;

namespace Photosphere.ServiceLocatorGeneration.Generation
{
    internal class VariablesGenerator
    {
        private readonly IReadOnlyCollection<ClassMetadata> _classes;

        public VariablesGenerator(IReadOnlyCollection<ClassMetadata> classes)
        {
            _classes = classes;
        }

        public IReadOnlyList<string> Generate(
            string className, IReadOnlyCollection<string> parametersTypes, ISet<string> alreadyActivatedList)
        {
            var result = new List<string>();
            if (parametersTypes == null)
            {
                result.Add(string.Format(
                    TemplatesResource.VariableStatement,
                    className.ToLowerCamelCase(),
                    string.Format(TemplatesResource.NewInstanceStatement, className, string.Empty)
                ));
                return result;
            }

            var parametersList = new List<string>();
            var parameterClassMetadatas = GetParameterClassInfos(parametersTypes);
            foreach (var parameterClassMetadata in parameterClassMetadatas)
            {
                var varName = parameterClassMetadata.ClassName.ToLowerCamelCase();
                parametersList.Add(varName);

                if (alreadyActivatedList.Contains(varName))
                {
                    continue;
                }
                alreadyActivatedList.Add(varName);
                result.AddRange(Generate(
                    parameterClassMetadata.ClassName,
                    parameterClassMetadata.CtorParametersTypesNames?.ToArray(),
                    alreadyActivatedList
                ));
            }
            result.Add(string.Format(
                TemplatesResource.VariableStatement,
                className.ToLowerCamelCase(),
                string.Format(TemplatesResource.NewInstanceStatement, className, parametersList.JoinByCommaAndSpace())
            ));
            return result;
        }

        private IEnumerable<ClassMetadata> GetParameterClassInfos(IEnumerable<string> parametersTypes) =>
            parametersTypes
                .Select(pt => _classes.FirstOrDefault(x => x.BaseTypesNames != null && x.BaseTypesNames.Contains(pt)))
                .Where(x => x != null);
    }
}