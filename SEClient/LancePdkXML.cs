using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Xml;
using System.Reflection;

namespace SERemoteLib
{
    /// <summary>
    /// The serializer to generate an XML document. This class offers some functions to create general xml-elements
    /// for the transfer of data over SOAP to a PDK instrument.
    /// </summary>
    public sealed class PdkXmlSerializer
    {
        private static IDictionary<Type, String> typeMappings;

        /// <summary>
        /// The XmlElement name of a parameterlist-element.
        /// </summary>
        public static readonly String ParamlistElementname = "param-list";
        /// <summary>
        /// The XmlElement name of a parameter-element.
        /// </summary>
        public static readonly String ParamElementname = "param";
        /// <summary>
        /// The XmlElement name of a parameter-element.
        /// </summary>
        public static readonly String ItemElementname = "item";
        /// <summary>
        /// The XmlAttribute name of a type-attribute.
        /// </summary>
        public static readonly String TypeAttributename = "type";
        /// <summary>
        /// The XmlAttribute name of a sequence type-attribute.
        /// </summary>
        public static readonly String SequenceTypeAttributename = "sequenceType";
        /// <summary>
        /// The XmlAttribute name of a name-attribute.
        /// </summary>
        public static readonly String NameAttributename = "name";

        /// <summary>
        /// The XmlElement name of a method-element.
        /// </summary>
        public static readonly String MethodElementname = "method";
        /// <summary>
        /// The XmlElement name of a methodfunction-element.
        /// </summary>
        public static readonly String MethodfunctionElementname = "mf";
        /// <summary>
        /// The XmlElement name of a record-element.
        /// </summary>
        public static readonly String RecordElementname = "record";

        private static readonly ILog logger = LogManager.GetLogger(typeof(PdkXmlSerializer));

        /// <summary>
        /// Initializes a new instance of the <see cref="PdkXmlSerializer"/> class.
        /// </summary>
        private PdkXmlSerializer()
        { }


        /// <summary>
        /// Serializes the record <paramref name="record"/> into the PDK specific XML. 
        /// </summary>
        /// <param name="record">The browsable screen record.</param>
        /// <returns></returns>
        public static string SerializeRecord(object record)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement recordElement = CreateRecordElement(record, doc);
            doc.AppendChild(recordElement);

            if (logger.IsDebugEnabled)
            {
                logger.Debug("Serialized " + record.GetType().ToString() + " into " + doc.InnerXml);
            }

            return doc.InnerXml.Replace("\r\n", "&#xA;").Replace("\n", "&#xA;").Replace("\t", "&#x9;");
        }

        private static XmlElement CreateRecordElement(object recordObject, XmlDocument doc)
        {
            XmlElement recordElement = doc.CreateElement(RecordElementname);

            XmlAttribute recordTypeAttribute = doc.CreateAttribute(TypeAttributename);
            recordTypeAttribute.InnerText = recordObject.GetType().Name;
            recordElement.Attributes.Append(recordTypeAttribute);

            XmlElement paramListElement = CreateParameterListElement(recordObject, doc);
            recordElement.AppendChild(paramListElement);

            return recordElement;
        }

        /// <summary>
        /// Initializes the type-mapping dictonary.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "add collection members")]
        static PdkXmlSerializer()
        {
            typeMappings = new Dictionary<Type, String>();

            typeMappings.Add(typeof(string), "wstring");
            typeMappings.Add(typeof(bool), "boolean");
            typeMappings.Add(typeof(short), "short");
            typeMappings.Add(typeof(DateTime), "datetime");
            typeMappings.Add(typeof(decimal), "decfloat");
            typeMappings.Add(typeof(int), "long");
            typeMappings.Add(typeof(byte), "octet");
            //typeMappings.Add(typeof(decfloat), "decfloat");
            typeMappings.Add(typeof(double), "double");
        }

        /// <summary>
        /// Maps a <paramref name="type"/> to the according PDK type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String MapType(Type type)
        {
            if (typeMappings.ContainsKey(type))
            {
                return typeMappings[type];
            }
            else if (type.IsEnum)
            {
                return "short";
            }
            else if (type.IsArray)
            {
                return "sequence";
            }
            else
            {
                return "record";
            }
        }

