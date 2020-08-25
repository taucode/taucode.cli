using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Git.Workers
{
    public class CloneExecutor : CommonExecutor
    {
        public CloneExecutor()
            : base(
                typeof(CloneExecutor).Assembly.GetResourceText(".Git.NoName.Clone.lisp", true),
                null,
                true)
        {
        }
    }
}
