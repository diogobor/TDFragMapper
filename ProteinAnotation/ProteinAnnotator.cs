/**
 * Program:     ProteinAnnotation
 * Author:      Diogo Borges Lima
 * Created:     6/6/2019
 * Update by:   Diogo Borges Lima
 * Description: Protein Anotator class
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Threading;
using ProteinAnnotation.Utils;

namespace ProteinAnnotation
{

    public partial class ProteinAnnotator : UserControl
    {
        /// <summary>
        /// Constants
        /// </summary>
        private const int SPACER = 15;
        private const int FONTSIZE_PROTEINSEQUENCE = 15;
        private const int FONTSIZE_PROTEINSEQUENCE_PTM = 10;
        private const int FONTSIZE_NC_TERM_NUMBERS = 12;
        private const int X1OFFSET = 10;

        private int Y1OFFSET = 10;

        /// <summary>
        /// Local variables
        /// </summary>
        private Graphics graphicsPeptAnnotator;
        private Bitmap imageBuffer;
        private string Protein { get; set; }
        private string Peptides { get; set; }
        private string AnotationAlfa { get; set; }
        private string AnotationBeta { get; set; }
        private int XlPos1 { get; set; }
        private int XlPos2 { get; set; }
        private ToolTip toolTipAnnotation;
        Brush[] AminoAcidColor { get; set; }
        //Tuple<Point, description, aminoacid, position>
        private List<Tuple<Point, string, string, int>> PTMsPoints { get; set; }
        private bool ShowToolTip;
        private PTMPopUp popUpPTMwindow;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProteinAnnotator()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            toolTipAnnotation = new ToolTip();
            PTMsPoints = new List<Tuple<Point, string, string, int>>();
            popUpPTMwindow = new PTMPopUp();
            #region Initialize Color array
            AminoAcidColor = new Brush[26];            AminoAcidColor[0] = System.Drawing.Brushes.Red;            AminoAcidColor[1] = System.Drawing.Brushes.Purple;            AminoAcidColor[2] = System.Drawing.Brushes.Green;            AminoAcidColor[3] = System.Drawing.Brushes.DarkKhaki;            AminoAcidColor[4] = System.Drawing.Brushes.MediumTurquoise;            AminoAcidColor[5] = System.Drawing.Brushes.Brown;            AminoAcidColor[6] = System.Drawing.Brushes.Salmon;            AminoAcidColor[7] = System.Drawing.Brushes.DarkViolet;            AminoAcidColor[8] = System.Drawing.Brushes.Navy;            AminoAcidColor[9] = System.Drawing.Brushes.Gray;            AminoAcidColor[10] = System.Drawing.Brushes.Maroon;            AminoAcidColor[11] = System.Drawing.Brushes.Magenta;            AminoAcidColor[12] = System.Drawing.Brushes.MediumAquamarine;            AminoAcidColor[13] = System.Drawing.Brushes.LightSteelBlue;            AminoAcidColor[14] = System.Drawing.Brushes.BurlyWood;            AminoAcidColor[15] = System.Drawing.Brushes.Beige;            AminoAcidColor[16] = System.Drawing.Brushes.Aqua;            AminoAcidColor[17] = System.Drawing.Brushes.OliveDrab;            AminoAcidColor[18] = System.Drawing.Brushes.OrangeRed;            AminoAcidColor[19] = System.Drawing.Brushes.PaleGreen;            AminoAcidColor[20] = System.Drawing.Brushes.PapayaWhip;            AminoAcidColor[21] = System.Drawing.Brushes.Peru;            AminoAcidColor[22] = System.Drawing.Brushes.Silver;            AminoAcidColor[23] = System.Drawing.Brushes.SeaShell;            AminoAcidColor[24] = System.Drawing.Brushes.SkyBlue;            AminoAcidColor[25] = System.Drawing.Brushes.Turquoise;
            #endregion
        }

        private void panelAnotator_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPosition = new Point(e.X, e.Y);

            List<Tuple<Point, string, string, int>> possiblePTM = (from ptm in PTMsPoints
                                                                   where ptm.Item1.X <= e.X && ptm.Item1.X + SPACER >= e.X
                                                                   && ptm.Item1.Y <= e.Y && ptm.Item1.Y + 22 >= e.Y
                                                                   select ptm).ToList();

            if (possiblePTM.Count > 0)
            {
                List<string> ptmDescription = (from ptm in possiblePTM
                                               select ptm.Item2).Distinct().ToList();
                string aminoacid = (from ptm in possiblePTM
                                    select ptm.Item3 + "(" + ptm.Item4 + ")").Distinct().FirstOrDefault();
                if (!popUpPTMwindow.IsOpen)
                {
                    popUpPTMwindow.Setup(aminoacid, ptmDescription, new Point(e.X, e.Y), this.PointToScreen(this.Parent.Location), this.Parent.Location);
                    popUpPTMwindow.Show();
                    ShowToolTip = false;
                }
            }
            else
            {
                if (popUpPTMwindow.IsOpen)
                {
                    popUpPTMwindow.IsOpen = false;
                    popUpPTMwindow.Hide();
                }
            }
        }

        public void DrawProteoformsPeptides(bool isSingleLine, string proteinSequence, List<(string, List<(string, int, string)>)> annotationProteoforms = null, List<(string, bool, string, int, string)> annotationPeptides = null, string[] PTMs_color = null, int[] posPTM_peptides = null, double[] massProteoformPTMs = null, double[] massPeptidesPTMs = null)
        {
            List<Tuple<string, List<Tuple<string, int, string>>>> newAnnotationProteoforms = new List<Tuple<string, List<Tuple<string, int, string>>>>();
            annotationProteoforms.ForEach(protfm =>
            {
                List<Tuple<string, int, string>> mods = (from mod in protfm.Item2.AsParallel()
                                                         select Tuple.Create(mod.Item1, mod.Item2, mod.Item3)).ToList();
                newAnnotationProteoforms.Add(Tuple.Create(protfm.Item1, mods));
            });

            List<Tuple<string, bool, string, int, string>> newAnnotationPeptides = (from pept in annotationPeptides
                                                                                    select pept.ToTuple()).ToList();

            this.DrawProteoformsPeptides(isSingleLine, proteinSequence, newAnnotationProteoforms, newAnnotationPeptides, PTMs_color);
        }

        /// <summary>
        /// Methord responsible for drawing protein sequence with possible proteoforms and peptides annotated.
        /// </summary>
        /// <param name="isSingleLine">Print protein sequence in a single line or in multiple lines</param>
        /// <param name="proteinSequence"></param>
        /// <param name="annotationProteoforms"></param>
        /// <param name="annotationPeptides"></param>
        /// <param name="annotationUniquePeptides"></param>
        /// <param name="PTMs_Proteoform">"Tuple<aminoacid, position, description>"</param>
        /// <param name="PTMs_color"></param>
        /// <param name="posPTM_peptides"></param>
        /// <param name="massProteoformPTMs"></param>
        /// <param name="massPeptidesPTMs"></param>
        public void DrawProteoformsPeptides(bool isSingleLine, string proteinSequence, List<Tuple<string, List<Tuple<string, int, string>>>> annotationProteoforms = null, List<Tuple<string, bool, string, int, string>> annotationPeptides = null, string[] PTMs_color = null, int[] posPTM_peptides = null, double[] massProteoformPTMs = null, double[] massPeptidesPTMs = null)
        {
            #region Setting Values
            //Tuple<proteoform, modifications( List<Tuple<aminoacid, position, PTMdescription>>) >
            Dictionary<int, IGrouping<int, Tuple<string, int, string>>> groupedProteoformPtmsBySite = null;
            //Tuple<peptide, isUnique, aminoacid, position, description>
            Dictionary<int, IGrouping<int, Tuple<string, bool, string, int, string>>> groupedPeptPtmsBySite = null;

            if (annotationProteoforms != null && annotationProteoforms.Count > 0)
            {
                groupedProteoformPtmsBySite = (from prot in annotationProteoforms
                                               from mods in prot.Item2
                                               group mods by mods.Item2 into newGroup
                                               orderby newGroup.Key
                                               select newGroup).ToDictionary(v => v.Key, v => v);
            }

            if (annotationPeptides != null && annotationPeptides.Count > 0)
            {
                groupedPeptPtmsBySite = (from pept in annotationPeptides
                                         group pept by pept.Item4 into newGroup
                                         orderby newGroup.Key
                                         select newGroup).ToDictionary(v => v.Key, v => v);
            }

            Protein = proteinSequence;
            Y1OFFSET = 10;

            int panelAnnotatorHeight = ((proteinSequence.Length / 50) + 3 * 14) * FONTSIZE_PROTEINSEQUENCE * 2;
            if (isSingleLine)
                imageBuffer = new Bitmap((proteinSequence.Length + 6) * SPACER + X1OFFSET, panelAnnotatorHeight);
            else
                imageBuffer = new Bitmap(panelAnnotator.Width, panelAnnotatorHeight);
            graphicsPeptAnnotator = Graphics.FromImage(imageBuffer);
            graphicsPeptAnnotator.Clear(Color.White);
            #endregion

            double xLastPosProtein = 0.0;
            double xLastPosPept2 = 0.0;
            double yLastPosProtein = 0.0;
            double yLastPosPept2 = 0.0;

            //Setting font
            Font _font = new System.Drawing.Font("Courier New", FONTSIZE_PROTEINSEQUENCE, FontStyle.Bold);
            Font _fontPTM = new System.Drawing.Font("Courier New", FONTSIZE_PROTEINSEQUENCE_PTM, FontStyle.Bold);
            Font _fontNC_term_Number = new System.Drawing.Font("Courier New", FONTSIZE_NC_TERM_NUMBERS, FontStyle.Bold);

            #region Writing protein and peptides

            List<int>[] matchLocationsTopDown = new List<int>[proteinSequence.Length];
            List<int>[] matchLocationsBottomUp = new List<int>[proteinSequence.Length];
            List<int>[] matchLocationsBottomUpUniquePepts = new List<int>[proteinSequence.Length];

            #region Fill array Matched aminoacids in protein sequence

            #region Proteoforms

            //Tuple<proteoform, aminoacid, position,description>

            if (annotationProteoforms != null && annotationProteoforms.Count > 0)
            {
                int countProteoform = 0;
                foreach (Tuple<string, List<Tuple<string, int, string>>> proteoform in annotationProteoforms)
                {
                    Match locationProtfm = Regex.Match(proteinSequence, proteoform.Item1);

                    var maxValues = matchLocationsTopDown.Skip(locationProtfm.Index).Take(locationProtfm.Length).Where(item => item != null);
                    int maxValue = 0;
                    if (maxValues.Count() > 0)
                        maxValue = maxValues.Select(item => item.Count).Max();

                    if (maxValue == 0 && locationProtfm.Index > 0 && matchLocationsTopDown[locationProtfm.Index - 1] != null && matchLocationsTopDown[locationProtfm.Index - 1].Count == 1)
                        maxValue = 1;//It means that the current proteoform is the same level that the previous one

                    for (int i = locationProtfm.Index; i < locationProtfm.Index + locationProtfm.Length; i++)
                    {
                        if (matchLocationsTopDown[i] == null)
                        {
                            List<int> list = new List<int>();
                            for (int offSetAdd = 0; offSetAdd < maxValue; offSetAdd++)
                                list.Add(-1);
                            list.Add(countProteoform);
                            matchLocationsTopDown[i] = list;
                        }
                        else if (matchLocationsTopDown[i].Count >= maxValue)
                            matchLocationsTopDown[i].Add(countProteoform);
                        else
                        {
                            int threshold = Math.Abs(maxValue - matchLocationsTopDown[i].Count);
                            List<int> list = matchLocationsTopDown[i];
                            for (int offSetAdd = 0; offSetAdd < threshold; offSetAdd++)
                                list.Add(-1);
                            list.Add(countProteoform);
                            matchLocationsTopDown[i] = list;
                        }
                    }
                    countProteoform++;
                }
            }
            #endregion

            #region Peptides
            if (annotationPeptides != null && annotationPeptides.Count > 0)
            {
                #region Common peptides
                int countPept = 0;
                foreach (Tuple<string, bool, string, int, string> peptide in annotationPeptides.Where(item => !item.Item2))
                {
                    string cleanedPeptide = Util.CleanPeptide(peptide.Item1);
                    Match locationPept = Regex.Match(proteinSequence, cleanedPeptide);

                    var maxValues = matchLocationsBottomUp.Skip(locationPept.Index).Take(locationPept.Length).Where(item => item != null);
                    int maxValue = 0;
                    if (maxValues.Count() > 0)
                        maxValue = maxValues.Select(item => item.Count).Max();

                    if (maxValue == 0 && locationPept.Index > 0 && matchLocationsBottomUp[locationPept.Index - 1] != null && matchLocationsBottomUp[locationPept.Index - 1].Count == 1)
                        maxValue = 1;//It means that the current peptide is the same level that the previous one

                    for (int i = locationPept.Index; i < locationPept.Index + locationPept.Length; i++)
                    {
                        if (matchLocationsBottomUp[i] == null)
                        {
                            List<int> list = new List<int>();
                            for (int offSetAdd = 0; offSetAdd < maxValue; offSetAdd++)
                                list.Add(-1);
                            list.Add(countPept);
                            matchLocationsBottomUp[i] = list;
                        }
                        else if (matchLocationsBottomUp[i].Count >= maxValue)
                            matchLocationsBottomUp[i].Add(countPept);
                        else
                        {
                            int threshold = Math.Abs(maxValue - matchLocationsBottomUp[i].Count);
                            List<int> list = matchLocationsBottomUp[i];
                            for (int offSetAdd = 0; offSetAdd < threshold; offSetAdd++)
                                list.Add(-1);
                            list.Add(countPept);
                            matchLocationsBottomUp[i] = list;
                        }
                    }
                    countPept++;
                }
                #endregion

                #region Unique peptides
                countPept = 0;
                foreach (Tuple<string, bool, string, int, string> peptide in annotationPeptides.Where(item => item.Item2))
                {
                    string cleanedPeptide = Util.CleanPeptide(peptide.Item1);
                    Match locationPept = Regex.Match(proteinSequence, cleanedPeptide);

                    var maxValues = matchLocationsBottomUpUniquePepts.Skip(locationPept.Index).Take(locationPept.Length).Where(item => item != null);
                    int maxValue = 0;
                    if (maxValues.Count() > 0)
                        maxValue = maxValues.Select(item => item.Count).Max();

                    //if (maxValue == 0 && locationPept.Index > 0 && matchLocationsBottomUpUniquePepts[locationPept.Index - 1] != null && matchLocationsBottomUpUniquePepts[locationPept.Index - 1].Count == 1)
                    //    maxValue = 1;//It means that the current peptide is the same level that the previous one

                    if (maxValue == 0 && locationPept.Index > 0 && matchLocationsBottomUpUniquePepts[locationPept.Index - 1] != null)
                        maxValue = matchLocationsBottomUpUniquePepts[locationPept.Index - 1].Count;//It means that the current peptide is the same level that the previous one


                    for (int i = locationPept.Index; i < locationPept.Index + locationPept.Length; i++)
                    {
                        if (matchLocationsBottomUpUniquePepts[i] == null)
                        {
                            List<int> list = new List<int>();
                            for (int offSetAdd = 0; offSetAdd < maxValue; offSetAdd++)
                                list.Add(-1);
                            list.Add(countPept);
                            matchLocationsBottomUpUniquePepts[i] = list;
                        }
                        else if (matchLocationsBottomUpUniquePepts[i].Count >= maxValue)
                            matchLocationsBottomUpUniquePepts[i].Add(countPept);
                        else
                        {
                            int threshold = Math.Abs(maxValue - matchLocationsBottomUpUniquePepts[i].Count);
                            List<int> list = matchLocationsBottomUpUniquePepts[i];
                            for (int offSetAdd = 0; offSetAdd < threshold; offSetAdd++)
                                list.Add(-1);
                            list.Add(countPept);
                            matchLocationsBottomUpUniquePepts[i] = list;
                        }
                    }
                    countPept++;
                }
                #endregion
            }
            #endregion

            #endregion

            using (graphicsPeptAnnotator)
            {
                graphicsPeptAnnotator.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                int maxValueTopDown = 1;
                int maxValueUniquePeptides = 1;
                int STEP_PROTEIN_SEQUENCE = 50;
                if (isSingleLine)
                {
                    STEP_PROTEIN_SEQUENCE = proteinSequence.Length - 1;
                    var maxValuesTopDown = matchLocationsTopDown.Where(item => item != null);
                    if (maxValuesTopDown.Count() > 0)
                        maxValueTopDown = maxValuesTopDown.Select(item => item.Count).Max();

                    var maxValuesUniquePeptides = matchLocationsBottomUpUniquePepts.Where(item => item != null);
                    if (maxValuesUniquePeptides.Count() > 0)
                        maxValueUniquePeptides = maxValuesUniquePeptides.Select(item => item.Count).Max();
                }

                int proteinDisplay = 0;
                int highestYoffsetPerLine = Y1OFFSET + FONTSIZE_PROTEINSEQUENCE + 10;
                for (int proteinCount = 0; proteinCount < proteinSequence.Length; proteinDisplay++, proteinCount++)
                {
                    double j = proteinCount + 1;
                    double remainderStepProteinSequence = j % STEP_PROTEIN_SEQUENCE;

                    if (!isSingleLine)
                    {
                        #region Print sequence

                        if (proteinCount % STEP_PROTEIN_SEQUENCE == 0)
                        {
                            if (proteinCount == 0)
                                graphicsPeptAnnotator.DrawString(" N ", _fontNC_term_Number, Brushes.Gray, new Point((proteinDisplay * SPACER) + X1OFFSET, Y1OFFSET + 3));
                            else
                                graphicsPeptAnnotator.DrawString((proteinCount + 1).ToString("000") + " ", _fontNC_term_Number, Brushes.Gray, new Point((proteinDisplay * SPACER) + X1OFFSET, Y1OFFSET + 3));
                            proteinDisplay += 3;

                            graphicsPeptAnnotator.DrawString(proteinSequence[proteinCount].ToString(), _font, Brushes.Black, new Point((proteinDisplay * SPACER) + X1OFFSET, Y1OFFSET));

                            var maxValuesTopDown = matchLocationsTopDown.Skip(proteinCount).Take(STEP_PROTEIN_SEQUENCE).Where(item => item != null);
                            if (maxValuesTopDown.Count() > 0)
                                maxValueTopDown = maxValuesTopDown.Select(item => item.Count).Max();

                            var maxValuesUniquePeptides = matchLocationsBottomUpUniquePepts.Skip(proteinCount).Take(STEP_PROTEIN_SEQUENCE).Where(item => item != null);
                            if (maxValuesUniquePeptides.Count() > 0)
                                maxValueUniquePeptides = maxValuesUniquePeptides.Select(item => item.Count).Max();
                        }
                        else
                        {
                            if (remainderStepProteinSequence == 0)
                            {
                                graphicsPeptAnnotator.DrawString(proteinSequence[proteinCount].ToString(), _font, Brushes.Black, new Point((proteinDisplay * SPACER) + X1OFFSET, Y1OFFSET));

                                graphicsPeptAnnotator.DrawString("  " + (proteinCount + 1).ToString("000"), _fontNC_term_Number, Brushes.Gray, new Point((proteinDisplay * SPACER) + X1OFFSET, Y1OFFSET + 3));
                            }
                            else
                            {
                                graphicsPeptAnnotator.DrawString(proteinSequence[proteinCount].ToString(), _font, Brushes.Black, new Point((proteinDisplay * SPACER) + X1OFFSET, Y1OFFSET));
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region Print sequence
                        if (proteinCount == 0)
                        {
                            graphicsPeptAnnotator.DrawString(" N ", _fontNC_term_Number, Brushes.Gray, new Point((proteinDisplay * SPACER) + X1OFFSET, Y1OFFSET + 3));
                            proteinDisplay += 3;
                        }

                        graphicsPeptAnnotator.DrawString(proteinSequence[proteinCount].ToString(), _font, Brushes.Black, new Point((proteinDisplay * SPACER) + X1OFFSET, Y1OFFSET));

                        #endregion
                    }

                    Pen penProteoform = new Pen(Brushes.DarkOrange);
                    penProteoform.Width = 2;

                    Pen penPeptide = new Pen(Brushes.DarkBlue);
                    penPeptide.Width = 2;

                    Pen penUniquePeptide = new Pen(Brushes.Blue);
                    penUniquePeptide.Width = 2;

                    bool isDescriptionPTMAdded = false;
                    int xOffset = (proteinDisplay * SPACER) + X1OFFSET;
                    int yOffset = Y1OFFSET + FONTSIZE_PROTEINSEQUENCE + 10;

                    if (matchLocationsTopDown[proteinCount] != null)
                    {
                        StringBuilder sbDesc = new StringBuilder();
                        string aminoacid = string.Empty;
                        int currentYoffset = yOffset;

                        #region Draw proteoform ptm and take description to put inside of PopUP
                        for (int countProteoforms = 0; countProteoforms < matchLocationsTopDown[proteinCount].Count; countProteoforms++)
                        {
                            if (matchLocationsTopDown[proteinCount][countProteoforms] > -1)
                            {
                                Point p1 = new Point(xOffset, currentYoffset);
                                Point p2 = new Point(xOffset + SPACER, currentYoffset);
                                graphicsPeptAnnotator.DrawLine(penProteoform, p1, p2);

                                //Tuple<proteoform,aminoacid,position,description>
                                Tuple<string, List<Tuple<string, int, string>>> proteoform = annotationProteoforms[matchLocationsTopDown[proteinCount][countProteoforms]];
                                for (int countMod = 0; countMod < proteoform.Item2.Count; countMod++)
                                {
                                    if (proteoform.Item2[countMod].Item2 == (proteinCount + 1))
                                    {
                                        int description_ptm = Array.IndexOf(PTMs_color, proteoform.Item2[countMod].Item3);
                                        if (description_ptm > -1)
                                            graphicsPeptAnnotator.DrawString(proteinSequence[proteinCount].ToString().ToLower(), _fontPTM, AminoAcidColor[description_ptm], new Point((proteinDisplay * SPACER) + X1OFFSET + 4, currentYoffset - 10));
                                        else
                                            graphicsPeptAnnotator.DrawString(proteinSequence[proteinCount].ToString().ToLower(), _fontPTM, Brushes.Blue, new Point((proteinDisplay * SPACER) + X1OFFSET + 4, currentYoffset - 10));

                                        sbDesc.AppendLine(proteoform.Item2[countMod].Item3);
                                        aminoacid = proteoform.Item2[countMod].Item1;
                                        break;
                                    }
                                }
                            }
                            currentYoffset += 10;
                        }
                        #endregion

                        #region Set PTM point to show pop up when mouse over
                        if (!String.IsNullOrEmpty(aminoacid) && !isDescriptionPTMAdded)
                        {
                            List<string> distinctDesc = new List<string>(Regex.Split(sbDesc.ToString(), "\r\n")).Distinct().ToList();
                            distinctDesc.RemoveAll(item => String.IsNullOrEmpty(item));

                            //Tuple<Point, description, aminoacid, position>
                            PTMsPoints.Add(Tuple.Create(new Point((proteinDisplay * SPACER) + X1OFFSET, Y1OFFSET), String.Join("\n", distinctDesc), aminoacid.Replace("}", "N-Term"), proteinCount + 1));
                            isDescriptionPTMAdded = true;
                        }
                        #endregion
                    }

                    highestYoffsetPerLine = (Y1OFFSET + FONTSIZE_PROTEINSEQUENCE + 10) + maxValueTopDown * 10;
                    yOffset = highestYoffsetPerLine;
                    if (matchLocationsBottomUpUniquePepts[proteinCount] != null)
                    {
                        StringBuilder sbDesc = new StringBuilder();
                        string aminoacid = string.Empty;
                        int currentYoffset = yOffset;

                        #region Draw peptide ptm and take description to put inside of PopUP
                        for (int countUniquePeptides = 0; countUniquePeptides < matchLocationsBottomUpUniquePepts[proteinCount].Count; countUniquePeptides++)
                        {
                            if (matchLocationsBottomUpUniquePepts[proteinCount][countUniquePeptides] > -1)
                            {
                                Point p1 = new Point(xOffset, currentYoffset);
                                Point p2 = new Point(xOffset + SPACER, currentYoffset);
                                graphicsPeptAnnotator.DrawLine(penUniquePeptide, p1, p2);

                                //Tuple<peptide,isUnique,aminoacid,position,description>
                                Tuple<string, bool, string, int, string> peptide = annotationPeptides.Where(item => item.Item2).ToList()[matchLocationsBottomUpUniquePepts[proteinCount][countUniquePeptides]];
                                if (peptide.Item4 == (proteinCount + 1))
                                {
                                    int description_ptm = Array.IndexOf(PTMs_color, peptide.Item5);
                                    if (description_ptm > -1)
                                        graphicsPeptAnnotator.DrawString(proteinSequence[proteinCount].ToString().ToLower(), _fontPTM, AminoAcidColor[description_ptm], new Point((proteinDisplay * SPACER) + X1OFFSET + 4, currentYoffset - 10));
                                    else
                                        graphicsPeptAnnotator.DrawString(proteinSequence[proteinCount].ToString().ToLower(), _fontPTM, Brushes.Blue, new Point((proteinDisplay * SPACER) + X1OFFSET + 4, currentYoffset - 10));

                                    sbDesc.AppendLine(peptide.Item5);
                                    aminoacid = peptide.Item3;
                                }
                            }

                            currentYoffset += 10;
                        }
                        #endregion

                        #region Set PTM point to show pop up when mouse over
                        if (!String.IsNullOrEmpty(aminoacid) && !isDescriptionPTMAdded)
                        {
                            #region Take proteoform PTM description to put inside of PopUP
                            if (groupedProteoformPtmsBySite != null && groupedProteoformPtmsBySite.Count > 0 && groupedProteoformPtmsBySite.ContainsKey(proteinCount + 1))
                            {
                                //Tuple<proteoform, aminoacid, position, description>
                                IGrouping<int, Tuple<string, int, string>> descriptions;
                                groupedProteoformPtmsBySite.TryGetValue(proteinCount + 1, out descriptions);
                                if (descriptions != null)
                                {
                                    foreach (var desc in descriptions)
                                    {
                                        sbDesc.AppendLine(desc.Item3);
                                        aminoacid = desc.Item1;
                                    }
                                }
                            }
                            #endregion

                            List<string> distinctDesc = new List<string>(Regex.Split(sbDesc.ToString(), "\r\n")).Distinct().ToList();
                            distinctDesc.RemoveAll(item => String.IsNullOrEmpty(item));

                            //Tuple<Point, description, aminoacid, position>
                            PTMsPoints.Add(Tuple.Create(new Point((proteinDisplay * SPACER) + X1OFFSET, Y1OFFSET), String.Join("\n", distinctDesc), aminoacid.Replace("}", "N-Term"), proteinCount + 1));
                            isDescriptionPTMAdded = true;
                        }
                        #endregion
                    }

                    highestYoffsetPerLine = (Y1OFFSET + FONTSIZE_PROTEINSEQUENCE + 10) + maxValueTopDown * 10 + maxValueUniquePeptides * 10;
                    yOffset = highestYoffsetPerLine;
                    if (matchLocationsBottomUp[proteinCount] != null)
                    {
                        StringBuilder sbDesc = new StringBuilder();
                        string aminoacid = string.Empty;
                        int currentYoffset = yOffset;

                        #region Draw peptide ptm and take description to put inside of PopUP
                        for (int countPeptides = 0; countPeptides < matchLocationsBottomUp[proteinCount].Count; countPeptides++)
                        {
                            if (matchLocationsBottomUp[proteinCount][countPeptides] > -1)
                            {
                                Point p1 = new Point(xOffset, currentYoffset);
                                Point p2 = new Point(xOffset + SPACER, currentYoffset);
                                graphicsPeptAnnotator.DrawLine(penPeptide, p1, p2);
                                //Tuple<peptide,isUnique,aminoacid,position,description>
                                Tuple<string, bool, string, int, string> peptide = annotationPeptides.Where(item => !item.Item2).ToList()[matchLocationsBottomUp[proteinCount][countPeptides]];
                                if (peptide.Item4 == (proteinCount + 1))
                                {
                                    int description_ptm = Array.IndexOf(PTMs_color, peptide.Item5);
                                    if (description_ptm > -1)
                                        graphicsPeptAnnotator.DrawString(proteinSequence[proteinCount].ToString().ToLower(), _fontPTM, AminoAcidColor[description_ptm], new Point((proteinDisplay * SPACER) + X1OFFSET + 4, currentYoffset - 10));
                                    else
                                        graphicsPeptAnnotator.DrawString(proteinSequence[proteinCount].ToString().ToLower(), _fontPTM, Brushes.Blue, new Point((proteinDisplay * SPACER) + X1OFFSET + 4, currentYoffset - 10));

                                    sbDesc.AppendLine(peptide.Item5);
                                    aminoacid = peptide.Item3;
                                }
                                currentYoffset += 10;
                            }
                        }
                        #endregion

                        #region Set PTM point to show pop up when mouse over
                        if (!String.IsNullOrEmpty(aminoacid) && !isDescriptionPTMAdded)
                        {
                            #region Take proteoform PTM description to put inside of PopUP
                            if (groupedProteoformPtmsBySite != null && groupedProteoformPtmsBySite.Count > 0 && groupedProteoformPtmsBySite.ContainsKey(proteinCount + 1))
                            {
                                //Tuple<proteoform, aminoacid, position, description>
                                IGrouping<int, Tuple<string, int, string>> descriptions;
                                groupedProteoformPtmsBySite.TryGetValue(proteinCount + 1, out descriptions);
                                if (descriptions != null)
                                {
                                    foreach (var desc in descriptions)
                                    {
                                        sbDesc.AppendLine(desc.Item3);
                                        aminoacid = desc.Item1;
                                    }
                                }
                            }
                            #endregion

                            List<string> distinctDesc = new List<string>(Regex.Split(sbDesc.ToString(), "\r\n")).Distinct().ToList();
                            distinctDesc.RemoveAll(item => String.IsNullOrEmpty(item));

                            //Tuple<Point, description, aminoacid, position>
                            PTMsPoints.Add(Tuple.Create(new Point((proteinDisplay * SPACER) + X1OFFSET, Y1OFFSET), String.Join("\n", distinctDesc), aminoacid.Replace("}", "N-Term"), proteinCount + 1));
                            isDescriptionPTMAdded = true;
                        }
                        #endregion
                    }

                    if (!isSingleLine)
                    {
                        if (remainderStepProteinSequence == 0)
                        {
                            xLastPosProtein = ((proteinDisplay + 4) * SPACER) + X1OFFSET;
                            proteinDisplay = -1;
                            Y1OFFSET = highestYoffsetPerLine > Y1OFFSET ? highestYoffsetPerLine + (2 * FONTSIZE_PROTEINSEQUENCE) - 2 : Y1OFFSET + (4 * FONTSIZE_PROTEINSEQUENCE) - 2;
                        }
                    }

                    if (proteinCount == proteinSequence.Length - 1)
                    {
                        graphicsPeptAnnotator.DrawString(" C ", _fontNC_term_Number, Brushes.Gray, new Point(((proteinDisplay + 1) * SPACER) + X1OFFSET, Y1OFFSET + 3));
                        Y1OFFSET = yOffset > Y1OFFSET ? yOffset : Y1OFFSET;

                        if (isSingleLine)
                            xLastPosProtein = (proteinSequence.Length + 6) * SPACER + X1OFFSET;
                        else
                            xLastPosProtein = ((proteinCount + 4) * SPACER) + X1OFFSET;
                    }
                }
                int finalLastPosProtein = Y1OFFSET + FONTSIZE_PROTEINSEQUENCE - 20;
                yLastPosProtein = finalLastPosProtein > Y1OFFSET ? finalLastPosProtein : Y1OFFSET + FONTSIZE_PROTEINSEQUENCE;
            }
            #endregion

            int newHeightRect = (int)(yLastPosProtein + yLastPosPept2 + FONTSIZE_PROTEINSEQUENCE);
            int newWidthRect = (int)(xLastPosProtein > xLastPosPept2 ? xLastPosProtein : xLastPosPept2);
            Rectangle cropRect = new Rectangle(0, 0, newWidthRect, newHeightRect);
            imageBuffer = this.CropImage(imageBuffer, cropRect);
            panelAnnotator.Height = newHeightRect;
            panelAnnotator.Width = newWidthRect;
            panelAnnotator.BackgroundImage = imageBuffer;
        }

        /// <summary>
        /// Method responsable for cropping image
        /// </summary>
        /// <param name="bmpImage"></param>
        /// <param name="cropArea"></param>
        /// <returns></returns>
        private Bitmap CropImage(Bitmap bmpImage, Rectangle cropArea)
        {
            Bitmap src = bmpImage;
            Bitmap target = new Bitmap(cropArea.Width, cropArea.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                 cropArea,
                                 GraphicsUnit.Pixel);
            }
            return target;
        }

        /// <summary>
        /// Method responsable for painting tha main panel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            //if (imageBuffer != null)
            //{
            //    var rc = new Rectangle(this.ClientSize.Width - panelAnnotator.BackgroundImage.Width,
            //        this.ClientSize.Height - panelAnnotator.BackgroundImage.Height,
            //        panelAnnotator.BackgroundImage.Width, panelAnnotator.BackgroundImage.Height);

            //    panelAnnotator.BackgroundImageLayout = ImageLayout.Center;
            //}
        }

        /// <summary>
        /// Method responsible for drawing intra link connector 
        /// </summary>
        /// <param name="xlPos1"></param>
        /// <param name="xlPos2"></param>
        private void DrawIntraLinker(int xlPos1, int xlPos2)
        {
            IntraLinkerVerticalLine(xlPos1);
            IntraLinkerVerticalLine(xlPos2);

            Pen p = new Pen(Brushes.Black);
            p.Width = 2;

            Point p1 = new Point(((xlPos1 - 1) * SPACER) + X1OFFSET + (FONTSIZE_PROTEINSEQUENCE / 2) + 2, Y1OFFSET + FONTSIZE_PROTEINSEQUENCE + 25);
            Point p2 = new Point(((xlPos2 - 1) * SPACER) + X1OFFSET + (FONTSIZE_PROTEINSEQUENCE / 2) + 2, Y1OFFSET + FONTSIZE_PROTEINSEQUENCE + 25);

            graphicsPeptAnnotator.DrawLine(p, p1, p2);
        }

        /// <summary>
        /// Method responsible for drawing inter link connector
        /// </summary>
        /// <param name="xlPos"></param>
        private void IntraLinkerVerticalLine(int xlPos)
        {
            //Draw the Intra Cross Linker Vertical Line
            Pen p = new Pen(Brushes.Black);
            p.Width = 2;

            Point p1 = new Point(((xlPos - 1) * SPACER) + X1OFFSET + (FONTSIZE_PROTEINSEQUENCE / 2) + 2, Y1OFFSET + FONTSIZE_PROTEINSEQUENCE + 5);
            Point p2 = new Point(((xlPos - 1) * SPACER) + X1OFFSET + (FONTSIZE_PROTEINSEQUENCE / 2) + 2, Y1OFFSET + FONTSIZE_PROTEINSEQUENCE + 25);

            graphicsPeptAnnotator.DrawLine(p, p1, p2);
        }

        /// <summary>
        /// Method responsible for drawing the presence of 'b' fragment ion
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        private void DrawLeftBreak(int xOffset, int yOffset, bool isAlpha, bool isIntraLink, int currentPos, bool isAlphaXL, bool isBetaXL)
        {
            Pen penAlpha = new Pen(Brushes.Blue);
            Pen penBeta = new Pen(Brushes.Red);
            penAlpha.Width = 2;
            penBeta.Width = 2;

            Pen penAlphaXL = new Pen(Brushes.DarkOrange);
            Pen penBetaXL = new Pen(Brushes.DarkOrange);
            penAlphaXL.Width = 2;
            penBetaXL.Width = 2;

            Point p1 = new Point(xOffset, yOffset);
            Point p2 = new Point(xOffset, yOffset + FONTSIZE_PROTEINSEQUENCE + 2);
            Point p3 = new Point(xOffset - 8, yOffset + 8 + FONTSIZE_PROTEINSEQUENCE);

            if (!isIntraLink)
            {
                if (isAlpha)
                {
                    if (isAlphaXL)
                    {
                        graphicsPeptAnnotator.DrawLine(penAlpha, p1, p2);
                        graphicsPeptAnnotator.DrawLine(penAlphaXL, p2, p3);
                        System.Drawing.Font f = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.DarkOrange, new Point((p3.X - 5), p3.Y));
                    }
                    else
                    {
                        graphicsPeptAnnotator.DrawLine(penAlpha, p1, p2);
                        graphicsPeptAnnotator.DrawLine(penAlpha, p2, p3);
                        System.Drawing.Font f = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.Blue, new Point((p3.X - 5), p3.Y));
                    }
                }
                else
                {
                    if (isBetaXL)
                    {
                        graphicsPeptAnnotator.DrawLine(penBeta, p1, p2);
                        graphicsPeptAnnotator.DrawLine(penBetaXL, p2, p3);
                        System.Drawing.Font f = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.DarkOrange, new Point((p3.X - 5), p3.Y));
                    }
                    else
                    {
                        graphicsPeptAnnotator.DrawLine(penBeta, p1, p2);
                        graphicsPeptAnnotator.DrawLine(penBeta, p2, p3);
                        System.Drawing.Font f = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.Red, new Point((p3.X - 5), p3.Y));
                    }
                }
            }
            else
            {
                Pen penMiddle = new Pen(Brushes.Gray);
                penMiddle.Width = 2;
                System.Drawing.Font f = new System.Drawing.Font("Arial", 6, FontStyle.Bold);

                graphicsPeptAnnotator.DrawLine(penMiddle, p1, p2);

                if (isAlphaXL)
                {
                    graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.DarkOrange, new Point((p3.X - 5), p3.Y));
                    graphicsPeptAnnotator.DrawLine(penBetaXL, p2, p3);
                }
                else
                {
                    graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.Red, new Point((p3.X - 5), p3.Y));
                    graphicsPeptAnnotator.DrawLine(penBeta, p2, p3);
                }
            }
        }

        /// <summary>
        /// Method responsible for drawing the presence of 'y' fragment ion
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        private void DrawRightBreak(int xOffset, int yOffset, bool isAlpha, bool isIntraLink, int currentPos, bool isAlphaXL, bool isBetaXL)
        {
            Pen penAlpha = new Pen(Brushes.Blue);
            Pen penBeta = new Pen(Brushes.Red);
            penAlpha.Width = 2;
            penBeta.Width = 2;

            Pen penAlphaXL = new Pen(Brushes.DarkOrange);
            Pen penBetaXL = new Pen(Brushes.DarkOrange);
            penAlphaXL.Width = 2;
            penBetaXL.Width = 2;

            Point p1 = new Point(xOffset, yOffset);
            Point p2 = new Point(xOffset, yOffset + FONTSIZE_PROTEINSEQUENCE + 2);
            Point p3 = new Point(xOffset + 6, yOffset - 6);

            if (!isIntraLink)
            {
                if (isAlpha)
                {
                    if (isAlphaXL)
                    {
                        graphicsPeptAnnotator.DrawLine(penAlpha, p1, p2);
                        graphicsPeptAnnotator.DrawLine(penAlphaXL, p1, p3);
                        System.Drawing.Font f = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.DarkOrange, new Point((p3.X), p3.Y - 10));
                    }
                    else
                    {
                        graphicsPeptAnnotator.DrawLine(penAlpha, p1, p2);
                        graphicsPeptAnnotator.DrawLine(penAlpha, p1, p3);
                        System.Drawing.Font f = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.Blue, new Point((p3.X), p3.Y - 10));
                    }
                }
                else
                {
                    if (isBetaXL)
                    {
                        graphicsPeptAnnotator.DrawLine(penBetaXL, p1, p2);
                        graphicsPeptAnnotator.DrawLine(penBetaXL, p1, p3);
                        System.Drawing.Font f = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.DarkOrange, new Point((p3.X), p3.Y - 10));
                    }
                    else
                    {
                        graphicsPeptAnnotator.DrawLine(penBeta, p1, p2);
                        graphicsPeptAnnotator.DrawLine(penBeta, p1, p3);
                        System.Drawing.Font f = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.Red, new Point((p3.X), p3.Y - 10));
                    }
                }
            }
            else
            {
                Pen penMiddle = new Pen(Brushes.Gray);
                penMiddle.Width = 2;
                graphicsPeptAnnotator.DrawLine(penMiddle, p1, p2);
                System.Drawing.Font f = new System.Drawing.Font("Arial", 6, FontStyle.Bold);

                if (isAlphaXL)
                {
                    graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.DarkOrange, new Point((p3.X), p3.Y - 10));
                    graphicsPeptAnnotator.DrawLine(penAlphaXL, p1, p3);
                }
                else
                {
                    graphicsPeptAnnotator.DrawString(currentPos.ToString(), f, Brushes.Blue, new Point((p3.X), p3.Y - 10));
                    graphicsPeptAnnotator.DrawLine(penAlpha, p1, p3);
                }


            }
        }

        /// <summary>
        /// Method responsible for reading predictedIons
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<AnnotationItem> ParseAnnotation(string text)
        {
            List<AnnotationItem> theAnnotations = new List<AnnotationItem>();

            string[] tmp = Regex.Split(text, "\n");

            try
            {
                foreach (string t in tmp)
                {
                    if (t.Length == 0) { continue; }
                    string[] cols = Regex.Split(t, "-");
                    AnnotationItem a = new AnnotationItem(cols[0], int.Parse(cols[1]));
                    theAnnotations.Add(a);
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Annotation does not seem to be in a correct format.");
            }

            return theAnnotations;
        }

        /// <summary>
        /// Class responsible for translating predicted Ions
        /// </summary>
        public class AnnotationItem
        {
            public string Aminoacid { get; set; }
            public int Position { get; set; }

            public AnnotationItem(string aminoacid, int position)
            {
                Aminoacid = aminoacid;
                Position = position;
            }
        }

        /// <summary>
        /// Method responsible for calling Paint Method
        /// </summary>
        public void RefreshPeptideAnotator()
        {
            panelAnnotator.Refresh();//Call Paint method
        }

        /// <summary>
        /// Method responsible for painting the panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelAnnotator_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (!String.IsNullOrEmpty(Protein))
            {
                //if (!String.IsNullOrEmpty(Peptides))
                //    this.DrawProteoformsPeptides(Protein, Peptides, AnotationAlfa);
                //else
                //    this.DrawIntraLink(Protein, AnotationAlfa, XlPos1, XlPos2);
            }
        }

        /// <summary>
        /// Method responsible for copying image to Clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(imageBuffer);
            return;

            //MemoryStream memoryStream = new MemoryStream();
            //Graphics mainGraphics = Graphics.FromImage(imageBuffer);
            //imageBuffer.Save(memoryStream, ImageFormat.Png);

            //IntPtr ipHdc = mainGraphics.GetHdc();
            //Metafile mf = new Metafile(memoryStream, ipHdc);
            //mainGraphics = Graphics.FromImage(mf);
            //mainGraphics.FillEllipse(Brushes.Gray, 0, 0, 100, 100);
            //mainGraphics.Dispose();
            //mf.Save(memoryStream, ImageFormat.Png);
            //IDataObject dataObject = new DataObject();
            //dataObject.SetData("PNG", false, memoryStream);
            //System.Windows.Forms.Clipboard.SetDataObject(dataObject, true);
        }

        /// <summary>
        /// Method responsible for calling save image method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePeptideAnnotation();
        }

        /// <summary>
        /// Method responsible for saving image
        /// </summary>
        public void SavePeptideAnnotation()
        {
            saveFileDialog1.Filter = "PNG graphics (*.png)|*.png";

            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                imageBuffer.Save(saveFileDialog1.FileName, ImageFormat.Png);
                MessageBox.Show("Peptide annotation saved successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void timerReset_Tick(object sender, EventArgs e)
        {
            toolTipAnnotation.RemoveAll();
            timerReset.Enabled = false;
        }
    }
}
