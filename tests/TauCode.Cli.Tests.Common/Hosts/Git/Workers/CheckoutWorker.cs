using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.TextClasses;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tests.Common.Hosts.Git.Workers
{
    public class CheckoutWorker : CliWorkerBase
    {
        public CheckoutWorker()
            : base(
                typeof(CheckoutWorker).Assembly.GetResourceText(".Git.NoName.Checkout.lisp", true),
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
                    ["bad-key-fallback"] = BadKeyFallback
                });
        }

        protected override INode CreateNodeTree()
        {
            var root = base.CreateNodeTree();

            var allNodes = root.FetchTree();
            var node = (ActionNode)allNodes.Single(x => string.Equals(x.Name, "existing-branch-node", StringComparison.InvariantCultureIgnoreCase));
            node.AdditionalChecker = (token, accumulator) => !token.ToString().StartsWith("-");

            return root;
        }

        public override void HandleFallback(FallbackNodeAcceptedTokenException ex)
        {
            this.Output.WriteLine($"Bad key or option: {ex.Token}.");
        }

        private bool BadKeyFallback(FallbackNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is TextToken textToken)
            {
                return textToken.Class is KeyTextClass;
            }

            return false;
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            if (entries.ContainsOption("new-branch"))
            {
                var newBranchName = entries.GetArgument("new-branch-name");
                var baseBranchName = entries.GetArgument("base-branch-name");

                this.Output.WriteLine($"Checkout new branch: '{newBranchName}' based on '{baseBranchName}'.");
            }
            else
            {
                var branchName = entries.GetArgument("existing-branch");
                var options = entries.GetAllOptionAliases();
                this.Output.WriteLine($"Checkout existing branch: '{branchName}'.");
                this.Output.WriteLine("Options:");
                foreach (var option in options)
                {
                    this.Output.WriteLine(option);
                }
            }
        }
    }
}
