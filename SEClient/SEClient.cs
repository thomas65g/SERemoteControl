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
using System.Xml.Serialization;
using log4net;
using MT.Platform.Common;
using Services.NotificationBroker.Notifications;

namespace SERemoteConnection
{

    public class SEResult: ResultMessageSubscriber
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SEClient));

        public SEResult() :base("SEResult")
        {
        }

        public override void Update(uint jobId, string xmlResultMessage)
        {
            string text= string.Format("[R] jobId({0}) result({1}):", jobId, xmlResultMessage );
            NotificationBroker.Send(new LogWrittenNotification { Text = text, Sender = this.GetType().FullName, Stamp = DateTime.Now }, NotificationScope.Local);            
        }        
    }


    public class SEClient
    {
        private short m_jobId;
        private static readonly ILog logger = LogManager.GetLogger(typeof(SEClient));

        bool LoggingIsPending { set; get; }

        //private SEConnection m_connection;
        private CommandDispatcher m_commandDispatcher;

        private SEResult m_seResult;

        enum Status
        {
            Attached,
            Detached
        };

        private static IDictionary<String, Status> statsuMappings = new Dictionary<String, Status>()
        {
            { "Attached", Status.Attached },
            { "Detached", Status.Detached }
        };

        public SEClient(SEConnection connection)
        {
            m_commandDispatcher = new CommandDispatcher(connection);
            m_seResult = new SEResult();
            m_commandDispatcher.Subscribe(m_seResult);
            m_jobId = -1;
        }

        public void Open(string host, int port)
        {

        }

        public void Close()
        {

        }

        public bool Attach()
        {
            CommandSimpleRequest command = new CommandSimpleRequest("connect");

            bool success = false;
            m_commandDispatcher.Submit(command);

            if (command.WaitForResponse())
            {
                success = command.m_record.m_bSuccess;
            }

            return success;
        }

        public bool Detach()
        {
            CommandSimpleRequest command = new CommandSimpleRequest("disconnect");

            bool success = false;

            m_commandDispatcher.Submit(command);

            if (command.WaitForResponse())
            {
                success = command.m_record.m_bSuccess;
            }

            return success;
        }



        public string GetStatus()
        {
            string stringStatus = "not connected";

            CommandGetState command = new CommandGetState();

            m_commandDispatcher.Submit(command);

            if (command.WaitForResponse())
            {
                stringStatus = command.m_record.m_status;
            }

            return stringStatus;
        }

        public string[] getListOfMethods()
        {
            string[] list = new string[0];

            CommandGetListOfMethods command = new CommandGetListOfMethods();

            m_commandDispatcher.Submit(command);

            if (command.WaitForResponse())
            {
                list = command.m_list;
            }

            return list;

        }

        public string[] getListOfSensors()
        {
            string[] list = new string[0];

            CommandGetListOfSensors command = new CommandGetListOfSensors();

            m_commandDispatcher.Submit(command);

            if (command.WaitForResponse())
            {
                list = command.m_list;
            }

            return list;

        }

        public MT.pHLab.SE.V1.moduleConfigParamRecord[] getListOfModules()
        {
            MT.pHLab.SE.V1.moduleConfigParamRecord[] list = null;

            CommandGetListOfModules command = new CommandGetListOfModules();

            m_commandDispatcher.Submit(command);

            if (command.WaitForResponse())
            {
                list = command.m_list;
            }

            return list;
        }

        public bool setModule(string moduleId, string sensorId)
        {
            bool success = false;

            CommandSetModule command = new CommandSetModule();
            command.m_request.m_moduleId = moduleId;
            command.m_request.m_sensorId = sensorId;

            m_commandDispatcher.Submit(command);

            if (command.WaitForResponse())
            {
                success = command.m_response.m_bSuccess;
            }
            return success;
        }

        public bool startMethod(string methodId, string sampleId, string comment)
        {
            bool success = false;

            CommandStartMethod command = new CommandStartMethod();
            command.MethodId = methodId;
            command.SampleId = sampleId;
            command.Comment = comment;

            m_commandDispatcher.Submit(command);

            if (command.WaitForResponse())
            {
                success = command.success;
                m_jobId = command.m_JobId;
            }

            return success;
        }

        public bool terminateMethod()
        {
            bool success = false;

            CommandTerminateMethod command = new CommandTerminateMethod();
            command.JobId = m_jobId;

            m_commandDispatcher.Submit(command);

            if (command.WaitForResponse())
            {
                success = command.m_response.m_bSuccess;
            }
            return success;
        }

        public byte[] exportTable( EItemType eItemType )
        {
            CommandExportTable command = new CommandExportTable(eItemType);
            m_commandDispatcher.Submit(command);
            if (command.WaitForResponse())
            {
                return command.Table;
            }
            return null;
        }

        public bool importTable( byte[] table )
        {
            CommandImportTable command = new CommandImportTable();
            command.Table= table;

            m_commandDispatcher.Submit(command);
            if (command.WaitForResponse())
            {

            }
            return true;
        }

        public string getSettings( string setting )
        {
            CommandSettingGet command = new CommandSettingGet();
            command.Setting = setting;

            m_commandDispatcher.Submit(command);
            if (command.WaitForResponse())
            {

            }
            return command.Value;
        }

        public bool setSettings(string setting, string value )
        {
            CommandSettingSet command = new CommandSettingSet();
            command.Setting = setting;
            command.Value = value;

            m_commandDispatcher.Submit(command);
            if (command.WaitForResponse())
            {
                return true;
            }
            return false;
        }

        public bool showScreenLogin( string[] userlist, ref string username, ref string password )
        {

            CommandScreenLogin command = new CommandScreenLogin();
            command.Usererlist = userlist;

            LoggingIsPending = true;

            m_commandDispatcher.Submit(command);
            if (command.WaitForResponse())
            {
                username= command.Username;
                password= command.Password;
            }

            LoggingIsPending = false;
            return true;
        }

        public bool showScreenLoginCancel()
        {
            CommandSimpleRequest command = new CommandSimpleRequest("login.loginCancel");

            bool success = false;
            m_commandDispatcher.Submit(command);

            if (command.WaitForResponse())
            {
                success = command.m_record.m_bSuccess;
            }

            return success;
        }
    }

}
