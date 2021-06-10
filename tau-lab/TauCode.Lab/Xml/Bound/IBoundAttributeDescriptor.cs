using System.Reflection;

namespace TauCode.Lab.Xml.Bound
{
    public interface IBoundAttributeDescriptor
    {
        string AttributeName { get; }
        PropertyInfo Property { get; }
        bool IsMandatory { get; }
    }
}
