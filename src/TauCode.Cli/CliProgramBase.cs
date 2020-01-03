using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;

namespace TauCode.Cli
{
    public abstract class CliProgramBase : ICliProgram
    {
        private string[] _arguments;
        private readonly string _version;
        private readonly ICliAddIn[] _addIns;
        private readonly ILexer _tinyLispLexer;
        private readonly IParser _parser;
        private readonly ILexer _inputLexer;
        private readonly TinyLispPseudoReader _tinyLispPseudoReader;

        private INode _root;

        protected CliProgramBase(
            string name,
            string description,
            bool supportsHelp,
            string version,
            ICliAddIn[] addIns)
        {
            _arguments = new string[] { };
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Description =
                description ??
                throw new ArgumentNullException(nameof(description)); // todo: need this at all? use <help>!
            this.SupportsHelp = supportsHelp;
            _version = version;
            _addIns = addIns.ToArray(); // todo checks
            _parser = new ParserLab();
            _inputLexer = new CliLexer();
            _tinyLispLexer = new TinyLispLexer();
            _tinyLispPseudoReader = new TinyLispPseudoReader();
        }

        protected IReadOnlyList<ICliAddIn> GetAddIns() => _addIns;

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
            var tokens = _inputLexer.Lexize(input);
            var results = _parser.Parse(_root, tokens);

            throw new NotImplementedException();

            //return 1488;
        }

        private INode BuildRoot()
        {
            var addIns = this.GetAddIns();

            var nodeFamily = new NodeFamily("todo-program-family-name");
            var root = new IdleNode(nodeFamily, "<program root>");

            foreach (var addIn in addIns)
            {
                var addInNode = new ExactTextNode(
                    addIn.Name,
                    TermTextClass.Instance,
                    null,
                    nodeFamily,
                    null); // todo: give it a name

                root.EstablishLink(addInNode);

                var processors = addIn.Processors;
                foreach (var processor in processors)
                {
                    //var processorNode = processor.BuildNode();
                    var processorNode = this.BuildProcessorNode(processor.GetType().FullName, processor.GetGrammar());
                    addInNode.EstablishLink(processorNode);
                }
            }

            return root;
        }

        private INode BuildProcessorNode(string processorTag, string grammar)
        {
            var lispTokens = _tinyLispLexer.Lexize(grammar);
            var form = _tinyLispPseudoReader.Read(lispTokens);
            INodeFactory nodeFactory = new CliNodeFactory(processorTag); // todo: multi-use of node factory. NodeFamily {get;set;}.
            IBuilder builder = new Builder();
            INode processorNode = builder.Build(nodeFactory, form);
            return processorNode;
        }

        public TextWriter Output { get; set; }
    }
}
