using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cli.Commands;
using TauCode.Cli.Exceptions;
using TauCode.Parsing;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;

namespace TauCode.Cli
{
    public abstract class CliHostBase : CliFunctionalityProviderBase, ICliHost
    {
        #region Nested

        private class AddInRecord
        {
            private readonly Dictionary<string, ICliExecutor> _executors;
            private readonly ICliExecutor _singleUnnamedExecutor;

            public AddInRecord(ICliAddIn addIn, IReadOnlyCollection<ICliExecutor> executors)
            {
                this.AddIn = addIn;

                if (executors.Count == 0)
                {
                    throw new CliException("Add-in cannot have zero executors."); // actually, will never happen; CliAddInBase checks it.
                }

                if (executors.Any(x => x.Name == null))
                {
                    if (executors.Count > 1)
                    {
                        // actually, won't ever get here, since this thing is checked by CliAddInBase.
                        throw new CliException("Add-in can have either exactly one nameless executor, or more than one executors all named.");
                    }

                    _singleUnnamedExecutor = executors.Single();
                }
                else
                {
                    _executors = executors.ToDictionary(
                        x => x.Name,
                        x => x);
                }
            }

            public ICliAddIn AddIn { get; }

            public ICliExecutor GetExecutor(string executorName)
            {
                if (executorName == null)
                {
                    if (_singleUnnamedExecutor == null)
                    {
                        throw new CliException("Internal error.");
                    }

                    return _singleUnnamedExecutor;
                }

                return _executors[executorName];
            }
        }

        #endregion

        #region Fields

        private TextWriter _output;
        private TextReader _input;

        private ILexer _lexer;
        private IParser _parser;
        private readonly INodeFamily _nodeFamily;

        private readonly Dictionary<string, AddInRecord> _addInRecords;
        private AddInRecord _singleUnnamedAddInRecord;

        private List<ICliAddIn> _addInList;
        private Dictionary<INode, ICliExecutor> _nodesByExecutorss;

        #endregion

        #region Constructor

