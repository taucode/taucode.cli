using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using TauCode.Cli.Commands;
using TauCode.Extensions;
using TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Tools;
using TauCode.Lab.Dev;
using TauCode.Lab.Dev.Data;
using TauCode.Lab.Dev.Instruments;
using TauCode.Lab.Xml;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Executors
{
    public class DevIdleCheckExecutor : LibDevExecutor
    {
        private string _solutionDirectoryPath;
        private DirectoryInfo _solutionDirectory;
        private string _solutionName;
        private string _solutionFilePath;

        public DevIdleCheckExecutor()
            : base(
                typeof(DevIdleCheckExecutor).Assembly.GetResourceText($"{nameof(DevIdleCheckExecutor)}.lisp", true),
                null,
                false)
        {
        }

        public void Process_TodoOld(IList<CliCommandEntry> entries)
        {
            _solutionName = null;

            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);
            _solutionDirectoryPath = this.ResolveDirectory(summary);

            _solutionDirectory = new DirectoryInfo(_solutionDirectoryPath);
            var subDirs = _solutionDirectory.GetDirectories();

            var directoryCheckRunner = new DirectoryCheckRunnerTodo();
            var fileCheckRunner = new FileCheckRunnerTodo();

            #region root directories

            var directoryChecks = new[]
            {
                new DirectoryCheckTodo(".git", true),
                new DirectoryCheckTodo(".trash", false),
                new DirectoryCheckTodo(".vs", false),
                new DirectoryCheckTodo("assets", false),
                new DirectoryCheckTodo("build", true),
                new DirectoryCheckTodo("doc", false),
                new DirectoryCheckTodo("misc", true),
                new DirectoryCheckTodo("nuget", true),
                new DirectoryCheckTodo("src", true),
                new DirectoryCheckTodo("test", true),
            };

            var rootSubdirectoriesCheckResults = directoryCheckRunner.Run(_solutionDirectoryPath, directoryChecks);
            var rootSubdirectoriesTotalResult = this.ReportDirectoryCheckResults(
                _solutionDirectoryPath,
                rootSubdirectoriesCheckResults);

            #endregion

            #region root files

            var fileChecks = new[]
            {
                new FileCheckTodo(
                    ".gitignore",
                    true,
                    null),
                new FileCheckTodo(
                    "LICENSE.txt",
                    true,
                    () => this.GetType().Assembly.GetResourceBytes("LICENSE.txt", true)),
                new FileCheckTodo(
                    "nuget.config",
                    true,
                    () => this.GetType().Assembly.GetResourceBytes("nuget.config.dev.xml", true)),
                new FileCheckTodo(
                    "nuget.config.dev.xml",
                    true,
                    () => this.GetType().Assembly.GetResourceBytes("nuget.config.dev.xml", true)),
                new FileCheckTodo(
                    "nuget.config.prod.xml",
                    true,
                    () => this.GetType().Assembly.GetResourceBytes("nuget.config.prod.xml", true)),
                new FileCheckTodo(
                    "README.md",
                    true,
                    null),
            };

            var rootFilesCheckResults = fileCheckRunner.Run(_solutionDirectoryPath, fileChecks);
            var rootFilesCheckTotalResult = this.ReportFileCheckResults(
                _solutionDirectoryPath,
                rootFilesCheckResults,
                this.SolutionDirectoryCustomCheck);

            #endregion

            #region build files

            fileChecks = new FileCheckTodo[]
            {
                new FileCheckTodo(
                    "azure-pipelines-dev.yml",
                    true,
                    () => this.GetType().Assembly.GetResourceBytes("azure-pipelines-dev.yml", true)),
                new FileCheckTodo(
                    "azure-pipelines-main.yml",
                    true,
                    () => this.GetType().Assembly.GetResourceBytes("azure-pipelines-main.yml", true)),
            };

            var buildDirectoryPath = _solutionDirectory.GetDirectories("build").Single().FullName;
            var buildFilesCheckResults = fileCheckRunner.Run(buildDirectoryPath, fileChecks);
            var buildFilesCheckTotalResult = this.ReportFileCheckResults(
                buildDirectoryPath,
                buildFilesCheckResults,
                null);

            #endregion

            #region misc files

            fileChecks = new FileCheckTodo[]
            {
                new FileCheckTodo(
                    "changelog.txt",
                    true,
                    null),
                new FileCheckTodo(
                    "links.txt",
                    false,
                    null),
                new FileCheckTodo(
                    "todo.txt",
                    true,
                    null),
            };

            var miscDirectoryPath = _solutionDirectory.GetDirectories("misc").Single().FullName;
            var miscFilesCheckResults = fileCheckRunner.Run(miscDirectoryPath, fileChecks);
            var miscFilesCheckTotalResult = this.ReportFileCheckResults(
                miscDirectoryPath,
                miscFilesCheckResults,
                null);

            #endregion

            #region nuget files

            fileChecks = new FileCheckTodo[]
            {
                new FileCheckTodo(
                    $"{_solutionName}.nuspec",
                    true,
                    null),
            };

            var nugetDirectoryPath = _solutionDirectory.GetDirectories("nuget").Single().FullName;
            var nugetFilesCheckResults = fileCheckRunner.Run(nugetDirectoryPath, fileChecks);
            var nugetFilesCheckTotalResult = this.ReportFileCheckResults(
                nugetDirectoryPath,
                nugetFilesCheckResults,
                null);


            #endregion

            #region src sub-directories

            directoryChecks = new DirectoryCheckTodo[]
            {
                new DirectoryCheckTodo(
                    _solutionName,
                    true),
            };

            var srcDirectoryPath = _solutionDirectory.GetDirectories("src").Single().FullName;
            var srcSubdirectoriesCheckResults = directoryCheckRunner.Run(srcDirectoryPath, directoryChecks);
            var srcSubdirectoriesTotalResult = this.ReportDirectoryCheckResults(
                srcDirectoryPath,
                srcSubdirectoriesCheckResults);

            #endregion

            #region test sub-directories

            directoryChecks = new DirectoryCheckTodo[]
            {
                new DirectoryCheckTodo(
                    $"{_solutionName}.Tests",
                    true),
            };

            var testDirectoryPath = _solutionDirectory.GetDirectories("test").Single().FullName;
            var testSubdirectoriesCheckResults = directoryCheckRunner.Run(testDirectoryPath, directoryChecks);
            var testSubdirectoriesTotalResult = this.ReportDirectoryCheckResults(
                testDirectoryPath,
                testSubdirectoriesCheckResults);

            #endregion

            #region solution

            var ide = new Ide();
            _solutionFilePath = Path.Combine(_solutionDirectoryPath, $"{_solutionName}.sln");
            ide.LoadSolution(_solutionFilePath);
            var solution = ide.Solution;

            var folders = solution.GetSolutionFolders();

            bool solutionCheck;

            do
            {
                solutionCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    LibDevHelper.CollectionsAreEquivalent(
                        folders
                            .Select(x => x.Name)
                            .OrderBy(x => x),
                        new[]
                        {
                            "build",
                            "misc",
                            "nuget",
                            "src",
                            "test",
                        }),
                    "Solution folders are correct");

                if (!solutionCheck)
                {
                    break;
                }

                solutionCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    LibDevHelper.CollectionsAreEquivalent(
                        folders
                            .Single(x => x.Name == "build")
                            .IncludedFileLocalPaths
                            .OrderBy(x => x),
                        new[]
                        {
                            @"build\azure-pipelines-dev.yml",
                            @"build\azure-pipelines-main.yml",
                        }),
                    "'build' solution folder items are correct");

                if (!solutionCheck)
                {
                    break;
                }

            } while (false);

            #endregion

            #region nuspec

            var nuspecCheck = true;

            var project = solution.GetSolutionProjects().Single(x => x.Project.Name == _solutionName).Project;
            var dependencies = project
                .ItemGroups
                .SelectMany(x => x.PackageReferences)
                .Select(x => $"{x.Include}:{x.Version}")
                .ToList();

            var nuspecDoc = new XmlDocument();
            nuspecDoc.Load(@$"{_solutionDirectoryPath}\nuget\{_solutionName}.nuspec");
            var serializer = new Serializer();
            serializer.Settings = new SerializationSettings
            {
                BoundPropertyValueConverter = new Nuspec.NuspecConverter(),
            };

            var nuspec = serializer.DeserializeXmlDocument<Nuspec>(nuspecDoc);

            do
            {
                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Declaration.Version == "1.0" &&
                    nuspec.Declaration.Encoding == "utf-8" &&
                    nuspec.Declaration.Standalone == "yes",
                    "nuspec declaration");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Xmlns == "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd",
                    "nuspec declaration");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata != null,
                    "nuspec metadata not null");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.Id == _solutionName,
                    "nuspec package id");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    LibDevHelper.IsValidDevVersion(nuspec.Metadata?.Version),
                    "nuspec version (dev)");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.Authors == "TauCode",
                    "nuspec authors");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.Owners == "TauCode",
                    "nuspec owners");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.RequireLicenseAcceptance == false,
                    "nuspec require license acceptance");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.License?.Type == "file",
                    "nuspec license type");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.License?.Value == "LICENSE.txt",
                    "nuspec license file");

                if (!nuspecCheck)
                {
                    break;
                }

                var url = $"https://github.com/taucode/{_solutionName.ToLowerInvariant()}";

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.ProjectUrl == url,
                    "nuspec project url");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.Repository?.Type == "git",
                    "nuspec repository type");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.Repository?.Url == url,
                    "nuspec repository url");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.Description != null,
                    "nuspec description");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.ReleaseNotes != null,
                    "nuspec release notes");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspec.Metadata?.Tags?.StartsWith("taucode ") ?? false,
                    "nuspec tags contains taucode");

                if (!nuspecCheck)
                {
                    break;
                }

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    !nuspec.Metadata?.Tags.Contains("todo") ?? false,
                    "nuspec tags doesn't contains 'to-do'");

                if (!nuspecCheck)
                {
                    break;
                }


                nuspecCheck =
                    nuspec.Metadata?.Dependencies != null &&
                    (nuspec.Metadata?.Dependencies.Groups?.Count ?? -1) == 1 &&
                    nuspec.Metadata.Dependencies.Groups.Single().TargetFramework == ".NETStandard2.1";

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    nuspecCheck,
                    "nuspec dependencies group is .NETStandard2.1");

                if (!nuspecCheck)
                {
                    break;
                }

                var nuspecDependencies = nuspec
                    .Metadata
                    .Dependencies
                    .Groups
                    .Single()
                    .Dependencies
                    .Select(x => $"{x.Id}:{x.Version}")
                    .ToList();

                nuspecCheck = LibDevHelper.ReportCondition(
                    this.Output,
                    LibDevHelper.CollectionsAreEquivalent(
                        dependencies,
                        nuspecDependencies),
                    "nuspec dependencies");

                if (!nuspecCheck)
                {
                    break;
                }
            } while (false);

            #endregion

            #region git

            var gitCheck = true;

            do
            {
                var gitCheck1 = LibDevHelper.ReportGitResult(
                    this.Output,
                    _solutionDirectoryPath,
                    "fetch",
                    new string[0]);

                var gitCheck2 = LibDevHelper.ReportGitResult(
                    this.Output,
                    _solutionDirectoryPath,
                    "status",
                    new []
                    {
                        "On branch dev",
                        "Your branch is up to date with 'origin/dev'.",
                        "nothing to commit, working tree clean",
                    });

                var gitCheck3 = LibDevHelper.ReportGitResult(
                    this.Output,
                    _solutionDirectoryPath,
                    "branch",
                    new[]
                    {
                        "* dev",
                        "  main",
                    });

                var gitCheck4 = LibDevHelper.ReportGitResult(
                    this.Output,
                    _solutionDirectoryPath,
                    "branch --remote",
                    new[]
                    {
                        "  origin/HEAD -> origin/main",
                        "  origin/dev",
                        "  origin/main",
                    });

                gitCheck =
                    gitCheck1 &&
                    gitCheck2 &&
                    gitCheck3 &&
                    gitCheck4 &&
                    true;

            } while (false);


            #endregion

            var result =
                rootSubdirectoriesTotalResult &&
                rootFilesCheckTotalResult &&
                buildFilesCheckTotalResult &&
                miscFilesCheckTotalResult &&
                nugetFilesCheckTotalResult &&
                srcSubdirectoriesTotalResult &&
                testSubdirectoriesTotalResult &&
                solutionCheck &&
                nuspecCheck &&
                gitCheck &&
                true;

            this.Output.WriteLine($"## Total Result: {result.ToPassedString()}");
        }

        private bool ReportDirectoryCheckResults(
            string directory,
            IReadOnlyDictionary<string, DirectoryCheckResultTodo> directoryCheckResults)
        {
            this.Output.WriteLine($"# Checking sub-directories of: '{directory}'");
            this.Output.WriteLine();

            var result = true;

            foreach (var key in directoryCheckResults.Keys)
            {
                var directoryCheckResult = directoryCheckResults[key];
                this.Output.WriteLine($"{key.PadRight(80)}{directoryCheckResult}");

                if (directoryCheckResult != DirectoryCheckResultTodo.Ok)
                {
                    result = false;
                }
            }

            this.Output.WriteLine();
            this.Output.WriteLine($"# Result: '{result.ToPassedString()}'");
            this.Output.WriteLine();

            return result;
        }

        private bool ReportFileCheckResults(
            string directory,
            IReadOnlyDictionary<string, FileCheckResultTodo> filesCheckResults,
            Func<string, FileCheckResultTodo> customCheck)
        {
            this.Output.WriteLine($"# Checking files of: '{directory}'");
            this.Output.WriteLine();

            bool result = true;

            foreach (var pair in filesCheckResults)
            {
                var key = pair.Key;
                var value = pair.Value;

                if (value == FileCheckResultTodo.Unexpected)
                {
                    if (customCheck == null)
                    {
                        // unexpected
                    }
                    else
                    {
                        value = customCheck(key);
                    }
                }

                if (!value.IsIn(FileCheckResultTodo.Ok, FileCheckResultTodo.Ignored))
                {
                    result = false;
                }

                this.Output.WriteLine($"{key.PadRight(80)}{value}");
            }

            this.Output.WriteLine();
            this.Output.WriteLine($"# Result: '{result.ToPassedString()}'");
            this.Output.WriteLine();

            return result;
        }

        private FileCheckResultTodo SolutionDirectoryCustomCheck(string localFileName)
        {
            if (localFileName.EndsWith(".sln"))
            {
                if (_solutionName != null)
                {
                    throw new NotImplementedException(); // more than one solution
                }

                if (
                    localFileName.ToLowerInvariant() == $"{_solutionDirectory.Name}.sln" &&
                    localFileName.StartsWith("TauCode.")
                )
                {
                    _solutionName = Path.GetFileNameWithoutExtension(localFileName);
                    return FileCheckResultTodo.Ok;
                }
            }
            else if (localFileName.EndsWith(".user"))
            {
                return FileCheckResultTodo.Ignored;
            }

            return FileCheckResultTodo.Unexpected;
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);

            var directory = this.ResolveDirectory(summary);
            var solutionChecker = new SolutionChecker(directory)
            {
                IsDev = true,
            };

            solutionChecker.CheckFileSystem();
            solutionChecker.CheckStructure();
            solutionChecker.CheckNuspec();
        }
    }
}
