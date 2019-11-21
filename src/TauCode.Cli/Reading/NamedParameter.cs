namespace TauCode.Cli.Reading
{
    public class NamedParameter
    {
        internal NamedParameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}
