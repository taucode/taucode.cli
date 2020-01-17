﻿using System;

namespace TauCode.Cli.Data
{
    // todo arrange + regions
    public class CliCommandEntry
    {
        private CliCommandEntry(CliCommandEntryKind kind, string alias, string key, string value)
        {
            this.Kind = kind;
            this.Alias = alias ?? throw new ArgumentNullException(nameof(alias));
            this.Key = key;
            this.Value = value;
        }

        public CliCommandEntryKind Kind { get; }
        public string Alias { get; }
        public string Key { get; }
        public string Value { get; }

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
            throw new NotImplementedException();
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

        public void SetKeyValue(string value)
        {
            throw new NotImplementedException();
        }
    }
}
