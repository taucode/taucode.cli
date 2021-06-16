using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Commands;
using TauCode.Extensions;
using TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Tools;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Executors
{
    public class DevIdleCheckExecutor : LibDevExecutor
    {
        private string _solutionDirectoryPath;
        private DirectoryInfo _solutionDirectory;
        private string _solutionName;

        public DevIdleCheckExecutor()
            : base(
                typeof(DevIdleCheckExecutor).Assembly.GetResourceText($"{nameof(DevIdleCheckExecutor)}.lisp", true),
                null,
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            _solutionName = null;

            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);
            _solutionDirectoryPath = this.ResolveDirectory(summary);

            _solutionDirectory = new DirectoryInfo(_solutionDirectoryPath);
            var subDirs = _solutionDirectory.GetDirectories();

            var directoryCheckRunner = new DirectoryCheckRunner();
            var fileCheckRunner = new FileCheckRunner();

            #region root directories

            var directoryChecks = new[]
            {
                new DirectoryCheck(".git", true),
                new DirectoryCheck(".trash", false),
                new DirectoryCheck(".vs", false),
                new DirectoryCheck("assets", false),
                new DirectoryCheck("build", true),
                new DirectoryCheck("doc", false),
                new DirectoryCheck("misc", true),
                new DirectoryCheck("nuget", true),
                new DirectoryCheck("src", true),
                new DirectoryCheck("test", true),
            };

            var rootSubdirectoriesCheckResults = directoryCheckRunner.Run(_solutionDirectoryPath, directoryChecks);
            var rootSubdirectoriesTotalResult = this.ReportDirectoryCheckResults(
                _solutionDirectoryPath,
                rootSubdirectoriesCheckResults);

            #endregion

            #region root files

            var fileChecks = new[]
            {
                new FileCheck(
                    ".gitignore",
                    true,
                    null),
                new FileCheck(
                    "LICENSE.txt",
                    true,
                    () => this.GetType().Assembly.GetResourceBytes("LICENSE.txt", true)),
                new FileCheck(
                    "nuget.config",
                    true,
                    () => this.GetType().Assembly.GetResourceBytes("nuget.config.dev.xml", true)),
                new FileCheck(
                    "nuget.config.dev.xml",
                    true,
                    () => this.GetType().Assembly.GetResourceBytes("nuget.config.dev.xml", true)),
                new FileCheck(
                    "nuget.config.prod.xml",
                    true,
                    () => this.GetType().Assembly.GetResourceBytes("nuget.config.prod.xml", true)),
                new FileCheck(
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

            fileChecks = new FileCheck[]
            {
                new FileCheck(
                    "azure-pipelines-dev.yml",
                    true,
                    () => this.GetType().Assembly.GetResourceBytes("azure-pipelines-dev.yml", true)),
                new FileCheck(
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

            fileChecks = new FileCheck[]
            {
                new FileCheck(
                    "changelog.txt",
                    true,
                    null),
                new FileCheck(
                    "links.txt",
                    false,
                    null),
                new FileCheck(
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

            var result =
                rootSubdirectoriesTotalResult &&
                rootFilesCheckTotalResult &&
                buildFilesCheckTotalResult &&
                miscFilesCheckTotalResult &&
                true;

            this.Output.WriteLine($"## Total Result: {result.ToPassedString()}");
        }

        private bool ReportDirectoryCheckResults(
            string directory,
            IReadOnlyDictionary<string, DirectoryCheckResult> directoryCheckResults)
        {
            this.Output.WriteLine($"# Checking sub-directories of: '{directory}'");
            this.Output.WriteLine();

            var result = true;

            foreach (var key in directoryCheckResults.Keys)
            {
                var directoryCheckResult = directoryCheckResults[key];
                this.Output.WriteLine($"{key.PadRight(80)}{directoryCheckResult}");

                if (directoryCheckResult != DirectoryCheckResult.Ok)
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
            IReadOnlyDictionary<string, FileCheckResult> filesCheckResults,
            Func<string, FileCheckResult> customCheck)
        {
            this.Output.WriteLine($"# Checking files of: '{directory}'");
            this.Output.WriteLine();

            bool result = true;

            foreach (var pair in filesCheckResults)
            {
                var key = pair.Key;
                var value = pair.Value;

                if (value == FileCheckResult.Unexpected)
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

                if (!value.IsIn(FileCheckResult.Ok, FileCheckResult.Ignored))
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

        private FileCheckResult SolutionDirectoryCustomCheck(string localFileName)
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
                    return FileCheckResult.Ok;
                }
            }
            else if (localFileName.EndsWith(".user"))
            {
                return FileCheckResult.Ignored;
            }

            return FileCheckResult.Unexpected;
        }
    }
}