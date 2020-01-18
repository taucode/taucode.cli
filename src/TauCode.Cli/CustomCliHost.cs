using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.Exceptions;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;

namespace TauCode.Cli
{
    public class CustomCliHost : ICliHost
    {
        #region Fields

        private TextWriter _output;
        private TextReader _input;

        private ILexer _lexer;
        private IParser _parser;

        private INode _node;
        private readonly INodeFamily _nodeFamily;

        #endregion

        #region Constructor

        public CustomCliHost(string name)
        {
            this.Name = name;

            _output = TextWriter.Null;
            _input = TextReader.Null;

            _nodeFamily = new NodeFamily($"Node family for empty host '{this.Name}'");
        }

        #endregion

        #region Protected

        protected ILexer Lexer => _lexer ?? (_lexer = this.CreateLexer());

        protected IParser Parser => _parser ?? (_parser = this.CreateParser());

        protected virtual ILexer CreateLexer() => new CliLexer();

        protected virtual IParser CreateParser() => new Parser
        {
            WantsOnlyOneResult = true,
            Root = this.Node,
        };

        protected virtual INode BuildNode()
        {
            var node = new IdleNode(_nodeFamily, "Idle root of empty host");
            return node;
        }

        #endregion

        #region ICliHost Members

        public IReadOnlyList<ICliAddIn> GetAddIns() => new List<ICliAddIn>(); // never has add-ins

        public CliCommand ParseCommand(params string[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(input)}' cannot contain nulls.", nameof(input));
            }

            var inputString = string.Join(" ", input);
            var tokens = this.Lexer.Lexize(inputString);

            // it is expected that some node will throw CliCustomHandlerException or other custom exception.
            this.Parser.Parse(tokens);

            // should not get here
            throw new CliException(
                $"'{nameof(ParseCommand)}' is expected to throw an instance of '{typeof(CliCustomHandlerException).FullName}'.");
        }

        public void DispatchCommand(CliCommand command) =>
            throw new NotSupportedException($"Use custom handlers, or override '{nameof(BuildNode)}'.");

        #endregion

        #region ICliFunctionalityProvider Members

        public string Name { get; }

        public TextWriter Output
        {
            get => _output;
            set => _output = value ?? TextWriter.Null;
        }

        public TextReader Input
        {
            get => _input;
            set => _input = value ?? TextReader.Null;
        }

        public INode Node
        {
            get
            {
                if (_node == null)
                {
                    _node = this.BuildNode();
                }

                return _node;
            }
        }

        public string Version => null;

        public bool SupportsHelp => false;

        public string GetHelp() => throw new NotSupportedException(
            "Default implementation does not support this method. Introduce help manually if you need it.");

        #endregion
    }
}
