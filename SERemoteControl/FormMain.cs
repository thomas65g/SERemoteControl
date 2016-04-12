using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using MT.Platform.Common;
using SERemoteLib;
using Services.NotificationBroker.Notifications;

namespace SERemoteControl
{
    public partial class FormMain : Form
    {
        private SERemoteConnection.SEClient m_client;
        private SERemoteConnection.SEConnection m_connection;

        /// <summary>
        /// Make use of logging infrastructure.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(FormMain));

        /// <summary>
        /// This logger property is used to log events
        /// </summary>
        public static ILog Logger
        {
            get { return _logger; }
        }

        /// <summary>
        /// Marshal calls back to the UI thread. Do not access UI controls from background threads (e.g. callback operation calls!)
        /// </summary>
        private readonly SynchronizationContext uiSynchronizationContext;

        public FormMain()
        {
            InitializeComponent();

            uiSynchronizationContext = SynchronizationContext.Current;

            //((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).Root.AddAppender(this);

            // register a notification handler to log
            NotificationBroker.Register(typeof(LogWrittenNotification), (object thesender, Notification notification) =>
            {
                LogWrittenNotification logNotification = notification as LogWrittenNotification;

                // only write on UI thread!
                uiSynchronizationContext.Send((object state) =>
                {
                    Log(state as string);
                }, logNotification.Text);
            }, null);


            this.comboBox2.DataSource = Enum.GetValues(typeof(SERemoteConnection.EItemType));

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = String.Format("Mettler Toledo - SE Cockpit V{0}", version);

            EnableTabAndControls(false);
        }

        public void EnableTabAndControls( bool enable )
        {
            EnableTab(tabPage2, enable);
            EnableTab(tabPage3, enable);
            EnableTab(tabPage4, enable);
            EnableTab(tabPage5, enable);
            EnableTab(tabPage6, enable);
            buttonConnect.Enabled= !enable;
            buttonDetach.Enabled= enable;
            buttonGetState.Enabled = enable;
            buttonAttach.Enabled= enable;
            buttonDisconnect.Enabled= enable;
        }

        public static void EnableTab(TabPage page, bool enable)
        {
            foreach (Control ctl in page.Controls) ctl.Enabled = enable;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            m_connection = new SERemoteConnection.SEConnection();

            string address = this.comboBoxSevenExcellence.Text;

            try
            {
                Uri uri = new Uri(address);

                if (m_connection.Open(uri.Host, uri.Port))
                {
                    m_client = new SERemoteConnection.SEClient(m_connection);

                    EnableTabAndControls(true);

                    if (this.comboBoxSevenExcellence.FindString(address) == -1)
                    {
                        this.comboBoxSevenExcellence.Items.Add(address);
                    }

                }
            }
            catch( UriFormatException  )
            {
            }
        }

        private void buttonAttach_Click(object sender, EventArgs e)
        {
            if (m_client!=null)
                m_client.Attach();
        }

        private void buttonDetach_Click(object sender, EventArgs e)
        {
            if (m_client != null)
                m_client.Detach();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (m_client != null)
            {
                m_connection.Close();
                EnableTabAndControls(false);
            }
        }

        private void buttonGetState_Click(object sender, EventArgs e)
        {
            if (m_client != null)
                textBoxState.Text = m_client.GetStatus();
        }

        private void buttonStartMethod_Click(object sender, EventArgs e)
        {
            if (m_client != null)
            {
                int index= this.listboxListOfMethods.SelectedIndex;
                if (index>=0)
                {
                    string methodId= this.listboxListOfMethods.SelectedItem.ToString();
                    string comment= this.textBoxMethodInstruction.Text;
                    string sampleId = this.textBoxMethodSampleId.Text;
                    m_client.startMethod(methodId, sampleId, comment);
                }
            }
        }


        private void buttonTermiate_Click(object sender, EventArgs e)
        {
            if (m_client != null)
            {
                m_client.terminateMethod();
            }
        }

        private void LogUI(string text)
        {
            // make sure we are on UI thread
            if (this.InvokeRequired)
            {
                throw new InvalidOperationException("must only be called on UI thread!");
            }

            // log to list view
            ListViewItem item = listviewLog.Items.Add(new ListViewItem(new string[] { DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture), text }));
            item.EnsureVisible();
        }

        /// <summary>
        /// Write to list view log and scroll automatically.
        /// </summary>
        /// <param name="text"></param>
        public void Log(string text)
        {
            if (this.InvokeRequired)
            {
                // switch to UI thread, because call came from bg thread
                this.uiSynchronizationContext.Send((state) => { LogUI(text); }, null);
            }
            else
            {
                LogUI(text);
            }
        }

