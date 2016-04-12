using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using XMLSerializationCustomization;
using log4net;
using System.Collections;

namespace MT.pHLab.SE.V1
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
            typeMappings.Add(typeof(decfloat), "decfloat");
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
                else if (val.GetType() == typeof(decfloat))
                {
                    returnValue = ((decfloat)val).Value;
                }
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

    public sealed class PdkXmlDeserializer
    {
        private static IDictionary<String, Type> typeMappings = new Dictionary<String, Type>()
        {
            { "wstring", typeof(string) },
            { "boolean", typeof(bool) },
            { "short", typeof(short) },    
            { "datetime", typeof(DateTime) },
            { "decfloat", typeof(decimal) },
            { "long", typeof(int) },
            { "octet", typeof(byte) },     
            { "decfloat", typeof(decfloat) },
            { "double", typeof(double) }
        };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "add collection members")]
        static PdkXmlDeserializer()
        {
        }

        public static Type ResolveType(string stringType)
        {
            if (typeMappings.ContainsKey(stringType))
            {
                return typeMappings[stringType];
            }
            return null;
        }
    }

    public class PDKXmlWriter
    {
        static private void WriteProperty(object record, PropertyInfo property, System.Xml.XmlWriter write)
        {
            write.WriteStartElement("param");
            write.WriteAttributeString("name", property.Name);
            string parameterType = PdkXmlSerializer.MapType(property.GetGetMethod().ReturnType);
            write.WriteAttributeString("type", parameterType);

            object val = property.GetValue(record, null);
            if (val != null)
            {
                if (parameterType == "record")
                {
                    WriteRecord(val, write);
                }
                else if (parameterType == "sequence")
                {
                    if (property.PropertyType == typeof(string[]))
                    {
                        write.WriteAttributeString(PdkXmlSerializer.SequenceTypeAttributename, PdkXmlSerializer.MapType(typeof(string)));
                        Array ar = (Array)val;
                        for (int i = 0; i < ar.Length; ++i)
                        {
                            write.WriteStartElement(PdkXmlSerializer.ItemElementname);
                            write.WriteString(ar.GetValue(i).ToString());
                            write.WriteEndElement();
                        }
                    }
                    else
                    {
                        write.WriteAttributeString(PdkXmlSerializer.SequenceTypeAttributename, PdkXmlSerializer.RecordElementname);
                        Array ar = (Array)val;
                        for (int i = 0; i < ar.Length; ++i)
                        {
                            write.WriteStartElement(PdkXmlSerializer.ItemElementname);
                            WriteRecord(ar.GetValue(i), write);
                        }
                    }
                }
                else
                {
                    write.WriteString(PdkXmlSerializer.SerializeValue(val));
                }
            }
            write.WriteEndElement();
        }


        static private void WriteParameterList(object record, PropertyInfo[] properties, System.Xml.XmlWriter writer)
        {
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
                    WriteProperty(record, property, writer);
                }
            }
        }

        static public void WriteRecord(object record, System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("record");
            writer.WriteAttributeString("type", record.GetType().Name);
            writer.WriteStartElement("param-list");
            PropertyInfo[] properties = record.GetType().GetProperties();
            WriteParameterList(record, properties, writer);
            writer.WriteEndElement();
        }

    }


    public class IGenericRecord : IXmlSerializable
    {
        /*
        * <?xml version="1.0" encoding="utf-8" standalone="yes" ?>
        * <Telegram xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="LancePlatform">
        * <Response requestId="102478">
         * 
        * <record type="Response_StatusRecord">
        * <param-list>
        * <param name="m_status" type="wstring">Attached</param>
        * </param-list>
        * </record>
         * 
        * </Response>
        * </Telegram>
        */
        Dictionary<string, Type> _propertyTypeDic = new Dictionary<string, Type>();
        Dictionary<string, PropertyInfo> _propertyInfoDic = new Dictionary<string, PropertyInfo>();

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            PDKXmlWriter.WriteRecord(this, writer);
        }


        //<?xml version="1.0" encoding="utf-8" standalone="yes" ?>
        //<Telegram xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="LancePlatform">
        //  <Response requestId="0">
        //    <record type="Response_setupGetItemListRecord">
        //      <param-list>
        //        <param name="m_itemList" type="sequence" sequenceType="wstring">
        //          <item>M001</item>
        //          <item>M002</item>
        //          <item>M021</item>
        //          <item>A8000</item>
        //        </param>
        //      </param-list>
        //    </record>
        //  </Response>
        //</Telegram>;

        //
        //<?xml version="1.0" encoding="utf-8" standalone="yes" ?>
        //<Telegram xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="LancePlatform">
        //  <Response requestId="0">
        //    <record type="Response_StatusRecord">
        //      <param-list>
        //        <param name="m_status" type="wstring">Connected</param>
        //      </param-list>
        //    </record>
        //  </Response>
        //</Telegram>;

        protected object CreateFormString(Type type, string value)
        {
            if (type == typeof(string))
            {
                return value;
            }
            else if (type == typeof(bool))
            {
                return Convert.ToBoolean(value);
            }
            else if (type == typeof(short))
            {
                return Convert.ToInt16(value);
            }
            else if (type == typeof(DateTime))
            {
                return Convert.ToDateTime(value);
            }
            else if (type == typeof(decimal))
            {
                return Convert.ToDecimal(value);
            }
            else if (type == typeof(int))
            {
                return Convert.ToInt32(value);
            }
            else if (type == typeof(byte))
            {
                return Convert.ToByte(value);
            }
            else if (type == typeof(decfloat))
            {
                return Convert.ToDecimal(value);
            }
            else if (type == typeof(double))
            {
                return Convert.ToDouble(value);
            }
            return null;
        }

        protected T[] CreatFromXml<T>(System.Xml.XmlReader reader) where T: IGenericRecord
        {
            List<T> list = new List<T>();
            reader.ReadToFollowing("item");
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadToFollowing("record");
                // reader stays on "record"                           
                T tempValue = default(T);
                //IGenericRecord tempValue = null;
                tempValue = CreateFromXml<T>(reader);
                if (tempValue != null)
                {
                    list.Add(tempValue);
                    reader.ReadEndElement();
                }
            }
            return list.ToArray();
        }
        //<?xml version="1.0" encoding="utf-8" standalone="yes" ?>
        //<Telegram xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="LancePlatform">
        //  <Response requestId="0">
        //    <record type="Response_moduleGetRecord">
        //      <param-list>
        //        <param name="m_moduleConfigs" type="sequence" sequenceType="record">
        //          <item>
        //            <record type="moduleConfigParamRecord">
        //              <param-list>
        //                <param name="m_moduleId" type="wstring">A</param>
        //                <param name="m_sensorId" type="wstring" />
        //                <param name="m_tempSensorId" type="wstring" />
        //              </param-list>
        //            </record>
        //          </item>
        //        </param>
        //      </param-list>
        //    </record>
        //  </Response>
        //</Telegram>;

        protected void ReadProperty(System.Xml.XmlReader reader)
        {
            string nodeName = reader.Name;
            if (nodeName.Equals("param") && (reader.NodeType == XmlNodeType.Element))
            {
                string valueType = reader.GetAttribute("type");
                string valueName = reader.GetAttribute("name");

                if (_propertyTypeDic != null && _propertyTypeDic.ContainsKey(valueName))
                {
                    if (valueType == "sequence")
                    {
                        string sequenceType = reader.GetAttribute("sequenceType");
                        if (sequenceType == PdkXmlSerializer.RecordElementname)
                        {
                            Type arrayType = _propertyTypeDic[valueName];
                            Type elementType = arrayType.GetElementType();
                            if (elementType == typeof(moduleConfigParamRecord))
                            {
                                moduleConfigParamRecord[] array = CreatFromXml<moduleConfigParamRecord>(reader);
                                _propertyInfoDic[valueName].SetValue(this, array);
                            }
                        }
                        if (sequenceType == PdkXmlSerializer.SequenceTypeAttributename)
                        {
                            ; // fill it
                        }
                        else
                            if (sequenceType == "wstring")
                            {
                                XmlSerializer stringSerializer = new XmlSerializer(typeof(string));

                                List<string> list = new List<string>();
                                reader.Read();
                                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                                {
                                    reader.ReadStartElement(PdkXmlSerializer.ItemElementname);
                                    string content = reader.ReadString();
                                    list.Add(content);
                                    reader.ReadEndElement();
                                    reader.MoveToContent();
                                }
                                _propertyInfoDic[valueName].SetValue(this, list.ToArray(), null);
                            }
                    }
                    else
                        if (valueType == "record")
                        {

                        }
                        else
                        {
                            reader.ReadStartElement(nodeName);
                            object tempValue = null;
                            tempValue = CreateFormString(_propertyTypeDic[valueName], reader.ReadString());
                            reader.Read();
                            if (tempValue != null && _propertyInfoDic != null && _propertyInfoDic.ContainsKey(valueName))
                            {
                                _propertyInfoDic[valueName].SetValue(this, tempValue, null);
                            }
                        }
                } //if "param"
            } // if

        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlUtilities.GetTypePropertyDic(GetType(), ref _propertyTypeDic, ref _propertyInfoDic);
            // reader stays on record
            reader.ReadToFollowing("param-list");
            try
            {
                reader.Read();
                while ( reader.IsStartElement() )
                {
                    ReadProperty(reader);
                } // while
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static public IGenericRecord CreateFromXml(System.Xml.XmlReader reader)
        {
            // reader stays on "record"
            string recordType = reader.GetAttribute("type");

            IGenericRecord record = null;

            if (recordType == "moduleConfigParamRecord")
            {
                record = new MT.pHLab.SE.V1.moduleConfigParamRecord();
                record.ReadXml(reader);
            }

            return record;
        }

        private static T CreateFromXml<T>(System.Xml.XmlReader reader) where T : IGenericRecord
        {
            // reader stays on "record"
            string recordType = reader.GetAttribute("type");

            IGenericRecord record = null;

            if (recordType == "moduleConfigParamRecord")
            {
                record = new MT.pHLab.SE.V1.moduleConfigParamRecord();
                record.ReadXml(reader);
            }

            if (record is T)
            {
                return (T)record;
            }
            else
            {
                try
                {
                    return (T)Convert.ChangeType(record, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }
        }

    }

    #region Physical properties
    public partial class Response_StatusRecord : IGenericRecord
    {
    }
    #endregion

    public partial class Request_methodParamsRecord : IGenericRecord
    {
    }

    public partial class Request_setupGetItemListRecord : IGenericRecord
    {
    }
    public partial class Response_setupGetItemListRecord : IGenericRecord
    {
    }
    public partial class Response_SimpleResultRecord : IGenericRecord
    {
    }
    public partial class Response_moduleGetRecord : IGenericRecord
    {
    }
    public partial class moduleConfigParamRecord : IGenericRecord
    {
    }
    public partial class Request_moduleSetRecord : IGenericRecord
    {
    }
    public partial class Request_terminateMethodRecord : IGenericRecord
    {
    }
    public partial class Response_startMethodRecord : IGenericRecord
    {
    }
    public partial class Request_setupExportRecord : IGenericRecord
    {
    }
    public partial class Request_getSettingRecord : IGenericRecord
    {
    }
    public partial class Response_getSettingRecord : IGenericRecord
    {
    }
    public partial class Request_setSettingRecord : IGenericRecord
    {
    }
    public partial class Request_loginRecord : IGenericRecord
    {
    }
    public partial class Response_loginRecord : IGenericRecord
    {
    }
    
}