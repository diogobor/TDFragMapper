namespace ProteinAnnotation
{
    partial class PTMPopUp
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
            this.groupBoxPTM = new System.Windows.Forms.GroupBox();
            this.labelPTMs = new System.Windows.Forms.Label();
            this.timerPTM = new System.Windows.Forms.Timer(this.components);
            this.groupBoxPTM.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxPTM
            // 
            this.groupBoxPTM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPTM.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBoxPTM.Controls.Add(this.labelPTMs);
            this.groupBoxPTM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxPTM.Location = new System.Drawing.Point(3, 2);
            this.groupBoxPTM.Name = "groupBoxPTM";
            this.groupBoxPTM.Size = new System.Drawing.Size(181, 41);
            this.groupBoxPTM.TabIndex = 0;
            this.groupBoxPTM.TabStop = false;
            this.groupBoxPTM.Text = "PTM(s):";
            // 
            // labelPTMs
            // 
            this.labelPTMs.AutoSize = true;
            this.labelPTMs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPTMs.Location = new System.Drawing.Point(6, 17);
            this.labelPTMs.Name = "labelPTMs";
            this.labelPTMs.Size = new System.Drawing.Size(35, 13);
            this.labelPTMs.TabIndex = 1;
            this.labelPTMs.Text = "label2";
            // 
            // timerPTM
            // 
            this.timerPTM.Interval = 5000;
            this.timerPTM.Tick += new System.EventHandler(this.timerPTM_Tick);
            // 
            // PTMPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(187, 45);
            this.Controls.Add(this.groupBoxPTM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PTMPopUp";
            this.Text = "PTMPopUp";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PTMPopUp_FormClosed);
            this.groupBoxPTM.ResumeLayout(false);
            this.groupBoxPTM.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPTM;
        private System.Windows.Forms.Label labelPTMs;
        private System.Windows.Forms.Timer timerPTM;
    }
}