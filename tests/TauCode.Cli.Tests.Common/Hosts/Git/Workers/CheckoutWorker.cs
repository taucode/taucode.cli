using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.Exceptions;
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
                    ["bad-option-fallback"] = BadOptionFallback
                });
        }

        protected override INode CreateNodeTree()
        {
            var root = base.CreateNodeTree();

            var allNodes = root.FetchTree();
            var pathNodes = allNodes
                .Where(x => x is TextNode)
                .Cast<TextNodeBase>()
                .Where(x =>
                    x is TextNode textNode &&
                    textNode.TextClasses.Contains(PathTextClass.Instance))
                .Cast<TextNode>()
                .ToList();

            pathNodes.ForEach(x => x.AdditionalChecker = (token, accumulator) => !token.ToString().StartsWith("-"));

            return root;
        }

        public override FallbackInterceptedCliException HandleFallback(FallbackNodeAcceptedTokenException ex)
        {
            return new FallbackInterceptedCliException($"Bad key or option: {ex.Token}.");
        }

        private bool BadOptionFallback(FallbackNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is TextToken textToken)
            {
                return textToken.Class is KeyTextClass;
            }

            return false;
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            this.Output.WriteLine("Git Checkout");
            var options = entries.GetAllOptionAliases();
            this.Output.WriteLine($"Options: {string.Join(", ", options)}");
            this.Output.WriteLine("Arguments:");
            var arguments = entries.GetAllArguments();
            foreach (var argument in arguments)
            {
                this.Output.WriteLine($"{argument.Item1} = {argument.Item2}");
            }
        }
    }
}
