using System;
using System.Collections.Generic;
using System.Diagnostics;
using TauCode.Lab.Dev.Data.SolutionItems;

// todo clean
namespace TauCode.Lab.Dev.Data
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public class Solution
    {
        private readonly List<IPrincipalSolutionItem> _principalSolutionItems;
        private readonly List<string> _configurationPlatforms;

        internal Solution(string name)
        {
            // todo checks

            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            _principalSolutionItems = new List<IPrincipalSolutionItem>();
            _configurationPlatforms = new List<string>();
        }

        public void AddPrincipalSolutionItem(
            IPrincipalSolutionItem principalSolutionItem,
            SolutionFolder parentSolutionFolder)
        {
            // todo: check not added already
            // todo: check 'parentSolutionFolder' is ours

            if (principalSolutionItem == null)
            {
                throw new ArgumentNullException(nameof(principalSolutionItem));
            }

            _principalSolutionItems.Add(principalSolutionItem);

            if (parentSolutionFolder != null)
            {
                this.MovePrincipalSolutionItem(principalSolutionItem, parentSolutionFolder);
            }
        }

        public void MovePrincipalSolutionItem(
            IPrincipalSolutionItem principalSolutionItem,
            SolutionFolder parentSolutionFolder)
        {
            // todo: check no child items with same name
            if (principalSolutionItem == null)
            {
                throw new ArgumentNullException(nameof(principalSolutionItem));
            }

            if (parentSolutionFolder == null)
            {
                throw new NotImplementedException(); // special case: move to solution root.
            }

            // 'principalSolutionItem' belongs to us?
            if (!_principalSolutionItems.Contains(principalSolutionItem))
            {
                throw new NotImplementedException();
            }

            // 'parentSolutionFolder' belongs to us?
            if (!_principalSolutionItems.Contains(parentSolutionFolder))
            {
                throw new NotImplementedException();
            }

            if (principalSolutionItem is SolutionFolder otherSolutionFolder)
            {
                if (parentSolutionFolder.IsNestedInto(otherSolutionFolder))
                {
                    throw new NotImplementedException(); // will get circular ref here
                }
            }

            var currentParent = principalSolutionItem.ParentSolutionFolder;
            if (currentParent != null)
            {
                currentParent.RemoveChild(principalSolutionItem);
            }

            parentSolutionFolder.AddChild(principalSolutionItem);
            ((PrincipalSolutionItemBase)principalSolutionItem).SetParent(parentSolutionFolder);
        }

        public string Name { get; }

        public string Directory { get; internal set; }

        public string FormatVersion { get; internal set; }

        public string VisualStudioVersion { get; internal set; }

        public string MinimumVisualStudioVersion { get; internal set; }

        public Guid? Guid { get; internal set; }

        public bool? HideSolutionNode { get; set; }

        public IReadOnlyList<IPrincipalSolutionItem> PrincipalSolutionItems => _principalSolutionItems;

        public IReadOnlyList<string> ConfigurationPlatforms => _configurationPlatforms;

        public void AddConfigurationPlatform(string configurationPlatform)
        {
            // todo: check strings correct; not existing already
            _configurationPlatforms.Add(configurationPlatform);
        }
    }
}
