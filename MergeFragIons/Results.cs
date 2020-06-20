using MergeFragIons.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MergeFragIons
{
    public partial class Results : Form
    {
        private GUI MyGui { get; set; }
        private Core Core;
        public Results()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MyGui.UpdateMaps();
        }

        public void Setup(Core core, GUI _gui)
        {
            MyGui = _gui;
            Core = core;
            this.proteinFragIons1.SetFragMethodDictionary(core.DictMaps, core.ProteinSequence, core.SequenceInformation, core.Has_And_LocalNormalization, core.GlobalNormalization);
            this.userControlFilterCondition1.Setup(Core, false);
            UpdateIntensities();
        }

        private void UpdateIntensities()
        {
            if (Core != null)
            {
                bool _tmp_Has_And_LocalNormalization = Core.Has_And_LocalNormalization;
                bool _tmp_GlobalNormalization = Core.GlobalNormalization;
                checkBoxIntensityPerMap.Checked = _tmp_Has_And_LocalNormalization;
                checkBoxIntensityGlobal.Checked = _tmp_GlobalNormalization;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveImageStripMenuItem_Click(object sender, EventArgs e)
        {
            this.proteinFragIons1.SetInitialXY();
            byte returnAnswer = this.proteinFragIons1.SaveFragmentIonsImage();
            if (returnAnswer == 0)
                System.Windows.Forms.MessageBox.Show("Image saved successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (returnAnswer == 1)
                System.Windows.Forms.MessageBox.Show("Failed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void readMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming soon!\n\nDeveloped by:\nDiogo Borges Lima (CeMM) - diogobor@gmail.com,\nJonathan Dhenin (Institut Pasteur) - jonathan.dhenin@pasteur.fr, & \nMathieu Dupré (Institut Pasteur) - mathieu.dupre@pasteur.fr", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            if (Core.DictMaps.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                        "There is no Map. Please, create one before displaying results.",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                return;
            }

            this.proteinFragIons1.Clear();
            this.proteinFragIons1.SetFragMethodDictionary(Core.DictMaps, Core.ProteinSequence, Core.SequenceInformation, Core.Has_And_LocalNormalization, Core.GlobalNormalization);
            this.tabControl1.SelectedIndex = 0;
        }

        private void checkBoxIntensityPerMap_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxIntensityGlobal.Checked && checkBoxIntensityPerMap.Checked)
                checkBoxIntensityGlobal.Checked = false;

            if (Core != null)
            {
                Core.Has_And_LocalNormalization = checkBoxIntensityPerMap.Checked;
                Core.GlobalNormalization = false;
            }
        }

        private void checkBoxIntensityGlobal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxIntensityPerMap.Checked && checkBoxIntensityGlobal.Checked)
                checkBoxIntensityPerMap.Checked = false;

            if (Core != null)
            {
                Core.GlobalNormalization = checkBoxIntensityGlobal.Checked;
                if (checkBoxIntensityGlobal.Checked)
                    Core.Has_And_LocalNormalization = true;
                else
                    Core.Has_And_LocalNormalization = false;
            }
        }
    }
}
