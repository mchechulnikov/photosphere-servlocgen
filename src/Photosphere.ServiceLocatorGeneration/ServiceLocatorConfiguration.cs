using System;
using System.Collections.Generic;

namespace Photosphere.ServiceLocatorGeneration
{
    public class ServiceLocatorConfiguration
    {
        public ServiceLocatorConfiguration()
        {
            ServiceLocatorPrefix = string.Empty;
            ParametersTypes = new List<Type>();
            ServicesTypes = new List<Type>();
        }

        public string HostProvidedPath { get; set; }

        public string ServiceLocatorPrefix { get; set; }

        public IReadOnlyCollection<Type> ParametersTypes { get; set; }

        public IReadOnlyCollection<Type> ServicesTypes { get; set; }
    }
}