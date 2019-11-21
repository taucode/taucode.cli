using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Cli.Building
{
    public class RootSyntaxBuilder
    {
        #region Static

        private static readonly HashSet<char> ValidCommandChars;
        private static readonly HashSet<char> ValidParameterChars;

        static RootSyntaxBuilder()
        {
            var list = new List<char>();
            list.AddRange(Enumerable.Range('a', 'z' - 'a' + 1).Select(x => (char)x));
            list.AddRange(Enumerable.Range('0', '9' - '0' + 1).Select(x => (char)x));
            list.Add('_');
            list.Add('-');
            ValidCommandChars = new HashSet<char>(list);

            list.AddRange(Enumerable.Range('A', 'Z' - 'A' + 1).Select(x => (char)x));
            ValidParameterChars = new HashSet<char>(list);
        }

        public static bool IsValidCommandChar(char c)
        {
            return ValidCommandChars.Contains(c);
        }

        public static bool IsValidParameterChar(char c)
        {
            return ValidParameterChars.Contains(c);
        }

        #endregion

        #region Fields

        private readonly List<CommandSyntaxBuilder> _commandSyntaxBuilders;

        #endregion

        #region Constructor

        public RootSyntaxBuilder()
        {
            _commandSyntaxBuilders = new List<CommandSyntaxBuilder>();
        }

        #endregion

        #region Intenral

        internal CommandSyntaxBuilder AddCommandImpl(string name)
        {
            this.CheckCanModify();

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var wantedName = name.Trim().ToLowerInvariant();
            var isValidName =
                wantedName == name &&
                wantedName.Length > 0 &&
                name.All(IsValidCommandChar);

            if (!isValidName)
            {
                throw new ArgumentException($"Bad command name: '{name}'.", nameof(name));
            }

            var commandBuilder = new CommandSyntaxBuilder(this, name);
            _commandSyntaxBuilders.Add(commandBuilder);
            return commandBuilder;
        }

        internal CommandSyntaxBuilder AddDefaultCommandImpl()
        {
            this.CheckCanModify();
            var commandSyntaxBuilder = new CommandSyntaxBuilder(this, null);
            _commandSyntaxBuilders.Add(commandSyntaxBuilder);
            return commandSyntaxBuilder;
        }

        #endregion

        #region Public

        public IReadOnlyList<CommandSyntaxBuilder> CommandSyntaxBuilders => _commandSyntaxBuilders;

        public void Validate()
        {
            // watch for default commands.
            if (_commandSyntaxBuilders.Any(x => x.Name == null))
            {
                if (_commandSyntaxBuilders.Count > 1)
                {
                    throw new SyntaxException("If a default command is supplied, no other commands can be added.");
                }
            }

            // watch for command dups.
            var dups = _commandSyntaxBuilders
                .GroupBy(x => x.Name)
                .Where(x => x.Count() > 1)
                .ToList();

            if (dups.Count > 0)
            {
                throw new SyntaxException($"Duplicate command: '{dups[0].Key}'.");
            }

            // watch for param name dups & alias dups.
            foreach (var commandSyntaxBuilder in _commandSyntaxBuilders)
            {
                var names = commandSyntaxBuilder.NamedParameterBuilders.Select(x => x.Name).ToList();
                var nameDups = names
                    .GroupBy(x => x)
                    .Where(x => x.Count() > 1)
                    .ToList();

                if (nameDups.Count > 0)
                {
                    throw new SyntaxException($"Duplicate parameter name: '{nameDups[0].Key}'.");
                }

                var list = commandSyntaxBuilder.NamedParameterBuilders.SelectMany(x => x.Aliases);
                var aliasDups = list
                    .GroupBy(x => x)
                    .Where(x => x.Count() > 1)
                    .ToList();

                if (aliasDups.Count > 0)
                {
                    throw new SyntaxException($"Duplicate alias: '{aliasDups[0].Key}'.");
                }
            }
        }

        public void Complete()
        {
            this.CheckCanModify();
            this.Validate();

            this.IsCompleted = true;
        }

        public bool IsCompleted { get; internal set; }

        #endregion
    }
}
