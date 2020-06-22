/**
 * Program:     TDFragMapper
 * Author:      Diogo Borges Lima
 * Update:      6/21/2020
 * Update by:   Diogo Borges Lima
 * Description: About class
 */
using TDFragMapper.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDFragMapper
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {

            string platform = System.Environment.OSVersion.Platform.ToString();
            if (platform.Contains("Win"))
            {
                osNameLabel.Text = Util.GetWindowsVersion();
            }
            else
            {
                osNameLabel.Text = "Unix";
            }

            processorNameLabel.Text = Util.GetProcessorName();
            RAMmemoryLabel.Text = Util.GetRAMMemory();

            string version = System.Environment.OSVersion.ServicePack.ToString();
            if (!version.Equals(""))
            {
                if (Util.Is64Bits())
                    versionOSLabel.Text = version + " (64 bits)";
                else
                    versionOSLabel.Text = version + " (32 bits)";
            }
            else
            {
                if (Util.Is64Bits())
                    versionOSLabel.Text = "64 bits";
                else
                    versionOSLabel.Text = "32 bits";
            }
            usrLabel.Text = System.Environment.UserName.ToString();
            machineNameLabel.Text = System.Environment.MachineName.ToString();

            try
            {
                versionNumberLabel.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch (Exception exception)
            {
                //Unable to retrieve version number
                Console.WriteLine("", exception.Message);
            }
        }

        private void buttonInitialSetting_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("Are you sure you want to reset the setting values ?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (answer == DialogResult.Yes)
            {
                MessageBox.Show("All values have been reset sucessfully!\nSoftware will be restarted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
        }
    }
}
