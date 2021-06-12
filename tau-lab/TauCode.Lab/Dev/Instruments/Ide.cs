using System;
using System.IO;
using System.Linq;
using System.Text;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Dev.Data.SolutionItems;
using TauCode.Lab.Dev.Instruments.ProjectSerialization;
using TauCode.Lab.Dev.Instruments.SolutionSerialization;
using TauCode.Lab.Utility;

namespace TauCode.Lab.Dev.Instruments
{
    public class Ide
    {
        public Solution Solution { get; private set; }

        public void LoadSolution(string solutionFilePath)
        {
            var reader = new SolutionReader();
            this.Solution = reader.Read(solutionFilePath);

            this.Solution.LoadProjects();
        }

        public void SaveSolution()
        {
            // solution itself
            var writer = new SolutionWriter();
            writer.WriteSolution(this.Solution, this.Solution.Directory);

            var solutionProjects = this.Solution.GetSolutionProjects();
            var projectWriter = new ProjectWriter();

            foreach (var solutionProject in solutionProjects)
            {
                // todo: get rid of 'definition' substring.
                var projectFilePath = Path.Combine(
                    this.Solution.Directory,
                    solutionProject.LocalProjectDefinitionFilePath);

                //solutionProject.LocalProjectDefinitionFilePath;

                projectWriter.Write(solutionProject.Project, projectFilePath);
            }
        }

        public void CloseSolution()
        {
            this.Solution = null;
        }

        /// <summary>
        /// Relocates solution structure (sub-directories, *.sln, *.csproj) to new directory.
        /// You should not use this method, it is for internal use only.
        /// </summary>
        /// <param name="newSolutionDirectory">Destination solution directory</param>
        public void RelocateSolutionStructureTo(string newSolutionDirectory)
        {
            this.Solution.Directory = newSolutionDirectory;
            var solutionWriter = new SolutionWriter();
            solutionWriter.WriteSolution(this.Solution, this.Solution.Directory);

            var solutionFolders = this.Solution.GetSolutionFolders();

            foreach (var solutionFolder in solutionFolders)
            {
                foreach (var localPath in solutionFolder.IncludedFileLocalPaths)
                {
                    throw new NotImplementedException();
                }
            }

            var solutionProjects = this.Solution.GetSolutionProjects();
            var projectWriter = new ProjectWriter();

            foreach (var solutionProject in solutionProjects)
            {
                var projectFilePath = Path.Combine(
                    this.Solution.Directory,
                    solutionProject.LocalProjectDefinitionFilePath);

                FileUtility.CreateDirectoryForFile(projectFilePath);

                projectWriter.Write(solutionProject.Project, projectFilePath);
            }
        }

        public void CreateSolution(string name, string directory)
        {
            // todo checks

            var solution = new Solution(name)
            {
                FormatVersion = "12.00",
                VisualStudioVersion = "16.0.31205.134",
                MinimumVisualStudioVersion = "10.0.40219.1",
                Guid = Guid.NewGuid(),
                HideSolutionNode = false,
                Directory = directory,
            };

            solution.AddConfigurationPlatform("Debug|Any CPU");
            solution.AddConfigurationPlatform("Release|Any CPU");

            var solutionWriter = new SolutionWriter();
            solutionWriter.WriteSolution(solution, directory);

            this.Solution = solution;
        }

