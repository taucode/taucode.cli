using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Exceptions;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Cli
{
    public static class CliExtensions
    {
        public static ICliFunctionalityProvider AddCustomHandler(
            this ICliFunctionalityProvider functionalityProvider,
            Action action,
            params string[] texts)
        {
            if (functionalityProvider == null)
            {
                throw new ArgumentNullException(nameof(functionalityProvider));
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
                throw new Exception("Bad texts. todo", ex);
            }

            INodeFamily nodeFamily = new NodeFamily("dummy"); // todo

            var node = new MultiTextRepresentationNode(
                tokens.Select(x => x.Text),
                new ITextClass[] { textClass },
                token => token.Text,
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
    }
}
