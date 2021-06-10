using System;

namespace TauCode.Lab.Dev.Instruments.SolutionSerialization.SolutionItems
{
    internal abstract class ReadPrincipalSolutionItem
    {
        protected ReadPrincipalSolutionItem(
            Guid typeGuid,
            string name,
            Guid guid)
        {
            this.TypeGuid = typeGuid;
            this.Name = name;
            this.Guid = guid;
        }

        internal Guid TypeGuid { get; }

        internal string Name { get; }

        internal Guid Guid { get; }
    }
}
