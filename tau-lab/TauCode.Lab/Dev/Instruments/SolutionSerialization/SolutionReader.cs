using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TauCode.Extensions;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Dev.Data.SolutionItems;
using TauCode.Lab.Dev.Instruments.SolutionSerialization.SolutionItems;

namespace TauCode.Lab.Dev.Instruments.SolutionSerialization
{
    // todo clean
    // todo: principal solution items' guids are unique
    internal class SolutionReader
    {
        private SolutionReaderContext _context;

        internal Solution Read(string solutionFilePath)
        {
            // todo checks

            _context = new SolutionReaderContext(solutionFilePath);


            while (true)
            {
                if (_context.Index == _context.FileLines.Length)
                {
                    break;
                }

                bool read;

                read = this.TryReadEmpty();
                if (read)
                {
                    continue;
                }

                read = this.TryReadComment();
                if (read)
                {
                    continue;
                }

                read = this.TryReadFormatVersion();
                if (read)
                {
                    continue;
                }

                read = this.TryReadVisualStudioVersion();
                if (read)
                {
                    continue;
                }

                read = this.TryReadMinimumVisualStudioVersion();
                if (read)
                {
                    continue;
                }

                read = this.TryReadProject();
                if (read)
                {
                    continue;
                }

                read = this.TryReadGlobal();
                if (read)
                {
                    continue;
                }

                throw new NotImplementedException(); // failed to parse line
            }

            var solution = new Solution(_context.SolutionName/*, _context.SolutionFileDirectoryPath*/);

            // leading props
            solution.VisualStudioVersion = _context.VisualStudioVersion;
            solution.MinimumVisualStudioVersion = _context.MinimumVisualStudioVersion;
            solution.FormatVersion = _context.FormatVersion;
            solution.Directory = _context.SolutionFileDirectoryPath;

            foreach (var item in _context.ReadPrincipalSolutionItems)
            {
                if (item is ReadSolutionFolder folder)
                {
                    var solutionFolder = new SolutionFolder(folder.Name, folder.Guid);
                    solution.AddPrincipalSolutionItem(solutionFolder, null);

                    foreach (var includedFileLocalPath in folder.IncludedFileLocalPaths)
                    {
                        solutionFolder.AddIncludedFile(includedFileLocalPath);
                    }
                }
                else if (item is ReadSolutionProject project)
                {
                    var solutionProject = new SolutionProject(
                        project.TypeGuid,
                        project.Name,
                        project.Guid,
                        project.LocalProjectDefinitionFilePath);

                    solution.AddPrincipalSolutionItem(solutionProject, null);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            
            foreach (var solutionConfigurationPlatform in _context.SolutionConfigurationPlatforms)
            {
                solution.AddConfigurationPlatform(solutionConfigurationPlatform);
            }

            foreach (var pair in _context.ProjectConfigurationPlatforms)
            {
                var key = pair.Key;
                var value = pair.Value;

                var parts = key.Split('.');
                var projectGuid = Guid.Parse(parts[0]);
                var projectConfigurationPlatformName = parts[1];
                var projectConfigurationPlatformSuffix = parts[2];
                if (parts.Length == 4)
                {
                    projectConfigurationPlatformSuffix += $".{parts[3]}";
                }

                var solutionProject = solution.GetSolutionProjectByGuid(projectGuid);
                solutionProject.AddConfigurationPlatform(
                    projectConfigurationPlatformName,
                    projectConfigurationPlatformSuffix,
                    value);
            }

            // resolve nested
            foreach (var pair in _context.NestedProjects)
            {
                var key = pair.Key;
                var value = pair.Value;

                var nestedItem = solution.GetPrincipalSolutionItemByGuid(key);
                var parentSolutionFolder = solution.GetSolutionFolderByGuid(value);

                solution.MovePrincipalSolutionItem(nestedItem, parentSolutionFolder);
            }

            // solution properties
            foreach (var pair in _context.SolutionProperties)
            {
                var key = pair.Key;
                var value = pair.Value;

                if (key == "HideSolutionNode")
                {
                    solution.HideSolutionNode = bool.Parse(value);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            // extensibility globals
            foreach (var pair in _context.ExtensibilityGlobals)
            {
                var key = pair.Key;
                var value = pair.Value;

                if (key == "SolutionGuid")
                {
                    solution.Guid = Guid.Parse(value);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return solution;
        }

        private bool TryReadEmpty()
        {
            var line = _context.FileLines[_context.Index].Trim();
            if (line == string.Empty)
            {
                _context.Index++;
                return true;
            }

            return false;
        }

        private bool TryReadComment()
        {
            var line = _context.FileLines[_context.Index].Trim();
            if (line.StartsWith('#'))
            {
                _context.Index++;
                return true;
            }

            return false;
        }

        private bool TryReadFormatVersion()
        {
            const string begin = "Microsoft Visual Studio Solution File, Format Version ";
            var line = _context.FileLines[_context.Index].Trim();
            if (line.StartsWith(begin))
            {
                // got it
                this.CheckNoGlobalYet();

                if (_context.FormatVersion != null)
                {
                    throw new NotImplementedException();
                }

                _context.FormatVersion = line.Substring(begin.Length).Trim();
                _context.Index++;
                return true;
            }

            return false;
        }

        private bool TryReadVisualStudioVersion()
        {
            const string begin = "VisualStudioVersion = ";
            var line = _context.FileLines[_context.Index].Trim();
            if (line.StartsWith(begin))
            {
                this.CheckNoGlobalYet();
                // got it
                if (_context.VisualStudioVersion != null)
                {
                    throw new NotImplementedException();
                }

                _context.VisualStudioVersion = line.Substring(begin.Length).Trim();
                _context.Index++;
                return true;
            }

            return false;
        }

        private bool TryReadMinimumVisualStudioVersion()
        {
            const string begin = "MinimumVisualStudioVersion = ";
            var line = _context.FileLines[_context.Index].Trim();
            if (line.StartsWith(begin))
            {
                this.CheckNoGlobalYet();
                // got it
                if (_context.MinimumVisualStudioVersion != null)
                {
                    throw new NotImplementedException();
                }

                _context.MinimumVisualStudioVersion = line.Substring(begin.Length).Trim();
                _context.Index++;
                return true;
            }

            return false;
        }

        private bool TryReadProject()
        {
            const string projectBeginRegex =
                "Project" +
                "\\(\"\\{" +
                "([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})" + // project type guid
                "\\}\"\\)" +
                " = " +
                "\"([^\"]+)\"\\, " + // project name, comma, space
                "\"([^\"]+)\"\\, " + // project local path, comma, space
                "\\\"{" +
                "([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})" + // project guid
                "\\}\"" +
                "";

            var line = _context.FileLines[_context.Index];
            var match = Regex.Match(line, projectBeginRegex);
            if (match.Success)
            {
                this.CheckNoGlobalYet();

                // todo: ugly & not optimal.
                var shift = ((IList<string>)_context.FileLines.Skip(_context.Index).ToList()).FindFirstIndexOf("EndProject");

                if (shift < 0)
                {
                    throw new NotImplementedException(); // todo
                }

                var endIndex = _context.Index + shift;

                var typeGuid = Guid.Parse(match.Groups[1].Value);
                var name = match.Groups[2].Value;
                var localPath = match.Groups[3].Value; // todo: localPath must be equal to name; check it.
                var projectGuid = Guid.Parse(match.Groups[4].Value);

                if (typeGuid == SolutionProjectType.SolutionFolder.Guid)
                {
                    this.ReadSolutionFolder(
                        name,
                        projectGuid,
                        _context.Index,
                        endIndex);

                    _context.Index = endIndex + 1;
                }
                else if (typeGuid.IsProjectTypeGuid() && endIndex == _context.Index + 1)
                {
                    var project = new ReadSolutionProject(typeGuid, name, localPath, projectGuid);
                    _context.ReadPrincipalSolutionItems.Add(project);

                    _context.Index = endIndex + 1;
                }
                else
                {
                    throw new NotImplementedException();
                }

                return true;
            }

            return false;
        }

        private void ReadSolutionFolder(
            string name,
            Guid projectGuid,
            int hintStartIndex,
            int hintEndIndex)
        {
            var folder = new ReadSolutionFolder(name, projectGuid);
            _context.ReadPrincipalSolutionItems.Add(folder);

            if (hintEndIndex == hintStartIndex + 1)
            {
                // folder with no files
                return;
            }

            var ok =
                _context.FileLines[hintStartIndex + 1] == "\tProjectSection(SolutionItems) = preProject" &&
                _context.FileLines[hintEndIndex - 1] == "\tEndProjectSection" &&
                true;

            if (!ok)
            {
                throw new NotImplementedException();
            }

            for (var index = hintStartIndex + 2; index <= hintEndIndex - 2; index++)
            {
                var line = _context.FileLines[index].Trim();
                var parts = line.Split('=').Select(x => x.Trim()).ToList();

                if (parts.Count != 2)
                {
                    throw new NotImplementedException();
                }

                if (parts[0] != parts[1])
                {
                    throw new NotImplementedException();
                }

                folder.AddIncludedFile(parts[0]);
            }
        }

        private bool TryReadGlobal()
        {
            var line = _context.FileLines[_context.Index];

            if (line == "Global")
            {
                this.CheckNoGlobalYet();

                _context.Index++;
                var gotEndGlobal = false;

                while (true)
                {
                    if (_context.Index == _context.FileLines.Length)
                    {
                        break;
                    }

                    bool read;

                    read = this.TryReadEmpty();
                    if (read)
                    {
                        continue;
                    }

                    read = this.TryReadComment();
                    if (read)
                    {
                        continue;
                    }

                    if (_context.FileLines[_context.Index] == "EndGlobal")
                    {
                        gotEndGlobal = true;
                        _context.Index++;
                        break;
                    }

                    read = this.TryReadSolutionConfigurationPlatformsGlobalSection();
                    if (read)
                    {
                        continue;
                    }

                    read = this.TryReadProjectConfigurationPlatformsGlobalSection();
                    if (read)
                    {
                        continue;
                    }

                    read = this.TryReadSolutionPropertiesGlobalSection();
                    if (read)
                    {
                        continue;
                    }

                    read = this.TryReadNestedProjectsGlobalSection();
                    if (read)
                    {
                        continue;
                    }

                    read = this.TryReadExtensibilityGlobalsGlobalSection();
                    if (read)
                    {
                        continue;
                    }
                }

                if (!gotEndGlobal)
                {
                    throw new NotImplementedException();
                }

                _context.GotGlobal = true;
                return true;
            }

            return false;
        }

        private bool TryReadSolutionConfigurationPlatformsGlobalSection()
        {
            const string globalSectionName = "SolutionConfigurationPlatforms";
            var dict = this.TryExtractGlobalSectionProperties(globalSectionName);

            if (dict == null)
            {
                return false;
            }

            if (_context.GotGlobalSections.Contains(globalSectionName))
            {
                throw new NotImplementedException();
            }

            foreach (var pair in dict)
            {
                var key = pair.Key;
                var value = pair.Value;

                if (key != value)
                {
                    throw new NotImplementedException();
                }

                _context.SolutionConfigurationPlatforms.Add(key);
            }

            _context.GotGlobalSections.Add(globalSectionName);
            return true;
        }

        private bool TryReadProjectConfigurationPlatformsGlobalSection()
        {
            const string globalSectionName = "ProjectConfigurationPlatforms";
            var dict = this.TryExtractGlobalSectionProperties(globalSectionName);

            if (dict == null)
            {
                return false;
            }

            if (_context.GotGlobalSections.Contains(globalSectionName))
            {
                throw new NotImplementedException();
            }

            foreach (var pair in dict)
            {
                _context.ProjectConfigurationPlatforms.Add(pair.Key, pair.Value);
            }

            _context.GotGlobalSections.Add(globalSectionName);
            return true;
        }

        private bool TryReadSolutionPropertiesGlobalSection()
        {
            const string globalSectionName = "SolutionProperties";
            var dict = this.TryExtractGlobalSectionProperties(globalSectionName);

            if (dict == null)
            {
                return false;
            }

            if (_context.GotGlobalSections.Contains(globalSectionName))
            {
                throw new NotImplementedException();
            }

            foreach (var pair in dict)
            {
                _context.SolutionProperties.Add(pair.Key, pair.Value);
            }

            _context.GotGlobalSections.Add(globalSectionName);
            return true;
        }

        private bool TryReadNestedProjectsGlobalSection()
        {
            const string globalSectionName = "NestedProjects";
            var dict = this.TryExtractGlobalSectionProperties(globalSectionName);

            if (dict == null)
            {
                return false;
            }

            if (_context.GotGlobalSections.Contains(globalSectionName))
            {
                throw new NotImplementedException();
            }

            foreach (var pair in dict)
            {
                var nestedProjectGuid = Guid.Parse(pair.Key);
                var holdingSolutionFolderGuid = Guid.Parse(pair.Value);

                _context.NestedProjects.Add(nestedProjectGuid, holdingSolutionFolderGuid);
            }

            _context.GotGlobalSections.Add(globalSectionName);
            return true;
        }

        private bool TryReadExtensibilityGlobalsGlobalSection()
        {
            const string globalSectionName = "ExtensibilityGlobals";
            var dict = this.TryExtractGlobalSectionProperties(globalSectionName);

            if (dict == null)
            {
                return false;
            }

            if (_context.GotGlobalSections.Contains(globalSectionName))
            {
                throw new NotImplementedException();
            }

            foreach (var pair in dict)
            {
                _context.ExtensibilityGlobals.Add(pair.Key, pair.Value);
            }

            _context.GotGlobalSections.Add(globalSectionName);
            return true;
        }

        private void CheckNoGlobalYet()
        {
            if (_context.GotGlobal)
            {
                throw new NotImplementedException();
            }
        }

        private void CheckNoGlobalSectionYet(string globalSectionName)
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, string> TryExtractGlobalSectionProperties(string globalSectionName)
        {
            var line = _context.FileLines[_context.Index].Trim();
            var pattern = $"GlobalSection\\({globalSectionName}\\) = (preSolution|postSolution)";

            var match = Regex.Match(line, pattern);

            if (match.Success)
            {
                var shift = ((IList<string>)_context.FileLines.Skip(_context.Index).ToList()).FindFirstIndexOf(x =>
                   x.Trim() == "EndGlobalSection");
                if (shift < 0)
                {
                    throw new NotImplementedException();
                }

                var endIndex = _context.Index + shift;

                var dict = new Dictionary<string, string>();

                for (var index = _context.Index + 1; index <= endIndex - 1; index++)
                {
                    var assignment = _context.FileLines[index].Trim();
                    var pair = assignment.Split('=').Select(x => x.Trim()).ToList();

                    if (
                        pair.Count != 2 ||
                        string.IsNullOrEmpty(pair[0]) ||
                        string.IsNullOrEmpty(pair[1]) ||
                        false
                        )
                    {
                        throw new NotImplementedException();
                    }

                    dict.Add(pair[0], pair[1]);
                }

                _context.Index = endIndex + 1;

                return dict;
            }

            return null;
        }
    }
}
