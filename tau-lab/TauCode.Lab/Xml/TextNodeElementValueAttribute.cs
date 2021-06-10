using System;

namespace TauCode.Lab.Xml
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TextNodeElementValueAttribute : Attribute
    {
        public TextNodeElementValueAttribute(string elementName = null)
        {
            this.ElementName = elementName;
        }

        public string ElementName { get; }
    }
}