        public SolutionProject CreateProject(
            SolutionFolder parentSolutionFolder,
            string projectName,
            IdeProjectType projectType,
            string localDirectory)
        {
            if ((localDirectory?.Length ?? -1) == 0)
            {
                throw new ArgumentException(); // todo: localDirectory can be null, but not empty string.
            }

            var sb = new StringBuilder();
            if (localDirectory == null)
            {
                // ok.
            }
            else
            {
                sb.Append(localDirectory);
                sb.Append(@"\");
            }

            sb.Append(projectName);
            sb.Append(@"\");

            sb.Append(projectName);
            sb.Append(".csproj");

            var localProjectFilePath = sb.ToString();
            var absoluteProjectFilePath = Path.Combine(this.Solution.Directory, localProjectFilePath);

            var relativeProjectFilePath = Path.GetRelativePath(this.Solution.Directory, absoluteProjectFilePath);

            var project = new Project(projectName);
            project.PropertyGroups.Add(new Project.PropertyGroup());

            SolutionProjectType solutionProjectType;
            string sdk;
            string targetFramework;


            switch (projectType)
            {
                case IdeProjectType.Library:
                    solutionProjectType = SolutionProjectType.DotNetCore;
                    sdk = "Microsoft.NET.Sdk"; // todo const
                    targetFramework = "netstandard2.1"; // todo const everywhere in this file
                    break;

                case IdeProjectType.ConsoleApp:
                    solutionProjectType = SolutionProjectType.DotNetCore;
                    sdk = "Microsoft.NET.Sdk"; // todo const
                    targetFramework = "netcoreapp3.1"; // todo const everywhere in this file
                    break;

                case IdeProjectType.NUnitTestProject:
                    solutionProjectType = SolutionProjectType.DotNetCore;
                    sdk = "Microsoft.NET.Sdk"; // todo const
                    targetFramework = "netcoreapp3.1"; // todo const everywhere in this file
                    break;

                default:
                    throw new NotImplementedException();
            }

            project.Sdk = sdk;

            var propertyGroup = project.PropertyGroups.Single();
            propertyGroup.TargetFramework = targetFramework;


            var solutionProject = new SolutionProject(
                solutionProjectType.Guid,
                projectName,
                Guid.NewGuid(),
                relativeProjectFilePath);

            solutionProject.Project = project;

            this.Solution.AddPrincipalSolutionItem(solutionProject, parentSolutionFolder);

            foreach (var solutionConfigurationPlatform in this.Solution.ConfigurationPlatforms   )
            {
                solutionProject.AddConfigurationPlatform(solutionConfigurationPlatform, "ActiveCfg", solutionConfigurationPlatform);
                solutionProject.AddConfigurationPlatform(solutionConfigurationPlatform, "Build.0", solutionConfigurationPlatform);
            }

            var absoluteProjectFileDirectory = Path.GetDirectoryName(absoluteProjectFilePath);
            Directory.CreateDirectory(absoluteProjectFileDirectory);

            var projectWriter = new ProjectWriter();
            projectWriter.Write(project, absoluteProjectFilePath);

            return solutionProject;
        }

        public SolutionFolder CreateSolutionFolder(string folderName, SolutionFolder parentSolutionFolder = null)
        {
            // todo checks

            var solutionFolder = new SolutionFolder(folderName, Guid.NewGuid());
            this.Solution.AddPrincipalSolutionItem(solutionFolder, parentSolutionFolder);

            return solutionFolder;
        }

        public void CreateDirectoryInSolution(string localDirectory)
        {
            var directory = Path.Combine(this.Solution.Directory, localDirectory);
            Directory.CreateDirectory(directory);
        }

        // todo: should be extension? not much of work for ide here.
        public void CreateFileInSolution(string localPath, string content)
        {
            var fileFullPath = Path.Combine(this.Solution.Directory, localPath);
            var directoryFullPath = Path.GetDirectoryName(fileFullPath);
            Directory.CreateDirectory(directoryFullPath);

            File.WriteAllText(fileFullPath, content, Encoding.UTF8);
        }

        // todo: should be extension? not much of work for ide here.
        public void CreateFileInSolution(string localPath, IContentGenerator contentGenerator)
        {
            var fileFullPath = Path.Combine(this.Solution.Directory, localPath);
            var directoryFullPath = Path.GetDirectoryName(fileFullPath);
            Directory.CreateDirectory(directoryFullPath);

            var bytes = contentGenerator.GetContent();

            File.WriteAllBytes(fileFullPath, bytes);
        }

        // todo: should be extension? not much of work for ide here.
        public void AddSolutionItems(SolutionFolder solutionFolder, params string[] localPaths)
        {
            // todo checks; 'solutionFolder' cannot be null

            foreach (var localPath in localPaths )
            {
                solutionFolder.AddIncludedFile(localPath);
            }
        }
    }
}
