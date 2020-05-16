namespace MergeFragIons
{
    partial class GUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerStatus = new System.Windows.Forms.Timer(this.components);
            this.tabControlMainWindow = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.listBoxConsole = new System.Windows.Forms.ListBox();
            this.groupBoxInputFiles = new System.Windows.Forms.GroupBox();
            this.buttonAddInputFile = new System.Windows.Forms.Button();
            this.dataGridViewInputFiles = new System.Windows.Forms.DataGridView();
            this.FragMethodCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActivationLevelCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrecursorChargeStateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReplicateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MSMSData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonBrowseProteinSequence = new System.Windows.Forms.Button();
            this.textBoxProteinSeq = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonDisplay = new System.Windows.Forms.Button();
            this.userControlFilterCondition1 = new MergeFragIons.UserControlFilterCondition();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControlMainWindow.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxLog.SuspendLayout();
            this.groupBoxInputFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInputFiles)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // openToolStripMenuItem
            // 
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readMeToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // readMeToolStripMenuItem
            // 
            resources.ApplyResources(this.readMeToolStripMenuItem, "readMeToolStripMenuItem");
            this.readMeToolStripMenuItem.Name = "readMeToolStripMenuItem";
            this.readMeToolStripMenuItem.Click += new System.EventHandler(this.readMeToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            // 
            // timerStatus
            // 
            this.timerStatus.Tick += new System.EventHandler(this.timerStatus_Tick);
            // 
            // tabControlMainWindow
            // 
            resources.ApplyResources(this.tabControlMainWindow, "tabControlMainWindow");
            this.tabControlMainWindow.Controls.Add(this.tabPage1);
            this.tabControlMainWindow.Controls.Add(this.tabPage2);
            this.tabControlMainWindow.Name = "tabControlMainWindow";
            this.tabControlMainWindow.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.groupBoxLog);
            this.tabPage1.Controls.Add(this.groupBoxInputFiles);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBoxLog
            // 
            resources.ApplyResources(this.groupBoxLog, "groupBoxLog");
            this.groupBoxLog.Controls.Add(this.listBoxConsole);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.TabStop = false;
            // 
            // listBoxConsole
            // 
            resources.ApplyResources(this.listBoxConsole, "listBoxConsole");
            this.listBoxConsole.FormattingEnabled = true;
            this.listBoxConsole.Name = "listBoxConsole";
            // 
            // groupBoxInputFiles
            // 
            resources.ApplyResources(this.groupBoxInputFiles, "groupBoxInputFiles");
            this.groupBoxInputFiles.Controls.Add(this.buttonAddInputFile);
            this.groupBoxInputFiles.Controls.Add(this.dataGridViewInputFiles);
            this.groupBoxInputFiles.Controls.Add(this.buttonOK);
            this.groupBoxInputFiles.Controls.Add(this.buttonBrowseProteinSequence);
            this.groupBoxInputFiles.Controls.Add(this.textBoxProteinSeq);
            this.groupBoxInputFiles.Controls.Add(this.label1);
            this.groupBoxInputFiles.Name = "groupBoxInputFiles";
            this.groupBoxInputFiles.TabStop = false;
            // 
            // buttonAddInputFile
            // 
            this.buttonAddInputFile.Image = global::MergeFragIons.Properties.Resources.addButton;
            resources.ApplyResources(this.buttonAddInputFile, "buttonAddInputFile");
            this.buttonAddInputFile.Name = "buttonAddInputFile";
            this.buttonAddInputFile.UseVisualStyleBackColor = true;
            this.buttonAddInputFile.Click += new System.EventHandler(this.buttonAddInputFile_Click);
            // 
            // dataGridViewInputFiles
            // 
            this.dataGridViewInputFiles.AllowUserToAddRows = false;
            this.dataGridViewInputFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInputFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FragMethodCol,
            this.ActivationLevelCol,
            this.PrecursorChargeStateCol,
            this.ReplicateCol,
            this.MSMSData});
            resources.ApplyResources(this.dataGridViewInputFiles, "dataGridViewInputFiles");
            this.dataGridViewInputFiles.Name = "dataGridViewInputFiles";
            this.dataGridViewInputFiles.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewInputFiles.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewInputFiles_CellClick);
            this.dataGridViewInputFiles.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewInputFiles_EditingControlShowing);
            this.dataGridViewInputFiles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridViewInputFiles_KeyPress);
            // 
            // FragMethodCol
            // 
            resources.ApplyResources(this.FragMethodCol, "FragMethodCol");
            this.FragMethodCol.MaxInputLength = 10;
            this.FragMethodCol.Name = "FragMethodCol";
            // 
            // ActivationLevelCol
            // 
            resources.ApplyResources(this.ActivationLevelCol, "ActivationLevelCol");
            this.ActivationLevelCol.MaxInputLength = 4;
            this.ActivationLevelCol.Name = "ActivationLevelCol";
            // 
            // PrecursorChargeStateCol
            // 
            resources.ApplyResources(this.PrecursorChargeStateCol, "PrecursorChargeStateCol");
            this.PrecursorChargeStateCol.MaxInputLength = 4;
            this.PrecursorChargeStateCol.Name = "PrecursorChargeStateCol";
            // 
            // ReplicateCol
            // 
            resources.ApplyResources(this.ReplicateCol, "ReplicateCol");
            this.ReplicateCol.MaxInputLength = 4;
            this.ReplicateCol.Name = "ReplicateCol";
            // 
            // MSMSData
            // 
            this.MSMSData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.MSMSData, "MSMSData");
            this.MSMSData.Name = "MSMSData";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonBrowseProteinSequence
            // 
            resources.ApplyResources(this.buttonBrowseProteinSequence, "buttonBrowseProteinSequence");
            this.buttonBrowseProteinSequence.Name = "buttonBrowseProteinSequence";
            this.toolTips.SetToolTip(this.buttonBrowseProteinSequence, resources.GetString("buttonBrowseProteinSequence.ToolTip"));
            this.buttonBrowseProteinSequence.UseVisualStyleBackColor = true;
            this.buttonBrowseProteinSequence.Click += new System.EventHandler(this.buttonBottomUpResults_Click);
            // 
            // textBoxProteinSeq
            // 
            resources.ApplyResources(this.textBoxProteinSeq, "textBoxProteinSeq");
            this.textBoxProteinSeq.Name = "textBoxProteinSeq";
            this.toolTips.SetToolTip(this.textBoxProteinSeq, resources.GetString("textBoxProteinSeq.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.toolTips.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Controls.Add(this.buttonDisplay);
            this.tabPage2.Controls.Add(this.userControlFilterCondition1);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonDisplay
            // 
            resources.ApplyResources(this.buttonDisplay, "buttonDisplay");
            this.buttonDisplay.Name = "buttonDisplay";
            this.buttonDisplay.UseVisualStyleBackColor = true;
            this.buttonDisplay.Click += new System.EventHandler(this.buttonDisplay_Click);
            // 
            // userControlFilterCondition1
            // 
            resources.ApplyResources(this.userControlFilterCondition1, "userControlFilterCondition1");
            this.userControlFilterCondition1.Name = "userControlFilterCondition1";
            // 
            // GUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMainWindow);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "GUI";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.GUI_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControlMainWindow.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBoxLog.ResumeLayout(false);
            this.groupBoxInputFiles.ResumeLayout(false);
            this.groupBoxInputFiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInputFiles)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Timer timerStatus;
        private System.Windows.Forms.TabControl tabControlMainWindow;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.ListBox listBoxConsole;
        private System.Windows.Forms.GroupBox groupBoxInputFiles;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonBrowseProteinSequence;
        private System.Windows.Forms.TextBox textBoxProteinSeq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem readMeToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.DataGridView dataGridViewInputFiles;
        private System.Windows.Forms.Button buttonAddInputFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn FragMethodCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActivationLevelCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrecursorChargeStateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReplicateCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MSMSData;
        private UserControlFilterCondition userControlFilterCondition1;
        private System.Windows.Forms.Button buttonDisplay;
    }
}

