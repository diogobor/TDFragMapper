using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using TDFragMapper.Controller;
using TDFragMapper.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;

namespace TDFragMapper
{
    class Program
    {
        /// <summary>
        /// List of Fragment Ions
        /// </summary>
        /// string,int,string,int -> FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Observed Mass, IntensityFile
        private List<(string, int, string, int, string, int, double, string)> FragIons { get; set; }

        /// string,int,string,int -> FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity
        private List<(string, int, string, int, string, int, double)> FragIonsWithIntensities { get; set; }
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
            //DataTableCollection dataTableCollection;
            //string path = @"Z:\data\Data_files 2\Xtract_files\CID\Xtract_LC_CID20_Precursor17_Replicate01.xls";
            //using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            //{
            //    using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
            //    {
            //        DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
            //        {
            //            ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
            //        });
            //        dataTableCollection = result.Tables;
            //        bool isSubRow = false;
            //        foreach (DataRow dataRow in dataTableCollection[0].Rows)
            //        {
            //            var values = dataRow.ItemArray;
            //            if (String.IsNullOrEmpty(values[1].ToString()))
            //            {
            //                isSubRow = false;
            //                continue;
            //            }
            //            if (values[1].ToString().StartsWith("Charge"))
            //            {
            //                isSubRow = true;
            //                continue;
            //            }
            //            if (isSubRow) continue;
            //            double monoIsotopicMass = Convert.ToDouble(values[1]);
            //            double intensity = Convert.ToDouble(values[2]);
            //            //ionType = values[1].ToString();
            //            //aminoacidPos = Convert.ToInt32(values[2]);
            //            //FragIons.Add((fragMethod, precursorChargeState, ionType, aminoacidPos, activationLevel, replicate, dataFile.Item6));
            //        }
            //    }
            //}
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

            Console.WriteLine("##################################################################################################################");
            Console.WriteLine("                                                                                                                              TDFragMapper - v. " + version + "\n");
            Console.WriteLine("                                                                                            Engineered by Diogo Borges Lima and MSBio - Institut Pasteur             \n");
            Console.WriteLine("##################################################################################################################");

            ReadProteinSequence();
            ReadFragmentIons();
            if (programParams.HasIntensities)
                ReadIntensities();
            else
                SetNullIntensitiesFragIons();

            //Inverse positions c-term series
            mainCore.ProcessFragIons();
            mainCore.HasIntensities = programParams.HasIntensities;

            Console.WriteLine(" Files have been loaded successfully!");

            FinishProcessing = true;
        }

        private void ReadProteinSequence()
        {
            Console.WriteLine(" Reading protein sequence file...");
            try
            {
                StreamReader sr = new StreamReader(programParams.ProteinSequenceFile);

                StringBuilder sbSeq = new StringBuilder();
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length > 0 && !line.StartsWith(">"))
                    {
                        sbSeq.Append(line);
                    }
                }
                sr.Close();
                mainCore.ProteinSequence = sbSeq.ToString();
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
            /// string,int,string,int -> FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Observed Mass, IntensityFile
            FragIons = new List<(string, int, string, int, string, int, double, string)>();

            /// <summary>
            /// List<(MS/MS Data, Fragmentation Method, Activation Level, Precursor Charge State, Replicate, Intensity Data)>
            /// </summary>
            foreach ((string, string, string, int, int, string) dataFile in programParams.InputFileList)
            {
                Console.WriteLine(" Reading {0} ...", dataFile.Item1);
                string ionType = "";
                int aminoacidPos = 0;
                double observedMass = 0;

                string fragMethod = dataFile.Item2;
                string activationLevel = dataFile.Item3;
                int precursorChargeState = dataFile.Item4;
                int replicate = dataFile.Item5;

                try
                {
                    DataTableCollection dataTableCollection;
                    using (var stream = File.Open(dataFile.Item1, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });
                            dataTableCollection = result.Tables;
                            foreach (DataRow dataRow in dataTableCollection[0].Rows)
                            {
                                var values = dataRow.ItemArray;
                                ionType = values[1].ToString();
                                aminoacidPos = Convert.ToInt32(values[2]);
                                observedMass = Convert.ToDouble(values[4]);
                                FragIons.Add((fragMethod, precursorChargeState, ionType, aminoacidPos, activationLevel, replicate, observedMass, dataFile.Item6));
                            }
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

            //mainCore.FragmentIons = FragIons.Distinct().ToList();

            FragIons = FragIons.Distinct().ToList();

        }

        private void ReadIntensities()
        {
            Console.WriteLine(" Reading intensity file(s)...");

            /// string,int,string,int -> FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity
            FragIonsWithIntensities = new List<(string, int, string, int, string, int, double)>();

            /// <summary>
            /// List of Intensities
            /// </summary>
            /// string,double,double -> Intensity file, monoisotopic mass, sum intensity
            List<(string, double, double)> Intensities;

            /// <summary>
            /// List<(MS/MS Data, Fragmentation Method, Activation Level, Precursor Charge State, Replicate, Intesity data)>
            /// </summary>
            foreach ((string, string, string, int, int, string) dataFile in programParams.InputFileList)
            {
                if (String.IsNullOrEmpty(dataFile.Item6))
                {
                    continue;
                }
                Console.WriteLine(" Reading {0} ...", dataFile.Item6);
                double monoIsotopicMass = 0;
                double intensity = 0;
                Intensities = new List<(string, double, double)>();

                try
                {
                    DataTableCollection dataTableCollection;
                    using (var stream = File.Open(dataFile.Item6, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });
                            dataTableCollection = result.Tables;
                            bool isSubRow = false;
                            foreach (DataRow dataRow in dataTableCollection[0].Rows)
                            {
                                var values = dataRow.ItemArray;
                                if (String.IsNullOrEmpty(values[1].ToString()))
                                {
                                    isSubRow = false;
                                    continue;
                                }
                                if (values[1].ToString().StartsWith("Charge"))
                                {
                                    isSubRow = true;
                                    continue;
                                }
                                if (isSubRow) continue;
                                monoIsotopicMass = Convert.ToDouble(values[1]);
                                intensity = Convert.ToDouble(values[2]);
                                Intensities.Add((dataFile.Item6, monoIsotopicMass, intensity));
                            }
                        }

                        FragIonsWithIntensities.AddRange(mainCore.MatchFragmentIonsAndIntensities(FragIons.Where(a=>a.Item8.Equals(dataFile.Item6)).ToList(), Intensities));
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

            mainCore.FragmentIons = FragIonsWithIntensities;
        }

        private void SetNullIntensitiesFragIons()
        {
            /// string,int,string,int -> FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity
            FragIonsWithIntensities = new List<(string, int, string, int, string, int, double)>();

            FragIons.ForEach(a =>
            {
                FragIonsWithIntensities.Add((a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6, 0));
            });
            mainCore.FragmentIons = FragIonsWithIntensities;
            FragIons = null;
        }
    }
}
