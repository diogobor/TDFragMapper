using Ionic.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ProtoBuf;
using System;
using System.Collections.Generic;
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
        public Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double, double)>, bool, bool, List<(string,string)>)> DictMaps { get; set; }
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
            byte returnOK = 2;//0 -> ok, 1 -> failed, 2 -> cancel

            Document doc = new Document(PageSize.A4, 30f, 20f, 150f, 40);

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));
                writer.PageEvent = new HeaderFooter();
                doc.Open();

                iTextSharp.text.Font customFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL);
                customFont = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Paragraph summary = new iTextSharp.text.Paragraph("Summary report", customFont);
                doc.Add(summary);

                customFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Paragraph summary_text = new iTextSharp.text.Paragraph("This file contains summary information for all files processed in TDFragMapper, including the used parameters and the contents of input files.", customFont);
                summary_text.SpacingBefore = 5;
                summary_text.SpacingAfter = 15;
                summary_text.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(summary_text);

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100f;
                PdfPCell cell_title = new PdfPCell(new Phrase("Name",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD))
                );

                table.AddCell(cell_title);
                cell_title = new PdfPCell(new Phrase("Description",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD))
                );
                table.AddCell(cell_title);

                PdfPCell cell_content = new PdfPCell(new Phrase("Protein sequence file:",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
                );

                table.AddCell(cell_content);
                cell_content = new PdfPCell(new Phrase(programParams.ProteinSequenceFile,
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
                );
                table.AddCell(cell_content);

                cell_content = new PdfPCell(new Phrase("Protein sequence:",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
                );
                table.AddCell(cell_content);
                cell_content = new PdfPCell(new Phrase(this.ProteinSequence,
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
                );
                table.AddCell(cell_content);

                cell_content = new PdfPCell(new Phrase("Sequence information:",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
                );
                table.AddCell(cell_content);
                if (!String.IsNullOrEmpty(programParams.SequenceInformation))
                {
                    cell_content = new PdfPCell(new Phrase(programParams.SequenceInformation,
                        FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
                    );
                }
                else
                {
                    cell_content = new PdfPCell(new Phrase("none",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
                );
                }
                table.AddCell(cell_content);

                cell_content = new PdfPCell(new Phrase("Intensity normalization:",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
                );
                table.AddCell(cell_content);

                if (this.Has_And_LocalNormalization)
                {
                    if (this.GlobalNormalization)
                    {
                        cell_content = new PdfPCell(new Phrase("Across all study maps",
                            FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL)));
                    }
                    else
                    {
                        cell_content = new PdfPCell(new Phrase("Per study map",
                            FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL)));
                    }
                }
                else
                    cell_content = new PdfPCell(new Phrase("none",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL)));
                table.AddCell(cell_content);

                cell_content = new PdfPCell(new Phrase("Total number of MS/MS data files:",
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                int numberOfDataFiles = programParams.InputFileList.Count;
                cell_content = new PdfPCell(new Phrase(String.Join(", ", numberOfDataFiles),
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                if (programParams.HasIntensities)
                {
                    cell_content = new PdfPCell(new Phrase("Total number of deconvoluted spectra files:",
                            FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL)));
                    table.AddCell(cell_content);

                    int numberOfDeconvSpecFiles = programParams.InputFileList.Where(a => !String.IsNullOrEmpty(a.Item6)).ToList().Count;
                    cell_content = new PdfPCell(new Phrase(String.Join(", ", numberOfDeconvSpecFiles),
                       FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
                   );
                    table.AddCell(cell_content);
                }

                cell_content = new PdfPCell(new Phrase("Fragmentation method(s):",
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                List<string> fragments = FragmentIons.Select(a => a.Item1).Distinct().ToList();
                cell_content = new PdfPCell(new Phrase(String.Join(", ", fragments),
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                cell_content = new PdfPCell(new Phrase("Activation level(s):",
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                List<(string, string)> actLevels = (from item in FragmentIons
                                                    select (item.Item1, item.Item5)).Distinct().ToList();
                cell_content = new PdfPCell(new Phrase(Regex.Replace(String.Join("\n", actLevels), "[(|)|,]", ""),
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                cell_content = new PdfPCell(new Phrase("Precursor charge state(s):",
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                fragments = FragmentIons.OrderBy(a => a.Item2).Select(a => a.Item2.ToString()).Distinct().ToList();
                cell_content = new PdfPCell(new Phrase(String.Join(", ", fragments),
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                cell_content = new PdfPCell(new Phrase("Replicate(s):",
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                fragments = FragmentIons.OrderBy(a => a.Item6).Select(a => a.Item6.ToString()).Distinct().ToList();
                cell_content = new PdfPCell(new Phrase(String.Join(", ", fragments),
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                cell_content = new PdfPCell(new Phrase("Total number of maps:",
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                cell_content = new PdfPCell(new Phrase(DictMaps.Count.ToString(),
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                cell_content = new PdfPCell(new Phrase("Discriminative map(s):",
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                cell_content = new PdfPCell(new Phrase(String.Join("\n", DiscriminativeMaps),
                   FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL))
               );
                table.AddCell(cell_content);

                doc.Add(table);

                returnOK = 0;//0 -> ok, 1 -> failed, 2 -> cancel
            }
            catch (Exception)
            {
                returnOK = 1;//0 -> ok, 1 -> failed, 2 -> cancel
            }
            finally
            {
                doc.Close();
            }

            return returnOK;
        }
    }

    class HeaderFooter : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            //base.OnEndPage(writer, document);

            PdfPTable tbHeader = new PdfPTable(3);
            tbHeader.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            tbHeader.DefaultCell.Border = 0;

            tbHeader.AddCell(new Paragraph());
            #region TDFragMapper logo
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(TDFragMapper.Properties.Resources.iconTDFragMapperShadow, System.Drawing.Imaging.ImageFormat.Bmp);
            img.ScaleAbsolute(60f, 60f);
            img.Alignment = Element.ALIGN_CENTER;
            PdfPCell imgCell = new PdfPCell(img);
            imgCell.HorizontalAlignment = Element.ALIGN_CENTER;
            imgCell.Border = 0;
            tbHeader.AddCell(imgCell);
            #endregion
            tbHeader.AddCell(new Paragraph());

            PdfPCell cell_title = new PdfPCell(new Phrase("TDFragMapper - Results",
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD))
                );
            cell_title.Border = 0;
            cell_title.HorizontalAlignment = Element.ALIGN_LEFT;
            tbHeader.AddCell(cell_title);

            tbHeader.AddCell(new Paragraph());

            string strDate = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
            cell_title = new PdfPCell(new Phrase(strDate,
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD))
                );
            cell_title.Border = 0;
            cell_title.HorizontalAlignment = Element.ALIGN_RIGHT;
            tbHeader.AddCell(cell_title);
            tbHeader.WriteSelectedRows(0, -1, doc.LeftMargin, writer.PageSize.GetTop(doc.TopMargin) + 100, writer.DirectContent);

            PdfPTable tbFooter = new PdfPTable(3);
            tbFooter.TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin;
            tbFooter.DefaultCell.Border = 0;
            tbFooter.AddCell(new Paragraph());
            tbFooter.AddCell(new Paragraph());
            cell_title = new PdfPCell(new Phrase("Page: " + writer.PageNumber,
                    FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD))
                );
            cell_title.Border = 0;
            cell_title.HorizontalAlignment = Element.ALIGN_RIGHT;
            tbFooter.AddCell(cell_title);
            tbFooter.WriteSelectedRows(0, -1, doc.LeftMargin, writer.PageSize.GetBottom(doc.BottomMargin) - 5, writer.DirectContent);
        }
    }
}
