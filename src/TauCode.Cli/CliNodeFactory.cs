using System;
using System.Linq;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli
{
    public class CliNodeFactory : NodeFactory
    {
        public CliNodeFactory(string nodeFamilyName) : base(nodeFamilyName)
        {
        }

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName();
            INode node;

            switch (car)
            {
                case "SUB-COMMAND":
                    node = this.CreateSubCommandNode(item);
                    break;

                case "KEY-WITH-VALUE":
                    node = this.CreateKeyWithValueNode(item);
                    break;

                case "KEY":
                    node = this.CreateKeyNode(item);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return node;
        }

        private INode CreateKeyNode(PseudoList item)
        {
            var todo = item.ToString();

            var keyNames = item
                .GetAllKeywordArguments(":key-names")
                .Select(x => ((StringAtom)x).Value)
                .ToList();

            var node = new MultiTextRepresentationNodeLab(
                keyNames,
                new ITextClass[] {KeyTextClass.Instance, },
                this.ProcessKeyAction,
                this.NodeFamily,
                item.GetItemName());

            return node;
        }

        private void ProcessKeyAction(IToken arg1, IResultAccumulator arg2)
        {
            throw new NotImplementedException();
        }

        private INode CreateSubCommandNode(PseudoList item)
        {
            var value = item.GetSingleKeywordArgument<StringAtom>(":value");
            var name = item.GetItemName();

            INode node = new ExactTextNode(
                value.Value,
                TermTextClass.Instance,
                this.AddSubCommandAction,
                this.NodeFamily,
                name);

            return node;
        }

        private INode CreateKeyWithValueNode(PseudoList item)
        {
            var textVariants = new string[] { "todo1", "todo2" };

            ActionNode keyNameNode = new MultiTextRepresentationNodeLab(
                textVariants,
                new ITextClass[] { KeyTextClass.Instance },
                this.AddKeyWithNodeKeyAction,
                this.NodeFamily,
                item.GetItemName());
            INode equalsNode = new ExactPunctuationNode('=', null, this.NodeFamily, null);
            INode choiceNode = this.CreateChoiceNode(item);

            keyNameNode.EstablishLink(equalsNode);
            equalsNode.EstablishLink(choiceNode);

            return keyNameNode;
        }

        private INode CreateChoiceNode(PseudoList item)
        {
            var keyValuesSubform = item.GetSingleKeywordArgument(":key-values");
            if (keyValuesSubform.GetCarSymbolName() != "CHOICE")
            {
                throw new NotImplementedException();
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
                textValues = values.Select(x => ((StringAtom)x).Value).ToArray(); // todo: try/catch.
            }

            var classTypes = classes.Select(x => this.ParseTextClass(((Symbol)x).Name));

            INode choiceNode = new MultiTextRepresentationNodeLab(
                textValues,
                classTypes,
                WatTodo,
                this.NodeFamily,
                null);

            return choiceNode;
        }

        private void WatTodo(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
        }

        private ITextClass ParseTextClass(string textClassSymbolName)
        {
            throw new NotImplementedException();
        }

        private void AddKeyWithNodeKeyAction(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
        }

        private void AddSubCommandAction(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
        }
    }
}
