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

                this.Values = valueList;
            }

            this.Description = description;
            this.DocSubstitution = docSubstitution;
        }

        public List<string> Values { get; }
        public string Description { get; }
        public string DocSubstitution { get; }
    }
}
