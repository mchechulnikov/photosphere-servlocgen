using System.Collections.Generic;

namespace Photosphere.ServiceLocatorGeneration
{
    public class ServiceLocatorConfiguration
    {
        public ServiceLocatorConfiguration()
        {
            ServiceLocatorPrefix = string.Empty;
            Parameters = new Dictionary<string, string>();
            ServicesTypesNames = new List<string>();
        }

        public string HostProvidedPath { get; set; }

        public string ServiceLocatorPrefix { get; set; }

        public IReadOnlyDictionary<string, string> Parameters { get; set; }

        public IReadOnlyCollection<string> ServicesTypesNames { get; set; }
    }
}