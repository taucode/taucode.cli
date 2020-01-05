using System;
using TauCode.Cli.Exceptions;

namespace TauCode.Cli.Demo
{
    public class Program
    {
        private class ExitException : Exception { }

        private static void Main()
        {
            ICliHost host = new DemoHost();

            host.Output = Console.Out;
            host.Input = Console.In;

            host.AddCustomHandler(
                () => throw new ExitException(),
                "exit");

            host.AddCustomHandler(
                Console.Clear,
                "cls");

            while (true)
            {
                Console.Write("args >");
                var line = Console.ReadLine();

                try
                {
                    var command = host.ParseCommand(line);
                    host.DispatchCommand(command);
                }
                catch (ExitException)
                {
                    break;
                }
                catch (CliCustomHandlerException)
                {
                    // ignore.
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
