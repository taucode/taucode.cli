using System;
using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Git.Workers
{
    public class CloneWorker : CliWorkerBase
    {
        public CloneWorker()
            : base(
                typeof(CloneWorker).Assembly.GetResourceText(".Git.NoName.Clone.lisp", true),
                null,
                true)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            throw new NotImplementedException();
            //this.Output.WriteLine("git clone");
            //var repoUrl = entries.GetSingleEntryByAlias<PathEntry>("repo-url").Path;
            //var repoPath = entries.GetSingleEntryByAlias<PathEntry>("repo-path").Path;

            //this.Output.WriteLine($"url  : {repoUrl}");
            //this.Output.WriteLine($"path : {repoPath}");
        }
    }
}
