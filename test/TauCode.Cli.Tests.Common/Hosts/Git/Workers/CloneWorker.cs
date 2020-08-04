using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Git.Workers
{
    public class CloneWorker : CommonWorker
    {
        public CloneWorker()
            : base(
                typeof(CloneWorker).Assembly.GetResourceText(".Git.NoName.Clone.lisp", true),
                null,
                true)
        {
        }
    }
}
