using System;
using TauCode.Lab.Dev.Data.SolutionItems;

namespace TauCode.Lab.Dev.Data
{
    public abstract class PrincipalSolutionItemBase : IPrincipalSolutionItem
    {
        protected PrincipalSolutionItemBase(
            Guid typeGuid,
            string name,
            Guid guid)
        {
            this.TypeGuid = typeGuid; // todo: check
            this.Name = name ?? throw new ArgumentNullException(nameof(name)); // todo: more checks
            this.Guid = guid;
        }

        public Guid TypeGuid { get; }
        public string Name { get; }
        public Guid Guid { get; }
        public SolutionFolder ParentSolutionFolder { get; private set; }

        internal void SetParent(SolutionFolder newParent)
        {
            this.ParentSolutionFolder = newParent;
        }
    }
}
