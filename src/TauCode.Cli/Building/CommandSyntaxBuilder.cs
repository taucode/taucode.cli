using System;
using System.Collections.Generic;
using System.Linq;


namespace TauCode.Cli.Building
{
    public class CommandSyntaxBuilder
    {
        #region Fields

        private readonly List<NamedParameterSyntaxBuilder> _namedParameterSyntaxBuilders;

        #endregion

        #region Constructor

        internal CommandSyntaxBuilder(RootSyntaxBuilder rootSyntaxBuilder, string name)
        {
            this.SyntaxBuilder = rootSyntaxBuilder;
            this.Name = name;
            _namedParameterSyntaxBuilders = new List<NamedParameterSyntaxBuilder>();
        }

        #endregion

        #region Internal

        internal NamedParameterSyntaxBuilder AddNamedParameterImpl(string name, params string[] aliases)
        {
            this.CheckCanModify();

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (aliases.Length == 0)
            {
                throw new ArgumentException("Aliases cannot be empty.", nameof(aliases));
            }

            if (aliases.Any(x => x == null))
            {
                throw new ArgumentException("Alias cannot be null.", nameof(aliases));
            }

            foreach (var alias in aliases)
            {
                if (!alias.IsValidAlias())
                {
                    throw new ArgumentException($"Invalid alias: '{alias}'", nameof(aliases));
                }
            }

            var dups = aliases
                .GroupBy(x => x)
                .Where(x => x.Count() > 1)
                .ToList();

            if (dups.Count > 0)
            {
                throw new ArgumentException($"Duplicate alias: '{dups[0].Key}'.", nameof(aliases));
            }

            var parameterSyntaxBuilder = new NamedParameterSyntaxBuilder(this, name, aliases);
            _namedParameterSyntaxBuilders.Add(parameterSyntaxBuilder);

            return parameterSyntaxBuilder;
        }

        #endregion

        #region Public

        public RootSyntaxBuilder SyntaxBuilder { get; }

        public string Name { get; }

        public IReadOnlyList<NamedParameterSyntaxBuilder> NamedParameterBuilders => _namedParameterSyntaxBuilders;

        #endregion
    }
}
