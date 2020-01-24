using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.Exceptions;
using TauCode.Cli.TextClasses;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli
{
    public class CliWorkerNodeFactory : NodeFactoryBase
    {
        #region Fields

        private readonly IDictionary<string, Func<FallbackNode, IToken, IResultAccumulator, bool>> _fallbackPredicates;

        #endregion

        #region Constructor

        public CliWorkerNodeFactory(
            string nodeFamilyName,
            IDictionary<string, Func<FallbackNode, IToken, IResultAccumulator, bool>> fallbackPredicates = null)
            : base(
                nodeFamilyName,
                new List<ITextClass>
                {
                    IntegerTextClass.Instance,
                    KeyTextClass.Instance,
                    PathTextClass.Instance,
                    TermTextClass.Instance,
                    StringTextClass.Instance,
                    UrlTextClass.Instance,
                },
                true)
        {
            _fallbackPredicates = fallbackPredicates != null
                ? fallbackPredicates.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value)
                : new Dictionary<string, Func<FallbackNode, IToken, IResultAccumulator, bool>>();
        }


        #endregion

        #region Overridden

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName().ToLowerInvariant();
            if (car == "worker")
            {
                INode workerNode;

                var workerName = item.GetSingleKeywordArgument<Symbol>(":worker-name", true)?.Name;
                if (workerName == null)
                {
                    workerNode = new IdleNode(this.NodeFamily, $"Root node for unnamed worker of type {this.GetType().FullName}");
                }
                else
                {
                    workerNode = new MultiTextNode(
                        new string[] { item.GetSingleKeywordArgument<StringAtom>(":verb").Value },
                        new ITextClass[]
                        {
                            TermTextClass.Instance,
                        },
                        true,
                        WorkerAction,
                        this.NodeFamily,
                        $"Worker Node. Name: [{workerName}]");

                    workerNode.Properties["worker-name"] = workerName;
                }

                return workerNode;
            }

            var node = base.CreateNode(item);

            if (node == null)
            {
                throw new CliException($"Could not build node for item '{car}'.");
            }

            if (node is FallbackNode)
            {
                return node;
            }

            if (!(node is ActionNode))
            {
                throw new CliException($"'{typeof(ActionNode).Name}' instance was expected to be created.");
            }

            var baseResult = (ActionNode)node;

            var action = item.GetSingleKeywordArgument<Symbol>(":action", true)?.Name?.ToLowerInvariant();
            string alias;

            switch (action)
            {
                case "key":
                    baseResult.Action = KeyAction;
                    alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;
                    baseResult.Properties["alias"] = alias;
                    break;

                case "value":
                    baseResult.Action = ValueAction;
                    break;

                case "option":
                    baseResult.Action = OptionAction;
                    alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;
                    baseResult.Properties["alias"] = alias;
                    break;

                case "argument":
                    baseResult.Action = ArgumentAction;
                    alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;
                    baseResult.Properties["alias"] = alias;
                    break;


                default:
                    throw new CliException($"Keyword ':action' is missing or invalid for item '{car}'.");
            }

            return baseResult;
        }

        protected override Func<FallbackNode, IToken, IResultAccumulator, bool> CreateFallbackPredicate(string nodeName)
        {
            if (nodeName == null)
            {
                throw new ArgumentNullException(nameof(nodeName), "Cannot resolve fallback predicated for unnamed node.");
            }

            var key = nodeName.ToLowerInvariant();

            var predicate = _fallbackPredicates.GetOrDefault(key);
            if (predicate == null)
            {
                throw new CliException($"Fallback predicate not found for node '{key}'.");
            }

            return predicate;
        }


        #endregion

        #region Node Actions

        private void WorkerAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            resultAccumulator.EnsureWorkerCommand(node.Properties["worker-name"]);
        }

        private void KeyAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.EnsureWorkerCommand();
            var alias = node.Properties["alias"];
            var key = TokenToKey(token);

            var entry = CliCommandEntry.CreateKeyValuePair(alias, key);
            command.Entries.Add(entry);
        }

        private void ValueAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.GetLastResult<CliCommand>();
            var entry = command.Entries.Last();
            var textToken = (TextToken)token;
            entry.SetKeyValue(textToken.Text);
        }

        private void OptionAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.GetLastResult<CliCommand>();

            var alias = node.Properties["alias"];
            var key = TokenToKey(token);

            var entry = CliCommandEntry.CreateOption(alias, key);
            command.Entries.Add(entry);
        }

        private void ArgumentAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.EnsureWorkerCommand();

            var alias = node.Properties["alias"];
            var argument = TokenToArgument(token);
            var entry = CliCommandEntry.CreateArgument(alias, argument);
            command.Entries.Add(entry);
        }


        #endregion

        #region Misc

        private static string TokenToKey(IToken token)
        {
            var textToken = (TextToken)token;
            if (textToken.Class is KeyTextClass)
            {
                return textToken.Text;
            }

            throw new CliException($"Token '{token}' of type '{token.GetType().FullName}' cannot be converted to key.");
        }


        private string TokenToArgument(IToken token)
        {
            var textToken = (TextToken)token;
            return textToken.Text;
        }

        #endregion
    }
}
