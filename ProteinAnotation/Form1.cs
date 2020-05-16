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

namespace ProteinAnnotation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonPeptideAnotation_Click(object sender, EventArgs e)
        {
            string protein = "QSALTQPRSVSGSPGQSVTISCTGTSSDIGGYNFVSWYQQHPGKAPKLMIYDATKRPSGVPDRFSGSKSGNTASLTISGLQAEDEADYYCCSYAGDYTPGVVFGGGTKLTVLGQPKAAPSVTLFPPSSEELQANKATLVCLISDFYPGAVTVAWKADSSPVKAGVETTTPSKQSNNKYAASSYLSLTPEQWKSHRSYSCQVTHEGSTVEKTVAPTECS";

            //List<(fragmentationMethod, precursorCharge,IonType, aaPosition)>
            List<(string, int, string, int)> fragmentIons = new List<(string, int, string, int)>();

            try
            {
                StreamReader sr = new StreamReader(@"C:\Users\diogo\Documents\Project2020\MergeFragIons\data\20200406_JD_031_Data_LC_fragments.txt");

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

            //this.protein.PreparePictureProteinFragmentIons(true, protein, fragmentIons);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
