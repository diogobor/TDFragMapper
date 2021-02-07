using Ionic.Zip;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TDFragMapper.Utils;

namespace TDFragMapper.Controller
{
    [ProtoContract]
    public class Core
    {
        /// <summary>
        /// List of Fragment Ions: FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity
        /// </summary>
        [ProtoMember(1)]
        public List<(string, int, string, int, string, int, double, double)> FragmentIons { get; set; }
        [ProtoMember(2)]
        public string ProteinSequence { get; set; }
        /// <summary>
        /// Contains information about PTM
        /// </summary>
        [ProtoMember(3)]
        public string SequenceInformation { get; set; }
        /// <summary>
        /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions), isGoldenComplementaryPairs, isBondCleavageConfidence, List<eachStudyItem,color>>
        /// </summary>
        [ProtoMember(4)]
        public Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double, double)>, bool, bool, List<(string, string)>)> DictMaps { get; set; }
        [ProtoMember(5)]
        public bool Has_And_LocalNormalization { get; set; }
        [ProtoMember(6)]
        public bool GlobalNormalization { get; set; }
        [ProtoMember(7)]
        public bool HasMergeMaps { get; set; }
        [ProtoMember(8)]
        public bool HasIntensities { get; set; }
        [ProtoMember(9)]
        public ProgramParams programParams { get; set; }

        [ProtoMember(10)]
        public List<string> DiscriminativeMaps { get; set; }
        [ProtoMember(11)]
        public bool IsRelativeIntensity { get; set; } = true;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Core() { }

        /// <summary>
        /// Method responsible for updating inverse amino acids positions (x, y and z series)
        /// </summary>
        public void ProcessFragIons()
        {
            int proteinLength = ProteinSequence.Length;
            List<(string, int, string, int, string, int, double, double)> fragmentIons = FragmentIons.Where(a => a.Item3.Equals("X") || a.Item3.Equals("Y") || a.Item3.Equals("Z")).ToList();

            for (int i = 0; i < fragmentIons.Count; i++)
            {
                fragmentIons[i] = (fragmentIons[i].Item1, fragmentIons[i].Item2, fragmentIons[i].Item3, proteinLength - fragmentIons[i].Item4 + 1, fragmentIons[i].Item5, fragmentIons[i].Item6, fragmentIons[i].Item7, fragmentIons[i].Item8);
            }

            FragmentIons.RemoveAll(a => a.Item3.Equals("X") || a.Item3.Equals("Y") || a.Item3.Equals("Z"));
            FragmentIons.AddRange(fragmentIons);
        }

        public List<(string, int, string, int, string, int, double, double)> MatchFragmentIonsAndIntensities(List<(string, int, string, int, string, int, double, string, double)> FragIonsWithFileName,
            List<(string, double, double)> Intensities)
        {
            /// string,int,string,int -> FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity, Theoretical Mass
            List<(string, int, string, int, string, int, double, double)> FragIonsWithIntensities = new List<(string, int, string, int, string, int, double, double)>();
            /// string,int,string,int -> Item1: FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; Item2: PrecursorChargeState, Item3: IonType: A,B,C,X,Y,Z; Item4: Aminoacid Position; Item5: Activation Level; Item6: Replicate; Item7: Observed Mass; Item8: IntensityFile, Item9: Theoretical Mass
            foreach ((string, int, string, int, string, int, double, string, double) fragment in FragIonsWithFileName)
            {
                List<(string, double, double)> currentIntesities = Intensities.Where(item => item.Item1.Equals(fragment.Item8)).ToList();
                double MH = fragment.Item7 + Utils.Util.HYDROGENMASS;

                /// string,double,double -> Intensity file, monoisotopic mass, sum intensity
                (string, double, double) intensity = currentIntesities.Where(item => Math.Abs(Utils.Util.PPM(item.Item2, MH)) < 5).FirstOrDefault();
                if (intensity.Item1 != null)
                    FragIonsWithIntensities.Add((fragment.Item1, fragment.Item2, fragment.Item3, fragment.Item4, fragment.Item5, fragment.Item6, intensity.Item3, fragment.Item9));
                else
                {
                    intensity = currentIntesities.Where(item => Math.Abs(Utils.Util.PPM(item.Item2, fragment.Item7)) < 5).FirstOrDefault();
                    if (intensity.Item1 != null)
                        FragIonsWithIntensities.Add((fragment.Item1, fragment.Item2, fragment.Item3, fragment.Item4, fragment.Item5, fragment.Item6, intensity.Item3, fragment.Item9));
                    else
                        FragIonsWithIntensities.Add((fragment.Item1, fragment.Item2, fragment.Item3, fragment.Item4, fragment.Item5, fragment.Item6, 0, fragment.Item9));
                }
            }

            return FragIonsWithIntensities;
        }

        public void SerializeResults(string fileName)
        {
            MemoryStream fileToCompress = new MemoryStream();
            Serializer.SerializeWithLengthPrefix(fileToCompress, this, PrefixStyle.Base128, 1);

            fileToCompress.Seek(0, SeekOrigin.Begin);   // <-- must do this after writing the stream!

            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.Password = "TDFr@gM@pp3r!";
                zipFile.AddEntry("FileCompressed", fileToCompress);
                zipFile.Save(fileName);
            }
        }

        public Core DeserializeResults(string fileName)
        {
            using (var ms = new MemoryStream())
            {
                using (ZipFile zip = ZipFile.Read(fileName))
                {
                    ZipEntry entry = zip["FileCompressed"];
                    entry.ExtractWithPassword(ms, "TDFr@gM@pp3r!");// extract uncompressed content into a memorystream 

                    ms.Seek(0, SeekOrigin.Begin); // <-- must do this after writing the stream!

                    List<Core> toDeserialize = Serializer.DeserializeItems<Core>(ms, PrefixStyle.Base128, 1).ToList();
                    return toDeserialize[0];
                }
            }
        }


        public byte ExportResultsToPDF(string fileName)
        {
            byte returnOK = 0;//0 -> ok, 1 -> failed, 2 -> cancel

            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "TDFragmapper Summary report";
            document.Info.Author = "TDFragmapper";

            // Create an empty page
            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.A4;

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XSize size = gfx.PageSize;
            size.Height -= 10;
            size.Width -= 40;
            XRect rect = new XRect(new XPoint(), size);
            rect.Inflate(-5, -15);

            double offsetY = 100;
            // Create a format
            XStringFormat format = new XStringFormat();

            // Create a font
            XFont font = new XFont("Arial", 10, XFontStyle.Bold);

            format.Alignment = XStringAlignment.Near;//Left

            #region header
            // Draw the text
            rect.Offset(25, offsetY);
            gfx.DrawString("TDFragMapper - Results", font, XBrushes.Black, rect, format);

            // If you're going to read from the stream, you may need to reset the position to the start

            Bitmap _icon = TDFragMapper.Properties.Resources.iconTDFragMapperShadow;
            MemoryStream msIcon = new MemoryStream();
            msIcon.Position = 0;
            _icon.Save(msIcon, System.Drawing.Imaging.ImageFormat.Png);
            XImage image = XImage.FromStream(msIcon);

            // Left position in point
            double center_x = (size.Width - 60) / 2 + 25;
            gfx.DrawImage(image, center_x, 50, 60, 60);

            string strDate = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
            // Draw the text
            format.Alignment = XStringAlignment.Far;//Right
            gfx.DrawString(strDate, font, XBrushes.Black, rect, format);

            font = new XFont("Arial", 14, XFontStyle.Bold);
            format.Alignment = XStringAlignment.Near;//Left
            #endregion

            #region title

            offsetY = 50;
            rect.Offset(0, offsetY);
            gfx.DrawString("Summary report", font, XBrushes.Black, rect, format);

            offsetY = 25;
            rect.Offset(0, offsetY);
            XTextFormatter tf = new XTextFormatter(gfx);
            tf.Alignment = XParagraphAlignment.Justify;
            font = new XFont("Arial", 10, XFontStyle.Regular);
            string text = "This file contains summary information for all files processed in TDFragMapper, including the used parameters and the contents of input files.";
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            #endregion

            #region content

            //page structure options
            int el_height = 30;
            int rect_height = 17;
            double dist_Y = 20;
            int row_number = 1;

            XPen borderPen = new XPen(XColors.Black);
            borderPen.Width = 0.5;

            #region Header
            font = new XFont("Arial", 10, XFontStyle.Bold);

            (dist_Y, row_number) = this.createRowTable("Name", "Description", row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height);

            #endregion

            #region Rows

            font = new XFont("Arial", 10, XFontStyle.Regular);

            #region Protein sequence file

            (dist_Y, row_number) = this.createRowTable("Protein sequence file:", programParams.ProteinSequenceFile, row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height);

            #endregion

            #region Protein sequence

            StringBuilder sb_proteinSeq = new StringBuilder();
            int str_limit = 39;
            int i = 0;

            int row_seq = 1;
            for (i = 0; i < ProteinSequence.Length; i += str_limit, row_seq++)
            {
                if (i + str_limit < ProteinSequence.Length)
                    sb_proteinSeq.Append(ProteinSequence.Substring(i, str_limit) + " ");
                else
                    sb_proteinSeq.Append(ProteinSequence.Substring(i, ProteinSequence.Length - i) + " ");
            }

            row_seq--;

            (dist_Y, row_number) = createRowTable("Protein sequence:", sb_proteinSeq.ToString(), row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height, row_seq);

            int lineHeight = 17;
            double error_offsetY;
            if (row_seq < 9)
                error_offsetY = (0.73 * lineHeight * (row_seq - 1));
            else
                error_offsetY = (0.79 * lineHeight * (row_seq - 1));
            dist_Y += error_offsetY;

            #endregion

            #region Sequence information

            String info = "";

            if (!String.IsNullOrEmpty(programParams.SequenceInformation))
            {
                StringBuilder sb_SeqInfo = new StringBuilder();
                str_limit = 39;
                i = 0;

                row_seq = 1;
                for (i = 0; i < ProteinSequence.Length; i += str_limit, row_seq++)
                {
                    if (i + str_limit < ProteinSequence.Length)
                        sb_SeqInfo.Append(ProteinSequence.Substring(i, str_limit) + " ");
                    else
                        sb_SeqInfo.Append(ProteinSequence.Substring(i, ProteinSequence.Length - i) + " ");
                }

                row_seq--;

                info = sb_SeqInfo.ToString();
                (dist_Y, row_number) = createRowTable("Sequence information:", info, row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height, row_seq);
                lineHeight = 20;
                row_number += (row_seq - 2);
                dist_Y = lineHeight * (row_number + 0.3);
            }
            else
            {
                info = "none";
                (dist_Y, row_number) = createRowTable("Sequence information:", info, row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height);
            }

            #endregion

            #region Intensity normalization

            info = "";

            if (this.Has_And_LocalNormalization)
            {
                if (this.GlobalNormalization)
                    info = "Across all study maps";
                else
                    info = "Per study map";
            }
            else
                info = "none";

            (dist_Y, row_number) = createRowTable("Intensity normalization:", info, row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height);

            #endregion

            #region Total number of MS/MS data files

            (dist_Y, row_number) = createRowTable("Total number of MS/MS data files:", programParams.InputFileList.Count.ToString(), row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height);

            #endregion

            #region Total number of deconvoluted spectra files

            if (programParams.HasIntensities)
            {
                int numberOfDeconvSpecFiles = programParams.InputFileList.Where(a => !String.IsNullOrEmpty(a.Item6)).ToList().Count;
                (dist_Y, row_number) = createRowTable("Total number of deconvoluted spectra files:", numberOfDeconvSpecFiles.ToString(), row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height);
            }

            #endregion

            #region Fragmentation method(s)

            List<string> fragments = FragmentIons.Select(a => a.Item1).Distinct().ToList();
            (dist_Y, row_number) = createRowTable("Fragmentation method(s):", String.Join(", ", fragments), row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height);

            #endregion

            #region Activation level(s)

            List<(string, string)> actLevels = (from item in FragmentIons
                                                select (item.Item1, item.Item5)).Distinct().ToList();
            info = Regex.Replace(String.Join("\n", actLevels), "[(|)|,]", "");
            row_seq = info.Count(f => (f == '\n'));
            
            (dist_Y, row_number) = createRowTable("Activation level(s):", info, row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height, row_seq);
            lineHeight = 17;
            if(row_seq < 9)
                error_offsetY = (0.73 * lineHeight * (row_seq - 1));
            else
                error_offsetY = (0.79 * lineHeight * (row_seq - 1));
            dist_Y += error_offsetY;

            #endregion

            #region Precursor charge state(s)

            fragments = FragmentIons.OrderBy(a => a.Item2).Select(a => a.Item2.ToString()).Distinct().ToList();
            (dist_Y, row_number) = createRowTable("Precursor charge state(s):", String.Join(", ", fragments), row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height);

            #endregion

            #region Replicate(s)

            fragments = FragmentIons.OrderBy(a => a.Item6).Select(a => a.Item6.ToString()).Distinct().ToList();
            (dist_Y, row_number) = createRowTable("Replicate(s):", String.Join(", ", fragments), row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height);

            #endregion

            #region Total number of maps

            (dist_Y, row_number) = createRowTable("Total number of maps:", DictMaps.Count.ToString(), row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height);

            #endregion

            #region Discriminative map(s)

            info = String.Join("", DiscriminativeMaps);
            row_seq = info.Count(f => (f == '\n'));
            (dist_Y, row_number) = createRowTable("Discriminative map(s):", info, row_number, gfx, tf, font, format, borderPen, size, dist_Y, rect_height, el_height, row_seq);

            #endregion

            #endregion

            #endregion

            #region footer 
            font = new XFont("Arial", 11, XFontStyle.Bold);
            format.Alignment = XStringAlignment.Far;
            format.LineAlignment = XLineAlignment.Far;
            rect.Offset(0, -175);
            gfx.DrawString("Page: " + document.PageCount.ToString(), font, XBrushes.Black, rect, format);
            #endregion

            document.Save(fileName);

            return returnOK;
        }

        private (double, int) createRowTable(string name, string description, int row_number, XGraphics gfx, XTextFormatter tf, XFont font, XStringFormat format, XPen borderPen, XSize size, double dist_Y2, int rect_height, int el_height, double row_seq = 1.5)
        {
            double lineHeight = 20;
            int marginLeft = 30;
            int marginTop = 210;

            int el1_width = Convert.ToInt32(size.Width / 2) - 5;
            int el2_width = Convert.ToInt32(size.Width / 2) - 5;

            int offSetX_1 = el1_width;

            int interLine_X_1 = 5;
            int interLine_Y = 3;
            double error_offsetY = 0;
            if (row_seq == 1.5)
                row_seq -= 0.5;
            else if (row_seq < 9)
                error_offsetY = 0.7 * row_seq;
            else 
                error_offsetY = 0.3 * row_seq;

            //Name

            gfx.DrawRectangle(borderPen, marginLeft, marginTop + dist_Y2, el1_width, (rect_height - error_offsetY) * row_seq);
            tf.DrawString(

                name,
                font,
                XBrushes.Black,
                new XRect(marginLeft + interLine_X_1, marginTop + dist_Y2 + interLine_Y, el1_width, el_height * row_seq),
                format);

            //Description

            gfx.DrawRectangle(borderPen, marginLeft + offSetX_1, marginTop + dist_Y2, el2_width, (rect_height - error_offsetY) * row_seq);
            tf.DrawString(
                description,
                font,
                XBrushes.Black,
                new XRect(marginLeft + offSetX_1 + interLine_X_1, marginTop + dist_Y2 + interLine_Y, el2_width, el_height * row_seq),
                format);

            row_number++;
            return (dist_Y2 + lineHeight, row_number);

        }
    }
}
