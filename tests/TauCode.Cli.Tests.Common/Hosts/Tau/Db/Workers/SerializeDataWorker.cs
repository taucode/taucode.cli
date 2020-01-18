using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Cli.Data;
using TauCode.Cli.TextClasses;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db.Workers
{
    public class SerializeDataWorker : CliWorkerBase
    {
        public SerializeDataWorker()
            : base(
                typeof(SerializeDataWorker).Assembly.GetResourceText(".Tau.Db.SerializeData.lisp", true),
                "sd-1.0",
                true)
        {
        }

        protected override CliWorkerNodeFactory CreateNodeFactory()
        {
            return new CliWorkerNodeFactory(
                this.CreateNodeFactoryName(),
                new Dictionary<string, Func<FallbackNode, IToken, IResultAccumulator, bool>>
                {
                    ["bad-option-or-key"] = BadOptionOrKey,
                });
        }

        private bool BadOptionOrKey(FallbackNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is TextToken textToken)
            {
                return textToken.Class is KeyTextClass;
            }

            return false;
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            this.Output.WriteLine("Serialize Data");
            var connection = entries.GetArgument("connection-string");
            var provider = entries.GetSingleKeyValue("provider");
            var excludedTables = entries.GetKeyValues("exclude-table");

            var sb = new StringBuilder();
            sb.Append($"Provider: {provider}; ");
            sb.Append($"Excluded Tables: {string.Join(", ", excludedTables)}; ");
            sb.Append($"Connection String: {connection}; ");

            if (entries.ContainsOption("verbose"))
            {
                sb.Append($"Verbose; ");
            }
            
            this.Output.WriteLine(sb.ToString());
        }
    }
}
