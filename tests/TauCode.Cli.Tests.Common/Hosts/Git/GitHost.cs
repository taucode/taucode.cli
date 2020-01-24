using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.Hosts.Git
{
    public class GitHost : CliHostBase
    {
        public GitHost()
            : base("git", "git-1.0", true)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new GitAddIn(),
            };
        }
    }
}
