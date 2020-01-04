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

namespace TauCode.Cli
{
    public abstract class CliProgramBase : ICliProgram
    {
        private string[] _arguments;
        private readonly string _version;

        private IParser _parser;
        private ILexer _inputLexer;
        private TextWriter _textWriter;

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
        }

        protected abstract IReadOnlyList<ICliAddIn> GetAddIns();

        protected virtual IParser CreateParser() => new CliParser();

        protected virtual ILexer CreateLexer() => new CliLexer();

        protected ILexer Lexer => _inputLexer ?? (_inputLexer = this.CreateLexer());
        protected IParser Parser => _parser ?? (_parser = this.CreateParser());

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
                _root = this.BuildRoot();
            }

            var input = string.Join(" ", this.Arguments);
            var tokens = this.Lexer.Lexize(input);
            var command = (CliCommand)this.Parser.Parse(_root, tokens).Single();

            if (command.AddInName == "VersionGetter")
            {
                this.Output.WriteLine(this.GetVersion());
                return 0;
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

        private INode BuildRoot()
        {
            var addIns = this.GetAddIns();

            var nodeFamily = new NodeFamily("todo-program-family-name");
            var root = new IdleNode(nodeFamily, "<program root>");

            if (this.GetVersion() != null)
            {
                var versionNode = new ExactTextNode(
                    "--version",
                    KeyTextClass.Instance,
                    (node, token, resultAccumulator) => throw new StopParsingExceptionLab("Version requested", this.GetVersion()),
                    nodeFamily,
                    null);

                root.EstablishLink(versionNode);
            }

            foreach (var addIn in addIns)
            {
                var addInNode = new ExactTextNode(
                    addIn.Name,
                    TermTextClass.Instance,
                    this.ProcessAddInName,
                    nodeFamily,
                    null); // todo: give it a name

                addInNode.Properties["add-in-name"] = addIn.Name;
                root.EstablishLink(addInNode);

                var processors = addIn.GetProcessors();
                foreach (var processor in processors)
                {
                    addInNode.EstablishLink(processor.Node);
                }
            }

            return root;
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
