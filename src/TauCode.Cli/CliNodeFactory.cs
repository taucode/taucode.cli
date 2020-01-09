using System;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.Data.Entries;
using TauCode.Cli.Exceptions;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;

namespace TauCode.Cli
{
    public class CliNodeFactory : NodeFactoryBase
    {
        #region Constructor

        public CliNodeFactory(string nodeFamilyName)
            : base(nodeFamilyName)
        {
        }

        #endregion

        #region Overridden

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName();
            INode node;

            switch (car)
            {
                case "WORKER":
                    node = this.CreateWorkerNode(item);
                    break;

                case "KEY-WITH-VALUE":
                    node = this.CreateKeyEqualsValueNode(item);
                    break;

                case "KEY-VALUE-PAIR":
                    node = this.CreateKeyValuePairNode(item);
                    break;

                case "KEY":
                    node = this.CreateKeyNode(item);
                    break;

                case "PATH":
                    node = this.CreatePathNode(item);
                    break;

                default:
                    throw new CliException($"Unexpected symbol: '{car}'");
            }

            return node;
        }

        #endregion

        #region Node Creators

        private INode CreateWorkerNode(PseudoList item)
        {
            var verbs = item
                .GetAllKeywordArguments(":verbs", true)
                .Select(x => ((StringAtom)x).Value)
                .ToList();

            INode node;
            string workerName;

            if (verbs.Any())
            {
                node = new MultiTextNode(
                    verbs,
                    new ITextClass[] { TermTextClass.Instance },
                    this.ProcessWorkerName,
                    this.NodeFamily,
                    item.GetItemName());

                workerName = item.GetSingleKeywordArgument<Symbol>(":worker-name").Name;
            }
            else
            {
                node = new IdleNode(this.NodeFamily, item.GetItemName());
                workerName = item.GetSingleKeywordArgument<Symbol>(":worker-name", true)?.Name;

                if (workerName != null)
                {
                    throw new NotImplementedException(); // error. if worker-name is present, there should be verbs, and vice versa
                }
            }

            node.Properties["worker-name"] = workerName;
            return node;
        }

        private INode CreatePathNode(PseudoList item)
        {
            var alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;

            var node = new TextNode(
                PathTextClass.Instance,
                this.ProcessPath,
                this.NodeFamily,
                item.GetItemName());
            node.Properties["alias"] = alias;

            return node;
        }

        private INode CreateKeyNode(PseudoList item)
        {
            var alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;

            var keyNames = item
                .GetAllKeywordArguments(":key-names")
                .Select(x => ((StringAtom)x).Value)
                .ToList();

            var node = new MultiTextNode(
                keyNames,
                new ITextClass[] { KeyTextClass.Instance, },
                this.ProcessKey,
                this.NodeFamily,
                item.GetItemName());
            node.Properties["alias"] = alias;

            return node;
        }

        private INode CreateKeyEqualsValueNode(PseudoList item)
        {
            var alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;

            var keyNames = item
                .GetAllKeywordArguments(":key-names")
                .Select(x => ((StringAtom)x).Value)
                .ToList();

            ActionNode keyNameNode = new MultiTextNode(
                keyNames,
                new ITextClass[] { KeyTextClass.Instance },
                this.ProcessKeySucceededByValue,
                this.NodeFamily,
                item.GetItemName());
            keyNameNode.Properties["alias"] = alias;

            INode equalsNode = new ExactPunctuationNode('=', null, this.NodeFamily, null);
            INode choiceNode = this.CreateKeyChoiceNode(item);

            keyNameNode.EstablishLink(equalsNode);
            equalsNode.EstablishLink(choiceNode);

            return keyNameNode;
        }

        private INode CreateKeyValuePairNode(PseudoList item)
        {
            var alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;

            var keyNames = item
                .GetAllKeywordArguments(":key-names")
                .Select(x => ((StringAtom)x).Value)
                .ToList();

            ActionNode keyNameNode = new MultiTextNode(
                keyNames,
                new ITextClass[] { KeyTextClass.Instance },
                this.ProcessKeySucceededByValue,
                this.NodeFamily,
                item.GetItemName());
            keyNameNode.Properties["alias"] = alias;

            INode choiceNode = this.CreateKeyChoiceNode(item);

            keyNameNode.EstablishLink(choiceNode);

            return keyNameNode;
        }

