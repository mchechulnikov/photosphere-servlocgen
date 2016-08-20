using System.Collections.Generic;
using System.Linq;
using Photosphere.ServiceLocatorGeneration.Analysis.Metadata;
using Photosphere.ServiceLocatorGeneration.Extensions;
using Photosphere.ServiceLocatorGeneration.Templates;

namespace Photosphere.ServiceLocatorGeneration.Generation
{
    internal class UsingDirectivesGenerator
    {
        public string Generate(IReadOnlyCollection<ClassMetadata> classesMetadata) =>
            classesMetadata
                .Select(t => t.Namespace)
                .Where(n => n != null)
                .Distinct()
                .Select(ServiceLocatorTemplate.UsingDirective)
                .JoinByNewLine();
    }
}