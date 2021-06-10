//using System;
//using System.Linq;

//// todo get rid of.
//namespace TauCode.Cli.HostRunners
//{
//    public class ProductionHostRunner : ICliHostRunner
//    {
//        #region Fields

//        private readonly ICliHost _host;
//        private readonly bool _showLine;

//        #endregion

//        #region Constructor

//        public ProductionHostRunner(ICliHost host, bool showLine = false)
//        {
//            _host = host ?? throw new ArgumentNullException(nameof(host));
//            _showLine = showLine;
//        }

//        #endregion

//        #region ICliHostRunner Members

//        public virtual int Run(string[] args)
//        {
//            if (args == null)
//            {
//                throw new ArgumentNullException(nameof(args));
//            }

//            if (args.Any(x => x == null))
//            {
//                throw new ArgumentException($"'{nameof(args)}' cannot contain nulls.");
//            }

//            var line = string.Join(" ", args);

//            if (_showLine)
//            {
//                _host.Output.WriteLine(line);
//            }

//            try
//            {
//                var command = _host.ParseLine(line);
//                _host.DispatchCommand(command);
//                return 0;
//            }
//            catch (Exception ex)
//            {
//                _host.Output.WriteLine(ex);
//                return -1;
//            }
//        }

//        public ICliHost Host => _host;


//        #endregion
//    }
//}
