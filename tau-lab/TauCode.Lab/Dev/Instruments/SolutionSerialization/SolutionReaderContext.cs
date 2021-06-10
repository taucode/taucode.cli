using System;
using System.Collections.Generic;
using System.IO;
using TauCode.Lab.Dev.Instruments.SolutionSerialization.SolutionItems;

namespace TauCode.Lab.Dev.Instruments.SolutionSerialization
{
    internal class SolutionReaderContext
    {
        internal SolutionReaderContext(string solutionFilePath)
        {
            this.SolutionFilePath = solutionFilePath;
            this.SolutionFileDirectoryPath = Path.GetDirectoryName(this.SolutionFilePath);
            this.SolutionFileName = Path.GetFileName(this.SolutionFilePath);
            this.SolutionName = Path.GetFileNameWithoutExtension(this.SolutionFilePath);

            this.ReadPrincipalSolutionItems = new List<ReadPrincipalSolutionItem>();

            this.GotGlobalSections = new HashSet<string>();
            this.SolutionConfigurationPlatforms = new List<string>();
            this.ProjectConfigurationPlatforms = new Dictionary<string, string>();
            this.SolutionProperties = new Dictionary<string, string>();
            this.NestedProjects = new Dictionary<Guid, Guid>();
            this.ExtensibilityGlobals = new Dictionary<string, string>();

            this.FileLines = File.ReadAllLines(this.SolutionFilePath);
            this.Index = 0;
        }

        internal string SolutionFilePath;
        internal string SolutionFileDirectoryPath;
        internal string SolutionFileName;

        internal string VisualStudioVersion;
        internal string MinimumVisualStudioVersion;
        internal string FormatVersion;

        internal string SolutionName;

        internal string[] FileLines;
        internal int Index;

        internal bool GotGlobal;
        internal HashSet<string> GotGlobalSections;

        internal List<ReadPrincipalSolutionItem> ReadPrincipalSolutionItems;

        internal List<string> SolutionConfigurationPlatforms;
        internal Dictionary<string, string> ProjectConfigurationPlatforms;
        internal Dictionary<string, string> SolutionProperties;

        /// <summary>
        /// keys is 'which project is nested', value is 'solution folder the project is nested into'
        /// </summary>
        internal Dictionary<Guid, Guid> NestedProjects;
        internal Dictionary<string, string> ExtensibilityGlobals;

    }
}
