namespace TauCode.Lab.Xml
{
    public interface IChildElementDescriptor
    {
        string ElementName { get; }
        int MinOccurrence { get; }
        int? MaxOccurrence { get; }
    }
}
