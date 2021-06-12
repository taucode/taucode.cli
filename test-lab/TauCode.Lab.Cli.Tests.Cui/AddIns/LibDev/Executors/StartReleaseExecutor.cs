using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.CommandSummary;
using TauCode.Cli.Data;
using TauCode.Extensions;
using TauCode.Lab.Dev.Instruments;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Executors
{
    public class StartReleaseExecutor : LibDevExecutor
    {
        public StartReleaseExecutor()
            : base(
                typeof(StartReleaseExecutor).Assembly.GetResourceText($"{nameof(StartReleaseExecutor)}.lisp", true),
                null,
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);
            var directory = this.ResolveDirectory(summary);

            this.Output.WriteLine("git branch");
            var lines = LibDevHelper.RunGitCommandReturnLines(directory, "branch");
            foreach (var line in lines)
            {
                this.Output.WriteLine(line);
            }
            if (!lines.AreEquivalentTo(
                "* dev",
                "main"))
            {
                throw new NotImplementedException();
            }

            this.Output.WriteLine();

            this.Output.WriteLine("git branch --remote");
            lines = LibDevHelper.RunGitCommandReturnLines(directory, "branch --remote");
            foreach (var line in lines)
            {
                this.Output.WriteLine(line);
            }
            if (!lines.AreEquivalentTo(
                "origin/HEAD -> origin/main",
                "origin/dev",
                "origin/main"))
            {
                throw new NotImplementedException();
            }

            this.Output.WriteLine();
            this.Output.WriteLine("git fetch");
            lines = LibDevHelper.RunGitCommandReturnLines(directory, "fetch");
            foreach (var line in lines)
            {
                this.Output.WriteLine(line);
            }
            if (!lines.AreEquivalentTo(/* empty */))
            {
                throw new NotImplementedException();
            }

            var ide = new Ide();
            var solutionFilePath = new DirectoryInfo(directory)
                .GetFiles("*.sln")
                .Single()
                .FullName;

            ide.LoadSolution(solutionFilePath);

            var nuspec = ide.Solution.LoadNuspec();

            var version = nuspec.Metadata.Version;

            throw new NotImplementedException();
        }
    }
}
