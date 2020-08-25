using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Exceptions;
using TauCode.Cli.TextClasses;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tests.Common.Hosts.Git.Workers
{
    public class CheckoutWorker : CommonWorker
    {
        public CheckoutWorker()
            : base(
                typeof(CheckoutWorker).Assembly.GetResourceText(".Git.NoName.Checkout.lisp", true),
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
    }
}
