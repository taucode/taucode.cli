using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli
{
    public interface ICliShell
    {
        ICliAddIn AddIn { get; }
        void Run();
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
