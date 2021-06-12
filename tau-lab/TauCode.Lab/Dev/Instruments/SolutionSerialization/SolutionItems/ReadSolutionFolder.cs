using System;
using System.Collections.Generic;
using TauCode.Lab.Dev.Data;

namespace TauCode.Lab.Dev.Instruments.SolutionSerialization.SolutionItems
{
    internal class ReadSolutionFolder : ReadPrincipalSolutionItem
    {
        private readonly List<string> _includedFileLocalPaths;

        internal ReadSolutionFolder(string name, Guid guid)
            : base(
                SolutionProjectType.SolutionFolder.Guid,
                name,
                guid)
        {
            _includedFileLocalPaths = new List<string>();
        }

        internal void AddIncludedFile(string localPath)
        {
            _includedFileLocalPaths.Add(localPath);
        }

        internal IReadOnlyList<string> IncludedFileLocalPaths => _includedFileLocalPaths;
    }
}
