using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using log4net;

namespace SERemoteConnection
{

    public class CommandSimpleRequest : AbstractCommand
    {
        public MT.pHLab.SE.V1.Response_SimpleResultRecord m_record;
        string m_simpleRequest;

        public CommandSimpleRequest(string simpleRequest)
        {
            m_simpleRequest = simpleRequest;
        }

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SESimpleRequest request = new MT.pHLab.SE.V1.SESimpleRequest(GetRequestId(), m_simpleRequest);
            connection.Write(request.CreateTelegram());

            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {
            bool success = false;

            MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_SimpleResultRecord))
            {
                m_record = new MT.pHLab.SE.V1.Response_SimpleResultRecord();
                m_record.ReadXml(reader);

                success = true;
                
            }

            setResponse();

            return success;
        }
    }

    public class CommandGetState : AbstractCommand
    {
        public MT.pHLab.SE.V1.Response_StatusRecord m_record;

        public CommandGetState()
        {

        }

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SESimpleRequest request = new MT.pHLab.SE.V1.SESimpleRequest(GetRequestId(), "connection.getStatus");
            connection.Write(request.CreateTelegram());

            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {
            bool success = false;

            MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_StatusRecord))
            {
                m_record = (MT.pHLab.SE.V1.Response_StatusRecord)response.CreateInstance();

                success = true;
            }

            setResponse();

            return success;
        }
    }

    public class CommandGetListOfModules : AbstractCommand
    {
        public MT.pHLab.SE.V1.moduleConfigParamRecord[] m_list;

        public CommandGetListOfModules()
        {

        }

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SESimpleRequest request = new MT.pHLab.SE.V1.SESimpleRequest(GetRequestId(), "module.get");
            connection.Write(request.CreateTelegram());
            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {
            bool success = false;

            MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_moduleGetRecord))
            {
                MT.pHLab.SE.V1.Response_moduleGetRecord record = new MT.pHLab.SE.V1.Response_moduleGetRecord();
                record.ReadXml(reader);
                m_list = record.m_moduleConfigs;
                success = true;
            }

            setResponse();

            return success;
        }
    }

    public class CommandSetModule : AbstractCommand
    {

        /*
<?xml version="1.0" encoding="utf-8" standalone="yes" ?>
<Telegram xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="LancePlatform">
  <Request requestId="145623" requestType="module.set">
    <record type="Request_moduleSetRecord">
      <param-list>
        <param name="m_moduleId" type="wstring">A</param>
        <param name="m_sensorId" type="wstring">MTPHSensor</param>
      </param-list>
    </record>
  </Request>
</Telegram>
         */
        public MT.pHLab.SE.V1.Request_moduleSetRecord m_request;
        public MT.pHLab.SE.V1.Response_SimpleResultRecord m_response;

        public CommandSetModule()
        {
            m_request = new MT.pHLab.SE.V1.Request_moduleSetRecord();
        }

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SEParamRequest request = new MT.pHLab.SE.V1.SEParamRequest(GetRequestId());
            var record = m_request;
            connection.Write(request.CreateTelegram(record));
            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {
            bool success = false;

            MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_SimpleResultRecord))
            {
                m_response = new MT.pHLab.SE.V1.Response_SimpleResultRecord();
                m_response.ReadXml(reader);

                success = m_response.m_bSuccess;

                setResponse();
            }

            return success;
        }
    }

    public class CommandTerminateMethod : AbstractCommand
    {
        public MT.pHLab.SE.V1.Request_terminateMethodRecord m_request;
        public MT.pHLab.SE.V1.Response_SimpleResultRecord m_response;

        public short JobId { set; get; }

        public CommandTerminateMethod()
        {
            m_request = new MT.pHLab.SE.V1.Request_terminateMethodRecord();
        }

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SEParamRequest request = new MT.pHLab.SE.V1.SEParamRequest(GetRequestId());
            m_request.m_jobId = JobId;
            var record = m_request;
            connection.Write(request.CreateTelegram(record));
            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {
            bool success = false;

            MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_SimpleResultRecord))
            {
                m_response = new MT.pHLab.SE.V1.Response_SimpleResultRecord();
                m_response.ReadXml(reader);

                success = m_response.m_bSuccess;

                setResponse();
            }

            return success;
        }
    }

    public class CommandGetListOfMethods : AbstractCommand
    {
        public string[] m_list;

        public CommandGetListOfMethods()
        {

        }

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SEParamRequest request = new MT.pHLab.SE.V1.SEParamRequest(GetRequestId());
            var record = new MT.pHLab.SE.V1.Request_setupGetItemListRecord();
            record.m_itemType = "Methods";
            connection.Write(request.CreateTelegram(record));
            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {
            bool success = false;

            MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_setupGetItemListRecord))
            {
                var record = new MT.pHLab.SE.V1.Response_setupGetItemListRecord();
                record.ReadXml(reader);
                m_list = record.m_itemList;
            }

            setResponse();

            return success;
        }
    }

    public class CommandGetListOfSensors : AbstractCommand
    {
        public string[] m_list;

        public CommandGetListOfSensors()
        {

        }

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SEParamRequest request = new MT.pHLab.SE.V1.SEParamRequest(GetRequestId());
            var record = new MT.pHLab.SE.V1.Request_setupGetItemListRecord();
            record.m_itemType = "Sensors";
            connection.Write(request.CreateTelegram(record));
            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {
            bool success = false;

            MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_setupGetItemListRecord))
            {
                var record = new MT.pHLab.SE.V1.Response_setupGetItemListRecord();
                record.ReadXml(reader);
                m_list = record.m_itemList;
            }

            setResponse();

            return success;
        }
    }

    public class CommandStartMethod : AbstractCommand
    {
        public string MethodId { set; get; }
        public string SampleId { set; get; }
        public string Comment { set; get; }

        public bool success = false;
        public short m_JobId;

        public CommandStartMethod()
        {

        }

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SEParamRequest request = new MT.pHLab.SE.V1.SEParamRequest(GetRequestId());
            var record = new MT.pHLab.SE.V1.Request_methodParamsRecord();
            record.m_methodId = MethodId;
            record.m_sampleId = SampleId;
            record.m_comment = Comment;
            connection.Write(request.CreateTelegram(record));
            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {

            MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_startMethodRecord))
            {
                var record = new MT.pHLab.SE.V1.Response_startMethodRecord();
                record.ReadXml(reader);
                success = record.m_methodId == MethodId;
                m_JobId = record.m_jobId;
            } else
            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_SimpleResultRecord))
            {
                var record = new MT.pHLab.SE.V1.Response_SimpleResultRecord();
                record.ReadXml(reader);

                success = record.m_bSuccess;
            }

            setResponse();

            return success;
        }
    }

    public enum EItemType
    {
        Sensors,
        Tables,
        Users,
        BufStd,
        Methods
    };

    /// <summary>
    /// Modified XML writer that writes (almost) no namespaces out with pretty formatting
    /// </summary>
    /// <seealso cref="http://blogs.msdn.com/b/kaevans/archive/2004/08/02/206432.aspx"/>
    public class XmlNoNamespaceWriter : XmlTextWriter
    {
        private bool _SkipAttribute = false;
        //private int _EncounteredNamespaceCount = 0;

        public XmlNoNamespaceWriter( TextWriter writer)
            : base(writer)
        {
            this.Formatting = System.Xml.Formatting.Indented; 
        }

        public override void WriteStartDocument()
        {
            // Do nothing (omit the declaration)
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            base.WriteStartElement(null, localName, null);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            //If the prefix or localname are "xmlns", don't write it.
            //HOWEVER... if the 1st element (root?) has a namespace we will write it.
            if ((prefix.CompareTo("xmlns") == 0 || 
                 localName.CompareTo("xmlns") == 0))
            {
                _SkipAttribute = true;
            }
            else
            {
                base.WriteStartAttribute(null, localName, null);
            }
        }

        public override void WriteString(string text)
        {
            //If we are writing an attribute, the text for the xmlns
            //or xmlns:prefix declaration would occur here.  Skip
            //it if this is the case.
            if (!_SkipAttribute)
            {
                base.WriteString(text);
            }
        }

        public override void WriteEndAttribute()
        {
            //If we skipped the WriteStartAttribute call, we have to
            //skip the WriteEndAttribute call as well or else the XmlWriter
            //will have an invalid state.
            if (!_SkipAttribute)
            {
                base.WriteEndAttribute();
            }
            //reset the boolean for the next attribute.
            _SkipAttribute = false;
        }

        public override void WriteQualifiedName(string localName, string ns)
        {
            //Always write the qualified name using only the
            //localname.
            base.WriteQualifiedName(localName, null);
        }
    }

    public class CommandExportTable : AbstractCommand
    {

        public bool success = false;
        protected EItemType m_eItemType;
        MT.pHLab.SE.V1.Response_SimpleResultRecord m_response;
        public byte[] Table { set; get;  }

        public CommandExportTable(EItemType eItemType)
        {
            m_eItemType = eItemType;
        }

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SEParamRequest request = new MT.pHLab.SE.V1.SEParamRequest(GetRequestId());
            var record = new MT.pHLab.SE.V1.Request_setupExportRecord();
            record.m_itemGroup = m_eItemType.ToString();
            record.m_itemName = "";
            connection.Write(request.CreateTelegram(record));
            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {
            reader.Read();
            if (reader.Name.Equals("pdk-db") && (reader.NodeType == XmlNodeType.Element))
            {
                XmlReader _reader = reader.ReadSubtree();
                XmlDocument doc = new XmlDocument();
                doc.Load(_reader);

                StringWriter textWriter = new StringWriter();
                using (var xmlWriter = new XmlNoNamespaceWriter(textWriter))
                {
                    doc.Save(xmlWriter);
                    Console.WriteLine(textWriter.ToString() );
                    Table = System.Text.Encoding.UTF8.GetBytes(textWriter.ToString());
                }
                success = true;
            }
            else
                if (reader.Name.Equals("record") && (reader.NodeType == XmlNodeType.Element))
                {
                    MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

                    if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_SimpleResultRecord))
                    {
                        m_response = new MT.pHLab.SE.V1.Response_SimpleResultRecord();
                        m_response.ReadXml(reader);

                        success = m_response.m_bSuccess;
                    }
                }

            setResponse();

            return success;
        }
    }

    public class CommandImportTable : AbstractCommand
    {

        public bool success = false;
        protected EItemType m_eItemType;
        MT.pHLab.SE.V1.Response_SimpleResultRecord m_response;

        public Byte[] Table { set; get; }

        public CommandImportTable()
        {
        }

        public override bool Submit(SEConnection connection)
        {
            XmlWriterSettings settings = new XmlWriterSettings();   
            settings.Indent = false;
            settings.Encoding = new UTF8Encoding(false);   
            using (MemoryStream output = new MemoryStream())   
            {
                using (var writer = XmlWriter.Create(output, settings ))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Telegram");
                    writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteStartElement("Request");
                    writer.WriteAttributeString("requestType", "setup.import");
                    writer.WriteAttributeString("requestId", GetRequestId().ToString());
                    {
                        string content = Encoding.UTF8.GetString(Table);
                        string unformated = XElement.Parse(content).ToString(SaveOptions.DisableFormatting);
                        Console.WriteLine(unformated);

                        writer.WriteRaw(unformated);
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                connection.Write(output.ToArray());
            }
            return true;
        }

//<?xml version="1.0" encoding="utf-8" standalone="yes" ?>
//<Telegram xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="LancePlatform">
//  <Response requestId="152015">
//    <record type="Response_SimpleResultRecord">
//      <param-list>
//        <param name="m_bSuccess" type="boolean">true</param>
//        <param name="m_errorReason" type="wstring" />
//      </param-list>
//    </record>
//  </Response>
//</Telegram>
        public override bool OnResponse(XmlReader reader)
        {
            //reader.Read();
            //if (reader.Name.Equals("record") && (reader.NodeType == XmlNodeType.Element))
            {
                MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

                if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_SimpleResultRecord))
                {
                    m_response = new MT.pHLab.SE.V1.Response_SimpleResultRecord();
                    m_response.ReadXml(reader);

                    success = m_response.m_bSuccess;
                }
            }

            setResponse();

            return success;
        }
    }

    public class CommandSettingGet : AbstractCommand
    {
        public string Setting { set; get; }
        public string Value { set; get; }

        public CommandSettingGet()
        {

        }

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SEParamRequest request = new MT.pHLab.SE.V1.SEParamRequest(GetRequestId());
            var record = new MT.pHLab.SE.V1.Request_getSettingRecord();
            record.m_settingName = Setting;
            connection.Write(request.CreateTelegram(record));
            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {
            bool success = false;

            MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_getSettingRecord))
            {
                var record = new MT.pHLab.SE.V1.Response_getSettingRecord();
                record.ReadXml(reader);
                Value = record.m_settingValue;
            }

            setResponse();

            return success;
        }
    }

    public class CommandSettingSet : AbstractCommand
    {
        public string Setting { set; get; }
        public string Value { set; get; }

        public CommandSettingSet()
        {

        }

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SEParamRequest request = new MT.pHLab.SE.V1.SEParamRequest(GetRequestId());
            var record = new MT.pHLab.SE.V1.Request_setSettingRecord();
            record.m_settingName = Setting;
            record.m_settingValue = Value;
            connection.Write(request.CreateTelegram(record));
            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {
            bool success = false;

            MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_SimpleResultRecord))
            {
                var record = new MT.pHLab.SE.V1.Response_SimpleResultRecord();
                record.ReadXml(reader);
                success = record.m_bSuccess;
            }

            setResponse();

            return success;
        }
    }



    public class CommandScreenLogin : AbstractCommand
    {
        public string[] Usererlist{ set; get; }
        public string Username{ set; get; }
        public string Password{ set; get; }

        public CommandScreenLogin()
        {

        }

        /*
         * <?xml version="1.0" encoding="utf-8" standalone="yes" ?>
        <Telegram xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="LancePlatform">
          <Request requestId="132440" requestType="login">
            <record type="Request_loginRecord">
              <param-list>
                <param name="m_usernames" type="sequence" sequenceType="wstring">
                  <item>Tom</item>
                  <item>Jerry</item>
                  <item>Alex</item>
                </param>
              </param-list>
            </record>
          </Request>
        </Telegram>
         * */

        public override bool Submit(SEConnection connection)
        {
            MT.pHLab.SE.V1.SEParamRequest request = new MT.pHLab.SE.V1.SEParamRequest(GetRequestId());
            var record = new MT.pHLab.SE.V1.Request_loginRecord();
            if (Usererlist != null)
            {
                record.m_usernames = Usererlist;
            }
            else
            {
                record.m_usernames = new string[] { };

            }
            connection.Write(request.CreateTelegram(record));
            return true;
        }

        public override bool OnResponse(XmlReader reader)
        {
            bool success = false;

            MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

            if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_loginRecord))
            {
                var record = new MT.pHLab.SE.V1.Response_loginRecord();
                record.ReadXml(reader);
                Username = record.m_username;
                Password = record.m_password;
            }

            setResponse();

            return success;
        }
    }

}
