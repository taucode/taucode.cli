using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Cli.Data.Entries;
using TauCode.Extensions;

namespace TauCode.Cli.Demo.Hosts.Git.Workers
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

        public override void Process(IList<ICliCommandEntry> entries)
        {
            this.Output.WriteLine("git clone");
            var repoUrl = entries.GetSingleEntryByAlias<PathEntry>("repo-url").Path;
            var repoPath = entries.GetSingleEntryByAlias<PathEntry>("repo-path").Path;

            this.Output.WriteLine($"url  : {repoUrl}");
            this.Output.WriteLine($"path : {repoPath}");
        }
    }
}
