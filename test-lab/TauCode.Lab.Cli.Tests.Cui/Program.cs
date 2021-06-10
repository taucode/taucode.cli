using System;
using System.Globalization;
using TauCode.Cli;

namespace TauCode.Lab.Cli.Tests.Cui
{
    public class Program : CliProgramBase
    {
        #region Static Main

        private static void Main(string[] args)
        {
            Inflector.Inflector.SetDefaultCultureFunc = () => new CultureInfo("en-US");

            var program = new Program();
            program.Run(args);
        }

        #endregion

        public Program()
            : base(Console.In, Console.Out, true)
        {
        }

        protected override ICliHost CreateHost() => new Host();
    }
}
