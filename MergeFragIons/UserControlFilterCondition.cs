using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDFragMapper.Controller;
using System.Text.RegularExpressions;

namespace TDFragMapper
{
    public partial class UserControlFilterCondition : UserControl
    {
        private Core Core { get; set; }
        private Regex numberCaptured = new Regex("[0-9|\\.]+", RegexOptions.Compiled);

        // List of Fragment Ions: FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity
        private List<(string, int, string, int, string, int, double, string)> allFragmentIonsAllConditions { get; set; }

        private List<(Button, Button)> Add_Remove_MapBtnList { get; set; }

        private int numberOfConditions = 1;
        private int SPACE_Y = 263;

        public UserControlFilterCondition()
        {
            InitializeComponent();
        }

        public void ResetMaps()
        {
            foreach (Control c in this.Controls)
            {
                if (c is GroupBox)
                {
                    if (!c.Name.Contains("groupBoxMap_0"))
                        c.Visible = false;
                }
            }
            numberOfConditions = 1;
        }

        private void SetTagsInitialCondition()
        {
            buttonAddFixedCondition1.Tag = new object[]{
                comboBoxCondition1_0
            };

            buttonRemoveFixedCondition1.Tag = new object[] {
                comboBoxCondition1_0
            };

            buttonAddFixedCondition2.Tag = new object[] {
                comboBoxCondition1_0
            };

            buttonRemoveFixedCondition2.Tag = new object[] {
                comboBoxCondition1_0
            };

            buttonAddFixedCondition3.Tag = new object[] {
                comboBoxCondition1_0
            };

            buttonRemoveFixedCondition3.Tag = new object[] {
                comboBoxCondition1_0
            };

            buttonAddStudyCondition.Tag = new object[] {
                comboBoxCondition1_0
            };

            buttonRemoveStudyCondition.Tag = new object[] {
                comboBoxCondition1_0
            };

            comboBoxCondition1_0.Tag = new object[] {
                comboBoxCondition1_0,
                comboBoxCondition2_0,
                comboBoxCondition3_0,
                comboBoxStudyCondition_0,
                listBoxAllFixedCondition1,
                listBoxAllFixedCondition2,
                listBoxAllFixedCondition3,
                listBoxAllStudyCondition,
                listBoxSelectedFixedCondition1,
                listBoxSelectedFixedCondition2,
                listBoxSelectedFixedCondition3,
                listBoxSelectedStudyCondition0,
                buttonAddMap_0,
                buttonAddFixedCondition1,
                allFragmentIonsAllConditions,
                checkBoxGoldenComplemPairs0,
                checkBoxCleavageConfidence0
            };

            comboBoxCondition2_0.Tag = new object[] {
                comboBoxCondition1_0
            };

            comboBoxCondition3_0.Tag = new object[] {
                comboBoxCondition1_0
            };

            comboBoxStudyCondition_0.Tag = new object[] {
                comboBoxCondition1_0
            };

            Add_Remove_MapBtnList = new List<(Button, Button)>() { (buttonAddMap_0, null) };
        }

