using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TauCode.Lab.Dev.Data.SolutionItems
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public class SolutionProject : PrincipalSolutionItemBase
    {
        private readonly List<SolutionProjectConfigurationPlatform> _configurationPlatforms;

        public SolutionProject(
            Guid typeGuid,
            string name,
            Guid guid,
            string localProjectDefinitionFilePath)
            : base(typeGuid, name, guid)
        {
            this.LocalProjectDefinitionFilePath =
                localProjectDefinitionFilePath
                ??
                throw new ArgumentNullException(nameof(localProjectDefinitionFilePath));

            _configurationPlatforms = new List<SolutionProjectConfigurationPlatform>();
        }

        public Project Project { get; internal set; }

        public string LocalProjectDefinitionFilePath { get; }

        public IReadOnlyList<SolutionProjectConfigurationPlatform> ConfigurationPlatforms => _configurationPlatforms;

        public void AddConfigurationPlatform(
            string name,
            string suffix,
            string solutionConfigurationPlatform)
        {
            var solutionProjectConfigurationPlatform = new SolutionProjectConfigurationPlatform(
                name,
                suffix,
                solutionConfigurationPlatform);

            _configurationPlatforms.Add(solutionProjectConfigurationPlatform);
        }
    }
}
