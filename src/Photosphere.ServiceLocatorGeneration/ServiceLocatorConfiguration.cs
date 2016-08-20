using System;
using System.Collections.Generic;

namespace Photosphere.ServiceLocatorGeneration
{
    public class ServiceLocatorConfiguration
    {
        public ServiceLocatorConfiguration()
        {
            ServiceLocatorPrefix = string.Empty;
            Parameters = new Dictionary<Type, string>();
            ServicesTypes = new List<Type>();
        }

        public string HostProvidedPath { get; set; }

        public string ServiceLocatorPrefix { get; set; }

        public IReadOnlyDictionary<Type, string> Parameters { get; set; }

        public IReadOnlyCollection<Type> ServicesTypes { get; set; }
    }
}