using System;
using System.IO;
using System.Linq;
using TauCode.Extensions;
using TauCode.Lab.Data.Graphs;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Dev.Instruments;
using TauCode.Lab.Utility;

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
            //var solutionFilePath = _solutionDirectoryInfo.GetFiles("*.sln").Single();


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
                new FileCheck(@"LICENSE.txt", true, this.GetContent("LICENSE.txt")),
                new FileCheck(
                    @"nuget.config",
                    true,
                    this.IsDev ? this.GetContent("nuget.config.dev.xml") : this.GetContent("nuget.config.prod.xml")),
                new FileCheck(@"nuget.config.dev.xml", true, this.GetContent("nuget.config.dev.xml")),
                new FileCheck(@"nuget.config.prod.xml", true, this.GetContent("nuget.config.prod.xml")),
                new FileCheck(@"README.md", true),
                new FileCheck($@"{this.Solution.Name}.sln", true),


                new FileCheck(@"build\azure-pipelines-dev.yml", true, this.GetContent("azure-pipelines-dev.yml")),
                new FileCheck(@"build\azure-pipelines-main.yml", true, this.GetContent("azure-pipelines-main.yml")),

                new FileCheck(@"build\changelog.txt", true),
                new FileCheck(@"build\todo.txt", true),
                new FileCheck(@"build\links.txt", false),

                new FileCheck($@"nuget\{this.Solution.Name}.nuspec", false),

                new FileCheck($@"src\{this.Solution.Name}\{this.Solution.Name}.csproj", true),

                new FileCheck($@"test\{this.Solution.Name}.Tests\{this.Solution.Name}.Tests.csproj", true),
            };

            var directoryCheckRunner = new DirectoryCheckRunner();
            directoryCheckRunner.Run(this.SolutionDirectoryPath, checks, this.Filter);
        }

        private bool Filter(
            INode<FileSystemObjectInfo> parentDirectoryNode,
            FileSystemObjectType type,
            string localName)
        {
            if (type == FileSystemObjectType.Directory)
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
            else if (type == FileSystemObjectType.File)
            {
                if (localName.EndsWith(".user"))
                {
                    return false;
                }
            }

            return true;
        }


        private string GetContent(string resourceName)
        {
            return this.GetType().Assembly.GetResourceText(resourceName, true);
        }


        public void CheckStructure()
        {
            throw new NotImplementedException();
        }

        public void CheckNuspec()
        {
            throw new NotImplementedException();
        }
    }
}
