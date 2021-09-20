using TDFragMapper.Controller;
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

namespace TDFragMapper
{
    public partial class Results : Form
    {
        private GUI MyGui { get; set; }
        private Core Core;
        private Regex numberCaptured = new Regex("[0-9|\\.]+", RegexOptions.Compiled);

        /// <summary>
        /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions)>
        /// </summary>
        private Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double, double)>, bool, bool, List<(string, string)>)> DictMapsWithoutMergeConditions { get; set; }

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
            this.proteinFragIons1.SetFragMethodDictionary_Plot(core.DictMaps, core.ProteinSequence, core.SequenceInformation, core.programParams.ModificationSites, core.Has_And_LocalNormalization, core.GlobalNormalization, false, false, core.IsRelativeIntensity);
            this.userControlFilterCondition1.Setup(Core, false);
            this.UpdateIntensities();
            this.FillListBoxMergeConditions();
        }

        private void UpdateIntensities()
        {
            if (Core != null)
            {
                if (Core.HasIntensities)
                    groupBoxIntensityNorm.Enabled = true;
                else
                    groupBoxIntensityNorm.Enabled = false;

                bool _tmp_Has_And_LocalNormalization = Core.Has_And_LocalNormalization;
                bool _tmp_GlobalNormalization = Core.GlobalNormalization;
                checkBoxIntensityPerMap.Checked = _tmp_Has_And_LocalNormalization;
                checkBoxIntensityGlobal.Checked = _tmp_GlobalNormalization;

                bool _tmp_IsRelativeIntensity = Core.IsRelativeIntensity;
                radioButtonAbsInten.Checked = !_tmp_IsRelativeIntensity;
                radioButtonRelInten.Checked = _tmp_IsRelativeIntensity;

            }
            else
                groupBoxIntensityNorm.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveImageStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void readMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string manuscript_url = "https://protocolexchange.researchsquare.com/article/pex-1051/v1";
            System.Diagnostics.Process.Start(manuscript_url);
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

            #region set intensitiesCheckboxes

            if (Core != null)
            {
                Core.Has_And_LocalNormalization = checkBoxIntensityPerMap.Checked;
                Core.GlobalNormalization = checkBoxIntensityGlobal.Checked;
                if (checkBoxIntensityGlobal.Checked)
                    Core.Has_And_LocalNormalization = true;
            }

            #endregion

            Core.HasMergeMaps = false;
            this.FillListBoxMergeConditions();

            string _key = "Merge#Merge#Merge#0";
            if (Core.DictMaps.ContainsKey(_key))
            {
                Core.DictMaps.Remove(_key);
                Core.HasMergeMaps = false;
            }
            this.proteinFragIons1.Clear();
            this.proteinFragIons1.SetFragMethodDictionary_Plot(Core.DictMaps, Core.ProteinSequence, Core.SequenceInformation, Core.programParams.ModificationSites, Core.Has_And_LocalNormalization, Core.GlobalNormalization, Core.HasMergeMaps, false, Core.IsRelativeIntensity);
            this.tabControl1.SelectedIndex = 0;
        }

        private void FillListBoxMergeConditions()
        {
            listBoxAllMergeConditions.Items.Clear();
            listBoxSelectedMergeConditions.Items.Clear();
            Core.DiscriminativeMaps = new List<string>();
            /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions, isGoldenComplementaryPairs, isBondCleavageConfidence)>
            /// List of All Fragment Ions: FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity, Theoretical Mass
            foreach (KeyValuePair<string, (string, string, string, List<(string, int, string, int, string, int, double, double)>, bool, bool, List<(string, string)>)> entry in Core.DictMaps)
            {
                if (entry.Key.StartsWith("Merge")) continue;

                string[] cols = Regex.Split(entry.Key, "#");
                string studyCondition = cols[0];
                List<string> studyConditions = null;
                string FirstCondition = string.Empty;
                string SecondCondition = string.Empty;
                string ThirdCondition = string.Empty;
                int Map = Convert.ToInt32(cols[3]);
                Map++;

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

                foreach (string study_condition in studyConditions)
                {
                    StringBuilder sbStudyCondition = new StringBuilder();
                    sbStudyCondition.Append("Map ");
                    sbStudyCondition.Append(Map);
                    sbStudyCondition.Append(":");
                    sbStudyCondition.Append(" ");
                    sbStudyCondition.Append(FirstCondition);
                    sbStudyCondition.Append("; ");
                    sbStudyCondition.Append(SecondCondition);
                    sbStudyCondition.Append("; ");
                    sbStudyCondition.Append(ThirdCondition);
                    sbStudyCondition.Append("; ");
                    sbStudyCondition.Append(study_condition);

                    listBoxAllMergeConditions.Items.Add(sbStudyCondition);
                    Core.DiscriminativeMaps.Add(sbStudyCondition.ToString() + "\n");
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

        private void buttonMerge_Click(object sender, EventArgs e)
        {
            DictMapsWithoutMergeConditions = new Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double, double)>, bool, bool, List<(string, string)>)>(Core.DictMaps);
            List<(string, int, string, int, string, int, double, double)> allFragmentIonsAllConditions = new List<(string, int, string, int, string, int, double, double)>();

            foreach (StringBuilder item in listBoxSelectedMergeConditions.Items)
            {
                string[] colsMap = Regex.Split(item.ToString(), ":");
                int map = Convert.ToInt32(numberCaptured.Matches(colsMap[0])[0].Value);
                map--;

                string[] colsCondition = Regex.Split(colsMap[1], ";");
                string first_condition_value = colsCondition[0].Trim();
                string second_condition_value = colsCondition[1].Trim();
                string third_condition_value = colsCondition[2].Trim();
                string study_condition_value = colsCondition[3].Trim();

                /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions, isGoldenComplementaryPairs, isBondCleavageConfidence)>
                /// List of All Fragment Ions: FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity, Theoretical Mass
                foreach (KeyValuePair<string, (string, string, string, List<(string, int, string, int, string, int, double, double)>, bool, bool, List<(string, string)>)> entry in Core.DictMaps)
                {
                    List<(string, int, string, int, string, int, double, double)> FragmentIons = null;
                    if (!entry.Key.StartsWith("Merge") && entry.Key.EndsWith("#" + map.ToString()))
                        FragmentIons = entry.Value.Item4;
                    else
                        continue;

                    #region First condition
                    if (entry.Value.Item1.StartsWith("Frag"))//First condition is 'Fragmentation Method'
                    {
                        string[] colsSubCondition = Regex.Split(first_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item1.Equals(sbCondition)).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    else if (entry.Value.Item1.StartsWith("Act"))//First condition is 'Activation Level'
                    {
                        string[] colsSubCondition = Regex.Split(first_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item5.Equals(sbCondition)).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    else if (entry.Value.Item1.StartsWith("Prec"))//First condition is 'Precursor Charge State'
                    {
                        string[] colsSubCondition = Regex.Split(first_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item2.ToString().Equals(sbCondition)).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    else if (entry.Value.Item1.StartsWith("Repl"))//First condition is 'Replicates'
                    {
                        string[] colsSubCondition = Regex.Split(first_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item6.ToString().Equals(sbCondition.Replace("R", ""))).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    #endregion

                    #region Second condition
                    if (entry.Value.Item2.StartsWith("Frag"))//Second condition is 'Fragmentation Method'
                    {
                        string[] colsSubCondition = Regex.Split(second_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item1.Equals(sbCondition)).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    else if (entry.Value.Item2.StartsWith("Act"))//Second condition is 'Activation Level'
                    {
                        string[] colsSubCondition = Regex.Split(second_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item5.Equals(sbCondition)).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    else if (entry.Value.Item2.StartsWith("Prec"))//Second condition is 'Precursor Charge State'
                    {
                        string[] colsSubCondition = Regex.Split(second_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item2.ToString().Equals(sbCondition)).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    else if (entry.Value.Item2.StartsWith("Repl"))//Second condition is 'Replicates'
                    {
                        string[] colsSubCondition = Regex.Split(second_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item6.ToString().Equals(sbCondition.Replace("R", ""))).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    #endregion

                    #region Third condition
                    if (entry.Value.Item3.StartsWith("Frag"))//Third condition is 'Fragmentation Method'
                    {
                        string[] colsSubCondition = Regex.Split(third_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item1.Equals(sbCondition)).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    else if (entry.Value.Item3.StartsWith("Act"))//Third condition is 'Activation Level'
                    {
                        string[] colsSubCondition = Regex.Split(third_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item5.Equals(sbCondition)).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    else if (entry.Value.Item3.StartsWith("Prec"))//Third condition is 'Precursor Charge State'
                    {
                        string[] colsSubCondition = Regex.Split(third_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item2.ToString().Equals(sbCondition)).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    else if (entry.Value.Item3.StartsWith("Repl"))//Third condition is 'Replicates'
                    {
                        string[] colsSubCondition = Regex.Split(third_condition_value, "&");
                        List<(string, int, string, int, string, int, double, double)> _tmpFragmentIons = new List<(string, int, string, int, string, int, double, double)>();
                        foreach (string sbCondition in colsSubCondition)
                            _tmpFragmentIons.AddRange(FragmentIons.Where(a => a.Item6.ToString().Equals(sbCondition.Replace("R", ""))).ToList());

                        FragmentIons = _tmpFragmentIons;
                    }
                    #endregion

                    #region Study condition
                    string[] cols = Regex.Split(entry.Key, "#");
                    string studyCondition = cols[0];

                    if (studyCondition.StartsWith("Frag"))//Study condition is 'Fragmentation Method'
                    {
                        FragmentIons = FragmentIons.Where(a => a.Item1.Equals(study_condition_value)).ToList();
                    }
                    else if (studyCondition.StartsWith("Act"))//Study condition is 'Activation Level'
                    {
                        FragmentIons = FragmentIons.Where(a => a.Item5.Equals(study_condition_value)).ToList();
                    }
                    else if (studyCondition.StartsWith("Prec"))//Study condition is 'Precursor Charge State'
                    {
                        FragmentIons = FragmentIons.Where(a => a.Item2.ToString().Equals(study_condition_value)).ToList();
                    }
                    else if (studyCondition.StartsWith("Repl"))//Study condition is 'Replicates'
                    {
                        FragmentIons = FragmentIons.Where(a => a.Item6.ToString().Equals(study_condition_value.Replace("R", ""))).ToList();
                    }
                    #endregion

                    allFragmentIonsAllConditions.AddRange(FragmentIons);
                }
            }

            #region creating merged fragIons 

            allFragmentIonsAllConditions = allFragmentIonsAllConditions.Distinct(new Utils.StructureComparer()).ToList();
            List<(string, int, string, int, string, int, double, double)> newFragIons = new List<(string, int, string, int, string, int, double, double)>();
            foreach (var frag in allFragmentIonsAllConditions)
                newFragIons.Add(("", frag.Item2, frag.Item3, frag.Item4, frag.Item5, frag.Item6, frag.Item7, frag.Item8));
            allFragmentIonsAllConditions = newFragIons;
            newFragIons = null;
            if (allFragmentIonsAllConditions.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("There is no condition to merge!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            string _key = "Merge#Merge#Merge#0";
            Core.DictMaps.Add(_key, ("Merge", "Merge", "Merge", allFragmentIonsAllConditions, false, false, new List<(string, string)>()));
            Core.HasMergeMaps = true;
            Core.Has_And_LocalNormalization = true;
            Core.GlobalNormalization = true;

            this.proteinFragIons1.Clear();
            this.proteinFragIons1.SetFragMethodDictionary_Plot(Core.DictMaps, Core.ProteinSequence, Core.SequenceInformation, Core.programParams.ModificationSites, Core.Has_And_LocalNormalization, Core.GlobalNormalization, Core.HasMergeMaps, checkBoxAddCleavageFrequency.Checked);
            this.tabControl1.SelectedIndex = 0;

            Core.DictMaps = DictMapsWithoutMergeConditions;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutScreen = new About();
            aboutScreen.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.Filter = "TDFragMapper results (*.tdfm)|*.tdfm"; // Filter files by extension
            dlg.Title = "Save results";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string fileName = dlg.FileName;
                try
                {
                    Core.SerializeResults(fileName);
                    MessageBox.Show("Results have been stored successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void imageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.proteinFragIons1.SetInitialXY();
            byte returnAnswer = this.proteinFragIons1.SaveFragmentIonsImage();
            if (returnAnswer == 0)
                System.Windows.Forms.MessageBox.Show("Image saved successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (returnAnswer == 1)
                System.Windows.Forms.MessageBox.Show("Failed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void summaryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "summary_report.pdf"; // Default file name
            dlg.Filter = "PDF file (*.pdf)|*.pdf"; // Filter files by extension
            dlg.Title = "Summary report";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                byte returnAnswer = Core.ExportResultsToPDF(dlg.FileName);
                if (returnAnswer == 0)
                    System.Windows.Forms.MessageBox.Show("Data has been exported successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (returnAnswer == 1)
                    System.Windows.Forms.MessageBox.Show("Export data failed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Results_ResizeEnd(object sender, EventArgs e)
        {
            int height_maps = this.userControlFilterCondition1.Height + 60;
            this.tabPageFilter.AutoScrollMinSize = new Size(this.tabPageFilter.AutoScrollMinSize.Width, height_maps);
        }

        private void userControlFilterCondition1_Resize(object sender, EventArgs e)
        {
            Results_ResizeEnd(sender, e);
        }

        private void radioButtonRelInten_CheckedChanged(object sender, EventArgs e)
        {
            if (Core != null)
                Core.IsRelativeIntensity = radioButtonRelInten.Checked;
        }

        private void radioButtonAbsInten_CheckedChanged(object sender, EventArgs e)
        {
            if (Core != null)
                Core.IsRelativeIntensity = !radioButtonAbsInten.Checked;
        }

        private void templateFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string template_url = "https://github.com/diogobor/TDFragMapper/tree/master/Templates";
            System.Diagnostics.Process.Start(template_url);
        }
    }
}
