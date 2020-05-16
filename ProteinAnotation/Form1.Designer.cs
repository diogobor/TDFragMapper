namespace ProteinAnnotation
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonPeptideAnotation = new System.Windows.Forms.Button();
            this.groupBoxProteinAnnotator = new System.Windows.Forms.GroupBox();
            this.groupBoxPeptides = new System.Windows.Forms.GroupBox();
            this.richTextBoxAnotationBeta = new System.Windows.Forms.RichTextBox();
            this.textBoxPeptideSequence2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownXLPos2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownXLPos1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxProteoforms = new System.Windows.Forms.GroupBox();
            this.richTextBoxAnotationAlfa = new System.Windows.Forms.RichTextBox();
            this.textBoxPeptideSequence1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBoxUniquePeptides = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBoxPeptides.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXLPos2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXLPos1)).BeginInit();
            this.groupBoxProteoforms.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPeptideAnotation
            // 
            this.buttonPeptideAnotation.Location = new System.Drawing.Point(445, 120);
            this.buttonPeptideAnotation.Name = "buttonPeptideAnotation";
            this.buttonPeptideAnotation.Size = new System.Drawing.Size(171, 23);
            this.buttonPeptideAnotation.TabIndex = 15;
            this.buttonPeptideAnotation.Text = "Go";
            this.buttonPeptideAnotation.UseVisualStyleBackColor = true;
            this.buttonPeptideAnotation.Click += new System.EventHandler(this.buttonPeptideAnotation_Click);
            // 
            // groupBoxProteinAnnotator
            // 
            this.groupBoxProteinAnnotator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProteinAnnotator.AutoSize = true;
            this.groupBoxProteinAnnotator.Location = new System.Drawing.Point(12, 185);
            this.groupBoxProteinAnnotator.Name = "groupBoxProteinAnnotator";
            this.groupBoxProteinAnnotator.Size = new System.Drawing.Size(1131, 147);
            this.groupBoxProteinAnnotator.TabIndex = 16;
            this.groupBoxProteinAnnotator.TabStop = false;
            this.groupBoxProteinAnnotator.Text = "Protein annotation";
            // 
            // groupBoxPeptides
            // 
            this.groupBoxPeptides.Controls.Add(this.richTextBoxAnotationBeta);
            this.groupBoxPeptides.Location = new System.Drawing.Point(154, 12);
            this.groupBoxPeptides.Name = "groupBoxPeptides";
            this.groupBoxPeptides.Size = new System.Drawing.Size(138, 167);
            this.groupBoxPeptides.TabIndex = 26;
            this.groupBoxPeptides.TabStop = false;
            this.groupBoxPeptides.Text = "Identified Peptides";
            // 
            // richTextBoxAnotationBeta
            // 
            this.richTextBoxAnotationBeta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxAnotationBeta.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxAnotationBeta.Name = "richTextBoxAnotationBeta";
            this.richTextBoxAnotationBeta.Size = new System.Drawing.Size(132, 148);
            this.richTextBoxAnotationBeta.TabIndex = 5;
            this.richTextBoxAnotationBeta.Text = "CID-25-B-8\nCID-25-B-10\nCID-25-B-11\nCID-25-B-12\nCID-25-B-13\nCID-25-B-18\nCID-25-B-2" +
    "0\nCID-25-Y-140\nCID-25-Y-141\nCID-25-Y-143";
            // 
            // textBoxPeptideSequence2
            // 
            this.textBoxPeptideSequence2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxPeptideSequence2.Location = new System.Drawing.Point(516, 42);
            this.textBoxPeptideSequence2.Name = "textBoxPeptideSequence2";
            this.textBoxPeptideSequence2.Size = new System.Drawing.Size(100, 20);
            this.textBoxPeptideSequence2.TabIndex = 25;
            this.textBoxPeptideSequence2.Text = "HELLOWORLD";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(442, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Beta Peptide";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(457, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "XL Pos 2:";
            // 
            // numericUpDownXLPos2
            // 
            this.numericUpDownXLPos2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownXLPos2.Location = new System.Drawing.Point(516, 94);
            this.numericUpDownXLPos2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownXLPos2.Name = "numericUpDownXLPos2";
            this.numericUpDownXLPos2.Size = new System.Drawing.Size(100, 20);
            this.numericUpDownXLPos2.TabIndex = 22;
            this.numericUpDownXLPos2.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // numericUpDownXLPos1
            // 
            this.numericUpDownXLPos1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownXLPos1.Location = new System.Drawing.Point(516, 68);
            this.numericUpDownXLPos1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownXLPos1.Name = "numericUpDownXLPos1";
            this.numericUpDownXLPos1.Size = new System.Drawing.Size(100, 20);
            this.numericUpDownXLPos1.TabIndex = 21;
            this.numericUpDownXLPos1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(457, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "XL Pos 1:";
            // 
            // groupBoxProteoforms
            // 
            this.groupBoxProteoforms.Controls.Add(this.richTextBoxAnotationAlfa);
            this.groupBoxProteoforms.Location = new System.Drawing.Point(12, 12);
            this.groupBoxProteoforms.Name = "groupBoxProteoforms";
            this.groupBoxProteoforms.Size = new System.Drawing.Size(136, 167);
            this.groupBoxProteoforms.TabIndex = 19;
            this.groupBoxProteoforms.TabStop = false;
            this.groupBoxProteoforms.Text = "Identified Proteoforms";
            // 
            // richTextBoxAnotationAlfa
            // 
            this.richTextBoxAnotationAlfa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxAnotationAlfa.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxAnotationAlfa.Name = "richTextBoxAnotationAlfa";
            this.richTextBoxAnotationAlfa.Size = new System.Drawing.Size(130, 148);
            this.richTextBoxAnotationAlfa.TabIndex = 5;
            this.richTextBoxAnotationAlfa.Text = resources.GetString("richTextBoxAnotationAlfa.Text");
            // 
            // textBoxPeptideSequence1
            // 
            this.textBoxPeptideSequence1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxPeptideSequence1.Location = new System.Drawing.Point(516, 16);
            this.textBoxPeptideSequence1.Name = "textBoxPeptideSequence1";
            this.textBoxPeptideSequence1.Size = new System.Drawing.Size(100, 20);
            this.textBoxPeptideSequence1.TabIndex = 18;
            this.textBoxPeptideSequence1.Text = "VKLQQDDIGARLKEVQEAAKSSSLFKKMVELVSFAPFKGAAQALENANDISEGLVSDYLKSVLELNLPSGSSKETIGLGIS" +
    "DKNLGPSIKEIFPHVECHSNEIVQDLLRGIRFGV";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(442, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Alfa Peptide";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxUniquePeptides);
            this.groupBox1.Location = new System.Drawing.Point(301, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 167);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Identified Peptides";
            // 
            // richTextBoxUniquePeptides
            // 
            this.richTextBoxUniquePeptides.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxUniquePeptides.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxUniquePeptides.Name = "richTextBoxUniquePeptides";
            this.richTextBoxUniquePeptides.Size = new System.Drawing.Size(132, 148);
            this.richTextBoxUniquePeptides.TabIndex = 5;
            this.richTextBoxUniquePeptides.Text = "EVQEAAK\nSSSLFKK\nMVELVSFAPFK\nRFGV";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(12, 338);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1131, 237);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Protein Annotator WPF";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 587);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxPeptides);
            this.Controls.Add(this.textBoxPeptideSequence2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownXLPos2);
            this.Controls.Add(this.numericUpDownXLPos1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBoxProteoforms);
            this.Controls.Add(this.textBoxPeptideSequence1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBoxProteinAnnotator);
            this.Controls.Add(this.buttonPeptideAnotation);
            this.Name = "Form1";
            this.Text = "Protein Anotator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxPeptides.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXLPos2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXLPos1)).EndInit();
            this.groupBoxProteoforms.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonPeptideAnotation;
        private System.Windows.Forms.GroupBox groupBoxProteinAnnotator;
        private System.Windows.Forms.GroupBox groupBoxPeptides;
        private System.Windows.Forms.RichTextBox richTextBoxAnotationBeta;
        private System.Windows.Forms.TextBox textBoxPeptideSequence2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownXLPos2;
        private System.Windows.Forms.NumericUpDown numericUpDownXLPos1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxProteoforms;
        private System.Windows.Forms.RichTextBox richTextBoxAnotationAlfa;
        private System.Windows.Forms.TextBox textBoxPeptideSequence1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBoxUniquePeptides;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

