using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Cli.Descriptors
{
    public class CliWorkerValueDescriptor
    {
        public CliWorkerValueDescriptor(
            IEnumerable<string> values,
            string description,
            string docSubstitution)
        {
            if (values != null)
            {
                var valueList = values.ToList();
                if (!valueList.Any())
                {
                    throw new ArgumentException($"'{nameof(values)}' cannot be empty.");
                }

                if (valueList.Any(x => x == null))
                {
                    throw new ArgumentException($"'{nameof(values)}' cannot contain nulls.");
                }

                if (valueList.Distinct().Count() != valueList.Count)
                {
                    throw new ArgumentException($"'{nameof(values)}' must contain unique values.");
                }

                this.Values = new HashSet<string>(valueList);
            }

            this.Description = description;
            this.DocSubstitution = docSubstitution;
        }

        public HashSet<string> Values { get; }
        public string Description { get; }
        public string DocSubstitution { get; }
    }
}
