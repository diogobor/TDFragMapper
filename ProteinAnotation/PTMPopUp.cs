using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProteinAnnotation
{
    public partial class PTMPopUp : Form
    {
        public bool IsOpen { get; set; }
        public PTMPopUp()
        {
            InitializeComponent();
            groupBoxPTM.Paint += PaintBorderlessGroupBox;
        }

        private void PaintBorderlessGroupBox(object sender, PaintEventArgs p)
        {
            GroupBox box = (GroupBox)sender;
            Graphics gfx = p.Graphics;
            Pen pen = new Pen(Color.Black, 1);
            gfx.DrawLine(pen, 0, 5, 0, p.ClipRectangle.Height - 2);
            gfx.DrawLine(pen, 0, 5, 10, 5);
            string title = groupBoxPTM.Text;
            int width = title.Length * 70 / 10;
            gfx.DrawLine(pen, width, 5, p.ClipRectangle.Width - 2, 5);
            gfx.DrawLine(pen, p.ClipRectangle.Width - 2, 5, p.ClipRectangle.Width - 2, p.ClipRectangle.Height - 2);
            gfx.DrawLine(pen, p.ClipRectangle.Width - 2, p.ClipRectangle.Height - 2, 0, p.ClipRectangle.Height - 2);
        }

        public void Setup(string aminoacid, List<String> ptms, Point positionPTM, Point screenPosition, Point parentPosition)
        {
            groupBoxPTM.Text = " " + aminoacid + " ";
            labelPTMs.Text = String.Join("", ptms.Distinct());
            timerPTM.Enabled = true;
            IsOpen = true;
            int width = groupBoxPTM.PreferredSize.Width > this.labelPTMs.Width ? groupBoxPTM.PreferredSize.Width : this.labelPTMs.Width;
            this.Width = width + 45;
            this.Height = this.labelPTMs.Height + 35;
            // ##### To correct position when window is open first time
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.Manual;
            this.BringToFront();
            // #####
            this.Location = new Point(positionPTM.X + screenPosition.X - parentPosition.X, positionPTM.Y + screenPosition.Y - parentPosition.Y);
        }

        private void timerPTM_Tick(object sender, EventArgs e)
        {
            IsOpen = false;
            this.Hide();

        }

        private void PTMPopUp_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
