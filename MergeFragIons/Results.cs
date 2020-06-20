using MergeFragIons.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
            this.UpdateIntensities();
            this.FillListBoxMergeConditions();
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

            this.FillListBoxMergeConditions();
            this.proteinFragIons1.Clear();
            this.proteinFragIons1.SetFragMethodDictionary(Core.DictMaps, Core.ProteinSequence, Core.SequenceInformation, Core.Has_And_LocalNormalization, Core.GlobalNormalization);
            this.tabControl1.SelectedIndex = 0;
        }

        private void FillListBoxMergeConditions()
        {
            /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions)>
            /// List of All Fragment Ions: FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity
            foreach (KeyValuePair<string, (string, string, string, List<(string, int, string, int, string, int, double)>)> entry in Core.DictMaps)
            {
                string[] cols = Regex.Split(entry.Key, "#");
                string studyCondition = cols[0];
                List<string> studyConditions = null;
                string FirstCondition = string.Empty;
                string SecondCondition = string.Empty;
                string ThirdCondition = string.Empty;

                #region Study Condition
                if (studyCondition.StartsWith("Frag"))//First condition is 'Fragmentation Method'
                {
                    studyConditions = (from a in entry.Value.Item4
                                       select a.Item1).Distinct().ToList();
                }
                else if (studyCondition.StartsWith("Act"))//First condition is 'Activation Level'
                {
                    studyConditions = (from a in entry.Value.Item4
                                       select a.Item5).Distinct().ToList();
                }
                else if (studyCondition.StartsWith("Prec"))//First condition is 'Precursor Charge State'
                {
                    studyConditions = (from a in entry.Value.Item4
                                       select a.Item2.ToString()).Distinct().ToList();
                }
                else if (studyCondition.StartsWith("Repl"))//First condition is 'Replicates'
                {
                    studyConditions = (from a in entry.Value.Item4
                                       select a.Item6.ToString()).Distinct().ToList();
                }
                #endregion

                #region First condition
                if (entry.Value.Item1.StartsWith("Frag"))//First condition is 'Fragmentation Method'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select a.Item1).Distinct().ToList();
                    FirstCondition = String.Join("&", conditionList);
                }
                else if (entry.Value.Item1.StartsWith("Act"))//First condition is 'Activation Level'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select a.Item5).Distinct().ToList();
                    FirstCondition = String.Join("&", conditionList);
                }
                else if (entry.Value.Item1.StartsWith("Prec"))//First condition is 'Precursor Charge State'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select a.Item2.ToString()).Distinct().ToList();
                    FirstCondition = String.Join("&", conditionList);
                }
                else if (entry.Value.Item1.StartsWith("Repl"))//First condition is 'Replicates'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select "R" + a.Item6.ToString()).Distinct().ToList();
                    FirstCondition = String.Join("&", conditionList);
                }
                #endregion

                #region Second condition
                if (entry.Value.Item2.StartsWith("Frag"))//First condition is 'Fragmentation Method'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select a.Item1).Distinct().ToList();
                    SecondCondition = String.Join("&", conditionList);
                }
                else if (entry.Value.Item2.StartsWith("Act"))//First condition is 'Activation Level'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select a.Item5).Distinct().ToList();
                    SecondCondition = String.Join("&", conditionList);
                }
                else if (entry.Value.Item2.StartsWith("Prec"))//First condition is 'Precursor Charge State'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select a.Item2.ToString()).Distinct().ToList();
                    SecondCondition = String.Join("&", conditionList);
                }
                else if (entry.Value.Item2.StartsWith("Repl"))//First condition is 'Replicates'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select "R" + a.Item6.ToString()).Distinct().ToList();
                    SecondCondition = String.Join("&", conditionList);
                }
                #endregion

                #region Third condition
                if (entry.Value.Item3.StartsWith("Frag"))//First condition is 'Fragmentation Method'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select a.Item1).Distinct().ToList();
                    ThirdCondition = String.Join("&", conditionList);
                }
                else if (entry.Value.Item3.StartsWith("Act"))//First condition is 'Activation Level'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select a.Item5).Distinct().ToList();
                    ThirdCondition = String.Join("&", conditionList);
                }
                else if (entry.Value.Item3.StartsWith("Prec"))//First condition is 'Precursor Charge State'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select a.Item2.ToString()).Distinct().ToList();
                    ThirdCondition = String.Join("&", conditionList);
                }
                else if (entry.Value.Item3.StartsWith("Repl"))//First condition is 'Replicates'
                {
                    List<string> conditionList = (from a in entry.Value.Item4
                                                  select "R" + a.Item6.ToString()).Distinct().ToList();
                    ThirdCondition = String.Join("&", conditionList);
                }
                #endregion

                listBoxAllMergeConditions.Items.Clear();
                
                foreach (string study_condition in studyConditions)
                {
                    StringBuilder sbStudyCondition = new StringBuilder();
                    sbStudyCondition.Append(FirstCondition);
                    sbStudyCondition.Append("; ");
                    sbStudyCondition.Append(SecondCondition);
                    sbStudyCondition.Append("; ");
                    sbStudyCondition.Append(ThirdCondition);
                    sbStudyCondition.Append("; ");
                    sbStudyCondition.Append(study_condition);

                    listBoxAllMergeConditions.Items.Add(sbStudyCondition);
                }
            }
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

        private void buttonIntensity_Click(object sender, EventArgs e)
        {
            buttonFilter_Click(sender, e);
        }

        private void buttonAddMergeCondition_Click(object sender, EventArgs e)
        {
            List<object> removedMergedCondition = new List<object>();
            //Get all values into listbox
            foreach (object item in listBoxAllMergeConditions.SelectedItems)
            {
                listBoxSelectedMergeConditions.Items.Add(item);
                removedMergedCondition.Add(item);
            }

            removedMergedCondition.ForEach(item =>
            {
                listBoxAllMergeConditions.Items.Remove(item);
            });
        }

        private void buttonRemoveMergeCondition_Click(object sender, EventArgs e)
        {
            List<object> removedSelectedMergedCondition = new List<object>();
            //Get all values into listbox
            foreach (object item in listBoxSelectedMergeConditions.SelectedItems)
            {
                listBoxAllMergeConditions.Items.Add(item);
                removedSelectedMergedCondition.Add(item);
            }

            removedSelectedMergedCondition.ForEach(item =>
            {
                listBoxSelectedMergeConditions.Items.Remove(item);
            });
        }
    }
}
