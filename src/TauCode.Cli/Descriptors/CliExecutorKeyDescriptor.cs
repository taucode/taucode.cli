﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Cli.Descriptors
{
    public class CliExecutorKeyDescriptor
    {
        public CliExecutorKeyDescriptor(
            string alias,
            IEnumerable<string> keys,
            bool isMandatory,
            bool allowsMultiple,
            IEnumerable<string> values,
            string valueDescription,
            string docSubstitution)
        {
            this.Alias = alias ?? throw new ArgumentNullException(nameof(alias));

            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var keyList = keys.ToList();
            if (!keyList.Any())
            {
                throw new ArgumentException($"'{nameof(keys)}' cannot be empty.");
            }

            if (keyList.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(keys)}' cannot contain nulls.");
            }

            if (keyList.Distinct().Count() != keyList.Count)
            {
                throw new ArgumentException($"'{nameof(keys)}' must contain unique values.");
            }

            this.Keys = keyList;

            this.IsMandatory = isMandatory;
            this.AllowsMultiple = allowsMultiple;

            this.ValueDescriptor = new CliExecutorValueDescriptor(values, valueDescription, docSubstitution);
        }

        public string Alias { get; }
        public IReadOnlyList<string> Keys { get; }
        public bool IsMandatory { get; }
        public bool AllowsMultiple { get; }
        public CliExecutorValueDescriptor ValueDescriptor { get; }
    }
}
