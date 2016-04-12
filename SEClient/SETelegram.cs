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

    public abstract class SERequest
    {
        public static IDictionary<Type, String> typeMapping = new Dictionary<Type, String>()
        {
            { typeof(Response_SimpleResultRecord), "Response_SimpleResultRecord" },
            { typeof(Response_moduleGetRecord), "Response_moduleGetRecord" },
            { typeof(Request_methodParamsRecord), "Request_methodParamsRecord" },
            { typeof(Request_setupGetItemListRecord), "Request_setupGetItemListRecord" },
            { typeof(Request_setupExportRecord), "Request_setupExportRecord" },
        };

        public static IDictionary<Type, String> requestMapping = new Dictionary<Type, String>()
        {
            { typeof(Response_SimpleResultRecord), "Response_SimpleResultRecord" },
            { typeof(Response_StatusRecord), "Response_StatusRecord" },
            { typeof(Response_moduleGetRecord), "Response_moduleGetRecord" },
            { typeof(Request_methodParamsRecord), "method.start" },
            { typeof(Request_setupGetItemListRecord), "setup.getItemList" },
            { typeof(Request_moduleSetRecord), "module.set" },
            { typeof(Request_terminateMethodRecord), "method.terminate" },
            { typeof(Request_setupExportRecord), "setup.export" },
            { typeof(Request_setSettingRecord ), "setting.setItem" },
            { typeof(Request_getSettingRecord ), "setting.getItem" },
            { typeof(Request_loginRecord), "login" }
        };

        public int RequestId { get; private set; }

        public SERequest(int requestId)
        {
            RequestId = requestId;
        }

		public static String MapRequest(Type type)
		{
			if (requestMapping.ContainsKey(type))
			{
                return requestMapping[type];
			}
            return null;
        }

        public void WriteXml( IGenericRecord record, XmlWriter writer )
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Telegram");
            writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteStartElement("Request");
                writer.WriteAttributeString("requestType", MapRequest(record.GetType()));
                writer.WriteAttributeString("requestId", RequestId.ToString() );
                PDKXmlWriter.WriteRecord(record, writer );
                writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }

    public class SESimpleRequest : SERequest
    {
        string m_stringSimpleRequest;

        public SESimpleRequest( int requestId, string request)
            : base(requestId)
        {            
            m_stringSimpleRequest= request;
        }

        public void WriteXml( XmlWriter writer )
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Telegram");
            writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            //writer.WriteAttributeString("xmlns", "tm");
                writer.WriteStartElement("Request");
                writer.WriteAttributeString("requestType", m_stringSimpleRequest);
                writer.WriteAttributeString("requestId", this.RequestId.ToString());
                writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        public byte[] CreateTelegram()
        {
            XmlWriterSettings settings = new XmlWriterSettings();   
            settings.Indent = false;
            settings.Encoding = new UTF8Encoding(false);   
            using (MemoryStream output = new MemoryStream())   
            {
                //using (var xw = XmlTextWriter.Create(output, new UTF8Encoding(false)))
                using (var xw = XmlWriter.Create(output, settings ))
                {
                    // Build Xml with xw.
                    WriteXml(xw);
                }
                return output.ToArray();
            }
        }
    }

    public class SEParamRequest : SERequest
    {
        public SEParamRequest(int requestId)
            : base(requestId)
        {
        }

        public byte[] CreateTelegram(IGenericRecord record  )
        {
            //Encoding utf8noBOM = new UTF8Encoding(false);   
            //using (MemoryStream output = new MemoryStream())   
            //{   
            //    using (XmlWriter writer = XmlWriter.Create(output, settings))   
            //    {   
            //        writer.WriteStartDocument();   
            //        writer.WriteStartElement("Colors");   
            //        writer.WriteElementString("Color", "RED");   
            //        writer.WriteEndDocument();   
            //    }   
            //    result = Encoding.Default.GetString(output.ToArray());   
            //}   
            XmlWriterSettings settings = new XmlWriterSettings();   
            settings.Indent = false;
            settings.Encoding = new UTF8Encoding(false);   
            using (MemoryStream output = new MemoryStream())   
            {
                //using (var xw = XmlTextWriter.Create(output, new UTF8Encoding(false)))
                using (var xw = XmlWriter.Create(output, settings ))
                {
                    // Build Xml with xw.
                    WriteXml(record, xw);
                }
                return output.ToArray();
            }
        }
    }


    public class SEResponse
    {
        /*
        * <?xml version="1.0" encoding="utf-8" standalone="yes" ?>
        * <Telegram xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="LancePlatform">
        * <Response requestId="102478">
        * <record type="Response_StatusRecord">
        * <param-list>
        * <param name="m_status" type="wstring">Attached</param>
        * </param-list>
        * </record>
        * </Response>
        * </Telegram>
        */
        private static IDictionary<String, Type> typeMappings = new Dictionary<String, Type>()
        {
            { "Response_SimpleResultRecord", typeof(Response_SimpleResultRecord) },
            { "Response_StatusRecord", typeof(Response_StatusRecord) },
            { "Response_moduleGetRecord", typeof(Response_moduleGetRecord) },
            { "Response_setupGetItemListRecord", typeof(Response_setupGetItemListRecord) },
            { "Response_startMethodRecord", typeof(Response_startMethodRecord) },
            { "Response_getSettingRecord", typeof(Response_getSettingRecord) },
            { "Response_loginRecord", typeof(Response_loginRecord) },
        };

        XmlReader m_reader;

        string m_recordType;

        static public SEResponse CreateFromXml(XmlReader reader)
        {
            SEResponse response = null;

            // Build Xml with xw.
            response = new SEResponse(reader);

            return response;
        }

        public SEResponse( XmlReader reader )
        {
            m_reader = reader;
            m_reader.ReadToFollowing("record");
            m_recordType = m_reader.GetAttribute("type");
        }


        public Type GetRecordType()
        {
            return typeMappings[m_recordType];
        }

        public object CreateInstance()
        {
            if (typeMappings[m_recordType] == typeof(MT.pHLab.SE.V1.Response_StatusRecord) )
            {
                MT.pHLab.SE.V1.Response_StatusRecord record = new MT.pHLab.SE.V1.Response_StatusRecord();

                record.ReadXml(m_reader);
                return record;
            } else
            if (typeMappings[m_recordType] == typeof(MT.pHLab.SE.V1.Response_setupGetItemListRecord))
            {
                MT.pHLab.SE.V1.Response_setupGetItemListRecord record = new MT.pHLab.SE.V1.Response_setupGetItemListRecord();

                record.ReadXml(m_reader);
                return record;
            }
            return null;
        }

    }

}