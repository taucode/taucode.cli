namespace TauCode.Cli.CliCommandEntries
{
    public class KeyValueCliCommandEntry : ICliCommandEntry
    {
        public string Alias { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
