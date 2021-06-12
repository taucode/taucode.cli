using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TauCode.Lab.Dev.Data.SolutionItems
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public class SolutionFolder : PrincipalSolutionItemBase
    {
        private readonly List<IPrincipalSolutionItem> _childPrincipalSolutionItems;
        private readonly List<string> _includedFileLocalPaths;

        public SolutionFolder(string name, Guid guid)
            : base(SolutionProjectType.SolutionFolder.Guid, name, guid)
        {
            _childPrincipalSolutionItems = new List<IPrincipalSolutionItem>();
            _includedFileLocalPaths = new List<string>();
        }

        public IReadOnlyList<IPrincipalSolutionItem> ChildPrincipalSolutionItems => _childPrincipalSolutionItems;

        public void AddIncludedFile(string localPath)
        {
            _includedFileLocalPaths.Add(localPath);
        }

        public IReadOnlyList<string> IncludedFileLocalPaths => _includedFileLocalPaths;

        internal void AddChild(IPrincipalSolutionItem principalSolutionItem)
        {
            _childPrincipalSolutionItems.Add(principalSolutionItem);
        }

        internal void RemoveChild(IPrincipalSolutionItem principalSolutionItem)
        {
            _childPrincipalSolutionItems.Remove(principalSolutionItem);
        }
    }
}
