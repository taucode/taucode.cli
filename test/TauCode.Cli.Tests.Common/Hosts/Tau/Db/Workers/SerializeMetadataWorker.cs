using System;
using System.Collections.Generic;
using TauCode.Cli.TextClasses;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db.Workers
{
    public class SerializeMetadataWorker : CommonWorker
    {
        public SerializeMetadataWorker()
            : base(
                typeof(DbAddIn).Assembly.GetResourceText("SerializeMetadata.lisp", true),
                null,
                true)
        {
        }

        protected override CliExecutorNodeFactory CreateNodeFactory()
        {
            return new CliExecutorNodeFactory(
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
