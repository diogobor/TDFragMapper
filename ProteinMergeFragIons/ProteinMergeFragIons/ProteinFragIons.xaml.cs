﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProteinMergeFragIons
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ProteinFragIons : UserControl
    {
        /// <summary>
        /// Constants
        /// </summary>
        private const float SPACER_X = 27f;
        private const double ROWWIDTH_TABLE = 40;
        private const int FRAGMENT_ION_HEIGHT = 30;
        private const int FONTSIZE_BOX_CONDITION_SERIE = 55;
        private const int FONTSIZE_PROTEINSEQUENCE = 42;
        private const int FONTSIZE_AMINOACID_POSITION = 25;
        private const int WIDTH_LINE = 12;

        private string ProteinSequence { get; set; }
        private string ProteinSequenceInformation { get; set; }

        /// <summary>
        /// Local variables
        /// </summary>
        private double HighestX { get; set; }
        private double HighestY { get; set; }
        private bool showUVPD { get; set; } = false;
        private bool showEThcD { get; set; } = false;
        private bool showHCD { get; set; } = false;
        private bool showCID { get; set; } = false;
        private bool showSID { get; set; } = false;
        private bool showECD { get; set; } = false;
        private bool showETD { get; set; } = false;
        private bool showPrecursorChargeState { get; set; } = false;
        private bool showActivationLevel { get; set; } = false;
        private bool showReplicates { get; set; } = false;

        private bool isPrecursorChargeState { get; set; } = false;
        private bool isActivationLevel { get; set; } = false;
        private bool isFragmentationMethod { get; set; } = false;
        private bool IsGlobalIntensityMap { get; set; } = false;
        private bool HasMergeConditions { get; set; } = false;

        private Dictionary<string, List<string>> FragMethodsWithPrecursorChargeOrActivationLevelDict { get; set; }

        private int SPACER_Y = 0;
        private string[] PrecursorChargeStatesOrActivationLevelsColors { get; set; }
        private SolidColorBrush labelBrush_PTN = new SolidColorBrush(Colors.Black);
        private SolidColorBrush[] FRAGMENT_ION_LINE_COLORS { get; set; }
        private Dictionary<string, SolidColorBrush> FRAGMENT_ION_LINE_COLORS_DICT { get; set; }

        public ProteinFragIons()
        {
            InitializeComponent();
            #region Initialize Color array
            FRAGMENT_ION_LINE_COLORS = new SolidColorBrush[51];
            FRAGMENT_ION_LINE_COLORS[0] = new SolidColorBrush(Colors.Blue);
            FRAGMENT_ION_LINE_COLORS[1] = new SolidColorBrush(Colors.Yellow);
            FRAGMENT_ION_LINE_COLORS[2] = new SolidColorBrush(Colors.Green);
            FRAGMENT_ION_LINE_COLORS[3] = new SolidColorBrush(Colors.Brown);
            FRAGMENT_ION_LINE_COLORS[4] = new SolidColorBrush(Colors.Aqua);
            FRAGMENT_ION_LINE_COLORS[5] = new SolidColorBrush(Colors.Black);
            FRAGMENT_ION_LINE_COLORS[6] = new SolidColorBrush(Colors.DarkGreen);
            FRAGMENT_ION_LINE_COLORS[7] = new SolidColorBrush(Colors.DarkViolet);
            FRAGMENT_ION_LINE_COLORS[8] = new SolidColorBrush(Colors.Navy);
            FRAGMENT_ION_LINE_COLORS[9] = new SolidColorBrush(Colors.Gray);
            FRAGMENT_ION_LINE_COLORS[10] = new SolidColorBrush(Colors.Maroon);
            FRAGMENT_ION_LINE_COLORS[11] = new SolidColorBrush(Colors.Magenta);
            FRAGMENT_ION_LINE_COLORS[12] = new SolidColorBrush(Colors.MediumAquamarine);
            FRAGMENT_ION_LINE_COLORS[13] = new SolidColorBrush(Colors.LightSteelBlue);
            FRAGMENT_ION_LINE_COLORS[14] = new SolidColorBrush(Colors.BurlyWood);
            FRAGMENT_ION_LINE_COLORS[15] = new SolidColorBrush(Colors.Beige);
            FRAGMENT_ION_LINE_COLORS[16] = new SolidColorBrush(Colors.Aqua);
            FRAGMENT_ION_LINE_COLORS[17] = new SolidColorBrush(Colors.OliveDrab);
            FRAGMENT_ION_LINE_COLORS[18] = new SolidColorBrush(Colors.OrangeRed);
            FRAGMENT_ION_LINE_COLORS[19] = new SolidColorBrush(Colors.PaleGreen);
            FRAGMENT_ION_LINE_COLORS[20] = new SolidColorBrush(Colors.PapayaWhip);
            FRAGMENT_ION_LINE_COLORS[21] = new SolidColorBrush(Colors.Peru);
            FRAGMENT_ION_LINE_COLORS[22] = new SolidColorBrush(Colors.Silver);
            FRAGMENT_ION_LINE_COLORS[23] = new SolidColorBrush(Colors.SeaShell);
            FRAGMENT_ION_LINE_COLORS[24] = new SolidColorBrush(Colors.SkyBlue);
            FRAGMENT_ION_LINE_COLORS[25] = new SolidColorBrush(Colors.Turquoise);
            FRAGMENT_ION_LINE_COLORS[26] = new SolidColorBrush(Colors.Tan);
            FRAGMENT_ION_LINE_COLORS[27] = new SolidColorBrush(Colors.Teal);
            FRAGMENT_ION_LINE_COLORS[28] = new SolidColorBrush(Colors.Thistle);
            FRAGMENT_ION_LINE_COLORS[29] = new SolidColorBrush(Colors.Tomato);
            FRAGMENT_ION_LINE_COLORS[30] = new SolidColorBrush(Colors.Transparent);
            FRAGMENT_ION_LINE_COLORS[31] = new SolidColorBrush(Colors.Wheat);
            FRAGMENT_ION_LINE_COLORS[32] = new SolidColorBrush(Colors.White);
            FRAGMENT_ION_LINE_COLORS[33] = new SolidColorBrush(Colors.WhiteSmoke);
            FRAGMENT_ION_LINE_COLORS[34] = new SolidColorBrush(Colors.Yellow);
            FRAGMENT_ION_LINE_COLORS[35] = new SolidColorBrush(Colors.YellowGreen);
            FRAGMENT_ION_LINE_COLORS[36] = new SolidColorBrush(Colors.AliceBlue);
            FRAGMENT_ION_LINE_COLORS[37] = new SolidColorBrush(Colors.AntiqueWhite);
            FRAGMENT_ION_LINE_COLORS[38] = new SolidColorBrush(Colors.Aqua);
            FRAGMENT_ION_LINE_COLORS[39] = new SolidColorBrush(Colors.Aquamarine);
            FRAGMENT_ION_LINE_COLORS[40] = new SolidColorBrush(Colors.Azure);
            FRAGMENT_ION_LINE_COLORS[41] = new SolidColorBrush(Colors.Beige);
            FRAGMENT_ION_LINE_COLORS[42] = new SolidColorBrush(Colors.Bisque);
            FRAGMENT_ION_LINE_COLORS[43] = new SolidColorBrush(Colors.Black);
            FRAGMENT_ION_LINE_COLORS[44] = new SolidColorBrush(Colors.BlanchedAlmond);
            FRAGMENT_ION_LINE_COLORS[45] = new SolidColorBrush(Colors.Blue);
            FRAGMENT_ION_LINE_COLORS[46] = new SolidColorBrush(Colors.BlueViolet);
            FRAGMENT_ION_LINE_COLORS[47] = new SolidColorBrush(Colors.Brown);
            FRAGMENT_ION_LINE_COLORS[48] = new SolidColorBrush(Colors.BurlyWood);
            FRAGMENT_ION_LINE_COLORS[49] = new SolidColorBrush(Colors.CadetBlue);
            FRAGMENT_ION_LINE_COLORS[50] = new SolidColorBrush(Colors.Chartreuse);
            #endregion
        }

        private void SetCanvasScrollBarSize(double width = 4096, double height = 2028)
        {
            MyCanvas.Width = width;
            MyCanvas.Height = height;
        }
        public void SetFragMethodDictionary(Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double)>)> DictMaps, string proteinSequence, string proteinSequenceInformation, bool hasIntensityperMap = false, bool isGlobalIntensityMap = false, bool hasMergeMaps = false)
        {
            isPrecursorChargeState = false;
            isActivationLevel = false;
            isFragmentationMethod = false;
            IsGlobalIntensityMap = isGlobalIntensityMap;
            ProteinSequence = proteinSequence;
            ProteinSequenceInformation = proteinSequenceInformation;
            HasMergeConditions = hasMergeMaps;

            //List<(fragmentationMethod, precursorCharge/activation level,IonType, aaPosition, intensity)>
            List<(string, string, string, int, double)> fragmentIons = new List<(string, string, string, int, double)>();
            FragMethodsWithPrecursorChargeOrActivationLevelDict = new Dictionary<string, List<string>>();
            List<int> allPrecursorChargeStateList = new List<int>();
            PrecursorChargeStatesOrActivationLevelsColors = null;

            foreach (KeyValuePair<string, (string, string, string, List<(string, int, string, int, string, int, double)>)> entry in DictMaps)
            {
                if (HasMergeConditions)
                {
                    if (entry.Key.StartsWith("Merge"))
                    {
                        List<string> fragmentationMethodList = entry.Value.Item4.Select(a => a.Item1).Distinct().ToList();
                        FragMethodsWithPrecursorChargeOrActivationLevelDict.Add(entry.Value.Item1 + "#MC", fragmentationMethodList);
                        fragmentIons.AddRange((from eachEntry in entry.Value.Item4
                                               select (eachEntry.Item1, eachEntry.Item5, eachEntry.Item3, eachEntry.Item4, eachEntry.Item7)).OrderByDescending(a => a.Item1).ToList());

                    }
                    else
                        continue;
                }
                else
                {
                    if (entry.Key.StartsWith("Precursor Charge State"))
                    //one key -> many maps
                    {
                        string[] cols = Regex.Split(entry.Key, "#");
                        List<string> precursorChargeStateList = entry.Value.Item4.Select(a => a.Item2).OrderByDescending(a => a).Select(a => a.ToString()).Distinct().ToList();
                        FragMethodsWithPrecursorChargeOrActivationLevelDict.Add(cols[2] + "#PCS", precursorChargeStateList);
                        var currentFragIons = (from eachEntry in entry.Value.Item4
                                               select (eachEntry.Item1, eachEntry.Item2, eachEntry.Item3, eachEntry.Item4, eachEntry.Item7)).OrderByDescending(a => a.Item2).ToList();
                        fragmentIons.AddRange((from eachEntry in currentFragIons
                                               select (eachEntry.Item1, eachEntry.Item2.ToString(), eachEntry.Item3, eachEntry.Item4, eachEntry.Item5)).ToList());

                        #region Get all Precursor ChargeStates
                        isPrecursorChargeState = true;
                        allPrecursorChargeStateList.AddRange(precursorChargeStateList.Select(a => int.Parse(a)).Distinct());
                        #endregion

                    }
                    else if (entry.Key.StartsWith("Activation Level"))
                    //one key -> many maps
                    {
                        string[] cols = Regex.Split(entry.Key, "#");
                        List<string> activationLevelList = entry.Value.Item4.Select(a => a.Item5).Distinct().OrderByDescending(a => a).ToList();
                        FragMethodsWithPrecursorChargeOrActivationLevelDict.Add(cols[2] + "#AL", activationLevelList);
                        fragmentIons.AddRange((from eachEntry in entry.Value.Item4
                                               select (eachEntry.Item1, eachEntry.Item5, eachEntry.Item3, eachEntry.Item4, eachEntry.Item7)).OrderByDescending(a => a.Item2).ToList());

                        isActivationLevel = true;
                    }
                    else if (entry.Key.StartsWith("Fragmentation Method"))
                    {
                        List<string> fragmentationMethodList = entry.Value.Item4.Select(a => a.Item1).Distinct().ToList();
                        FragMethodsWithPrecursorChargeOrActivationLevelDict.Add(entry.Value.Item1 + "#FM", fragmentationMethodList);
                        fragmentIons.AddRange((from eachEntry in entry.Value.Item4
                                               select (eachEntry.Item1, eachEntry.Item5, eachEntry.Item3, eachEntry.Item4, eachEntry.Item7)).OrderByDescending(a => a.Item1).ToList());

                        isFragmentationMethod = true;
                    }
                }
            }

            if (!this.HasMergeConditions)
            {
                if (isPrecursorChargeState)
                {
                    #region Get all Precursor Charge States
                    allPrecursorChargeStateList = allPrecursorChargeStateList.Distinct().ToList();
                    //order precursor charge states
                    allPrecursorChargeStateList.Sort((a, b) => b.CompareTo(a));
                    if (PrecursorChargeStatesOrActivationLevelsColors == null)
                        PrecursorChargeStatesOrActivationLevelsColors = allPrecursorChargeStateList.Select(a => a.ToString()).ToArray();
                    else
                        PrecursorChargeStatesOrActivationLevelsColors = PrecursorChargeStatesOrActivationLevelsColors.Concat(allPrecursorChargeStateList.Select(a => a.ToString())).ToArray();
                    #endregion
                }
                if (isActivationLevel)
                {
                    #region Get all Activation Levels
                    if (PrecursorChargeStatesOrActivationLevelsColors == null)
                    {
                        PrecursorChargeStatesOrActivationLevelsColors = (from item in fragmentIons
                                                                         select item.Item2).Distinct().OrderByDescending(a => a).ToArray();
                    }
                    else
                    {
                        PrecursorChargeStatesOrActivationLevelsColors = PrecursorChargeStatesOrActivationLevelsColors.Concat((from item in fragmentIons
                                                                                                                              select item.Item2).Distinct().OrderByDescending(a => a)).ToArray();
                    }
                    #endregion
                }
                if (isFragmentationMethod)
                {
                    #region Get all Fragmentation Methods
                    if (PrecursorChargeStatesOrActivationLevelsColors == null)
                    {
                        PrecursorChargeStatesOrActivationLevelsColors = (from item in fragmentIons
                                                                         select item.Item1).Distinct().OrderByDescending(a => a).ToArray();
                    }
                    else
                    {
                        PrecursorChargeStatesOrActivationLevelsColors = PrecursorChargeStatesOrActivationLevelsColors.Concat((from item in fragmentIons
                                                                                                                              select item.Item1).Distinct().OrderByDescending(a => a)).ToArray();
                    }
                    #endregion
                }

                PrecursorChargeStatesOrActivationLevelsColors = PrecursorChargeStatesOrActivationLevelsColors.Distinct().ToArray();

                Array.Sort<string>(PrecursorChargeStatesOrActivationLevelsColors, new Comparison<string>(
                      (i1, i2) => i2.Length.CompareTo(i1.Length)));
            }

            PreparePictureProteinFragmentIons(true, fragmentIons, hasIntensityperMap);

        }
        public void PreparePictureProteinFragmentIons(bool isSingleLine, List<(string, string, string, int, double)> fragmentIons, bool hasIntensityperMap = false)
        {
            SPACER_Y = 0;

            this.DrawProteinFragmentIons(isSingleLine, fragmentIons, hasIntensityperMap);
        }

        /// <summary>
        /// Method responsible for cleaning all canvas
        /// </summary>
        public void Clear()
        {
            MyCanvas.Children.Clear();
        }

        public void DrawProteinFragmentIons(bool isSingleLine, List<(string, string, string, int, double)> fragmentIons, bool hasIntensityperMap = false)
        {
            if (FragMethodsWithPrecursorChargeOrActivationLevelDict == null)
                return;

            if (!this.HasMergeConditions)
            {
                #region set which fragmentation method will be shown and the order
                Dictionary<string, List<string>> tmpFragMethodsWithPrecursorChargeOrActivationLevelDict = new Dictionary<string, List<string>>();

                Dictionary<string, List<string>> currentDict = FragMethodsWithPrecursorChargeOrActivationLevelDict.Where(a => Regex.Split(a.Key, "#")[0].Contains("UVPD")).ToDictionary(x => x.Key, y => y.Value);
                if (currentDict.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in currentDict)
                        tmpFragMethodsWithPrecursorChargeOrActivationLevelDict.Add(kvp.Key, kvp.Value);
                    showUVPD = true;
                }
                else
                    showUVPD = false;

                currentDict = FragMethodsWithPrecursorChargeOrActivationLevelDict.Where(a => Regex.Split(a.Key, "#")[0].Contains("EThcD")).ToDictionary(x => x.Key, y => y.Value);
                if (currentDict.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in currentDict)
                        tmpFragMethodsWithPrecursorChargeOrActivationLevelDict.Add(kvp.Key, kvp.Value);
                    showEThcD = true;
                }
                else
                    showEThcD = false;

                currentDict = FragMethodsWithPrecursorChargeOrActivationLevelDict.Where(a => Regex.Split(a.Key, "#")[0].Contains("CID")).ToDictionary(x => x.Key, y => y.Value);
                if (currentDict.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in currentDict)
                        tmpFragMethodsWithPrecursorChargeOrActivationLevelDict.Add(kvp.Key, kvp.Value);
                    showCID = true;
                }
                else
                    showCID = false;

                currentDict = FragMethodsWithPrecursorChargeOrActivationLevelDict.Where(a => Regex.Split(a.Key, "#")[0].Contains("HCD")).ToDictionary(x => x.Key, y => y.Value);
                if (currentDict.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in currentDict)
                        tmpFragMethodsWithPrecursorChargeOrActivationLevelDict.Add(kvp.Key, kvp.Value);
                    showHCD = true;
                }
                else
                    showHCD = false;

                currentDict = FragMethodsWithPrecursorChargeOrActivationLevelDict.Where(a => Regex.Split(a.Key, "#")[0].Contains("SID")).ToDictionary(x => x.Key, y => y.Value);
                if (currentDict.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in currentDict)
                        tmpFragMethodsWithPrecursorChargeOrActivationLevelDict.Add(kvp.Key, kvp.Value);
                    showSID = true;
                }
                else
                    showSID = false;

                currentDict = FragMethodsWithPrecursorChargeOrActivationLevelDict.Where(a => Regex.Split(a.Key, "#")[0].Contains("ECD")).ToDictionary(x => x.Key, y => y.Value);
                if (currentDict.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in currentDict)
                        tmpFragMethodsWithPrecursorChargeOrActivationLevelDict.Add(kvp.Key, kvp.Value);
                    showECD = true;
                }
                else
                    showECD = false;

                currentDict = FragMethodsWithPrecursorChargeOrActivationLevelDict.Where(a => Regex.Split(a.Key, "#")[0].Contains("ETD")).ToDictionary(x => x.Key, y => y.Value);
                if (currentDict.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in currentDict)
                        tmpFragMethodsWithPrecursorChargeOrActivationLevelDict.Add(kvp.Key, kvp.Value);
                    showETD = true;
                }
                else
                    showETD = false;

                currentDict = FragMethodsWithPrecursorChargeOrActivationLevelDict.Where(a => Regex.Split(a.Key, "#")[0].Contains("Precursor Charge State")).ToDictionary(x => x.Key, y => y.Value);
                if (currentDict.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in currentDict)
                        tmpFragMethodsWithPrecursorChargeOrActivationLevelDict.Add(kvp.Key, kvp.Value);
                    showPrecursorChargeState = true;
                }
                else
                    showPrecursorChargeState = false;

                currentDict = FragMethodsWithPrecursorChargeOrActivationLevelDict.Where(a => Regex.Split(a.Key, "#")[0].Contains("Activation Level")).ToDictionary(x => x.Key, y => y.Value);
                if (currentDict.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in currentDict)
                        tmpFragMethodsWithPrecursorChargeOrActivationLevelDict.Add(kvp.Key, kvp.Value);
                    showActivationLevel = true;
                }
                else
                    showActivationLevel = false;

                currentDict = FragMethodsWithPrecursorChargeOrActivationLevelDict.Where(a => Regex.Split(a.Key, "#")[0].Contains("Replicates")).ToDictionary(x => x.Key, y => y.Value);
                if (currentDict.Count > 0)
                {
                    foreach (KeyValuePair<string, List<string>> kvp in currentDict)
                        tmpFragMethodsWithPrecursorChargeOrActivationLevelDict.Add(kvp.Key, kvp.Value);
                    showReplicates = true;
                }
                else
                    showReplicates = false;

                FragMethodsWithPrecursorChargeOrActivationLevelDict = tmpFragMethodsWithPrecursorChargeOrActivationLevelDict;
                #endregion
            }

            #region set initial variables
            List<double> PtnCharPositions = new List<double>();
            double initialXLine = 0;
            double initialYLine = 0;

            initialXLine = 60;
            initialYLine = 60;
            Color COLOR_SERIES_RECTANGLE = new Color();
            COLOR_SERIES_RECTANGLE.R = 91;
            COLOR_SERIES_RECTANGLE.G = 91;
            COLOR_SERIES_RECTANGLE.B = 91;
            COLOR_SERIES_RECTANGLE.A = 35;

            double global_intensity_normalization_factor = 0;
            if (this.IsGlobalIntensityMap)
                global_intensity_normalization_factor = fragmentIons.Max(a => a.Item5);

            #endregion

            #region Plot vertical lines

            #region Draw Fragment ion lines
            double HeightRectA = 0;
            double HeightRectNterm = 0;
            double HeightRectC = 0;
            double HeightRectX = 0;
            double HeightRectY = 0;
            double HeightRectZ = 0;
            int offSetY = 0;
            int leftOffsetProtein = 0;
            // Create a blue and a black Brush  
            SolidColorBrush backgroundColor = new SolidColorBrush();
            SolidColorBrush blackBrush = new SolidColorBrush();

            int countCurrentFragMethod = 0;
            foreach (KeyValuePair<string, List<string>> currentFragMethod in FragMethodsWithPrecursorChargeOrActivationLevelDict)
            {
                isPrecursorChargeState = false;
                isActivationLevel = false;
                isFragmentationMethod = false;

                List<string> PrecursorChargesOrActivationLevels = currentFragMethod.Value;

                string[] cols = Regex.Split(currentFragMethod.Key, "#");
                string fragMethod = cols[0];
                string studyCondition = cols[1];
                if (studyCondition.Equals("PCS"))
                    isPrecursorChargeState = true;
                else if (studyCondition.Equals("AL"))
                    isActivationLevel = true;
                else if (studyCondition.Equals("FM"))
                    isFragmentationMethod = true;

                leftOffsetProtein = PrecursorChargesOrActivationLevels.Max(a => a.Length);

                List<(string, string, string, int, double)> currentFragmentIons = null;
                if (this.HasMergeConditions || (isFragmentationMethod && (showPrecursorChargeState || showActivationLevel || showReplicates)))
                    currentFragmentIons = fragmentIons;
                else
                    currentFragmentIons = fragmentIons.Where(a => a.Item1.Equals(fragMethod)).ToList();

                //Series left -> right (a,b,c)
                List<(string, string, string, int, double)> currentAFragmentIons = currentFragmentIons.Where(a => a.Item3.Equals("A")).ToList();
                List<(string, string, string, int, double)> currentBFragmentIons = currentFragmentIons.Where(a => a.Item3.Equals("B")).ToList();
                List<(string, string, string, int, double)> currentCFragmentIons = currentFragmentIons.Where(a => a.Item3.Equals("C")).ToList();
                //Series right-> left (x,y,z)
                List<(string, string, string, int, double)> currentXFragmentIons = currentFragmentIons.Where(a => a.Item3.Equals("X")).ToList();
                List<(string, string, string, int, double)> currentYFragmentIons = currentFragmentIons.Where(a => a.Item3.Equals("Y")).ToList();
                List<(string, string, string, int, double)> currentZFragmentIons = currentFragmentIons.Where(a => a.Item3.Equals("Z")).ToList();

                double intensity_normalization = 0;

                if (fragMethod.Equals("Merge"))
                {
                    #region Merge conditions

                    leftOffsetProtein = 0;
                    if (IsGlobalIntensityMap)
                        intensity_normalization = global_intensity_normalization_factor;
                    else if (hasIntensityperMap)
                        intensity_normalization = currentFragmentIons.Max(a => a.Item5);

                    HeightRectNterm = 0;
                    HeightRectY = 0;
                    SPACER_Y = 0;
                    int offsetRectCID = offSetY;

                    #region Plot protein Sequence
                    List<Label> proteinCharsAndSpaces = new List<Label>();
                    PlotProteinSequence(PtnCharPositions, proteinCharsAndSpaces, leftOffsetProtein);
                    #endregion

                    #region Serie N-Term
                    int countPrecursorChargesNterm = 0;
                    List<(string, string, string, int, double)> fragmentIonsNterm = currentAFragmentIons.Concat(currentBFragmentIons).Concat(currentCFragmentIons).ToList();
                    if (fragmentIonsNterm.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, fragmentIonsNterm, proteinCharsAndSpaces, ref countPrecursorChargesNterm, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectNterm = (countPrecursorChargesNterm + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesNterm * 9.5);

                        // create Background rect N-term
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectNterm, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "Nter");
                    }
                    #endregion

                    #region Update protein position
                    double proteinY = HeightRectNterm + 10;
                    for (int i = 0; i < ProteinSequence.Length; i++)
                    {
                        MyCanvas.Children.Add(proteinCharsAndSpaces[i]);
                        Canvas.SetTop(proteinCharsAndSpaces[i], initialYLine + proteinY + offSetY);
                    }
                    #endregion

                    #region Serie C-term
                    SPACER_Y = 0;
                    offSetY += (int)initialYLine + (int)proteinY;

                    int countPrecursorChargesCterm = 0;
                    List<(string, string, string, int, double)> fragmentIonsCterm = currentXFragmentIons.Concat(currentYFragmentIons).Concat(currentZFragmentIons).ToList();
                    if (fragmentIonsCterm.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, fragmentIonsCterm, proteinCharsAndSpaces, ref countPrecursorChargesCterm, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectY = (countPrecursorChargesCterm + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesCterm * 9.5);

                        // create Background rect C-term
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectY, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "Cter");
                    }
                    #endregion

                    #region rectangle FragMethod
                    double height_rect = HeightRectNterm + HeightRectY + 70;
                    double font_pos_condition = HeightRectNterm + 90;
                    if (fragMethod.Equals("CID"))
                        RectCondition(initialYLine, offsetRectCID, height_rect, font_pos_condition, ref offSetY, "CID");
                    else if (fragMethod.Equals("HCD"))
                        RectCondition(initialYLine, offsetRectCID, height_rect, font_pos_condition, ref offSetY, "HCD");
                    else if (fragMethod.Equals("SID"))
                        RectCondition(initialYLine, offsetRectCID, height_rect, font_pos_condition, ref offSetY, "SID");
                    else
                        RectCondition(initialYLine, offsetRectCID, height_rect, font_pos_condition, ref offSetY, "");
                    #endregion

                    #endregion
                }
                else if (fragMethod.Equals("UVPD"))
                {
                    #region UVPD -> fragmentation method

                    if (IsGlobalIntensityMap)
                        intensity_normalization = global_intensity_normalization_factor;
                    else if (hasIntensityperMap)
                        intensity_normalization = currentFragmentIons.Max(a => a.Item5);

                    HeightRectA = 0;
                    HeightRectNterm = 0;
                    HeightRectC = 0;
                    HeightRectX = 0;
                    HeightRectY = 0;
                    HeightRectZ = 0;

                    #region Plot protein Sequence
                    List<Label> proteinCharsAndSpaces = new List<Label>();
                    PlotProteinSequence(PtnCharPositions, proteinCharsAndSpaces, leftOffsetProtein);
                    #endregion

                    #region Serie A
                    int countPrecursorChargesA = 0;
                    if (currentAFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentAFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesA, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectA = (countPrecursorChargesA + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesA * 9.5);

                        // create Background rect Serie A
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectA, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "A");
                    }
                    #endregion

                    #region Serie B
                    SPACER_Y = 0;
                    offSetY = (int)initialYLine + (int)HeightRectA - FRAGMENT_ION_HEIGHT - 15;

                    int countPrecursorChargesB = 0;
                    if (currentBFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentBFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesB, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectNterm = (countPrecursorChargesB + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesB * 9.5);

                        // create Background rect Serie B
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectNterm, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "B");
                    }
                    #endregion

                    #region Serie C
                    SPACER_Y = 0;
                    if (HeightRectNterm != 0)
                    {
                        offSetY += (int)initialYLine + (int)HeightRectNterm - FRAGMENT_ION_HEIGHT - 15;
                    }
                    else
                    {
                        offSetY += (int)initialYLine - FRAGMENT_ION_HEIGHT - 15;
                    }

                    int countPrecursorChargesC = 0;
                    if (currentCFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentCFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesC, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectC = (countPrecursorChargesC + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesC * 9.5);

                        // create Background rect Serie C
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectC, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "C");
                    }
                    #endregion

                    #region Update protein position

                    double proteinY = HeightRectC - initialYLine + 2 * FRAGMENT_ION_HEIGHT + 10;
                    for (int i = 0; i < ProteinSequence.Length; i++)
                    {
                        MyCanvas.Children.Add(proteinCharsAndSpaces[i]);
                        Canvas.SetTop(proteinCharsAndSpaces[i], initialYLine + proteinY + offSetY);
                    }
                    proteinY = HeightRectA + HeightRectNterm + HeightRectC + 65;
                    #endregion

                    #region Serie X
                    SPACER_Y = 0;
                    offSetY = (int)initialYLine + (int)proteinY - 10;

                    int countPrecursorChargesX = 0;
                    if (currentXFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentXFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesX, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectX = (countPrecursorChargesX + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesX * 9.5);

                        // create Background rect Serie X
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectX, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "X");
                    }
                    #endregion

                    #region Serie Y
                    SPACER_Y = 0;
                    offSetY += (int)initialYLine + (int)HeightRectX - FRAGMENT_ION_HEIGHT - 15;

                    int countPrecursorChargesY = 0;
                    if (currentYFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentYFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesY, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectY = (countPrecursorChargesY + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesY * 9.5);

                        // create Background rect Serie Y
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectY, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "Y");
                    }
                    #endregion

                    #region Serie Z
                    SPACER_Y = 0;
                    offSetY += (int)initialYLine + (int)HeightRectY - FRAGMENT_ION_HEIGHT - 15;

                    int countPrecursorChargesZ = 0;
                    if (currentZFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentZFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesZ, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectZ = (countPrecursorChargesZ + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesZ * 9.5);

                        // create Background rect Serie Z
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectZ, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "Z");
                    }
                    #endregion

                    #region rectangle FragMethod UVPD
                    double height_rect = (HeightRectA + HeightRectNterm + HeightRectC + HeightRectX + HeightRectY + HeightRectZ + 145);
                    double font_pos_condition = HeightRectA + HeightRectNterm + HeightRectC + 130;
                    RectCondition(initialYLine, 0, height_rect, font_pos_condition, ref offSetY, "UVPD");
                    #endregion

                    #endregion
                }
                else if (fragMethod.Equals("EThcD"))
                {
                    #region EThcD -> fragmentation method

                    if (IsGlobalIntensityMap)
                        intensity_normalization = global_intensity_normalization_factor;
                    else if (hasIntensityperMap)
                        intensity_normalization = currentFragmentIons.Max(a => a.Item5);

                    HeightRectNterm = 0;
                    HeightRectC = 0;
                    HeightRectY = 0;
                    HeightRectZ = 0;
                    SPACER_Y = 0;
                    int offsetRectEThcD = offSetY;

                    #region Plot protein Sequence
                    List<Label> proteinCharsAndSpaces = new List<Label>();
                    PlotProteinSequence(PtnCharPositions, proteinCharsAndSpaces, leftOffsetProtein);
                    #endregion

                    #region Serie B
                    int countPrecursorChargesB = 0;
                    if (currentBFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentBFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesB, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectNterm = (countPrecursorChargesB + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesB * 9.5);

                        // create Background rect Serie B
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectNterm, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "B");
                    }
                    #endregion

                    #region Serie C
                    SPACER_Y = 0;
                    if (HeightRectNterm != 0)
                        offSetY += (int)initialYLine + (int)HeightRectNterm - FRAGMENT_ION_HEIGHT - 15;

                    int countPrecursorChargesC = 0;
                    if (currentCFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentCFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesC, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectC = (countPrecursorChargesC + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesC * 9.5);

                        // create Background rect Serie C
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectC, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "C");
                    }
                    #endregion

                    #region Update protein position
                    double proteinY = HeightRectC - initialYLine + 2 * FRAGMENT_ION_HEIGHT + 10;
                    for (int i = 0; i < ProteinSequence.Length; i++)
                    {
                        MyCanvas.Children.Add(proteinCharsAndSpaces[i]);
                        Canvas.SetTop(proteinCharsAndSpaces[i], initialYLine + proteinY + offSetY);
                    }
                    #endregion

                    #region Serie Y 
                    SPACER_Y = 0;
                    offSetY += (int)initialYLine + (int)proteinY;

                    int countPrecursorChargesY = 0;
                    if (currentYFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentYFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesY, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectY = (countPrecursorChargesY + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesY * 9.5);

                        // create Background rect Serie Y
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectY, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "Y");
                    }
                    #endregion

                    #region Serie Z
                    SPACER_Y = 0;
                    offSetY += (int)initialYLine + (int)HeightRectY - FRAGMENT_ION_HEIGHT - 15;

                    int countPrecursorChargesZ = 0;
                    if (currentZFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentZFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesZ, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectZ = (countPrecursorChargesZ + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesZ * 9.5);

                        // create Background rect Serie Z
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectZ, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "Z");
                    }
                    #endregion

                    #region rectangle FragMethod EThcD
                    double height_rect = 0;
                    if (HeightRectNterm == 0 || HeightRectZ == 0)
                        height_rect = (HeightRectNterm + HeightRectC + HeightRectY + HeightRectZ + 85);
                    else
                        height_rect = (HeightRectNterm + HeightRectC + HeightRectY + HeightRectZ + 100);
                    double font_pos_condition = HeightRectNterm + HeightRectC + 130;

                    RectCondition(initialYLine, offsetRectEThcD, height_rect, font_pos_condition, ref offSetY, "EThcD");
                    #endregion

                    SPACER_Y += 42;

                    #endregion
                }
                else if (fragMethod.Equals("CID") || fragMethod.Equals("HCD") || fragMethod.Equals("SID"))
                {
                    #region CID or HCD or SID -> fragmentation method

                    if (IsGlobalIntensityMap)
                        intensity_normalization = global_intensity_normalization_factor;
                    else if (hasIntensityperMap)
                        intensity_normalization = currentFragmentIons.Max(a => a.Item5);

                    HeightRectNterm = 0;
                    HeightRectY = 0;
                    SPACER_Y = 0;
                    int offsetRectCID = offSetY;

                    #region Plot protein Sequence
                    List<Label> proteinCharsAndSpaces = new List<Label>();
                    PlotProteinSequence(PtnCharPositions, proteinCharsAndSpaces, leftOffsetProtein);
                    #endregion

                    #region Serie B
                    int countPrecursorChargesB = 0;
                    if (currentBFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentBFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesB, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectNterm = (countPrecursorChargesB + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesB * 9.5);

                        // create Background rect Serie B
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectNterm, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "B");
                    }
                    #endregion

                    #region Update protein position
                    double proteinY = HeightRectNterm + 10;
                    for (int i = 0; i < ProteinSequence.Length; i++)
                    {
                        MyCanvas.Children.Add(proteinCharsAndSpaces[i]);
                        Canvas.SetTop(proteinCharsAndSpaces[i], initialYLine + proteinY + offSetY);
                    }
                    #endregion

                    #region Serie Y
                    SPACER_Y = 0;
                    offSetY += (int)initialYLine + (int)proteinY;

                    int countPrecursorChargesY = 0;
                    if (currentYFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentYFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesY, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectY = (countPrecursorChargesY + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesY * 9.5);

                        // create Background rect Serie Y
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectY, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "Y");
                    }
                    #endregion

                    #region rectangle FragMethod
                    double height_rect = HeightRectNterm + HeightRectY + 70;
                    double font_pos_condition = HeightRectNterm + 90;
                    if (fragMethod.Equals("CID"))
                        RectCondition(initialYLine, offsetRectCID, height_rect, font_pos_condition, ref offSetY, "CID");
                    else if (fragMethod.Equals("HCD"))
                        RectCondition(initialYLine, offsetRectCID, height_rect, font_pos_condition, ref offSetY, "HCD");
                    else
                        RectCondition(initialYLine, offsetRectCID, height_rect, font_pos_condition, ref offSetY, "SID");
                    #endregion

                    #endregion
                }
                else if (fragMethod.Equals("ECD") || fragMethod.Equals("ETD"))
                {
                    #region ECD or ETD -> fragmentation method

                    if (IsGlobalIntensityMap)
                        intensity_normalization = global_intensity_normalization_factor;
                    else if (hasIntensityperMap)
                        intensity_normalization = currentFragmentIons.Max(a => a.Item5);

                    HeightRectC = 0;
                    HeightRectZ = 0;
                    SPACER_Y = 0;
                    int offsetRectECD_ETD = offSetY;

                    #region Plot protein Sequence
                    List<Label> proteinCharsAndSpaces = new List<Label>();
                    PlotProteinSequence(PtnCharPositions, proteinCharsAndSpaces, leftOffsetProtein);
                    #endregion

                    #region Serie C
                    int countPrecursorChargesC = 0;
                    if (currentCFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentCFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesC, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectC = (countPrecursorChargesC + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesC * 9.5);

                        // create Background rect Serie C
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectC, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "C");
                    }
                    #endregion

                    #region Update protein position
                    double proteinY = HeightRectC + 10;
                    for (int i = 0; i < ProteinSequence.Length; i++)
                    {
                        MyCanvas.Children.Add(proteinCharsAndSpaces[i]);
                        Canvas.SetTop(proteinCharsAndSpaces[i], initialYLine + proteinY + offSetY);
                    }
                    #endregion

                    #region Serie Z
                    SPACER_Y = 0;
                    offSetY += (int)initialYLine + (int)proteinY;

                    int countPrecursorChargesZ = 0;
                    if (currentZFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentZFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesZ, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectZ = (countPrecursorChargesZ + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesZ * 9.5);

                        // create Background rect Serie Z
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectZ, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "Z");
                    }
                    #endregion

                    #region rectangle FragMethod
                    double height_rect = HeightRectC + HeightRectZ + 70;
                    double font_pos_condition = HeightRectC + 90;
                    if (fragMethod.Equals("ECD"))
                        RectCondition(initialYLine, offsetRectECD_ETD, height_rect, font_pos_condition, ref offSetY, "ECD");
                    else
                        RectCondition(initialYLine, offsetRectECD_ETD, height_rect, font_pos_condition, ref offSetY, "ETD");
                    #endregion

                    SPACER_Y += 45;
                    #endregion
                }
                else
                {
                    #region fragmentation method

                    if (IsGlobalIntensityMap)
                        intensity_normalization = global_intensity_normalization_factor;
                    else if (hasIntensityperMap)
                        intensity_normalization = currentFragmentIons.Max(a => a.Item5);

                    HeightRectA = 0;
                    HeightRectNterm = 0;
                    HeightRectC = 0;
                    HeightRectX = 0;
                    HeightRectY = 0;
                    HeightRectZ = 0;
                    SPACER_Y = 0;
                    int offsetRectFragmentation = offSetY;

                    #region Plot protein Sequence
                    List<Label> proteinCharsAndSpaces = new List<Label>();
                    PlotProteinSequence(PtnCharPositions, proteinCharsAndSpaces, leftOffsetProtein);
                    #endregion

                    #region Serie A
                    int countPrecursorChargesA = 0;
                    if (currentAFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentAFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesA, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectA = (countPrecursorChargesA + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesA * 9.5);

                        // create Background rect Serie A
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectA, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "A");
                    }
                    #endregion

                    #region Serie B
                    SPACER_Y = 0;
                    offSetY += (int)initialYLine + (int)HeightRectA - FRAGMENT_ION_HEIGHT - 15;

                    int countPrecursorChargesB = 0;
                    if (currentBFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentBFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesB, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectNterm = (countPrecursorChargesB + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesB * 9.5);

                        // create Background rect Serie B
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectNterm, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "B");
                    }
                    #endregion

                    #region Serie C
                    SPACER_Y = 0;
                    if (HeightRectNterm != 0)
                    {
                        offSetY += (int)initialYLine + (int)HeightRectNterm - FRAGMENT_ION_HEIGHT - 15;
                    }
                    else
                    {
                        offSetY += (int)initialYLine - FRAGMENT_ION_HEIGHT - 15;
                    }

                    int countPrecursorChargesC = 0;
                    if (currentCFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentCFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesC, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectC = (countPrecursorChargesC + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesC * 9.5);

                        // create Background rect Serie C
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectC, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "C");
                    }
                    #endregion

                    #region Update protein position

                    double proteinY = HeightRectC - initialYLine + 2 * FRAGMENT_ION_HEIGHT + 10;
                    for (int i = 0; i < ProteinSequence.Length; i++)
                    {
                        MyCanvas.Children.Add(proteinCharsAndSpaces[i]);
                        Canvas.SetTop(proteinCharsAndSpaces[i], initialYLine + proteinY + offSetY);
                    }
                    #endregion

                    #region Serie X
                    SPACER_Y = 0;
                    offSetY += (int)initialYLine + (int)proteinY + 15;

                    int countPrecursorChargesX = 0;
                    if (currentXFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentXFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesX, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectX = (countPrecursorChargesX + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesX * 9.5);

                        // create Background rect Serie X
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectX, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "X");
                    }
                    #endregion

                    #region Serie Y
                    SPACER_Y = 0;
                    offSetY += (int)initialYLine + (int)HeightRectX - FRAGMENT_ION_HEIGHT - 15;

                    int countPrecursorChargesY = 0;
                    if (currentYFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentYFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesY, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectY = (countPrecursorChargesY + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesY * 9.5);

                        // create Background rect Serie Y
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectY, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "Y");
                    }
                    #endregion

                    #region Serie Z
                    SPACER_Y = 0;
                    offSetY += (int)initialYLine + (int)HeightRectY - FRAGMENT_ION_HEIGHT - 15;

                    int countPrecursorChargesZ = 0;
                    if (currentZFragmentIons.Count > 0)
                    {
                        PlotFragmentIons(initialYLine, offSetY, PrecursorChargesOrActivationLevels, currentZFragmentIons, proteinCharsAndSpaces, ref countPrecursorChargesZ, countCurrentFragMethod, hasIntensityperMap, intensity_normalization);
                        HeightRectZ = (countPrecursorChargesZ + 1) * FRAGMENT_ION_HEIGHT + (countPrecursorChargesZ * 9.5);

                        // create Background rect Serie Z
                        CreateSerieRectangle(initialXLine, initialYLine, COLOR_SERIES_RECTANGLE, HeightRectZ, backgroundColor, blackBrush, proteinCharsAndSpaces, offSetY, "Z");
                    }
                    #endregion

                    #region rectangle FragMethod
                    double height_rect = (HeightRectA + HeightRectNterm + HeightRectC + HeightRectX + HeightRectY + HeightRectZ + 145);
                    double font_pos_condition = HeightRectA + HeightRectNterm + HeightRectC + 130;
                    if (showPrecursorChargeState)
                        RectCondition(initialYLine, offsetRectFragmentation, height_rect, font_pos_condition, ref offSetY, "Precursor Charge States");
                    else if (showActivationLevel)
                        RectCondition(initialYLine, offsetRectFragmentation, height_rect, font_pos_condition, ref offSetY, "Activation Levels");
                    else
                        RectCondition(initialYLine, offsetRectFragmentation, height_rect, font_pos_condition, ref offSetY, "Replicates");
                    #endregion

                    #endregion
                }

                #region Plot Legend Study condition -> labels
                SolidColorBrush labelBrush_PrecursorChargeState = new SolidColorBrush(Colors.Black);
                Label StudyConditionLabel = new Label();
                StudyConditionLabel.FontFamily = new FontFamily("Courier New");
                StudyConditionLabel.FontWeight = FontWeights.Bold;
                StudyConditionLabel.FontSize = FONTSIZE_PROTEINSEQUENCE;
                StudyConditionLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);

                StudyConditionLabel.Foreground = labelBrush_PrecursorChargeState;
                StudyConditionLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                MyCanvas.Children.Add(StudyConditionLabel);
                Canvas.SetLeft(StudyConditionLabel, 100);
                Canvas.SetTop(StudyConditionLabel, offSetY);

                bool printIntensity = false;

                if (hasIntensityperMap)
                {
                    if (IsGlobalIntensityMap && countCurrentFragMethod == FragMethodsWithPrecursorChargeOrActivationLevelDict.Count - 1)
                        printIntensity = true;
                    else if (!IsGlobalIntensityMap)
                        printIntensity = true;
                }

                if (this.HasMergeConditions)
                    printIntensity = true;

                double ColorsTop = offSetY;
                double GridWidth = 540;

                #region Plot Residue cleavages table
                if (!IsGlobalIntensityMap)
                    CreateResidueCleavagesTable(PrecursorChargesOrActivationLevels, currentFragmentIons, ref offSetY, out GridWidth);
                #endregion

                if (this.HasMergeConditions)
                {
                    #region Insity label

                    StudyConditionLabel.Content = "Cleavage frequency:";

                    #region Start Intensity Label
                    SolidColorBrush labelBrush_IntensityLabel = new SolidColorBrush(Colors.Gray);
                    Label StartIntensityLabel = new Label();
                    StartIntensityLabel.FontFamily = new FontFamily("Courier New");
                    StartIntensityLabel.FontWeight = FontWeights.Bold;
                    StartIntensityLabel.FontSize = 20;
                    StartIntensityLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                    StartIntensityLabel.Content = "0";
                    StartIntensityLabel.Foreground = labelBrush_IntensityLabel;
                    StartIntensityLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                    MyCanvas.Children.Add(StartIntensityLabel);
                    Canvas.SetLeft(StartIntensityLabel, 90 + 27 * (StudyConditionLabel.Content.ToString().Length));
                    Canvas.SetTop(StartIntensityLabel, ColorsTop - 10);
                    #endregion

                    GridWidth /= 5;
                    double accumulativeGridWidth = 0;
                    for (double countGradient = 0.1; countGradient < 1; countGradient += 0.20)
                        accumulativeGridWidth = CreateIntensityBox(countCurrentFragMethod, StudyConditionLabel, ColorsTop, GridWidth, accumulativeGridWidth, countGradient);

                    #region End Intensity Label
                    Label EndIntensityLabel = new Label();
                    EndIntensityLabel.FontFamily = new FontFamily("Courier New");
                    EndIntensityLabel.FontWeight = FontWeights.Bold;
                    EndIntensityLabel.FontSize = 20;
                    EndIntensityLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                    if (this.HasMergeConditions)
                        EndIntensityLabel.Content = intensity_normalization.ToString();
                    else
                        EndIntensityLabel.Content = intensity_normalization.ToString("0.0e+0");
                    EndIntensityLabel.Foreground = labelBrush_IntensityLabel;
                    EndIntensityLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                    MyCanvas.Children.Add(EndIntensityLabel);
                    Canvas.SetLeft(EndIntensityLabel, 20 + 27 * (StudyConditionLabel.Content.ToString().Length) + (GridWidth * 6) - (27 * EndIntensityLabel.Content.ToString().Length));
                    Canvas.SetTop(EndIntensityLabel, ColorsTop - 10);
                    #endregion

                    #endregion

                    #region Residue Cleavages label

                    StringBuilder _content = new StringBuilder();
                    _content.Append("Residue Cleavages: ");
                    List<int> positions = currentFragmentIons.Select(a => a.Item4).Distinct().ToList();
                    _content.Append((((double)positions.Count / (double)ProteinSequence.Length) * 100).ToString("0.00"));
                    _content.Append("%");

                    offSetY += 90;
                    Label ResidueCleavageLabel = new Label();
                    ResidueCleavageLabel.FontFamily = new FontFamily("Courier New");
                    ResidueCleavageLabel.FontWeight = FontWeights.Bold;
                    ResidueCleavageLabel.FontSize = FONTSIZE_PROTEINSEQUENCE;
                    ResidueCleavageLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                    ResidueCleavageLabel.Content = _content.ToString();
                    ResidueCleavageLabel.Foreground = labelBrush_PrecursorChargeState;
                    ResidueCleavageLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                    MyCanvas.Children.Add(ResidueCleavageLabel);
                    Canvas.SetLeft(ResidueCleavageLabel, 100);
                    Canvas.SetTop(ResidueCleavageLabel, offSetY);
                    offSetY += 30;
                    #endregion
                }
                else if (isPrecursorChargeState && !isActivationLevel && !isFragmentationMethod)
                {
                    if (printIntensity)
                    {
                        #region Insity label

                        StudyConditionLabel.Content = "Intensity scale:";

                        #region Start Intensity Label
                        SolidColorBrush labelBrush_IntensityLabel = new SolidColorBrush(Colors.Gray);
                        Label StartIntensityLabel = new Label();
                        StartIntensityLabel.FontFamily = new FontFamily("Courier New");
                        StartIntensityLabel.FontWeight = FontWeights.Bold;
                        StartIntensityLabel.FontSize = 20;
                        StartIntensityLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                        StartIntensityLabel.Content = "0";
                        StartIntensityLabel.Foreground = labelBrush_IntensityLabel;
                        StartIntensityLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        MyCanvas.Children.Add(StartIntensityLabel);
                        Canvas.SetLeft(StartIntensityLabel, 90 + 27 * (StudyConditionLabel.Content.ToString().Length));
                        Canvas.SetTop(StartIntensityLabel, ColorsTop - 10);
                        #endregion

                        GridWidth /= 5;
                        double accumulativeGridWidth = 0;
                        for (double countGradient = 0.1; countGradient < 1; countGradient += 0.20)
                            accumulativeGridWidth = CreateIntensityBox(countCurrentFragMethod, StudyConditionLabel, ColorsTop, GridWidth, accumulativeGridWidth, countGradient);

                        #region End Intensity Label
                        Label EndIntensityLabel = new Label();
                        EndIntensityLabel.FontFamily = new FontFamily("Courier New");
                        EndIntensityLabel.FontWeight = FontWeights.Bold;
                        EndIntensityLabel.FontSize = 20;
                        EndIntensityLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                        EndIntensityLabel.Content = intensity_normalization.ToString("0.0e+0");
                        EndIntensityLabel.Foreground = labelBrush_IntensityLabel;
                        EndIntensityLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        MyCanvas.Children.Add(EndIntensityLabel);
                        Canvas.SetLeft(EndIntensityLabel, 20 + 27 * (StudyConditionLabel.Content.ToString().Length) + (GridWidth * 5));
                        Canvas.SetTop(EndIntensityLabel, ColorsTop - 10);
                        #endregion

                        #endregion
                    }
                    else if (!hasIntensityperMap)
                    {
                        #region Plot Legend Precursor Charge State
                        StudyConditionLabel.Content = "Precursor Charge State:";

                        #region Plot Legend precursor charge state -> boxes

                        for (int i = 0; i < PrecursorChargesOrActivationLevels.Count; i++)
                        {
                            // Create a Rectangle  
                            Rectangle PrecursorChargeRetangle = new Rectangle();
                            PrecursorChargeRetangle.Height = 20;
                            PrecursorChargeRetangle.Width = 20;
                            // Set Rectangle's width and color  
                            PrecursorChargeRetangle.StrokeThickness = 0.5;

                            // Fill rectangle with color 
                            int _index = Array.FindIndex(PrecursorChargeStatesOrActivationLevelsColors, a => a.Equals(PrecursorChargesOrActivationLevels[i]));
                            PrecursorChargeRetangle.Fill = FRAGMENT_ION_LINE_COLORS[_index];
                            // Add Rectangle to the Grid.  
                            MyCanvas.Children.Add(PrecursorChargeRetangle);
                            int sumXoffset = 0;
                            for (int countOffset = 0; countOffset < i; countOffset++) sumXoffset += PrecursorChargesOrActivationLevels[countOffset].Length + 3;
                            Canvas.SetLeft(PrecursorChargeRetangle, 100 + 27 * (StudyConditionLabel.Content.ToString().Length + sumXoffset));
                            Canvas.SetTop(PrecursorChargeRetangle, ColorsTop + 15);
                            Canvas.SetZIndex(PrecursorChargeRetangle, -1);

                            Label precursorChargeStateLabelEachOne = new Label();
                            precursorChargeStateLabelEachOne.FontFamily = new FontFamily("Courier New");
                            precursorChargeStateLabelEachOne.FontWeight = FontWeights.Bold;
                            precursorChargeStateLabelEachOne.FontSize = FONTSIZE_PROTEINSEQUENCE;
                            precursorChargeStateLabelEachOne.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                            precursorChargeStateLabelEachOne.Content = PrecursorChargesOrActivationLevels[i] + "+";
                            precursorChargeStateLabelEachOne.Foreground = labelBrush_PrecursorChargeState;
                            precursorChargeStateLabelEachOne.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                            MyCanvas.Children.Add(precursorChargeStateLabelEachOne);
                            sumXoffset = 0;
                            for (int countOffset = 0; countOffset < i; countOffset++) sumXoffset += PrecursorChargesOrActivationLevels[countOffset].Length + 3;
                            Canvas.SetLeft(precursorChargeStateLabelEachOne, 120 + 27 * (StudyConditionLabel.Content.ToString().Length + sumXoffset));
                            Canvas.SetTop(precursorChargeStateLabelEachOne, ColorsTop);
                        }
                        #endregion

                        #endregion
                    }
                }
                else if (isActivationLevel && !isPrecursorChargeState && !isFragmentationMethod)
                {
                    if (printIntensity)
                    {
                        #region Insity label
                        StudyConditionLabel.Content = "Intensity scale:";

                        #region Start Intensity Label
                        SolidColorBrush labelBrush_IntensityLabel = new SolidColorBrush(Colors.Gray);
                        Label StartIntensityLabel = new Label();
                        StartIntensityLabel.FontFamily = new FontFamily("Courier New");
                        StartIntensityLabel.FontWeight = FontWeights.Bold;
                        StartIntensityLabel.FontSize = 20;
                        StartIntensityLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                        StartIntensityLabel.Content = "0";
                        StartIntensityLabel.Foreground = labelBrush_IntensityLabel;
                        StartIntensityLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        MyCanvas.Children.Add(StartIntensityLabel);
                        Canvas.SetLeft(StartIntensityLabel, 90 + 27 * (StudyConditionLabel.Content.ToString().Length));
                        Canvas.SetTop(StartIntensityLabel, ColorsTop - 10);
                        #endregion

                        GridWidth /= 5;
                        double accumulativeGridWidth = 0;
                        for (double countGradient = 0.1; countGradient < 1; countGradient += 0.20)
                            accumulativeGridWidth = CreateIntensityBox(countCurrentFragMethod, StudyConditionLabel, ColorsTop, GridWidth, accumulativeGridWidth, countGradient);

                        #region End Intensity Label
                        Label EndIntensityLabel = new Label();
                        EndIntensityLabel.FontFamily = new FontFamily("Courier New");
                        EndIntensityLabel.FontWeight = FontWeights.Bold;
                        EndIntensityLabel.FontSize = 20;
                        EndIntensityLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                        EndIntensityLabel.Content = intensity_normalization.ToString("0.0e+0"); ;
                        EndIntensityLabel.Foreground = labelBrush_IntensityLabel;
                        EndIntensityLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        MyCanvas.Children.Add(EndIntensityLabel);
                        Canvas.SetLeft(EndIntensityLabel, 20 + 27 * (StudyConditionLabel.Content.ToString().Length) + (GridWidth * 5));
                        Canvas.SetTop(EndIntensityLabel, ColorsTop - 10);
                        #endregion

                        #endregion
                    }
                    else if (!hasIntensityperMap)
                    {
                        #region Plot Legend activation level
                        StudyConditionLabel.Content = "Activation Level:";

                        #region Plot Legend activation level -> boxes

                        for (int i = 0; i < PrecursorChargesOrActivationLevels.Count; i++)
                        {
                            // Create a Rectangle  
                            Rectangle ActivationLevelRetangle = new Rectangle();
                            ActivationLevelRetangle.Height = 20;
                            ActivationLevelRetangle.Width = 20;
                            // Set Rectangle's width and color  
                            ActivationLevelRetangle.StrokeThickness = 0.5;

                            // Fill rectangle with color 
                            int _index = Array.FindIndex(PrecursorChargeStatesOrActivationLevelsColors, a => a.Equals(PrecursorChargesOrActivationLevels[i]));
                            ActivationLevelRetangle.Fill = FRAGMENT_ION_LINE_COLORS[_index];
                            // Add Rectangle to the Grid.  
                            MyCanvas.Children.Add(ActivationLevelRetangle);
                            int sumXoffset = 0;
                            for (int countOffset = 0; countOffset < i; countOffset++) sumXoffset += PrecursorChargesOrActivationLevels[countOffset].Length + 2;
                            Canvas.SetLeft(ActivationLevelRetangle, 100 + 27 * (StudyConditionLabel.Content.ToString().Length + sumXoffset));
                            Canvas.SetTop(ActivationLevelRetangle, ColorsTop + 15);
                            Canvas.SetZIndex(ActivationLevelRetangle, -1);

                            Label precursorChargeStateLabelEachOne = new Label();
                            precursorChargeStateLabelEachOne.FontFamily = new FontFamily("Courier New");
                            precursorChargeStateLabelEachOne.FontWeight = FontWeights.Bold;
                            precursorChargeStateLabelEachOne.FontSize = FONTSIZE_PROTEINSEQUENCE;
                            precursorChargeStateLabelEachOne.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                            precursorChargeStateLabelEachOne.Content = PrecursorChargesOrActivationLevels[i];
                            precursorChargeStateLabelEachOne.Foreground = labelBrush_PrecursorChargeState;
                            precursorChargeStateLabelEachOne.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                            MyCanvas.Children.Add(precursorChargeStateLabelEachOne);
                            sumXoffset = 0;
                            for (int countOffset = 0; countOffset < i; countOffset++) sumXoffset += PrecursorChargesOrActivationLevels[countOffset].Length + 2;
                            Canvas.SetLeft(precursorChargeStateLabelEachOne, 120 + 27 * (StudyConditionLabel.Content.ToString().Length + sumXoffset));
                            Canvas.SetTop(precursorChargeStateLabelEachOne, ColorsTop);
                        }
                        #endregion

                        #endregion
                    }
                }
                else if (isFragmentationMethod && !isPrecursorChargeState && !isActivationLevel)
                {
                    if (printIntensity)
                    {
                        #region Insity label
                        StudyConditionLabel.Content = "Intensity scale:";

                        #region Start Intensity Label
                        SolidColorBrush labelBrush_IntensityLabel = new SolidColorBrush(Colors.Gray);
                        Label StartIntensityLabel = new Label();
                        StartIntensityLabel.FontFamily = new FontFamily("Courier New");
                        StartIntensityLabel.FontWeight = FontWeights.Bold;
                        StartIntensityLabel.FontSize = 20;
                        StartIntensityLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                        StartIntensityLabel.Content = "0";
                        StartIntensityLabel.Foreground = labelBrush_IntensityLabel;
                        StartIntensityLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        MyCanvas.Children.Add(StartIntensityLabel);
                        Canvas.SetLeft(StartIntensityLabel, 90 + 27 * (StudyConditionLabel.Content.ToString().Length));
                        Canvas.SetTop(StartIntensityLabel, ColorsTop - 10);
                        #endregion

                        GridWidth /= 5;
                        double accumulativeGridWidth = 0;
                        for (double countGradient = 0.1; countGradient < 1; countGradient += 0.20)
                            accumulativeGridWidth = CreateIntensityBox(countCurrentFragMethod, StudyConditionLabel, ColorsTop, GridWidth, accumulativeGridWidth, countGradient);

                        #region End Intensity Label
                        Label EndIntensityLabel = new Label();
                        EndIntensityLabel.FontFamily = new FontFamily("Courier New");
                        EndIntensityLabel.FontWeight = FontWeights.Bold;
                        EndIntensityLabel.FontSize = 20;
                        EndIntensityLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                        EndIntensityLabel.Content = intensity_normalization.ToString("0.0e+0"); ;
                        EndIntensityLabel.Foreground = labelBrush_IntensityLabel;
                        EndIntensityLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        MyCanvas.Children.Add(EndIntensityLabel);
                        Canvas.SetLeft(EndIntensityLabel, 20 + 27 * (StudyConditionLabel.Content.ToString().Length) + (GridWidth * 5));
                        Canvas.SetTop(EndIntensityLabel, ColorsTop - 10);
                        #endregion

                        #endregion
                    }
                    else if (!hasIntensityperMap)
                    {
                        #region Plot Legend Fragmentation Method
                        StudyConditionLabel.Content = "Fragmentation Method:";

                        #region Plot Legend Fragmentation Method -> boxes

                        for (int i = 0; i < PrecursorChargesOrActivationLevels.Count; i++)
                        {
                            // Create a Rectangle  
                            Rectangle FragmentationMethodRetangle = new Rectangle();
                            FragmentationMethodRetangle.Height = 20;
                            FragmentationMethodRetangle.Width = 20;
                            // Set Rectangle's width and color  
                            FragmentationMethodRetangle.StrokeThickness = 0.5;

                            // Fill rectangle with color 
                            int _index = Array.FindIndex(PrecursorChargeStatesOrActivationLevelsColors, a => a.Equals(PrecursorChargesOrActivationLevels[i]));
                            FragmentationMethodRetangle.Fill = FRAGMENT_ION_LINE_COLORS[_index];
                            // Add Rectangle to the Grid.  
                            MyCanvas.Children.Add(FragmentationMethodRetangle);
                            int sumXoffset = 0;
                            for (int countOffset = 0; countOffset < i; countOffset++) sumXoffset += PrecursorChargesOrActivationLevels[countOffset].Length + 2;
                            Canvas.SetLeft(FragmentationMethodRetangle, 100 + 27 * (StudyConditionLabel.Content.ToString().Length + sumXoffset));
                            Canvas.SetTop(FragmentationMethodRetangle, ColorsTop + 15);
                            Canvas.SetZIndex(FragmentationMethodRetangle, -1);

                            Label precursorChargeStateLabelEachOne = new Label();
                            precursorChargeStateLabelEachOne.FontFamily = new FontFamily("Courier New");
                            precursorChargeStateLabelEachOne.FontWeight = FontWeights.Bold;
                            precursorChargeStateLabelEachOne.FontSize = FONTSIZE_PROTEINSEQUENCE;
                            precursorChargeStateLabelEachOne.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                            precursorChargeStateLabelEachOne.Content = PrecursorChargesOrActivationLevels[i];
                            precursorChargeStateLabelEachOne.Foreground = labelBrush_PrecursorChargeState;
                            precursorChargeStateLabelEachOne.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                            MyCanvas.Children.Add(precursorChargeStateLabelEachOne);

                            sumXoffset = 0;
                            for (int countOffset = 0; countOffset < i; countOffset++) sumXoffset += PrecursorChargesOrActivationLevels[countOffset].Length + 2;
                            Canvas.SetLeft(precursorChargeStateLabelEachOne, 120 + 27 * (StudyConditionLabel.Content.ToString().Length + sumXoffset));
                            Canvas.SetTop(precursorChargeStateLabelEachOne, ColorsTop);
                        }
                        #endregion

                        #endregion
                    }
                }

                #endregion
                countCurrentFragMethod++;
            }

            if (PtnCharPositions.Count == 0) return;
            HighestX = initialXLine + PtnCharPositions[PtnCharPositions.Count - 1] - 40;
            HighestY = initialYLine + offSetY - 10;
            this.SetCanvasScrollBarSize(HighestX + 100, HighestY + 100);

            Label proteinSeqInformationLabel = new Label();
            #region Plot Legend Protein Sequence Information
            if (!String.IsNullOrEmpty(ProteinSequenceInformation))
            {
                SolidColorBrush labelBrush_PrecursorChargeState = new SolidColorBrush(Colors.Black);
                proteinSeqInformationLabel.FontFamily = new FontFamily("Courier New");
                proteinSeqInformationLabel.FontWeight = FontWeights.Bold;
                proteinSeqInformationLabel.FontSize = FONTSIZE_PROTEINSEQUENCE;
                proteinSeqInformationLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                proteinSeqInformationLabel.Content = "Sequence information: " + ProteinSequenceInformation;
                proteinSeqInformationLabel.Foreground = labelBrush_PrecursorChargeState;
                proteinSeqInformationLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                MyCanvas.Children.Add(proteinSeqInformationLabel);
                Canvas.SetLeft(proteinSeqInformationLabel, 100);
                Canvas.SetTop(proteinSeqInformationLabel, HighestY);
            }
            #endregion

            #region plot aminoacid number on the top of the sequence

            //First aminoacid
            Label numberAATop1 = new Label();
            numberAATop1.FontFamily = new FontFamily("Courier New");
            numberAATop1.FontWeight = FontWeights.Bold;
            numberAATop1.FontSize = FONTSIZE_AMINOACID_POSITION;
            numberAATop1.Content = 1;
            numberAATop1.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            MyCanvas.Children.Add(numberAATop1);
            double initialXProtein = PtnCharPositions[0];
            Canvas.SetLeft(numberAATop1, initialXProtein);
            Canvas.SetTop(numberAATop1, initialYLine - 50);

            // Create a Rectangle  
            Rectangle AANumberRectangle = new Rectangle();
            AANumberRectangle.Height = 30;
            AANumberRectangle.Width = PtnCharPositions[PtnCharPositions.Count - 1] + 10;
            // Set Rectangle's width and color  
            AANumberRectangle.StrokeThickness = 0.5;

            // Fill rectangle with blue color  
            AANumberRectangle.Fill = new SolidColorBrush(COLOR_SERIES_RECTANGLE);
            // Add Rectangle to the Grid.  
            MyCanvas.Children.Add(AANumberRectangle);
            Canvas.SetLeft(AANumberRectangle, initialXLine + 40);
            Canvas.SetTop(AANumberRectangle, initialYLine - 48);

            //float offsetSPACER = 1.5f;
            for (int i = 0; i <= ProteinSequence.Length; i += 50)
            {
                if (i % 50 == 0 && i > 0)
                {
                    Label numberAATop = new Label();
                    numberAATop.FontFamily = new FontFamily("Courier New");
                    numberAATop.FontWeight = FontWeights.Bold;
                    numberAATop.FontSize = FONTSIZE_AMINOACID_POSITION;
                    numberAATop.Content = i;
                    numberAATop.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                    MyCanvas.Children.Add(numberAATop);
                    Canvas.SetLeft(numberAATop, PtnCharPositions[i - 1]);
                    Canvas.SetTop(numberAATop, initialYLine - 50);
                }
            }

            if (ProteinSequence.Length % 50 != 0)//Print the last AAnumber
            {
                Label numberAATop = new Label();
                numberAATop.FontFamily = new FontFamily("Courier New");
                numberAATop.FontWeight = FontWeights.Bold;
                numberAATop.FontSize = FONTSIZE_AMINOACID_POSITION;
                numberAATop.Content = ProteinSequence.Length;
                numberAATop.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                MyCanvas.Children.Add(numberAATop);
                Canvas.SetLeft(numberAATop, PtnCharPositions[PtnCharPositions.Count - 1]);
                Canvas.SetTop(numberAATop, initialYLine - 50);
            }
            #endregion

            #endregion

            #endregion
        }

        private double CreateIntensityBox(int countCurrentFragMethod, Label StudyConditionLabel, double ColorsTop, double GridWidth, double accumulativeGridWidth, double countGradient)
        {
            // Create a Rectangle
            Rectangle PrecursorChargeRetangle = new Rectangle();
            PrecursorChargeRetangle.Height = 20;
            PrecursorChargeRetangle.Width = GridWidth;
            PrecursorChargeRetangle.StrokeThickness = 2;
            PrecursorChargeRetangle.Stroke = new SolidColorBrush(Colors.LightGray);
            // Set Rectangle's width and color  
            SolidColorBrush currentColor = null;
            if (IsGlobalIntensityMap)
                currentColor = new SolidColorBrush(FRAGMENT_ION_LINE_COLORS[0].Color);
            else
                currentColor = new SolidColorBrush(FRAGMENT_ION_LINE_COLORS[countCurrentFragMethod].Color);
            currentColor.Opacity = countGradient;
            PrecursorChargeRetangle.Fill = currentColor;

            if (countGradient > 0.1)
                accumulativeGridWidth += GridWidth;
            MyCanvas.Children.Add(PrecursorChargeRetangle);
            Canvas.SetLeft(PrecursorChargeRetangle, 100 + 27 * (StudyConditionLabel.Content.ToString().Length) + accumulativeGridWidth);
            Canvas.SetTop(PrecursorChargeRetangle, ColorsTop + 25);
            Canvas.SetZIndex(PrecursorChargeRetangle, -1);
            return accumulativeGridWidth;
        }

        private void CreateResidueCleavagesTable(List<string> PrecursorChargesOrActivationLevels, List<(string, string, string, int, double)> currentFragmentIons, ref int offsetY, out double GridWidth)
        {
            List<(string, double)> precursorChargeStatesOrActivationLevelsOrFragMethods = new List<(string, double)>();

            foreach (string precursorChargeOrActivationLevel in PrecursorChargesOrActivationLevels)
            {
                List<(string, string, string, int, double)> currentPrecursorCharge = null;

                if (isFragmentationMethod && (showPrecursorChargeState || showActivationLevel || showReplicates))
                    currentPrecursorCharge = currentFragmentIons.Where(a => a.Item1.Equals(precursorChargeOrActivationLevel)).ToList();
                else
                    currentPrecursorCharge = currentFragmentIons.Where(a => a.Item2.Equals(precursorChargeOrActivationLevel)).ToList();

                List<int> positions = currentPrecursorCharge.Select(a => a.Item4).Distinct().ToList();
                precursorChargeStatesOrActivationLevelsOrFragMethods.Add((precursorChargeOrActivationLevel, ((double)positions.Count / (double)ProteinSequence.Length) * 100));

            }

            int maxWidth = precursorChargeStatesOrActivationLevelsOrFragMethods.Select(a => a.Item1.Length).Max();
            if (maxWidth < 6) maxWidth = 5;

            // Create the Grid
            Grid DynamicGrid = new Grid();
            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Center;
            DynamicGrid.VerticalAlignment = VerticalAlignment.Center;

            // Create Columns
            ColumnDefinition gridCol1 = new ColumnDefinition();
            gridCol1.Width = new GridLength(600);
            DynamicGrid.ColumnDefinitions.Add(gridCol1);

            for (int i = 0; i < precursorChargeStatesOrActivationLevelsOrFragMethods.Count; i++)
            {
                double gridLength = maxWidth * ROWWIDTH_TABLE;
                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Width = new GridLength(gridLength);
                DynamicGrid.ColumnDefinitions.Add(gridCol);
            }

            // Create Rows
            // Title
            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.Height = new GridLength(45);
            //Content
            RowDefinition gridRow2 = new RowDefinition();
            gridRow2.Height = new GridLength(45);
            DynamicGrid.RowDefinitions.Add(gridRow1);
            DynamicGrid.RowDefinitions.Add(gridRow2);

            for (int i = 0; i < precursorChargeStatesOrActivationLevelsOrFragMethods.Count; i++)
            {
                //Borders
                Rectangle rectRowColor = new Rectangle();
                rectRowColor.StrokeThickness = 1;
                rectRowColor.Stroke = new SolidColorBrush(Colors.Black);
                Grid.SetRow(rectRowColor, 0);
                Grid.SetColumn(rectRowColor, (i + 1));
                DynamicGrid.Children.Add(rectRowColor);

                // Add row header
                TextBlock txtBlock = new TextBlock();
                if (isPrecursorChargeState)
                    txtBlock.Text = precursorChargeStatesOrActivationLevelsOrFragMethods[i].Item1 + "+";
                else
                    txtBlock.Text = precursorChargeStatesOrActivationLevelsOrFragMethods[i].Item1;
                txtBlock.FontFamily = new FontFamily("Courier New");
                txtBlock.FontSize = FONTSIZE_PROTEINSEQUENCE;
                txtBlock.FontWeight = FontWeights.Bold;
                txtBlock.Foreground = new SolidColorBrush(Colors.Black);
                txtBlock.VerticalAlignment = VerticalAlignment.Center;
                txtBlock.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(txtBlock, 0);
                Grid.SetColumn(txtBlock, (i + 1));
                DynamicGrid.Children.Add(txtBlock);
            }

            Rectangle rectRowResidueCleavagesTitle = new Rectangle();
            rectRowResidueCleavagesTitle.StrokeThickness = 1;
            rectRowResidueCleavagesTitle.Stroke = new SolidColorBrush(Colors.Black);
            Grid.SetRow(rectRowResidueCleavagesTitle, 0);
            Grid.SetColumn(rectRowResidueCleavagesTitle, 0);
            DynamicGrid.Children.Add(rectRowResidueCleavagesTitle);

            Rectangle rectRowResidueCleavagesLabel = new Rectangle();
            rectRowResidueCleavagesLabel.StrokeThickness = 1;
            rectRowResidueCleavagesLabel.Stroke = new SolidColorBrush(Colors.Black);
            rectRowResidueCleavagesLabel.Fill = new SolidColorBrush(Colors.LightGray);
            Grid.SetRow(rectRowResidueCleavagesLabel, 1);
            Grid.SetColumn(rectRowResidueCleavagesLabel, 0);
            DynamicGrid.Children.Add(rectRowResidueCleavagesLabel);

            TextBlock txtBlock_content = new TextBlock();
            txtBlock_content.Text = "Residue cleavages (%)";
            txtBlock_content.FontFamily = new FontFamily("Courier New");
            txtBlock_content.FontSize = FONTSIZE_PROTEINSEQUENCE;
            txtBlock_content.FontWeight = FontWeights.Bold;
            txtBlock_content.Foreground = new SolidColorBrush(Colors.Black);
            txtBlock_content.VerticalAlignment = VerticalAlignment.Center;
            txtBlock_content.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(txtBlock_content, 1);
            Grid.SetColumn(txtBlock_content, 0);
            DynamicGrid.Children.Add(txtBlock_content);

            for (int i = 0; i < precursorChargeStatesOrActivationLevelsOrFragMethods.Count; i++)
            {
                //Background color and borders
                Rectangle rectRowColor = new Rectangle();
                rectRowColor.StrokeThickness = 1;
                rectRowColor.Stroke = new SolidColorBrush(Colors.Black);
                rectRowColor.Fill = new SolidColorBrush(Colors.LightGray);
                Grid.SetRow(rectRowColor, 1);
                Grid.SetColumn(rectRowColor, (i + 1));
                DynamicGrid.Children.Add(rectRowColor);

                // Add row text
                TextBlock txtBlock = new TextBlock();
                txtBlock.Text = precursorChargeStatesOrActivationLevelsOrFragMethods[i].Item2.ToString("0.00");
                txtBlock.FontFamily = new FontFamily("Courier New");
                txtBlock.FontSize = FONTSIZE_PROTEINSEQUENCE;
                txtBlock.FontWeight = FontWeights.Bold;
                txtBlock.Foreground = new SolidColorBrush(Colors.Black);
                txtBlock.VerticalAlignment = VerticalAlignment.Center;
                txtBlock.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(txtBlock, 1);
                Grid.SetColumn(txtBlock, (i + 1));
                DynamicGrid.Children.Add(txtBlock);
            }

            MyCanvas.Children.Add(DynamicGrid);
            Canvas.SetLeft(DynamicGrid, 100);
            Canvas.SetTop(DynamicGrid, offsetY + 70);

            offsetY += 130;

            double _width = 170;
            for (int count = 1; count < DynamicGrid.ColumnDefinitions.Count; count++)
                _width += DynamicGrid.ColumnDefinitions[count].Width.Value;

            GridWidth = _width;
        }

        private void RectCondition(double initialYLine, int offsetCondition, double Height_Rect, double font_pos_condition, ref int offsetY, string nameCondition)
        {
            Rectangle Rectangle_Condition = new Rectangle();
            Rectangle_Condition.Height = Height_Rect;
            Rectangle_Condition.Width = 80;
            // Create a blue and a black Brush  
            SolidColorBrush backgroundColorCondition = new SolidColorBrush();
            backgroundColorCondition.Color = Colors.Gray;

            // Fill rectangle with blue color  
            Rectangle_Condition.Fill = backgroundColorCondition;
            // Add Rectangle to the Grid.  
            MyCanvas.Children.Add(Rectangle_Condition);
            Canvas.SetLeft(Rectangle_Condition, 15);
            Canvas.SetTop(Rectangle_Condition, initialYLine + offsetCondition);
            Canvas.SetZIndex(Rectangle_Condition, -1);

            Label label_condition = new Label();
            label_condition.FontFamily = new FontFamily("Courier New");
            label_condition.FontWeight = FontWeights.SemiBold;
            label_condition.FontSize = FONTSIZE_BOX_CONDITION_SERIE - 20;
            label_condition.Content = nameCondition;
            label_condition.Foreground = new SolidColorBrush(Colors.White);
            label_condition.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            label_condition.RenderTransform = new RotateTransform(270);
            MyCanvas.Children.Add(label_condition);
            Canvas.SetLeft(label_condition, 30);
            if (isFragmentationMethod && showPrecursorChargeState)
            {
                Canvas.SetTop(label_condition, initialYLine + font_pos_condition + offsetCondition + 178.5);
            }
            else if (isFragmentationMethod && showActivationLevel)
            {
                Canvas.SetTop(label_condition, initialYLine + font_pos_condition + offsetCondition + 118.5);
            }
            else if (isFragmentationMethod && showReplicates)
            {
                Canvas.SetTop(label_condition, initialYLine + font_pos_condition + offsetCondition + 28.5);
            }
            else
            {
                label_condition.FontSize = FONTSIZE_BOX_CONDITION_SERIE;
                Canvas.SetLeft(label_condition, 20);
                Canvas.SetTop(label_condition, initialYLine + font_pos_condition + offsetCondition);
            }

            offsetY = (int)Canvas.GetTop(Rectangle_Condition) + (int)Rectangle_Condition.Height + 10;
        }

        private void CreateSerieRectangle(double initialXLine, double initialYLine, Color COLOR_SERIES_RECTANGLE, double HeightRect, SolidColorBrush backgroundColor, SolidColorBrush blackBrush, List<Label> proteinCharsAndSpaces, double offSetY, string serie)
        {
            // Create a Rectangle  
            Rectangle ABCSeriesFullRectangle = new Rectangle();
            ABCSeriesFullRectangle.Height = HeightRect;
            double proteinCharPosXWidth = 10 + Canvas.GetLeft(proteinCharsAndSpaces[ProteinSequence.Length - 1]);

            ABCSeriesFullRectangle.Width = proteinCharPosXWidth;
            // Set Rectangle's width and color  
            ABCSeriesFullRectangle.StrokeThickness = 0.5;

            // Fill rectangle with light gray color  
            ABCSeriesFullRectangle.Fill = new SolidColorBrush(COLOR_SERIES_RECTANGLE);

            // Add Rectangle to the Grid.  
            MyCanvas.Children.Add(ABCSeriesFullRectangle);
            Canvas.SetLeft(ABCSeriesFullRectangle, initialXLine - 40);
            Canvas.SetTop(ABCSeriesFullRectangle, initialYLine + offSetY);
            Canvas.SetZIndex(ABCSeriesFullRectangle, -1);

            #region rect SERIE
            Rectangle ABCSeriesRectangle = new Rectangle();
            ABCSeriesRectangle.Height = HeightRect;
            ABCSeriesRectangle.Width = 80;
            // Create a blue and a black Brush  
            backgroundColor.Color = Colors.Gray;
            blackBrush.Color = Colors.Black;
            // Set Rectangle's width and color  
            ABCSeriesRectangle.StrokeThickness = 0.5;

            // Fill rectangle with gray color  
            ABCSeriesRectangle.Fill = backgroundColor;
            // Add Rectangle to the Grid.  
            MyCanvas.Children.Add(ABCSeriesRectangle);
            Canvas.SetLeft(ABCSeriesRectangle, initialXLine + proteinCharPosXWidth - 40);
            Canvas.SetTop(ABCSeriesRectangle, initialYLine + offSetY);
            Canvas.SetZIndex(ABCSeriesRectangle, -1);

            Label Serie = new Label();
            Serie.FontFamily = new FontFamily("Courier New");
            Serie.FontWeight = FontWeights.SemiBold;
            if (this.HasMergeConditions)
                Serie.FontSize = FONTSIZE_BOX_CONDITION_SERIE - 26;
            else
                Serie.FontSize = FONTSIZE_BOX_CONDITION_SERIE;
            Serie.Content = serie;
            Serie.Foreground = new SolidColorBrush(Colors.White); ;
            Serie.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            MyCanvas.Children.Add(Serie);
            if (this.HasMergeConditions)
                Canvas.SetLeft(Serie, initialXLine + proteinCharPosXWidth - 40);
            else
                Canvas.SetLeft(Serie, initialXLine + proteinCharPosXWidth - 20);
            double fontY = (HeightRect - 45) / 2 - 5;
            if (this.HasMergeConditions)
                Canvas.SetTop(Serie, initialYLine + fontY + offSetY + 5);
            else
                Canvas.SetTop(Serie, initialYLine + fontY + offSetY);

            #endregion
        }

        private void PlotFragmentIons(double initialYLine, int offSetY, List<string> PrecursorChargesOrActivationLevelOrFragMethods, List<(string, string, string, int, double)> currentFragmentIons, List<Label> proteinCharsAndSpaces, ref int countPrecursorChargeState, int intensityColorsMap_index = 0, bool hasIntensityperMap = false, double local_intensity_normalization = 0)
        {
            if (!this.HasMergeConditions)
            {
                foreach (string precursorChargeOrActivationLevel in PrecursorChargesOrActivationLevelOrFragMethods)
                {
                    List<(string, string, string, int, double)> currentPrecursorCharge = null;

                    if (isFragmentationMethod && (showPrecursorChargeState || showActivationLevel || showReplicates))
                        currentPrecursorCharge = currentFragmentIons.Where(a => a.Item1.Equals(precursorChargeOrActivationLevel)).ToList();
                    else
                        currentPrecursorCharge = currentFragmentIons.Where(a => a.Item2.Equals(precursorChargeOrActivationLevel)).ToList();
                    countPrecursorChargeState = PlotBarsFragIons(initialYLine, offSetY, proteinCharsAndSpaces, countPrecursorChargeState, intensityColorsMap_index, hasIntensityperMap, local_intensity_normalization, precursorChargeOrActivationLevel, currentPrecursorCharge);
                }
            }
            else
            {
                countPrecursorChargeState = PlotBarsFragIons(initialYLine, offSetY, proteinCharsAndSpaces, countPrecursorChargeState, intensityColorsMap_index, hasIntensityperMap, local_intensity_normalization, string.Empty, currentFragmentIons);
            }
        }

        private int PlotBarsFragIons(double initialYLine, int offSetY, List<Label> proteinCharsAndSpaces, int countPrecursorChargeState, int intensityColorsMap_index, bool hasIntensityperMap, double local_intensity_normalization, string precursorChargeOrActivationLevel, List<(string, string, string, int, double)> currentPrecursorCharge)
        {
            for (int count = 0; count < currentPrecursorCharge.Count; count++)
            {
                // Drawing a line
                Line l = new Line();
                double proteinCharPosX = 16 + Canvas.GetLeft(proteinCharsAndSpaces[currentPrecursorCharge[count].Item4 - 1]);
                l.X1 = proteinCharPosX;
                l.X2 = proteinCharPosX;
                l.Y1 = SPACER_Y;
                l.Y2 = SPACER_Y + FRAGMENT_ION_HEIGHT;

                string currentPrecursorChargeStateOrActivationLevel = string.Empty;
                if (this.HasMergeConditions || (isFragmentationMethod && (showPrecursorChargeState || showActivationLevel || showReplicates)))
                    currentPrecursorChargeStateOrActivationLevel = currentPrecursorCharge[count].Item1;
                else
                    currentPrecursorChargeStateOrActivationLevel = currentPrecursorCharge[count].Item2;

                if (hasIntensityperMap)
                {
                    SolidColorBrush currentColor = null;
                    if (IsGlobalIntensityMap)
                        currentColor = new SolidColorBrush(FRAGMENT_ION_LINE_COLORS[0].Color);
                    else
                        currentColor = new SolidColorBrush(FRAGMENT_ION_LINE_COLORS[intensityColorsMap_index].Color);
                    currentColor.Opacity = currentPrecursorCharge[count].Item5 / local_intensity_normalization;
                    l.Stroke = currentColor;
                }
                else
                {
                    int _index = Array.FindIndex(PrecursorChargeStatesOrActivationLevelsColors, a => a.Equals(currentPrecursorChargeStateOrActivationLevel));
                    l.Stroke = FRAGMENT_ION_LINE_COLORS[_index];
                }
                l.StrokeThickness = WIDTH_LINE;
                if (isPrecursorChargeState && !isActivationLevel && !isFragmentationMethod)
                    l.ToolTip = "Charge: " + currentPrecursorCharge[count].Item2.ToString() + "+\nPosition: " + currentPrecursorCharge[count].Item4.ToString();
                else if (isActivationLevel && !isPrecursorChargeState && !isFragmentationMethod)
                    l.ToolTip = "Activation Level: " + currentPrecursorCharge[count].Item2.ToString() + "\nPosition: " + currentPrecursorCharge[count].Item4.ToString();
                else if (isFragmentationMethod && !isPrecursorChargeState && !isActivationLevel)
                    l.ToolTip = "Fragmentation Method: " + currentPrecursorCharge[count].Item1 + "\nPosition: " + currentPrecursorCharge[count].Item4.ToString();
                else // Merge Frag Ion
                    l.ToolTip = "Cleavage Frequency: "+ currentPrecursorCharge[count].Item5 + "\nPosition: " + currentPrecursorCharge[count].Item4.ToString();

                MyCanvas.Children.Add(l);
                Canvas.SetTop(l, initialYLine + offSetY);

            }
            if (currentPrecursorCharge.Count > 0)
            {
                #region plot fragment legend on the left
                Label fragmentLabel = new Label();
                fragmentLabel.FontFamily = new FontFamily("Courier New");
                fragmentLabel.FontWeight = FontWeights.Bold;
                fragmentLabel.FontSize = FONTSIZE_PROTEINSEQUENCE - 12;
                if (isPrecursorChargeState)
                    fragmentLabel.Content = precursorChargeOrActivationLevel + "+";
                else
                    fragmentLabel.Content = precursorChargeOrActivationLevel;
                fragmentLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                fragmentLabel.Foreground = labelBrush_PTN;
                fragmentLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                MyCanvas.Children.Add(fragmentLabel);
                Canvas.SetLeft(fragmentLabel, 100);
                Canvas.SetTop(fragmentLabel, initialYLine + offSetY + SPACER_Y);

                #endregion
                countPrecursorChargeState++;
                SPACER_Y += 35;
            }

            return countPrecursorChargeState;
        }

        private void PlotProteinSequence(List<double> PtnCharPositions, List<Label> proteinCharsAndSpaces, int leftOffset)
        {
            if (isPrecursorChargeState) leftOffset++;
            PtnCharPositions.Clear();
            for (int i = 0; i < ProteinSequence.Length; i++)
            {
                Label proteinLabel = new Label();
                proteinLabel.FontFamily = new FontFamily("Courier New");
                proteinLabel.FontWeight = FontWeights.Bold;
                proteinLabel.FontSize = FONTSIZE_PROTEINSEQUENCE;
                proteinLabel.Content = ProteinSequence[i];
                proteinLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                proteinLabel.Foreground = labelBrush_PTN;
                proteinLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                proteinCharsAndSpaces.Add(proteinLabel);

                Canvas.SetLeft(proteinLabel, leftOffset * SPACER_X + 75 + 30 * (i + 1));
                PtnCharPositions.Add(leftOffset * SPACER_X + 75 + 30 * (i + 1));
            }
        }

        /// <summary>
        /// Method responsable for saving SIM Net image
        /// </summary>
        /// <returns></returns>
        public byte SaveFragmentIonsImage()
        {
            byte returnOK = 0;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.Filter = "Tiff Image|*.tiff|Png Image|*.png|Jpg Image|*.jpg";// Filter files by extension
            dlg.Title = "Export Image Fragment Ions";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                if (!dlg.FileName.EndsWith(".ps"))
                {
                    try
                    {
                        this.SaveGraph(dlg.FileName);
                        returnOK = 0;//0 -> ok, 1 -> failed, 2 -> cancel
                    }
                    catch (Exception e)
                    {
                        returnOK = 1;//0 -> ok, 1 -> failed, 2 -> cancel
                    }
                }
                else
                {
                    //returnOK = ExportDataToPS(isCircularViewer, dlg.FileName);
                }
            }
            else
                returnOK = 2;//0 -> ok, 1 -> failed, 2 -> cancel
            return returnOK;
        }

        public void SetInitialXY()
        {
            MyScrollBar.ScrollToHome();
        }

        /// <summary>
        /// Saves the stage plot to an image file
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveGraph(string fileName)
        {
            int _height = (int)(MyCanvas.Height * 5);
            _height = _height > 3000 ? _height : 3000;
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)(HighestX * 3.2), _height, 300d, 300d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(MyCanvas);

            // Make a PNG encoder.
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = System.IO.File.OpenWrite(fileName))
            {
                pngEncoder.Save(fs);
            }
        }
    }
}
