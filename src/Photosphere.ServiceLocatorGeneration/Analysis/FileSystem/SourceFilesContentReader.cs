using System;
using System.Collections.Generic;
using System.IO;
using Photosphere.ServiceLocatorGeneration.Extensions;

namespace Photosphere.ServiceLocatorGeneration.FileSystem
{
    internal class SourceFilesContentReader
    {
        private readonly string _projectPath;
        private readonly string _extension;

        public SourceFilesContentReader(ServiceLocatorConfiguration configuration, string extension)
        {
            if (!extension.IsWord())
            {
                throw new ArgumentException($"String `{extension}` is not extension");
            }
            _projectPath = GetProjectPath(configuration.HostProvidedPath);
            _extension = extension;
        }

        public IReadOnlyCollection<string> Read()
        {
            var searchPattern = "*." + _extension;
            return Directory.GetFiles(_projectPath, searchPattern, SearchOption.AllDirectories);
        }

        private static string GetProjectPath(string hostProvidedPath) =>
            Path.GetFullPath(hostProvidedPath).Replace("\\", "\\\\");
    }
}