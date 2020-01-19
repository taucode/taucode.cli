using System;
using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Cli.TextClasses;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db.Workers
{
    public class DropAllTablesWorker : CliWorkerBase
    {
        public DropAllTablesWorker()
            : base(
                typeof(DropAllTablesWorker).Assembly.GetResourceText(".Tau.Db.DropAllTables.lisp", true),
                "dat-1.0",
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
            this.Output.WriteLine("Dummy implementation. Get back here when ready.");
        }
    }
}
