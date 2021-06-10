using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using TauCode.Extensions;
using TauCode.Lab.Xml.Bound;
using TauCode.Lab.Xml.Unbound;

namespace TauCode.Lab.Xml
{
    public static class XmlExtensions
    {
        internal static bool HasAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
        {
            var attr = memberInfo.GetCustomAttribute<TAttribute>();
            return attr != null;
        }

        internal static bool IsSimpleType(this Type type)
        {
            return
                type.IsIn(
                    typeof(int),
                    typeof(long),
                    typeof(double),
                    typeof(decimal),
                    typeof(bool),
                    typeof(string),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan)) ||
                type.IsEnum;
        }

        internal static bool IsNullableSimpleType(this Type type)
        {
            if (type.IsGenericType)
            {
                var generic = type.GetGenericTypeDefinition();
                if (generic == typeof(Nullable<>))
                {
                    var innerType = type.GetGenericArguments().Single();
                    return innerType.IsSimpleType();
                }
            }

            return false;
        }

        internal static bool IsNullableEnumType(this Type type)
        {
            if (type.IsGenericType)
            {
                var generic = type.GetGenericTypeDefinition();
                if (generic == typeof(Nullable<>))
                {
                    var innerType = type.GetGenericArguments().Single();
                    return innerType.IsEnum;
                }
            }

            return false;
        }

        internal static Type ExtractValueType(this Type type)
        {
            if (type.IsValueType && !type.IsGenericType)
            {
                return type;
            }

            if (type.IsGenericType)
            {
                var generic = type.GetGenericTypeDefinition();
                if (generic == typeof(Nullable<>))
                {
                    var innerType = type.GetGenericArguments().Single();
                    return innerType;
                }
            }

            throw new NotImplementedException();
        }

        internal static bool IsElementType(this Type type)
        {
            return typeof(IElement).IsAssignableFrom(type);
        }

        internal static bool IsElementListType(this Type type)
        {
            if (type.IsGenericType)
            {
                var generic = type.GetGenericTypeDefinition();
                if (generic == typeof(IList<>))
                {
                    var innerType = type.GetGenericArguments().Single();
                    return innerType.IsElementType();
                }
            }

            return false;
        }

        internal static bool IsComplexElementType(this Type type)
        {
            return typeof(IComplexElement).IsAssignableFrom(type);
        }

        internal static bool IsTextNodeElementType(this Type type)
        {
            return typeof(ITextNodeElement).IsAssignableFrom(type);
        }

        internal static IReadOnlyList<string> GetBoundSchemaMandatoryAttributeNames(this IBoundSchema boundSchema)
        {
            if (boundSchema is BoundSchemaBase boundSchemaImpl)
            {
                return boundSchemaImpl.MandatoryAttributeNames;
            }

            throw new NotImplementedException();
        }

        public static IReadOnlyList<string> GetUnboundSchemaMandatoryAttributeNames(this IUnboundSchema unboundSchema)
        {
            if (unboundSchema is UnboundSchemaBase unboundSchemaImpl)
            {
                return unboundSchemaImpl.MandatoryAttributeNames;
            }

            throw new NotImplementedException();
        }

        public static IReadOnlyDictionary<string, UnboundChildElementDescriptor> GetUnboundChildElementDescriptorsDictionary(
            this IUnboundSchema unboundSchema)
        {
            // todo checks

            if (unboundSchema is UnboundSchemaBase unboundSchemaImpl)
            {
                return unboundSchemaImpl.UnboundChildElementDescriptorByElementName;
            }

            throw new NotImplementedException();
        }

        internal static void SetParsedValue(this PropertyInfo property, object instance, string propertyStringValue)
        {
            var type = property.PropertyType;
            object propertyValue;

            if (type == typeof(string))
            {
                propertyValue = propertyStringValue;
            }
            else if (type.IsIn(typeof(int), typeof(int?)))
            {
                propertyValue = int.Parse(propertyStringValue, CultureInfo.InvariantCulture);
            }
            else if (type.IsIn(typeof(bool), typeof(bool?)))
            {
                propertyValue = bool.Parse(propertyStringValue);
            }
            else if (type.IsEnum || type.IsNullableEnumType())
            {
                var enumType = type.ExtractValueType();
                propertyValue = Enum.Parse(enumType, propertyStringValue);
            }
            else
            {
                throw new NotImplementedException();
            }

            property.SetValue(instance, propertyValue);
        }

        internal static bool IsTextNodeXmlElement(this XmlElement xmlElement)
        {
            foreach (var childNode in xmlElement.ChildNodes)
            {
                if (childNode is XmlComment)
                {
                    continue;
                }

                if (childNode is XmlText)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        internal static string GetTextNodeElementValue(this XmlElement xmlElement)
        {
            XmlText textNode = null;

            foreach (var node in xmlElement.ChildNodes)
            {
                if (node is XmlComment)
                {
                    continue;
                }

                if (node is XmlText textNodeCurrent)
                {
                    if (textNode != null)
                    {
                        throw new NotImplementedException();
                    }

                    textNode = textNodeCurrent;
                    continue;
                }

                throw new NotImplementedException();
            }

            if (textNode == null)
            {
                throw new NotImplementedException();
            }

            return textNode.Value;
        }

        // todo clean
        //public static string ToXmlString(this XmlDocument document)
        //{
        //    using var stream = new MemoryStream();
        //    using var writer = new XmlTextWriter(stream, Encoding.UTF8);
        //    using var reader = new StreamReader(stream);

        //    writer.Formatting = Formatting.Indented;
        //    document.WriteContentTo(writer);

        //    writer.Flush();
        //    stream.Flush();

        //    stream.Position = 0;
        //    var xmlString = reader.ReadToEnd();

        //    return xmlString;
        //}

        //internal static string ToXmlTextValue(object value)
        //{
        //    if (value is string s)
        //    {
        //        return s;
        //    }
        //    else if (value is bool boolValue)
        //    {
        //        return boolValue.ToString();
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