        protected CliHostBase(
            string name,
            string version,
            bool supportsHelp)
            : base(name, version, supportsHelp)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name)); // host cannot be nameless.
            }

            _nodeFamily = new NodeFamily($"Node family for host '{this.Name}'");
            _addInRecords = new Dictionary<string, AddInRecord>();

            _output = TextWriter.Null;
            _input = TextReader.Null;
        }

        #endregion

        #region Private

        private AddInRecord GetAddInRecord(string addInName)
        {
            if (addInName == null)
            {
                if (_singleUnnamedAddInRecord == null)
                {
                    throw new CliException("This host supports only named add-ins.");
                }

                return _singleUnnamedAddInRecord;
            }

            var record = _addInRecords.GetValueOrDefault(addInName);
            if (record == null)
            {
                throw new CliException($"Add-in not found: '{addInName}'.");
            }

            return record;
        }

        #endregion

        #region Protected

        protected ILexer Lexer => _lexer ??= this.CreateLexer();

        protected IParser Parser => _parser ??= this.CreateParser();

        protected virtual ILexer CreateLexer() => new CliLexer();

        protected virtual IParser CreateParser() => new Parser
        {
            WantsOnlyOneResult = true,
            Root = this.Node,
        };

        protected abstract IReadOnlyList<ICliAddIn> CreateAddIns();

        protected IReadOnlyDictionary<INode, ICliExecutor> NodesByExecutors => _nodesByExecutorss ??= CreateNodesByExecutors();

        protected Dictionary<INode, ICliExecutor> CreateNodesByExecutors()
        {
            var result = new Dictionary<INode, ICliExecutor>();

            var addIns = this.GetAddIns();
            foreach (var addIn in addIns)
            {
                var executors = addIn.GetExecutors();
                foreach (var executor in executors)
                {
                    var executorRoot = executor.Node;
                    var executorTree = executorRoot.FetchTree().Where(x => !(x is EndNode || x is IdleNode));

                    foreach (var node in executorTree)
                    {
                        result.Add(node, executor);
                    }
                }
            }

            return result;
        }

        #endregion

        #region Overridden

        protected override void OnNodeCreated()
        {
            if (this.Version != null)
            {
                this.AddVersion();
            }

            if (this.SupportsHelp)
            {
                this.AddHelp();
            }
        }

        protected override INode CreateNodeTree()
        {
            var addIns = this.CreateAddIns();
            if (addIns == null)
            {
                throw new CliException($"'{nameof(CreateAddIns)}' must not return null.");
            }

            if (addIns.Count == 0)
            {
                _addInList = new List<ICliAddIn>();
                var dummyRoot = new IdleNode(_nodeFamily, $"Dummy root node of empty host '{this.Name}'");
                dummyRoot.EstablishLink(EndNode.Instance);

                return dummyRoot;
            }

            var validTypes = addIns.All(x => x is CliAddInBase);
            if (!validTypes)
            {
                throw new CliException($"'{nameof(CreateAddIns)}' must return instances of type '{typeof(CliAddInBase).FullName}'.");
            }

            if (addIns.Any(x => x.Name == null))
            {
                if (addIns.Count > 1)
                {
                    throw new CliException($"'{nameof(CreateAddIns)}' must return either all add-ins having non-null name, or exactly one add-in with null name.");
                }

                var singleUnnamedAddIn = addIns.Single();
                _singleUnnamedAddInRecord = new AddInRecord(singleUnnamedAddIn, singleUnnamedAddIn.GetExecutors());
            }

            foreach (var addIn in addIns)
            {
                ((CliAddInBase)addIn).Host = this;
            }

            var root = new IdleNode(_nodeFamily, $"Root node of host '{this.Name}'");

            if (_singleUnnamedAddInRecord == null)
            {
                // all add-ins are named
                foreach (var addIn in addIns)
                {
                    root.EstablishLink(addIn.Node);

                    var record = new AddInRecord(
                        addIn,
                        addIn.GetExecutors());
                    _addInRecords.Add(addIn.Name, record);
                }

                _addInList = _addInRecords
                    .Values
                    .Select(x => x.AddIn)
                    .ToList();
            }
            else
            {
                root.EstablishLink(_singleUnnamedAddInRecord.AddIn.Node);

                _addInList = new List<ICliAddIn>
                {
                    _singleUnnamedAddInRecord.AddIn,
                };
            }

            return root;
        }

        public sealed override TextWriter Output
        {
            get => _output;
            set => _output = value ?? TextWriter.Null;
        }

        public sealed override TextReader Input
        {
            get => _input;
            set => _input = value ?? TextReader.Null;
        }

        protected override string GetHelpImpl()
        {
            if (_singleUnnamedAddInRecord != null)
            {
                return _singleUnnamedAddInRecord.AddIn.GetHelp();
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine("Available add-ins:");
                foreach (var record in _addInRecords.Values)
                {
                    var addIn = record.AddIn;
                    sb.AppendLine(addIn.Name);
                }

                return sb.ToString();
            }
        }

        #endregion

        #region ICliHost Members

        public IReadOnlyList<ICliAddIn> GetAddIns()
        {
            var dummyNode = this.Node; // make node-build happen
            return _addInList;
        }

        public CliCommand ParseCommand(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var tokens = this.Lexer.Lexize(input);

            try
            {
                var command = (CliCommand)this.Parser.Parse(tokens).Single();
                return command;
            }
            catch (FallbackNodeAcceptedTokenException ex)
            {
                var executor = this.NodesByExecutors[ex.FallbackNode];

                FallbackInterceptedCliException interceptEx;

                try
                {
                    interceptEx = executor.HandleFallback(ex);
                }
                catch (Exception executorEx)
                {
                    throw new CliException(
                        $"Executor's '{nameof(ICliExecutor.HandleFallback)}' thrown an exception when requested to handle fallback. Executor name: '{executor.Name}'. Executor type: '{executor.GetType().FullName}'.",
                        executorEx);
                }

                if (interceptEx == null)
                {
                    throw new CliException(
                        $"Executor's '{nameof(ICliExecutor.HandleFallback)}' returned null when requested to handle fallback. Executor name: '{executor.Name}'. Executor type: '{executor.GetType().FullName}'.");
                }

                throw interceptEx;
            }
        }

        public void DispatchCommand(CliCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var addInRecord = this.GetAddInRecord(command.AddInName);
            var executor = addInRecord.GetExecutor(command.ExecutorName);

            executor.Process(command.Entries);
        }

        public Task DispatchCommandAsync(CliCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var addInRecord = this.GetAddInRecord(command.AddInName);
            var executor = addInRecord.GetExecutor(command.ExecutorName);

            return executor.ProcessAsync(command.Entries, cancellationToken);
        }

        #endregion
    }
}
