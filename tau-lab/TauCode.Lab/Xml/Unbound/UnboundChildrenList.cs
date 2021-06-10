using System;
using System.Collections;
using System.Collections.Generic;

namespace TauCode.Lab.Xml.Unbound
{
    // todo regions
    internal class UnboundChildrenList : IList<IElement>
    {
        private readonly List<IElement> _elements;

        internal UnboundChildrenList()
        {
            _elements = new List<IElement>();
        }


        public IEnumerator<IElement> GetEnumerator() => _elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _elements.GetEnumerator();

        public void Add(IElement item)
        {
            // todo checks

            _elements.Add(item);
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(IElement item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(IElement[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(IElement item)
        {
            throw new System.NotImplementedException();
        }

        public int Count => throw new NotImplementedException();
        public bool IsReadOnly => throw new NotImplementedException();
        public int IndexOf(IElement item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, IElement item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        public IElement this[int index]
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
    }
}
