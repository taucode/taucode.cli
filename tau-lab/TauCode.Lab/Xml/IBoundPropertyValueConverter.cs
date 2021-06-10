using System.Reflection;

namespace TauCode.Lab.Xml
{
    public interface IBoundPropertyValueConverter
    {
        string ToStringValue(PropertyInfo property, object value);
        object FromStringValue(PropertyInfo property, string valueString);
    }
}
