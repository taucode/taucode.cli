using System.Collections.Generic;

namespace TauCode.Lab.Xml
{
    public interface IAttributeCollection
    {
        string this[string attributeName] { get; set; }
        void Add(string attributeName, string attributeValue);
        bool Remove(string attributeName);
        IReadOnlyList<string> GetNames();
        bool Contains(string attributeName);
    }
}