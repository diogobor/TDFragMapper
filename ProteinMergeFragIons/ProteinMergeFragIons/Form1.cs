using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProteinMergeFragIons
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /// <summary>
            /// Main dictionary will all maps: <key: Study condition#FixedCondition1, value: (fixedCond1, fixedCond2, fixedCond3, allFragmentIonsAllConditions)>
            /// </summary>
            Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double)>)> DictMaps = new Dictionary<string, (string, string, string, List<(string, int, string, int, string, int, double)>)>();

            //string protein = "EVQLVESGGGLVQPGGSLRLSCVASGFTLNNYDMHWVRQGIGKGLEWVSKIEVQLVESGGGLVQPGGSLRLSCVASGFTLNNYDMHWVRQGIGKGLEWVSKIEVQLVESGGGLVQPGGSLRLSCVASGFTLNNYDMHWVRQGIGKGLEWVSKIEVQLVESGGGLVQPGGSLRLSCVASGFTLNNYDMHWVRQGIGKGLEWVSKIEVQLVESGGGLVQPGGSLRLSCVASGFTLNNYDMHWVRQGIGKGLEWVSKI";
            string protein = "QSALTQPRSVSGSPGQSVTISCTGTSSDIGGYNFVSWYQQHPGKAPKLMIYDATKRPSGVPDRFSGSKSGNTASLTISGLQAEDEADYYCCSYAGDYTPGVVFGGGTKLTVLGQPKAAPSVTLFPPSSEELQANKATLVCLISDFYPGAVTVAWKADSSPVKAGVETTTPSKQSNNKYAASSYLSLTPEQWKSHRSYSCQVTHEGSTVEKTVAPTECSQSALTQPRSVSGSPGQSVTISCTGTSSDIGGYNFVSWYQQHPGKAPKLMIYDATKRPSGVPDRFSGSKSGNTASLTISGLQAEDEADYYCCSYAGDYTPGVVFGGGTKLTVLGQPKAAPSVTLFPPSSEELQANKATLVCLISDFYPGAVTVAWKADSSPVKAGVETTTPSKQSNNKYAASSYLSLTPEQWKS";


            /// <summary>
            /// List of Fragment Ions
            /// </summary>
            /// string,int,string,int -> FragmentationMethod: UVPD, EThcD, CID, HCD, SID, ECD, ETD; PrecursorChargeState, IonType: A,B,C,X,Y,Z, Aminoacid Position, Activation Level, Replicate, Intensity
            List<(string, int, string, int, string, int, double)> fragIons = new List<(string, int, string, int, string, int, double)>();


            List<(string, string, string, int, int)> inputFiles = new List<(string, string, string, int, int)>();

            #region files
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID20_Precursor11_Replicate01.xlsx", "CID", "20", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID20_Precursor17_Replicate01.xlsx", "CID", "20", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID20_Precursor22_Replicate01.xlsx", "CID", "20", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID20_Precursor25_Replicate01.xlsx", "CID", "20", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID20_Precursor11_Replicate01.xlsx", "CID", "20", 11, 2));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID20_Precursor17_Replicate01.xlsx", "CID", "20", 17, 2));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID20_Precursor22_Replicate01.xlsx", "CID", "20", 22, 2));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID20_Precursor25_Replicate01.xlsx", "CID", "20", 25, 2));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID25_Precursor11_Replicate01.xlsx", "CID", "25", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID25_Precursor17_Replicate01.xlsx", "CID", "25", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID25_Precursor22_Replicate01.xlsx", "CID", "25", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID25_Precursor25_Replicate01.xlsx", "CID", "25", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID27_Precursor11_Replicate01.xlsx", "CID", "27", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID27_Precursor17_Replicate01.xlsx", "CID", "27", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID27_Precursor22_Replicate01.xlsx", "CID", "27", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID27_Precursor25_Replicate01.xlsx", "CID", "27", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID30_Precursor11_Replicate01.xlsx", "CID", "30", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID30_Precursor17_Replicate01.xlsx", "CID", "30", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID30_Precursor22_Replicate01.xlsx", "CID", "30", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\CID\\LC_CID30_Precursor25_Replicate01.xlsx", "CID", "30", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET5hcD5_Precursor11_Replicate01.xlsx", "EThcD", "5/5", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET5hcD5_Precursor17_Replicate01.xlsx", "EThcD", "5/5", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET5hcD5_Precursor22_Replicate01.xlsx", "EThcD", "5/5", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET5hcD5_Precursor25_Replicate01.xlsx", "EThcD", "5/5", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET10hcD8_Precursor11_Replicate01.xlsx", "EThcD", "10/8", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET10hcD8_Precursor17_Replicate01.xlsx", "EThcD", "10/8", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET10hcD8_Precursor22_Replicate01.xlsx", "EThcD", "10/8", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET10hcD8_Precursor25_Replicate01.xlsx", "EThcD", "10/8", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET10hcD10_Precursor11_Replicate01.xlsx", "EThcD", "10/10", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET10hcD10_Precursor17_Replicate01.xlsx", "EThcD", "10/10", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET10hcD10_Precursor22_Replicate01.xlsx", "EThcD", "10/10", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET10hcD10_Precursor25_Replicate01.xlsx", "EThcD", "10/10", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET15hcD8_Precursor11_Replicate01.xlsx", "EThcD", "15/8", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET15hcD8_Precursor17_Replicate01.xlsx", "EThcD", "15/8", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET15hcD8_Precursor22_Replicate01.xlsx", "EThcD", "15/8", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET15hcD8_Precursor25_Replicate01.xlsx", "EThcD", "15/8", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET15hcD10_Precursor11_Replicate01.xlsx", "EThcD", "15/10", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET15hcD10_Precursor17_Replicate01.xlsx", "EThcD", "15/10", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET15hcD10_Precursor22_Replicate01.xlsx", "EThcD", "15/10", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\EThcD\\LC_ET15hcD10_Precursor25_Replicate01.xlsx", "EThcD", "15/10", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD5_Precursor11_Replicate01.xlsx", "HCD", "5", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD5_Precursor17_Replicate01.xlsx", "HCD", "5", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD5_Precursor22_Replicate01.xlsx", "HCD", "5", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD5_Precursor25_Replicate01.xlsx", "HCD", "5", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD8_Precursor11_Replicate01.xlsx", "HCD", "8", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD8_Precursor17_Replicate01.xlsx", "HCD", "8", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD8_Precursor22_Replicate01.xlsx", "HCD", "8", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD8_Precursor25_Replicate01.xlsx", "HCD", "8", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD10_Precursor17_Replicate01.xlsx", "HCD", "10", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD10_Precursor22_Replicate01.xlsx", "HCD", "10", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD10_Precursor25_Replicate01.xlsx", "HCD", "10", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD12_Precursor11_Replicate01.xlsx", "HCD", "12", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD10_Precursor11_Replicate01.xlsx", "HCD", "10", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD12_Precursor17_Replicate01.xlsx", "HCD", "12", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD12_Precursor22_Replicate01.xlsx", "HCD", "12", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\HCD\\LC_HCD12_Precursor25_Replicate01.xlsx", "HCD", "12", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD35_Precursor11_Replicate01.xlsx", "UVPD", "35", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD35_Precursor17_Replicate01.xlsx", "UVPD", "35", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD35_Precursor22_Replicate01.xlsx", "UVPD", "35", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD35_Precursor25_Replicate01.xlsx", "UVPD", "35", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD40_Precursor11_Replicate01.xlsx", "UVPD", "40", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD40_Precursor17_Replicate01.xlsx", "UVPD", "40", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD40_Precursor22_Replicate01.xlsx", "UVPD", "40", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD40_Precursor25_Replicate01.xlsx", "UVPD", "40", 25, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD45_Precursor11_Replicate01.xlsx", "UVPD", "45", 11, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD45_Precursor17_Replicate01.xlsx", "UVPD", "45", 17, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD45_Precursor22_Replicate01.xlsx", "UVPD", "45", 22, 1));
            inputFiles.Add(("Z:\\data\\Data_files\\UVPD\\LC_UVPD45_Precursor25_Replicate01.xlsx", "UVPD", "45", 25, 1));
            #endregion

            /// <summary>
            /// List<(MS/MS Data, Fragmentation Method, Activation Level, Precursor Charge State, Replicate)>
            /// </summary>
            foreach ((string, string, string, int, int) dataFile in inputFiles)
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
                                fragIons.Add((fragMethod, precursorChargeState, ionType, aminoacidPos, activationLevel, replicate, 0));
                        }
                    }
                }
                catch (Exception exce)
                {
                    System.Windows.Forms.MessageBox.Show(
                                        "Error to read some files:\n" + exce.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                }
            }

            List<(string, int, string, int, string, int, double)> tmpFragIons = new List<(string, int, string, int, string, int, double)>();
            double count = 0.5;
            foreach ((string, int, string, int, string, int, double) frag in fragIons)
            {
                tmpFragIons.Add((frag.Item1, frag.Item2, frag.Item3, frag.Item4, frag.Item5, frag.Item6, count));
                count += 1.75;
            }
            fragIons = tmpFragIons;

            //Example: CID, Act level: 10,15,20, Repl: 1, Prec Charge: 25,22,17,11

            //List<(fragmentationMethod, precursorCharge,IonType, aaPosition,activation level,replicate, intensity)>
            List<(string, int, string, int, string, int, double)> currentFragIons = new List<(string, int, string, int, string, int, double)>();
            string _key = string.Empty;

            ////currentFragIons = fragIons.Where(a => a.Item1.Equals("HCD") && a.Item6 == 1 && (a.Item5.Equals("8") || a.Item5.Equals("10") || a.Item5.Equals("12"))).ToList();
            //currentFragIons = fragIons.Where(a => a.Item1.Equals("HCD") && (a.Item2 == 25 || a.Item2 == 22)).ToList();
            //_key = "Precursor Charge State#Fragmentation Method#HCD#0";
            //DictMaps.Add(_key, ("Fragmentation Method", "Activation Level", "Replicates", currentFragIons));

            //currentFragIons = fragIons.Where(a => a.Item1.Equals("HCD") && (a.Item2 == 17)).ToList();
            //_key = "Precursor Charge State#Fragmentation Method#HCD#1";
            //DictMaps.Add(_key, ("Fragmentation Method", "Activation Level", "Replicates", currentFragIons));

            //currentFragIons = fragIons.Where(a => a.Item1.Equals("EThcD") && (a.Item2 == 25 || a.Item2 == 11 || a.Item2 == 22)).ToList();
            //_key = "Precursor Charge State#Fragmentation Method#EThcD#0";
            //DictMaps.Add(_key, ("Fragmentation Method", "Activation Level", "Replicates", currentFragIons));

            ////currentFragIons = fragIons.Where(a => a.Item1.Equals("CID") && a.Item6 == 1 && (a.Item5.Equals("25") || a.Item5.Equals("20"))).ToList();
            //currentFragIons = fragIons.Where(a => a.Item1.Equals("CID") && (a.Item2 == 22 || a.Item2 == 17)).ToList();
            //_key = "Precursor Charge State#Fragmentation Method#CID#0";
            //DictMaps.Add(_key, ("Fragmentation Method", "Activation Level", "Replicates", currentFragIons));

            //currentFragIons = fragIons.Where(a => a.Item1.Equals("CID") && a.Item6 == 1 && (a.Item5.Equals("25") || a.Item5.Equals("20"))).ToList();
            currentFragIons = fragIons.Where(a => a.Item1.Equals("CID") && (a.Item2 == 22 || a.Item2 == 17) /*&& a.Item6 == 1*/).ToList();
            _key = "Replicates#Fragmentation Method#CID#0";
            DictMaps.Add(_key, ("Fragmentation Method", "Activation Level", "Precursor Charge State", currentFragIons));

            //currentFragIons = fragIons.Where(a => a.Item1.Equals("CID") && (a.Item2 == 22 || a.Item2 == 17) && a.Item6 == 2).ToList();
            //_key = "Replicates#Fragmentation Method#CID#1";
            //DictMaps.Add(_key, ("Fragmentation Method", "Activation Level", "Precursor Charge State", currentFragIons));

            //currentFragIons = fragIons.Where(a => a.Item1.Equals("UVPD") && a.Item6 == 1 && (a.Item5.Equals("35") || a.Item5.Equals("40"))).ToList();
            //_key = "Precursor Charge State#Fragmentation Method#UVPD#0";
            //DictMaps.Add(_key, ("Fragmentation Method", "Activation Level", "Replicates", currentFragIons));

            //currentFragIons = fragIons.Where(a => a.Item1.Equals("CID") && a.Item2 == 25 && a.Item6 == 1 && (a.Item5.Equals("25") || a.Item5.Equals("27"))).ToList();
            //_key = "Activation Level#Fragmentation Method#CID#0";
            //DictMaps.Add(_key, ("Fragmentation Method", "Precursor Charge State", "Replicates", currentFragIons));

            //currentFragIons = fragIons.Where(a => a.Item1.Equals("EThcD") && (a.Item2 == 17 || a.Item2 == 11) && a.Item6 == 1 && (a.Item5.Equals("5/5") || a.Item5.Equals("10/8"))).ToList();
            //_key = "Activation Level#Fragmentation Method#EThcD#0";
            //DictMaps.Add(_key, ("Fragmentation Method", "Precursor Charge State", "Replicates", currentFragIons));

            //currentFragIons = fragIons.Where(a => a.Item1.Equals("EThcD") && (a.Item2 == 17 || a.Item2 == 11) && a.Item6 == 1 && (a.Item5.Equals("10/10"))).ToList();
            //_key = "Activation Level#Fragmentation Method#EThcD#1";
            //DictMaps.Add(_key, ("Fragmentation Method", "Precursor Charge State", "Replicates", currentFragIons));

            //currentFragIons = fragIons.Where(a => a.Item2 == 25 && (a.Item1.Equals("CID")|| a.Item1.Equals("HCD"))).ToList();
            //_key = "Fragmentation Method#Precursor Charge State#25#0";
            //DictMaps.Add(_key, ("Precursor Charge State", "Activation Level", "Replicates", currentFragIons));

            //currentFragIons = fragIons.Where(a => a.Item2 == 25 && (a.Item1.Equals("EThcD") || a.Item1.Equals("UVPD"))).ToList();
            //_key = "Fragmentation Method#Precursor Charge State#25#1";
            //DictMaps.Add(_key, ("Precursor Charge State", "Activation Level", "Replicates", currentFragIons));

            currentFragIons = fragIons.Where(a => a.Item1.Equals("CID") && (a.Item2 == 22 || a.Item2 == 17)).ToList();

            #region creating merged fragIons 

            //List<(fragmentationMethod, precursorCharge,IonType, aaPosition,activation level,replicate, intensity)>
            List<(string, int, string, int, string, int, double)> currentNtermFragIons = currentFragIons.Where(a => a.Item3.Equals("A") || a.Item3.Equals("B") || a.Item3.Equals("C")).ToList();
            List<(string, int, string, int, string, int, double)> currentCtermFragIons = currentFragIons.Where(a => a.Item3.Equals("X") || a.Item3.Equals("Y") || a.Item3.Equals("Z")).ToList();

            var groupedNtermFragIons = currentNtermFragIons.GroupBy(a => a.Item4).Select(grp => grp.ToList()).ToList();
            currentNtermFragIons = new List<(string, int, string, int, string, int, double)>();
            foreach (var nTermFragIon in groupedNtermFragIons)
                currentNtermFragIons.Add(("", 0, "B", nTermFragIon[0].Item4, "", 1, nTermFragIon.Count));

            var groupedCtermFragIons = currentCtermFragIons.GroupBy(a => a.Item4).Select(grp => grp.ToList()).ToList();
            currentCtermFragIons = new List<(string, int, string, int, string, int, double)>();
            foreach (var cTermFragIon in groupedCtermFragIons)
                currentCtermFragIons.Add(("", 0, "Y", cTermFragIon[0].Item4, "", 1, cTermFragIon.Count));


            currentFragIons = currentNtermFragIons.Concat(currentCtermFragIons).ToList();
            #endregion

            //_key = "Merge#Merge#Merge#0";
            //DictMaps.Add(_key, ("Merge", "Merge", "Merge", currentFragIons));

            this.proteinFragIons1.SetFragMethodDictionary(DictMaps, protein, "N-Term Pyro-Glu", false, false, false);

            return;







            //List<(fragmentationMethod, precursorCharge,IonType, aaPosition)>
            List<(string, int, string, int)> fragmentIons = new List<(string, int, string, int)>();

            #region temp set up
            Dictionary<string, List<int>> FragMethodsWithPrecursorChargeList = new Dictionary<string, List<int>>();

            //FragMethodsWithPrecursorChargeList.Add("UVPD", new List<int>() { 58, 54, 47, 39 });
            //FragMethodsWithPrecursorChargeList.Add("UVPD", new List<int>() { 25, 22, 17, 11 });
            //FragMethodsWithPrecursorChargeList.Add("ETHCD", new List<int>() { 25, 22, 17, 11 });
            //FragMethodsWithPrecursorChargeList.Add("SID", new List<int>() { 25, 22, 17, 11 });
            //FragMethodsWithPrecursorChargeList.Add("CID", new List<int>() { 58, 54, 47, 39 });
            //FragMethodsWithPrecursorChargeList.Add("CID", new List<int>() { 25, 22, 11 });
            //FragMethodsWithPrecursorChargeList.Add("HCD", new List<int>() { 58, 54, 47, 39 });
            FragMethodsWithPrecursorChargeList.Add("HCD", new List<int>() { 25, 22, 17, 11 });
            //FragMethodsWithPrecursorChargeList.Add("ECD", new List<int>() { 25, 22, 11 });
            //FragMethodsWithPrecursorChargeList.Add("ETD", new List<int>() { 25, 22, 11 });

            #endregion

            try
            {
                StreamReader sr = new StreamReader(@"C:\Users\diogo\Documents\Project2020\MergeFragIons\data\20200406_JD_031_Data_LC_fragments.txt");
                //StreamReader sr = new StreamReader(@"Z:\data\20200406_JD_031_Data_HC_fragments.txt");

                string fragMethod = "";
                int precursorCharge = 0;
                string ionType = "";
                int aminoacidPosition = 0;

                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("Fragmentation method")) continue;
                    if (line.Length > 0)
                    {
                        string[] cols = Regex.Split(line, "\t");
                        fragMethod = cols[0];
                        precursorCharge = Convert.ToInt32(cols[1].Substring(0, cols[1].Length - 1));
                        ionType = cols[2];
                        aminoacidPosition = Convert.ToInt32(cols[3]);
                        fragmentIons.Add((fragMethod, precursorCharge, ionType, aminoacidPosition));
                    }
                }
                sr.Close();
            }
            catch (Exception) { }

            //this.proteinFragIons1.SetFragMethodDictionary(FragMethodsWithPrecursorChargeList);
            //this.proteinFragIons1.PreparePictureProteinFragmentIons(true, protein, fragmentIons);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.proteinFragIons1.SaveFragmentIonsImage();
        }
        private SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }
    }
}
