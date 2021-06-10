using System;
using System.IO;
using System.Linq;
using System.Text;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Dev.Data.SolutionItems;

namespace TauCode.Lab.Dev.Instruments.SolutionSerialization
{
    internal class SolutionWriter
    {
        #region Fields

        private TextWriter _textWriter;
        private Solution _solution;

        #endregion

        #region Public

        internal void WriteSolution(Solution solution, TextWriter textWriter)
        {
            // todo check args

            _textWriter = textWriter;
            _solution = solution;

            // empty line
            _textWriter.WriteLine();

            // format version
            _textWriter.Write("Microsoft Visual Studio Solution File, Format Version ");
            _textWriter.WriteLine(solution.FormatVersion);

            // comment
            _textWriter.WriteLine("# Visual Studio Version 16"); // todo: 16 is also should be read?

            // visual studio version
            _textWriter.Write("VisualStudioVersion = ");
            _textWriter.WriteLine(solution.VisualStudioVersion);

            // minimal visual studio version
            _textWriter.Write("MinimumVisualStudioVersion = ");
            _textWriter.WriteLine(solution.MinimumVisualStudioVersion);

            // projects (& solution folders)
            var principalSolutionItems = solution.PrincipalSolutionItems;

            foreach (var item in principalSolutionItems)
            {
                if (item is SolutionFolder solutionFolder)
                {
                    this.WriteSolutionFolder(solutionFolder);
                }
                else if (item is SolutionProject solutionProject)
                {
                    this.WriteSolutionProject(solutionProject);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            // global
            _textWriter.WriteLine("Global");

            this.WriteSolutionConfigurationPlatforms();
            this.WriteProjectConfigurationPlatforms();
            this.WriteSolutionProperties();
            this.WriteNestedProjects();
            this.WriteExtensibilityGlobals();

            _textWriter.WriteLine("EndGlobal");
        }

        internal void WriteSolution(Solution solution, string solutionDirectoryPath)
        {
            // todo check args
            var solutionFilePath = Path.Combine(solutionDirectoryPath, $"{solution.Name}.sln");

            using var streamWriter = new StreamWriter(solutionFilePath, false, Encoding.UTF8);
            this.WriteSolution(solution, streamWriter);
            streamWriter.Flush();
        }

        #endregion

        #region Private

        private void WriteSolutionFolder(SolutionFolder solutionFolder)
        {
            this.WritePrincipalItemBegin(solutionFolder, solutionFolder.Name);
            if (solutionFolder.IncludedFileLocalPaths.Count > 0)
            {
                _textWriter.WriteLine("\tProjectSection(SolutionItems) = preProject");

                foreach (var localPath in solutionFolder.IncludedFileLocalPaths)
                {
                    _textWriter.Write("\t\t");
                    _textWriter.WriteLine($"{localPath} = {localPath}");
                }

                _textWriter.WriteLine("\tEndProjectSection");
            }

            this.WritePrincipalItemEnd();
        }

        private void WriteSolutionProject(SolutionProject solutionProject)
        {
            this.WritePrincipalItemBegin(solutionProject, solutionProject.LocalProjectDefinitionFilePath);
            this.WritePrincipalItemEnd();
        }

        private void WritePrincipalItemBegin(IPrincipalSolutionItem item, string localPathSubstitution)
        {

            _textWriter.WriteLine(
                $"Project(\"{item.TypeGuid.ToSolutionString()}\") = \"{item.Name}\", \"{localPathSubstitution}\", \"{item.Guid.ToSolutionString()}\"");
        }

        private void WritePrincipalItemEnd()
        {
            _textWriter.WriteLine("EndProject");
        }

        private void WriteSolutionConfigurationPlatforms()
        {
            this.WriteSolutionGlobalSectionBegin("SolutionConfigurationPlatforms", true);

            foreach (var configurationPlatform in _solution.ConfigurationPlatforms)
            {
                _textWriter.Write("\t\t");
                _textWriter.Write(configurationPlatform);
                _textWriter.Write(" = ");
                _textWriter.Write(configurationPlatform);

                _textWriter.WriteLine();
            }

            this.WriteSolutionGlobalSectionEnd();
        }

        private void WriteProjectConfigurationPlatforms()
        {
            this.WriteSolutionGlobalSectionBegin("ProjectConfigurationPlatforms", false);

            var projects = _solution.GetSolutionProjects();

            foreach (var project in projects)
            {
                foreach (var p in project.ConfigurationPlatforms)
                {
                    _textWriter.WriteLine($"\t\t{project.Guid.ToSolutionString()}.{p.Name}.{p.Suffix} = {p.SolutionConfigurationPlatform}");
                }
            }

            this.WriteSolutionGlobalSectionEnd();
        }

        private void WriteSolutionProperties()
        {
            if (_solution.HideSolutionNode.HasValue)
            {
                this.WriteSolutionGlobalSectionBegin("SolutionProperties", true);

                _textWriter.Write("\t\t");
                _textWriter.Write("HideSolutionNode = ");
                _textWriter.Write(_solution.HideSolutionNode.Value.ToString().ToUpperInvariant());
                _textWriter.WriteLine();

                this.WriteSolutionGlobalSectionEnd();
            }
        }

        private void WriteNestedProjects()
        {
            var nestedItems = _solution.PrincipalSolutionItems
                .Where(x => x.ParentSolutionFolder != null)
                .ToList();

            if (nestedItems.Any())
            {
                this.WriteSolutionGlobalSectionBegin("NestedProjects", true);

                foreach (var nestedItem in nestedItems)
                {
                    _textWriter.Write("\t\t");
                    _textWriter.WriteLine(
                        $"{nestedItem.Guid.ToSolutionString()} = {nestedItem.ParentSolutionFolder.Guid.ToSolutionString()}");
                }

                this.WriteSolutionGlobalSectionEnd();
            }
        }

        private void WriteExtensibilityGlobals()
        {
            if (_solution.Guid.HasValue)
            {
                this.WriteSolutionGlobalSectionBegin("ExtensibilityGlobals", false);

                _textWriter.WriteLine($"\t\tSolutionGuid = {_solution.Guid.Value.ToSolutionString()}");

                this.WriteSolutionGlobalSectionEnd();
            }
        }

        private void WriteSolutionGlobalSectionBegin(string sectionName, bool preSolution)
        {
            var prePostSolution = preSolution ? "preSolution" : "postSolution";

            _textWriter.WriteLine($"\tGlobalSection({sectionName}) = {prePostSolution}");
        }

        private void WriteSolutionGlobalSectionEnd()
        {
            _textWriter.WriteLine("\tEndGlobalSection");
        }

        #endregion
    }
}
