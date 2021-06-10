using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Type = System.Type;

namespace TauCode.Lab.Xml
{
    // todo clean
    // todo regions
    // todo: ISerializer?
    public class Serializer
    {
        private readonly SerializationSettings _defaultSettings;
        private readonly IBoundPropertyValueConverter _defaultConverter;

        public Serializer()
        {
            _defaultSettings = SerializationSettings.CreateDefault();
            _defaultConverter = new DefaultBoundPropertyValueConverter();
        }

        public SerializationSettings Settings { get; set; }

        private SerializationSettings GetSafeSettings() => this.Settings ?? _defaultSettings;

        private IBoundPropertyValueConverter GetSafeConverter() =>
            this.GetSafeSettings().BoundPropertyValueConverter ?? _defaultConverter;

        public TElement DeserializeXmlDocument<TElement>(XmlDocument xmlDocument)
            where TElement : IElement, IDocument
        {
            // todo checks

            if (xmlDocument == null)
            {
                throw new ArgumentNullException(nameof(xmlDocument));
            }

            if (xmlDocument.DocumentElement == null)
            {
                throw new ArgumentException(
                    $"'{nameof(XmlDocument.DocumentElement)}' must not be null.",
                    nameof(xmlDocument));
            }

            var element = (TElement)DeserializeXmlElement(xmlDocument.DocumentElement, typeof(TElement));

            if (xmlDocument.DocumentElement.Name != element.RootElementName)
            {
                throw new NotImplementedException();
            }

            var document = (IDocument)element;

            var firstChild = xmlDocument.FirstChild;
            if (firstChild is XmlDeclaration xmlDeclaration)
            {
                var declaration = new Declaration(
                    xmlDeclaration.Version,
                    xmlDeclaration.Encoding,
                    xmlDeclaration.Standalone);

                document.Declaration = declaration;
            }

            if (xmlDocument.DocumentElement.HasAttribute("xmlns"))
            {
                document.Xmlns = xmlDocument.DocumentElement.GetAttribute("xmlns");
            }

            return element;
        }

        public TElement DeserializeXmlElement<TElement>(XmlElement xmlElement)
            where TElement : IElement
        {
            // todo checks
            return (TElement)DeserializeXmlElement(xmlElement, typeof(TElement));
        }

        public XmlDocument SerializeDocument(IDocument document)
        {
            // todo checks
            var element = document as IElement;
            if (element == null)
            {
                throw new NotImplementedException();
            }

            var xmlDocument = new XmlDocument();
            if (document.Declaration != null)
            {
                var xmlDeclaration = xmlDocument.CreateXmlDeclaration(
                    document.Declaration.Version,
                    document.Declaration.Encoding,
                    document.Declaration.Standalone);

                xmlDocument.AppendChild(xmlDeclaration);
            }

            var xmlElement = this.SerializeElement(element, document.RootElementName, xmlDocument);
            xmlDocument.AppendChild(xmlElement);

            if (document.Xmlns != null)
            {
                xmlDocument.DocumentElement.SetAttribute("xmlns", document.Xmlns);
            }


            return xmlDocument;
        }

        private XmlElement SerializeElement(IElement element, string elementName, XmlDocument xmlDocument)
        {
            var xmlElement = xmlDocument.CreateElement(elementName);
            var descriptor = ElementTypeDescriptorCache.Get(element.GetType());

            // deal with bound attributes
            foreach (var pair in descriptor.BoundSchema.BoundAttributeDescriptors)
            {
                var attributeName = pair.Key;
                var attributeDescriptor = pair.Value;

                var propertyValue = attributeDescriptor.Property.GetValue(element);
                if (propertyValue == null)
                {
                    continue;
                }

                var attributeValue = this.GetSafeConverter().ToStringValue(attributeDescriptor.Property, propertyValue);
                xmlElement.SetAttribute(attributeName, attributeValue);
            }

            // deal with unbound attributes
            foreach (var unboundAttributeName in descriptor.UnboundSchema.UnboundAttributes.Keys)
            {
                throw new NotImplementedException();
            }

            if (descriptor.ElementType.IsComplexElementType()) // todo: cached to some enum
            {
                this.SerializeComplexElementContent(xmlElement, (IComplexElement)element, descriptor);
            }
            else if (descriptor.ElementType.IsTextNodeElementType()) // todo: cached to some enum
            {
                this.SerializeTextNodeElementContent(xmlElement, (ITextNodeElement)element);
            }
            else
            {
                throw new NotImplementedException(); // shouldn't be, actually.
            }

            return xmlElement;
        }

