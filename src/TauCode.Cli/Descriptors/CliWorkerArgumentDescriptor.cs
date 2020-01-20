using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Cli.Descriptors
{
    public class CliWorkerArgumentDescriptor
    {
        public CliWorkerArgumentDescriptor(
            string alias,
            IEnumerable<string> values,
            string description,
            string docSubstitution)
        {
            this.Alias = alias ?? throw new ArgumentNullException(nameof(alias));

            if (values != null)
            {
                var valueList = values.ToList();
                if (!valueList.Any())
                {
                    throw new NotImplementedException(); // todo
                }

                if (valueList.Any(x => x == null))
                {
                    throw new NotImplementedException(); // todo
                }

                if (valueList.Distinct().Count() != valueList.Count)
                {
                    throw new NotImplementedException(); // todo
                }

                this.Values = new HashSet<string>(valueList);
            }

            this.Description = description;
            this.DocSubstitution = docSubstitution;
        }

        public string Alias { get; }
        public HashSet<string> Values { get; }
        public string Description { get; }
        public string DocSubstitution { get; }
    }
}
