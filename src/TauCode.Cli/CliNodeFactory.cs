using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli
{
    public class CliNodeFactory : NodeFactoryBase
    {
        public CliNodeFactory(
            string nodeFamilyName)
            : base(
                nodeFamilyName,
                new List<ITextClass>
                {
                    KeyTextClass.Instance,
                    PathTextClass.Instance,
                    TermTextClass.Instance,
                    StringTextClass.Instance,
                    UrlTextClass.Instance,
                },
                true)
        {
        }

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName().ToLowerInvariant();
            if (car == "worker")
            {
                INode workerNode;

                var workerName = item.GetSingleKeywordArgument<Symbol>(":worker-name", true)?.Name;
                if (workerName == null)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    workerNode = new MultiTextNode(
                        item
                            .GetAllKeywordArguments(":verbs")
                            .Cast<StringAtom>()
                            .Select(x => x.Value)
                            .ToList(),
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

            var baseResult = (ActionNode)base.CreateNode(item);
            var action = item.GetSingleKeywordArgument<Symbol>(":action", true)?.Name?.ToLowerInvariant();
            string alias;

            switch (action)
            {
                case "key":
                    baseResult.Action = KeyAction;
                    alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;
                    baseResult.Properties["alias"] = alias;
                    break;

                case "argument":
                    baseResult.Action = ArgumentAction;
                    alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;
                    baseResult.Properties["alias"] = alias;
                    break;

                case "value":
                    baseResult.Action = ValueAction;
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (baseResult == null)
            {
                throw new NotImplementedException();
            }

            return baseResult;
        }

        private void WorkerAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            if (resultAccumulator.Count == 0)
            {
                throw new NotImplementedException();
            }
            else
            {
                var command = resultAccumulator.GetLastResult<CliCommand>();
                command.WorkerName = node.Properties["worker-name"];
            }
        }

        private void KeyAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.GetLastResult<CliCommand>();
            var entry = new CliCommandEntry
            {
                Alias = node.Properties["alias"],
            };
            command.Entries.Add(entry);
        }

        private void ValueAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.GetLastResult<CliCommand>();
            var entry = command.Entries.Last();
            var textToken = (TextToken)token;
            entry.Value = textToken.Text;
        }

        private void ArgumentAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
        }
    }
}
