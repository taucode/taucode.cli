using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TauCode.Cli.Commands;
using TauCode.Cli.Descriptors;
using TauCode.Cli.Exceptions;
using TauCode.Cli.Help;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli
{
    public static class CliExtensions
    {
        #region Misc

        private static ITextClass GetVerbTextClass(string tokenText)
        {
            try
            {
                ILexer lexer = new CliLexer();
                var tokens = lexer.Lexize(tokenText);

                do
                {
                    if (tokens.Count != 1)
                    {
                        break;
                    }

                    var token = tokens.Single();
                    if (token is TextToken textToken)
                    {
                        var textClass = textToken.Class;
                        if (textClass is TermTextClass || textClass is KeyTextClass)
                        {
                            return textClass;
                        }
                    }

                } while (false);

                throw new CliException("Verb for custom handler must be term or key.");

            }
            catch (Exception ex)
            {
                throw new CliException($"Invalid verb for custom handler: '{tokenText}'.", ex);
            }
        }
        
        private static INodeFamily CheckArgumentsAndGetOrCreateFamily(
            this ICliFunctionalityProvider functionality)
        {
            if (functionality == null)
            {
                throw new ArgumentNullException(nameof(functionality));
            }

            if (functionality.Name == null)
            {
                throw new CliException("Cannot add custom handler to a nameless functionality.");
            }

            var familyName = $"Family for custom handler nodes for functionality '{functionality.Name}' of type '{functionality.GetType().FullName}'.";

            var links = functionality.Node.ResolveLinks()
                .Where(x => x.Family?.Name == familyName)
                .ToList();

            var nodeFamily = links.Any() ? links.First().Family : new NodeFamily(familyName);
            return nodeFamily;
        }

        #endregion

        #region Custom Handlers Support

        public static ICliFunctionalityProvider AddCustomHandlerWithParameter(
            this ICliFunctionalityProvider functionalityProvider,
            Action<IToken> handler,
            string tokenText)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (tokenText == null)
            {
                throw new ArgumentNullException(nameof(tokenText));
            }

            var family = CheckArgumentsAndGetOrCreateFamily(functionalityProvider);
            var verbClass = GetVerbTextClass(tokenText);
            var commandNode = new ExactTextNode(
                tokenText,
                verbClass,
                true,
                null,
                family,
                $"Custom handler node for verb '{tokenText}'");

            var argumentNode = new CustomActionNode(
                (node, token, resultAccumulator) =>
                {
                    handler(token);
                    throw new CliCustomHandlerException();
                },
                (token, resultAccumulator) => true,
                family,
                $"Argument node for custom handler with verb '{tokenText}'.");

            functionalityProvider.Node.EstablishLink(commandNode);
            commandNode.EstablishLink(argumentNode);

            return functionalityProvider;
        }

        public static ICliFunctionalityProvider AddCustomHandler(
            this ICliFunctionalityProvider functionalityProvider,
            Action<ICliFunctionalityProvider> handler,
            string tokenText)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (tokenText == null)
            {
                throw new ArgumentNullException(nameof(tokenText));
            }

            var family = CheckArgumentsAndGetOrCreateFamily(functionalityProvider);
            var verbClass = GetVerbTextClass(tokenText);
            var commandNode = new ExactTextNode(
                tokenText,
                verbClass,
                true,
                (node, token, resultAccumulator) =>
                {
                    handler(functionalityProvider);
                    throw new CliCustomHandlerException();
                },
                family,
                $"Custom handler node for verb '{tokenText}'");

            functionalityProvider.Node.EstablishLink(commandNode);
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
                (x) => functionalityProvider.Output.WriteLine(x.Version),
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
                (x) => functionalityProvider.Output.WriteLine(x.GetHelp()),
                "--help");
        }

        public static ICliAddIn AddShellExit(this ICliAddIn addIn, string exitTokenText = "exit")
        {
            addIn.AddCustomHandler(x => throw new ExitShellException(), exitTokenText);
            return addIn;
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

        public static string[] GetArguments(this IEnumerable<CliCommandEntry> entries, string argumentAlias)
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

            var arguments = entries
                .Where(x =>
                    x.Kind == CliCommandEntryKind.Argument &&
                    string.Equals(x.Alias, argumentAlias, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Value)
                .ToArray();

            return arguments;
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

        public static CliCommandEntry[] GetKeyEntries(this IEnumerable<CliCommandEntry> entries, string keyAlias)
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
                .ToArray();
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

        public static CliCommand EnsureExecutorCommand(this IResultAccumulator resultAccumulator, string executorName)
        {
            if (resultAccumulator == null)
            {
                throw new ArgumentNullException(nameof(resultAccumulator));
            }

            if (resultAccumulator.Count == 0)
            {
                var command = CliCommand.CreateExecutorCommand(executorName);
                resultAccumulator.AddResult(command);
                return command;
            }
            else
            {
                var command = resultAccumulator.GetLastResult<CliCommand>();
                command.SetExecutorName(executorName);
                return command;
            }
        }

        public static CliCommand EnsureExecutorCommand(this IResultAccumulator resultAccumulator)
        {
            if (resultAccumulator == null)
            {
                throw new ArgumentNullException(nameof(resultAccumulator));
            }

            if (resultAccumulator.Count == 0)
            {
                var command = CliCommand.CreateNamelessExecutorCommand();
                resultAccumulator.AddResult(command);
                return command;
            }
            else
            {
                var command = resultAccumulator.GetLastResult<CliCommand>();
                return command;
            }
        }

        //public static CliCommand ParseLine(this ICliHost host, string line)
        //{
        //    if (host == null)
        //    {
        //        throw new ArgumentNullException(nameof(host));
        //    }

        //    if (line == null)
        //    {
        //        throw new ArgumentNullException(nameof(line));
        //    }

        //    return host.ParseCommand(new[] { line });
        //}

        #endregion

        #region Help

        public static string GetHelp(this CliExecutorDescriptor descriptor)
        {
            var margin = 20;
            var maxLength = 20;

            var sb = new StringBuilder();
            var helpBuilder = new HelpBuilder();

            sb.AppendLine(descriptor.Description);
            if (descriptor.UsageSamples.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Usage samples:");
                foreach (var usageSample in descriptor.UsageSamples)
                {
                    sb.AppendLine(usageSample);
                }
            }

            if (descriptor.Keys.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Keys:");

                foreach (var key in descriptor.Keys)
                {
                    sb.Append(string.Join(", ", key.Keys));

                    var docSubstitution = key.ValueDescriptor.DocSubstitution ?? $"{key.Alias}";

                    sb.Append($" <{docSubstitution}>");

                    helpBuilder.WriteHelp(sb, key.ValueDescriptor.Description, margin, maxLength);
                }
            }

            if (descriptor.Arguments.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Arguments:");
                foreach (var argument in descriptor.Arguments)
                {
                    var docSubstitution = argument.DocSubstitution ?? $"{argument.Alias}";

                    sb.Append($"<{docSubstitution}>");

                    helpBuilder.WriteHelp(sb, argument.Description, margin, maxLength);
                }
            }

            if (descriptor.Options.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Options:");
                foreach (var option in descriptor.Options)
                {
                    sb.Append(string.Join(", ", option.Options));

                    helpBuilder.WriteHelp(sb, option.Description, margin, maxLength);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
