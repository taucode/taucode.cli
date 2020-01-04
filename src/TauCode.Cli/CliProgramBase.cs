using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Lab.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Cli
{
    public abstract class CliProgramBase : ICliProgram
    {
        private string[] _arguments;
        private readonly string _version;
        private readonly INodeFamily _nodeFamily;

        private IParser _parser;
        private ILexer _inputLexer;
        private TextWriter _textWriter;

        private readonly Dictionary<string, Action> _customHandlers;

        private INode _root;

        protected CliProgramBase(
            string name,
            string description,
            bool supportsHelp,
            string version)
        {
            _arguments = new string[] { };
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Description =
                description ??
                throw new ArgumentNullException(nameof(description)); // todo: need this at all? use <help>!
            this.SupportsHelp = supportsHelp;
            _version = version;
            //_parser = new ParserLab();
            //_inputLexer = new CliLexer();
            _textWriter = TextWriter.Null;
            _customHandlers = new Dictionary<string, Action>();
            _nodeFamily = new NodeFamily("todo-program-family-name");
        }

        protected abstract IReadOnlyList<ICliAddIn> GetAddIns();

        protected virtual IParser CreateParser() => new CliParser();

        protected virtual ILexer CreateLexer() => new CliLexer();

        protected ILexer Lexer => _inputLexer ?? (_inputLexer = this.CreateLexer());
        protected IParser Parser => _parser ?? (_parser = this.CreateParser());

        protected void AddCustomHandler(TextToken token, Action action)
        {
            var customHandlerNode = new ExactTextNode(
                token.Text,
                token.Class,
                (node, tokenArg, resultAccumulator) =>
                {
                    throw new StopParsingExceptionLab("Custom Handler Requested", ((TextToken)tokenArg).Text);
                },
                _nodeFamily,
                null);

            _root.EstablishLink(customHandlerNode);
            _customHandlers.Add(token.Text, action);
        }

        public string Name { get; }

        public string[] Arguments
        {
            get => _arguments;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.Any(x => x == null))
                {
                    throw new ArgumentException($"'{nameof(Arguments)}' cannot contain nulls.");
                }

                _arguments = value;
            }
        }

        public string Description { get; }
        public bool SupportsHelp { get; }
        public string GetVersion() => _version;

        public string GetHelp()
        {
            throw new System.NotImplementedException();
        }

        public int Run()
        {
            if (_root == null)
            {
                this.BuildRoot();
            }

            var input = string.Join(" ", this.Arguments);
            var tokens = this.Lexer.Lexize(input);
            var command = (CliCommand)this.Parser.Parse(_root, tokens).Single();

            if (command.AddInName == "<CustomHandler>")
            {
                var action = _customHandlers[command.ProcessorAlias];

                try
                {
                    action();
                    return 0;
                }
                catch (Exception e)
                {
                    this.Output.WriteLine(e);
                    return -1;
                }
            }

            // todo: lines below might throw (?)
            var addIn = this.GetAddIns().Single(x => x.Name == command.AddInName);
            var processor = addIn.GetProcessors().Single(x => x.Alias == command.ProcessorAlias);

            try
            {
                processor.Process(command.Entries);
                return 0;
            }
            catch (Exception e)
            {
                this.Output.WriteLine(e);
                return -1;
            }
        }

        private void BuildRoot()
        {
            var addIns = this.GetAddIns();
            _root = new IdleNode(_nodeFamily, "<program root>");

            if (this.GetVersion() != null)
            {
                this.AddCustomHandler(
                    new TextToken(KeyTextClass.Instance, NoneTextDecoration.Instance, "--version"),
                    () =>
                    {
                        this.Output.WriteLine(this.GetVersion());
                    });

                //var versionNode = new ExactTextNode(
                //    "--version",
                //    KeyTextClass.Instance,
                //    (node, token, resultAccumulator) => throw new StopParsingExceptionLab("Version requested", this.GetVersion()),
                //    _nodeFamily,
                //    null);

                //root.EstablishLink(versionNode);
            }

            foreach (var addIn in addIns)
            {
                var addInNode = new ExactTextNode(
                    addIn.Name,
                    TermTextClass.Instance,
                    this.ProcessAddInName,
                    _nodeFamily,
                    null); // todo: give it a name

                addInNode.Properties["add-in-name"] = addIn.Name;
                _root.EstablishLink(addInNode);

                var processors = addIn.GetProcessors();
                foreach (var processor in processors)
                {
                    addInNode.EstablishLink(processor.Node);
                }
            }
        }

        private void ProcessAddInName(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = new CliCommand
            {
                AddInName = node.Properties["add-in-name"],
            };

            resultAccumulator.AddResult(command);
        }

        public TextWriter Output
        {
            get => _textWriter;
            set => _textWriter = value ?? TextWriter.Null;
        }
    }
}
