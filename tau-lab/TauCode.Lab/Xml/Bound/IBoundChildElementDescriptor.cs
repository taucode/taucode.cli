using System.Reflection;

namespace TauCode.Lab.Xml.Bound
{
    internal interface IBoundChildElementDescriptor : IChildElementDescriptor
    {
        PropertyInfo Property { get; }
    }
}
