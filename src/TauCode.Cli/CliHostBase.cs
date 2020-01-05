using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;

namespace TauCode.Cli
{
    public abstract class CliHostBase : ICliHost
    {
        #region Nested

        private class AddInRecord
        {
            public AddInRecord(INode node, ICliAddIn addIn/*, IEnumerable<ICliWorker> workers*/)
            {
                this.Node = node;
                this.AddIn = addIn;
                //this.Workers = workers
                //    .ToDictionary(x => x.Name, x => x);
            }

            public INode Node { get; }
            public ICliAddIn AddIn { get; }
            //public IDictionary<string, ICliWorker> Workers { get; }
        }

        #endregion

        #region Fields

        private ILexer _lexer;
        private IParser _parser;
        private INode _node;
        private readonly INodeFamily _nodeFamily;

        private readonly IDictionary<string, AddInRecord> _addIns;

        #endregion

        #region Constructor

        protected CliHostBase()
        {
            _nodeFamily = new NodeFamily("todo-program-family-name");
            _addIns = new Dictionary<string, AddInRecord>();
        }

        #endregion

        #region Private

        private void ProcessAddInName(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = new CliCommand
            {
                AddInName = node.Properties["add-in-name"],
            };

            resultAccumulator.AddResult(command);
        }

        #endregion

        #region Protected

        protected ILexer Lexer => _lexer ?? (_lexer = this.CreateLexer());
        protected IParser Parser => _parser ?? (_parser = this.CreateParser());

        protected virtual ILexer CreateLexer() => new CliLexer();

        protected virtual IParser CreateParser() => new CliParser();

        protected virtual INode BuildNode()
        {
            var addIns = this.CreateAddIns();
            var root = new IdleNode(_nodeFamily, "<program root>");

            foreach (var addIn in addIns)
            {
                //var addInNode = new ExactTextNode(
                //    addIn.Name,
                //    TermTextClass.Instance,
                //    this.ProcessAddInName,
                //    _nodeFamily,
                //    null); // todo: give it a name

                //addInNode.Properties["add-in-name"] = addIn.Name;
                //root.EstablishLink(addInNode);

                //var workers = addIn.CreateWorkers();
                //foreach (var worker in workers)
                //{
                //    addInNode.EstablishLink(worker.Node);
                //}

                var node = addIn.Node;

                var record = new AddInRecord(node, addIn);
                _addIns.Add(addIn.Name, record);
            }

            return root;
        }

        #endregion

        #region ICliHost Members

        public abstract ICliAddIn[] CreateAddIns();

        public CliCommand ParseCommand(params string[] input)
        {
            // todo check args

            var inputString = string.Join(" ", input);
            var tokens = this.Lexer.Lexize(inputString);

            var command = (CliCommand)this.Parser.Parse(this.Node, tokens).Single();
            return command;
        }

        public void DispatchCommand(CliCommand command)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICliFunctionalityProvider Members

        public string Name => throw new NotImplementedException();
        public TextWriter Output => throw new NotImplementedException();
        public TextReader Input => throw new NotImplementedException();
        public INode Node => _node ?? (_node = this.BuildNode());
        public string Version => throw new NotImplementedException();
        public bool SupportsHelp => throw new NotImplementedException();

        public string GetHelp()
        {
            throw new NotImplementedException();
        }
        
        #endregion
    }
}
