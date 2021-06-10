using Inflector;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using TauCode.Lab.Xml.Unbound;

namespace TauCode.Lab.Xml
{
    internal static class ElementTypeDescriptorCache
    {
        private static Dictionary<Type, IElementTypeDescriptor> _descriptors;

        static ElementTypeDescriptorCache()
        {
            _descriptors = new Dictionary<Type, IElementTypeDescriptor>();
        }

        internal static IElementTypeDescriptor Get(Type elementType)
        {
            // todo checks: not null, proper type

            var descriptor = _descriptors.GetValueOrDefault(elementType);

            if (descriptor == null)
            {
                descriptor = CreateDescriptor(elementType);
            }

            return descriptor;
        }

        private static IElementTypeDescriptor CreateDescriptor(Type elementType)
        {
            var elementTypeDescriptor = new ElementTypeDescriptor(elementType);
            _descriptors.Add(elementType, elementTypeDescriptor);

            #region bound schema

            var props = elementType.GetProperties();

            foreach (var prop in props)
            {
                var skip =
                    prop.Name == nameof(IElement.UnboundSchema) ||
                    prop.Name == nameof(IElement.UnboundAttributes) ||
                    prop.Name == nameof(IComplexElement.UnboundChildren) ||
                    prop.Name == nameof(ITextNodeElement.Value) ||
                    prop.HasAttribute<XmlIgnoreAttribute>() ||
                    false;

                if (skip)
                {
                    continue;
                }

                if (prop.HasAttribute<AttributeValueAttribute>())
                {
                    if (prop.PropertyType.IsSimpleType() || prop.PropertyType.IsNullableSimpleType())
                    {
                        // ok.
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    if (
                        prop.HasAttribute<TextNodeElementValueAttribute>() ||
                        prop.HasAttribute<ElementAttribute>())
                    {
                        throw new NotImplementedException();
                    }

                    var isMandatory = prop.PropertyType.IsValueType; // todo: error. Nullable`1 is also value type.
                    var attributeName = prop.GetCustomAttribute<AttributeValueAttribute>().AttributeName;
                    elementTypeDescriptor.BoundSchemaInternal.AddBoundAttribute(attributeName, prop, isMandatory);
                }
                else if (prop.HasAttribute<TextNodeElementValueAttribute>())
                {
                    if (prop.PropertyType.IsSimpleType() || prop.PropertyType.IsNullableSimpleType())
                    {
                        // ok.
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    if (
                        prop.HasAttribute<AttributeValueAttribute>() ||
                        prop.HasAttribute<ElementAttribute>())
                    {
                        throw new NotImplementedException();
                    }

                    var elementName = prop.GetCustomAttribute<TextNodeElementValueAttribute>().ElementName ?? prop.Name;
                    var isMandatory = prop.PropertyType.IsValueType; // todo: error. Nullable`1 is also value type.

                    elementTypeDescriptor.BoundSchemaInternal.AddBoundTextNodeElementValue(elementName, prop, isMandatory);
                }
                else if (prop.HasAttribute<ElementAttribute>())
                {
                    if (typeof(IElement).IsAssignableFrom(prop.PropertyType) || prop.PropertyType.IsElementListType())
                    {
                        // that's correct
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    if (
                        prop.HasAttribute<TextNodeElementValueAttribute>() ||
                        prop.HasAttribute<AttributeValueAttribute>())
                    {
                        throw new NotImplementedException();
                    }

                    var elementName = prop.GetCustomAttribute<ElementAttribute>().ElementName;

                    if (prop.PropertyType.IsElementType())
                    {
                        elementTypeDescriptor.BoundSchemaInternal.AddBoundElement(elementName, prop, 0, null); // todo: respect isMandatory of the attribute
                    }
                    else if (prop.PropertyType.IsElementListType())
                    {
                        elementTypeDescriptor.BoundSchemaInternal.AddBoundElement(elementName, prop, 0, null);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    // no attributes around
                    if (prop.PropertyType.IsSimpleType())
                    {
                        var isMandatory = prop.PropertyType != typeof(string);
                        elementTypeDescriptor.BoundSchemaInternal.AddBoundAttribute(prop.Name, prop, isMandatory);
                    }
                    else if (prop.PropertyType.IsNullableSimpleType())
                    {
                        elementTypeDescriptor.BoundSchemaInternal.AddBoundAttribute(prop.Name, prop, false);
                    }
                    else if (prop.PropertyType.IsElementType())
                    {
                        elementTypeDescriptor.BoundSchemaInternal.AddBoundElement(prop.Name, prop, 0, null);
                    }
                    else if (prop.PropertyType.IsElementListType())
                    {
                        var singularForm = prop.Name.Singularize(); // todo todo0 BAAAD: schema depends on current inflector locale?!
                        elementTypeDescriptor.BoundSchemaInternal.AddBoundElement(singularForm, prop, 0, null);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            #endregion

            #region unbound schema

            var ctor = elementType.GetConstructor(Type.EmptyTypes); // todo check
            var element = (IElement)ctor.Invoke(new object[0]);

            var originalSchema =
                new UnboundSchemaBase(element.UnboundSchema ?? UnboundSchemaBase.RestrictingEmptyUnboundSchema);

            // todo: check unbound schema doesn't contain bound names

            elementTypeDescriptor.UnboundSchemaInternal = originalSchema;

            #endregion

            return elementTypeDescriptor;
        }
    }
}
