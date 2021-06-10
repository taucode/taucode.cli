﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TauCode.Cli.Exceptions;
using TauCode.Parsing.Exceptions;

// todo get rid of.
namespace TauCode.Cli.HostRunners
{
    public class DemoHostRunner : ICliHostRunner
    {
        #region Nested

        private class ExitException : Exception { }

        #endregion

        #region Fields

        private readonly CustomCliHost _idle;
        private readonly Dictionary<string, ICliHost> _hosts;
        private ICliHost _currentHost;

        #endregion

        #region Constructor

        public DemoHostRunner(
            string idleHostName,
            IEnumerable<ICliHost> hosts)
        {
            _idle = new CustomCliHost(idleHostName);

            if (hosts == null)
            {
                throw new ArgumentNullException(nameof(hosts));
            }

            var hostList = hosts.ToList();

            if (hostList.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(hosts)}' must not contain nulls.", nameof(hosts));
            }

            hostList.Add(_idle);
            _hosts = hostList.ToDictionary(x => x.Name, x => x);
        }

        #endregion

        #region Protected

        protected virtual void InitHosts()
        {
            throw new NotImplementedException();
            //_idle
            //    .AddCustomHandler(
            //        () => Console.WriteLine(this.GetHelp()),
            //        "--help")
            //    .AddCustomHandler(
            //        () => Console.WriteLine(this.GetAllHostNames()),
            //        "--all-hosts");

            //_hosts.Values.ToList().ForEach(x =>
            //{
            //    x.Output = Console.Out;
            //    x.Input = Console.In;

            //    x
            //        .AddCustomHandler(
            //            Console.Clear,
            //            "cls")
            //        .AddCustomHandler(
            //            () => throw new ExitException(),
            //            "exit")
            //        .AddCustomHandlerWithParameter(
            //            (token) =>
            //            {
            //                if (token is TextToken textToken && textToken.Class is TermTextClass)
            //                {
            //                    var name = textToken.Text;
            //                    _currentHost = _hosts[name];
            //                }
            //                else
            //                {
            //                    throw new CliException("Host ID expected.");
            //                }
            //            },
            //            "--host");

            //});

            //_currentHost = _idle;
        }

        protected virtual string GetHelp()
        {
            var sb = new StringBuilder();
            sb.AppendLine("--help               Shows available commands");
            sb.AppendLine("--all-hosts          Shows all available host names");
            sb.AppendLine("--host <hostname>    Switches to a specified host");

            return sb.ToString();
        }

        protected virtual string GetAllHostNames()
        {
            return string.Join(
                Environment.NewLine,
                _hosts.Values.Select(x => x.Name));
        }

        protected virtual string MakePrompt()
        {
            return $"{_currentHost.Name} >";
        }

        #endregion

        #region Public

        public IReadOnlyDictionary<string, ICliHost> Hosts => _hosts;

        public void SetHost(string hostName)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException(nameof(hostName));
            }

            if (!_hosts.ContainsKey(hostName))
            {
                throw new InvalidOperationException($"Host '{hostName}' not found.");
            }

            _currentHost = _hosts[hostName];
        }

        #endregion

        #region ICliHostRunner Members

        public virtual int Run(string[] args)
        {
            this.InitHosts();

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
                    var command = _currentHost.ParseLine(line);
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

            return 0;
        }

        public ICliHost Host => _currentHost;

        #endregion
    }
}