        private INode CreateKeyChoiceNode(PseudoList item)
        {
            var keyValuesSubform = item.GetSingleKeywordArgument(":key-values");
            if (keyValuesSubform.GetCarSymbolName() != "CHOICE")
            {
                throw new CliException("'CHOICE' symbol expected.");
            }

            var classes = keyValuesSubform.GetAllKeywordArguments(":classes").ToList();
            var values = keyValuesSubform.GetAllKeywordArguments(":values").ToList();

            var anyText = values.Count == 1 && values.Single().Equals(Symbol.Create("*"));
            string[] textValues;

            if (anyText)
            {
                textValues = null;
            }
            else
            {
                textValues = values.Select(x => ((StringAtom)x).Value).ToArray();
            }

            var textClasses = classes.Select(x => this.ParseTextClass(((Symbol)x).Name));

            INode choiceNode;

            if (textValues == null)
            {
                choiceNode = new TextNode(
                    textClasses,
                    ProcessKeyChoice,
                    this.NodeFamily,
                    null);
            }
            else
            {
                choiceNode = new MultiTextNode(
                    textValues,
                    textClasses,
                    ProcessKeyChoice,
                    this.NodeFamily,
                    null);
            }

            return choiceNode;
        }

        #endregion

        #region Node Actions

        private void ProcessWorkerName(ActionNode actionNode, IToken token, IResultAccumulator resultAccumulator)
        {
            if (resultAccumulator.Count == 0)
            {
                // no command yet
                var command = new CliCommand
                {
                    AddInName = null, // Add-in is obviously unnamed since there was no command pushed to result accumulator
                    WorkerName = actionNode.Properties["worker-name"],
                };
                resultAccumulator.AddResult(command);
            }
            else
            {
                var command = resultAccumulator.GetLastResult<CliCommand>();
                command.WorkerName = actionNode.Properties["worker-name"];
            }
        }

        private void ProcessPath(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            CliCommand command;
            if (resultAccumulator.Count == 0)
            {
                command = new CliCommand
                {
                    AddInName = null,
                    WorkerName = null,
                };
            }
            else
            {
                command = resultAccumulator.GetLastResult<CliCommand>();
            }

            var textToken = (TextToken)token;
            var entry = new PathEntry
            {
                Alias = node.Properties["alias"],
                Path = textToken.Text,
            };
            command.Entries.Add(entry);
        }

        private void ProcessKey(ActionNode actionNode, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.GetLastResult<CliCommand>();
            var entry = new KeyCliCommandEntry
            {
                Alias = actionNode.Properties["alias"],
                Key = ((TextToken)token).Text,
            };
            command.Entries.Add(entry);
        }

        private void ProcessKeySucceededByValue(ActionNode actionNode, IToken token, IResultAccumulator resultAccumulator)
        {
            var subCommand = resultAccumulator.GetLastResult<CliCommand>();
            var entry = new KeyValueCliCommandEntry
            {
                Alias = actionNode.Properties["alias"],
                Key = ((TextToken)token).Text,
            };
            subCommand.Entries.Add(entry);
        }

        private void ProcessKeyChoice(ActionNode actionNode, IToken token, IResultAccumulator resultAccumulator)
        {
            var subCommand = resultAccumulator.GetLastResult<CliCommand>();
            var entry = (KeyValueCliCommandEntry)subCommand.Entries.Last();
            entry.Value = ((TextToken)token).Text;
        }

        #endregion

        #region Misc

        private ITextClass ParseTextClass(string textClassSymbolName)
        {
            switch (textClassSymbolName)
            {
                case "STRING":
                    return StringTextClass.Instance;

                case "TERM":
                    return TermTextClass.Instance;

                case "KEY":
                    return KeyTextClass.Instance;

                case "PATH":
                    return PathTextClass.Instance;

                default:
                    throw new CliException($"Unexpected text class designating symbol: '{textClassSymbolName}'.");
            }
        }

        #endregion
    }
}
