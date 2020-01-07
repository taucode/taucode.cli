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
    // todo clean up
    public abstract class CliHostBase : ICliHost
    {
        #region Fields

        private TextWriter _output;
        private TextReader _input;

        #endregion

        #region Nested

        private class AddInRecord
        {
            private readonly Dictionary<string, ICliWorker> _workers;

            public AddInRecord(/*INode node,*/ ICliAddIn addIn, IEnumerable<ICliWorker> workers)
            {
                //this.Node = node;
                this.AddIn = addIn;
                _workers = workers
                    .ToDictionary(x => x.Name, x => x);
            }

            //public INode Node { get; } // todo: need this?
            public ICliAddIn AddIn { get; }

            public ICliWorker GetWorker(string workerName)
            {
                return _workers[workerName];
            }
        }

        #endregion

        #region Fields

        private ILexer _lexer;
        private IParser _parser;
        private INode _node;
        private readonly INodeFamily _nodeFamily;

        private readonly IDictionary<string, AddInRecord> _addIns;
        private List<ICliAddIn> _addInList;

        #endregion

        #region Constructor

        protected CliHostBase(string name, string version, bool supportsHelp)
        {
            this.Name = name;
            this.Version = version;
            this.SupportsHelp = supportsHelp;

            _nodeFamily = new NodeFamily($"Node family for host '{this.Name}'");
            _addIns = new Dictionary<string, AddInRecord>();

            _output = TextWriter.Null;
            _input = TextReader.Null;

        }

        #endregion

        #region Private

        private INode BuildNode()
        {
            var addIns = this.CreateAddIns();
            if (addIns == null)
            {
                throw new CliException($"'{nameof(CreateAddIns)}' must not return null.");
            }

            if (addIns.Count == 0)
            {
                throw new CliException($"'{nameof(CreateAddIns)}' must not return empty collection.");
            }

            var validTypes = addIns.All(x => x is CliAddInBase);
            if (!validTypes)
            {
                throw new CliException($"'{nameof(CreateAddIns)}' must return instances of type '{typeof(CliAddInBase).FullName}'.");
            }

            if (addIns.Any(x => x.Name == null) && addIns.Count > 1)
            {
                throw new CliException($"'{nameof(CreateAddIns)}' must return either all add-ins having non-null name, or exactly one add-in with null name.");
            }

            foreach (var addIn in addIns)
            {
                ((CliAddInBase)addIn).Host = this;
            }

            var root = new IdleNode(_nodeFamily, $"Root node of host '{this.Name}'");

            foreach (var addIn in addIns)
            {
                var node = addIn.Node;
                root.EstablishLink(node);

                var record = new AddInRecord(
                    //node,
                    addIn,
                    addIn.GetWorkers());
                _addIns.Add(addIn.Name, record);
            }

            _addInList = _addIns
                .Values
                .Select(x => x.AddIn)
                .ToList();

            return root;
        }

        #endregion

        #region Protected

        protected ILexer Lexer => _lexer ?? (_lexer = this.CreateLexer());
        protected IParser Parser => _parser ?? (_parser = this.CreateParser());

        protected virtual ILexer CreateLexer() => new CliLexer();

        protected virtual IParser CreateParser() => new Parser();

        protected abstract IReadOnlyList<ICliAddIn> CreateAddIns();

        #endregion

        #region ICliHost Members

        public IReadOnlyList<ICliAddIn> GetAddIns()
        {
            var dummyNode = this.Node; // make node-build happen
            return _addInList;
        }

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

            var command = (CliCommand)this.Parser.Parse(this.Node, tokens).Single();
            return command;
        }

        public void DispatchCommand(CliCommand command)
        {
            var addInRecord = _addIns[command.AddInName];
            var worker = addInRecord.GetWorker(command.WorkerName);

            worker.Process(command.Entries);
        }

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

                    if (this.Version != null)
                    {
                        this.AddVersion();
                    }

                    if (this.SupportsHelp)
                    {
                        this.AddHelp();
                    }
                }

                return _node;
            }
        }

        public string Version { get; }

        public bool SupportsHelp { get; }

        public string GetHelp()
        {
            return "todo: help for host";
        }

        #endregion
    }
}
