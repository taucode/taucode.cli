using System;

namespace TauCode.Cli.Demo
{
    public class Program
    {
        private class ExitException : Exception { }

        private static void Main()
        {
            ICliHost host = new DemoHost();


            while (true)
            {
                Console.Write("args >");
                var line = Console.ReadLine();
                //program.Arguments = new[] { line };

                try
                {
                    //program.Run();
                    var command = host.ParseCommand(line);
                    host.DispatchCommand(command);
                }
                catch (ExitException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }


            //var program = new Program
            //{
            //    Output = Console.Out,
            //};


            //var goOn = true;

            //program.AddCustomHandler(
            //    null,
            //    null,
            //    "cls",
            //    TermTextClass.Instance,
            //    Console.Clear);

            //program.AddCustomHandler(
            //    null,
            //    null,
            //    "exit",
            //    TermTextClass.Instance,
            //    () => throw new ExitException());

            //while (true)
            //{
            //    Console.Write("args >");
            //    var line = Console.ReadLine();
            //    program.Arguments = new[] { line };

            //    try
            //    {
            //        program.Run();
            //    }
            //    catch (ExitException)
            //    {
            //        break;
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex);
            //    }
            //}

            //return 0;
        }

        //public Program()
        //    : base("demo", "Demo program", true, "demo-1.0.2")
        //{
        //}

        //protected override ILexer CreateLexer() => new DemoLexer();

        //protected override IReadOnlyList<ICliAddIn> GetAddIns()
        //{
        //    return new ICliAddIn[]
        //    {
        //        new DbAddIn(this),
        //    };
        //}
    }
}