        private void SerializeComplexElementContent(XmlElement xmlElement, IComplexElement complexElement, IElementTypeDescriptor hintDescriptor)
        {
            var xmlDocument = xmlElement.OwnerDocument;

            foreach (var boundChildElementDescriptor in hintDescriptor.BoundSchema.AllBoundChildElementDescriptors)
            {
                var prop = boundChildElementDescriptor.Property;

                if (prop.PropertyType.IsElementType()) // todo: cached to some enum in IBoundChildElementDescriptor
                {
                    // serialize element
                    var childElementObject = prop.GetValue(complexElement);
                    if (childElementObject == null)
                    {
                        // todo: deal with mandatory
                        continue; // this property won't get serialized
                    }

                    var childElement = (IElement)childElementObject;
                    var childXmlElement = this.SerializeElement(
                        childElement,
                        boundChildElementDescriptor.ElementName,
                        xmlDocument);

                    xmlElement.AppendChild(childXmlElement);
                }
                else if (prop.PropertyType.IsElementListType()) // todo: cached to some enum in IBoundChildElementDescriptor
                {
                    var propertyValueObject = prop.GetValue(complexElement);
                    if (propertyValueObject == null)
                    {
                        // nothing to serialize
                        continue;
                    }

                    var list = (IList)propertyValueObject;
                    foreach (var listItem in list)
                    {
                        if (listItem == null)
                        {
                            throw new NotImplementedException(); // nulls not allowed - they cannot be serialized.
                        }

                        var listElement = (IElement)listItem;

                        // todo: check listElement type.
                        var xmlListElement = this.SerializeElement(
                            listElement,
                            boundChildElementDescriptor.ElementName,
                            xmlDocument);

                        xmlElement.AppendChild(xmlListElement);
                    }
                }
                else if (prop.PropertyType.IsSimpleType() || prop.PropertyType.IsNullableSimpleType())
                {
                    var value = prop.GetValue(complexElement);

                    if (value == null)
                    {
                        continue; // todo temp. 'MinOccurrence' appeared to be '1' for prop type 'bool?' which is wrong.



                        //if (boundChildElementDescriptor.MinOccurrence > 0)
                        //{
                        //    throw new NotImplementedException();
                        //}
                        //else
                        //{
                        //    continue;
                        //}
                    }

                    var childXmlElement = xmlDocument.CreateElement(boundChildElementDescriptor.ElementName);

                    var text = this.GetSafeConverter().ToStringValue(prop, value);

                    var textNode = xmlDocument.CreateTextNode(text);
                    childXmlElement.AppendChild(textNode);

                    xmlElement.AppendChild(childXmlElement);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            // todo: deal with unbound and unknown elements
        }

        private void SerializeTextNodeElementContent(XmlElement xmlElement, ITextNodeElement textNodeElement)
        {
            if (textNodeElement.Value != null)
            {
                var textNode = xmlElement.OwnerDocument.CreateTextNode(textNodeElement.Value);
                xmlElement.AppendChild(textNode);
            }
        }

        private IElement DeserializeXmlElement(XmlElement xmlElement, Type elementType)
        {
            if (elementType.IsComplexElementType())
            {
                return DeserializeComplexXmlElement(xmlElement, elementType);
            }
            else if (elementType.IsTextNodeElementType())
            {
                return DeserializeTextNodeXmlElement(xmlElement, elementType);
            }
            else
            {
                throw new NotImplementedException();
            }

            //var ctor = elementType.GetConstructor(Type.EmptyTypes); // todo check not null
            //var descriptor = ElementTypeDescriptorCache.Get(elementType);



            //throw new NotImplementedException();
        }

        private IComplexElement DeserializeComplexXmlElement(XmlElement xmlElement, Type elementType)
        {
            var instance = (IComplexElement)Activator.CreateInstance(elementType);
            var descriptor = ElementTypeDescriptorCache.Get(elementType);

            #region attributes

            var boundMandatoryAttributeNames = descriptor.BoundSchema.GetBoundSchemaMandatoryAttributeNames().ToHashSet(); // todo: check is empty at end
            var unboundMandatoryAttributeNames = descriptor.UnboundSchema.GetUnboundSchemaMandatoryAttributeNames().ToHashSet(); // todo: check is empty at end
            var processedAttributeNames = new HashSet<string>();

            foreach (var xmlAttribute in xmlElement.Attributes.Cast<XmlAttribute>())
            {
                var xmlAttributeName = xmlAttribute.Name;

                if (xmlAttributeName == "xmlns")
                {
                    continue; // none of our business
                }

                if (processedAttributeNames.Contains(xmlAttributeName))
                {
                    throw new NotImplementedException();
                }

                var xmlAttributeValue = xmlAttribute.Value;

                // deal with bound
                var boundAttributeDescriptor =
                    descriptor.BoundSchema.BoundAttributeDescriptors.GetValueOrDefault(xmlAttributeName);

                if (boundAttributeDescriptor == null)
                {
                    // not bound to a property
                }
                else
                {
                    // got bound attr
                    boundAttributeDescriptor.Property.SetParsedValue(instance, xmlAttributeValue);

                    boundMandatoryAttributeNames.Remove(xmlAttributeName);
                    processedAttributeNames.Add(xmlAttributeName);

                    continue;
                }

                // deal with unbound
                var haveUnbound =
                    descriptor.UnboundSchema.UnboundAttributes.TryGetValue(xmlAttributeName, out var dummy);

                if (haveUnbound)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    // not among unbound attributes
                }

                // unknown attribute
                if (descriptor.UnboundSchema.AllowsUnknownAttributes)
                {
                    instance.UnboundAttributes.Add(xmlAttributeName, xmlAttributeValue);
                    processedAttributeNames.Add(xmlAttributeName);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            #endregion

            #region child elements

            foreach (var xmlChildNode in xmlElement.ChildNodes)
            {
                if (xmlChildNode is XmlElement xmlChildElement)
                {
                    var childElementName = xmlChildElement.Name;

                    if (descriptor.BoundSchema.BoundChildTextNodeElementValueDescriptors.ContainsKey(childElementName))
                    {
                        var boundElementDescriptor = descriptor.BoundSchema.BoundChildTextNodeElementValueDescriptors[childElementName];


                        if (boundElementDescriptor.Property.PropertyType.IsElementListType())
                        {
                            throw new NotImplementedException();

                            //var list = (IList)boundElementDescriptor.Property.GetValue(instance);
                            //if (list == null)
                            //{
                            //    throw new NotImplementedException();
                            //}

                            //var childListEntry = this.Deserialize(
                            //    xmlChildElement,
                            //    boundElementDescriptor.Property.PropertyType.GetGenericArguments().Single()); // todo: optimize?

                            //list.Add(childListEntry);
                        }
                        else
                        {
                            var textNodeValue = xmlChildElement.GetTextNodeElementValue();
                            boundElementDescriptor.Property.SetParsedValue(instance, textNodeValue);

                            //var childElement = this.Deserialize(
                            //    xmlChildElement,
                            //    boundElementDescriptor.Property.PropertyType);

                            //boundElementDescriptor.Property.SetValue(instance, childElement);
                        }
                    }
                    else if (descriptor.BoundSchema.BoundChildElementDescriptors.ContainsKey(childElementName))
                    {
                        var boundElementDescriptor = descriptor.BoundSchema.BoundChildElementDescriptors[childElementName];

                        if (boundElementDescriptor.Property.PropertyType.IsElementListType())
                        {
                            var list = (IList)boundElementDescriptor.Property.GetValue(instance);
                            if (list == null)
                            {
                                throw new NotImplementedException();
                            }

                            var childListEntry = this.DeserializeXmlElement(
                                xmlChildElement,
                                boundElementDescriptor.Property.PropertyType.GetGenericArguments().Single()); // todo: optimize?

                            list.Add(childListEntry);
                        }
                        else
                        {
                            var childElement = this.DeserializeXmlElement(
                                xmlChildElement,
                                boundElementDescriptor.Property.PropertyType);

                            boundElementDescriptor.Property.SetValue(instance, childElement);
                        }
                    }
                    else if (descriptor.UnboundSchema.GetUnboundChildElementDescriptorsDictionary().ContainsKey(childElementName))
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        if (descriptor.UnboundSchema.AllowsUnknownChildElements)
                        {
                            if (xmlElement.IsTextNodeXmlElement())
                            {
                                throw new NotImplementedException();
                            }
                            else
                            {
                                var childElement = this.DeserializeUnknownComplexXmlElement(xmlChildElement);
                                instance.UnboundChildren.Add(childElement);
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            #endregion

            return instance;
        }

        private ITextNodeElement DeserializeTextNodeXmlElement(XmlElement xmlElement, Type elementType)
        {
            var instance = (ITextNodeElement)Activator.CreateInstance(elementType);
            var descriptor = ElementTypeDescriptorCache.Get(elementType);

            // todo: copy-pasted snippet with bound attrs.

            #region attributes

            var boundMandatoryAttributeNames = descriptor.BoundSchema.GetBoundSchemaMandatoryAttributeNames().ToHashSet(); // todo: check is empty at end
            var unboundMandatoryAttributeNames = descriptor.UnboundSchema.GetUnboundSchemaMandatoryAttributeNames().ToHashSet(); // todo: check is empty at end
            var processedAttributeNames = new HashSet<string>();

            foreach (var xmlAttribute in xmlElement.Attributes.Cast<XmlAttribute>())
            {
                var xmlAttributeName = xmlAttribute.Name;

                if (processedAttributeNames.Contains(xmlAttributeName))
                {
                    throw new NotImplementedException();
                }

                var xmlAttributeValue = xmlAttribute.Value;

                // deal with bound
                var boundAttributeDescriptor =
                    descriptor.BoundSchema.BoundAttributeDescriptors.GetValueOrDefault(xmlAttributeName);

                if (boundAttributeDescriptor == null)
                {
                    // not bound to a property
                }
                else
                {
                    // got bound attr
                    boundAttributeDescriptor.Property.SetParsedValue(instance, xmlAttributeValue);

                    boundMandatoryAttributeNames.Remove(xmlAttributeName);
                    processedAttributeNames.Add(xmlAttributeName);

                    continue;
                }

                // deal with unbound
                var haveUnbound =
                    descriptor.UnboundSchema.UnboundAttributes.TryGetValue(xmlAttributeName, out var dummy);

                if (haveUnbound)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    // not among unbound attributes
                }

                // unknown attribute
                if (descriptor.UnboundSchema.AllowsUnknownAttributes)
                {
                    instance.UnboundAttributes.Add(xmlAttributeName, xmlAttributeValue);
                    processedAttributeNames.Add(xmlAttributeName);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            #endregion

            #region text node value

            instance.Value = xmlElement
                .ChildNodes
                .Cast<XmlNode>()
                .Single(x => x is XmlText).Value; // todo: can be zero and not single. e.g. <Foo/> instead of <Foo></Foo> ?

            #endregion

            return instance;
        }

        private UnknownComplexElement DeserializeUnknownComplexXmlElement(XmlElement xmlElement)
        {
            var instance = new UnknownComplexElement(xmlElement.Name);

            // attributes
            foreach (var xmlAttribute in xmlElement.Attributes.Cast<XmlAttribute>())
            {
                instance.UnboundAttributes.Add(xmlAttribute.Name, xmlAttribute.Value);
            }

            // child elements
            foreach (var childNode in xmlElement.ChildNodes)
            {
                if (childNode is XmlComment)
                {
                    continue;
                }

                if (childNode is XmlElement childXmlElement)
                {
                    if (childXmlElement.IsTextNodeXmlElement())
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        var childElement = this.DeserializeUnknownComplexXmlElement(childXmlElement);
                        instance.UnboundChildren.Add(childElement);

                        continue;
                    }
                }

                throw new NotImplementedException();
            }

            return instance;
        }
    }
}
