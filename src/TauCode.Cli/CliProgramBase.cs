using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TauCode.Cli
{
    public abstract class CliProgramBase
    {
        protected CliProgramBase(
            TextReader input,
            TextWriter output,
            bool supportsShell)
        {
            this.Input = input;
            this.Output = output;
            this.SupportsShell = supportsShell;
        }


        protected abstract ICliHost CreateHost();

        protected ICliHost Host { get; private set; }

        public TextReader Input { get; }

        public TextWriter Output { get; }

        public bool SupportsShell { get; }

        public int Run(string[] args)
        {
            this.Host = this.CreateHost();

            if (this.SupportsShell)
            {
                foreach (var addIn in this.Host.GetAddIns())
                {
                    addIn.AddCustomHandler(x => throw new ShellRequestedException(x), "--shell");
                }
            }

            try
            {
                var command = this.Host.ParseCommand(args);
                this.Host.DispatchCommand(command);
            }
            catch (ShellRequestedException ex)
            {
                this.RunShell(ex.FunctionalityProvider as ICliAddIn);
            }
            catch (Exception ex)
            {
                this.Output.WriteLine(ex);
                return -1;
            }

            return 0;
        }

        private void RunShell(ICliAddIn addIn)
        {
            addIn.InitContext();

            while (true)
            {
                var prompt = this.MakePrompt(addIn);

                this.Output.Write(prompt);
                var input = this.Input.ReadLine() ?? string.Empty;

                if (input.Trim() == string.Empty)
                {
                    continue;
                }

                var fullInput = $"{addIn.Name} {input}";


                var command = this.Host.ParseCommand(new[] { fullInput });
                this.Host.DispatchCommand(command);
            }
        }

        private string MakePrompt(ICliAddIn addIn)
        {
            var sb = new StringBuilder();

            sb.Append("[");
            sb.Append(this.Host.Name);
            sb.Append(" ");
            sb.Append(addIn.Name);
            sb.Append("] ");

            sb.Append(addIn.Context);

            sb.Append(" >");

            return sb.ToString();
        }

        public Task<int> RunAsync(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
