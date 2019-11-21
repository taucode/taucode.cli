using System.Collections.Generic;

namespace TauCode.Cli.Building
{
    public class NamedParameterSyntaxBuilder
    {
        #region Fields

        private readonly string[] _aliases;
        private readonly List<ValueSyntaxBuilderBase> _valueBuilders;

        private bool _isMandatory;
        private string _defaultValue;

        #endregion

        #region Constructor

        internal NamedParameterSyntaxBuilder(CommandSyntaxBuilder commandBuilder, string name, string[] aliases)
        {
            this.CommandBuilder = commandBuilder;
            this.Name = name;
            _aliases = aliases;
            _valueBuilders = new List<ValueSyntaxBuilderBase>();
        }

        #endregion

        #region Internal

        internal void AddValueBuilderImpl(ValueSyntaxBuilderBase valueBuilder)
        {
            this.CheckCanModify();
            _valueBuilders.Add(valueBuilder);
        }

        #endregion

        #region Public

        public string Name { get; }

        public IReadOnlyList<string> Aliases => _aliases;

        public IReadOnlyList<ValueSyntaxBuilderBase> ValueBuilders => _valueBuilders;

        public CommandSyntaxBuilder CommandBuilder { get; }

        public bool IsMandatory
        {
            get => _isMandatory;
            internal set
            {
                this.CheckCanModify();
                _isMandatory = value;
            }
        }

        public string DefaultValue
        {
            get => _defaultValue;
            internal set
            {
                this.CheckCanModify();
                _defaultValue = value;
            }
        }

        #endregion
    }
}
