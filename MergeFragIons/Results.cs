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
        private Core Core;
        private Dictionary<string, List<int>> DictFragMethodsWithPrecursorChargeStates { get; set; }
        private Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double)>)> initialDictionaryMaps { get; set; }
        public Results()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Core.DictMaps = initialDictionaryMaps;
        }

        public void Setup(Core core)
        {
            Core = core;
            initialDictionaryMaps = new Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double)>)>(core.DictMaps);
            this.proteinFragIons1.SetFragMethodDictionary(core.DictMaps, core.ProteinSequence, core.SequenceInformation, true, false);
            this.userControlFilterCondition1.Setup(Core, false);
        }

        private void LoadListBoxConditions(ListBox listBoxAllFragMethods = null, ListBox listBoxAllPrecursorChargeStates = null)
        {
            if (Core == null ||
                Core.AllFragmentationMethods == null ||
                Core.AllFragmentationMethods.Count == 0 ||
                Core.AllPrecursorChargeStates == null ||
                Core.AllPrecursorChargeStates.Count == 0) return;

            foreach (string fragMet in Core.AllFragmentationMethods)
                listBoxAllFragMethods.Items.Add(fragMet);

            foreach (int precursorChargeState in Core.AllPrecursorChargeStates)
                listBoxAllPrecursorChargeStates.Items.Add(precursorChargeState);
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
            this.proteinFragIons1.SetFragMethodDictionary(Core.DictMaps, Core.ProteinSequence, Core.SequenceInformation);
            this.tabControl1.SelectedIndex = 0;
        }
    }
}