        private void buttonMethods_Click(object sender, EventArgs e)
        {
            if (m_client != null)
            {
                this.listboxListOfMethods.Items.Clear();
                string[] methods = m_client.getListOfMethods();
                if (methods!=null)
                    this.listboxListOfMethods.Items.AddRange(methods);
            }
        }

        private void buttonModules_Click(object sender, EventArgs e)
        {
            if (m_client != null)
            {
                this.listView1.Items.Clear();
                this.comboBoxModule.Items.Clear();

                MT.pHLab.SE.V1.moduleConfigParamRecord[] modules = m_client.getListOfModules();

                if (modules!=null)
                {
                    foreach (MT.pHLab.SE.V1.moduleConfigParamRecord module in modules)
                    {
                        ListViewItem item = new ListViewItem(module.m_moduleId);
                        item.SubItems.Add(module.m_sensorId);
                        item.SubItems.Add(module.m_tempSensorId);
                        this.listView1.Items.Add(item);
                        this.comboBoxModule.Items.Add(module.m_moduleId);
                    }
                }
            }
        }

        private void buttonConnectSensor_Click(object sender, EventArgs e)
        {
            if (m_client != null)
            {
                this.listView2.Items.Clear();
                this.comboBoxSensor.Items.Clear();

                this.listboxListOfMethods.Items.Clear();
                string[] sensors = m_client.getListOfSensors();

                foreach (string sensor in sensors)
                {
                    ListViewItem item = new ListViewItem(sensor);
                   // item.SubItems.Add(sensor);
                    this.listView2.Items.Add(item);
                    this.comboBoxSensor.Items.Add(sensor);
                }
            }
        }

