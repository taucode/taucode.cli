using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TauCode.Cli.Exceptions;
using TauCode.Cli.Tests.Common.Hosts.Curl;
using TauCode.Cli.Tests.Common.Hosts.Git;
using TauCode.Cli.Tests.Common.Hosts.Kubectl;
using TauCode.Cli.Tests.Common.Hosts.Tau;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tests.Exe
{
    // todo: clean up
    public class Program
    {
        #region Nested

        private class ExitException : Exception { }

        #endregion

        #region Static

        private static void Main()
        {
            var program = new Program();
            program.Run();
        }

        #endregion

        #region Fields

        private readonly IDictionary<string, ICliHost> _hosts;
        private ICliHost _currentHost;

        #endregion

        #region Constructor

        private Program()
        {
            var idleHost = new IdleHost();
            idleHost.AddCustomHandler(
                () => Console.WriteLine(this.GetHelp()),
                "--help");

            idleHost.AddCustomHandler(
                () => Console.WriteLine(this.GetAllHostsName()),
                "--all-hosts");
            _hosts = new ICliHost[]
                {
                    new TauHost(),
                    new GitHost(),
                    new CurlHost(),
                    new KubectlHost(),
                    idleHost,
                }
                .ToDictionary(x => x.Name, x => x);

            _hosts.Values.ToList().ForEach(x =>
            {
                x.Output = Console.Out;
                x.Input = Console.In;

                x.AddCustomHandler(
                    Console.Clear, 
                    "cls");

                x.AddCustomHandler(
                    () => throw new ExitException(),
                    "exit");

                x.AddCustomHandlerWithParameter(
                    (token) =>
                    {
                        if (token is TextToken textToken)
                        {
                            var name = textToken.Text;
                            _currentHost = _hosts[name];
                            throw new CliCustomHandlerException();
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    },
                    "--host"
                );

            });

            _currentHost = idleHost;
        }

        private string GetAllHostsName()
        {
            return string.Join(
                Environment.NewLine,
                _hosts.Values.Select(x => x.Name));
        }

        private string GetHelp()
        {
            var sb = new StringBuilder();
            sb.AppendLine("--help               Shows available commands");
            sb.AppendLine("--all-hosts          Shows all available host names");
            sb.AppendLine("--host <hostname>    Switches to a specified host");

            return sb.ToString();
        }

        #endregion

        private void Run()
        {
            while (true)
            {
                Console.Write(this.MakePrompt());
                var line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                try
                {
                    var command = _currentHost.ParseCommand(line);
                    _currentHost.DispatchCommand(command);
                }
                catch (UnexpectedTokenException ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("type --help for help.");
                }
                catch (ExitException)
                {
                    break;
                }
                catch (CliCustomHandlerException)
                {
                    // ignore.
                }
                catch (FallbackInterceptedCliException)
                {
                    // ignore.
                }
                catch (CliException ex)
                {
                    Console.Write("CLI Error: ");
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private string MakePrompt()
        {
            return $"{_currentHost.Name} >";
        }
    }
}
