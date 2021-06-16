using System.Collections.Generic;
using System.IO;
using TauCode.Cli;
using TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Executors;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev
{
    public class LibDevAddIn : CliAddInBase
    {
        public LibDevAddIn()
            : base("libdev", "1.0", true)
        {
            
        }

        protected override IReadOnlyList<ICliExecutor> CreateExecutors()
        {
            return new List<ICliExecutor>
            {
                new DependenciesExecutor(),
                new ChangeDirectoryExecutor(),
                new NewLibraryExecutor(),
                new StartReleaseExecutor(),
                new DevIdleCheckExecutor(),
            };
        }

        protected override ICliContext CreateContext()
        {
            return new LibDevContext
            {
                CurrentDirectory = Directory.GetCurrentDirectory(),
            };
        }
    }
}
