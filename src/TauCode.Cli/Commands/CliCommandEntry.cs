using System;

namespace TauCode.Cli.Commands
{
    public class CliCommandEntry
    {
        #region Constructor

        private CliCommandEntry(CliCommandEntryKind kind, string alias, string key, string value)
        {
            this.Kind = kind;
            this.Alias = alias ?? throw new ArgumentNullException(nameof(alias));
            this.Key = key;
            this.Value = value;
        }

        #endregion

        #region Public

        public CliCommandEntryKind Kind { get; }
        public string Alias { get; }
        public string Key { get; }
        public string Value { get; private set; } // todo: rename to public string StringValue { get; private set; }

        // todo: public IToken Token { get; private set; }

        public void SetKeyValue(string value)
        {
            this.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        #endregion

        #region Static

        public static CliCommandEntry CreateOption(string alias, string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var entry = new CliCommandEntry(CliCommandEntryKind.Option, alias, key, null);
            return entry;
        }

        public static CliCommandEntry CreateKeyValuePair(string alias, string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return new CliCommandEntry(CliCommandEntryKind.KeyValuePair, alias, key, null);
        }

        public static CliCommandEntry CreateArgument(string alias, string argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(nameof(argument));
            }

            var entry = new CliCommandEntry(CliCommandEntryKind.Argument, alias, null, argument);
            return entry;
        }


        #endregion
    }
}
