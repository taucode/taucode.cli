using System;
using System.Collections.Generic;
using TauCode.Cli.Demo.AddIns;
using TauCode.Cli.Demo.Exceptions;
using TauCode.Parsing.Lexing;

namespace TauCode.Cli.Demo
{
    public class Program : CliProgramBase
    {
        private static int Main(string[] args)
        {
            var program = new Program
            {
                Output = Console.Out,
            };

            while (true)
            {
                Console.Write("args >");
                var line = Console.ReadLine();
                program.Arguments = new[] { line };

                try
                {
                    program.Run();
                }
                catch (ExitProgramException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            return 0;
        }

        public Program()
            : base("demo", "Demo program", true, "1.0.2")
        {
        }

        protected override ILexer CreateLexer() => new DemoLexer();

        protected override IReadOnlyList<ICliAddIn> GetAddIns()
        {
            return new ICliAddIn[]
            {
                new DbAddIn(this),
            };
        }
    }
}
