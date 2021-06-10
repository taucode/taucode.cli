using System;

namespace TauCode.Lab.Dev.Instruments.SolutionSerialization.SolutionItems
{
    internal class ReadSolutionProject : ReadPrincipalSolutionItem
    {
        internal ReadSolutionProject(
            Guid typeGuid,
            string name,
            string localProjectDefinitionFilePath,
            Guid guid)
            : base(typeGuid, name, guid)
        {
            this.LocalProjectDefinitionFilePath = 
                localProjectDefinitionFilePath
                ??
                throw new ArgumentNullException(nameof(localProjectDefinitionFilePath));
        }

        internal string LocalProjectDefinitionFilePath { get; }
    }
}
