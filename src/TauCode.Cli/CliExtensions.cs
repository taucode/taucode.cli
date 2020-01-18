using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.Exceptions;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli
{
    public static class CliExtensions
    {
        #region Nested

        private class CatchAllAndThrowNode : ActionNode
        {
            public CatchAllAndThrowNode(Action<IToken> handler, INodeFamily family, string name)
                : base(BuildAction(handler), family, name)
            {
            }

            private static Action<ActionNode, IToken, IResultAccumulator> BuildAction(Action<IToken> handler)
            {
                void Result(ActionNode dummyActionNode, IToken token, IResultAccumulator resultAccumulator)
                {
                    handler(token);
                    throw new CliException("Custom handler hasn't thrown an exception while it was expected to.");
                }

                return Result;
            }

            protected override bool AcceptsTokenImpl(IToken token, IResultAccumulator resultAccumulator) => true;
        }


        #endregion

        #region Custom Handlers Support

        public static ICliFunctionalityProvider AddCustomHandlerWithParameter(
            this ICliFunctionalityProvider functionalityProvider,
            Action<IToken> handler,
            params string[] texts)
        {
            // todo: a lot of copy/paste (see AddCustomHandler method)
            if (functionalityProvider == null)
            {
                throw new ArgumentNullException(nameof(functionalityProvider));
            }

            if (functionalityProvider.Name == null)
            {
                throw new CliException("Cannot add custom handler to a nameless functionality.");
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (texts == null)
            {
                throw new ArgumentNullException(nameof(texts));
            }

            if (texts.Length == 0)
            {
                throw new ArgumentException($"'{nameof(texts)}' cannot be empty.");
            }

            var tokens = new List<TextToken>();
            ITextClass textClass;

            try
            {
                ILexer lexer = new CliLexer();

                foreach (var text in texts)
                {
                    var singleTextTokens = lexer.Lexize(text);
                    var isValid =
                        singleTextTokens.Count == 1 &&
                        singleTextTokens.Single() is TextToken;

                    if (!isValid)
                    {
                        throw new NotImplementedException(); // error.
                    }

                    var token = (TextToken)singleTextTokens.Single();
                    tokens.Add(token);
                }

                var classes = tokens.Select(x => x.Class).Distinct().ToList();
                if (classes.Count > 1)
                {
                    throw new NotImplementedException(); // error.
                }

                textClass = classes.Single();

                var decorations = tokens.Select(x => x.Decoration).Distinct().ToList();
                if (decorations.Count != 1)
                {
                    throw new NotImplementedException(); // error.
                }

                if (decorations.Single() != NoneTextDecoration.Instance)
                {
                    throw new NotImplementedException(); // error.
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Bad texts. todo", ex);
            }

            INodeFamily nodeFamily = new NodeFamily("dummy"); // todo

            var commandNode = new MultiTextNode(
                tokens.Select(x => x.Text),
                new[] { textClass },
                true,
                null,
                nodeFamily,
                null);

            var argumentNode = new CatchAllAndThrowNode(handler, nodeFamily, null);

            functionalityProvider.Node.EstablishLink(commandNode);
            commandNode.EstablishLink(argumentNode);

            return functionalityProvider;
        }

        public static ICliFunctionalityProvider AddCustomHandler(
            this ICliFunctionalityProvider functionalityProvider,
            Action action,
            params string[] texts)
        {
            if (functionalityProvider == null)
            {
                throw new ArgumentNullException(nameof(functionalityProvider));
            }

            if (functionalityProvider.Name == null)
            {
                throw new CliException("Cannot add custom handler to a nameless functionality.");
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (texts == null)
            {
                throw new ArgumentNullException(nameof(texts));
            }

            if (texts.Length == 0)
            {
                throw new ArgumentException($"'{nameof(texts)}' cannot be empty.");
            }

            var tokens = new List<TextToken>();
            ITextClass textClass;

            try
            {
                ILexer lexer = new CliLexer();

                foreach (var text in texts)
                {
                    var singleTextTokens = lexer.Lexize(text);
                    var isValid =
                        singleTextTokens.Count == 1 &&
                        singleTextTokens.Single() is TextToken;

                    if (!isValid)
                    {
                        throw new NotImplementedException(); // error.
                    }

                    var token = (TextToken)singleTextTokens.Single();
                    tokens.Add(token);
                }

                var classes = tokens.Select(x => x.Class).Distinct().ToList();
                if (classes.Count > 1)
                {
                    throw new NotImplementedException(); // error.
                }

                textClass = classes.Single();

                var decorations = tokens.Select(x => x.Decoration).Distinct().ToList();
                if (decorations.Count != 1)
                {
                    throw new NotImplementedException(); // error.
                }

                if (decorations.Single() != NoneTextDecoration.Instance)
                {
                    throw new NotImplementedException(); // error.
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Bad texts. todo", ex);
            }

            INodeFamily nodeFamily = new NodeFamily("dummy"); // todo

            var node = new MultiTextNode(
                tokens.Select(x => x.Text),
                new[] { textClass },
                true,
                (actionNode, token, resultAccumulator) =>
                {
                    action();
                    throw new CliCustomHandlerException();
                },
                nodeFamily,
                null);

            functionalityProvider.Node.EstablishLink(node);
            return functionalityProvider;
        }

        public static ICliFunctionalityProvider AddVersion(this ICliFunctionalityProvider functionalityProvider)
        {
            if (functionalityProvider == null)
            {
                throw new ArgumentNullException(nameof(functionalityProvider));
            }

            if (functionalityProvider.Version == null)
            {
                throw new ArgumentException(
                    $"Functionality provider '{functionalityProvider.Name}' doesn't support version.",
                    nameof(functionalityProvider));
            }

            return functionalityProvider.AddCustomHandler(
                () => functionalityProvider.Output.WriteLine(functionalityProvider.Version),
                "--version");
        }

        public static ICliFunctionalityProvider AddHelp(this ICliFunctionalityProvider functionalityProvider)
        {
            if (functionalityProvider == null)
            {
                throw new ArgumentNullException(nameof(functionalityProvider));
            }

            if (!functionalityProvider.SupportsHelp)
            {
                throw new ArgumentException(
                    $"Functionality provider '{functionalityProvider.Name}' doesn't support help.",
                    nameof(functionalityProvider));
            }

            return functionalityProvider.AddCustomHandler(
                () => functionalityProvider.Output.WriteLine(functionalityProvider.GetHelp()),
                "--help");
        }

        #endregion

        #region Cli Command Contents Support

        public static bool ContainsOption(this IEnumerable<CliCommandEntry> entries, string optionAlias)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            if (optionAlias == null)
            {
                throw new ArgumentNullException(nameof(optionAlias));
            }

            var wantedEntries = entries
                .Where(x =>
                    x.Kind == CliCommandEntryKind.Option &&
                    string.Equals(x.Alias, optionAlias, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            if (wantedEntries.Count == 0)
            {
                return false;
            }

            if (wantedEntries.Count > 1)
            {
                throw new CliException($"Multiple option. Alias: '{optionAlias.ToLowerInvariant()}'.");
            }

            return true;
        }

        public static string GetArgument(this IEnumerable<CliCommandEntry> entries, string argumentAlias)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            if (argumentAlias == null)
            {
                throw new ArgumentNullException(nameof(argumentAlias));
            }

            argumentAlias = argumentAlias.ToLowerInvariant();

            var wantedEntries = entries
                .Where(x =>
                    x.Kind == CliCommandEntryKind.Argument &&
                    string.Equals(x.Alias, argumentAlias, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            if (wantedEntries.Count == 0)
            {
                throw new CliException($"Argument '{argumentAlias}' not found.");
            }

            if (wantedEntries.Count > 1)
            {
                throw new CliException($"Argument '{argumentAlias}' appears more than one time.");
            }

            return wantedEntries.Single().Value;
        }

        public static string GetSingleKeyValue(this IEnumerable<CliCommandEntry> entries, string keyAlias)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            if (keyAlias == null)
            {
                throw new ArgumentNullException(nameof(keyAlias));
            }

            var wantedEntries = entries
                .Where(x =>
                    x.Kind == CliCommandEntryKind.KeyValuePair &&
                    string.Equals(x.Alias, keyAlias, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            if (wantedEntries.Count == 0)
            {
                throw new CliException($"Key is missing. Alias: '{keyAlias.ToLowerInvariant()}'.");
            }

            if (wantedEntries.Count > 1)
            {
                throw new CliException($"Multiple keys. Alias: '{keyAlias.ToLowerInvariant()}'.");
            }

            return wantedEntries.Single().Value;
        }

        public static string[] GetKeyValues(this IEnumerable<CliCommandEntry> entries, string keyAlias)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            if (keyAlias == null)
            {
                throw new ArgumentNullException(nameof(keyAlias));
            }

            return entries
                .Where(x =>
                    x.Kind == CliCommandEntryKind.KeyValuePair &&
                    string.Equals(x.Alias, keyAlias, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Value)
                .ToArray();
        }

        public static string[] GetAllOptionAliases(this IEnumerable<CliCommandEntry> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            var groups = entries
                .Where(x => x.Kind == CliCommandEntryKind.Option)
                .Select(x => x.Alias.ToLowerInvariant())
                .GroupBy(x => x)
                .ToList();

            var badGroup = groups.FirstOrDefault(x => x.Count() > 1);
            if (badGroup != null)
            {
                throw new CliException($"Multiple option. Alias: '{badGroup.Key}'.");
            }

            return groups
                .Select(x => x.Key)
                .ToArray();
        }

        public static Tuple<string, string>[] GetAllArguments(this IEnumerable<CliCommandEntry> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            return entries
                .Where(x => x.Kind == CliCommandEntryKind.Argument)
                .Select(x => Tuple.Create(x.Alias.ToLowerInvariant(), x.Value))
                .ToArray();
        }

        public static CliCommand AddAddInCommand(this IResultAccumulator resultAccumulator, string addInName)
        {
            if (resultAccumulator == null)
            {
                throw new ArgumentNullException(nameof(resultAccumulator));
            }

            if (resultAccumulator.Count != 0)
            {
                throw new CliException("Internal error: result accumulator must be empty.");
            }

            var command = CliCommand.CreateAddInCommand(addInName);
            resultAccumulator.AddResult(command);

            return command;
        }

        public static CliCommand EnsureWorkerCommand(this IResultAccumulator resultAccumulator, string workerName)
        {
            if (resultAccumulator == null)
            {
                throw new ArgumentNullException(nameof(resultAccumulator));
            }

            if (resultAccumulator.Count == 0)
            {
                var command = CliCommand.CreateWorkerCommand(workerName);
                resultAccumulator.AddResult(command);
                return command;
            }
            else
            {
                var command = resultAccumulator.GetLastResult<CliCommand>();
                command.SetWorkerName(workerName);
                return command;
            }
        }

        public static CliCommand EnsureWorkerCommand(this IResultAccumulator resultAccumulator)
        {
            if (resultAccumulator == null)
            {
                throw new ArgumentNullException(nameof(resultAccumulator));
            }

            if (resultAccumulator.Count == 1)
            {
                return resultAccumulator.GetLastResult<CliCommand>();
            }

            var command = CliCommand.CreateNamelessWorkerCommand();
            return command;
        }

        public static CliCommand ParseLine(this ICliHost host, string line)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            if (line == null)
            {
                throw new ArgumentNullException(nameof(line));
            }

            return host.ParseCommand(new[] { line });
        }

        #endregion
    }
}
