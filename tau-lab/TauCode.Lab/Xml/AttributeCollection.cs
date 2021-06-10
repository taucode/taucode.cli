using System.Collections.Generic;

namespace TauCode.Lab.Xml
{
    // todo: internal?
    internal class AttributeCollection : IAttributeCollection
    {
        #region Fields

        private readonly Dictionary<string, string> _attributes;

        #endregion

        #region ctor

        public AttributeCollection()
        {
            _attributes = new Dictionary<string, string>();
        }

        #endregion

        #region IAttributeCollection Members

        public string this[string attributeName]
        {
            // todo checks

            get => _attributes[attributeName];
            set => _attributes[attributeName] = value;
        }

        public void Add(string attributeName, string attributeValue)
        {
            // todo checks

            _attributes.Add(attributeName, attributeValue);
        }

        public bool Remove(string attributeName)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<string> GetNames()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(string attributeName)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
