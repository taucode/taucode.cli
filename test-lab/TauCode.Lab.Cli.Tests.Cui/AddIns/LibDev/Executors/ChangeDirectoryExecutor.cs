using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Commands;
using TauCode.Extensions;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Executors
{
    public class ChangeDirectoryExecutor : LibDevExecutor
    {
        public ChangeDirectoryExecutor()
            : base(
                typeof(ChangeDirectoryExecutor).Assembly.GetResourceText($"{nameof(ChangeDirectoryExecutor)}.lisp", true),
                null,
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);
            var directory = summary.Arguments["directory"].SingleOrDefault();
            if (directory == null)
            {
                this.Output.WriteLine(this.LibDevContext.CurrentDirectory);
            }
            else
            {
                if (directory == ".")
                {
                    // do nothing
                }
                else if (directory == "..")
                {
                    var dirInfo = new DirectoryInfo(this.LibDevContext.CurrentDirectory);
                    var parent =
                        dirInfo?.Parent?.FullName
                        ??
                        throw new DirectoryNotFoundException($"Parent directory of '{this.LibDevContext.CurrentDirectory}' not found.");

                    if (Directory.Exists(parent))
                    {
                        this.LibDevContext.CurrentDirectory = parent;
                    }
                    else
                    {
                        throw new DirectoryNotFoundException($"Directory '{parent}' not found.");
                    }
                }
                else
                {
                    if (Directory.Exists(directory))
                    {
                        this.LibDevContext.CurrentDirectory = Path.GetFullPath(directory);
                    }
                    else
                    {
                        throw new DirectoryNotFoundException($"Directory '{directory}' not found.");
                    }
                }
            }
        }
    }
}
