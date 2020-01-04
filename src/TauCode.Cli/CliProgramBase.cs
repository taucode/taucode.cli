﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TauCode.Cli.Data;
using TauCode.Cli.Exceptions;
using TauCode.Cli.TextClasses;
using TauCode.Extensions.Lab;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli
{
    // todo clean up
    public abstract class CliProgramBase : ICliProgram
    {
        private string[] _arguments;
        private readonly string _version;
        private readonly INodeFamily _nodeFamily;

        private IParser _parser;
        private ILexer _inputLexer;
        private TextWriter _textWriter;

        private readonly IDictionary<string, Action> _customHandlers;

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
            _textWriter = TextWriter.Null;
            _customHandlers = new Dictionary<string, Action>();
            _nodeFamily = new NodeFamily("todo-program-family-name");
        }

        protected abstract IReadOnlyList<ICliAddIn> GetAddIns();

        protected virtual IParser CreateParser() => new CliParser();

        protected virtual ILexer CreateLexer() => new CliLexer();

        protected ILexer Lexer => _inputLexer ?? (_inputLexer = this.CreateLexer());
        protected IParser Parser => _parser ?? (_parser = this.CreateParser());

        protected INode Root
        {
            get
            {
                if (_root == null)
                {
                    _root = this.BuildRoot();
                    this.EnrichRoot();
                }

                return _root;
            }
        }

        protected void AddCustomHandler(
            string addInName,
            string processorAlias,
            string tokenText,
            ITextClass textClass,
            Action action)
        {
            var customHandlerNode = new ExactTextNode(
                tokenText,
                textClass,
                (nodeArg, tokenArg, resultAccumulator) =>
                {
                    //throw new StopParsingExceptionLab("Custom Handler Requested", ((TextToken)tokenArg).Text);
                    var command = new CliCommand
                    {
                        AddInName = addInName,
                        ProcessorAlias = processorAlias,
                    };

                    throw new CliCustomHandlerException(command);
                },
                _nodeFamily,
                null);

            INode node;

            if (addInName == null)
            {
                node = this.Root;
            }
            else
            {
                throw new NotImplementedException();
            }

            node.EstablishLink(customHandlerNode);
            _customHandlers.Add(tokenText, action);
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
            var sb = new StringBuilder();

            sb.AppendLine("Available add-ins:");

            var addIns = this.GetAddIns();

            foreach (var addIn in addIns)
            {
                var addInName = addIn.Name;
                sb.AppendLine(addInName);
                sb.AppendLine("-".Repeat(addInName.Length));
                sb.AppendLine($"{this.Name} {addInName} <arguments>");
                if (addIn.SupportsHelp)
                {
                    sb.AppendLine($"{this.Name} {addInName} --help"); // todo
                }
            }

            return sb.ToString();
        }

        public int Run()
        {
            if (_root == null)
            {
                this.BuildRoot();
            }

            var input = string.Join(" ", this.Arguments);
            var tokens = this.Lexer.Lexize(input);

            CliCommand command;
            

            try
            {
                command = (CliCommand)this.Parser.Parse(_root, tokens).Single();
            }
            catch (CliCustomHandlerException ex)
            {
                throw new NotImplementedException();

                //var action = _customHandlers.GetOrDefault(ex.HandlerTokenText);
                //if (action == null)
                //{
                //    throw;
                //}

                //action();
                //return 111; // todo
            }

            //if (command.AddInName == "<CustomHandler>")
            //{
            //    var action = _customHandlers[command.ProcessorAlias];

            //    try
            //    {
            //        action();
            //        return 0;
            //    }
            //    catch (Exception e)
            //    {
            //        this.Output.WriteLine(e);
            //        return -1;
            //    }
            //}

            // todo: lines below might throw (?)
            var addIn = this.GetAddIns().Single(x => x.Name == command.AddInName);
            var processor = addIn.GetProcessors().Single(x => x.Alias == command.ProcessorAlias);

            processor.Process(command.Entries);
            return 222; // todo
        }

        private void EnrichRoot()
        {
            if (this.GetVersion() != null)
            {
                this.AddCustomHandler(
                    null,
                    null,
                    "--version",
                    KeyTextClass.Instance,
                    () =>
                    {
                        this.Output.WriteLine(this.GetVersion());
                    });
            }

            if (this.SupportsHelp)
            {
                this.AddCustomHandler(
                    null,
                    null,
                    "--help",
                    KeyTextClass.Instance,
                    () =>
                    {
                        this.Output.WriteLine(this.GetHelp());
                    });
            }
        }

        private INode BuildRoot()
        {
            var addIns = this.GetAddIns();
            var root = new IdleNode(_nodeFamily, "<program root>");

            foreach (var addIn in addIns)
            {
                var addInNode = new ExactTextNode(
                    addIn.Name,
                    TermTextClass.Instance,
                    this.ProcessAddInName,
                    _nodeFamily,
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
