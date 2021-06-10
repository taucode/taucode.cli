namespace TauCode.Cli
{
    public class CliContextBase : ICliContext
    {
        public static ICliContext Empty { get; } = new CliContextBase();

        public virtual void Dispose()
        {
            // idle
        }
    }
}
