using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using TauCode.Extensions;
using TauCode.Lab.Dev;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Dev.Instruments;
using TauCode.Lab.Extensions;
using TauCode.Lab.Utility;
using TauCode.Lab.Utility.Rho;
using TauCode.Lab.Xml;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Tools
{
    public class SolutionChecker
    {
        public SolutionChecker(string solutionDirectoryPath)
        {
            // todo check args

            this.SolutionDirectoryPath = solutionDirectoryPath;

            if (!Directory.Exists(this.SolutionDirectoryPath))
            {
                throw new NotImplementedException();
            }

            this.SolutionDirectoryInfo = new DirectoryInfo(this.SolutionDirectoryPath);
            this.SolutionFileInfo = this.SolutionDirectoryInfo.GetFiles("*.sln").Single();

            var ide = new Ide();
            ide.LoadSolution(this.SolutionFileInfo.FullName);
            this.Solution = ide.Solution;
        }

        public string SolutionDirectoryPath { get; }
        public DirectoryInfo SolutionDirectoryInfo { get; }
        public FileInfo SolutionFileInfo { get; }
        public Solution Solution { get; }

        public bool IsDev { get; set; }

        public void Prepare()
        {
            throw new NotImplementedException();
        }

        public void CheckFileSystem()
        {
            var checks = new FileSystemObjectCheck[]
            {
                new DirectoryCheck(@"assets", false),
                new DirectoryCheck(@"build", true),
                new DirectoryCheck(@"doc", false),
                new DirectoryCheck(@"misc", true),
                new DirectoryCheck(@"nuget", true),

                new DirectoryCheck(@"src", true),
                new DirectoryCheck($@"src\{this.Solution.Name}", true),

                new DirectoryCheck(@"test", true),
                new DirectoryCheck($@"test\{this.Solution.Name}.Tests", true),

                new DirectoryCheck(@"tools", false),

                new FileCheck(@".gitignore", true),
                new FileCheck(@"LICENSE.txt", true, this.GetByteContent("LICENSE.txt")),
                new FileCheck(
                    @"nuget.config",
                    true,
                    this.IsDev ? this.GetByteContent("nuget.config.dev.xml") : this.GetByteContent("nuget.config.prod.xml")),
                new FileCheck(@"nuget.config.dev.xml", true, this.GetByteContent("nuget.config.dev.xml")),
                new FileCheck(@"nuget.config.prod.xml", true, this.GetByteContent("nuget.config.prod.xml")),
                new FileCheck(@"README.md", true),
                new FileCheck($@"{this.Solution.Name}.sln", true),


                new FileCheck(@"build\azure-pipelines-dev.yml", true, this.GetByteContent("azure-pipelines-dev.yml")),
                new FileCheck(@"build\azure-pipelines-main.yml", true, this.GetByteContent("azure-pipelines-main.yml")),

                new FileCheck(@"misc\changelog.txt", true),
                new FileCheck(@"misc\todo.txt", true),
                new FileCheck(@"misc\links.txt", false),

                new FileCheck($@"nuget\{this.Solution.Name}.nuspec", false),

                new FileCheck($@"src\{this.Solution.Name}\{this.Solution.Name}.csproj", true),

                new FileCheck($@"test\{this.Solution.Name}.Tests\{this.Solution.Name}.Tests.csproj", true),
            };

            var directoryCheckRunner = new DirectoryCheckRunner();
            directoryCheckRunner.Run(this.SolutionDirectoryPath, checks, this.Filter);
        }

        private bool Filter(
            RhoDirectoryNode parentDirectoryNode,
            bool isDirectory,
            string localName)
        {
            if (isDirectory)
            {
                if (localName.StartsWith('.'))
                {
                    return false;
                }

                if (localName.ToLowerInvariant().IsIn("bin", "obj"))
                {
                    return false;
                }
            }
            else
            {
                if (localName.EndsWith(".user"))
                {
                    return false;
                }

                if (Path.GetExtension(localName) == ".cs")
                {
                    return false;
                }
            }

            return true;
        }


        private byte[] GetByteContent(string resourceName)
        {
            return this.GetType().Assembly.GetResourceBytes(resourceName, true);
        }


        public void CheckStructure()
        {
            var rootSolutionFolderNames = this
                .Solution
                .GetSolutionFolders()
                .Where(x => x.ParentSolutionFolder == null)
                .Select(x => x.Name)
                .ToList();

            var expectedSolutionFolderNames = new[]
            {
                "build",
                "misc",
                "nuget",
                "src",
                "test",
            };

            foreach (var expectedSolutionFolderName in expectedSolutionFolderNames)
            {
                if (!rootSolutionFolderNames.Contains(expectedSolutionFolderName))
                {
                    throw new NotImplementedException();
                }
            }

            var expectedBuildItems = new string[]
            {
                @"build\azure-pipelines-dev.yml",
                @"build\azure-pipelines-main.yml"
            };

            var buildItems = this.Solution.GetSolutionFolders().Single(x => x.Name == "build").IncludedFileLocalPaths;
            if (buildItems.Count != 2)
            {
                throw new NotImplementedException();
            }

            foreach (var expectedBuildItem in expectedBuildItems)
            {
                if (!buildItems.Contains(expectedBuildItem))
                {
                    throw new NotImplementedException();
                }
            }

            var expectedMiscItems = new string[]
            {
                @"misc\changelog.txt",
                @"misc\todo.txt"
            };

            var miscItems = this.Solution.GetSolutionFolders().Single(x => x.Name == "misc").IncludedFileLocalPaths;

            foreach (var expectedMiscItem in expectedMiscItems)
            {
                if (!miscItems.Contains(expectedMiscItem))
                {
                    throw new NotImplementedException();
                }
            }

            var nugetItem = this
                .Solution
                .GetSolutionFolders()
                .Single(x => x.Name == "nuget")
                .IncludedFileLocalPaths
                .Single();

            var expectedNugetItem = $@"nuget\{this.Solution.Name}.nuspec";

            if (nugetItem != expectedNugetItem)
            {
                throw new NotImplementedException();
            }

            var expectedProjectName = this.Solution.Name;

            var project = this
                .Solution
                .GetSolutionFolders()
                .Single(x => x.Name == "src")
                .GetSolutionFolderProjects()
                .Single(x => x.Name == expectedProjectName)
                .Project;

            var packageReferences = project
                .ItemGroups
                .SelectMany(x => x.PackageReferences)
                .Where(x => x.Include.StartsWith("TauCode."))
                .ToList();


            var expectedTestProjectName = $"{this.Solution.Name}.Tests";

            var targetFramework = project.PropertyGroups.Single().TargetFramework;
            if (targetFramework != "netstandard2.1")
            {
                throw new NotImplementedException();
            }

            var testProject = this
                .Solution
                .GetSolutionFolders()
                .Single(x => x.Name == "test")
                .GetSolutionFolderProjects()
                .Single(x => x.Name == expectedTestProjectName)
                .Project;

            targetFramework = testProject.PropertyGroups.Single().TargetFramework;
            if (targetFramework != "netcoreapp3.1")
            {
                throw new NotImplementedException();
            }
        }

        private Dictionary<string, string> GetPackageReferences()
        {
            var project = this
                .Solution
                .GetSolutionFolders()
                .Single(x => x.Name == "src")
                .GetSolutionFolderProjects()
                .Single(x => x.Name == this.Solution.Name)
                .Project;

            var packageReferences = project
                .ItemGroups
                .SelectMany(x => x.PackageReferences)
                .Where(x => x.Include.StartsWith("TauCode."))
                .ToList();

            return packageReferences
                .ToDictionary(x => x.Include, x => x.Version);

        }

        public void CheckNuspec()
        {
            var nuspecDoc = new XmlDocument();
            nuspecDoc.Load(@$"{this.SolutionDirectoryPath}\nuget\{this.Solution.Name}.nuspec");
            var serializer = new Serializer();
            serializer.Settings = new SerializationSettings
            {
                BoundPropertyValueConverter = new Nuspec.NuspecConverter(),
            };

            var nuspec = serializer.DeserializeXmlDocument<Nuspec>(nuspecDoc);

            LibDevHelper.CheckCondition(
                nuspec.Declaration.Version == "1.0" &&
                nuspec.Declaration.Encoding == "utf-8" &&
                nuspec.Declaration.Standalone == "yes",
                "nuspec declaration");


            LibDevHelper.CheckCondition(
                nuspec.Xmlns == "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd",
                "nuspec declaration");

            LibDevHelper.CheckCondition(
                nuspec.Metadata != null,
                "nuspec metadata not null");


            LibDevHelper.CheckCondition(
                nuspec.Metadata?.Id == this.Solution.Name,
                "nuspec package id");

            LibDevHelper.CheckCondition(
                LibDevHelper.IsValidDevVersion(nuspec.Metadata?.Version),
                "nuspec version (dev)");

            LibDevHelper.CheckCondition(
                nuspec.Metadata?.Authors == "TauCode",
                "nuspec authors");

            LibDevHelper.CheckCondition(

                nuspec.Metadata?.Owners == "TauCode",
                "nuspec owners");

            LibDevHelper.CheckCondition(

                nuspec.Metadata?.RequireLicenseAcceptance == false,
                "nuspec require license acceptance");

            LibDevHelper.CheckCondition(

                nuspec.Metadata?.License?.Type == "file",
                "nuspec license type");

            LibDevHelper.CheckCondition(

                nuspec.Metadata?.License?.Value == "LICENSE.txt",
                "nuspec license file");

            var url = $"https://github.com/taucode/{this.Solution.Name.ToLowerInvariant()}";

            LibDevHelper.CheckCondition(

                nuspec.Metadata?.ProjectUrl == url,
                "nuspec project url");

            LibDevHelper.CheckCondition(

                nuspec.Metadata?.Repository?.Type == "git",
                "nuspec repository type");

            LibDevHelper.CheckCondition(

                nuspec.Metadata?.Repository?.Url == url,
                "nuspec repository url");

            LibDevHelper.CheckCondition(

                nuspec.Metadata?.Description != null,
                "nuspec description");

            LibDevHelper.CheckCondition(

                nuspec.Metadata?.ReleaseNotes != null,
                "nuspec release notes");

            LibDevHelper.CheckCondition(

                nuspec.Metadata?.Tags?.StartsWith("taucode ") ?? false,
                "nuspec tags contains taucode");

            LibDevHelper.CheckCondition(

                !nuspec.Metadata?.Tags.Contains("todo") ?? false,
                "nuspec tags doesn't contains 'to-do'");

            var check =
                nuspec.Metadata?.Dependencies != null &&
                (nuspec.Metadata?.Dependencies.Groups?.Count ?? -1) == 1 &&
                nuspec.Metadata.Dependencies.Groups.Single().TargetFramework == ".NETStandard2.1";

            LibDevHelper.CheckCondition(
                check,
                "nuspec dependencies group is .NETStandard2.1");

            var nuspecDependencies = nuspec
                .Metadata
                .Dependencies
                .Groups
                .Single()
                .Dependencies
                .ToDictionary(x => x.Id, x => x.Version);

            var projectPackageReferences = this.GetPackageReferences();

            LibDevHelper.CheckCondition(
                nuspecDependencies.IsEquivalentToDictionary(projectPackageReferences),
                "nuspec dependencies");
        }
    }
}
