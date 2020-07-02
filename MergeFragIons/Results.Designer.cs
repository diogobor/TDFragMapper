namespace TDFragMapper
{
    partial class Results
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Results));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.elementHost2 = new System.Windows.Forms.Integration.ElementHost();
            this.proteinFragIons1 = new ProteinMergeFragIons.ProteinFragIons();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.userControlFilterCondition1 = new TDFragMapper.UserControlFilterCondition();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBoxIntensityNorm = new System.Windows.Forms.GroupBox();
            this.buttonIntensity = new System.Windows.Forms.Button();
            this.checkBoxIntensityGlobal = new System.Windows.Forms.CheckBox();
            this.checkBoxIntensityPerMap = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonRemoveMergeCondition = new System.Windows.Forms.Button();
            this.buttonAddMergeCondition = new System.Windows.Forms.Button();
            this.listBoxSelectedMergeConditions = new System.Windows.Forms.ListBox();
            this.listBoxAllMergeConditions = new System.Windows.Forms.ListBox();
            this.buttonMerge = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBoxMain.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBoxIntensityNorm.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1100, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageToolStripMenuItem,
            this.resultsToolStripMenuItem});
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("imageToolStripMenuItem.Image")));
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.imageToolStripMenuItem.Text = "Image";
            this.imageToolStripMenuItem.Click += new System.EventHandler(this.imageToolStripMenuItem_Click);
            // 
            // resultsToolStripMenuItem
            // 
            this.resultsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("resultsToolStripMenuItem.Image")));
            this.resultsToolStripMenuItem.Name = "resultsToolStripMenuItem";
            this.resultsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.resultsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.resultsToolStripMenuItem.Text = "Results";
            this.resultsToolStripMenuItem.Click += new System.EventHandler(this.resultsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(132, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exitToolStripMenuItem.Image")));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readMeToolStripMenuItem,
            this.toolStripSeparator2,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // readMeToolStripMenuItem
            // 
            this.readMeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("readMeToolStripMenuItem.Image")));
            this.readMeToolStripMenuItem.Name = "readMeToolStripMenuItem";
            this.readMeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.readMeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.readMeToolStripMenuItem.Text = "Read Me";
            this.readMeToolStripMenuItem.Click += new System.EventHandler(this.readMeToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutToolStripMenuItem.Image")));
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1076, 606);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.elementHost2);
            this.tabPage1.Controls.Add(this.elementHost1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1068, 580);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Display";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // elementHost2
            // 
            this.elementHost2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost2.Location = new System.Drawing.Point(3, 3);
            this.elementHost2.Name = "elementHost2";
            this.elementHost2.Size = new System.Drawing.Size(1062, 574);
            this.elementHost2.TabIndex = 1;
            this.elementHost2.Text = "elementHost2";
            this.elementHost2.Child = this.proteinFragIons1;
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(3, 3);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(1062, 574);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = null;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBoxMain);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1068, 580);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Filter";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMain.Controls.Add(this.userControlFilterCondition1);
            this.groupBoxMain.Controls.Add(this.buttonFilter);
            this.groupBoxMain.Location = new System.Drawing.Point(3, 6);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Size = new System.Drawing.Size(1059, 581);
            this.groupBoxMain.TabIndex = 0;
            this.groupBoxMain.TabStop = false;
            // 
            // userControlFilterCondition1
            // 
            this.userControlFilterCondition1.AutoScroll = true;
            this.userControlFilterCondition1.AutoSize = true;
            this.userControlFilterCondition1.Location = new System.Drawing.Point(6, 68);
            this.userControlFilterCondition1.MinimumSize = new System.Drawing.Size(1170, 244);
            this.userControlFilterCondition1.Name = "userControlFilterCondition1";
            this.userControlFilterCondition1.Size = new System.Drawing.Size(1170, 244);
            this.userControlFilterCondition1.TabIndex = 7;
            // 
            // buttonFilter
            // 
            this.buttonFilter.Image = global::TDFragMapper.Properties.Resources.iconFilter;
            this.buttonFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonFilter.Location = new System.Drawing.Point(11, 19);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(106, 29);
            this.buttonFilter.TabIndex = 6;
            this.buttonFilter.Text = "Filter";
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBoxIntensityNorm);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1068, 580);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Option Intensity";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBoxIntensityNorm
            // 
            this.groupBoxIntensityNorm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxIntensityNorm.Controls.Add(this.buttonIntensity);
            this.groupBoxIntensityNorm.Controls.Add(this.checkBoxIntensityGlobal);
            this.groupBoxIntensityNorm.Controls.Add(this.checkBoxIntensityPerMap);
            this.groupBoxIntensityNorm.Enabled = false;
            this.groupBoxIntensityNorm.Location = new System.Drawing.Point(6, 6);
            this.groupBoxIntensityNorm.Name = "groupBoxIntensityNorm";
            this.groupBoxIntensityNorm.Size = new System.Drawing.Size(1056, 581);
            this.groupBoxIntensityNorm.TabIndex = 3;
            this.groupBoxIntensityNorm.TabStop = false;
            this.groupBoxIntensityNorm.Text = "Intensity normalization";
            // 
            // buttonIntensity
            // 
            this.buttonIntensity.Image = ((System.Drawing.Image)(resources.GetObject("buttonIntensity.Image")));
            this.buttonIntensity.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonIntensity.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonIntensity.Location = new System.Drawing.Point(19, 97);
            this.buttonIntensity.Name = "buttonIntensity";
            this.buttonIntensity.Size = new System.Drawing.Size(135, 23);
            this.buttonIntensity.TabIndex = 3;
            this.buttonIntensity.Text = "Display";
            this.buttonIntensity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonIntensity.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonIntensity.UseVisualStyleBackColor = true;
            this.buttonIntensity.Click += new System.EventHandler(this.buttonIntensity_Click);
            // 
            // checkBoxIntensityGlobal
            // 
            this.checkBoxIntensityGlobal.AutoSize = true;
            this.checkBoxIntensityGlobal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxIntensityGlobal.Location = new System.Drawing.Point(19, 62);
            this.checkBoxIntensityGlobal.Name = "checkBoxIntensityGlobal";
            this.checkBoxIntensityGlobal.Size = new System.Drawing.Size(127, 17);
            this.checkBoxIntensityGlobal.TabIndex = 1;
            this.checkBoxIntensityGlobal.Text = "Across all study maps";
            this.checkBoxIntensityGlobal.UseVisualStyleBackColor = true;
            this.checkBoxIntensityGlobal.CheckedChanged += new System.EventHandler(this.checkBoxIntensityGlobal_CheckedChanged);
            // 
            // checkBoxIntensityPerMap
            // 
            this.checkBoxIntensityPerMap.AutoSize = true;
            this.checkBoxIntensityPerMap.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxIntensityPerMap.Location = new System.Drawing.Point(19, 30);
            this.checkBoxIntensityPerMap.Name = "checkBoxIntensityPerMap";
            this.checkBoxIntensityPerMap.Size = new System.Drawing.Size(93, 17);
            this.checkBoxIntensityPerMap.TabIndex = 0;
            this.checkBoxIntensityPerMap.Text = "Per study map";
            this.checkBoxIntensityPerMap.UseVisualStyleBackColor = true;
            this.checkBoxIntensityPerMap.CheckedChanged += new System.EventHandler(this.checkBoxIntensityPerMap_CheckedChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1068, 580);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Option Merging";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.buttonRemoveMergeCondition);
            this.groupBox1.Controls.Add(this.buttonAddMergeCondition);
            this.groupBox1.Controls.Add(this.listBoxSelectedMergeConditions);
            this.groupBox1.Controls.Add(this.listBoxAllMergeConditions);
            this.groupBox1.Controls.Add(this.buttonMerge);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1056, 581);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Available conditions:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(282, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Conditions to merge:";
            // 
            // buttonRemoveMergeCondition
            // 
            this.buttonRemoveMergeCondition.Image = global::TDFragMapper.Properties.Resources.arrow_left;
            this.buttonRemoveMergeCondition.Location = new System.Drawing.Point(247, 106);
            this.buttonRemoveMergeCondition.Name = "buttonRemoveMergeCondition";
            this.buttonRemoveMergeCondition.Size = new System.Drawing.Size(21, 23);
            this.buttonRemoveMergeCondition.TabIndex = 28;
            this.buttonRemoveMergeCondition.Tag = "1";
            this.buttonRemoveMergeCondition.UseVisualStyleBackColor = true;
            this.buttonRemoveMergeCondition.Click += new System.EventHandler(this.buttonRemoveMergeCondition_Click);
            // 
            // buttonAddMergeCondition
            // 
            this.buttonAddMergeCondition.Image = global::TDFragMapper.Properties.Resources.arrow_right;
            this.buttonAddMergeCondition.Location = new System.Drawing.Point(247, 68);
            this.buttonAddMergeCondition.Name = "buttonAddMergeCondition";
            this.buttonAddMergeCondition.Size = new System.Drawing.Size(21, 23);
            this.buttonAddMergeCondition.TabIndex = 27;
            this.buttonAddMergeCondition.Tag = "1";
            this.buttonAddMergeCondition.UseVisualStyleBackColor = true;
            this.buttonAddMergeCondition.Click += new System.EventHandler(this.buttonAddMergeCondition_Click);
            // 
            // listBoxSelectedMergeConditions
            // 
            this.listBoxSelectedMergeConditions.FormattingEnabled = true;
            this.listBoxSelectedMergeConditions.HorizontalScrollbar = true;
            this.listBoxSelectedMergeConditions.Location = new System.Drawing.Point(284, 44);
            this.listBoxSelectedMergeConditions.Name = "listBoxSelectedMergeConditions";
            this.listBoxSelectedMergeConditions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxSelectedMergeConditions.Size = new System.Drawing.Size(208, 108);
            this.listBoxSelectedMergeConditions.TabIndex = 29;
            // 
            // listBoxAllMergeConditions
            // 
            this.listBoxAllMergeConditions.FormattingEnabled = true;
            this.listBoxAllMergeConditions.HorizontalScrollbar = true;
            this.listBoxAllMergeConditions.Location = new System.Drawing.Point(21, 44);
            this.listBoxAllMergeConditions.Name = "listBoxAllMergeConditions";
            this.listBoxAllMergeConditions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAllMergeConditions.Size = new System.Drawing.Size(208, 108);
            this.listBoxAllMergeConditions.TabIndex = 26;
            // 
            // buttonMerge
            // 
            this.buttonMerge.Image = ((System.Drawing.Image)(resources.GetObject("buttonMerge.Image")));
            this.buttonMerge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonMerge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonMerge.Location = new System.Drawing.Point(21, 175);
            this.buttonMerge.Name = "buttonMerge";
            this.buttonMerge.Size = new System.Drawing.Size(135, 23);
            this.buttonMerge.TabIndex = 3;
            this.buttonMerge.Text = "Display";
            this.buttonMerge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonMerge.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonMerge.UseVisualStyleBackColor = true;
            this.buttonMerge.Click += new System.EventHandler(this.buttonMerge_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 636);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1100, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(158, 17);
            this.toolStripStatusLabel1.Text = "@2020 - All rights reserved®";
            // 
            // Results
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 658);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Results";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TDFragMapper :: Results";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBoxMain.ResumeLayout(false);
            this.groupBoxMain.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBoxIntensityNorm.ResumeLayout(false);
            this.groupBoxIntensityNorm.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readMeToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.Integration.ElementHost elementHost2;
        private ProteinMergeFragIons.ProteinFragIons proteinFragIons1;
        private System.Windows.Forms.Button buttonFilter;
        private UserControlFilterCondition userControlFilterCondition1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBoxIntensityNorm;
        private System.Windows.Forms.CheckBox checkBoxIntensityGlobal;
        private System.Windows.Forms.CheckBox checkBoxIntensityPerMap;
        private System.Windows.Forms.Button buttonIntensity;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonMerge;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonRemoveMergeCondition;
        private System.Windows.Forms.Button buttonAddMergeCondition;
        private System.Windows.Forms.ListBox listBoxSelectedMergeConditions;
        private System.Windows.Forms.ListBox listBoxAllMergeConditions;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resultsToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}