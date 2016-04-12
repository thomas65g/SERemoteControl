using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using MT.pHLab.SE.V1;
using MT.Platform.Common;
using SERemoteLib;
using Services.NotificationBroker.Notifications;

namespace SERemoteConnection
{
    public abstract class AbstractCommand
    {
        private ManualResetEvent m_eventResponse;
        private int m_requestId;

        static int mg_requestId = 0;

        public AbstractCommand()
        {
            m_eventResponse = new ManualResetEvent(false);
            m_requestId = mg_requestId++;
        }

        public int GetRequestId()
        {
            return m_requestId;
        }

        public abstract bool OnResponse(XmlReader reader);
        public abstract bool Submit(SEConnection connection);

        public void setResponse()
        {
            m_eventResponse.Set();
        }

        public bool WaitForResponse()
        {
            return m_eventResponse.WaitOne();
        }
    }

    public abstract class ResultMessageSubscriber : ISubscriber
    {
        public string ObserverName { get; private set; }
        public ResultMessageSubscriber(string name)
        {
            this.ObserverName = name;
        }
        public abstract void Update( uint jobId, string xmlResultMessage );
    }

    interface ISubscriber
    {
        void Update( uint jobId, string xmlResultMessage  );
    }

    public class CommandDispatcher
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CommandDispatcher));

        private List<AbstractCommand> m_commandList;

        private SEConnection m_connection;

        private List<ResultMessageSubscriber> m_observers = new List<ResultMessageSubscriber>();

        public CommandDispatcher(SEConnection connection)
        {
            m_connection = connection;
            m_connection.OnTelegram = new OnTelegramDelegate(this.OnTelegram);
            m_commandList = new List<AbstractCommand>();
        }

        ~CommandDispatcher()
        {
        }

        public void Subscribe(ResultMessageSubscriber observer)
        {
            m_observers.Add(observer);
        }

        public void Unsubscribe(ResultMessageSubscriber observer)
        {
            m_observers.Remove(observer);
        }


        public bool Submit(AbstractCommand command)
        {
            bool success = false;

            m_commandList.Add(command);

            if (command.Submit(m_connection))
            {
                success = command.WaitForResponse();
            }

            return success;
        }

        public bool OnResponse(XmlReader reader)
        {
            bool success = false;

            int requestId = Int32.Parse(reader.GetAttribute("requestId"));
            if (requestId >= 0)
            {
                foreach (var iCommand in m_commandList)
                {
                    if (iCommand.GetRequestId() == requestId)
                    {
                        success= iCommand.OnResponse(reader);

                        m_commandList.Remove(iCommand);

                        break;
                    }
                }
            }
            return success;
        }

        private void OnResultMessage(uint jobId, XmlReader reader)
        {
            string xmlResultMessage = reader.ReadOuterXml();

            m_observers.ForEach(x => x.Update(jobId, xmlResultMessage));

            try
            {
                ResultMessage rm = SEResultMessageSerializer.Deserialize(xmlResultMessage);
                Log(string.Format(CultureInfo.InvariantCulture, "notifyAnalysisResult: #{0} {1}"
                    , jobId
                    , SEResultMessageSerializer.SerializeString(rm)
                    ));
            }
            catch (Exception ex)
            {
                Log(string.Format(CultureInfo.InvariantCulture, "notifyAnalysisResult: #{0} ERROR DESERIALIZING \n{1}\n{2}"
                    , jobId, ex.ToString(), xmlResultMessage));
            }

        }

        //<?xml version="1.0" encoding="utf-8" standalone="yes" ?>
        //<Telegram xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="LancePlatform">
        //  <Notification jobId="1">
        //    <method>
        //      <mf type="MFTitleRecord">
        //        <param-list>
        //   ...

        private void OnNotification(XmlReader reader)
        {
            string jobId = reader.GetAttribute("jopId");
            if (reader.Read())
            {
                if (reader.Name.Equals("ResultMessage") && (reader.NodeType == XmlNodeType.Element))
                {
                    uint id = Convert.ToUInt32(jobId, 16);
                    OnResultMessage(id, reader);
                }
                else
                    if (reader.Name.Equals("EndOfMethod") && (reader.NodeType == XmlNodeType.Element))
                    {

                    }
            }
        }

        public void OnTelegram(byte[] telegram)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            string dump = utf8.GetString(telegram);
            logger.DebugFormat("OnTelegram: {0}", dump);

            using (XmlReader reader = XmlReader.Create(new StringReader(dump)))
            {
                if (reader.ReadToFollowing("Telegram"))
                {
                    if (reader.Read())
                        if (reader.Name.Equals("Response") && (reader.NodeType == XmlNodeType.Element))
                        {
                            OnResponse(reader);
                        }
                        else
                            if (reader.Name.Equals("Notification") && (reader.NodeType == XmlNodeType.Element))
                            {
                                OnNotification(reader);
                            }
                }
            }
        }

        /// <summary>
        /// Logs the specified text by raising the LogWritten event.
        /// </summary>
        /// <param name="text">The text.</param>
        private void Log(string text)
        {
            text = "[C] " + text;
            NotificationBroker.Send(new LogWrittenNotification { Text = text, Sender = this.GetType().FullName, Stamp = DateTime.Now }, NotificationScope.Local);
        }


    }

}
