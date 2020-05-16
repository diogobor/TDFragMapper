using ProteinAnnotation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProteinAnnotation
{
    /// <summary>
    /// Interaction logic for ProteinAnnotatorUC.xaml
    /// </summary>
    public partial class ProteinAnnotatorWPF : UserControl
    {
        /// <summary>
        /// Constants
        /// </summary>
        private const int SPACER = 15;
        private const int FONTSIZE_PROTEINSEQUENCE = 20;
        private const int FONTSIZE_PROTEINSEQUENCE_PTM = 14;
        private const int FONTSIZE_NC_TERM_NUMBERS = 16;
        private const int X1OFFSET = 10;
        private const int WIDTH_LINE = 2;

        /// <summary>
        /// Local variables
        /// </summary>
        int STEP_PROTEIN_SEQUENCE = 50;
        private SolidColorBrush[] AminoAcidColor { get; set; }
        private string Protein { get; set; }
        private SolidColorBrush BRUSH_LINE_COLOR_PROTEOFORMS = new SolidColorBrush(Colors.DarkOrange);
        private SolidColorBrush BRUSH_LINE_COLOR_TRUNCATEDPROTEOFORMS = new SolidColorBrush(Colors.DarkCyan);
        private SolidColorBrush BRUSH_LINE_COLOR_BIOMARKER_PROTEOFORMS = new SolidColorBrush(Colors.Red);
        private SolidColorBrush BRUSH_LINE_COLOR_UNIQUEPEPTIDES = new SolidColorBrush(Colors.Blue);
        private SolidColorBrush BRUSH_LINE_COLOR_COMMONPEPTIDES = new SolidColorBrush(Colors.DarkBlue);
        private SolidColorBrush BRUSH_LINE_COLOR_THEORETICALPROTEIN = new SolidColorBrush(Colors.Gray);
        private List<(string, List<string>)> ProteoformsPeptides { get; set; }

        //Tuple<peptide, isUnique, List<score, tool>, List<proteoforms>, List<protein>, List<aminoacid, position, PTMdescription>>
        private List<Tuple<string, bool, List<Tuple<double, string>>, List<string>, List<string>, List<Tuple<string, int, string>>>> Peptides { get; set; }

        private bool IsSingleLine { get; set; }

        /// <summary>
        /// Public variables
        /// </summary>
        //Tuple<proteoform, score, tool, isValid, theoretical mass, modifications( List<Tuple<aminoacid, position, PTMdescription>>) >
        public List<(string, List<(double, string, bool, double)>)> AnnotationProteoforms { get; set; }

        public ProteinAnnotatorWPF()
        {
            InitializeComponent();
            #region Initialize Color array
            AminoAcidColor = new SolidColorBrush[51];
            AminoAcidColor[0] = new SolidColorBrush(Colors.Red);
            AminoAcidColor[1] = new SolidColorBrush(Colors.Purple);
            AminoAcidColor[2] = new SolidColorBrush(Colors.Green);
            AminoAcidColor[3] = new SolidColorBrush(Colors.DarkKhaki);
            AminoAcidColor[4] = new SolidColorBrush(Colors.MediumTurquoise);
            AminoAcidColor[5] = new SolidColorBrush(Colors.Brown);
            AminoAcidColor[6] = new SolidColorBrush(Colors.Salmon);
            AminoAcidColor[7] = new SolidColorBrush(Colors.DarkViolet);
            AminoAcidColor[8] = new SolidColorBrush(Colors.Navy);
            AminoAcidColor[9] = new SolidColorBrush(Colors.Gray);
            AminoAcidColor[10] = new SolidColorBrush(Colors.Maroon);
            AminoAcidColor[11] = new SolidColorBrush(Colors.Magenta);
            AminoAcidColor[12] = new SolidColorBrush(Colors.MediumAquamarine);
            AminoAcidColor[13] = new SolidColorBrush(Colors.LightSteelBlue);
            AminoAcidColor[14] = new SolidColorBrush(Colors.BurlyWood);
            AminoAcidColor[15] = new SolidColorBrush(Colors.Beige);
            AminoAcidColor[16] = new SolidColorBrush(Colors.Aqua);
            AminoAcidColor[17] = new SolidColorBrush(Colors.OliveDrab);
            AminoAcidColor[18] = new SolidColorBrush(Colors.OrangeRed);
            AminoAcidColor[19] = new SolidColorBrush(Colors.PaleGreen);
            AminoAcidColor[20] = new SolidColorBrush(Colors.PapayaWhip);
            AminoAcidColor[21] = new SolidColorBrush(Colors.Peru);
            AminoAcidColor[22] = new SolidColorBrush(Colors.Silver);
            AminoAcidColor[23] = new SolidColorBrush(Colors.SeaShell);
            AminoAcidColor[24] = new SolidColorBrush(Colors.SkyBlue);
            AminoAcidColor[25] = new SolidColorBrush(Colors.Turquoise);
            AminoAcidColor[26] = new SolidColorBrush(Colors.Tan);
            AminoAcidColor[27] = new SolidColorBrush(Colors.Teal);
            AminoAcidColor[28] = new SolidColorBrush(Colors.Thistle);
            AminoAcidColor[29] = new SolidColorBrush(Colors.Tomato);
            AminoAcidColor[30] = new SolidColorBrush(Colors.Transparent);
            AminoAcidColor[31] = new SolidColorBrush(Colors.Wheat);
            AminoAcidColor[32] = new SolidColorBrush(Colors.White);
            AminoAcidColor[33] = new SolidColorBrush(Colors.WhiteSmoke);
            AminoAcidColor[34] = new SolidColorBrush(Colors.Yellow);
            AminoAcidColor[35] = new SolidColorBrush(Colors.YellowGreen);
            AminoAcidColor[36] = new SolidColorBrush(Colors.AliceBlue);
            AminoAcidColor[37] = new SolidColorBrush(Colors.AntiqueWhite);
            AminoAcidColor[38] = new SolidColorBrush(Colors.Aqua);
            AminoAcidColor[39] = new SolidColorBrush(Colors.Aquamarine);
            AminoAcidColor[40] = new SolidColorBrush(Colors.Azure);
            AminoAcidColor[41] = new SolidColorBrush(Colors.Beige);
            AminoAcidColor[42] = new SolidColorBrush(Colors.Bisque);
            AminoAcidColor[43] = new SolidColorBrush(Colors.Black);
            AminoAcidColor[44] = new SolidColorBrush(Colors.BlanchedAlmond);
            AminoAcidColor[45] = new SolidColorBrush(Colors.Blue);
            AminoAcidColor[46] = new SolidColorBrush(Colors.BlueViolet);
            AminoAcidColor[47] = new SolidColorBrush(Colors.Brown);
            AminoAcidColor[48] = new SolidColorBrush(Colors.BurlyWood);
            AminoAcidColor[49] = new SolidColorBrush(Colors.CadetBlue);
            AminoAcidColor[50] = new SolidColorBrush(Colors.Chartreuse);
            #endregion
            AnnotationProteoforms = new List<(string, List<(double, string, bool, double)>)>();
        }

        private void SetCanvasScrollBarSize(double width = 4096, double height = 2028)
        {
            MyCanvas.Width = width;
            MyCanvas.Height = height;
        }

        public void PreparePictureProteinFragmentIons(bool isSingleLine, string proteinSequence, List<(string, int, string, int)> fragmentIons)
        {
            this.DrawProteinFragmentIons(isSingleLine, proteinSequence, fragmentIons);
        }

        public void DrawProteinFragmentIons(bool isSingleLine, string proteinSequence, List<(string, int, string, int)> fragmentIons)
        {
            //Tuple<proteinSequence, List<chain(or signal peptide), start position, end position>, List<modification-> ( description, position )>>


            double proteinWidth = 60 + 24 * (proteinSequence.Length + 2) + 20;
            //if (!isSingleLine)
            //{
            //    proteinWidth = 60 + 24 * (STEP_PROTEIN_SEQUENCE + 2) + 20;
            //    double proteinHeight = (proteinSequence.Length / STEP_PROTEIN_SEQUENCE) * (10 * (annotationProteoforms.Count + 2.5) + 10 * (annotationPeptides.Count + 2.5));
            //    proteinHeight = proteinHeight < 2028 ? 2028 : proteinHeight;
            //    this.SetCanvasScrollBarSize(proteinWidth, proteinHeight);
            //}
            //else
            this.SetCanvasScrollBarSize(proteinWidth);

            this.IsSingleLine = isSingleLine;

            #region Setting Values
            Protein = proteinSequence;
            //Peptides = annotationPeptides;
            double initialXLine = 0;
            double initialYLine = 0;

            //Setting protein font
            //List<int>[] matchLocationsTopDown = new List<int>[proteinSequence.Item1.Length];
            //List<int>[] matchLocationsBottomUp = new List<int>[proteinSequence.Item1.Length];
            //List<int>[] matchLocationsBottomUpUniquePepts = new List<int>[proteinSequence.Item1.Length];

            int maxValueTopDown = 0;
            int maxValueUniquePeptides = 0;
            int maxValueCommonPeptides = 0;

            SolidColorBrush labelBrush_N_C_Term = new SolidColorBrush(Colors.Gray);
            Label proteinNTerm = new Label();
            proteinNTerm.FontFamily = new FontFamily("Courier New");
            proteinNTerm.FontWeight = FontWeights.SemiBold;
            proteinNTerm.FontSize = FONTSIZE_NC_TERM_NUMBERS;
            proteinNTerm.Content = "N ";
            proteinNTerm.Foreground = labelBrush_N_C_Term;
            proteinNTerm.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            MyCanvas.Children.Add(proteinNTerm);
            Canvas.SetLeft(proteinNTerm, 20);
            Canvas.SetTop(proteinNTerm, 35);

            //var proteoformWithScore = (from protfm in annotationProteoforms
            //                           select protfm.Item1 + String.Join(",", protfm.Item2.Select(item => item.Item1))).ToList();

            if (isSingleLine)
            {
                #region Plot sequence
                //maxValueTopDown = annotationProteoforms.Count;

                SolidColorBrush labelBrush_PTN = new SolidColorBrush(Colors.Black);
                Label proteinLabel = new Label();
                proteinLabel.FontFamily = new FontFamily("Courier New");
                proteinLabel.FontWeight = FontWeights.Bold;
                proteinLabel.FontSize = FONTSIZE_PROTEINSEQUENCE;
                proteinLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
                StringBuilder sbPtnSeq = new StringBuilder();
                for (int i = 0; i < proteinSequence.Length; i++)
                    sbPtnSeq.Append(proteinSequence[i] + " ");
                proteinLabel.Content = sbPtnSeq.ToString();
                proteinLabel.Foreground = labelBrush_PTN;
                proteinLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                MyCanvas.Children.Add(proteinLabel);
                Canvas.SetLeft(proteinLabel, 60);
                Canvas.SetTop(proteinLabel, 35);

                initialXLine = Double.IsNaN(Canvas.GetLeft(proteinLabel)) ? 0 : Canvas.GetLeft(proteinLabel);
                initialYLine = Double.IsNaN(Canvas.GetTop(proteinLabel)) ? 25 : Canvas.GetTop(proteinLabel) + 25;

                Label proteinCTerm = new Label();
                proteinCTerm.FontFamily = new FontFamily("Courier New");
                proteinCTerm.FontWeight = FontWeights.SemiBold;
                proteinCTerm.FontSize = FONTSIZE_NC_TERM_NUMBERS;
                proteinCTerm.Content = "C";
                proteinCTerm.Foreground = labelBrush_N_C_Term;
                proteinCTerm.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                MyCanvas.Children.Add(proteinCTerm);
                Canvas.SetLeft(proteinCTerm, proteinLabel.DesiredSize.Width + 60);
                Canvas.SetTop(proteinCTerm, 35);

                #region plot aminoacid number on the top of the sequence

                //First aminoacid
                Label numberAATop1 = new Label();
                numberAATop1.FontFamily = new FontFamily("Courier New");
                numberAATop1.FontSize = FONTSIZE_PROTEINSEQUENCE_PTM - 2;
                numberAATop1.Content = 1;
                numberAATop1.Foreground = labelBrush_N_C_Term;
                numberAATop1.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                MyCanvas.Children.Add(numberAATop1);
                Canvas.SetLeft(numberAATop1, initialXLine);
                Canvas.SetTop(numberAATop1, initialYLine - 50);

                // Create a Rectangle  
                Rectangle AANumberRectangle = new Rectangle();
                AANumberRectangle.Height = 16;
                AANumberRectangle.Width = proteinSequence.Length * 24 + 10;
                // Create a blue and a black Brush  
                SolidColorBrush backgroundColor = new SolidColorBrush();
                backgroundColor.Color = Colors.LightGray;
                SolidColorBrush blackBrush = new SolidColorBrush();
                blackBrush.Color = Colors.Black;
                // Set Rectangle's width and color  
                AANumberRectangle.StrokeThickness = 0.5;

                // Fill rectangle with blue color  
                AANumberRectangle.Fill = backgroundColor;
                AANumberRectangle.Opacity = 0.20;
                // Add Rectangle to the Grid.  
                MyCanvas.Children.Add(AANumberRectangle);
                Canvas.SetLeft(AANumberRectangle, initialXLine);
                Canvas.SetTop(AANumberRectangle, initialYLine - 48);

                for (int i = 0; i <= proteinSequence.Length; i += 50)
                {
                    if (i % 50 == 0 && i > 0)
                    {
                        Label numberAATop = new Label();
                        numberAATop.FontFamily = new FontFamily("Courier New");
                        numberAATop.FontSize = FONTSIZE_PROTEINSEQUENCE_PTM - 2;
                        numberAATop.Content = i;
                        numberAATop.Foreground = labelBrush_N_C_Term;
                        numberAATop.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        MyCanvas.Children.Add(numberAATop);
                        Canvas.SetLeft(numberAATop, initialXLine + 24 * (i - 1) + 2);
                        Canvas.SetTop(numberAATop, initialYLine - 50);
                    }
                }

                if (proteinSequence.Length % 50 != 0)//Print the last AAnumber
                {
                    Label numberAATop = new Label();
                    numberAATop.FontFamily = new FontFamily("Courier New");
                    numberAATop.FontSize = FONTSIZE_PROTEINSEQUENCE_PTM - 2;
                    numberAATop.Content = proteinSequence.Length;
                    numberAATop.Foreground = labelBrush_N_C_Term;
                    numberAATop.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                    MyCanvas.Children.Add(numberAATop);
                    Canvas.SetLeft(numberAATop, initialXLine + 24 * (proteinSequence.Length - 1));
                    Canvas.SetTop(numberAATop, initialYLine - 50);
                }
                #endregion

                #endregion

                #region Plot lines

                #region Draw proteoform lines

                #region Context menu image
                //Creating image to put in contextmenu icon
                BitmapSource bmpSource = Imaging.CreateBitmapSourceFromHBitmap(
                   ProteinAnnotation.Properties.Resources.report.GetHbitmap(),
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

                BitmapSource userAssessmentBmp = Imaging.CreateBitmapSourceFromHBitmap(
                   ProteinAnnotation.Properties.Resources.userAssessment.GetHbitmap(),
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

                #endregion

                for (int countProteoforms = 1; countProteoforms <= maxValueTopDown; countProteoforms++)
                {
                    bool hasNtermMod = false;
                    int startProteoform = 0;
                    int endProteoform = proteinSequence.Length;
                    //int startProteoform = Array.FindIndex(matchLocationsTopDown, item => item != null && item.Contains(countProteoforms - 1));
                    //int endProteoform = Array.FindLastIndex(matchLocationsTopDown, item => item != null && item.Contains(countProteoforms - 1));
                    if (startProteoform == -1 || endProteoform == -1) continue;

                    //Drawing a line
                    Line l = new Line();
                    l.X1 = initialXLine + 24 * startProteoform;
                    l.X2 = initialXLine + 24 * endProteoform + 20;
                    l.Y1 = initialYLine + 10 * countProteoforms;
                    l.Y2 = initialYLine + 10 * countProteoforms;
                    l.Name = "_" + (countProteoforms - 1).ToString() + "_";

                    l.StrokeThickness = WIDTH_LINE;
                    //if (annotationProteoforms[countProteoforms - 1].Item3 == 3)//Intact Proteoform
                    //{
                    //    l.Stroke = BRUSH_LINE_COLOR_PROTEOFORMS;
                    //    sbPtms.Append("_topDown_intact_");
                    //}
                    //else if (annotationProteoforms[countProteoforms - 1].Item3 == 6)//Truncated Proteoform
                    //{
                    //    l.Stroke = BRUSH_LINE_COLOR_TRUNCATEDPROTEOFORMS;
                    //    sbPtms.Append("_topDown_truncated_");
                    //}
                    //else // 9 - Tagged Proteoform
                    //{
                    //    l.Stroke = BRUSH_LINE_COLOR_BIOMARKER_PROTEOFORMS;
                    //    sbPtms.Append("_topDown_biomarker_");
                    //}

                    //l.Tag = new object[] { sbPtms.ToString(), annotationProteoforms[countProteoforms - 1] };

                    l.MouseEnter += line_MouseEnter;
                    l.MouseLeave += line_MouseLeave;

                    StringBuilder sbToolTip = new StringBuilder();
                    //sbToolTip.AppendLine("Proteoform: " + annotationProteoforms[countProteoforms - 1].Item1);
                    sbToolTip.AppendLine("Start Position: " + (startProteoform + 1));
                    sbToolTip.AppendLine("End Position: " + (endProteoform + 1));
                    //if (annotationProteoforms[countProteoforms - 1].Item2.Count > 1)
                    //    sbToolTip.AppendLine();
                    //foreach (Tuple<double, string, bool, double, List<Tuple<string, int, string>>> param in annotationProteoforms[countProteoforms - 1].Item2)
                    //{
                    //    sbToolTip.AppendLine("Search Engine: " + param.Item2);
                    //    sbToolTip.AppendLine("Score: " + param.Item1);
                    //    sbToolTip.AppendLine();
                    //}
                    sbToolTip.Remove(sbToolTip.Length - 4, 4);
                    l.ToolTip = sbToolTip.ToString();

                    l.MouseLeftButtonDown += line_MouseDoubleClick;

                    #region Context Menu proteoform
                    ContextMenu cm = new ContextMenu();
                    MenuItem mi = new MenuItem();
                    mi.Header = "Select Peptides";
                    mi.Click += miSelectPeptides_Click;

                    #region creating peptide icon
                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Source = bmpSource;
                    mi.Icon = img;
                    #endregion

                    mi.Tag = new object[] { l };
                    cm.Items.Add(mi);
                    Separator separator = new Separator();
                    cm.Items.Add(separator);

                    MenuItem assessment = new MenuItem();
                    assessment.Header = "User assessment";
                    #region creating peptide icon
                    System.Windows.Controls.Image assessmentImg = new System.Windows.Controls.Image();
                    assessmentImg.Source = userAssessmentBmp;
                    assessment.Icon = assessmentImg;
                    #endregion

                    #region Assessment subItems
                    MenuItem subItemGood = new MenuItem();
                    subItemGood.Header = "Valid proteoform";
                    subItemGood.Click += subItemAssessment_Click;
                    subItemGood.Tag = new object[] { assessment, "Good", countProteoforms - 1 };
                    assessment.Items.Add(subItemGood);

                    MenuItem subItemBad = new MenuItem();
                    subItemBad.Header = "Invalid proteoform";
                    subItemBad.Click += subItemAssessment_Click;
                    subItemBad.Tag = new object[] { assessment, "Bad", countProteoforms - 1 };
                    assessment.Items.Add(subItemBad);

                    //if (annotationProteoforms[countProteoforms - 1].Item2[0].Item3)
                    //{
                    //    ((MenuItem)assessment.Items[0]).IsChecked = true;
                    //    ((MenuItem)assessment.Items[1]).IsChecked = false;
                    //}
                    //else
                    //{
                    //    ((MenuItem)assessment.Items[0]).IsChecked = false;
                    //    ((MenuItem)assessment.Items[1]).IsChecked = true;
                    //}

                    #endregion

                    cm.Items.Add(assessment);
                    l.ContextMenu = cm;

                    #endregion

                    MyCanvas.Children.Add(l);
                    Canvas.SetZIndex(l, -1);

                    if (hasNtermMod)
                    {
                        l = new Line();
                        l.X1 = 18;
                        l.X2 = initialXLine + 24 * startProteoform;
                        l.Y1 = initialYLine + 10 * countProteoforms;
                        l.Y2 = initialYLine + 10 * countProteoforms;
                        l.StrokeThickness = WIDTH_LINE - 1;
                        l.Stroke = BRUSH_LINE_COLOR_PROTEOFORMS;
                        l.Name = "_" + (countProteoforms - 1).ToString() + "_";
                        //l.Tag = new object[] { sbPtms.ToString(), annotationProteoforms[countProteoforms - 1] };

                        sbToolTip = new StringBuilder();
                        //sbToolTip.AppendLine("Proteoform: " + annotationProteoforms[countProteoforms - 1].Item1);
                        sbToolTip.AppendLine("Start Position: " + (startProteoform + 1));
                        sbToolTip.AppendLine("End Position: " + (endProteoform + 1));
                        //if (annotationProteoforms[countProteoforms - 1].Item2.Count > 1)
                        //    sbToolTip.AppendLine();
                        //foreach (Tuple<double, string, bool, double, List<Tuple<string, int, string>>> param in annotationProteoforms[countProteoforms - 1].Item2)
                        //{
                        //    sbToolTip.AppendLine("Search Engine: " + param.Item2);
                        //    sbToolTip.AppendLine("Score: " + param.Item1);
                        //    sbToolTip.AppendLine();
                        //}
                        sbToolTip.Remove(sbToolTip.Length - 4, 4);
                        l.ToolTip = sbToolTip.ToString();

                        l.StrokeDashArray = new DoubleCollection(new double[2] { 3, 1 });
                        MyCanvas.Children.Add(l);
                        Canvas.SetZIndex(l, -1);
                    }
                }

                #endregion


                #endregion

            }
            //else
            //{
            //    int stepOneByOne = 1;
            //    double lastProteinWidth = 0;
            //    List<Tuple<string, bool, List<Tuple<double, string>>, List<string>, List<string>, List<Tuple<string, int, string>>>> uniquePeptides = Peptides.Where(item => item.Item2).ToList();
            //    List<Tuple<string, bool, List<Tuple<double, string>>, List<string>, List<string>, List<Tuple<string, int, string>>>> commonPeptides = Peptides.Where(item => !item.Item2).ToList();
            //    int currentYlabel = 4;
            //    double lastYPlottedLine = 0;
            //    for (int proteinCount = 0; proteinCount < proteinSequence.Item1.Length; proteinCount += STEP_PROTEIN_SEQUENCE, stepOneByOne++)
            //    {
            //        List<int>[] valuesTopDown = null;
            //        List<int>[] valuesUniquePeptides = null;
            //        List<int>[] valuesCommonPeptides = null;

            //        #region Write Protein
            //        string currentSplitProtein = "";
            //        if (proteinCount + STEP_PROTEIN_SEQUENCE < proteinSequence.Item1.Length)
            //            currentSplitProtein = proteinSequence.Item1.Substring(proteinCount, STEP_PROTEIN_SEQUENCE);
            //        else
            //            currentSplitProtein = proteinSequence.Item1.Substring(proteinCount, proteinSequence.Item1.Length - proteinCount);
            //        SolidColorBrush labelBrush_PTN = new SolidColorBrush(Colors.Black);
            //        Label proteinLabel = new Label();
            //        proteinLabel.FontFamily = new FontFamily("Courier New");
            //        proteinLabel.FontWeight = FontWeights.Bold;
            //        proteinLabel.FontSize = FONTSIZE_PROTEINSEQUENCE;
            //        proteinLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
            //        StringBuilder sbPtnSeq = new StringBuilder();
            //        for (int j = 0; j < currentSplitProtein.Length; j++)
            //            sbPtnSeq.Append(currentSplitProtein[j] + " ");
            //        proteinLabel.Content = sbPtnSeq.ToString();
            //        proteinLabel.Foreground = labelBrush_PTN;
            //        proteinLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            //        MyCanvas.Children.Add(proteinLabel);
            //        Canvas.SetLeft(proteinLabel, 60);
            //        Canvas.SetTop(proteinLabel, 32);

            //        if (proteinCount > 0)
            //        {
            //            bool IsThereProteoformOrPeptideLines = false;
            //            #region Check if there is lines in the previous sequence to be printed
            //            valuesTopDown = matchLocationsTopDown.Skip((stepOneByOne - 2) * STEP_PROTEIN_SEQUENCE).Take(STEP_PROTEIN_SEQUENCE).Where(item => item != null).ToArray();
            //            if (valuesTopDown.Count() > 0)
            //                IsThereProteoformOrPeptideLines = true;

            //            if (!IsThereProteoformOrPeptideLines)
            //            {
            //                valuesUniquePeptides = matchLocationsBottomUpUniquePepts.Skip((stepOneByOne - 2) * STEP_PROTEIN_SEQUENCE).Take(STEP_PROTEIN_SEQUENCE).Where(item => item != null).ToArray();
            //                if (valuesUniquePeptides.Count() > 0)
            //                    IsThereProteoformOrPeptideLines = true;

            //                if (!IsThereProteoformOrPeptideLines)
            //                {
            //                    valuesCommonPeptides = matchLocationsBottomUp.Skip((stepOneByOne - 2) * STEP_PROTEIN_SEQUENCE).Take(STEP_PROTEIN_SEQUENCE).Where(item => item != null).ToArray();
            //                    if (valuesCommonPeptides.Count() > 0)
            //                        IsThereProteoformOrPeptideLines = true;
            //                }
            //            }
            //            #endregion

            //            #region Take values to know how many lines needs to be skipped
            //            var amountChains = proteinSequence.Item2.Where(item => item.Item2 >= (STEP_PROTEIN_SEQUENCE * (stepOneByOne - 1)) || item.Item3 <= (STEP_PROTEIN_SEQUENCE * stepOneByOne)).ToList().Count;
            //            currentYlabel = (int)lastYPlottedLine + 10;

            //            if (IsThereProteoformOrPeptideLines)
            //            {
            //                if (amountChains == 0)
            //                    currentYlabel += 20;
            //                else
            //                    currentYlabel += amountChains * 10;
            //            }
            //            else
            //            {
            //                if (amountChains > 0)
            //                    currentYlabel += amountChains * 5;
            //            }

            //            Canvas.SetTop(proteinLabel, currentYlabel);
            //            #endregion
            //        }
            //        valuesTopDown = matchLocationsTopDown.Skip(proteinCount).Take(STEP_PROTEIN_SEQUENCE).Where(item => item != null).ToArray();
            //        if (valuesTopDown.Count() > 0)
            //            maxValueTopDown = valuesTopDown.Select(item => item.Count).Max();

            //        valuesUniquePeptides = matchLocationsBottomUpUniquePepts.Skip(proteinCount).Take(STEP_PROTEIN_SEQUENCE).Where(item => item != null).ToArray();
            //        if (valuesUniquePeptides.Count() > 0)
            //            maxValueUniquePeptides = valuesUniquePeptides.Select(item => item.Count).Max();

            //        valuesCommonPeptides = matchLocationsBottomUp.Skip(proteinCount).Take(STEP_PROTEIN_SEQUENCE).Where(item => item != null).ToArray();
            //        if (valuesCommonPeptides.Count() > 0)
            //            maxValueCommonPeptides = valuesCommonPeptides.Select(item => item.Count).Max();

            //        initialXLine = Double.IsNaN(Canvas.GetLeft(proteinLabel)) ? 0 : Canvas.GetLeft(proteinLabel);
            //        initialYLine = Double.IsNaN(Canvas.GetTop(proteinLabel)) ? 25 : Canvas.GetTop(proteinLabel) + 25;
            //        lastProteinWidth = proteinLabel.DesiredSize.Width;

            //        Label stepProteinLabelC_Term = new Label();
            //        stepProteinLabelC_Term.FontFamily = new FontFamily("Courier New");
            //        stepProteinLabelC_Term.FontWeight = FontWeights.SemiBold;
            //        stepProteinLabelC_Term.FontSize = FONTSIZE_NC_TERM_NUMBERS;
            //        stepProteinLabelC_Term.Foreground = labelBrush_N_C_Term;
            //        stepProteinLabelC_Term.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));

            //        Label stepProteinLabelN_Term = new Label();
            //        stepProteinLabelN_Term.FontFamily = new FontFamily("Courier New");
            //        stepProteinLabelN_Term.FontWeight = FontWeights.SemiBold;
            //        stepProteinLabelN_Term.FontSize = FONTSIZE_NC_TERM_NUMBERS;
            //        stepProteinLabelN_Term.Foreground = labelBrush_N_C_Term;
            //        stepProteinLabelN_Term.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));

            //        if (proteinCount + STEP_PROTEIN_SEQUENCE < proteinSequence.Item1.Length)
            //        {
            //            stepProteinLabelC_Term.Content = (STEP_PROTEIN_SEQUENCE * stepOneByOne).ToString("000");
            //            MyCanvas.Children.Add(stepProteinLabelC_Term);
            //            Canvas.SetLeft(stepProteinLabelC_Term, lastProteinWidth + initialXLine);
            //            if (proteinCount == 0)
            //                Canvas.SetTop(stepProteinLabelC_Term, currentYlabel + 28);
            //            else
            //                Canvas.SetTop(stepProteinLabelC_Term, currentYlabel + 2);

            //            if (proteinCount != 0)
            //            {
            //                stepProteinLabelN_Term.Content = ((STEP_PROTEIN_SEQUENCE * (stepOneByOne - 1)) + 1).ToString("000");
            //                MyCanvas.Children.Add(stepProteinLabelN_Term);
            //                Canvas.SetLeft(stepProteinLabelN_Term, 15);
            //                Canvas.SetTop(stepProteinLabelN_Term, currentYlabel + 2);
            //            }
            //        }
            //        else
            //        {
            //            Label proteinCTerm = new Label();
            //            proteinCTerm.FontFamily = new FontFamily("Courier New");
            //            proteinCTerm.FontWeight = FontWeights.SemiBold;
            //            proteinCTerm.FontSize = FONTSIZE_NC_TERM_NUMBERS;
            //            proteinCTerm.Content = " C";
            //            proteinCTerm.Foreground = labelBrush_N_C_Term;
            //            proteinCTerm.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            //            MyCanvas.Children.Add(proteinCTerm);
            //            Canvas.SetLeft(proteinCTerm, lastProteinWidth + initialXLine);
            //            if (proteinCount == 0)
            //                Canvas.SetTop(proteinCTerm, currentYlabel);
            //            else
            //                Canvas.SetTop(proteinCTerm, currentYlabel + 2);

            //            if (proteinCount != 0)
            //            {
            //                stepProteinLabelN_Term.Content = ((STEP_PROTEIN_SEQUENCE * (stepOneByOne - 1)) + 1).ToString("000");
            //                MyCanvas.Children.Add(stepProteinLabelN_Term);
            //                Canvas.SetLeft(stepProteinLabelN_Term, 15);
            //                Canvas.SetTop(stepProteinLabelN_Term, currentYlabel + 2);
            //            }
            //        }

            //        #region Plot theoretical PTMs

            //        var resultantList = proteinSequence.Item3.Where(item => item.Item2 >= (STEP_PROTEIN_SEQUENCE * (stepOneByOne - 1)) && item.Item2 <= (STEP_PROTEIN_SEQUENCE * stepOneByOne))
            //            .GroupBy(s => s.Item2)
            //            .Select(grp => grp.ToList())
            //             .ToList();

            //        foreach (List<Tuple<string, int>> ptms in resultantList)
            //        {
            //            string description = String.Join("\n", (from ptm in ptms
            //                                                    select ptm.Item1));
            //            Label theoreticalPtmLab = new Label();
            //            theoreticalPtmLab.FontFamily = new FontFamily("Courier New");
            //            theoreticalPtmLab.FontSize = FONTSIZE_PROTEINSEQUENCE_PTM;
            //            theoreticalPtmLab.ToolTip = "PTM(s):\n" + description + "\n\nPosition: " + ptms[0].Item2;
            //            theoreticalPtmLab.Content = proteinSequence.Item1[ptms[0].Item2 - 1].ToString().ToLower();
            //            theoreticalPtmLab.Foreground = labelBrush_N_C_Term;
            //            theoreticalPtmLab.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            //            theoreticalPtmLab.MouseEnter += ptm_MouseEnter;
            //            theoreticalPtmLab.MouseLeave += ptm_MouseLeave;
            //            MyCanvas.Children.Add(theoreticalPtmLab);
            //            Canvas.SetLeft(theoreticalPtmLab, initialXLine + 24 * ((ptms[0].Item2 - 1) - (STEP_PROTEIN_SEQUENCE * (stepOneByOne - 1))) + 2);
            //            Canvas.SetTop(theoreticalPtmLab, initialYLine - 38);
            //        }

            //        #endregion

            //        #region Draw chains

            //        double chainYLine = initialYLine;
            //        foreach (Tuple<string, int, int> proteinChain in proteinSequence.Item2)
            //        {
            //            int startProteoformOriginal = proteinChain.Item2 - 1;
            //            if (startProteoformOriginal > proteinCount + STEP_PROTEIN_SEQUENCE - 1) continue;//Peptide started after limit per line
            //            int startProteoform = startProteoformOriginal - ((stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE);
            //            int endProteoformOriginal = proteinChain.Item3;
            //            int endProteoform = endProteoformOriginal - ((stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE);
            //            if (endProteoformOriginal > proteinCount + STEP_PROTEIN_SEQUENCE - 1)
            //                endProteoform = STEP_PROTEIN_SEQUENCE;
            //            if (startProteoform <= -1 && endProteoform <= -1) continue;
            //            else if (startProteoform <= -1 && endProteoformOriginal > 0)//Proteoform started in a previous line
            //                startProteoform = 0;

            //            //Drawing a line
            //            Line chain = new Line();
            //            chain.X1 = initialXLine + 24 * startProteoform;
            //            chain.X2 = initialXLine + 24 * endProteoform;
            //            chain.Y1 = chainYLine + 10;
            //            chain.Y2 = chainYLine + 10;
            //            chain.StrokeThickness = 1.8;
            //            chain.Stroke = BRUSH_LINE_COLOR_THEORETICALPROTEIN;
            //            chain.StrokeDashArray = new DoubleCollection(new double[2] { 3, 1 });
            //            chain.ToolTip = proteinChain.Item1 + " [" + proteinChain.Item2 + " - " + proteinChain.Item3 + "]";
            //            chain.MouseEnter += line_MouseEnter;
            //            chain.MouseLeave += line_MouseLeave;
            //            chain.Tag = new object[] { "_alwaysAppear_" };
            //            chain.Name = "_" + proteinChain.Item2 + "_" + proteinChain.Item3 + "_";
            //            MyCanvas.Children.Add(chain);
            //            Canvas.SetZIndex(chain, -1);
            //            chainYLine += 5;
            //        }

            //        #region left bracket
            //        Line leftBodyBracket = new Line();
            //        leftBodyBracket.X1 = 15;
            //        leftBodyBracket.X2 = 15;
            //        leftBodyBracket.Y1 = initialYLine > lastYPlottedLine ? initialYLine - 35 : lastYPlottedLine - 35;
            //        leftBodyBracket.Y2 = chainYLine + 20;
            //        leftBodyBracket.StrokeThickness = 1.8;
            //        leftBodyBracket.Stroke = BRUSH_LINE_COLOR_THEORETICALPROTEIN;
            //        leftBodyBracket.Tag = new object[] { "_alwaysAppear_" };
            //        MyCanvas.Children.Add(leftBodyBracket);

            //        Line leftTopBracket = new Line();
            //        leftTopBracket.X1 = 15;
            //        leftTopBracket.X2 = 20;
            //        leftTopBracket.Y1 = initialYLine > lastYPlottedLine ? initialYLine - 35 : lastYPlottedLine - 35;
            //        leftTopBracket.Y2 = initialYLine > lastYPlottedLine ? initialYLine - 35 : lastYPlottedLine - 35;
            //        leftTopBracket.StrokeThickness = 1.8;
            //        leftTopBracket.Stroke = BRUSH_LINE_COLOR_THEORETICALPROTEIN;
            //        leftTopBracket.Tag = new object[] { "_alwaysAppear_" };
            //        MyCanvas.Children.Add(leftTopBracket);

            //        Line leftBottomBracket = new Line();
            //        leftBottomBracket.X1 = 15;
            //        leftBottomBracket.X2 = 20;
            //        leftBottomBracket.Y1 = chainYLine + 20;
            //        leftBottomBracket.Y2 = chainYLine + 20;
            //        leftBottomBracket.StrokeThickness = 1.8;
            //        leftBottomBracket.Stroke = BRUSH_LINE_COLOR_THEORETICALPROTEIN;
            //        leftBottomBracket.Tag = new object[] { "_alwaysAppear_" };
            //        MyCanvas.Children.Add(leftBottomBracket);
            //        #endregion

            //        #region right bracket
            //        Line rightBodyBracket = new Line();
            //        rightBodyBracket.X1 = initialXLine + 24 * STEP_PROTEIN_SEQUENCE + 55;
            //        rightBodyBracket.X2 = initialXLine + 24 * STEP_PROTEIN_SEQUENCE + 55;
            //        rightBodyBracket.Y1 = initialYLine > lastYPlottedLine ? initialYLine - 35 : lastYPlottedLine - 35;
            //        rightBodyBracket.Y2 = chainYLine + 20;
            //        rightBodyBracket.StrokeThickness = 1.8;
            //        rightBodyBracket.Stroke = BRUSH_LINE_COLOR_THEORETICALPROTEIN;
            //        rightBodyBracket.Tag = new object[] { "_alwaysAppear_" };
            //        MyCanvas.Children.Add(rightBodyBracket);

            //        Line rightTopBracket = new Line();
            //        rightTopBracket.X1 = initialXLine + 24 * STEP_PROTEIN_SEQUENCE + 50;
            //        rightTopBracket.X2 = initialXLine + 24 * STEP_PROTEIN_SEQUENCE + 55;
            //        rightTopBracket.Y1 = initialYLine > lastYPlottedLine ? initialYLine - 35 : lastYPlottedLine - 35;
            //        rightTopBracket.Y2 = initialYLine > lastYPlottedLine ? initialYLine - 35 : lastYPlottedLine - 35;
            //        rightTopBracket.StrokeThickness = 1.8;
            //        rightTopBracket.Stroke = BRUSH_LINE_COLOR_THEORETICALPROTEIN;
            //        rightTopBracket.Tag = new object[] { "_alwaysAppear_" };
            //        MyCanvas.Children.Add(rightTopBracket);

            //        Line rightBottomBracket = new Line();
            //        rightBottomBracket.X1 = initialXLine + 24 * STEP_PROTEIN_SEQUENCE + 50;
            //        rightBottomBracket.X2 = initialXLine + 24 * STEP_PROTEIN_SEQUENCE + 55;
            //        rightBottomBracket.Y1 = chainYLine + 20;
            //        rightBottomBracket.Y2 = chainYLine + 20;
            //        rightBottomBracket.StrokeThickness = 1.8;
            //        rightBottomBracket.Stroke = BRUSH_LINE_COLOR_THEORETICALPROTEIN;
            //        rightBottomBracket.Tag = new object[] { "_alwaysAppear_" };
            //        MyCanvas.Children.Add(rightBottomBracket);
            //        #endregion

            //        initialYLine = chainYLine + 35;
            //        #endregion

            //        #endregion

            //        #region Draw proteoform lines

            //        #region Context menu image
            //        //Creating image to put in contextmenu icon
            //        BitmapSource bmpSource = Imaging.CreateBitmapSourceFromHBitmap(
            //           ProteinAnnotation.Properties.Resources.report.GetHbitmap(),
            //           IntPtr.Zero,
            //           Int32Rect.Empty,
            //           BitmapSizeOptions.FromEmptyOptions());

            //        BitmapSource userAssessmentBmp = Imaging.CreateBitmapSourceFromHBitmap(
            //           ProteinAnnotation.Properties.Resources.userAssessment.GetHbitmap(),
            //           IntPtr.Zero,
            //           Int32Rect.Empty,
            //           BitmapSizeOptions.FromEmptyOptions());

            //        #endregion

            //        for (int countProteoforms = 1; countProteoforms <= maxValueTopDown; countProteoforms++)
            //        {
            //            bool hasNtermMod = false;

            //            #region Check sequence bounds 
            //            int startProteoformOriginal = Array.FindIndex(matchLocationsTopDown, item => item != null && item.Contains(countProteoforms - 1));
            //            if (startProteoformOriginal > proteinCount + STEP_PROTEIN_SEQUENCE - 1) continue;//Peptide started after limit per line
            //            int startProteoform = startProteoformOriginal - ((stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE);
            //            int endProteoformOriginal = Array.FindLastIndex(matchLocationsTopDown, item => item != null && item.Contains(countProteoforms - 1));
            //            int endProteoform = endProteoformOriginal - ((stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE);
            //            if (endProteoformOriginal > proteinCount + STEP_PROTEIN_SEQUENCE - 1)
            //                endProteoform = STEP_PROTEIN_SEQUENCE - 1;
            //            if (startProteoform <= -1 && endProteoform <= -1) continue;
            //            else if (startProteoform <= -1 && endProteoformOriginal > 0)//Proteoform started in a previous line
            //                startProteoform = 0;
            //            #endregion

            //            List<Tuple<string, int, string>> mods = annotationProteoforms[countProteoforms - 1].Item2[0].Item5;
            //            StringBuilder sbPtms = new StringBuilder();
            //            foreach (Tuple<string, int, string> mod in mods)
            //            {
            //                if (mod.Item2 >= startProteoformOriginal && mod.Item2 <= endProteoformOriginal + 1)
            //                {
            //                    SolidColorBrush labelBrush_PTM = new SolidColorBrush(Colors.DarkRed);
            //                    Label ptmLabel = new Label();
            //                    ptmLabel.FontFamily = new FontFamily("Courier New");
            //                    ptmLabel.FontSize = FONTSIZE_PROTEINSEQUENCE_PTM;
            //                    ptmLabel.LayoutTransform = new System.Windows.Media.ScaleTransform(1.0, 1.0);
            //                    if (mod.Item1.Equals("}"))
            //                    {
            //                        ptmLabel.ToolTip = "Amino acid: N-Terminal\nPosition: " + mod.Item2 + "\nDescription: " + mod.Item3;
            //                        ptmLabel.Content = "n";
            //                        hasNtermMod = true;
            //                    }
            //                    else
            //                    {
            //                        ptmLabel.ToolTip = "Amino acid: " + mod.Item1 + "\nPosition: " + mod.Item2 + "\nDescription: " + mod.Item3;
            //                        ptmLabel.Content = mod.Item1.ToLower();
            //                    }
            //                    int description_ptm = Array.IndexOf(PTMs_color, mod.Item3);
            //                    if (description_ptm > -1)
            //                        ptmLabel.Foreground = AminoAcidColor[description_ptm];
            //                    else
            //                        ptmLabel.Foreground = labelBrush_PTM;
            //                    ptmLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            //                    ptmLabel.MouseEnter += ptm_MouseEnter;
            //                    ptmLabel.MouseLeave += ptm_MouseLeave;

            //                    sbPtms.Append("_" + mod.Item3 + "_");

            //                    if (mod.Item2 >= (stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE &&
            //                        mod.Item2 <= stepOneByOne * STEP_PROTEIN_SEQUENCE)//Check if ptm position is in the current protein line
            //                    {
            //                        MyCanvas.Children.Add(ptmLabel);
            //                        if (mod.Item1.Equals("}"))
            //                            Canvas.SetLeft(ptmLabel, 20);
            //                        else
            //                            Canvas.SetLeft(ptmLabel, initialXLine + 24 * ((mod.Item2 - (stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE) - 1));
            //                        Canvas.SetTop(ptmLabel, initialYLine + 10 * (countProteoforms - 1) - 4);
            //                        Canvas.SetZIndex(ptmLabel, 1);
            //                    }
            //                }
            //            }

            //            //Drawing a line
            //            Line l = new Line();
            //            l.X1 = initialXLine + 24 * startProteoform;
            //            l.X2 = initialXLine + 24 * endProteoform + 20;
            //            l.Y1 = initialYLine + 10 * countProteoforms;
            //            l.Y2 = initialYLine + 10 * countProteoforms;
            //            l.Name = "_" + (countProteoforms - 1).ToString() + "_";

            //            l.StrokeThickness = WIDTH_LINE;
            //            if (annotationProteoforms[countProteoforms - 1].Item3 == 3)//Intact Proteoform
            //            {
            //                l.Stroke = BRUSH_LINE_COLOR_PROTEOFORMS;
            //                sbPtms.Append("_topDown_intact_");
            //            }
            //            else if (annotationProteoforms[countProteoforms - 1].Item3 == 6)//Truncated Proteoform
            //            {
            //                l.Stroke = BRUSH_LINE_COLOR_TRUNCATEDPROTEOFORMS;
            //                sbPtms.Append("_topDown_truncated_");
            //            }
            //            else // 9 - Tagged Proteoform
            //            {
            //                l.Stroke = BRUSH_LINE_COLOR_BIOMARKER_PROTEOFORMS;
            //                sbPtms.Append("_topDown_biomarker_");
            //            }

            //            l.Tag = new object[] { sbPtms.ToString() };

            //            l.MouseEnter += line_MouseEnter;
            //            l.MouseLeave += line_MouseLeave;

            //            StringBuilder sbToolTip = new StringBuilder();
            //            sbToolTip.AppendLine("Proteoform: " + annotationProteoforms[countProteoforms - 1].Item1);
            //            sbToolTip.AppendLine("Start Position: " + (startProteoformOriginal + 1));
            //            sbToolTip.AppendLine("End Position: " + (endProteoformOriginal + 1));
            //            if (annotationProteoforms[countProteoforms - 1].Item2.Count > 1)
            //                sbToolTip.AppendLine();
            //            foreach (Tuple<double, string, bool, double, List<Tuple<string, int, string>>> param in annotationProteoforms[countProteoforms - 1].Item2)
            //            {
            //                sbToolTip.AppendLine("Search Engine: " + param.Item2);
            //                sbToolTip.AppendLine("Score: " + param.Item1);
            //                sbToolTip.AppendLine();
            //            }
            //            sbToolTip.Remove(sbToolTip.Length - 4, 4);
            //            l.ToolTip = sbToolTip.ToString();

            //            #region Context Menu proteoform
            //            ContextMenu cm = new ContextMenu();
            //            MenuItem mi = new MenuItem();
            //            mi.Header = "Select Peptides";
            //            mi.Click += miSelectPeptides_Click;

            //            #region creating peptide icon
            //            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            //            img.Source = bmpSource;
            //            mi.Icon = img;
            //            #endregion

            //            mi.Tag = new object[] { l };
            //            cm.Items.Add(mi);
            //            Separator separator = new Separator();
            //            cm.Items.Add(separator);

            //            MenuItem assessment = new MenuItem();
            //            assessment.Header = "User assessment";
            //            #region creating peptide icon
            //            System.Windows.Controls.Image assessmentImg = new System.Windows.Controls.Image();
            //            assessmentImg.Source = userAssessmentBmp;
            //            assessment.Icon = assessmentImg;
            //            #endregion

            //            #region Assessment subItems
            //            MenuItem subItemGood = new MenuItem();
            //            subItemGood.Header = "Valid proteoform";
            //            subItemGood.Click += subItemAssessment_Click;
            //            subItemGood.Tag = new object[] { assessment, "Good", countProteoforms - 1 };
            //            assessment.Items.Add(subItemGood);

            //            MenuItem subItemBad = new MenuItem();
            //            subItemBad.Header = "Invalid proteoform";
            //            subItemBad.Click += subItemAssessment_Click;
            //            subItemBad.Tag = new object[] { assessment, "Bad", countProteoforms - 1 };
            //            assessment.Items.Add(subItemBad);

            //            if (annotationProteoforms[countProteoforms - 1].Item2[0].Item3)
            //            {
            //                ((MenuItem)assessment.Items[0]).IsChecked = true;
            //                ((MenuItem)assessment.Items[1]).IsChecked = false;
            //            }
            //            else
            //            {
            //                ((MenuItem)assessment.Items[0]).IsChecked = false;
            //                ((MenuItem)assessment.Items[1]).IsChecked = true;
            //            }

            //            #endregion
            //            cm.Items.Add(assessment);

            //            l.ContextMenu = cm;

            //            #endregion

            //            MyCanvas.Children.Add(l);
            //            Canvas.SetZIndex(l, -1);

            //            if ((startProteoformOriginal - ((stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE)) > -1 && //It means that modifications happen in the same line
            //                hasNtermMod)
            //            {
            //                l = new Line();
            //                l.X1 = 18;
            //                l.X2 = initialXLine + 24 * startProteoform;
            //                l.Y1 = initialYLine + 10 * countProteoforms;
            //                l.Y2 = initialYLine + 10 * countProteoforms;
            //                l.StrokeThickness = WIDTH_LINE - 1;
            //                l.Stroke = BRUSH_LINE_COLOR_PROTEOFORMS;
            //                //l.MouseEnter += n_term_line_MouseEnter;
            //                //l.MouseLeave += n_term_line_MouseLeave;
            //                l.Name = "_" + (countProteoforms - 1).ToString() + "_";
            //                l.Tag = new object[] { sbPtms.ToString() };

            //                sbToolTip = new StringBuilder();
            //                sbToolTip.AppendLine("Proteoform: " + annotationProteoforms[countProteoforms - 1].Item1);
            //                sbToolTip.AppendLine("Start Position: " + (startProteoformOriginal + 1));
            //                sbToolTip.AppendLine("End Position: " + (endProteoformOriginal + 1));
            //                if (annotationProteoforms[countProteoforms - 1].Item2.Count > 1)
            //                    sbToolTip.AppendLine();
            //                foreach (Tuple<double, string, bool, double, List<Tuple<string, int, string>>> param in annotationProteoforms[countProteoforms - 1].Item2)
            //                {
            //                    sbToolTip.AppendLine("Search Engine: " + param.Item2);
            //                    sbToolTip.AppendLine("Score: " + param.Item1);
            //                    sbToolTip.AppendLine();
            //                }
            //                sbToolTip.Remove(sbToolTip.Length - 4, 4);
            //                l.ToolTip = sbToolTip.ToString();

            //                l.StrokeDashArray = new DoubleCollection(new double[2] { 3, 1 });
            //                MyCanvas.Children.Add(l);
            //                Canvas.SetZIndex(l, -1);
            //            }
            //        }

            //        #endregion

            //        double lastYProteoform = initialYLine + 10 * maxValueTopDown;
            //        double lastYUniquePeptide = initialYLine + 10 * maxValueTopDown;

            //        //Get the last Y line position to plot new protein sequence
            //        lastYPlottedLine = lastYUniquePeptide;

            //        #region Draw unique peptides

            //        for (int countPeptides = 1; countPeptides <= uniquePeptides.Count; countPeptides++)
            //        {
            //            bool hasNtermMod = false;

            //            #region Check sequence bounds 
            //            int startUniquePeptideOriginal = Array.FindIndex(matchLocationsBottomUpUniquePepts, item => item != null && item.Contains(countPeptides - 1));
            //            if (startUniquePeptideOriginal > proteinCount + STEP_PROTEIN_SEQUENCE - 1) continue;//Peptide started after limit per line
            //            int startUniquePeptide = startUniquePeptideOriginal - ((stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE);
            //            int endUniquePeptideOriginal = Array.FindLastIndex(matchLocationsBottomUpUniquePepts, item => item != null && item.Contains(countPeptides - 1));
            //            int endUniquePeptide = endUniquePeptideOriginal - ((stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE);
            //            if (endUniquePeptideOriginal > proteinCount + STEP_PROTEIN_SEQUENCE - 1)
            //                endUniquePeptide = STEP_PROTEIN_SEQUENCE - 1;
            //            if (startUniquePeptide <= -1 && endUniquePeptide <= -1) continue;
            //            else if (startUniquePeptide <= -1 && endUniquePeptideOriginal > 0)//Peptide started in a previous line
            //                startUniquePeptide = 0;
            //            #endregion

            //            Tuple<string, bool, List<Tuple<double, string>>, List<string>, List<string>, List<Tuple<string, int, string>>> currentPeptide = uniquePeptides[countPeptides - 1];
            //            foreach (Tuple<string, int, string> mod in currentPeptide.Item6)
            //            {
            //                if (mod.Item2 >= startUniquePeptideOriginal && mod.Item2 <= endUniquePeptideOriginal)
            //                {
            //                    SolidColorBrush labelBrush_PTM = new SolidColorBrush(Colors.DarkRed);
            //                    Label ptmLabel = new Label();
            //                    ptmLabel.FontFamily = new FontFamily("Courier New");
            //                    ptmLabel.FontSize = FONTSIZE_PROTEINSEQUENCE_PTM;
            //                    if (mod.Item1.Equals("}"))
            //                    {
            //                        ptmLabel.ToolTip = "Amino acid: N-Terminal\nPosition: " + mod.Item2 + "\nDescription: " + mod.Item3;
            //                        ptmLabel.Content = "n";
            //                        hasNtermMod = true;
            //                    }
            //                    else
            //                    {
            //                        ptmLabel.ToolTip = "Amino acid: " + mod.Item1 + "\nPosition: " + mod.Item2 + "\nDescription: " + mod.Item3;
            //                        ptmLabel.Content = mod.Item1.ToLower();
            //                    }

            //                    int description_ptm = Array.IndexOf(PTMs_color, mod.Item3);
            //                    if (description_ptm > -1)
            //                        ptmLabel.Foreground = AminoAcidColor[description_ptm];
            //                    else
            //                        ptmLabel.Foreground = labelBrush_PTM;
            //                    ptmLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            //                    ptmLabel.MouseEnter += ptm_MouseEnter;
            //                    ptmLabel.MouseLeave += ptm_MouseLeave;

            //                    if (mod.Item2 >= (stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE &&
            //                        mod.Item2 <= stepOneByOne * STEP_PROTEIN_SEQUENCE)//Check if ptm position is in the current protein line
            //                    {
            //                        MyCanvas.Children.Add(ptmLabel);
            //                        if (mod.Item1.Equals("}"))
            //                            Canvas.SetLeft(ptmLabel, 20);
            //                        else
            //                            Canvas.SetLeft(ptmLabel, initialXLine + 24 * ((mod.Item2 - (stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE) - 1));
            //                        Canvas.SetTop(ptmLabel, (lastYProteoform + 10 * (countPeptides - 1)) - 4);
            //                        Canvas.SetZIndex(ptmLabel, 1);
            //                    }
            //                }
            //            }

            //            //Drawing a line
            //            Line l = new Line();
            //            l.X1 = initialXLine + 24 * startUniquePeptide;
            //            l.X2 = initialXLine + 24 * endUniquePeptide + 20;
            //            l.Y1 = lastYProteoform + 10 * countPeptides;
            //            l.Y2 = lastYProteoform + 10 * countPeptides;
            //            lastYUniquePeptide = l.Y1;
            //            l.StrokeThickness = WIDTH_LINE;
            //            l.Stroke = BRUSH_LINE_COLOR_UNIQUEPEPTIDES;
            //            l.MouseEnter += line_MouseEnter;
            //            l.MouseLeave += line_MouseLeave;

            //            StringBuilder sbId = new StringBuilder();
            //            foreach (string proteoform in currentPeptide.Item4)
            //            {
            //                int indexProteoformList = proteoformWithScore.FindIndex(item => item.Equals(proteoform));
            //                sbId.Append("_");
            //                if (indexProteoformList > -1)
            //                    sbId.Append(indexProteoformList);
            //            }
            //            sbId.Append("_");
            //            l.Name = sbId.ToString();

            //            StringBuilder sbPtms = new StringBuilder();
            //            foreach (Tuple<string, int, string> mod in currentPeptide.Item6)
            //                sbPtms.Append("_" + mod.Item3 + "_");
            //            sbPtms.Append("_bottomUp_");
            //            l.Tag = new object[] { sbPtms.ToString() };

            //            StringBuilder sbToolTip = new StringBuilder();
            //            sbToolTip.AppendLine("Unique Peptide:\n" + currentPeptide.Item1);
            //            sbToolTip.AppendLine("Protein(s): " + String.Join(",\n                 ", currentPeptide.Item5.Distinct()));
            //            sbToolTip.AppendLine("Start Position: " + (startUniquePeptideOriginal + 1));
            //            sbToolTip.AppendLine("End Position: " + (endUniquePeptideOriginal + 1));

            //            foreach (Tuple<double, string> score in currentPeptide.Item3)
            //            {
            //                sbToolTip.AppendLine("Search Engine: " + score.Item2);
            //                sbToolTip.AppendLine("Score: " + score.Item1.ToString("0.00000"));
            //                sbToolTip.AppendLine();
            //            }
            //            sbToolTip.Remove(sbToolTip.Length - 4, 4);
            //            l.ToolTip = sbToolTip.ToString();
            //            MyCanvas.Children.Add(l);
            //            Canvas.SetZIndex(l, -1);

            //            if ((startUniquePeptideOriginal - ((stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE)) > -1 && //It means that modifications hapen in the same line
            //                hasNtermMod)
            //            {
            //                l = new Line();
            //                l.X1 = 18;
            //                l.X2 = initialXLine + 24 * startUniquePeptide;
            //                l.Y1 = lastYProteoform + 10 * countPeptides;
            //                l.Y2 = lastYProteoform + 10 * countPeptides;
            //                l.StrokeThickness = WIDTH_LINE - 1;
            //                l.Stroke = BRUSH_LINE_COLOR_UNIQUEPEPTIDES;
            //                //l.MouseEnter += n_term_line_MouseEnter;
            //                //l.MouseLeave += n_term_line_MouseLeave;
            //                l.Name = sbId.ToString();
            //                l.Tag = new object[] { sbPtms.ToString() };
            //                sbToolTip = new StringBuilder();
            //                sbToolTip.AppendLine("Unique Peptide:\n" + currentPeptide.Item1);
            //                sbToolTip.AppendLine("Protein(s): " + String.Join(",\n                 ", currentPeptide.Item5.Distinct()));
            //                sbToolTip.AppendLine("Start Position: " + (startUniquePeptideOriginal + 1));
            //                sbToolTip.AppendLine("End Position: " + (endUniquePeptideOriginal + 1));

            //                foreach (Tuple<double, string> score in currentPeptide.Item3)
            //                {
            //                    sbToolTip.AppendLine("Search Engine: " + score.Item2);
            //                    sbToolTip.AppendLine("Score: " + score.Item1.ToString("0.00000"));
            //                    sbToolTip.AppendLine();
            //                }
            //                sbToolTip.Remove(sbToolTip.Length - 4, 4);
            //                l.ToolTip = sbToolTip.ToString();
            //                l.StrokeDashArray = new DoubleCollection(new double[2] { 3, 1 });
            //                MyCanvas.Children.Add(l);
            //                Canvas.SetZIndex(l, -1);
            //            }
            //        }
            //        #endregion

            //        //Get the last Y line position to plot new protein sequence
            //        lastYPlottedLine = lastYUniquePeptide;

            //        #region Draw common peptides

            //        for (int countPeptides = 1; countPeptides <= commonPeptides.Count; countPeptides++)
            //        {
            //            bool hasNtermMod = false;

            //            #region Check sequence bounds 
            //            int startCommonPeptideOriginal = Array.FindIndex(matchLocationsBottomUp, item => item != null && item.Contains(countPeptides - 1));
            //            if (startCommonPeptideOriginal > proteinCount + STEP_PROTEIN_SEQUENCE - 1) continue;//Peptide started after limit per line
            //            int startCommonPeptide = startCommonPeptideOriginal - ((stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE);
            //            int endCommonPeptideOriginal = Array.FindLastIndex(matchLocationsBottomUp, item => item != null && item.Contains(countPeptides - 1));
            //            int endCommonPeptide = endCommonPeptideOriginal - ((stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE);
            //            if (endCommonPeptideOriginal > proteinCount + STEP_PROTEIN_SEQUENCE - 1)
            //                endCommonPeptide = STEP_PROTEIN_SEQUENCE - 1;
            //            if (startCommonPeptide <= -1 && endCommonPeptide <= -1) continue;
            //            else if (startCommonPeptide <= -1 && endCommonPeptideOriginal > 0)//Peptide started in a previous line
            //                startCommonPeptide = 0;
            //            #endregion

            //            Tuple<string, bool, List<Tuple<double, string>>, List<string>, List<string>, List<Tuple<string, int, string>>> currentPeptide = commonPeptides[countPeptides - 1];
            //            foreach (Tuple<string, int, string> mod in currentPeptide.Item6)
            //            {
            //                if (mod.Item2 >= startCommonPeptideOriginal && mod.Item2 <= endCommonPeptideOriginal)
            //                {
            //                    SolidColorBrush labelBrush_PTM = new SolidColorBrush(Colors.DarkRed);
            //                    Label ptmLabel = new Label();
            //                    ptmLabel.FontFamily = new FontFamily("Courier New");
            //                    ptmLabel.FontSize = FONTSIZE_PROTEINSEQUENCE_PTM;
            //                    if (currentPeptide.Item3.Equals("}"))
            //                    {
            //                        ptmLabel.ToolTip = "Amino acid: N-Terminal\nPosition: " + mod.Item2 + "\nDescription: " + mod.Item3;
            //                        ptmLabel.Content = "n";
            //                        hasNtermMod = true;
            //                    }
            //                    else
            //                    {
            //                        ptmLabel.ToolTip = "Amino acid: " + mod.Item1 + "\nPosition: " + mod.Item2 + "\nDescription: " + mod.Item3;
            //                        ptmLabel.Content = mod.Item1.ToLower();
            //                    }
            //                    int description_ptm = Array.IndexOf(PTMs_color, mod.Item3);
            //                    if (description_ptm > -1)
            //                        ptmLabel.Foreground = AminoAcidColor[description_ptm];
            //                    else
            //                        ptmLabel.Foreground = labelBrush_PTM;
            //                    ptmLabel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            //                    ptmLabel.MouseEnter += ptm_MouseEnter;
            //                    ptmLabel.MouseLeave += ptm_MouseLeave;

            //                    if (mod.Item2 >= (stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE &&
            //                        mod.Item2 <= stepOneByOne * STEP_PROTEIN_SEQUENCE)//Check if ptm position is in the current protein line
            //                    {
            //                        MyCanvas.Children.Add(ptmLabel);
            //                        if (currentPeptide.Item3.Equals("}"))
            //                            Canvas.SetLeft(ptmLabel, 20);
            //                        else
            //                            Canvas.SetLeft(ptmLabel, initialXLine + 24 * ((mod.Item2 - (stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE) - 1));
            //                        Canvas.SetTop(ptmLabel, (lastYUniquePeptide + 10 * (countPeptides - 1)) - 4);
            //                        Canvas.SetZIndex(ptmLabel, 1);
            //                    }
            //                }
            //            }
            //            //Drawing a line
            //            Line l = new Line();
            //            l.X1 = initialXLine + 24 * startCommonPeptide;
            //            l.X2 = initialXLine + 24 * endCommonPeptide + 20;
            //            l.Y1 = lastYUniquePeptide + 10 * countPeptides;
            //            l.Y2 = lastYUniquePeptide + 10 * countPeptides;
            //            lastYPlottedLine = l.Y1;
            //            l.StrokeThickness = WIDTH_LINE;
            //            l.Stroke = BRUSH_LINE_COLOR_COMMONPEPTIDES;
            //            l.MouseEnter += line_MouseEnter;
            //            l.MouseLeave += line_MouseLeave;

            //            StringBuilder sbId = new StringBuilder();
            //            foreach (string proteoform in currentPeptide.Item4)
            //            {
            //                int indexProteoformList = proteoformWithScore.FindIndex(item => item.Equals(proteoform));
            //                sbId.Append("_");
            //                if (indexProteoformList > -1)
            //                    sbId.Append(indexProteoformList);
            //            }
            //            sbId.Append("_");
            //            l.Name = sbId.ToString();

            //            StringBuilder sbPtms = new StringBuilder();
            //            foreach (Tuple<string, int, string> mod in currentPeptide.Item6)
            //                sbPtms.Append("_" + mod.Item3 + "_");
            //            sbPtms.Append("_bottomUp_");
            //            l.Tag = new object[] { sbPtms.ToString() };

            //            StringBuilder sbToolTip = new StringBuilder();
            //            sbToolTip.AppendLine("Common Peptide:\n" + currentPeptide.Item1);
            //            sbToolTip.AppendLine("Protein(s): " + String.Join(",\n                 ", currentPeptide.Item5.Distinct()));
            //            sbToolTip.AppendLine("Start Position: " + (startCommonPeptideOriginal + 1));
            //            sbToolTip.AppendLine("End Position: " + (endCommonPeptideOriginal + 1));

            //            foreach (Tuple<double, string> score in currentPeptide.Item3)
            //            {
            //                sbToolTip.AppendLine("Search Engine: " + score.Item2);
            //                sbToolTip.AppendLine("Score: " + score.Item1.ToString("0.00000"));
            //                sbToolTip.AppendLine();
            //            }
            //            sbToolTip.Remove(sbToolTip.Length - 4, 4);
            //            l.ToolTip = sbToolTip.ToString();
            //            MyCanvas.Children.Add(l);
            //            Canvas.SetZIndex(l, -1);

            //            if ((startCommonPeptideOriginal - ((stepOneByOne - 1) * STEP_PROTEIN_SEQUENCE)) > -1 && //It means that modifications hapen in the same line
            //                hasNtermMod)
            //            {
            //                l = new Line();
            //                l.X1 = 18;
            //                l.X2 = initialXLine + 24 * startCommonPeptide;
            //                l.Y1 = lastYProteoform + 10 * countPeptides;
            //                l.Y2 = lastYProteoform + 10 * countPeptides;
            //                l.StrokeThickness = WIDTH_LINE - 1;
            //                l.Stroke = BRUSH_LINE_COLOR_COMMONPEPTIDES;
            //                //l.MouseEnter += n_term_line_MouseEnter;
            //                //l.MouseLeave += n_term_line_MouseLeave;
            //                l.Name = sbId.ToString();
            //                l.Tag = new object[] { sbPtms.ToString() };
            //                sbToolTip = new StringBuilder();
            //                sbToolTip.AppendLine("Common Peptide:\n" + currentPeptide.Item1);
            //                sbToolTip.AppendLine("Protein(s): " + String.Join(",\n                 ", currentPeptide.Item5.Distinct()));
            //                sbToolTip.AppendLine("Start Position: " + (startCommonPeptideOriginal + 1));
            //                sbToolTip.AppendLine("End Position: " + (endCommonPeptideOriginal + 1));

            //                foreach (Tuple<double, string> score in currentPeptide.Item3)
            //                {
            //                    sbToolTip.AppendLine("Search Engine: " + score.Item2);
            //                    sbToolTip.AppendLine("Score: " + score.Item1.ToString("0.00000"));
            //                    sbToolTip.AppendLine();
            //                }
            //                sbToolTip.Remove(sbToolTip.Length - 4, 4);
            //                l.ToolTip = sbToolTip.ToString();
            //                l.StrokeDashArray = new DoubleCollection(new double[2] { 3, 1 });
            //                MyCanvas.Children.Add(l);
            //                Canvas.SetZIndex(l, -1);
            //            }
            //        }
            //        #endregion
            //    }
            //}

            #endregion
        }

        private void subItemAssessment_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)((object[])((MenuItem)sender).Tag)[0];
            string assessment = (string)((object[])((MenuItem)sender).Tag)[1];
            int countProtfm = (int)((object[])((MenuItem)sender).Tag)[2];

            if (assessment.Equals("Good"))
            {
                ((MenuItem)mi.Items[0]).IsChecked = true;
                ((MenuItem)mi.Items[1]).IsChecked = false;
                List<(double, string, bool, double)> newValues = new List<(double, string, bool, double)>();
                AnnotationProteoforms[countProtfm].Item2.ForEach(param =>
                {
                    newValues.Add((param.Item1, param.Item2, true, param.Item4));
                });
                AnnotationProteoforms[countProtfm] = (AnnotationProteoforms[countProtfm].Item1, newValues);
            }
            else
            {
                ((MenuItem)mi.Items[0]).IsChecked = false;
                ((MenuItem)mi.Items[1]).IsChecked = true;
                List<(double, string, bool, double)> newValues = new List<(double, string, bool, double)>();
                AnnotationProteoforms[countProtfm].Item2.ForEach(param =>
                {
                    newValues.Add((param.Item1, param.Item2, false, param.Item4));
                });
                AnnotationProteoforms[countProtfm] = (AnnotationProteoforms[countProtfm].Item1, newValues);
            }
        }

        public void HighLightProteoformOrPeptides(string modification = null)
        {
            #region Highlight peptide lines
            if (!String.IsNullOrEmpty(modification))
            {
                //Reset lines
                foreach (object o in MyCanvas.Children)
                {
                    if (o is Line)
                    {
                        Line line = (Line)o;
                        object[] chain = line.Tag != null ? ((object[])line.Tag) : new object[0];
                        if (!chain[0].Equals("_alwaysAppear_"))
                            line.Opacity = 0.25;
                    }
                }

                foreach (object o in MyCanvas.Children)
                {
                    if (o is Line)
                    {
                        Line line = (Line)o;
                        if (line.Tag == null) continue;
                        string tagLine = (string)((object[])line.Tag)[0];
                        string[] cols = Regex.Split(tagLine, "_");
                        foreach (string col in cols)
                        {
                            if (String.IsNullOrEmpty(col)) continue;

                            if (col.Contains(modification) || modification.Contains(col))
                            {
                                line.Opacity = 1;
                            }
                        }
                    }
                }
            }
            else
            {
                //Reset lines
                foreach (object o in MyCanvas.Children)
                {
                    if (o is Line)
                    {
                        Line line = (Line)o;
                        line.Opacity = 1;
                    }
                }
            }
            #endregion

        }

        private void line_MouseEnter(object sender, RoutedEventArgs e)
        {
            Line l1 = (Line)sender;
            if (IsSingleLine)
                l1.StrokeThickness = WIDTH_LINE + 2;
            else
            {
                foreach (object o in MyCanvas.Children)
                {
                    if (o is Line)
                    {
                        Line line = (Line)o;
                        if (line.Name.ToString().Equals(l1.Name.ToString()) &&
                            line.ToolTip.ToString().Equals(l1.ToolTip.ToString()) &&
                            (line.StrokeDashArray.Count == 0 || line.StrokeDashArray.Count == 2))
                            line.StrokeThickness = WIDTH_LINE + 2;
                    }
                }
            }
        }

        private void line_MouseLeave(object sender, RoutedEventArgs e)
        {
            Line l1 = (Line)sender;
            if (IsSingleLine)
                l1.StrokeThickness = WIDTH_LINE;
            else
            {
                foreach (object o in MyCanvas.Children)
                {
                    if (o is Line)
                    {
                        Line line = (Line)o;
                        if (line.Name.ToString().Equals(l1.Name.ToString()) &&
                            line.ToolTip.ToString().Equals(l1.ToolTip.ToString()) &&
                            (line.StrokeDashArray.Count == 0 || line.StrokeDashArray.Count == 2))
                            line.StrokeThickness = WIDTH_LINE;
                    }
                }
            }

        }

        private void n_term_line_MouseEnter(object sender, RoutedEventArgs e)
        {
            Line l1 = (Line)sender;
            l1.StrokeThickness = WIDTH_LINE + 1;
        }

        private void n_term_line_MouseLeave(object sender, RoutedEventArgs e)
        {
            Line l1 = (Line)sender;
            l1.StrokeThickness = WIDTH_LINE - 1;
        }

        private void ptm_MouseEnter(object sender, RoutedEventArgs e)
        {
            Label l1 = (Label)sender;
            l1.FontWeight = FontWeights.Bold;
            l1.FontSize = FONTSIZE_PROTEINSEQUENCE_PTM + 2;
        }

        private void ptm_MouseLeave(object sender, RoutedEventArgs e)
        {
            Label l1 = (Label)sender;
            l1.FontWeight = FontWeights.Normal;
            l1.FontSize = FONTSIZE_PROTEINSEQUENCE_PTM;
        }

        private void line_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (e.ClickCount == 2)
            //{
            //    Tuple<string, List<Tuple<double, string, bool, double, List<Tuple<string, int, string>>>>, int> proteoform = (Tuple<string, List<Tuple<double, string, bool, double, List<Tuple<string, int, string>>>>, int>)((object[])((Line)sender).Tag)[1];
            //}
        }

        private void miSelectPeptides_Click(object sender, RoutedEventArgs e)
        {
            Line mainLine = (Line)((object[])((MenuItem)sender).Tag)[0];
            int indexProteoform = Convert.ToInt32(Regex.Split(mainLine.Name, "_").Where(item => !String.IsNullOrEmpty(item)).ToList()[0]);
            //Reset lines
            foreach (object o in MyCanvas.Children)
            {
                if (o is Line)
                {
                    Line line = (Line)o;
                    object[] chain = line.Tag != null ? ((object[])line.Tag) : new object[0];
                    if (!chain[0].Equals("_alwaysAppear_"))
                        line.Opacity = 0.25;
                }
            }

            foreach (object o in MyCanvas.Children)
            {
                if (o is Line)
                {
                    Line line = (Line)o;
                    string[] cols = Regex.Split(line.Name, "_");
                    foreach (string col in cols)
                    {
                        if (String.IsNullOrEmpty(col)) continue;

                        if (indexProteoform == Convert.ToInt32(col))
                        {
                            line.Opacity = 1;
                        }
                    }
                }
            }
        }

        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Reset lines
            foreach (object o in MyCanvas.Children)
            {
                if (o is Line)
                {
                    Line line = (Line)o;
                    line.Opacity = 1;
                }
            }
        }
    }
}
