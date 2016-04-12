using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MT.pHLab.SE.V1;
using SERemoteLib;




namespace UnitTestProject
{
    internal static class PhInstrumentSerializationHelper
    {

        /// <summary>
        /// Tries do deserialize the given xml data to the given type.
        /// </summary>
        /// <returns>
        /// An object of the given type if the data could be deserialized, <c>null</c>
        /// otherwise.
        /// </returns>
        public static T DeserializeDeviceData<T>(this string xml)
        {
            object deviceData = null;
            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    XmlRootAttribute xmlRoot = new XmlRootAttribute();
                    xmlRoot.Namespace = "tf";
                    xmlRoot.IsNullable = true;

                    var serializer = new XmlSerializer(typeof(T), xmlRoot);
                    deviceData = serializer.Deserialize(reader);
                }
            }
            catch (XmlException )
            {
            }
            catch (InvalidOperationException )
            {
            }
            return (T)deviceData;
        }
    }


    [TestClass]
    public class UnitTestResultMessage
    {
        [TestMethod]
        public void Test01()
        {
            string telegram = "<IntegerResult xmlns='LancePlatform'><timestamp>2016-02-03T12:29:08.4670</timestamp><value>1</value></IntegerResult>";

            StringReader stringReader = new StringReader(telegram);
            XmlTextReader reader = new XMLSerializationCustomization.PdkXmlTextReader(stringReader);

            {
                XmlRootAttribute xmlRoot = new XmlRootAttribute();
                xmlRoot.Namespace = "tf";
                xmlRoot.IsNullable = true;

                IntegerResult objResultMessage = new IntegerResult();
                XmlSerializer xmlser = new XmlSerializer(objResultMessage.GetType(), xmlRoot);
                objResultMessage = (IntegerResult)xmlser.Deserialize(reader);
            }
        }

        [TestMethod]
        public void Test01StringResult()
        {
            string telegram = "<StringResult xmlns='LancePlatform'><timestamp>2016-02-04T09:27:32.7300</timestamp><value>Hello world, here I am</value></StringResult>";

            StringReader stringReader = new StringReader(telegram);
            XmlTextReader reader = new XMLSerializationCustomization.PdkXmlTextReader(stringReader);

            {
                StringResult objResultMessage = new StringResult();

                XmlRootAttribute xmlRoot = new XmlRootAttribute();
                xmlRoot.Namespace = "tf";
                xmlRoot.IsNullable = true;

                XmlSerializer xmlser = new XmlSerializer(objResultMessage.GetType(), xmlRoot);
                objResultMessage = (StringResult)xmlser.Deserialize(reader);
            }
        }

        [TestMethod]
        public void Test01Decfloat()
        {
            //string telegram = "<DecimalResult xmlns='LancePlatform'><timestamp>2016-02-04T09:27:32.7300</timestamp><state>eDecimalResultState_Valid</state><value>1.034</value></DecimalResult>";
            string telegram = "<DecimalResult xmlns='tf'><timestamp>2016-02-04T09:27:32.7300</timestamp><state>0</state><value>1.034</value></DecimalResult>";

            StringReader stringReader = new StringReader(telegram);
            XmlTextReader reader = new XMLSerializationCustomization.PdkXmlTextReader(stringReader);

            {
                DecimalResult objResultMessage = new DecimalResult();

                XmlRootAttribute xmlRoot = new XmlRootAttribute();
                xmlRoot.Namespace = "tf";
                xmlRoot.IsNullable = true;

                XmlSerializer xmlser = new XmlSerializer(objResultMessage.GetType(), xmlRoot);
                objResultMessage = (DecimalResult)xmlser.Deserialize(reader);
            }
        }


        [TestMethod]
        public void Test02_ResultMessage()
        {
            //string BAD telegram =  "<ResultMessage xmlns='LancePlatform'><rackid></rackid><slotid></slotid><groupid>MeasType1:Measure1</groupid><unit>0</unit><unitstring></unitstring><precision>0</precision><result><ResultPh><rawValue>-235.0</rawValue><rawTemperature>23.4</rawTemperature><temperatureUnit>31</temperatureUnit><resultValue>10.99</resultValue><resultUnit>10</resultUnit><resultQuality>2</resultQuality><resultLimitState>0</resultLimitState><timeStamp>2016-02-04 14:56:54</timeStamp></ResultPh></result></ResultMessage>";
            //string telegram = "<ResultMessage xmlns='LancePlatform'><rackid></rackid><slotid></slotid><groupid>MeasType1:Measure1</groupid><unit>0</unit><unitstring></unitstring><precision>0</precision><result><DecimalResult><timestamp>2016-02-04T09:27:32.7300</timestamp><state>eDecimalResultState_Valid</state><value>1.034</value></DecimalResult></result></ResultMessage>";
            // OK string telegram =  "<?xml version='1.0' encoding='utf-8' standalone='yes' ?><Telegram xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns='LancePlatform'><Notification jobId='2'><ResultMessage><rackid></rackid><slotid></slotid><groupid>TIMESTAMPS</groupid><unit>0</unit><unitstring></unitstring><precision>0</precision><result><IntegerResult><timestamp>2016-02-04T09:27:32.7300</timestamp><value>1</value></IntegerResult></result></ResultMessage></Notification></Telegram>";
            // OK string telegram = "<ResultMessage xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='tf'><rackid>A</rackid><slotid>B</slotid><groupid>TIMESTAMPS</groupid><unit>0</unit><unitstring></unitstring><precision>0</precision><result><IntegerResult><timestamp>2016-02-03T12:29:08.4670</timestamp><value>1</value></IntegerResult></result></ResultMessage>";
            // OK string telegram = "<ResultMessage xmlns='tf'><rackid>A</rackid><slotid>B</slotid><groupid>TIMESTAMPS</groupid><unit>0</unit><unitstring></unitstring><precision>0</precision><result><IntegerResult><timestamp>2016-02-03T12:29:08.4670</timestamp><value>1</value></IntegerResult></result></ResultMessage>";
            //string telegram = "<ResultMessage><rackid>A</rackid><slotid>B</slotid><groupid>TIMESTAMPS</groupid><unit>0</unit><unitstring></unitstring><precision>0</precision><result><IntegerResult><timestamp>2016-02-03T12:29:08.4670</timestamp><value>1</value></IntegerResult></result></ResultMessage>";
            string telegram = "<ResultMessage  xmlns='tf'><rackid>Hallo</rackid><slotid>Echo</slotid><groupid>MeasType1:Measure1</groupid><unit>0</unit><unitstring></unitstring><precision>0</precision><result><ResultPh><rawValue>174.5</rawValue><rawTemperature>24.5</rawTemperature><temperatureUnit>31</temperatureUnit><resultValue>4.04</resultValue><resultUnit>10</resultUnit><resultQuality>2</resultQuality><resultLimitState>0</resultLimitState><timeStamp>2016-02-05 17:54:07</timeStamp></ResultPh></result></ResultMessage>";

            StringReader stringReader= new StringReader(telegram);
            XmlTextReader reader = new XMLSerializationCustomization.PdkXmlTextReader(stringReader);

            reader.ReadToFollowing("ResultMessage");

            {
                ResultMessage objResultMessage = new ResultMessage();

                //var resultMessage = PhInstrumentSerializationHelper.DeserializeDeviceData<ResultMessage>(telegram);
                XmlRootAttribute xmlRoot = new XmlRootAttribute();
                xmlRoot.Namespace = "tf";
                xmlRoot.IsNullable = true;

                XmlSerializer xmlser = new XmlSerializer(typeof(ResultMessage), xmlRoot);
                objResultMessage = (ResultMessage)xmlser.Deserialize(reader);
            }
        }

        [TestMethod]
        public void Test03()
        {
            string xmlstring;

            {
                StringWriter stream = new StringWriter();

                ResultMessage message = new ResultMessage();
                message.groupid = "GroupId";
                message.rackid = "RackId";
                message.slotid = "SlotId";
                message.result = new TreasureFleetAnyResult();
                message.result.Item = new DecimalResult();
                ((DecimalResult)message.result.Item).value = 4712.4m;

                XmlSerializer serializer = new XmlSerializer(message.GetType());
                serializer.Serialize(stream, message);

                xmlstring = stream.ToString();
            }


            {
                StringReader xmlStream = new StringReader(xmlstring);
                ResultMessage message = new ResultMessage();

                XmlRootAttribute xmlRoot = new XmlRootAttribute();
                //xmlRoot.Namespace = "tf";
                xmlRoot.ElementName = "Telegram";
                xmlRoot.IsNullable = true;

                XmlSerializer xmlser = new XmlSerializer(message.GetType());
                message = (ResultMessage)xmlser.Deserialize(xmlStream);
            }
        }

        [TestMethod]
        public void Test04()
        {
            string xmlstring;
            {
                StringWriter stream = new StringWriter();

                ResultMessage message = new ResultMessage();
                message.groupid = "GroupId";
                message.rackid = "RackId";
                message.slotid = "SlotId";
                message.result = new TreasureFleetAnyResult();
                message.result.Item = new DecimalResult();
                ((DecimalResult)message.result.Item).value = 4712.4m;

                XmlRootAttribute xmlRoot = new XmlRootAttribute();
                xmlRoot.Namespace = "tf";
                xmlRoot.IsNullable = true;
                XmlSerializer serializer = new XmlSerializer(message.GetType(), xmlRoot);
                serializer.Serialize(stream, message);

                xmlstring = stream.ToString();
            }


            {
                StringReader xmlStream = new StringReader(xmlstring);
                ResultMessage message = new ResultMessage();

                XmlRootAttribute xmlRoot = new XmlRootAttribute();
                xmlRoot.Namespace = "tf";
                xmlRoot.IsNullable = true;

                XmlSerializer xmlser = new XmlSerializer(message.GetType(), xmlRoot);
                message = (ResultMessage)xmlser.Deserialize(xmlStream);
            }
        }

      
        [TestMethod]
        public void Test05_Response()
        {
            string telegram= "<?xml version='1.0' encoding='utf-8' standalone='yes' ?><Telegram xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns='LancePlatform'><Response requestId='0'><record type='Response_setupGetItemListRecord'><param-list><param name='m_itemList' type='sequence' sequenceType='wstring'><item>M001</item><item>M002</item><item>M003</item><item>M004</item><item>M005</item><item>M006</item><item>M007</item><item>M008</item><item>M009</item><item>M010</item><item>M011</item><item>M012</item><item>M013</item><item>M014</item><item>M015</item><item>M016</item><item>M017</item><item>M018</item><item>M019</item><item>M020</item><item>M021</item></param></param-list></record></Response></Telegram>";
            {
                XmlReader reader = XmlReader.Create(new StringReader(telegram));
                MT.pHLab.SE.V1.SEResponse response = MT.pHLab.SE.V1.SEResponse.CreateFromXml(reader);

                if (response.GetRecordType() == typeof(MT.pHLab.SE.V1.Response_setupGetItemListRecord))
                {
                    var record = new MT.pHLab.SE.V1.Response_setupGetItemListRecord();
                    record.ReadXml(reader);
                }
            }
        }
    }
}
