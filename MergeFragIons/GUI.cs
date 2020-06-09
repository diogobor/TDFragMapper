/**
 * Program:     Merge Fragment Ions
 * Author:      Diogo Borges Lima
 * Update:      4/12/2020
 * Update by:   Diogo Borges Lima
 * Description: GUI class
 */
using MergeFragIons.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MergeFragIons
{
    public partial class GUI : Form
    {
        private TextWriter _writer = null; // That's our custom TextWriter class
        private Thread mainThread { get; set; }
        private Program mainProgramGUI { get; set; }

        private string[] args { get; set; }

        public GUI(string[] args)
        {
            InitializeComponent();

            this.args = args;

            ActiveListBoxControl();

            // Create a Save button column
            DataGridViewImageButtonBrowseColumn columnBrowse = new DataGridViewImageButtonBrowseColumn();
            // Set column values
            columnBrowse.Name = "btnMSMSInputFile";
            columnBrowse.HeaderText = "";
            this.dataGridViewInputFiles.Columns.Add(columnBrowse);

            AddNewRowDatagridInputFiles();
        }

        /// <summary>
        /// Method responsible for filling datagridViewInputFiles
        /// </summary>
        private void AddNewRowDatagridInputFiles()
        {
            dataGridViewInputFiles.Rows.Add();

            foreach (DataGridViewRow row in dataGridViewInputFiles.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                row.HeaderCell.Style.Font = new Font("Tahoma", 9.75F);
                row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                DataGridViewButtonCell btnCell = (DataGridViewButtonCell)row.Cells[5];
                btnCell.FlatStyle = FlatStyle.Standard;
                btnCell.Value = "Browse file";
            }
            dataGridViewInputFiles.RowHeadersWidth = 50;
        }

        private void ActiveListBoxControl()
        {
            // Instantiate the writer
            _writer = new ListBoxStreamWriter(listBoxConsole);
            // Redirect the out Console stream

            // Close previous output stream and redirect output to standard output.
            Console.Out.Close();
            Console.SetOut(_writer);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
            System.Windows.Forms.Application.Exit();
        }

        private void buttonBottomUpResults_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Protein file (*.txt)|*.txt";
            openFileDialog.FileName = "";
            if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                textBoxProteinSeq.Text = openFileDialog.FileName;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ProgramParams myParams = GetParamsFromScreen();
            if (!CheckParams(myParams)) return;

            if (mainThread == null)
            {
                mainProgramGUI = new Program();
                mainProgramGUI.programParams = myParams;
                mainThread = new Thread(new ThreadStart(mainProgramGUI.ReadInputFiles));
            }

            if (buttonOK.Text.Equals("OK") && mainProgramGUI != null)
            {
                mainThread.Start();
                timerStatus.Enabled = true;

                #region Disabling some fields
                groupBoxInputFiles.Enabled = false;
                buttonBrowseProteinSequence.Enabled = false;
                buttonAddInputFile.Enabled = false;
                buttonDisplay.Enabled = false;
                this.userControlFilterCondition1.Enabled = false;
                #endregion

                buttonOK.Text = "Stop";
                //change button icon
                buttonOK.Image = MergeFragIons.Properties.Resources.button_cancel_little;
            }
            else if (mainProgramGUI != null)
            {
                DialogResult answer = System.Windows.Forms.MessageBox.Show("Are you sure you want to stop the process ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (answer == DialogResult.Yes)
                {
                    #region Enabling some fields
                    groupBoxInputFiles.Enabled = true;
                    buttonBrowseProteinSequence.Enabled = true;
                    buttonAddInputFile.Enabled = true;
                    buttonDisplay.Enabled = false;
                    this.userControlFilterCondition1.Enabled = false;
                    #endregion

                    buttonOK.Text = "OK";
                    //change button icon
                    buttonOK.Image = MergeFragIons.Properties.Resources.goBtn;

                    if (mainThread != null)//When xlThread is null in this point is because the user stops the process, but before clicking on Yes button, the search is finished.
                    {
                        mainThread.Abort();
                        mainThread = null;
                    }
                }
            }
        }

        private bool CheckParams(ProgramParams myParams)
        {
            if (String.IsNullOrEmpty(myParams.ProteinSequenceFile))
            {
                System.Windows.Forms.MessageBox.Show(
                        "'Protein Sequence' field is empty. Please, select one directory.",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                return false;
            }

            if (!String.IsNullOrEmpty(myParams.msgErrorInputFiles))
            {
                string error = myParams.msgErrorInputFiles.ToString().Substring(0, myParams.msgErrorInputFiles.ToString().Length - 2);
                System.Windows.Forms.MessageBox.Show(
                                        "Some fields are empty on the following lines:\n" + error,
                                        "Warning",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private ProgramParams GetParamsFromScreen()
        {
            ProgramParams myParams = new ProgramParams();
            myParams.ProteinSequenceFile = textBoxProteinSeq.Text;
            myParams.SequenceInformation = textBoxSeqInfo.Text;

            /// <summary>
            /// List<(MS/MS Data, Fragmentation Method, Activation Level, Precursor Charge State, Replicate)>
            /// </summary>
            List<(string, string, string, int, int)> inputFileList = new List<(string, string, string, int, int)>();

            #region Read datagridviewInputfiles
            StringBuilder sbError = new StringBuilder();
            for (int iRow = 0; iRow < dataGridViewInputFiles.RowCount; iRow++)
            {
                DataGridViewRow GridRow = dataGridViewInputFiles.Rows[iRow];
                if ((GridRow.Cells[0].Value != null && !String.IsNullOrEmpty(GridRow.Cells[0].Value.ToString())) &&
                    (GridRow.Cells[1].Value != null && !String.IsNullOrEmpty(GridRow.Cells[1].Value.ToString())) &&
                    (GridRow.Cells[2].Value != null && !String.IsNullOrEmpty(GridRow.Cells[2].Value.ToString())) &&
                    (GridRow.Cells[3].Value != null && !String.IsNullOrEmpty(GridRow.Cells[3].Value.ToString())) &&
                    (GridRow.Cells[4].Value != null && !String.IsNullOrEmpty(GridRow.Cells[4].Value.ToString())))
                {
                    if (GridRow.Cells[0].Value.ToString().ToLower().Equals("etchd"))
                    {
                        inputFileList.Add((GridRow.Cells[4].Value.ToString(),
                            "EThcD",
                            GridRow.Cells[1].Value.ToString(),
                            Convert.ToInt32(GridRow.Cells[2].Value.ToString()),
                            Convert.ToInt32(GridRow.Cells[3].Value.ToString())));
                    }
                    else
                    {
                        inputFileList.Add((GridRow.Cells[4].Value.ToString(),
                            GridRow.Cells[0].Value.ToString(),
                            GridRow.Cells[1].Value.ToString(),
                            Convert.ToInt32(GridRow.Cells[2].Value.ToString()),
                            Convert.ToInt32(GridRow.Cells[3].Value.ToString())));
                    }
                }
                else
                    sbError.Append((iRow + 1) + ", ");
            }
            myParams.msgErrorInputFiles = sbError.ToString();

            myParams.InputFileList = inputFileList;
            #endregion
            return myParams;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //About aboutScreen = new About();
            //aboutScreen.ShowDialog();
        }

        private void resultsBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ResultsForm window = new ResultsForm();
            //window.ShowDialog();
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            if (mainProgramGUI != null && mainProgramGUI.FinishProcessing)
            {

                mainThread = null;
                timerStatus.Enabled = false;

                #region Enabling some fields
                groupBoxInputFiles.Enabled = true;
                buttonBrowseProteinSequence.Enabled = true;
                buttonAddInputFile.Enabled = true;
                buttonDisplay.Enabled = true;
                this.userControlFilterCondition1.Enabled = true;
                #endregion

                buttonOK.Text = "OK";
                //change button icon
                buttonOK.Image = MergeFragIons.Properties.Resources.goBtn;

                this.userControlFilterCondition1.Setup(mainProgramGUI.mainCore);
                tabControlMainWindow.SelectedIndex = 1;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Coming soon!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //ResultsForm window = new ResultsForm();
            //if (window.LoadResults())
            //    window.ShowDialog();
        }

        private void readMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Coming soon!\n\nDeveloped by:\nDiogo Borges Lima (CeMM) - diogobor@gmail.com,\nJonathan Dhenin (Institut Pasteur) - jonathan.dhenin@pasteur.fr, & \nMathieu Dupré (Institut Pasteur) - mathieu.dupre@pasteur.fr", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //HelpForm help = new HelpForm();
            //help.ShowDialog();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            #region open automatically a pcmb file
            try
            {
                string[] myAppData = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;

                Console.WriteLine(" Loading file: " + myAppData[0]);

                //ResultsForm window = new ResultsForm();
                //if (window.LoadResultsFromScreen(myAppData[0]))
                //    window.ShowDialog();
                //else
                //    MessageBox.Show("Failed to load file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception) { }

            if (args != null && args.Length > 0)
            {
                string[] myAppData = args;

                Console.WriteLine(" Loading file: " + myAppData[0]);

                //ResultsForm window = new ResultsForm();
                //if (window.LoadResultsFromScreen(myAppData[0]))
                //    window.ShowDialog();
                //else
                //    MessageBox.Show("Failed to load file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion
        }

        private void dataGridViewInputFiles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
            {
                if (dataGridViewInputFiles.Columns[e.ColumnIndex].GetType().Equals(typeof(DataGridViewImageButtonBrowseColumn)))
                {
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    dlg.FileName = ""; // Default file name
                    dlg.DefaultExt = ".xlsx"; // Default file extension
                    dlg.Filter = "Excel File(*.xlsx)|*.xlsx|CSV File (*.csv)|*.csv";
                    dlg.Title = "Open input files";

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();
                    if (result == true)
                    {
                        dataGridViewInputFiles[4, e.RowIndex].Value = dlg.FileName;
                    }
                }
            }
        }

        private void dataGridViewInputFiles_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(ColumnOnlyNumbers_KeyPress);
            e.Control.KeyPress -= new KeyPressEventHandler(ColumnActivationLevel_KeyPress);
            e.Control.KeyPress -= new KeyPressEventHandler(ColumnFragMeth_KeyPress);

            if (dataGridViewInputFiles.CurrentCell.ColumnIndex == 2 || dataGridViewInputFiles.CurrentCell.ColumnIndex == 3) //Precursor Charge State & Replicate cols
            {
                TextBox tb = e.Control as TextBox;
                tb.CharacterCasing = CharacterCasing.Upper;

                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(ColumnOnlyNumbers_KeyPress);
                }
            }

            if (dataGridViewInputFiles.CurrentCell.ColumnIndex == 1) //Activation Level col
            {
                TextBox tb = e.Control as TextBox;
                tb.CharacterCasing = CharacterCasing.Upper;

                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(ColumnActivationLevel_KeyPress);
                }
            }

            if (dataGridViewInputFiles.CurrentCell.ColumnIndex == 0) //Frag Method col
            {
                TextBox tb = e.Control as TextBox;
                tb.CharacterCasing = CharacterCasing.Upper;

                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(ColumnFragMeth_KeyPress);
                }
            }
        }

        private void ColumnFragMeth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)
                || e.KeyChar == 8 /* 'backspace' */
                || e.KeyChar == 3 /* CTRL + C */)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            if (e.KeyChar == 22)/* CTRL + V */
                PasteDataToDataGridView();
        }

        private void ColumnOnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)
                || e.KeyChar == 8 /* 'backspace' */
                || e.KeyChar == 3 /* CTRL + C */)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            if (e.KeyChar == 22)/* CTRL + V */
                PasteDataToDataGridView();
        }

        private void ColumnActivationLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)
                || e.KeyChar == 47
                || e.KeyChar == 8 /* 'backspace' */
                || e.KeyChar == 3 /* CTRL + C */)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            if (e.KeyChar == 22)/* CTRL + V */
                PasteDataToDataGridView();
        }

        private void PasteDataToDataGridView()
        {
            #region Clean datagridView
            try
            {
                //Clear DataGridView
                int rowCount = dataGridViewInputFiles.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    dataGridViewInputFiles.Rows.Clear();
                    --rowCount;
                }
            }
            catch (Exception) { }
            #endregion

            #region Fill datagridView
            try
            {
                string s = System.Windows.Clipboard.GetText();
                string[] lines = Regex.Split(s, "[\r\n]");
                int iCol = 0;
                DataGridViewCell oCell;
                foreach (string line in lines)
                {
                    if (!String.IsNullOrEmpty(line))
                    {
                        int iRow = dataGridViewInputFiles.Rows.Add();
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < this.dataGridViewInputFiles.ColumnCount)
                            {
                                oCell = dataGridViewInputFiles[iCol + i, iRow];
                                oCell.Value = Convert.ChangeType(sCells[i], oCell.ValueType);
                            }
                            else break;
                        }
                    }
                    else continue;
                }
            }
            catch (FormatException)
            {
                System.Windows.MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
            #endregion

            #region Format datagridView
            foreach (DataGridViewRow row in dataGridViewInputFiles.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                row.HeaderCell.Style.Font = new Font("Tahoma", 9.75F);
                row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                DataGridViewButtonCell btnCell = (DataGridViewButtonCell)row.Cells[5];
                btnCell.FlatStyle = FlatStyle.Standard;
                btnCell.Value = "Browse file";
            }
            dataGridViewInputFiles.RowHeadersWidth = 50;
            #endregion
        }

        private void buttonAddInputFile_Click(object sender, EventArgs e)
        {
            AddNewRowDatagridInputFiles();
        }

        private void dataGridViewInputFiles_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 22)/* CTRL + V */
                PasteDataToDataGridView();
        }

        private void buttonDisplay_Click(object sender, EventArgs e)
        {
            if(mainProgramGUI.mainCore.DictMaps.Count==0)
            {
                System.Windows.Forms.MessageBox.Show(
                        "There is no Map. Please, create one before displaying results.",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                return;
            }
            Results results = new Results();
            results.Setup(mainProgramGUI.mainCore);
            results.ShowDialog();
        }
    }


    /// <summary>
    /// An abastract class that implements the functionality of an image button
    /// except for a single abstract method to load the Normal, Hot and Disabled 
    /// images that represent the icon that is displayed on the button. The loading
    /// of these images is done in each derived concrete class.
    /// </summary>
    public abstract class DataGridViewImageButtonCell : DataGridViewButtonCell
    {
        private bool _enabled;                // Is the button enabled
        private PushButtonState _buttonState; // What is the button state
        protected Image _buttonImageHot;      // The hot image
        protected Image _buttonImageNormal;   // The normal image
        protected Image _buttonImageDisabled; // The disabled image
        private int _buttonImageOffset;       // The amount of offset or border around the image

        protected DataGridViewImageButtonCell()
        {
            // In my project, buttons are disabled by default
            _enabled = false;
            _buttonState = PushButtonState.Disabled;

            // Changing this value affects the appearance of the image on the button.
            _buttonImageOffset = 2;

            // Call the routine to load the images specific to a column.
            LoadImages();
        }

        // Button Enabled Property
        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;
                _buttonState = value ? PushButtonState.Normal : PushButtonState.Disabled;
            }
        }

        // PushButton State Property
        public PushButtonState ButtonState
        {
            get { return _buttonState; }
            set { _buttonState = value; }
        }

        // Image Property
        // Returns the correct image based on the control's state.
        public Image ButtonImage
        {
            get
            {
                switch (_buttonState)
                {
                    case PushButtonState.Disabled:
                        return _buttonImageDisabled;

                    case PushButtonState.Hot:
                        return _buttonImageHot;

                    case PushButtonState.Normal:
                        return _buttonImageNormal;

                    case PushButtonState.Pressed:
                        return _buttonImageNormal;

                    case PushButtonState.Default:
                        return _buttonImageNormal;

                    default:
                        return _buttonImageNormal;
                }
            }
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            //base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            // Draw the cell background, if specified.
            if ((paintParts & DataGridViewPaintParts.Background) ==
                DataGridViewPaintParts.Background)
            {
                SolidBrush cellBackground =
                    new SolidBrush(cellStyle.BackColor);
                graphics.FillRectangle(cellBackground, cellBounds);
                cellBackground.Dispose();
            }

            // Draw the cell borders, if specified.
            if ((paintParts & DataGridViewPaintParts.Border) ==
                DataGridViewPaintParts.Border)
            {
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                    advancedBorderStyle);
            }

            // Calculate the area in which to draw the button.
            // Adjusting the following algorithm and values affects
            // how the image will appear on the button.
            Rectangle buttonArea = cellBounds;

            Rectangle buttonAdjustment =
                BorderWidths(advancedBorderStyle);

            buttonArea.X += buttonAdjustment.X;
            buttonArea.Y += buttonAdjustment.Y;
            buttonArea.Height -= buttonAdjustment.Height;
            buttonArea.Width -= buttonAdjustment.Width;

            Rectangle imageArea = new Rectangle(
                buttonArea.X + _buttonImageOffset,
                buttonArea.Y + _buttonImageOffset,
                16,
                16);

            ButtonRenderer.DrawButton(graphics, buttonArea, ButtonImage, imageArea, false, ButtonState);
        }

        // An abstract method that must be created in each derived class.
        // The images in the derived class will be loaded here.
        public abstract void LoadImages();
    }

    /// <summary>
    /// Create a column class to display the Save buttons.
    /// </summary>
    public class DataGridViewImageButtonBrowseColumn : DataGridViewButtonColumn
    {
        public DataGridViewImageButtonBrowseColumn()
        {
            this.CellTemplate = new DataGridViewImageButtonBrowseCell();
            this.Width = 25;
            this.Resizable = DataGridViewTriState.False;
        }
    }

    /// <summary>
    /// Create a cell class to display the Save button cells. It is derived from the 
    /// abstract class DataGridViewImageButtonCell. The only method that has to be 
    /// implemented is LoadImages to load the Normal, Hot and Disabled Save images.
    /// </summary>
    public class DataGridViewImageButtonBrowseCell : DataGridViewImageButtonCell
    {
        public override void LoadImages()
        {

            // Load the Normal, Hot and Disabled "Delete" images here.
            // Load them from a resource file, local file, hex string, etc.

            BitmapSource bmpSource = Imaging.CreateBitmapSourceFromHBitmap(
                                   MergeFragIons.Properties.Resources.load.GetHbitmap(),
                                   IntPtr.Zero,
                                   Int32Rect.Empty,
                                   BitmapSizeOptions.FromEmptyOptions());

            _buttonImageHot = (Image)MergeFragIons.Properties.Resources.load;
            _buttonImageNormal = (Image)MergeFragIons.Properties.Resources.load;
            _buttonImageDisabled = (Image)MergeFragIons.Properties.Resources.load;
        }
    }
}
