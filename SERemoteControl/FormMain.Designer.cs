namespace SERemoteControl
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabCockpit = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.comboBoxSevenExcellence = new System.Windows.Forms.ComboBox();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.textBoxState = new System.Windows.Forms.TextBox();
            this.buttonGetState = new System.Windows.Forms.Button();
            this.buttonDetach = new System.Windows.Forms.Button();
            this.buttonAttach = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.comboBoxSensor = new System.Windows.Forms.ComboBox();
            this.comboBoxModule = new System.Windows.Forms.ComboBox();
            this.buttonConnectSensorToModule = new System.Windows.Forms.Button();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnSensor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonSensors = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderModule = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSensor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonModules = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.buttonTermiate = new System.Windows.Forms.Button();
            this.textBoxMethodSampleId = new System.Windows.Forms.TextBox();
            this.textBoxMethodInstruction = new System.Windows.Forms.TextBox();
            this.buttonStartMethod = new System.Windows.Forms.Button();
            this.listboxListOfMethods = new System.Windows.Forms.ListBox();
            this.buttonMethods = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textBoxSettingValue = new System.Windows.Forms.TextBox();
            this.comboBoxSettingTag = new System.Windows.Forms.ComboBox();
            this.buttonSetupSetting = new System.Windows.Forms.Button();
            this.listViewSettings = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonRequestSetupItemList = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.buttonLoginCancel = new System.Windows.Forms.Button();
            this.checkBoxProposedUsers = new System.Windows.Forms.CheckBox();
            this.textBoxUsernameList = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.listviewLog = new System.Windows.Forms.ListView();
            this.columnHeaderTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLog = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabCockpit.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 24);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.listviewLog);
            this.splitContainer.Size = new System.Drawing.Size(611, 471);
            this.splitContainer.SplitterDistance = 305;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.tabCockpit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(611, 305);
            this.panel1.TabIndex = 0;
            // 
            // tabCockpit
            // 
            this.tabCockpit.Controls.Add(this.tabPage1);
            this.tabCockpit.Controls.Add(this.tabPage2);
            this.tabCockpit.Controls.Add(this.tabPage3);
            this.tabCockpit.Controls.Add(this.tabPage4);
            this.tabCockpit.Controls.Add(this.tabPage5);
            this.tabCockpit.Controls.Add(this.tabPage6);
            this.tabCockpit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCockpit.Location = new System.Drawing.Point(0, 0);
            this.tabCockpit.Name = "tabCockpit";
            this.tabCockpit.SelectedIndex = 0;
            this.tabCockpit.Size = new System.Drawing.Size(611, 305);
            this.tabCockpit.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.comboBoxSevenExcellence);
            this.tabPage1.Controls.Add(this.buttonClearLog);
            this.tabPage1.Controls.Add(this.textBoxState);
            this.tabPage1.Controls.Add(this.buttonGetState);
            this.tabPage1.Controls.Add(this.buttonDetach);
            this.tabPage1.Controls.Add(this.buttonAttach);
            this.tabPage1.Controls.Add(this.buttonDisconnect);
            this.tabPage1.Controls.Add(this.buttonConnect);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(603, 279);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Connection";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // comboBoxSevenExcellence
            // 
            this.comboBoxSevenExcellence.FormattingEnabled = true;
            this.comboBoxSevenExcellence.Location = new System.Drawing.Point(128, 29);
            this.comboBoxSevenExcellence.Name = "comboBoxSevenExcellence";
            this.comboBoxSevenExcellence.Size = new System.Drawing.Size(213, 21);
            this.comboBoxSevenExcellence.TabIndex = 7;
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearLog.Location = new System.Drawing.Point(522, 247);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(75, 23);
            this.buttonClearLog.TabIndex = 6;
            this.buttonClearLog.Text = "Clear Log";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.buttonClearLog_Click);
            // 
            // textBoxState
            // 
            this.textBoxState.Location = new System.Drawing.Point(128, 117);
            this.textBoxState.Name = "textBoxState";
            this.textBoxState.Size = new System.Drawing.Size(181, 20);
            this.textBoxState.TabIndex = 0;
            // 
            // buttonGetState
            // 
            this.buttonGetState.Location = new System.Drawing.Point(28, 114);
            this.buttonGetState.Name = "buttonGetState";
            this.buttonGetState.Size = new System.Drawing.Size(75, 23);
            this.buttonGetState.TabIndex = 5;
            this.buttonGetState.Text = "State";
            this.buttonGetState.UseVisualStyleBackColor = true;
            this.buttonGetState.Click += new System.EventHandler(this.buttonGetState_Click);
            // 
            // buttonDetach
            // 
            this.buttonDetach.Location = new System.Drawing.Point(28, 161);
            this.buttonDetach.Name = "buttonDetach";
            this.buttonDetach.Size = new System.Drawing.Size(75, 23);
            this.buttonDetach.TabIndex = 3;
            this.buttonDetach.Text = "Detach";
            this.buttonDetach.UseVisualStyleBackColor = true;
            this.buttonDetach.Click += new System.EventHandler(this.buttonDetach_Click);
            // 
            // buttonAttach
            // 
            this.buttonAttach.Location = new System.Drawing.Point(28, 70);
            this.buttonAttach.Name = "buttonAttach";
            this.buttonAttach.Size = new System.Drawing.Size(75, 23);
            this.buttonAttach.TabIndex = 2;
            this.buttonAttach.Text = "Attach";
            this.buttonAttach.UseVisualStyleBackColor = true;
            this.buttonAttach.Click += new System.EventHandler(this.buttonAttach_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(28, 203);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonDisconnect.TabIndex = 1;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(28, 28);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.comboBoxSensor);
            this.tabPage2.Controls.Add(this.comboBoxModule);
            this.tabPage2.Controls.Add(this.buttonConnectSensorToModule);
            this.tabPage2.Controls.Add(this.listView2);
            this.tabPage2.Controls.Add(this.buttonSensors);
            this.tabPage2.Controls.Add(this.listView1);
            this.tabPage2.Controls.Add(this.buttonModules);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(603, 279);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Modules";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // comboBoxSensor
            // 
            this.comboBoxSensor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSensor.FormattingEnabled = true;
            this.comboBoxSensor.Location = new System.Drawing.Point(246, 218);
            this.comboBoxSensor.Name = "comboBoxSensor";
            this.comboBoxSensor.Size = new System.Drawing.Size(307, 21);
            this.comboBoxSensor.TabIndex = 7;
            // 
            // comboBoxModule
            // 
            this.comboBoxModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxModule.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxModule.FormattingEnabled = true;
            this.comboBoxModule.Location = new System.Drawing.Point(91, 218);
            this.comboBoxModule.Name = "comboBoxModule";
            this.comboBoxModule.Size = new System.Drawing.Size(149, 21);
            this.comboBoxModule.TabIndex = 6;
            // 
            // buttonConnectSensorToModule
            // 
            this.buttonConnectSensorToModule.Location = new System.Drawing.Point(10, 218);
            this.buttonConnectSensorToModule.Name = "buttonConnectSensorToModule";
            this.buttonConnectSensorToModule.Size = new System.Drawing.Size(75, 23);
            this.buttonConnectSensorToModule.TabIndex = 5;
            this.buttonConnectSensorToModule.Text = "Connect";
            this.buttonConnectSensorToModule.UseVisualStyleBackColor = true;
            this.buttonConnectSensorToModule.Click += new System.EventHandler(this.buttonConnectSensorToModule_Click);
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnSensor,
            this.columnHeader2});
            this.listView2.FullRowSelect = true;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(91, 100);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(462, 104);
            this.listView2.TabIndex = 4;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            this.listView2.SelectedIndexChanged += new System.EventHandler(this.listView2_SelectedIndexChanged);
            // 
            // columnSensor
            // 
            this.columnSensor.Text = "Sensor";
            this.columnSensor.Width = 190;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Sensor";
            this.columnHeader2.Width = 200;
            // 
            // buttonSensors
            // 
            this.buttonSensors.Location = new System.Drawing.Point(10, 100);
            this.buttonSensors.Name = "buttonSensors";
            this.buttonSensors.Size = new System.Drawing.Size(75, 22);
            this.buttonSensors.TabIndex = 3;
            this.buttonSensors.Text = "Sensor";
            this.buttonSensors.UseVisualStyleBackColor = true;
            this.buttonSensors.Click += new System.EventHandler(this.buttonConnectSensor_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderModule,
            this.columnHeaderSensor});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(91, 19);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(462, 75);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeaderModule
            // 
            this.columnHeaderModule.Text = "Module";
            this.columnHeaderModule.Width = 190;
            // 
            // columnHeaderSensor
            // 
            this.columnHeaderSensor.Text = "Sensor";
            this.columnHeaderSensor.Width = 200;
            // 
            // buttonModules
            // 
            this.buttonModules.Location = new System.Drawing.Point(10, 19);
            this.buttonModules.Name = "buttonModules";
            this.buttonModules.Size = new System.Drawing.Size(75, 23);
            this.buttonModules.TabIndex = 0;
            this.buttonModules.Text = "Modules";
            this.buttonModules.UseVisualStyleBackColor = true;
            this.buttonModules.Click += new System.EventHandler(this.buttonModules_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.buttonTermiate);
            this.tabPage3.Controls.Add(this.textBoxMethodSampleId);
            this.tabPage3.Controls.Add(this.textBoxMethodInstruction);
            this.tabPage3.Controls.Add(this.buttonStartMethod);
            this.tabPage3.Controls.Add(this.listboxListOfMethods);
            this.tabPage3.Controls.Add(this.buttonMethods);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(603, 279);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Methods";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonTermiate
            // 
            this.buttonTermiate.Location = new System.Drawing.Point(11, 197);
            this.buttonTermiate.Name = "buttonTermiate";
            this.buttonTermiate.Size = new System.Drawing.Size(75, 23);
            this.buttonTermiate.TabIndex = 5;
            this.buttonTermiate.Text = "Terminate";
            this.buttonTermiate.UseVisualStyleBackColor = true;
            this.buttonTermiate.Click += new System.EventHandler(this.buttonTermiate_Click);
            // 
            // textBoxMethodSampleId
            // 
            this.textBoxMethodSampleId.Location = new System.Drawing.Point(181, 131);
            this.textBoxMethodSampleId.Name = "textBoxMethodSampleId";
            this.textBoxMethodSampleId.Size = new System.Drawing.Size(405, 20);
            this.textBoxMethodSampleId.TabIndex = 4;
            // 
            // textBoxMethodInstruction
            // 
            this.textBoxMethodInstruction.Location = new System.Drawing.Point(181, 157);
            this.textBoxMethodInstruction.Name = "textBoxMethodInstruction";
            this.textBoxMethodInstruction.Size = new System.Drawing.Size(405, 20);
            this.textBoxMethodInstruction.TabIndex = 3;
            // 
            // buttonStartMethod
            // 
            this.buttonStartMethod.Location = new System.Drawing.Point(10, 129);
            this.buttonStartMethod.Name = "buttonStartMethod";
            this.buttonStartMethod.Size = new System.Drawing.Size(75, 23);
            this.buttonStartMethod.TabIndex = 2;
            this.buttonStartMethod.Text = "Start Method";
            this.buttonStartMethod.UseVisualStyleBackColor = true;
            this.buttonStartMethod.Click += new System.EventHandler(this.buttonStartMethod_Click);
            // 
            // listboxListOfMethods
            // 
            this.listboxListOfMethods.FormattingEnabled = true;
            this.listboxListOfMethods.Location = new System.Drawing.Point(91, 17);
            this.listboxListOfMethods.Name = "listboxListOfMethods";
            this.listboxListOfMethods.Size = new System.Drawing.Size(495, 95);
            this.listboxListOfMethods.TabIndex = 1;
            // 
            // buttonMethods
            // 
            this.buttonMethods.Location = new System.Drawing.Point(10, 18);
            this.buttonMethods.Name = "buttonMethods";
            this.buttonMethods.Size = new System.Drawing.Size(75, 23);
            this.buttonMethods.TabIndex = 0;
            this.buttonMethods.Text = "Methods";
            this.buttonMethods.UseVisualStyleBackColor = true;
            this.buttonMethods.Click += new System.EventHandler(this.buttonMethods_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.textBoxSettingValue);
            this.tabPage4.Controls.Add(this.comboBoxSettingTag);
            this.tabPage4.Controls.Add(this.buttonSetupSetting);
            this.tabPage4.Controls.Add(this.listViewSettings);
            this.tabPage4.Controls.Add(this.buttonRequestSetupItemList);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(603, 279);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Settings";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBoxSettingValue
            // 
            this.textBoxSettingValue.Location = new System.Drawing.Point(340, 222);
            this.textBoxSettingValue.Name = "textBoxSettingValue";
            this.textBoxSettingValue.Size = new System.Drawing.Size(226, 20);
            this.textBoxSettingValue.TabIndex = 4;
            // 
            // comboBoxSettingTag
            // 
            this.comboBoxSettingTag.FormattingEnabled = true;
            this.comboBoxSettingTag.Location = new System.Drawing.Point(118, 222);
            this.comboBoxSettingTag.Name = "comboBoxSettingTag";
            this.comboBoxSettingTag.Size = new System.Drawing.Size(216, 21);
            this.comboBoxSettingTag.TabIndex = 3;
            // 
            // buttonSetupSetting
            // 
            this.buttonSetupSetting.Location = new System.Drawing.Point(20, 222);
            this.buttonSetupSetting.Name = "buttonSetupSetting";
            this.buttonSetupSetting.Size = new System.Drawing.Size(75, 23);
            this.buttonSetupSetting.TabIndex = 2;
            this.buttonSetupSetting.Text = "Setup";
            this.buttonSetupSetting.UseVisualStyleBackColor = true;
            this.buttonSetupSetting.Click += new System.EventHandler(this.buttonSetupSetting_Click);
            // 
            // listViewSettings
            // 
            this.listViewSettings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.listViewSettings.Location = new System.Drawing.Point(118, 23);
            this.listViewSettings.Name = "listViewSettings";
            this.listViewSettings.Size = new System.Drawing.Size(448, 176);
            this.listViewSettings.TabIndex = 1;
            this.listViewSettings.UseCompatibleStateImageBehavior = false;
            this.listViewSettings.View = System.Windows.Forms.View.Details;
            this.listViewSettings.SelectedIndexChanged += new System.EventHandler(this.listViewSettings_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Setting";
            this.columnHeader3.Width = 304;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Value";
            this.columnHeader4.Width = 125;
            // 
            // buttonRequestSetupItemList
            // 
            this.buttonRequestSetupItemList.Location = new System.Drawing.Point(20, 23);
            this.buttonRequestSetupItemList.Name = "buttonRequestSetupItemList";
            this.buttonRequestSetupItemList.Size = new System.Drawing.Size(75, 23);
            this.buttonRequestSetupItemList.TabIndex = 0;
            this.buttonRequestSetupItemList.Text = "List";
            this.buttonRequestSetupItemList.UseVisualStyleBackColor = true;
            this.buttonRequestSetupItemList.Click += new System.EventHandler(this.buttonRequestSetupItemListt_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.buttonImport);
            this.tabPage5.Controls.Add(this.buttonExport);
            this.tabPage5.Controls.Add(this.comboBox2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(603, 279);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Import/Export";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // buttonImport
            // 
            this.buttonImport.Location = new System.Drawing.Point(206, 91);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(75, 23);
            this.buttonImport.TabIndex = 3;
            this.buttonImport.Text = "Import...";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(206, 45);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(75, 23);
            this.buttonExport.TabIndex = 2;
            this.buttonExport.Text = "Export...";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(61, 47);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 1;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.buttonLoginCancel);
            this.tabPage6.Controls.Add(this.checkBoxProposedUsers);
            this.tabPage6.Controls.Add(this.textBoxUsernameList);
            this.tabPage6.Controls.Add(this.label2);
            this.tabPage6.Controls.Add(this.label1);
            this.tabPage6.Controls.Add(this.textBoxPassword);
            this.tabPage6.Controls.Add(this.textBoxUsername);
            this.tabPage6.Controls.Add(this.buttonLogin);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(603, 279);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Login";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // buttonLoginCancel
            // 
            this.buttonLoginCancel.Location = new System.Drawing.Point(34, 209);
            this.buttonLoginCancel.Name = "buttonLoginCancel";
            this.buttonLoginCancel.Size = new System.Drawing.Size(75, 24);
            this.buttonLoginCancel.TabIndex = 7;
            this.buttonLoginCancel.Text = "Cancel";
            this.buttonLoginCancel.UseVisualStyleBackColor = true;
            this.buttonLoginCancel.Click += new System.EventHandler(this.buttonLoginCancel_Click);
            // 
            // checkBoxProposedUsers
            // 
            this.checkBoxProposedUsers.AutoSize = true;
            this.checkBoxProposedUsers.Location = new System.Drawing.Point(34, 22);
            this.checkBoxProposedUsers.Name = "checkBoxProposedUsers";
            this.checkBoxProposedUsers.Size = new System.Drawing.Size(96, 17);
            this.checkBoxProposedUsers.TabIndex = 6;
            this.checkBoxProposedUsers.Text = "Proposed User";
            this.checkBoxProposedUsers.UseVisualStyleBackColor = true;
            // 
            // textBoxUsernameList
            // 
            this.textBoxUsernameList.Location = new System.Drawing.Point(34, 45);
            this.textBoxUsernameList.Name = "textBoxUsernameList";
            this.textBoxUsernameList.Size = new System.Drawing.Size(547, 20);
            this.textBoxUsernameList.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Username";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(123, 162);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(154, 20);
            this.textBoxPassword.TabIndex = 2;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(123, 125);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(154, 20);
            this.textBoxUsername.TabIndex = 1;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(34, 76);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(75, 23);
            this.buttonLogin.TabIndex = 0;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // listviewLog
            // 
            this.listviewLog.AllowColumnReorder = true;
            this.listviewLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.listviewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTime,
            this.columnHeaderLog});
            this.listviewLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listviewLog.FullRowSelect = true;
            this.listviewLog.GridLines = true;
            this.listviewLog.Location = new System.Drawing.Point(0, 0);
            this.listviewLog.Name = "listviewLog";
            this.listviewLog.Size = new System.Drawing.Size(611, 161);
            this.listviewLog.TabIndex = 0;
            this.listviewLog.UseCompatibleStateImageBehavior = false;
            this.listviewLog.View = System.Windows.Forms.View.Details;
            this.listviewLog.DoubleClick += new System.EventHandler(this.listviewLog_DoubleClick);
            this.listviewLog.Resize += new System.EventHandler(this.listviewLog_Resize);
            // 
            // columnHeaderTime
            // 
            this.columnHeaderTime.Text = "Time";
            // 
            // columnHeaderLog
            // 
            this.columnHeaderLog.Text = "Log";
            this.columnHeaderLog.Width = 508;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(611, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(52, 20);
            this.toolStripMenuItem1.Text = "About";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // FormMain
            // 
            this.ClientSize = new System.Drawing.Size(611, 495);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "SE Cockpit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClose);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResizeEnd += new System.EventHandler(this.FormMain_ResizeEnd);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabCockpit.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabCockpit;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListView listviewLog;
        private System.Windows.Forms.Button buttonGetState;
        private System.Windows.Forms.Button buttonDetach;
        private System.Windows.Forms.Button buttonAttach;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonModules;
        private System.Windows.Forms.Button buttonStartMethod;
        private System.Windows.Forms.ListBox listboxListOfMethods;
        private System.Windows.Forms.Button buttonMethods;
        private System.Windows.Forms.TextBox textBoxState;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.ColumnHeader columnHeaderLog;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeaderModule;
        private System.Windows.Forms.ColumnHeader columnHeaderSensor;
        private System.Windows.Forms.TextBox textBoxMethodInstruction;
        private System.Windows.Forms.TextBox textBoxMethodSampleId;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnSensor;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button buttonSensors;
        private System.Windows.Forms.Button buttonConnectSensorToModule;
        private System.Windows.Forms.Button buttonTermiate;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.ComboBox comboBoxSensor;
        private System.Windows.Forms.ComboBox comboBoxModule;
        private System.Windows.Forms.Button buttonRequestSetupItemList;
        private System.Windows.Forms.ListView listViewSettings;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Button buttonClearLog;
        private System.Windows.Forms.ComboBox comboBoxSevenExcellence;
        private System.Windows.Forms.Button buttonLoginCancel;
        private System.Windows.Forms.CheckBox checkBoxProposedUsers;
        private System.Windows.Forms.TextBox textBoxUsernameList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Button buttonSetupSetting;
        private System.Windows.Forms.TextBox textBoxSettingValue;
        private System.Windows.Forms.ComboBox comboBoxSettingTag;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;





    }
}