        public void Setup(Core core, bool isInitialResults = true)
        {
            Core = core;

            ResetFields(listBoxAllFixedCondition1,
                listBoxAllFixedCondition2,
                listBoxAllFixedCondition3,
                listBoxSelectedFixedCondition1,
                listBoxSelectedFixedCondition2,
                listBoxSelectedFixedCondition3,
                listBoxAllStudyCondition,
                listBoxSelectedStudyCondition0,
                comboBoxCondition1_0,
                comboBoxCondition2_0,
                comboBoxCondition3_0,
                comboBoxStudyCondition_0);

            if (Core != null)
            {
                allFragmentIonsAllConditions = Core.FragmentIons;
                SetTagsInitialCondition();
                if (isInitialResults)
                {
                    Core.DictMaps = new Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double, string)>, bool, bool)>();
                    comboBoxCondition2_0.Enabled = false;
                    comboBoxCondition3_0.Enabled = false;
                    comboBoxStudyCondition_0.Enabled = false;
                    checkBoxGoldenComplemPairs0.Checked = false;
                    checkBoxGoldenComplemPairs0.Enabled = false;
                    checkBoxCleavageConfidence0.Checked = false;
                }
                else
                    UpdateMaps();
            }
        }
        public void UpdateMaps()
        {
            int countCondition = 0;
            Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double, string)>, bool, bool)> _DictMaps = new Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double, string)>, bool, bool)>(Core.DictMaps);
            Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double, string)>, bool, bool)> _DictMapsToBeChanged = new Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double, string)>, bool, bool)>(Core.DictMaps);
            List<string> selectedItemsCondition1 = null;
            List<string> selectedItemsCondition2 = null;
            List<string> selectedItemsCondition3 = null;
            List<string> selectedItemsStudyCondition = null;

            List<(string, int, string, int, string, int, double, string)> allFragmentIonsAllConditions = null;
            List<(string, int, string, int, string, int, double, string)> _tempFragMethods = null;
            List<string> RemainItemscondition1 = null;
            List<string> RemainItemscondition2 = null;
            List<string> RemainItemscondition3 = null;
            List<string> RemainItemsstudycondition = null;

            foreach (KeyValuePair<string, (string, string, string, List<(string, int, string, int, string, int, double, string)>, bool, bool)> entry in _DictMaps)
            {
                allFragmentIonsAllConditions = Core.FragmentIons;
                _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                if (countCondition == 0)
                {
                    //For filtering

                    #region fill comboboxes
                    int _index = 0;
                    if (entry.Value.Item1.ToString().StartsWith("Frag"))
                        _index = 0;
                    else if (entry.Value.Item1.ToString().StartsWith("Activation"))
                        _index = 1;
                    else if (entry.Value.Item1.ToString().StartsWith("Replicates"))
                        _index = 2;
                    else if (entry.Value.Item1.ToString().StartsWith("Precursor"))
                        _index = 3;
                    comboBoxCondition1_0.SelectedIndex = _index;

                    if (entry.Value.Item2.ToString().StartsWith("Frag"))
                        _index = 0;
                    else if (entry.Value.Item2.ToString().StartsWith("Activation"))
                        _index = 1;
                    else if (entry.Value.Item2.ToString().StartsWith("Replicates"))
                        _index = 2;
                    else if (entry.Value.Item2.ToString().StartsWith("Precursor"))
                        _index = 3;
                    comboBoxCondition2_0.SelectedIndex = _index;

                    if (entry.Value.Item3.ToString().StartsWith("Frag"))
                        _index = 0;
                    else if (entry.Value.Item3.ToString().StartsWith("Activation"))
                        _index = 1;
                    else if (entry.Value.Item3.ToString().StartsWith("Replicates"))
                        _index = 2;
                    else if (entry.Value.Item3.ToString().StartsWith("Precursor"))
                        _index = 3;
                    comboBoxCondition3_0.SelectedIndex = _index;

                    if (entry.Key.ToString().StartsWith("Frag"))
                        _index = 0;
                    else if (entry.Key.ToString().StartsWith("Activation"))
                        _index = 1;
                    else if (entry.Key.ToString().StartsWith("Replicates"))
                        _index = 2;
                    else if (entry.Key.ToString().StartsWith("Precursor"))
                        _index = 3;
                    comboBoxStudyCondition_0.SelectedIndex = _index;
                    #endregion

                    #region fill fixed condition1 selected items
                    listBoxAllFixedCondition1.Items.Clear();
                    listBoxSelectedFixedCondition1.Items.Clear();
                    if (entry.Value.Item1.ToString().StartsWith("Frag"))
                    {
                        selectedItemsCondition1 = (from item in entry.Value.Item4
                                                   select item.Item1).Distinct().ToList();
                        RemainItemscondition1 = allFragmentIonsAllConditions.Select(a => a.Item1).Distinct().Except(selectedItemsCondition1).ToList();
                        listBoxAllFixedCondition1.Items.AddRange(RemainItemscondition1.ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition1)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item1.ToString().StartsWith("Activation"))
                    {
                        selectedItemsCondition1 = (from item in entry.Value.Item4
                                                   select item.Item5).Distinct().ToList();
                        RemainItemscondition1 = allFragmentIonsAllConditions.Select(a => a.Item5).Distinct().Except(selectedItemsCondition1).ToList();
                        listBoxAllFixedCondition1.Items.AddRange(RemainItemscondition1.ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition1)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item1.ToString().StartsWith("Replicates"))
                    {
                        selectedItemsCondition1 = (from item in entry.Value.Item4
                                                   select item.Item6.ToString()).Distinct().ToList();
                        RemainItemscondition1 = allFragmentIonsAllConditions.Select(a => a.Item6.ToString()).Distinct().Except(selectedItemsCondition1).ToList();
                        listBoxAllFixedCondition1.Items.AddRange((from item in RemainItemscondition1
                                                                  select "R" + item).ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition1)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item1.ToString().StartsWith("Precursor"))
                    {
                        selectedItemsCondition1 = (from item in entry.Value.Item4
                                                   select item.Item2.ToString()).Distinct().ToList();
                        RemainItemscondition1 = allFragmentIonsAllConditions.Select(a => a.Item2.ToString()).Distinct().Except(selectedItemsCondition1).ToList();
                        listBoxAllFixedCondition1.Items.AddRange(RemainItemscondition1.ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition1)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }

                    if (entry.Value.Item1.ToString().StartsWith("Replicates"))
                        listBoxSelectedFixedCondition1.Items.AddRange((from item in selectedItemsCondition1
                                                                       select "R" + item).ToArray());
                    else
                        listBoxSelectedFixedCondition1.Items.AddRange(selectedItemsCondition1.ToArray());
                    #endregion

                    #region fill fixed condition2 selected items
                    listBoxAllFixedCondition2.Items.Clear();
                    listBoxSelectedFixedCondition2.Items.Clear();
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                    if (entry.Value.Item2.ToString().StartsWith("Frag"))
                    {
                        selectedItemsCondition2 = (from item in entry.Value.Item4
                                                   select item.Item1).Distinct().ToList();

                        RemainItemscondition2 = allFragmentIonsAllConditions.Select(a => a.Item1).Distinct().Except(selectedItemsCondition2).ToList();
                        listBoxAllFixedCondition2.Items.AddRange(RemainItemscondition2.ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition2)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item2.ToString().StartsWith("Activation"))
                    {
                        selectedItemsCondition2 = (from item in entry.Value.Item4
                                                   select item.Item5).Distinct().ToList();

                        RemainItemscondition2 = allFragmentIonsAllConditions.Select(a => a.Item5).Distinct().Except(selectedItemsCondition2).ToList();
                        listBoxAllFixedCondition2.Items.AddRange(RemainItemscondition2.ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition2)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item2.ToString().StartsWith("Replicates"))
                    {
                        selectedItemsCondition2 = (from item in entry.Value.Item4
                                                   select item.Item6.ToString()).Distinct().ToList();

                        RemainItemscondition2 = allFragmentIonsAllConditions.Select(a => a.Item6.ToString()).Distinct().Except(selectedItemsCondition2).ToList();
                        listBoxAllFixedCondition2.Items.AddRange((from item in RemainItemscondition2
                                                                  select "R" + item).ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition2)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item2.ToString().StartsWith("Precursor"))
                    {
                        selectedItemsCondition2 = (from item in entry.Value.Item4
                                                   select item.Item2.ToString()).Distinct().ToList();

                        RemainItemscondition2 = allFragmentIonsAllConditions.Select(a => a.Item2.ToString()).Distinct().Except(selectedItemsCondition2).ToList();
                        listBoxAllFixedCondition2.Items.AddRange(RemainItemscondition2.ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition2)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }

                    if (entry.Value.Item2.ToString().StartsWith("Replicates"))
                        listBoxSelectedFixedCondition2.Items.AddRange((from item in selectedItemsCondition2
                                                                       select "R" + item).ToArray());
                    else
                        listBoxSelectedFixedCondition2.Items.AddRange(selectedItemsCondition2.ToArray());
                    #endregion

                    #region fill fixed condition3 selected items
                    listBoxAllFixedCondition3.Items.Clear();
                    listBoxSelectedFixedCondition3.Items.Clear();
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                    if (entry.Value.Item3.ToString().StartsWith("Frag"))
                    {
                        selectedItemsCondition3 = (from item in entry.Value.Item4
                                                   select item.Item1).Distinct().ToList();

                        RemainItemscondition3 = allFragmentIonsAllConditions.Select(a => a.Item1.ToString()).Distinct().Except(selectedItemsCondition3).ToList();
                        listBoxAllFixedCondition3.Items.AddRange(RemainItemscondition3.ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition3)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;

                    }
                    else if (entry.Value.Item3.ToString().StartsWith("Activation"))
                    {
                        selectedItemsCondition3 = (from item in entry.Value.Item4
                                                   select item.Item5).Distinct().ToList();

                        RemainItemscondition3 = allFragmentIonsAllConditions.Select(a => a.Item5.ToString()).Distinct().Except(selectedItemsCondition3).ToList();
                        listBoxAllFixedCondition3.Items.AddRange(RemainItemscondition3.ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition3)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item3.ToString().StartsWith("Replicates"))
                    {
                        selectedItemsCondition3 = (from item in entry.Value.Item4
                                                   select item.Item6.ToString()).Distinct().ToList();

                        RemainItemscondition3 = allFragmentIonsAllConditions.Select(a => a.Item6.ToString()).Distinct().Except(selectedItemsCondition3).ToList();
                        listBoxAllFixedCondition3.Items.AddRange((from item in RemainItemscondition3
                                                                  select "R" + item).ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition3)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item3.ToString().StartsWith("Precursor"))
                    {
                        selectedItemsCondition3 = (from item in entry.Value.Item4
                                                   select item.Item2.ToString()).Distinct().ToList();

                        RemainItemscondition3 = allFragmentIonsAllConditions.Select(a => a.Item2.ToString()).Distinct().Except(selectedItemsCondition3).ToList();
                        listBoxAllFixedCondition3.Items.AddRange(RemainItemscondition3.ToArray());

                        //Filter
                        foreach (string item in selectedItemsCondition3)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList().ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }

                    if (entry.Value.Item3.ToString().StartsWith("Replicates"))
                        listBoxSelectedFixedCondition3.Items.AddRange((from item in selectedItemsCondition3
                                                                       select "R" + item).ToArray());
                    else
                        listBoxSelectedFixedCondition3.Items.AddRange(selectedItemsCondition3.ToArray());
                    #endregion

                    #region fill Study selected items
                    listBoxAllStudyCondition.Items.Clear();
                    listBoxSelectedStudyCondition0.Items.Clear();
                    if (entry.Key.ToString().StartsWith("Frag"))
                    {
                        selectedItemsStudyCondition = (from item in entry.Value.Item4
                                                       select item.Item1).Distinct().ToList();

                        RemainItemsstudycondition = allFragmentIonsAllConditions.Select(a => a.Item1).Distinct().Except(selectedItemsStudyCondition).ToList();
                        listBoxAllStudyCondition.Items.AddRange(RemainItemsstudycondition.ToArray());
                    }
                    else if (entry.Key.ToString().StartsWith("Activation"))
                    {
                        selectedItemsStudyCondition = (from item in entry.Value.Item4
                                                       select item.Item5).Distinct().ToList();

                        RemainItemsstudycondition = allFragmentIonsAllConditions.Select(a => a.Item5).Distinct().Except(selectedItemsStudyCondition).ToList();
                        listBoxAllStudyCondition.Items.AddRange(RemainItemsstudycondition.ToArray());

                    }
                    else if (entry.Key.ToString().StartsWith("Replicates"))
                    {
                        selectedItemsStudyCondition = (from item in entry.Value.Item4
                                                       select item.Item6.ToString()).Distinct().ToList();
                        RemainItemsstudycondition = allFragmentIonsAllConditions.Select(a => a.Item6.ToString()).Distinct().Except(selectedItemsStudyCondition).ToList();
                        listBoxAllStudyCondition.Items.AddRange((from item in RemainItemsstudycondition
                                                                 select "R" + item).ToArray());
                    }
                    else if (entry.Key.ToString().StartsWith("Precursor"))
                    {
                        selectedItemsStudyCondition = (from item in entry.Value.Item4
                                                       select item.Item2.ToString()).Distinct().ToList();

                        RemainItemsstudycondition = allFragmentIonsAllConditions.Select(a => a.Item2.ToString()).Distinct().Except(selectedItemsStudyCondition).ToList();
                        listBoxAllStudyCondition.Items.AddRange(RemainItemsstudycondition.ToArray());
                    }

                    if (entry.Key.ToString().StartsWith("Replicates"))
                        listBoxSelectedStudyCondition0.Items.AddRange((from item in selectedItemsStudyCondition
                                                                       select "R" + item).ToArray());
                    else
                        listBoxSelectedStudyCondition0.Items.AddRange(selectedItemsStudyCondition.ToArray());
                    #endregion

                    comboBoxCondition1_0.Enabled = true;
                    comboBoxCondition2_0.Enabled = true;
                    comboBoxCondition3_0.Enabled = true;
                    comboBoxStudyCondition_0.Enabled = true;

                    #region Enabling checkboxes
                    Core.DictMaps = _DictMapsToBeChanged;
                    if (entry.Value.Item5)
                    {
                        checkBoxGoldenComplemPairs0.Enabled = true;
                        checkBoxGoldenComplemPairs0.Checked = entry.Value.Item5;
                    }
                    checkBoxCleavageConfidence0.Checked = entry.Value.Item6;

                    if (listBoxSelectedFixedCondition1.Items.Count == 1 &&
                        listBoxSelectedFixedCondition2.Items.Count == 1 &&
                        listBoxSelectedFixedCondition3.Items.Count == 1)
                        checkBoxGoldenComplemPairs0.Enabled = true;

                    #endregion

                    _DictMapsToBeChanged.Clear();
                    _DictMapsToBeChanged = null;
                    if (_DictMaps.Count > 1)
                        buttonAddMap_0.Enabled = false;
                    //else if (EnableAddNewMapBtn(comboBoxCondition1_0, comboBoxCondition2_0, comboBoxCondition3_0, comboBoxStudyCondition_0))
                    //    buttonAddMap_0.Enabled = true;
                }
                else
                {
                    string[] cols = Regex.Split(entry.Key, "#");
                    numberOfConditions = Convert.ToInt32(cols[3]);
                    #region fill comboboxes
                    int _indexcb1 = 0;
                    if (entry.Value.Item1.ToString().StartsWith("Frag"))
                        _indexcb1 = 0;
                    else if (entry.Value.Item1.ToString().StartsWith("Activation"))
                        _indexcb1 = 1;
                    else if (entry.Value.Item1.ToString().StartsWith("Replicates"))
                        _indexcb1 = 2;
                    else if (entry.Value.Item1.ToString().StartsWith("Precursor"))
                        _indexcb1 = 3;

                    int _indexcb2 = 0;
                    if (entry.Value.Item2.ToString().StartsWith("Frag"))
                        _indexcb2 = 0;
                    else if (entry.Value.Item2.ToString().StartsWith("Activation"))
                        _indexcb2 = 1;
                    else if (entry.Value.Item2.ToString().StartsWith("Replicates"))
                        _indexcb2 = 2;
                    else if (entry.Value.Item2.ToString().StartsWith("Precursor"))
                        _indexcb2 = 3;

                    int _indexcb3 = 0;
                    if (entry.Value.Item3.ToString().StartsWith("Frag"))
                        _indexcb3 = 0;
                    else if (entry.Value.Item3.ToString().StartsWith("Activation"))
                        _indexcb3 = 1;
                    else if (entry.Value.Item3.ToString().StartsWith("Replicates"))
                        _indexcb3 = 2;
                    else if (entry.Value.Item3.ToString().StartsWith("Precursor"))
                        _indexcb3 = 3;

                    int _indexcbStudy = 0;
                    if (entry.Key.ToString().StartsWith("Frag"))
                        _indexcbStudy = 0;
                    else if (entry.Key.ToString().StartsWith("Activation"))
                        _indexcbStudy = 1;
                    else if (entry.Key.ToString().StartsWith("Replicates"))
                        _indexcbStudy = 2;
                    else if (entry.Key.ToString().StartsWith("Precursor"))
                        _indexcbStudy = 3;
                    #endregion

                    #region fill fixed condition1 selected items
                    if (entry.Value.Item1.ToString().StartsWith("Frag"))
                    {
                        selectedItemsCondition1 = (from item in entry.Value.Item4
                                                   select item.Item1).Distinct().ToList();
                        RemainItemscondition1 = allFragmentIonsAllConditions.Select(a => a.Item1).Distinct().Except(selectedItemsCondition1).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition1)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item1.ToString().StartsWith("Activation"))
                    {
                        selectedItemsCondition1 = (from item in entry.Value.Item4
                                                   select item.Item5).Distinct().ToList();
                        RemainItemscondition1 = allFragmentIonsAllConditions.Select(a => a.Item5).Distinct().Except(selectedItemsCondition1).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition1)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item1.ToString().StartsWith("Replicates"))
                    {
                        selectedItemsCondition1 = (from item in entry.Value.Item4
                                                   select item.Item6.ToString()).Distinct().ToList();

                        RemainItemscondition1 = (from item in allFragmentIonsAllConditions.Select(a => a.Item6.ToString()).Distinct().Except(selectedItemsCondition1)
                                                 select "R" + item).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition1)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                        allFragmentIonsAllConditions = _tempFragMethods;

                        selectedItemsCondition1 = (from item in selectedItemsCondition1
                                                   select "R" + item).ToList();
                    }
                    else if (entry.Value.Item1.ToString().StartsWith("Precursor"))
                    {
                        selectedItemsCondition1 = (from item in entry.Value.Item4
                                                   select item.Item2.ToString()).Distinct().ToList();
                        RemainItemscondition1 = allFragmentIonsAllConditions.Select(a => a.Item2.ToString()).Distinct().Except(selectedItemsCondition1).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition1)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }

                    #endregion

                    #region fill fixed condition2 selected items
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                    if (entry.Value.Item2.ToString().StartsWith("Frag"))
                    {
                        selectedItemsCondition2 = (from item in entry.Value.Item4
                                                   select item.Item1).Distinct().ToList();

                        RemainItemscondition2 = allFragmentIonsAllConditions.Select(a => a.Item1).Distinct().Except(selectedItemsCondition2).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition2)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item2.ToString().StartsWith("Activation"))
                    {
                        selectedItemsCondition2 = (from item in entry.Value.Item4
                                                   select item.Item5).Distinct().ToList();

                        RemainItemscondition2 = allFragmentIonsAllConditions.Select(a => a.Item5).Distinct().Except(selectedItemsCondition2).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition2)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item2.ToString().StartsWith("Replicates"))
                    {
                        selectedItemsCondition2 = (from item in entry.Value.Item4
                                                   select item.Item6.ToString()).Distinct().ToList();

                        RemainItemscondition2 = (from item in allFragmentIonsAllConditions.Select(a => a.Item6.ToString()).Distinct().Except(selectedItemsCondition2)
                                                 select "R" + item).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition2)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                        allFragmentIonsAllConditions = _tempFragMethods;

                        selectedItemsCondition2 = (from item in selectedItemsCondition2
                                                   select "R" + item).ToList();
                    }
                    else if (entry.Value.Item2.ToString().StartsWith("Precursor"))
                    {
                        selectedItemsCondition2 = (from item in entry.Value.Item4
                                                   select item.Item2.ToString()).Distinct().ToList();

                        RemainItemscondition2 = allFragmentIonsAllConditions.Select(a => a.Item2.ToString()).Distinct().Except(selectedItemsCondition2).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition2)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }

                    #endregion

                    #region fill fixed condition3 selected items
                    listBoxAllFixedCondition3.Items.Clear();
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                    if (entry.Value.Item3.ToString().StartsWith("Frag"))
                    {
                        selectedItemsCondition3 = (from item in entry.Value.Item4
                                                   select item.Item1).Distinct().ToList();

                        RemainItemscondition3 = allFragmentIonsAllConditions.Select(a => a.Item1.ToString()).Distinct().Except(selectedItemsCondition3).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition3)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;

                    }
                    else if (entry.Value.Item3.ToString().StartsWith("Activation"))
                    {
                        selectedItemsCondition3 = (from item in entry.Value.Item4
                                                   select item.Item5).Distinct().ToList();

                        RemainItemscondition3 = allFragmentIonsAllConditions.Select(a => a.Item5.ToString()).Distinct().Except(selectedItemsCondition3).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition3)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }
                    else if (entry.Value.Item3.ToString().StartsWith("Replicates"))
                    {
                        selectedItemsCondition3 = (from item in entry.Value.Item4
                                                   select item.Item6.ToString()).Distinct().ToList();

                        RemainItemscondition3 = (from item in allFragmentIonsAllConditions.Select(a => a.Item6.ToString()).Distinct().Except(selectedItemsCondition3)
                                                 select "R" + item).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition3)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                        allFragmentIonsAllConditions = _tempFragMethods;

                        selectedItemsCondition3 = (from item in selectedItemsCondition3
                                                   select "R" + item).ToList();
                    }
                    else if (entry.Value.Item3.ToString().StartsWith("Precursor"))
                    {
                        selectedItemsCondition3 = (from item in entry.Value.Item4
                                                   select item.Item2.ToString()).Distinct().ToList();

                        RemainItemscondition3 = allFragmentIonsAllConditions.Select(a => a.Item2.ToString()).Distinct().Except(selectedItemsCondition3).ToList();

                        //Filter
                        foreach (string item in selectedItemsCondition3)
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList().ToList());
                        allFragmentIonsAllConditions = _tempFragMethods;
                    }

                    #endregion

                    #region fill Study selected items
                    if (entry.Key.ToString().StartsWith("Frag"))
                    {
                        selectedItemsStudyCondition = (from item in entry.Value.Item4
                                                       select item.Item1).Distinct().ToList();

                        RemainItemsstudycondition = allFragmentIonsAllConditions.Select(a => a.Item1).Distinct().Except(selectedItemsStudyCondition).ToList();
                    }
                    else if (entry.Key.ToString().StartsWith("Activation"))
                    {
                        selectedItemsStudyCondition = (from item in entry.Value.Item4
                                                       select item.Item5).Distinct().ToList();

                        RemainItemsstudycondition = allFragmentIonsAllConditions.Select(a => a.Item5).Distinct().Except(selectedItemsStudyCondition).ToList();

                    }
                    else if (entry.Key.ToString().StartsWith("Replicates"))
                    {
                        selectedItemsStudyCondition = (from item in entry.Value.Item4
                                                       select item.Item6.ToString()).Distinct().ToList();
                        RemainItemsstudycondition = (from item in allFragmentIonsAllConditions.Select(a => a.Item6.ToString()).Distinct().Except(selectedItemsStudyCondition)
                                                     select "R" + item).ToList();
                        selectedItemsStudyCondition = (from item in selectedItemsStudyCondition
                                                       select "R" + item).ToList();
                    }
                    else if (entry.Key.ToString().StartsWith("Precursor"))
                    {
                        selectedItemsStudyCondition = (from item in entry.Value.Item4
                                                       select item.Item2.ToString()).Distinct().ToList();

                        RemainItemsstudycondition = allFragmentIonsAllConditions.Select(a => a.Item2.ToString()).Distinct().Except(selectedItemsStudyCondition).ToList();
                    }

                    #endregion

                    try
                    {
                        (Button, Button) previousAddRemoveMap = Add_Remove_MapBtnList[numberOfConditions - 1];
                        previousAddRemoveMap.Item1.Enabled = false;
                        if (previousAddRemoveMap.Item2 != null)
                            previousAddRemoveMap.Item2.Enabled = false;
                    }
                    catch (Exception) { }

                    CreateNewMap(false,
                        _indexcb1,
                        _indexcb2,
                        _indexcb3,
                        _indexcbStudy,
                        RemainItemscondition1,
                        RemainItemscondition2,
                        RemainItemscondition3,
                        RemainItemsstudycondition,
                        selectedItemsCondition1,
                        selectedItemsCondition2,
                        selectedItemsCondition3,
                        selectedItemsStudyCondition,
                        entry.Value.Item5,
                        entry.Value.Item6);

                    comboBoxCondition1_0.Enabled = true;
                    comboBoxCondition2_0.Enabled = true;
                    comboBoxCondition3_0.Enabled = true;
                    comboBoxStudyCondition_0.Enabled = true;
                }
                countCondition++;
            }

            Core.DictMaps = _DictMaps;
        }

        private void CreateNewMap(bool isNewData = true,
            int _indexcb1 = 0,
            int _indexcb2 = 0,
            int _indexcb3 = 0,
            int _indexcbStudy = 0,
            List<string> AllItemsCondition1 = null,
            List<string> AllItemsCondition2 = null,
            List<string> AllItemsCondition3 = null,
            List<string> AllItemsStudyCondition = null,
            List<string> SelectedItemsCondition1 = null,
            List<string> SelectedItemsCondition2 = null,
            List<string> SelectedItemsCondition3 = null,
            List<string> SelectedItemsStudyCondition = null,
            bool IsGoldenComplementayPairs = false,
            bool IsBondCleavageConfidence = false)
        {
            //Instantiate Controls;
            GroupBox groupBoxMainMap = new GroupBox();

            //comboboxes
            ComboBox cbFixedCondition1 = new ComboBox();
            ComboBox cbFixedCondition2 = new ComboBox();
            ComboBox cbFixedCondition3 = new ComboBox();
            ComboBox cbStudyCondition = new ComboBox();

            #region GroupBox Fixed Condition
            GroupBox groupBoxFixedCondition = new GroupBox();
            groupBoxFixedCondition.Name = "groupBoxFixedCondition" + numberOfConditions;
            groupBoxFixedCondition.Location = new Point(9, 19);
            groupBoxFixedCondition.Size = new Size(836, 171);
            groupBoxFixedCondition.MinimumSize = new Size(10, 171);
            groupBoxFixedCondition.AutoSizeMode = AutoSizeMode.GrowOnly;
            groupBoxFixedCondition.AutoSize = false;
            groupBoxFixedCondition.Text = "Fixed Condition";

            #region Condition1

            #region listbox Fixed condition 1
            ListBox listboxFixedCondition1 = new ListBox();
            listboxFixedCondition1.Name = "listBoxAllFixedCondition1_" + numberOfConditions;
            listboxFixedCondition1.Location = new Point(17, 50);
            listboxFixedCondition1.Size = new Size(120, 108);
            listboxFixedCondition1.SelectionMode = SelectionMode.MultiExtended;
            groupBoxFixedCondition.Controls.Add(listboxFixedCondition1);
            #endregion

            #region label Selected fixed condition1
            Label labelSelectedFixedCondition1 = new Label();
            labelSelectedFixedCondition1.Name = "labelSelectedFixedCondition1_" + numberOfConditions;
            labelSelectedFixedCondition1.Location = new Point(186, 34);
            labelSelectedFixedCondition1.Size = new Size(80, 13);
            labelSelectedFixedCondition1.Text = "Selected Items:";
            groupBoxFixedCondition.Controls.Add(labelSelectedFixedCondition1);
            #endregion

            #region listbox Selected fixed condition 1
            ListBox listboxSelectedFixedCondition1 = new ListBox();
            listboxSelectedFixedCondition1.Name = "listBoxSelectedFixedCondition1_" + numberOfConditions;
            listboxSelectedFixedCondition1.Location = new Point(189, 50);
            listboxSelectedFixedCondition1.Size = new Size(77, 108);
            listboxSelectedFixedCondition1.SelectionMode = SelectionMode.MultiExtended;
            groupBoxFixedCondition.Controls.Add(listboxSelectedFixedCondition1);
            #endregion

            #region combobox Fixed condition 1
            cbFixedCondition1.Name = "comboBoxCondition1_" + numberOfConditions;
            cbFixedCondition1.Location = new Point(17, 23);
            cbFixedCondition1.Size = new Size(120, 21);
            cbFixedCondition1.Text = "Select one option...";
            cbFixedCondition1.Items.AddRange(new object[] { "Fragmentation Method", "Activation Level", "Replicates", "Precursor Charge State" });
            cbFixedCondition1.SelectedIndexChanged += new System.EventHandler(this.comboBoxCondition_SelectedIndexChanged);
            cbFixedCondition1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxCondition_KeyPress);
            groupBoxFixedCondition.Controls.Add(cbFixedCondition1);
            #endregion

            #region button Add and remove selected fixed condition 1
            Button btn_add_fixed_condition1_right = new Button();
            Button btn_remove_fixed_condition1_left = new Button();

            btn_add_fixed_condition1_right.Location = new Point(152, 74);
            btn_add_fixed_condition1_right.Size = new Size(21, 23);
            btn_add_fixed_condition1_right.Name = "buttonAddFixedCondition1_" + numberOfConditions;
            btn_add_fixed_condition1_right.Image = Properties.Resources.arrow_right;
            btn_add_fixed_condition1_right.UseVisualStyleBackColor = true;
            btn_add_fixed_condition1_right.TabIndex = 1;

            btn_add_fixed_condition1_right.Click += new System.EventHandler(this.buttonAddSelectedCondition_Click);

            btn_remove_fixed_condition1_left.Location = new Point(152, 112);
            btn_remove_fixed_condition1_left.Size = new Size(21, 23);
            btn_remove_fixed_condition1_left.Name = "buttonRemoveFixedCondition1_" + numberOfConditions;
            btn_remove_fixed_condition1_left.Image = Properties.Resources.arrow_left;
            btn_remove_fixed_condition1_left.UseVisualStyleBackColor = true;
            btn_remove_fixed_condition1_left.TabIndex = 2;
            btn_remove_fixed_condition1_left.Click += new System.EventHandler(this.buttonRemoveSelectedCondition_Click);

            groupBoxFixedCondition.Controls.Add(btn_add_fixed_condition1_right);
            groupBoxFixedCondition.Controls.Add(btn_remove_fixed_condition1_left);

            #endregion

            #endregion

            #region Condition2

            #region listbox Fixed condition 2
            ListBox listboxFixedCondition2 = new ListBox();
            listboxFixedCondition2.Name = "listBoxAllFixedCondition2_" + numberOfConditions;
            listboxFixedCondition2.Location = new Point(295, 50);
            listboxFixedCondition2.Size = new Size(120, 108);
            listboxFixedCondition2.SelectionMode = SelectionMode.MultiExtended;
            groupBoxFixedCondition.Controls.Add(listboxFixedCondition2);
            #endregion

            #region label Selected fixed condition2
            Label labelSelectedFixedCondition2 = new Label();
            labelSelectedFixedCondition2.Name = "labelSelectedFixedCondition2_" + numberOfConditions;
            labelSelectedFixedCondition2.Location = new Point(464, 34);
            labelSelectedFixedCondition2.Size = new Size(80, 13);
            labelSelectedFixedCondition2.Text = "Selected Items:";
            groupBoxFixedCondition.Controls.Add(labelSelectedFixedCondition2);
            #endregion

            #region listbox Selected fixed condition 2
            ListBox listboxSelectedFixedCondition2 = new ListBox();
            listboxSelectedFixedCondition2.Name = "listBoxSelectedFixedCondition2_" + numberOfConditions;
            listboxSelectedFixedCondition2.Location = new Point(467, 50);
            listboxSelectedFixedCondition2.Size = new Size(77, 108);
            listboxSelectedFixedCondition2.SelectionMode = SelectionMode.MultiExtended;
            groupBoxFixedCondition.Controls.Add(listboxSelectedFixedCondition2);
            #endregion

            #region combobox Fixed condition 2
            cbFixedCondition2.Name = "comboBoxCondition2_" + numberOfConditions;
            cbFixedCondition2.Location = new Point(295, 23);
            cbFixedCondition2.Size = new Size(120, 21);
            cbFixedCondition2.Text = "Select one option...";
            cbFixedCondition2.Items.AddRange(new object[] { "Fragmentation Method", "Activation Level", "Replicates", "Precursor Charge State" });
            cbFixedCondition2.SelectedIndexChanged += new System.EventHandler(this.comboBoxCondition_SelectedIndexChanged);
            cbFixedCondition2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxCondition_KeyPress);
            groupBoxFixedCondition.Controls.Add(cbFixedCondition2);
            #endregion

            #region button Add and remove selected fixed condition 2
            Button btn_add_fixed_condition2_right = new Button();
            Button btn_remove_fixed_condition2_left = new Button();

            btn_add_fixed_condition2_right.Location = new Point(431, 74);
            btn_add_fixed_condition2_right.Size = new Size(21, 23);
            btn_add_fixed_condition2_right.Name = "buttonAddFixedCondition2_" + numberOfConditions;
            btn_add_fixed_condition2_right.Image = Properties.Resources.arrow_right;
            btn_add_fixed_condition2_right.UseVisualStyleBackColor = true;
            btn_add_fixed_condition2_right.TabIndex = 1;
            btn_add_fixed_condition2_right.Click += new System.EventHandler(this.buttonAddSelectedCondition_Click);

            btn_remove_fixed_condition2_left.Location = new Point(431, 112);
            btn_remove_fixed_condition2_left.Size = new Size(21, 23);
            btn_remove_fixed_condition2_left.Name = "buttonRemoveFixedCondition2_" + numberOfConditions;
            btn_remove_fixed_condition2_left.Image = Properties.Resources.arrow_left;
            btn_remove_fixed_condition2_left.UseVisualStyleBackColor = true;
            btn_remove_fixed_condition2_left.TabIndex = 2;
            btn_remove_fixed_condition2_left.Click += new System.EventHandler(this.buttonRemoveSelectedCondition_Click);

            groupBoxFixedCondition.Controls.Add(btn_add_fixed_condition2_right);
            groupBoxFixedCondition.Controls.Add(btn_remove_fixed_condition2_left);

            #endregion

            #endregion

            #region Condition3

            #region listbox Fixed condition 3
            ListBox listboxFixedCondition3 = new ListBox();
            listboxFixedCondition3.Name = "listBoxAllFixedCondition3_" + numberOfConditions;
            listboxFixedCondition3.Location = new Point(576, 50);
            listboxFixedCondition3.Size = new Size(120, 108);
            listboxFixedCondition3.SelectionMode = SelectionMode.MultiExtended;
            groupBoxFixedCondition.Controls.Add(listboxFixedCondition3);
            #endregion

            #region label Selected fixed condition3
            Label labelSelectedFixedCondition3 = new Label();
            labelSelectedFixedCondition3.Name = "labelSelectedFixedCondition3_" + numberOfConditions;
            labelSelectedFixedCondition3.Location = new Point(745, 34);
            labelSelectedFixedCondition3.Size = new Size(80, 13);
            labelSelectedFixedCondition3.Text = "Selected Items:";
            groupBoxFixedCondition.Controls.Add(labelSelectedFixedCondition3);
            #endregion

            #region listbox Selected fixed condition 3
            ListBox listboxSelectedFixedCondition3 = new ListBox();
            listboxSelectedFixedCondition3.Name = "listBoxSelectedFixedCondition3_" + numberOfConditions;
            listboxSelectedFixedCondition3.Location = new Point(748, 50);
            listboxSelectedFixedCondition3.Size = new Size(77, 108);
            listboxSelectedFixedCondition3.SelectionMode = SelectionMode.MultiExtended;
            groupBoxFixedCondition.Controls.Add(listboxSelectedFixedCondition3);
            #endregion

            #region combobox Fixed condition 3
            cbFixedCondition3.Name = "comboBoxCondition3_" + numberOfConditions;
            cbFixedCondition3.Location = new Point(576, 23);
            cbFixedCondition3.Size = new Size(120, 21);
            cbFixedCondition3.Text = "Select one option...";
            cbFixedCondition3.Items.AddRange(new object[] { "Fragmentation Method", "Activation Level", "Replicates", "Precursor Charge State" });
            cbFixedCondition3.SelectedIndexChanged += new System.EventHandler(this.comboBoxCondition_SelectedIndexChanged);
            cbFixedCondition3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxCondition_KeyPress);
            groupBoxFixedCondition.Controls.Add(cbFixedCondition3);
            #endregion

            #region button Add and remove selected fixed condition 3
            Button btn_add_fixed_condition3_right = new Button();
            Button btn_remove_fixed_condition3_left = new Button();

            btn_add_fixed_condition3_right.Location = new Point(712, 74);
            btn_add_fixed_condition3_right.Size = new Size(21, 23);
            btn_add_fixed_condition3_right.Name = "buttonAddFixedCondition3_" + numberOfConditions;
            btn_add_fixed_condition3_right.Image = Properties.Resources.arrow_right;
            btn_add_fixed_condition3_right.UseVisualStyleBackColor = true;
            btn_add_fixed_condition3_right.TabIndex = 1;
            btn_add_fixed_condition3_right.Click += new System.EventHandler(this.buttonAddSelectedCondition_Click);

            btn_remove_fixed_condition3_left.Location = new Point(712, 112);
            btn_remove_fixed_condition3_left.Size = new Size(21, 23);
            btn_remove_fixed_condition3_left.Name = "buttonRemoveFixedCondition3_" + numberOfConditions;
            btn_remove_fixed_condition3_left.Image = Properties.Resources.arrow_left;
            btn_remove_fixed_condition3_left.UseVisualStyleBackColor = true;
            btn_remove_fixed_condition3_left.TabIndex = 2;
            btn_remove_fixed_condition3_left.Click += new System.EventHandler(this.buttonRemoveSelectedCondition_Click);

            groupBoxFixedCondition.Controls.Add(btn_add_fixed_condition3_right);
            groupBoxFixedCondition.Controls.Add(btn_remove_fixed_condition3_left);

            #endregion

            #endregion

            groupBoxMainMap.Controls.Add(groupBoxFixedCondition);

            #endregion

            #region GroupBox Study Condition
            GroupBox groupBoxStudyCondition = new GroupBox();
            groupBoxStudyCondition.Name = "groupBoxStudyCondition_" + numberOfConditions;
            groupBoxStudyCondition.Location = new Point(860, 19);
            groupBoxStudyCondition.Size = new Size(285, 171);
            groupBoxStudyCondition.MinimumSize = new Size(285, 171);
            groupBoxStudyCondition.AutoSizeMode = AutoSizeMode.GrowOnly;
            groupBoxStudyCondition.AutoSize = false;
            groupBoxStudyCondition.Text = "Study Condition";

            #region Study Condition

            #region listbox Study condition
            ListBox listboxStudyCondition = new ListBox();
            listboxStudyCondition.Name = "listBoxAllStudyCondition" + numberOfConditions;
            listboxStudyCondition.Location = new Point(17, 50);
            listboxStudyCondition.Size = new Size(120, 108);
            listboxStudyCondition.SelectionMode = SelectionMode.MultiExtended;
            groupBoxStudyCondition.Controls.Add(listboxStudyCondition);
            #endregion

            #region label Selected Study condition
            Label labelSelectedStudyCondition = new Label();
            labelSelectedStudyCondition.Name = "labelSelectedStudyCondition" + numberOfConditions;
            labelSelectedStudyCondition.Location = new Point(186, 34);
            labelSelectedStudyCondition.Size = new Size(80, 13);
            labelSelectedStudyCondition.Text = "Selected Items:";
            groupBoxStudyCondition.Controls.Add(labelSelectedStudyCondition);
            #endregion

            #region listbox Selected Study condition
            ListBox listboxSelectedStudyCondition = new ListBox();
            listboxSelectedStudyCondition.Name = "listBoxSelectedStudyCondition" + numberOfConditions;
            listboxSelectedStudyCondition.Location = new Point(189, 50);
            listboxSelectedStudyCondition.Size = new Size(77, 108);
            listboxSelectedStudyCondition.SelectionMode = SelectionMode.MultiExtended;
            groupBoxStudyCondition.Controls.Add(listboxSelectedStudyCondition);
            #endregion

            #region combobox Study condition
            cbStudyCondition.Name = "comboBoxStudyCondition" + numberOfConditions;
            cbStudyCondition.Location = new Point(17, 23);
            cbStudyCondition.Size = new Size(120, 21);
            cbStudyCondition.Text = "Select one option...";
            cbStudyCondition.Items.AddRange(new object[] { "Fragmentation Method", "Activation Level", "Replicates", "Precursor Charge State" });
            cbStudyCondition.SelectedIndexChanged += new System.EventHandler(this.comboBoxCondition_SelectedIndexChanged);
            cbStudyCondition.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxCondition_KeyPress);
            groupBoxStudyCondition.Controls.Add(cbStudyCondition);
            #endregion

            #region button Add and remove selected Study condition
            Button btn_add_study_condition_right = new Button();
            Button btn_remove_study_condition_left = new Button();

            btn_add_study_condition_right.Location = new Point(152, 74);
            btn_add_study_condition_right.Size = new Size(21, 23);
            btn_add_study_condition_right.Name = "buttonAddStudyCondition" + numberOfConditions;
            btn_add_study_condition_right.Image = Properties.Resources.arrow_right;
            btn_add_study_condition_right.UseVisualStyleBackColor = true;
            btn_add_study_condition_right.TabIndex = 1;
            btn_add_study_condition_right.Click += new System.EventHandler(this.buttonAddSelectedCondition_Click);

            btn_remove_study_condition_left.Location = new Point(152, 112);
            btn_remove_study_condition_left.Size = new Size(21, 23);
            btn_remove_study_condition_left.Name = "buttonRemoveStudyCondition" + numberOfConditions;
            btn_remove_study_condition_left.Image = Properties.Resources.arrow_left;
            btn_remove_study_condition_left.UseVisualStyleBackColor = true;
            btn_remove_study_condition_left.TabIndex = 2;
            btn_remove_study_condition_left.Click += new System.EventHandler(this.buttonRemoveSelectedCondition_Click);

            groupBoxStudyCondition.Controls.Add(btn_add_study_condition_right);
            groupBoxStudyCondition.Controls.Add(btn_remove_study_condition_left);

            #endregion

            #endregion

            groupBoxMainMap.Controls.Add(groupBoxStudyCondition);
            #endregion

            #region CheckBox options
            CheckBox checkBoxGoldenComplemPairs = new CheckBox();
            checkBoxGoldenComplemPairs.Name = "checkBoxGoldenComplemPairs" + numberOfConditions;
            checkBoxGoldenComplemPairs.Text = "Add golden complementary pairs";
            checkBoxGoldenComplemPairs.TextAlign = ContentAlignment.MiddleCenter;
            checkBoxGoldenComplemPairs.AutoSize = true;
            checkBoxGoldenComplemPairs.Location = new Point(860, 196);
            checkBoxGoldenComplemPairs.CheckedChanged += new System.EventHandler(this.checkBoxGoldenComplemPairs_CheckedChanged);

            CheckBox checkBoxCleavageConfidence = new CheckBox();
            checkBoxCleavageConfidence.Name = "checkBoxCleavageConfidence" + numberOfConditions;
            checkBoxCleavageConfidence.Text = "Add bond cleavage confidence";
            checkBoxCleavageConfidence.TextAlign = ContentAlignment.MiddleCenter;
            checkBoxCleavageConfidence.AutoSize = true;
            checkBoxCleavageConfidence.Location = new Point(860, 219);
            checkBoxCleavageConfidence.CheckedChanged += new System.EventHandler(this.checkBoxCleavageConfidence_CheckedChanged);


            groupBoxMainMap.Controls.Add(checkBoxGoldenComplemPairs);
            groupBoxMainMap.Controls.Add(checkBoxCleavageConfidence);
            #endregion

            #region button Add and remove map
            Button btn_add_map = new Button();
            Button btn_remove_map = new Button();

            groupBoxMainMap.Controls.Add(btn_add_map);
            groupBoxMainMap.Controls.Add(btn_remove_map);

            btn_add_map.Location = new Point(9, 213);
            btn_add_map.Size = new Size(137, 23);
            btn_add_map.Name = "buttonAddMap_" + numberOfConditions;
            btn_add_map.Image = Properties.Resources.addButton;
            btn_add_map.ImageAlign = ContentAlignment.MiddleLeft;
            btn_add_map.Text = "Add another map";
            btn_add_map.TextAlign = ContentAlignment.MiddleCenter;
            btn_add_map.UseVisualStyleBackColor = true;
            btn_add_map.TabIndex = 3;
            btn_add_map.Enabled = true;
            btn_add_map.Click += new System.EventHandler(this.buttonAddMap_Click);

            btn_remove_map.Location = new Point(160, 213);
            btn_remove_map.Size = new Size(115, 23);
            btn_remove_map.Name = "buttonRemoveMap_" + numberOfConditions;
            btn_remove_map.Image = Properties.Resources.button_cancel_little;
            btn_remove_map.ImageAlign = ContentAlignment.MiddleLeft;
            btn_remove_map.Text = "Remove map";
            btn_remove_map.TextAlign = ContentAlignment.MiddleCenter;
            btn_remove_map.UseVisualStyleBackColor = true;
            btn_remove_map.TabIndex = 4;
            btn_remove_map.Tag = new object[] { groupBoxMainMap, numberOfConditions };
            btn_remove_map.Click += new System.EventHandler(this.buttonRemoveMap_Click);

            if (Add_Remove_MapBtnList != null)
                Add_Remove_MapBtnList.Add((btn_add_map, btn_remove_map));
            #endregion

            #region set Tags - fields
            btn_add_fixed_condition1_right.Tag = new object[] { cbFixedCondition1 };
            btn_add_fixed_condition2_right.Tag = new object[] { cbFixedCondition1 };
            btn_add_fixed_condition3_right.Tag = new object[] { cbFixedCondition1 };
            btn_add_study_condition_right.Tag = new object[] { cbFixedCondition1 };

            btn_remove_fixed_condition1_left.Tag = new object[] { cbFixedCondition1 };
            btn_remove_fixed_condition2_left.Tag = new object[] { cbFixedCondition1 };
            btn_remove_fixed_condition3_left.Tag = new object[] { cbFixedCondition1 };
            btn_remove_study_condition_left.Tag = new object[] { cbFixedCondition1 };

            cbFixedCondition1.Tag = new object[] {
                cbFixedCondition1,
                cbFixedCondition2,
                cbFixedCondition3,
                cbStudyCondition,
                listboxFixedCondition1,
                listboxFixedCondition2,
                listboxFixedCondition3,
                listboxStudyCondition,
                listboxSelectedFixedCondition1,
                listboxSelectedFixedCondition2,
                listboxSelectedFixedCondition3,
                listboxSelectedStudyCondition,
                btn_add_map,
                btn_add_fixed_condition1_right,
                allFragmentIonsAllConditions,
                checkBoxGoldenComplemPairs,
                checkBoxCleavageConfidence
            };
            cbFixedCondition2.Tag = new object[] { cbFixedCondition1 };
            cbFixedCondition3.Tag = new object[] { cbFixedCondition1 };
            cbStudyCondition.Tag = new object[] { cbFixedCondition1 };

            if (!isNewData)
            {
                cbFixedCondition1.SelectedIndex = _indexcb1;
                cbFixedCondition2.SelectedIndex = _indexcb2;
                cbFixedCondition3.SelectedIndex = _indexcb3;
                cbStudyCondition.SelectedIndex = _indexcbStudy;
                cbFixedCondition1.Enabled = true;
                cbFixedCondition2.Enabled = true;
                cbFixedCondition3.Enabled = true;
                cbStudyCondition.Enabled = true;

                listboxFixedCondition1.Items.Clear();
                listboxFixedCondition1.Items.AddRange(AllItemsCondition1.ToArray());
                listboxFixedCondition2.Items.Clear();
                listboxFixedCondition2.Items.AddRange(AllItemsCondition2.ToArray());
                listboxFixedCondition3.Items.Clear();
                listboxFixedCondition3.Items.AddRange(AllItemsCondition3.ToArray());
                listboxStudyCondition.Items.Clear();
                listboxStudyCondition.Items.AddRange(AllItemsStudyCondition.ToArray());

                listboxSelectedFixedCondition1.Items.AddRange(SelectedItemsCondition1.ToArray());
                listboxSelectedFixedCondition2.Items.AddRange(SelectedItemsCondition2.ToArray());
                listboxSelectedFixedCondition3.Items.AddRange(SelectedItemsCondition3.ToArray());
                listboxSelectedStudyCondition.Items.AddRange(SelectedItemsStudyCondition.ToArray());

                #region Enabling checkbox
                if (IsBondCleavageConfidence)
                    checkBoxCleavageConfidence.CheckState = CheckState.Checked;
                if (IsGoldenComplementayPairs)
                {
                    checkBoxGoldenComplemPairs.Enabled = true;
                    checkBoxGoldenComplemPairs.CheckState = CheckState.Checked;
                }
                if (listboxSelectedFixedCondition1.Items.Count == 1 &&
                        listboxSelectedFixedCondition2.Items.Count == 1 &&
                        listboxSelectedFixedCondition3.Items.Count == 1)
                    checkBoxGoldenComplemPairs.Enabled = true;
                #endregion

            }

            #endregion


            groupBoxMainMap.Name = "groupBoxMap_" + numberOfConditions;
            groupBoxMainMap.Location = new Point(3, numberOfConditions * SPACE_Y);
            groupBoxMainMap.Size = new Size(1160, 248);
            groupBoxMainMap.MinimumSize = new Size(1160, 248);
            groupBoxMainMap.AutoSizeMode = AutoSizeMode.GrowOnly;
            groupBoxMainMap.AutoSize = false;
            groupBoxMainMap.Text = "Map " + (numberOfConditions + 1);
            groupBoxMainMap.Anchor = (AnchorStyles.Top | AnchorStyles.Left);

            this.Controls.Add(groupBoxMainMap);
            this.Height += 53;
            numberOfConditions++;
            btn_remove_map.Focus();
        }

        private void ResetFields(ListBox listBoxAllFixedCondition1,
            ListBox listBoxAllFixedCondition2,
            ListBox listBoxAllFixedCondition3,
            ListBox listBoxSelectedFixedCondition1,
            ListBox listBoxSelectedFixedCondition2,
            ListBox listBoxSelectedFixedCondition3,
            ListBox listBoxAllStudyCondition,
            ListBox listBoxSelectedStudyCondition,
            ComboBox comboBoxCondition1,
            ComboBox comboBoxCondition2,
            ComboBox comboBoxCondition3,
            ComboBox comboBoxStudyCondition)
        {
            if (listBoxAllFixedCondition1 != null)
                listBoxAllFixedCondition1.Items.Clear();
            if (listBoxSelectedFixedCondition1 != null)
                listBoxSelectedFixedCondition1.Items.Clear();
            if (listBoxAllFixedCondition2 != null)
                listBoxAllFixedCondition2.Items.Clear();
            if (listBoxSelectedFixedCondition2 != null)
                listBoxSelectedFixedCondition2.Items.Clear();
            if (listBoxAllFixedCondition3 != null)
                listBoxAllFixedCondition3.Items.Clear();
            if (listBoxSelectedFixedCondition3 != null)
                listBoxSelectedFixedCondition3.Items.Clear();
            if (listBoxAllStudyCondition != null)
                listBoxAllStudyCondition.Items.Clear();
            if (listBoxSelectedStudyCondition != null)
                listBoxSelectedStudyCondition.Items.Clear();

            if (comboBoxCondition1 != null && comboBoxCondition1.SelectedItem != null)
            {
                try
                {
                    BeginInvoke(new Action(() => comboBoxCondition1.SelectedItem = null));
                    BeginInvoke(new Action(() => comboBoxCondition1.Text = "Select one option..."));
                }
                catch (Exception)
                {
                    comboBoxCondition1.SelectedItem = null;
                    comboBoxCondition1.Text = "Select one option...";
                }
            }

            if (comboBoxCondition2 != null && comboBoxCondition2.SelectedItem != null)
            {
                try
                {
                    BeginInvoke(new Action(() => comboBoxCondition2.SelectedItem = null));
                    BeginInvoke(new Action(() => comboBoxCondition2.Text = "Select one option..."));
                }
                catch (Exception)
                {
                    comboBoxCondition2.SelectedItem = null;
                    comboBoxCondition2.Text = "Select one option...";
                }
            }

            if (comboBoxCondition3 != null && comboBoxCondition3.SelectedItem != null)
            {
                try
                {
                    BeginInvoke(new Action(() => comboBoxCondition3.SelectedItem = null));
                    BeginInvoke(new Action(() => comboBoxCondition3.Text = "Select one option..."));
                }
                catch (Exception)
                {
                    comboBoxCondition3.SelectedItem = null;
                    comboBoxCondition3.Text = "Select one option...";
                }
            }

            if (comboBoxStudyCondition != null && comboBoxStudyCondition.SelectedItem != null)
            {
                try
                {
                    BeginInvoke(new Action(() => comboBoxStudyCondition.SelectedItem = null));
                    BeginInvoke(new Action(() => comboBoxStudyCondition.Text = "Select one option..."));
                }
                catch (Exception)
                {
                    comboBoxStudyCondition.SelectedItem = null;
                    comboBoxStudyCondition.Text = "Select one option...";
                }
            }
        }

        private void comboBoxCondition_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBoxStudyCondition_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBoxCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem == null) return;

            ComboBox comboBoxCondition1 = null;
            ComboBox comboBoxCondition2 = null;
            ComboBox comboBoxCondition3 = null;
            ComboBox comboBoxStudyCondition = null;
            ListBox listBoxAllFixedCondition1 = null;
            ListBox listBoxAllFixedCondition2 = null;
            ListBox listBoxAllFixedCondition3 = null;
            ListBox listBoxAllStudyCondition = null;
            ListBox listBoxSelectedFixedCondition1 = null;
            ListBox listBoxSelectedFixedCondition2 = null;
            ListBox listBoxSelectedFixedCondition3 = null;
            ListBox listBoxSelectedStudyCondition = null;
            Button addNewMap = null;
            Button addSelectedCondition = null;
            List<(string, int, string, int, string, int, double, string)> allFragmentIonsAllConditions = null;
            CheckBox checkBoxGoldenComplemPairs = null;
            CheckBox checkBoxCleavageConfidence = null;

            if (((ComboBox)sender).Name.StartsWith("comboBoxCondition1"))
            {
                comboBoxCondition1 = (ComboBox)((object[])((ComboBox)sender).Tag)[0];
                comboBoxCondition2 = (ComboBox)((object[])((ComboBox)sender).Tag)[1];
                comboBoxCondition3 = (ComboBox)((object[])((ComboBox)sender).Tag)[2];
                comboBoxStudyCondition = (ComboBox)((object[])((ComboBox)sender).Tag)[3];
                listBoxAllFixedCondition1 = (ListBox)((object[])((ComboBox)sender).Tag)[4];
                listBoxAllFixedCondition2 = (ListBox)((object[])((ComboBox)sender).Tag)[5];
                listBoxAllFixedCondition3 = (ListBox)((object[])((ComboBox)sender).Tag)[6];
                listBoxAllStudyCondition = (ListBox)((object[])((ComboBox)sender).Tag)[7];
                listBoxSelectedFixedCondition1 = (ListBox)((object[])((ComboBox)sender).Tag)[8];
                listBoxSelectedFixedCondition2 = (ListBox)((object[])((ComboBox)sender).Tag)[9];
                listBoxSelectedFixedCondition3 = (ListBox)((object[])((ComboBox)sender).Tag)[10];
                listBoxSelectedStudyCondition = (ListBox)((object[])((ComboBox)sender).Tag)[11];
                addNewMap = (Button)((object[])((ComboBox)sender).Tag)[12];
                addSelectedCondition = (Button)((object[])((ComboBox)sender).Tag)[13];
                allFragmentIonsAllConditions = (List<(string, int, string, int, string, int, double, string)>)((object[])((ComboBox)sender).Tag)[14];
                checkBoxGoldenComplemPairs = (CheckBox)((object[])((ComboBox)sender).Tag)[15];
                checkBoxCleavageConfidence = (CheckBox)((object[])((ComboBox)sender).Tag)[16];
            }
            else
            {
                comboBoxCondition1 = (ComboBox)((object[])((ComboBox)sender).Tag)[0];
                comboBoxCondition2 = (ComboBox)((object[])((ComboBox)comboBoxCondition1).Tag)[1];
                comboBoxCondition3 = (ComboBox)((object[])((ComboBox)comboBoxCondition1).Tag)[2];
                comboBoxStudyCondition = (ComboBox)((object[])((ComboBox)comboBoxCondition1).Tag)[3];
                listBoxAllFixedCondition1 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[4];
                listBoxAllFixedCondition2 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[5];
                listBoxAllFixedCondition3 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[6];
                listBoxAllStudyCondition = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[7];
                listBoxSelectedFixedCondition1 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[8];
                listBoxSelectedFixedCondition2 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[9];
                listBoxSelectedFixedCondition3 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[10];
                listBoxSelectedStudyCondition = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[11];
                addNewMap = (Button)((object[])((ComboBox)comboBoxCondition1).Tag)[12];
                addSelectedCondition = (Button)((object[])((ComboBox)comboBoxCondition1).Tag)[13];
                allFragmentIonsAllConditions = (List<(string, int, string, int, string, int, double, string)>)((object[])((ComboBox)comboBoxCondition1).Tag)[14];
                checkBoxGoldenComplemPairs = (CheckBox)((object[])((ComboBox)comboBoxCondition1).Tag)[15];
                checkBoxCleavageConfidence = (CheckBox)((object[])((ComboBox)comboBoxCondition1).Tag)[16];
            }


            ListBox currentListBoxAllConditions = null;
            ListBox currentListBoxSelectedCondition = null;

            //Check which cb was selected
            if (((ComboBox)sender).Name.StartsWith("comboBoxCondition1"))
            {
                currentListBoxAllConditions = listBoxAllFixedCondition1;
                currentListBoxSelectedCondition = listBoxSelectedFixedCondition1;
            }
            else if (((ComboBox)sender).Name.StartsWith("comboBoxCondition2"))
            {
                currentListBoxAllConditions = listBoxAllFixedCondition2;
                currentListBoxSelectedCondition = listBoxSelectedFixedCondition2;
            }
            else if (((ComboBox)sender).Name.StartsWith("comboBoxCondition3"))
            {
                currentListBoxAllConditions = listBoxAllFixedCondition3;
                currentListBoxSelectedCondition = listBoxSelectedFixedCondition3;
            }
            else
            {
                currentListBoxAllConditions = listBoxAllStudyCondition;
                currentListBoxSelectedCondition = listBoxSelectedStudyCondition;
            }

            if (comboBoxCondition1.SelectedItem == null)
            {
                comboBoxCondition1.Focus();
                return;
            }

            if (comboBoxCondition2.SelectedItem == null && ((ComboBox)sender).Name.StartsWith("comboBoxCondition3"))
            {
                comboBoxCondition2.Focus();
                return;
            }

            if (comboBoxCondition3.SelectedItem == null && ((ComboBox)sender).Name.StartsWith("comboBoxStudyCondition"))
            {
                comboBoxCondition3.Focus();
                return;
            }

            string condition = ((ComboBox)sender).SelectedItem.ToString();

            if (((ComboBox)sender).Name.StartsWith("comboBoxCondition1"))
            {
                comboBoxCondition2.Enabled = false;
                comboBoxCondition3.Enabled = false;
                comboBoxStudyCondition.Enabled = false;
                checkBoxGoldenComplemPairs.Checked = false;
                checkBoxGoldenComplemPairs.Enabled = false;
                checkBoxCleavageConfidence.Checked = false;

                #region Reset all fields
                ResetFields(listBoxAllFixedCondition1,
                listBoxAllFixedCondition2,
                listBoxAllFixedCondition3,
                listBoxSelectedFixedCondition1,
                listBoxSelectedFixedCondition2,
                listBoxSelectedFixedCondition3,
                listBoxAllStudyCondition,
                listBoxSelectedStudyCondition,
                null,
                comboBoxCondition2,
                comboBoxCondition3,
                comboBoxStudyCondition);
                #endregion


                if (condition.StartsWith("Frag"))
                {
                    currentListBoxAllConditions.SelectionMode = SelectionMode.One;
                }
                else
                    currentListBoxAllConditions.SelectionMode = SelectionMode.MultiExtended;
            }


            bool isValid = true;
            if (!((ComboBox)sender).Name.StartsWith("comboBoxCondition1"))
            {
                isValid = CheckAvailabilityCondition(comboBoxCondition1, comboBoxCondition2, comboBoxCondition3, comboBoxStudyCondition, currentListBoxAllConditions, currentListBoxSelectedCondition, (ComboBox)sender);
            }

            if (isValid)
            {
                FillComboboxCondition(((ComboBox)sender), currentListBoxAllConditions, listBoxSelectedFixedCondition1, listBoxSelectedFixedCondition2, listBoxSelectedFixedCondition3, listBoxSelectedStudyCondition, allFragmentIonsAllConditions);

                if (listBoxSelectedFixedCondition1.Items.Count > 0 && comboBoxCondition1 != null && comboBoxCondition1.SelectedItem != null && comboBoxCondition1.SelectedItem.ToString().StartsWith("Frag"))
                    addSelectedCondition.Enabled = false;
                else
                    addSelectedCondition.Enabled = true;

                //Set tag to SelectedListBox to know the kind of data
                currentListBoxSelectedCondition.Tag = ((ComboBox)sender).SelectedItem.ToString();

                try
                {
                    List<string> _keys = Core.DictMaps.Keys.ToList();
                    if (_keys.Count > 0)
                    {
                        int numberOfCondition = Convert.ToInt32(Regex.Split(addNewMap.Name, "_")[1]);
                        string _key = _keys.Where(a => a.EndsWith("#" + numberOfCondition)).FirstOrDefault();
                        Core.DictMaps.Remove(_key);
                    }
                }
                catch (Exception) { }
            }
        }

        private bool EnableAddNewMapBtn(ComboBox comboBoxCondition1, ComboBox comboBoxCondition2, ComboBox comboBoxCondition3, ComboBox comboBoxStudyCondition)
        {
            string condition1 = "";
            string condition2 = "";
            string condition3 = "";
            string studyCondition = "";

            if (comboBoxCondition1.SelectedItem != null)
                condition1 = comboBoxCondition1.SelectedItem.ToString();
            if (comboBoxCondition2.SelectedItem != null)
                condition2 = comboBoxCondition2.SelectedItem.ToString();
            if (comboBoxCondition3.SelectedItem != null)
                condition3 = comboBoxCondition3.SelectedItem.ToString();
            if (comboBoxStudyCondition.SelectedItem != null)
                studyCondition = comboBoxStudyCondition.SelectedItem.ToString();

            if (!String.IsNullOrEmpty(condition1) &&
                !String.IsNullOrEmpty(condition2) &&
                !String.IsNullOrEmpty(condition3) &&
                !String.IsNullOrEmpty(studyCondition) &&
                condition1.StartsWith("Frag"))
            {
                int numberOfCondition = Convert.ToInt32(Regex.Split(comboBoxCondition1.Name, "_")[1]);
                if ((numberOfCondition + 1) != numberOfConditions) return false;
                return true;
            }
            else
                return false;
        }

        private void FillComboboxCondition(ComboBox currentCombobox, ListBox listboxAllConditions, ListBox listboxSelectedCondition1, ListBox listboxSelectedCondition2, ListBox listboxSelectedCondition3, ListBox listboxSelectedStudyCondition, List<(string, int, string, int, string, int, double, string)> allFragmentIonsAllConditions)
        {
            string condition = currentCombobox.SelectedItem.ToString();
            listboxAllConditions.Items.Clear();

            if (Core == null ||
                Core.FragmentIons == null ||
                Core.FragmentIons.Count == 0) return;
            // List of Fragment Ions: FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate


            //Check if it's first combobox
            if (currentCombobox.Name.StartsWith("comboBoxCondition1"))
            {
                listboxSelectedCondition1.Items.Clear();

                allFragmentIonsAllConditions = Core.FragmentIons;

                if (condition.StartsWith("Frag"))
                {
                    foreach (string frag in allFragmentIonsAllConditions.Select(a => a.Item1).Distinct().ToList())
                        listboxAllConditions.Items.Add(frag);
                }
                else if (condition.StartsWith("Act"))
                {
                    foreach (string frag in allFragmentIonsAllConditions.Select(a => a.Item5).Distinct().ToList())
                        listboxAllConditions.Items.Add(frag);
                }
                else if (condition.StartsWith("Prec"))
                {
                    foreach (int precursor in allFragmentIonsAllConditions.Select(a => a.Item2).Distinct().OrderByDescending(a => a).ToList())
                        listboxAllConditions.Items.Add(precursor);
                }
                else if (condition.StartsWith("Repl"))
                {
                    foreach (int replicate in allFragmentIonsAllConditions.Select(a => a.Item6).Distinct().OrderBy(a => a).ToList())
                        listboxAllConditions.Items.Add("R" + replicate);
                }
            }
            else if (currentCombobox.Name.StartsWith("comboBoxCondition2"))
            {
                listboxSelectedCondition2.Items.Clear();
                List<(string, int, string, int, string, int, double, string)> _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                if (listboxSelectedCondition1.Tag.Equals("Fragmentation Method"))
                {
                    foreach (string item in listboxSelectedCondition1.Items)//Frag Method
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition1.Tag.Equals("Activation Level"))
                {
                    foreach (string item in listboxSelectedCondition1.Items)//Activation Level
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition1.Tag.Equals("Replicates"))
                {
                    try
                    {
                        foreach (int item in listboxSelectedCondition1.Items)//Replicates
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                        }
                    }
                    catch (Exception)
                    {
                        foreach (string item in listboxSelectedCondition1.Items)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                    }
                }
                else if (listboxSelectedCondition1.Tag.Equals("Precursor Charge State"))
                {
                    foreach (int item in listboxSelectedCondition1.Items)//Precursor Charge State
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                    }
                }
                allFragmentIonsAllConditions = _tempFragMethods;

                if (condition.StartsWith("Frag"))
                {
                    foreach (string frag in allFragmentIonsAllConditions.Select(a => a.Item1).Distinct().ToList())
                        listboxAllConditions.Items.Add(frag);
                }
                else if (condition.StartsWith("Act"))
                {
                    foreach (string frag in allFragmentIonsAllConditions.Select(a => a.Item5).Distinct().ToList())
                        listboxAllConditions.Items.Add(frag);
                }
                else if (condition.StartsWith("Prec"))
                {
                    foreach (int precursor in allFragmentIonsAllConditions.Select(a => a.Item2).Distinct().OrderByDescending(a => a).ToList())
                        listboxAllConditions.Items.Add(precursor);
                }
                else if (condition.StartsWith("Repl"))
                {
                    foreach (int replicate in allFragmentIonsAllConditions.Select(a => a.Item6).Distinct().OrderBy(a => a).ToList())
                        listboxAllConditions.Items.Add("R" + replicate);
                }
            }
            else if (currentCombobox.Name.StartsWith("comboBoxCondition3"))
            {
                listboxSelectedCondition3.Items.Clear();
                List<(string, int, string, int, string, int, double, string)> _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                #region filter 1
                if (listboxSelectedCondition1.Tag.Equals("Fragmentation Method"))
                {
                    foreach (string item in listboxSelectedCondition1.Items)//Frag Method
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition1.Tag.Equals("Activation Level"))
                {
                    foreach (string item in listboxSelectedCondition1.Items)//Activation Level
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition1.Tag.Equals("Replicates"))
                {
                    try
                    {
                        foreach (int item in listboxSelectedCondition1.Items)//Replicates
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                        }
                    }
                    catch (Exception)
                    {
                        foreach (string item in listboxSelectedCondition1.Items)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                    }
                }
                else if (listboxSelectedCondition1.Tag.Equals("Precursor Charge State"))
                {
                    foreach (int item in listboxSelectedCondition1.Items)//Precursor Charge State
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                    }
                }
                allFragmentIonsAllConditions = _tempFragMethods;
                #endregion

                #region filter 2
                _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                if (listboxSelectedCondition2.Tag.Equals("Fragmentation Method"))
                {
                    foreach (string item in listboxSelectedCondition2.Items)//Frag Method
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition2.Tag.Equals("Activation Level"))
                {
                    foreach (string item in listboxSelectedCondition2.Items)//Activation Level
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition2.Tag.Equals("Replicates"))
                {
                    try
                    {
                        foreach (int item in listboxSelectedCondition2.Items)//Replicates
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                        }
                    }
                    catch (Exception)
                    {
                        foreach (string item in listboxSelectedCondition2.Items)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                    }
                }
                else if (listboxSelectedCondition2.Tag.Equals("Precursor Charge State"))
                {
                    foreach (int item in listboxSelectedCondition2.Items)//Precursor Charge State
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                    }
                }
                allFragmentIonsAllConditions = _tempFragMethods;
                #endregion

                if (condition.StartsWith("Frag"))
                {
                    foreach (string frag in allFragmentIonsAllConditions.Select(a => a.Item1).Distinct().ToList())
                        listboxAllConditions.Items.Add(frag);
                }
                else if (condition.StartsWith("Act"))
                {
                    foreach (string frag in allFragmentIonsAllConditions.Select(a => a.Item5).Distinct().ToList())
                        listboxAllConditions.Items.Add(frag);
                }
                else if (condition.StartsWith("Prec"))
                {
                    foreach (int precursor in allFragmentIonsAllConditions.Select(a => a.Item2).Distinct().OrderByDescending(a => a).ToList())
                        listboxAllConditions.Items.Add(precursor);
                }
                else if (condition.StartsWith("Repl"))
                {
                    foreach (int replicate in allFragmentIonsAllConditions.Select(a => a.Item6).Distinct().OrderBy(a => a).ToList())
                        listboxAllConditions.Items.Add("R" + replicate);
                }
            }
            else if (currentCombobox.Name.StartsWith("comboBoxStudyCondition"))
            {
                listboxSelectedStudyCondition.Items.Clear();
                allFragmentIonsAllConditions = Core.FragmentIons;
                List<(string, int, string, int, string, int, double, string)> _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                #region filter 1
                if (listboxSelectedCondition1.Tag.Equals("Fragmentation Method"))
                {
                    foreach (string item in listboxSelectedCondition1.Items)//Frag Method
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition1.Tag.Equals("Activation Level"))
                {
                    foreach (string item in listboxSelectedCondition1.Items)//Activation Level
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition1.Tag.Equals("Replicates"))
                {
                    try
                    {
                        foreach (int item in listboxSelectedCondition1.Items)//Replicates
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                        }
                    }
                    catch (Exception)
                    {
                        foreach (string item in listboxSelectedCondition1.Items)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                    }
                }
                else if (listboxSelectedCondition1.Tag.Equals("Precursor Charge State"))
                {
                    try
                    {
                        foreach (int item in listboxSelectedCondition1.Items)//Precursor Charge State
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                        }
                    }
                    catch (Exception)
                    {
                        foreach (string item in listboxSelectedCondition1.Items)//Precursor Charge State
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                        }
                    }
                }
                allFragmentIonsAllConditions = _tempFragMethods;
                #endregion

                #region filter 2
                _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                if (listboxSelectedCondition2.Tag.Equals("Fragmentation Method"))
                {
                    foreach (string item in listboxSelectedCondition2.Items)//Frag Method
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition2.Tag.Equals("Activation Level"))
                {
                    foreach (string item in listboxSelectedCondition2.Items)//Activation Level
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition2.Tag.Equals("Replicates"))
                {
                    try
                    {
                        foreach (int item in listboxSelectedCondition2.Items)//Replicates
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                        }
                    }
                    catch (Exception)
                    {
                        foreach (string item in listboxSelectedCondition2.Items)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                    }
                }
                else if (listboxSelectedCondition2.Tag.Equals("Precursor Charge State"))
                {
                    try
                    {
                        foreach (int item in listboxSelectedCondition2.Items)//Precursor Charge State
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                        }
                    }
                    catch (Exception)
                    {
                        foreach (string item in listboxSelectedCondition2.Items)//Precursor Charge State
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                        }
                    }
                }
                allFragmentIonsAllConditions = _tempFragMethods;
                #endregion

                #region filter 3
                _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                if (listboxSelectedCondition3.Tag.Equals("Fragmentation Method"))
                {
                    foreach (string item in listboxSelectedCondition3.Items)//Frag Method
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition3.Tag.Equals("Activation Level"))
                {
                    foreach (string item in listboxSelectedCondition3.Items)//Activation Level
                    {
                        _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                    }
                }
                else if (listboxSelectedCondition3.Tag.Equals("Replicates"))
                {
                    try
                    {
                        foreach (int item in listboxSelectedCondition3.Items)//Replicates
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                        }
                    }
                    catch (Exception)
                    {
                        foreach (string item in listboxSelectedCondition3.Items)//Replicates
                        {
                            int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                        }
                    }
                }
                else if (listboxSelectedCondition3.Tag.Equals("Precursor Charge State"))
                {
                    try
                    {
                        foreach (int item in listboxSelectedCondition3.Items)//Precursor Charge State
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                        }
                    }
                    catch (Exception)
                    {
                        foreach (string item in listboxSelectedCondition3.Items)//Precursor Charge State
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                        }
                    }
                }
                allFragmentIonsAllConditions = _tempFragMethods;
                #endregion

                if (condition.StartsWith("Frag"))
                {
                    foreach (string frag in allFragmentIonsAllConditions.Select(a => a.Item1).Distinct().ToList())
                        listboxAllConditions.Items.Add(frag);
                }
                else if (condition.StartsWith("Act"))
                {
                    foreach (string frag in allFragmentIonsAllConditions.Select(a => a.Item5).Distinct().ToList())
                        listboxAllConditions.Items.Add(frag);
                }
                else if (condition.StartsWith("Prec"))
                {
                    foreach (int precursor in allFragmentIonsAllConditions.Select(a => a.Item2).Distinct().OrderByDescending(a => a).ToList())
                        listboxAllConditions.Items.Add(precursor);
                }
                else if (condition.StartsWith("Repl"))
                {
                    foreach (int replicate in allFragmentIonsAllConditions.Select(a => a.Item6).Distinct().OrderBy(a => a).ToList())
                        listboxAllConditions.Items.Add("R" + replicate);
                }
            }
        }

        private bool CheckAvailabilityCondition(ComboBox comboBoxCondition1, ComboBox comboBoxCondition2, ComboBox comboBoxCondition3, ComboBox comboBoxStudyCondition, ListBox listBoxAllFixedCondition, ListBox listBoxSelectedFixedCondition, ComboBox currentCb)
        {
            bool isError = false;
            string condition1 = "";
            string condition2 = "";
            string condition3 = "";
            string studyCondition = "";

            if (comboBoxCondition1.SelectedItem != null)
                condition1 = comboBoxCondition1.SelectedItem.ToString();
            if (comboBoxCondition2.SelectedItem != null)
                condition2 = comboBoxCondition2.SelectedItem.ToString();
            if (comboBoxCondition3.SelectedItem != null)
                condition3 = comboBoxCondition3.SelectedItem.ToString();
            if (comboBoxStudyCondition.SelectedItem != null)
                studyCondition = comboBoxStudyCondition.SelectedItem.ToString();

            if (!String.IsNullOrEmpty(condition1) && (condition1.Equals(condition2) || condition1.Equals(condition3)) ||
                !String.IsNullOrEmpty(condition2) && condition2.Equals(condition3) ||
                !String.IsNullOrEmpty(studyCondition) && (studyCondition.Equals(condition1) || studyCondition.Equals(condition2) || studyCondition.Equals(condition3)))
            {
                isError = true;
            }

            if (isError)
            {
                System.Windows.Forms.MessageBox.Show(
                   "Condition '" + currentCb.SelectedItem.ToString() + "' has already been chosen!",
                   "Warning",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                //BeginInvoke(new Action(() => currentCb.SelectedItem = null));
                BeginInvoke(new Action(() => currentCb.Text = "Select one option..."));

                listBoxAllFixedCondition.Items.Clear();
                listBoxSelectedFixedCondition.Items.Clear();

                return false;
            }

            if (String.IsNullOrEmpty(condition1) &&
                String.IsNullOrEmpty(condition2) &&
                String.IsNullOrEmpty(condition3)) return false;//reload fields
            return true;
        }

        private void buttonAddSelectedCondition_Click(object sender, EventArgs e)
        {
            ComboBox comboBoxCondition1 = (ComboBox)((object[])((Button)sender).Tag)[0];
            ComboBox comboBoxCondition2 = (ComboBox)((object[])((ComboBox)comboBoxCondition1).Tag)[1];
            ComboBox comboBoxCondition3 = (ComboBox)((object[])((ComboBox)comboBoxCondition1).Tag)[2];
            ComboBox comboBoxStudyCondition = (ComboBox)((object[])((ComboBox)comboBoxCondition1).Tag)[3];
            ListBox listBoxAllFixedCondition1 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[4];
            ListBox listBoxAllFixedCondition2 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[5];
            ListBox listBoxAllFixedCondition3 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[6];
            ListBox listBoxAllStudyCondition = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[7];
            ListBox listBoxSelectedFixedCondition1 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[8];
            ListBox listBoxSelectedFixedCondition2 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[9];
            ListBox listBoxSelectedFixedCondition3 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[10];
            ListBox listBoxSelectedStudyCondition = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[11];
            Button addNewMap = (Button)((object[])((ComboBox)comboBoxCondition1).Tag)[12];
            Button addSelectedCondition = (Button)((object[])((ComboBox)comboBoxCondition1).Tag)[13];
            List<(string, int, string, int, string, int, double, string)> allFragmentIonsAllConditions = (List<(string, int, string, int, string, int, double, string)>)((object[])((ComboBox)comboBoxCondition1).Tag)[14];
            CheckBox checkBoxGoldenComplemPairs = (CheckBox)((object[])((ComboBox)comboBoxCondition1).Tag)[15];
            CheckBox checkBoxCleavageConfidence = (CheckBox)((object[])((ComboBox)comboBoxCondition1).Tag)[16];

            ListBox currentListBoxAllConditions = null;
            ListBox currentListBoxSelectedCondition = null;

            int totalItemsInitital = 0;

            //Check which button was selected
            if (((Button)sender).Name.StartsWith("buttonAddFixedCondition1"))
            {
                currentListBoxAllConditions = listBoxAllFixedCondition1;
                currentListBoxSelectedCondition = listBoxSelectedFixedCondition1;
            }
            else if (((Button)sender).Name.StartsWith("buttonAddFixedCondition2"))
            {
                currentListBoxAllConditions = listBoxAllFixedCondition2;
                currentListBoxSelectedCondition = listBoxSelectedFixedCondition2;
            }
            else if (((Button)sender).Name.StartsWith("buttonAddFixedCondition3"))
            {
                currentListBoxAllConditions = listBoxAllFixedCondition3;
                currentListBoxSelectedCondition = listBoxSelectedFixedCondition3;
            }
            else
            {
                currentListBoxAllConditions = listBoxAllStudyCondition;
                currentListBoxSelectedCondition = listBoxSelectedStudyCondition;
            }
            totalItemsInitital = currentListBoxSelectedCondition.Items.Count;

            List<object> removedFixedCondition = new List<object>();
            //Get all values into listbox
            foreach (object item in currentListBoxAllConditions.SelectedItems)
            {
                currentListBoxSelectedCondition.Items.Add(item);
                removedFixedCondition.Add(item);
            }

            removedFixedCondition.ForEach(item =>
            {
                currentListBoxAllConditions.Items.Remove(item);
            });

            //Check whether clean all fields based on the changing of selected Items
            if (currentListBoxSelectedCondition.Items.Count != totalItemsInitital)
            {
                checkBoxGoldenComplemPairs.Checked = false;
                checkBoxGoldenComplemPairs.Enabled = false;
                checkBoxCleavageConfidence.Checked = false;

                if (((Button)sender).Name.StartsWith("buttonAddFixedCondition1"))
                {
                    #region Reset all fields
                    ResetFields(null,
                    listBoxAllFixedCondition2,
                    listBoxAllFixedCondition3,
                    null,
                    listBoxSelectedFixedCondition2,
                    listBoxSelectedFixedCondition3,
                    listBoxAllStudyCondition,
                    listBoxSelectedStudyCondition,
                    null,
                    comboBoxCondition2,
                    comboBoxCondition3,
                    comboBoxStudyCondition);
                    #endregion
                    comboBoxCondition2.Enabled = true;
                    comboBoxCondition3.Enabled = false;
                    comboBoxStudyCondition.Enabled = false;
                    allFragmentIonsAllConditions = Core.FragmentIons;
                }
                else if (((Button)sender).Name.StartsWith("buttonAddFixedCondition2"))
                {
                    #region Reset all fields
                    ResetFields(null,
                    null,
                    listBoxAllFixedCondition3,
                    null,
                    null,
                    listBoxSelectedFixedCondition3,
                    listBoxAllStudyCondition,
                    listBoxSelectedStudyCondition,
                    null,
                    null,
                    comboBoxCondition3,
                    comboBoxStudyCondition);
                    #endregion
                    comboBoxCondition3.Enabled = true;
                    comboBoxStudyCondition.Enabled = false;
                    #region filter 2
                    allFragmentIonsAllConditions = Core.FragmentIons;
                    List<(string, int, string, int, string, int, double, string)> _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                    if (listBoxSelectedFixedCondition1.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Precursor Charge State"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition1.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition1.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                            }
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion
                }
                else if (((Button)sender).Name.StartsWith("buttonAddFixedCondition3"))
                {
                    #region Reset all fields
                    ResetFields(null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    listBoxAllStudyCondition,
                    listBoxSelectedStudyCondition,
                    null,
                    null,
                    null,
                    comboBoxStudyCondition);
                    #endregion
                    comboBoxStudyCondition.Enabled = true;
                    #region filter 2
                    allFragmentIonsAllConditions = Core.FragmentIons;
                    List<(string, int, string, int, string, int, double, string)> _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                    if (listBoxSelectedFixedCondition1.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Precursor Charge State"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition1.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition1.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                            }
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion

                    #region filter 3
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                    if (listBoxSelectedFixedCondition2.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition2.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition2.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition2.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition2.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6.ToString().Equals(item)).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Precursor Charge State"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition2.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition2.Items)//Precursor Charge State
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion
                }
            }

            if (removedFixedCondition.Count > 0 && comboBoxCondition1 != null && comboBoxCondition1.SelectedItem != null && comboBoxCondition1.SelectedItem.ToString().StartsWith("Frag"))
                addSelectedCondition.Enabled = false;

            if (((Button)sender).Name.StartsWith("buttonAddStudyCondition"))
            {
                if (comboBoxStudyCondition.SelectedItem != null)
                {
                    #region Filter all results

                    #region filter Condition1
                    allFragmentIonsAllConditions = Core.FragmentIons;
                    List<(string, int, string, int, string, int, double, string)> _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                    if (listBoxSelectedFixedCondition1.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Precursor Charge State"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition1.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition1.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                            }
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion

                    #region filter Condition2
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                    if (listBoxSelectedFixedCondition2.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition2.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition2.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition2.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition2.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Precursor Charge State"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition2.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition2.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                            }
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion

                    #region filter Condition3
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                    if (listBoxSelectedFixedCondition3.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition3.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition3.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition3.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition3.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition3.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition3.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition3.Tag.Equals("Precursor Charge State"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition3.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition3.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                            }
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion

                    #region filter Study Condition
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                    if (listBoxSelectedStudyCondition.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedStudyCondition.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedStudyCondition.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedStudyCondition.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedStudyCondition.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedStudyCondition.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedStudyCondition.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedStudyCondition.Tag.Equals("Precursor Charge State"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedStudyCondition.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedStudyCondition.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                            }
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion

                    #endregion

                    #region Create Map

                    string studyCondition = comboBoxStudyCondition.SelectedItem.ToString();
                    string fixedCondition1 = comboBoxCondition1.SelectedItem.ToString();
                    string fixedCondition2 = comboBoxCondition2.SelectedItem.ToString();
                    string fixedCondition3 = comboBoxCondition3.SelectedItem.ToString();

                    /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions)>
                    (string, string, string, List<(string, int, string, int, string, int, double, string)>, bool, bool) currentMap;

                    string numberOfCondition = Regex.Split(addNewMap.Name, "_")[1];
                    string _key = studyCondition + "#" + fixedCondition1 + "#" + listBoxSelectedFixedCondition1.Items[0].ToString() + "#" + numberOfCondition;
                    if (Core.DictMaps.TryGetValue(_key, out currentMap))
                    {
                        Core.DictMaps[_key] = (fixedCondition1, fixedCondition2, fixedCondition3, allFragmentIonsAllConditions, checkBoxGoldenComplemPairs.Checked, checkBoxCleavageConfidence.Checked);
                        Console.WriteLine(" Edited Map {0}.", (Convert.ToInt32(numberOfCondition) + 1));
                    }
                    else //New map
                    {
                        // List<string> _keys = Core.DictMaps.Keys.ToList();
                        // if (_keys.Exists(a => a.Contains(studyCondition + "#" + fixedCondition1 + "#" + listBoxSelectedFixedCondition1.Items[0].ToString())))
                        // {
                        //     System.Windows.Forms.MessageBox.Show(
                        //"Similar Map has already been created!",
                        //"Warning",
                        //MessageBoxButtons.OK,
                        //MessageBoxIcon.Warning);

                        //     #region Reset study fields
                        //     ResetFields(null,
                        //     null,
                        //     null,
                        //     null,
                        //     null,
                        //     null,
                        //     listBoxAllStudyCondition,
                        //     listBoxSelectedStudyCondition,
                        //     null,
                        //     null,
                        //     null,
                        //     comboBoxStudyCondition);
                        //     #endregion

                        //     return;
                        // }

                        Core.DictMaps.Add(_key, (fixedCondition1, fixedCondition2, fixedCondition3, allFragmentIonsAllConditions, checkBoxGoldenComplemPairs.Checked, checkBoxCleavageConfidence.Checked));
                        Console.WriteLine(" Created Map {0}.", (Convert.ToInt32(numberOfCondition) + 1));
                    }

                    #endregion

                    #region Enable golden complementary pairs checkbox

                    checkBoxGoldenComplemPairs.Checked = false;
                    if (listBoxSelectedFixedCondition1.Items.Count == 1 &&
                        listBoxSelectedFixedCondition2.Items.Count == 1 &&
                        listBoxSelectedFixedCondition3.Items.Count == 1)
                    {
                        checkBoxGoldenComplemPairs.Enabled = true;
                    }
                    else
                    {
                        checkBoxGoldenComplemPairs.Enabled = false;
                    }

                    #endregion
                }
            }
            addNewMap.Enabled = true;
        }

        private void buttonRemoveSelectedCondition_Click(object sender, EventArgs e)
        {
            ComboBox comboBoxCondition1 = (ComboBox)((object[])((Button)sender).Tag)[0];
            ComboBox comboBoxCondition2 = (ComboBox)((object[])((ComboBox)comboBoxCondition1).Tag)[1];
            ComboBox comboBoxCondition3 = (ComboBox)((object[])((ComboBox)comboBoxCondition1).Tag)[2];
            ComboBox comboBoxStudyCondition = (ComboBox)((object[])((ComboBox)comboBoxCondition1).Tag)[3];
            ListBox listBoxAllFixedCondition1 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[4];
            ListBox listBoxAllFixedCondition2 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[5];
            ListBox listBoxAllFixedCondition3 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[6];
            ListBox listBoxAllStudyCondition = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[7];
            ListBox listBoxSelectedFixedCondition1 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[8];
            ListBox listBoxSelectedFixedCondition2 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[9];
            ListBox listBoxSelectedFixedCondition3 = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[10];
            ListBox listBoxSelectedStudyCondition = (ListBox)((object[])((ComboBox)comboBoxCondition1).Tag)[11];
            Button addNewMap = (Button)((object[])((ComboBox)comboBoxCondition1).Tag)[12];
            Button addSelectedCondition = (Button)((object[])((ComboBox)comboBoxCondition1).Tag)[13];
            List<(string, int, string, int, string, int, double, string)> allFragmentIonsAllConditions = (List<(string, int, string, int, string, int, double, string)>)((object[])((ComboBox)comboBoxCondition1).Tag)[14];
            CheckBox checkBoxGoldenComplemPairs = (CheckBox)((object[])((ComboBox)comboBoxCondition1).Tag)[15];
            CheckBox checkBoxCleavageConfidence = (CheckBox)((object[])((ComboBox)comboBoxCondition1).Tag)[16];

            ListBox currentListBoxAllConditions = null;
            ListBox currentListBoxSelectedCondition = null;
            ComboBox currentComboBoxCondition = null;

            int totalItemsInitital = 0;

            //Check which button was selected
            if (((Button)sender).Name.StartsWith("buttonRemoveFixedCondition1"))
            {
                currentListBoxAllConditions = listBoxAllFixedCondition1;
                currentListBoxSelectedCondition = listBoxSelectedFixedCondition1;
                currentComboBoxCondition = comboBoxCondition1;
            }
            else if (((Button)sender).Name.StartsWith("buttonRemoveFixedCondition2"))
            {
                currentListBoxAllConditions = listBoxAllFixedCondition2;
                currentListBoxSelectedCondition = listBoxSelectedFixedCondition2;
                currentComboBoxCondition = comboBoxCondition2;
            }
            else if (((Button)sender).Name.StartsWith("buttonRemoveFixedCondition3"))
            {
                currentListBoxAllConditions = listBoxAllFixedCondition3;
                currentListBoxSelectedCondition = listBoxSelectedFixedCondition3;
                currentComboBoxCondition = comboBoxCondition3;
            }
            else
            {
                currentListBoxAllConditions = listBoxAllStudyCondition;
                currentListBoxSelectedCondition = listBoxSelectedStudyCondition;
                currentComboBoxCondition = comboBoxStudyCondition;
            }
            totalItemsInitital = currentListBoxSelectedCondition.Items.Count;

            List<object> removedFragMethods = new List<object>();
            //Get all values into listbox
            foreach (object item in currentListBoxSelectedCondition.SelectedItems)
            {
                currentListBoxAllConditions.Items.Add(item);
                removedFragMethods.Add(item);
            }

            removedFragMethods.ForEach(item =>
            {
                currentListBoxSelectedCondition.Items.Remove(item);
            });

            //Check whether clean all fields based on the changing of selected Items
            if (currentListBoxSelectedCondition.Items.Count != totalItemsInitital)
            {
                checkBoxGoldenComplemPairs.Checked = false;
                checkBoxGoldenComplemPairs.Enabled = false;
                checkBoxCleavageConfidence.Checked = false;

                if (((Button)sender).Name.StartsWith("buttonRemoveFixedCondition1"))
                {
                    #region Reset all fields
                    ResetFields(null,
                    listBoxAllFixedCondition2,
                    listBoxAllFixedCondition3,
                    null,
                    listBoxSelectedFixedCondition2,
                    listBoxSelectedFixedCondition3,
                    listBoxAllStudyCondition,
                    listBoxSelectedStudyCondition,
                    null,
                    comboBoxCondition2,
                    comboBoxCondition3,
                    comboBoxStudyCondition);
                    #endregion
                    if (currentListBoxSelectedCondition.Items.Count == 0)
                        comboBoxCondition2.Enabled = false;
                    comboBoxCondition3.Enabled = false;
                    comboBoxStudyCondition.Enabled = false;
                    allFragmentIonsAllConditions = Core.FragmentIons;
                }
                else if (((Button)sender).Name.StartsWith("buttonRemoveFixedCondition2"))
                {
                    #region Reset all fields
                    ResetFields(null,
                    null,
                    listBoxAllFixedCondition3,
                    null,
                    null,
                    listBoxSelectedFixedCondition3,
                    listBoxAllStudyCondition,
                    listBoxSelectedStudyCondition,
                    null,
                    null,
                    comboBoxCondition3,
                    comboBoxStudyCondition);
                    #endregion
                    if (currentListBoxSelectedCondition.Items.Count == 0)
                        comboBoxCondition3.Enabled = false;
                    comboBoxStudyCondition.Enabled = false;
                    #region filter 2
                    allFragmentIonsAllConditions = Core.FragmentIons;
                    List<(string, int, string, int, string, int, double, string)> _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                    if (listBoxSelectedFixedCondition1.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Precursor Charge State"))
                    {
                        foreach (int item in listBoxSelectedFixedCondition1.Items)//Precursor Charge State
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion
                }
                else if (((Button)sender).Name.StartsWith("buttonRemoveFixedCondition3"))
                {
                    #region Reset all fields
                    ResetFields(null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    listBoxAllStudyCondition,
                    listBoxSelectedStudyCondition,
                    null,
                    null,
                    null,
                    comboBoxStudyCondition);
                    #endregion
                    if (currentListBoxSelectedCondition.Items.Count == 0)
                        comboBoxStudyCondition.Enabled = false;

                    #region filter 2
                    allFragmentIonsAllConditions = Core.FragmentIons;
                    List<(string, int, string, int, string, int, double, string)> _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                    if (listBoxSelectedFixedCondition1.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Precursor Charge State"))
                    {
                        foreach (int item in listBoxSelectedFixedCondition1.Items)//Precursor Charge State
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion

                    #region filter 3
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                    if (listBoxSelectedFixedCondition2.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition2.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition2.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition2.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition2.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Precursor Charge State"))
                    {
                        foreach (int item in listBoxSelectedFixedCondition2.Items)//Precursor Charge State
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion
                }
                if (listBoxSelectedStudyCondition.Items.Count == 0)
                    addNewMap.Enabled = false;

                try
                {
                    List<string> _keys = Core.DictMaps.Keys.ToList();
                    if (_keys.Count > 0)
                    {
                        int numberOfCondition = Convert.ToInt32(Regex.Split(addNewMap.Name, "_")[1]);
                        string _key = _keys.Where(a => a.EndsWith("#" + numberOfCondition)).FirstOrDefault();
                        Core.DictMaps.Remove(_key);
                        Console.WriteLine(" Removed Map {0}.", (Convert.ToInt32(numberOfCondition) + 1));
                    }
                }
                catch (Exception) { }

            }

            if (comboBoxCondition1 != null &&
                comboBoxCondition1.SelectedItem != null &&
                comboBoxCondition1.SelectedItem.ToString().StartsWith("Frag") &&
                currentComboBoxCondition != null &&
                currentComboBoxCondition.SelectedItem != null &&
                currentComboBoxCondition.SelectedItem.ToString().StartsWith("Frag") &&
                addSelectedCondition.Enabled == false && removedFragMethods.Count > 0)
                addSelectedCondition.Enabled = true;

            if (((Button)sender).Name.StartsWith("buttonRemoveStudyCondition"))
            {
                if (comboBoxStudyCondition.SelectedItem != null)
                {
                    #region Filter all results

                    #region filter Condition1
                    allFragmentIonsAllConditions = Core.FragmentIons;
                    List<(string, int, string, int, string, int, double, string)> _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();
                    if (listBoxSelectedFixedCondition1.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition1.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition1.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition1.Tag.Equals("Precursor Charge State"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition1.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition1.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                            }
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion

                    #region filter Condition2
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                    if (listBoxSelectedFixedCondition2.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition2.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition2.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition2.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition2.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition2.Tag.Equals("Precursor Charge State"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition2.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition2.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                            }
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion

                    #region filter Condition3
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                    if (listBoxSelectedFixedCondition3.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition3.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition3.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedFixedCondition3.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedFixedCondition3.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition3.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition3.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedFixedCondition3.Tag.Equals("Precursor Charge State"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedFixedCondition3.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedFixedCondition3.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                            }
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion

                    #region filter Study Condition
                    _tempFragMethods = new List<(string, int, string, int, string, int, double, string)>();

                    if (listBoxSelectedStudyCondition.Tag.Equals("Fragmentation Method"))
                    {
                        foreach (string item in listBoxSelectedStudyCondition.Items)//Frag Method
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item1.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedStudyCondition.Tag.Equals("Activation Level"))
                    {
                        foreach (string item in listBoxSelectedStudyCondition.Items)//Activation Level
                        {
                            _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item5.Equals(item)).ToList());
                        }
                    }
                    else if (listBoxSelectedStudyCondition.Tag.Equals("Replicates"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedStudyCondition.Items)//Replicates
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedStudyCondition.Items)//Replicates
                            {
                                int replicate = Convert.ToInt32(numberCaptured.Matches(item)[0].Value);
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item6 == replicate).ToList());
                            }
                        }
                    }
                    else if (listBoxSelectedStudyCondition.Tag.Equals("Precursor Charge State"))
                    {
                        try
                        {
                            foreach (int item in listBoxSelectedStudyCondition.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2 == item).ToList());
                            }
                        }
                        catch (Exception)
                        {
                            foreach (string item in listBoxSelectedStudyCondition.Items)//Precursor Charge State
                            {
                                _tempFragMethods.AddRange(allFragmentIonsAllConditions.Where(a => a.Item2.ToString().Equals(item)).ToList());
                            }
                        }
                    }
                    allFragmentIonsAllConditions = _tempFragMethods;
                    #endregion

                    #endregion

                    #region Create Map
                    string studyCondition = comboBoxStudyCondition.SelectedItem.ToString();
                    string fixedCondition1 = comboBoxCondition1.SelectedItem.ToString();
                    string fixedCondition2 = comboBoxCondition2.SelectedItem.ToString();
                    string fixedCondition3 = comboBoxCondition3.SelectedItem.ToString();

                    /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions)>
                    (string, string, string, List<(string, int, string, int, string, int, double, string)>, bool, bool) currentMap;

                    string numberOfCondition = Regex.Split(addNewMap.Name, "_")[1];
                    string _key = studyCondition + "#" + fixedCondition1 + "#" + listBoxSelectedFixedCondition1.Items[0].ToString() + "#" + numberOfCondition;
                    if (Core.DictMaps.TryGetValue(_key, out currentMap))
                    {
                        Core.DictMaps[_key] = (fixedCondition1, fixedCondition2, fixedCondition3, allFragmentIonsAllConditions, false, false);
                        Console.WriteLine(" Edited Map {0}.", (Convert.ToInt32(numberOfCondition) + 1));
                    }
                    else //New map
                    {
                        Core.DictMaps.Add(_key, (fixedCondition1, fixedCondition2, fixedCondition3, allFragmentIonsAllConditions, false, false));
                        Console.WriteLine(" Created Map {0}.", (Convert.ToInt32(numberOfCondition) + 1));
                    }
                    #endregion
                }
            }

            //if (EnableAddNewMapBtn(comboBoxCondition1, comboBoxCondition2, comboBoxCondition3, comboBoxStudyCondition))
            //{
            //    if (listBoxSelectedStudyCondition.Items.Count == 0)
            //        addNewMap.Enabled = false;
            //    else
            //        addNewMap.Enabled = true;
            //}
        }

        private void buttonAddMap_Click(object sender, EventArgs e)
        {
            (Button, Button) previousAddRemoveMap = Add_Remove_MapBtnList[numberOfConditions - 1];
            previousAddRemoveMap.Item1.Enabled = false;
            if (previousAddRemoveMap.Item2 != null)
                previousAddRemoveMap.Item2.Enabled = false;

            CreateNewMap();
        }

        private void buttonRemoveMap_Click(object sender, EventArgs e)
        {
            GroupBox groupBoxToBeRemoved = (GroupBox)((object[])((Button)sender).Tag)[0];
            int numberOfCondition = (int)((object[])((Button)sender).Tag)[1];

            groupBoxToBeRemoved.Visible = false;
            this.Controls.Add(groupBoxToBeRemoved);
            numberOfConditions--;

            (Button, Button) previousAddRemoveConditionBtn = Add_Remove_MapBtnList[numberOfCondition - 1];
            previousAddRemoveConditionBtn.Item1.Enabled = true;
            if (previousAddRemoveConditionBtn.Item2 != null)
                previousAddRemoveConditionBtn.Item2.Enabled = true;
            Add_Remove_MapBtnList.RemoveAt(Add_Remove_MapBtnList.Count - 1);

            try
            {
                List<string> _keys = Core.DictMaps.Keys.ToList();
                string _key = _keys.Where(a => a.EndsWith("#" + numberOfCondition)).FirstOrDefault();
                Core.DictMaps.Remove(_key);
            }
            catch (Exception) { }

            this.Height -= 230;
        }

        private void checkBoxGoldenComplemPairs_CheckedChanged(object sender, EventArgs e)
        {
            int numberOfCondition = Convert.ToInt32(numberCaptured.Matches(((CheckBox)sender).Name)[0].Value);

            if (Core != null)
            {
                if (Core.DictMaps != null && Core.DictMaps.Count > 0)
                {
                    List<string> _keys = Core.DictMaps.Keys.ToList();
                    string _key = _keys.Where(a => a.EndsWith("#" + numberOfCondition)).FirstOrDefault();
                    if (!String.IsNullOrEmpty(_key))
                    {
                        /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions, isGoldenComplementaryPairs, isBondCleavageConfidence)>
                        (string, string, string, List<(string, int, string, int, string, int, double, string)>, bool, bool) currentMap;
                        if (Core.DictMaps.TryGetValue(_key, out currentMap))
                        {
                            Core.DictMaps[_key] = (currentMap.Item1, currentMap.Item2, currentMap.Item3, currentMap.Item4, ((CheckBox)sender).Checked, currentMap.Item6);
                            Console.WriteLine(" Edited Map {0}.", (Convert.ToInt32(numberOfCondition) + 1));
                        }
                    }
                }
            }
        }

        private void checkBoxCleavageConfidence_CheckedChanged(object sender, EventArgs e)
        {
            int numberOfCondition = Convert.ToInt32(numberCaptured.Matches(((CheckBox)sender).Name)[0].Value);

            if (Core != null)
            {
                if (Core.DictMaps != null && Core.DictMaps.Count > 0)
                {
                    List<string> _keys = Core.DictMaps.Keys.ToList();
                    string _key = _keys.Where(a => a.EndsWith("#" + numberOfCondition)).FirstOrDefault();
                    if (!String.IsNullOrEmpty(_key))
                    {
                        /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions, isGoldenComplementaryPairs, isBondCleavageConfidence)>
                        (string, string, string, List<(string, int, string, int, string, int, double, string)>, bool, bool) currentMap;
                        if (Core.DictMaps.TryGetValue(_key, out currentMap))
                        {
                            Core.DictMaps[_key] = (currentMap.Item1, currentMap.Item2, currentMap.Item3, currentMap.Item4, currentMap.Item5, ((CheckBox)sender).Checked);
                            Console.WriteLine(" Edited Map {0}.", (Convert.ToInt32(numberOfCondition) + 1));
                        }
                    }
                }
            }
        }
    }
}
