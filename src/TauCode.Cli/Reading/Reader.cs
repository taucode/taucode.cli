using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Building;
using TauCode.Cli.Parsing;
using TauCode.Cli.Parsing.Tokens;

namespace TauCode.Cli.Reading
{
    public class Reader
    {
        private readonly RootSyntaxBuilder _syntaxBuilder;

        private CommandSyntaxBuilder _commandBuilder;
        private List<TokenBase> _commandArguments;
        private int _argumentPos;
        private NamedParameterSyntaxBuilder _currentNamedParameterBuilder;

        private Command _command;

        public Reader(RootSyntaxBuilder syntaxBuilder)
        {
            if (syntaxBuilder == null)
            {
                throw new ArgumentNullException(nameof(syntaxBuilder));
            }

            if (!syntaxBuilder.IsCompleted)
            {
                throw new InvalidOperationException("Cannot use not completed syntax.");
            }

            _syntaxBuilder = syntaxBuilder;
        }

        public Command Read(IReadOnlyList<TokenBase> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            _argumentPos = 0;
            
            if (_syntaxBuilder.HasDefaultCommand())
            {
                _commandBuilder = _syntaxBuilder.CommandSyntaxBuilders.Single();
                _commandArguments = tokens.ToList();
            }
            else
            {
                var commandToken = tokens[0] as ValueToken;
                if (commandToken == null)
                {
                    throw new ReadException($"Invalid command: '{tokens[0].ToString()}'");
                }

                var commandText = commandToken.Value;

                // resolve command
                _commandBuilder = this.ResolveCommandBuilder(commandText);
                _commandArguments = tokens.Skip(1).ToList();
            }

            _command = new Command(_commandBuilder.Name);

            while (true)
            {
                if (this.IsEnd())
                {
                    // no more parameters.
                    this.SupplyDefaultParameters();
                    this.CheckMandatoryParameters();
                    break;
                }

                var token = this.GetCurrToken();

                if (token is ParameterToken)
                {
                    this.ReadNamedParameter();
                }
                else
                {
                    var value = this.ReadParameterValue();
                    _command.AddParameterValue(value);
                }
            }

            return _command;
        }

        private void CheckMandatoryParameters()
        {
            var mandatoryParameterBuilders = _commandBuilder.NamedParameterBuilders.Where(x => x.IsMandatory).ToList();

            foreach (var mandatoryParameterBuilder in mandatoryParameterBuilders)
            {
                if (!_command.CommandContainsParameterWithName(mandatoryParameterBuilder.Name))
                {
                    throw new ReadException($"Value for mandatory named parameter '{mandatoryParameterBuilder.Aliases[0]}' was not provided.");
                }
            }
        }

        private void SupplyDefaultParameters()
        {
            var paramsWithDefault = _commandBuilder.NamedParameterBuilders.Where(x => x.DefaultValue != null).ToList();

            foreach (var parameterBuilder in paramsWithDefault)
            {
                if (!_command.CommandContainsParameterWithName(parameterBuilder.Name))
                {
                    _command.AddNamedParameter(new NamedParameter(parameterBuilder.Name, parameterBuilder.DefaultValue));
                }
            }
        }

        private void ReadNamedParameter()
        {
            var token = (ParameterToken)this.GetCurrToken();
            
            var parameterBuilder = this.GetNamedParameterBuilder(token.ParameterAlias);
            _currentNamedParameterBuilder = parameterBuilder;
            this.MoveNext();

            if (_currentNamedParameterBuilder.RequiresValue())
            {
                if (this.IsEnd())
                {
                    throw new ReadException($"Parameter '{_currentNamedParameterBuilder.Aliases[0]}' requires a value.");
                }

                var value = this.ReadParameterValue();

                if (value == null)
                {
                    throw new ReadException($"Parameter '{_currentNamedParameterBuilder.Aliases[0]}' requires a value.");
                }

                var parameter = new NamedParameter(parameterBuilder.Name, value);
                _command.AddNamedParameter(parameter);
            }
            else
            {
                var parameter = new NamedParameter(parameterBuilder.Name, null);
                _command.AddNamedParameter(parameter);
            }

            _currentNamedParameterBuilder = null;
        }

        private string ReadParameterValue()
        {
            var token = this.GetCurrToken();

            if (token is ParameterToken)
            {
                // hit into another parameter; let's get out here.
                return null;
            }
            else if (token is ValueToken vt)
            {
                var value = vt.Value;
                this.CheckValueIsAcceptable(value);
                this.MoveNext();
                return value;
            }
            else if (token is QuotedStringToken qst)
            {
                var value = qst.UnquotedValue;
                this.CheckValueIsAcceptable(value);
                this.MoveNext();
                return value;
            }

            throw new ReadException("Internal reader error."); // should never get here, actually.
        }

        private void CheckValueIsAcceptable(string value)
        {
            if (_currentNamedParameterBuilder == null)
            {
                return;
            }

            if (ParameterAcceptsValue(_currentNamedParameterBuilder, value))
            {
                return;
            }

            throw new ReadException($"Parameter '{_currentNamedParameterBuilder.Aliases[0]}' won't accept value {value}");
        }

        private static bool ParameterAcceptsValue(NamedParameterSyntaxBuilder parameterBuilder, string value)
        {
            if (parameterBuilder.ValueBuilders.Any(x => x is AnyValueSyntaxBuilder))
            {
                return true;
            }

            var constValues = parameterBuilder
                .ValueBuilders
                .Where(x => x is EnumValueSyntaxBuilder)
                .Cast<EnumValueSyntaxBuilder>()
                .SelectMany(x => x.Values);

            var accepts = constValues.Contains(value);
            return accepts;
        }

        private void MoveNext()
        {
            _argumentPos++;
        }

        private NamedParameterSyntaxBuilder GetNamedParameterBuilder(string parameterAlias)
        {
            var parameterBuilder = _commandBuilder
                .NamedParameterBuilders
                .SingleOrDefault(x => x.Aliases.Contains(parameterAlias));

            if (parameterBuilder == null)
            {
                throw new ReadException($"Unknown parameter: '{parameterAlias}'.");
            }

            return parameterBuilder;
        }

        private TokenBase GetCurrToken()
        {
            return _commandArguments[_argumentPos];
        }

        private bool IsEnd()
        {
            return _argumentPos == _commandArguments.Count;
        }

        private CommandSyntaxBuilder ResolveCommandBuilder(string commandText)
        {
            var commandBuilder = _syntaxBuilder.CommandSyntaxBuilders.SingleOrDefault(x => x.Name == commandText);
            if (commandBuilder == null)
            {
                throw new ReadException($"Unknown command: '{commandText}'.");
            }

            return commandBuilder;
        }
    }
}
