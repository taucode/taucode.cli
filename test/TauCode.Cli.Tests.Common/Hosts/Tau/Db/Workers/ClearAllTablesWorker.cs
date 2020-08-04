using System;
using System.Collections.Generic;
using TauCode.Cli.TextClasses;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db.Workers
{
    public class ClearAllTablesWorker : CommonWorker
    {
        public ClearAllTablesWorker()
            : base(
                typeof(DbAddIn).Assembly.GetResourceText("ClearAllTables.lisp", true),
                null,
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
    }
}