        private void buttonConnectSensorToModule_Click(object sender, EventArgs e)
        {
            string sensorId = this.comboBoxSensor.Text;
            string moduleId = this.comboBoxModule.Text;

            if (m_client != null && moduleId != null && sensorId != null)
            {
                m_client.setModule(moduleId, sensorId);
                buttonModules_Click(sender, e);
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (m_client != null)
            {
                SERemoteConnection.EItemType eItemType = (SERemoteConnection.EItemType)Enum.Parse(typeof(SERemoteConnection.EItemType), this.comboBox2.SelectedValue.ToString());

                Byte[] table= null;
                table= m_client.exportTable(eItemType);

                if (table != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();

                    saveFileDialog.FileName = "Export-" + eItemType.ToString() + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    // the filter box and no extension is specified by the user.
                    saveFileDialog.DefaultExt = "xml";
                    saveFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.RestoreDirectory = true;
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        Stream myStream;
                        if ((myStream = saveFileDialog.OpenFile()) != null)
                        {
                            using (BinaryWriter writer = new BinaryWriter(myStream, Encoding.UTF8))
                            {
                                writer.Write(table);
                            }
                            myStream.Close();
                        }
                    }
                }
            }            
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (m_client != null)
            {
                Stream myStream = null;
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((myStream = openFileDialog.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                // Insert code to read the stream here.
                                byte[] table= ReadToEnd(myStream);
                                if (table != null)
                                {
                                    m_client.importTable(table);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string moduleId = this.listView1.SelectedItems[0].Text;

            if (moduleId != null)
            {
                this.comboBoxModule.SelectedIndex = this.comboBoxModule.FindStringExact(moduleId);
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sensorId = this.listView2.SelectedItems[0].Text;

            if (sensorId != null)
            {
                this.comboBoxSensor.SelectedIndex = this.comboBoxSensor.FindStringExact(sensorId);
            }
        }

        private void buttonRequestSetupItemListt_Click(object sender, EventArgs e)
        {
            if (m_client != null)
            {
                string[] settingNames = {
                    "UserSetting.guiLanguage",                         
                    "UserSetting.protocolLanguage",                    
                    "UserSetting.settingsBrightness",                  
                    "UserSetting.Beep.buttonBeep",                     
                    "UserSetting.Beep.userBeep",                       
                    "UserSetting.Beep.newsBeep",                       
                    "UserSetting.Beep.errorBeep",                      
                    "UserSetting.Beep.stabilityBeep",                  
                    "UserSetting.alphanumericKeyboard",                
                    "GlobalSetting.confirmEndOfAnalysis",              
                    "GlobalSetting.showRequiredResourcesAtStart",      
                    "GlobalSetting.showCalulatedResultsAfterAnalysis", 
                    "GlobalSetting.bSuppresPopups",                    
                    "GlobalSetting.globalTemperatureUnit",             
                    "GlobalSetting.globalBarometricPressureUnit",      
                    "GlobalSetting.lifeSpanExpireAction",              
                    "GlobalSetting.usableLifeExpireAction",            
                    "GlobalSetting.DataTime",                          
                    "GlobalSetting.Identification_ID",                 
                    //"NetworkSetting.keepAlive"  
                };

                this.comboBoxSettingTag.Items.Clear();

                this.listViewSettings.Items.Clear();

                foreach (string settingName in settingNames)
                {
                    string value = m_client.getSettings(settingName);

                    ListViewItem item = new ListViewItem(settingName);
                    item.SubItems.Add(value);
                    this.listViewSettings.Items.Add(item);

                    this.comboBoxSettingTag.Items.Add(settingName);
                }
            }
        }

        private void listViewSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            string settingTag = this.listViewSettings.SelectedItems[0].Text;

            if (settingTag != null)
            {
                this.comboBoxSettingTag.SelectedIndex = this.comboBoxSettingTag.FindStringExact(settingTag);
            }
        }

        private void buttonSetupSetting_Click(object sender, EventArgs e)
        {
            string settingTag = this.comboBoxSettingTag.Text;
            string settingValue = this.textBoxSettingValue.Text;

            if (m_client != null && settingTag != null && settingValue != null)
            {
                m_client.setSettings(settingTag, settingValue);
                buttonRequestSetupItemListt_Click(sender, e);
            }
        }

        private void FormMain_ResizeEnd(object sender, EventArgs e)
        {

        }

        private void listviewLog_Resize(object sender, System.EventArgs e)
        {
            SizeLastColumn((ListView)sender);
        }

        private void SizeLastColumn(ListView lv)
        {
            this.listviewLog.Columns[this.listviewLog.Columns.Count - 1].Width = -2;
        }


        private void listviewLog_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listviewLog.SelectedItems)
            {
                MessageBox.Show(item.SubItems[1].Text, item.SubItems[0].Text);
            }
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {

        }

        private void FormMain_FormClose(object sender, EventArgs e)
        {
            if (m_connection != null)
                m_connection.Close();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (SERemoteControl.Properties.Settings.Default.AddressHistory != null)
            {
                try
                {
                    StringReader sr = new StringReader(SERemoteControl.Properties.Settings.Default.AddressHistory);
                    var adresses = sr.ReadLine().Split(';');
                    this.comboBoxSevenExcellence.Items.AddRange(adresses.ToArray());
                }
                catch (System.NullReferenceException)
                {
                    this.comboBoxSevenExcellence.Items.Add("http://localhost:8016");
                    this.comboBoxSevenExcellence.Items.Add("http://localhost:8014");
                }

                string address= SERemoteControl.Properties.Settings.Default.CurrentAddress;
                int index= this.comboBoxSevenExcellence.FindString(address);
                if ( index!= -1)
                {
                    this.comboBoxSevenExcellence.SelectedIndex= index;
                }
            }

            this.textBoxUsernameList.Text= "Thomas;Xufeng;Toni;Max";
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SERemoteControl.Properties.Settings.Default.AddressHistory != null)
            {
                
                var list= new List<string>();
                {
                    foreach (var item in this.comboBoxSevenExcellence.Items)
                    {
                        list.Add(item.ToString());
                    }
                }

                SERemoteControl.Properties.Settings.Default.CurrentAddress = this.comboBoxSevenExcellence.Text;
                SERemoteControl.Properties.Settings.Default.AddressHistory = string.Join(";", list.ToArray());

                SERemoteControl.Properties.Settings.Default.Save();
            }
                
        }

        struct LoginRequestResult
        {
            public string username;
            public string password;
        }

        LoginRequestResult LoginRequest()
        {
            string[] usernamelist = null;
            if (this.checkBoxProposedUsers.Checked)
            {
                StringReader sr = new StringReader(this.textBoxUsernameList.Text);
                usernamelist = sr.ReadLine().Split(';');
            }

            LoginRequestResult result;
            result.username= "";
            result.password= "";

            if (m_client.showScreenLogin(usernamelist, ref result.username, ref result.password))
            {
                //
            }
            return result;
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            Task<LoginRequestResult> returnedTask = Task.Run(() => LoginRequest());

            LoginRequestResult result= await returnedTask;

            this.textBoxUsername.Text = result.username;
            this.textBoxPassword.Text = result.password;
        }


        private void buttonLoginCancel_Click(object sender, EventArgs e)
        {
            if (m_client.showScreenLoginCancel() )
            {
                // ...
            }            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox about= new AboutBox();
            about.ShowDialog();
        }
   
    }

}