        /// <summary>
        /// Serializes the <paramref name="val"/> value into the according XML representation.
        /// </summary>
        /// <param name="val">The value to serialize. MUST NOT be null</param>
        /// <returns>XML string representing the value.</returns>
        public static String SerializeValue(object val)
        {
            String returnValue = null;

            if (val == null)
            {
                throw new ArgumentException("The argument val MUST NOT be null.");
            }

            if (typeMappings.ContainsKey(val.GetType()))
            {
                if (val.GetType() == typeof(string))
                {
                    returnValue = (string)val;
                }
                else if (val.GetType() == typeof(bool))
                {
                    returnValue = XmlConvert.ToString((bool)val);
                }
                else if (val.GetType() == typeof(short))
                {
                    returnValue = XmlConvert.ToString((short)val);
                }
                else if (val.GetType() == typeof(DateTime))
                {
                    returnValue = XmlConvert.ToString((DateTime)val, "yyyy-MM-dd HH:mm:ss");
                }
                else if (val.GetType() == typeof(decimal))
                {
                    returnValue = XmlConvert.ToString((decimal)val);
                }
                else if (val.GetType() == typeof(int))
                {
                    returnValue = XmlConvert.ToString((int)val);
                }
                else if (val.GetType() == typeof(byte))
                {
                    returnValue = XmlConvert.ToString((byte)val);
                }
                //else if (val.GetType() == typeof(decfloat))
                //{
                //    returnValue = ((decfloat)val).Value;
                //}
                else if (val.GetType() == typeof(double))
                {
                    returnValue = XmlConvert.ToString((double)val);
                }
            }
            else if (val.GetType().IsEnum)
            {
                returnValue = XmlConvert.ToString((int)val);
            }
            return returnValue;
        }

        /// <summary>
        /// Creates the element param-list element with all its param subelements. Each property of the <paramref name="paramObject"/>
        /// is added as a param elemment.
        /// </summary>
        /// <param name="paramObject"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object")]
        private static XmlElement CreateParameterListElement(object paramObject, XmlDocument doc)
        {
            XmlElement paramListElement = doc.CreateElement(ParamlistElementname);

            //add parameters
            PropertyInfo[] properties = paramObject.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                bool ignoreThisProperty = false;
                foreach (System.Xml.Serialization.XmlIgnoreAttribute xmlIgnoreAttribute in property.GetCustomAttributes(typeof(System.Xml.Serialization.XmlIgnoreAttribute), false))
                {
                    ignoreThisProperty = true;
                    break;
                }

                if (!ignoreThisProperty)
                {
                    XmlElement paramElement = CreateParameterElement(property, paramObject, doc);
                    paramListElement.AppendChild(paramElement);
                }
            }

            return paramListElement;
        }

        /// <summary>
        /// Creates the param element for the <paramref name="property"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="paramObject"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static XmlElement CreateParameterElement(PropertyInfo property, object paramObject, XmlDocument doc)
        {
            XmlElement paramElement = doc.CreateElement(ParamElementname);

            XmlAttribute nameAttribute = doc.CreateAttribute(NameAttributename);
            nameAttribute.InnerText = property.Name;
            paramElement.Attributes.Append(nameAttribute);

            XmlAttribute paramTypeAttribute = doc.CreateAttribute(TypeAttributename);
            paramTypeAttribute.InnerText = MapType(property.GetGetMethod().ReturnType);
            paramElement.Attributes.Append(paramTypeAttribute);

            object val = property.GetValue(paramObject, null);
            if (val != null)
            {
                if (paramTypeAttribute.InnerText == "record")
                {
                    XmlElement recordElement = CreateRecordElement(val, doc);
                    paramElement.AppendChild(recordElement);
                }
                else if (paramTypeAttribute.InnerText == "sequence")
                {
                    XmlAttribute seqTypeAttribute = doc.CreateAttribute(SequenceTypeAttributename);
                    bool isComplexTypeSequence;
                    if (property.PropertyType == typeof(string[]))
                    {
                        seqTypeAttribute.InnerText = MapType(typeof(string));
                        isComplexTypeSequence = false;
                    }
                    else
                    {
                        seqTypeAttribute.InnerText = RecordElementname;
                        isComplexTypeSequence = true;
                    }
                    paramElement.Attributes.Append(seqTypeAttribute);

                    Array ar = (Array)val;
                    for (int i = 0; i < ar.Length; ++i)
                    {
                        XmlElement itemElement = doc.CreateElement(ItemElementname);
                        if (isComplexTypeSequence)
                        {
                            XmlElement recordElement = CreateRecordElement(ar.GetValue(i), doc);
                            itemElement.AppendChild(recordElement);
                        }
                        else
                        {
                            itemElement.InnerText = ar.GetValue(i).ToString();
                        }
                        paramElement.AppendChild(itemElement);
                    }
                }
                else
                {
                    paramElement.InnerText = SerializeValue(val);
                }
            }

            return paramElement;
        }
    }
}
