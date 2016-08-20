using System.Collections.Generic;
using System.Linq;
using Photosphere.ServiceLocatorGeneration.Extensions;
using Photosphere.ServiceLocatorGeneration.Metadata;
using static Photosphere.ServiceLocatorGeneration.Templates.ServiceLocatorTemplate;

namespace Photosphere.ServiceLocatorGeneration.Generation
{
    internal class VariablesGenerator
    {
        public IReadOnlyList<string> Generate(
            IReadOnlyCollection<ClassMetadata> metadatas,
            string className,
            IReadOnlyCollection<string> parametersTypes,
            ISet<string> alreadyActivatedList)
        {
            var result = new List<string>();
            if (parametersTypes == null)
            {
                result.Add(VariableStatement(
                    className.ToLowerCamelCase(),
                    NewInstanceStatement(className, string.Empty)
                ));
                return result;
            }

            var parametersList = new List<string>();
            var parameterClassMetadatas = GetParameterClassInfos(metadatas, parametersTypes);
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
                    metadatas,
                    parameterClassMetadata.ClassName,
                    parameterClassMetadata.CtorParametersTypesNames?.ToArray(),
                    alreadyActivatedList
                ));
            }
            result.Add(VariableStatement(
                className.ToLowerCamelCase(),
                NewInstanceStatement(className, parametersList.JoinByCommaAndSpace())
            ));
            return result;
        }

        private static IEnumerable<ClassMetadata> GetParameterClassInfos(
            IReadOnlyCollection<ClassMetadata> metadatas, IEnumerable<string> parametersTypes) =>
            parametersTypes
                .Select(pt => metadatas.FirstOrDefault(x => x.BaseTypesNames != null && x.BaseTypesNames.Contains(pt)))
                .Where(x => x != null);
    }
}