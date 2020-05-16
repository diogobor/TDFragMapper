using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MergeFragIons.Controller;
using MergeFragIons.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MergeFragIons
{
    class Program
    {

        public Core mainCore { get; set; }
        public bool FinishProcessing { get; set; }
        public ProgramParams programParams { get; set; }
        public string version = "";
        public string FinalTime { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            #region Setting Language
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            if (!Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\International", "LocaleName", null).ToString().ToLower().Equals("en-us"))
            {
                DialogResult answer = MessageBox.Show("The default language is not English. Do you want to change it to English ?\nThis tool works if only the default language is English.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (answer == DialogResult.Yes)
                {
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "Locale", "00000409");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "LocaleName", "en-US");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sCountry", "Estados Unidos");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sCurrency", "$");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sDate", "/");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sDecimal", ".");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sGrouping", "3;0");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sLanguage", "ENU");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sList", ",");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sLongDate", "dddd, MMMM dd, yyyy");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sMonDecimalSep", ".");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sMonGrouping", "3;0");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sMonThousandSep", ",");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sNativeDigits", "0123456789");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sNegativeSign", "-");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sPositiveSign", "");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sShortDate", "M/d/yyyy");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sThousand", ",");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sTime", ":");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sTimeFormat", "h:mm:ss tt");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sShortTime", "h:mm tt");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sYearMonth", "MMMM, yyyy");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iCalendarType", "1");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iCountry", "1");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iCurrDigits", "2");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iCurrency", "0");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iDate", "0");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iDigits", "2");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "NumShape", "1");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iFirstDayOfWeek", "6");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iFirstWeekOfYear", "0");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iLZero", "1");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iMeasure", "1");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iNegCurr", "0");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iNegNumber", "1");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iPaperSize", "1");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iTime", "0");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iTimePrefix", "0");
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iTLZero", "0");
                    MessageBox.Show("Software will be restarted!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Environment.Exit(0);
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    MessageBox.Show("Software will be closed!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Environment.Exit(0);
                    System.Windows.Forms.Application.Exit();
                }
            }
            #endregion

            #region debug

            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GUI(args));
        }

        public void ReadInputFiles()
        {
            mainCore = new Core();
            FinishProcessing = false;
            DateTime beginTimeSearch = DateTime.Now;
            string version = "";
            try
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch (Exception e1)
            {
                //Unable to retrieve version number
                Console.WriteLine("", e1);
                version = "";
            }

            Console.WriteLine("#################################################################################################################################################################");
            Console.WriteLine("                                                                                                                                                                                    Merge Fragment Ions - v. " + version + "\n");
            Console.WriteLine("                                                                                                                                                  Engineered by Diogo Borges Lima (CeMM) and MSBio - Institut Pasteur             \n");
            Console.WriteLine("#################################################################################################################################################################");

            ReadProteinSequence();
            ReadFragmentIons();

            mainCore.ProcessFragIons();

            FinishProcessing = true;
        }

        private void ReadProteinSequence()
        {
            Console.WriteLine(" Reading protein sequence file...");
            try
            {
                StreamReader sr = new StreamReader(programParams.ProteinSequenceFile);

                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length > 0)
                    {
                        mainCore.ProteinSequence = line;
                        break;
                    }
                }
                sr.Close();
            }
            catch (Exception) { }

            mainCore.SequenceInformation = programParams.SequenceInformation;
        }

        private void ReadFragmentIons()
        {
            Console.WriteLine(" Reading fragment ions file(s)...");
            /// <summary>
            /// List of Fragment Ions
            /// </summary>
            /// string,int,string,int -> FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate
            List<(string, int, string, int, string, int)> fragIons = new List<(string, int, string, int, string, int)>();

            /// <summary>
            /// List<(MS/MS Data, Fragmentation Method, Activation Level, Precursor Charge State, Replicate)>
            /// </summary>
            foreach ((string, string, string, int, int) dataFile in programParams.InputFileList)
            {
                Console.WriteLine(" Reading {0} ...", dataFile.Item1);
                string ionType = "";
                int aminoacidPos = 0;

                string fragMethod = dataFile.Item2;
                string activationLevel = dataFile.Item3;
                int precursorChargeState = dataFile.Item4;
                int replicate = dataFile.Item5;

                try
                {
                    using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(dataFile.Item1, false))
                    {
                        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                        SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                        string currentcellvalue = string.Empty;

                        foreach (Row r in sheetData.Elements<Row>())
                        {
                            int countCell = 0;
                            foreach (Cell currentcell in r.Elements<Cell>())
                            {
                                if (currentcell.CellValue == null) break;
                                if (currentcell.DataType != null)
                                {
                                    if (currentcell.DataType == CellValues.SharedString)
                                    {
                                        int id = -1;

                                        if (Int32.TryParse(currentcell.InnerText, out id))
                                        {
                                            SharedStringItem item = GetSharedStringItemById(workbookPart, id);

                                            if (item.Text != null)
                                            {
                                                //code to take the string value  
                                                currentcellvalue = item.Text.Text;
                                            }
                                            else if (item.InnerText != null)
                                            {
                                                currentcellvalue = item.InnerText;
                                            }
                                            else if (item.InnerXml != null)
                                            {
                                                currentcellvalue = item.InnerXml;
                                            }
                                        }
                                    }
                                    else
                                        currentcellvalue = currentcell.CellValue.Text;
                                }
                                else
                                    currentcellvalue = currentcell.CellValue.Text;
                                if (currentcellvalue.StartsWith("Name")) break;

                                if (countCell == 0)
                                {
                                    countCell++;
                                    continue;
                                }
                                else if (countCell == 1)
                                    ionType = currentcellvalue;
                                else if (countCell == 2)
                                {
                                    aminoacidPos = Convert.ToInt32(currentcellvalue);
                                    break;
                                }

                                countCell++;
                            }
                            if (!String.IsNullOrEmpty(ionType))
                                fragIons.Add((fragMethod, precursorChargeState, ionType, aminoacidPos, activationLevel, replicate));
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(
                                        "Error to read some files:\n" + e.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                }
            }

            mainCore.FragmentIons = fragIons.Distinct().ToList();

            Console.WriteLine(" Done!");
        }

        private SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }
    }
}
