/**
 * Program:     TDFragMapper
 * Author:      Diogo Borges Lima
 * Update:      4/12/2020
 * Update by:   Diogo Borges Lima
 * Description: GUI class
 */
using TDFragMapper.Utils;
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
using System.Reflection;

namespace TDFragMapper
{
    public partial class GUI : Form
    {
        private TextWriter _writer = null; // That's our custom TextWriter class
        private Thread mainThread { get; set; }
        private Program mainProgramGUI { get; set; }
        private Regex numberCaptured = new Regex("[0-9|\\.]+", RegexOptions.Compiled);

        private string[] args { get; set; }

        public GUI(string[] args)
        {
            InitializeComponent();

            this.args = args;

            ActiveListBoxControl();

            // Create a Save button column
            DataGridViewImageButtonBrowseColumn columnBrowseMSMSData = new DataGridViewImageButtonBrowseColumn();
            // Set column values
            columnBrowseMSMSData.Name = "btnMSMSInputFile";
            columnBrowseMSMSData.HeaderText = "";
            this.dataGridViewInputFiles.Columns.Insert(5, columnBrowseMSMSData);

            DataGridViewImageButtonBrowseColumn columnBrowseDeconvSpectra = new DataGridViewImageButtonBrowseColumn();
            // Set column values
            columnBrowseDeconvSpectra.Name = "btnDeconvSpectraInputFile";
            columnBrowseDeconvSpectra.HeaderText = "";
            this.dataGridViewInputFiles.Columns.Insert(7, columnBrowseDeconvSpectra);

            AddNewRowDatagridInputFiles();
        }

        public void UpdateMaps()
        {
            this.userControlFilterCondition1.ResetMaps();
            if (buttonDisplay.Enabled == false)
            {
                this.userControlFilterCondition1.Setup(null);
                return;
            }
            this.userControlFilterCondition1.UpdateMaps();
            if (mainProgramGUI != null && mainProgramGUI.mainCore != null)
            {
                this.checkBoxIntensityPerMap.Checked = mainProgramGUI.mainCore.Has_And_LocalNormalization;
                this.checkBoxIntensityGlobal.Checked = mainProgramGUI.mainCore.GlobalNormalization;
            }
        }

        /// <summary>
        /// Method responsible for filling datagridViewInputFiles
        /// </summary>
        private void AddNewRowDatagridInputFiles()
        {
            dataGridViewInputFiles.Rows.Add();

            foreach (DataGridViewRow row in dataGridViewInputFiles.Rows)
            {
                if (row.Index + 1 < 100)
                    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                else
                    row.HeaderCell.Value = String.Format("{00}", row.Index + 1);
                row.HeaderCell.Style.Font = new Font("Tahoma", 9.75F);
                if (dataGridViewInputFiles.Rows.Count < 100)
                    row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                else
                    row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                DataGridViewButtonCell btnCell = (DataGridViewButtonCell)row.Cells[5];
                btnCell.FlatStyle = FlatStyle.Standard;
                btnCell.Value = "Browse file";

                DataGridViewButtonCell btnCell2 = (DataGridViewButtonCell)row.Cells[7];
                btnCell2.FlatStyle = FlatStyle.Standard;
                btnCell2.Value = "Browse file";
            }
            if (dataGridViewInputFiles.Rows.Count < 100)
                dataGridViewInputFiles.RowHeadersWidth = 50;
            else
                dataGridViewInputFiles.RowHeadersWidth = 70;
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
            openFileDialog.Filter = "Protein file (*.txt)|*.txt|Fasta file (*.fasta)|*.fasta";
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
                #region Reset fields
                buttonDisplay.Enabled = false;
                this.userControlFilterCondition1.ResetMaps();
                this.userControlFilterCondition1.Enabled = false;
                groupBoxIntensityNorm.Enabled = false;
                buttonIntensity.Enabled = false;
                #endregion

                mainProgramGUI = new Program();
                mainProgramGUI.programParams = myParams;
                mainThread = new Thread(new ThreadStart(mainProgramGUI.ReadInputFiles));
            }

            if (buttonOK.Text.Equals("Next step") && mainProgramGUI != null)
            {
                mainThread.Start();
                timerStatus.Enabled = true;

                #region Disabling some fields
                textBoxProteinSeq.Enabled = false;
                textBoxSeqInfo.Enabled = false;
                dataGridViewInputFiles.Enabled = false;
                buttonBrowseProteinSequence.Enabled = false;
                buttonAddInputFile.Enabled = false;
                buttonDisplay.Enabled = false;
                this.userControlFilterCondition1.Enabled = false;
                groupBoxIntensityNorm.Enabled = false;
                buttonIntensity.Enabled = false;
                #endregion

                buttonOK.Enabled = true;
                buttonOK.Text = "Stop";
                //change button icon
                buttonOK.Image = TDFragMapper.Properties.Resources.button_cancel_little;
            }
            else if (mainProgramGUI != null)
            {
                DialogResult answer = System.Windows.Forms.MessageBox.Show("Are you sure you want to stop the process ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (answer == DialogResult.Yes)
                {
                    #region Enabling some fields
                    textBoxProteinSeq.Enabled = true;
                    textBoxSeqInfo.Enabled = true;
                    dataGridViewInputFiles.Enabled = true;
                    buttonBrowseProteinSequence.Enabled = true;
                    buttonAddInputFile.Enabled = true;
                    buttonDisplay.Enabled = false;
                    this.userControlFilterCondition1.Enabled = false;
                    groupBoxIntensityNorm.Enabled = false;
                    buttonIntensity.Enabled = false;
                    #endregion

                    buttonOK.Text = "Next step";
                    //change button icon
                    buttonOK.Image = TDFragMapper.Properties.Resources.goBtn;

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
                        "'Protein Sequence' field is empty. Please, select one file.",
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
            /// List<(MS/MS Data, Fragmentation Method, Activation Level, Precursor Charge State, Replicate, Intesity Data)>
            /// </summary>
            List<(string, string, string, int, int, string)> inputFileList = new List<(string, string, string, int, int, string)>();

            #region Read datagridviewInputfiles
            StringBuilder sbError = new StringBuilder();
            for (int iRow = 0; iRow < dataGridViewInputFiles.RowCount; iRow++)
            {
                DataGridViewRow GridRow = dataGridViewInputFiles.Rows[iRow];
                if ((GridRow.Cells[0].Value != null && !String.IsNullOrEmpty(GridRow.Cells[0].Value.ToString())) &&
                    (GridRow.Cells[1].Value != null && !String.IsNullOrEmpty(GridRow.Cells[1].Value.ToString())) &&
                    (GridRow.Cells[2].Value != null && !String.IsNullOrEmpty(GridRow.Cells[2].Value.ToString())) &&
                    (GridRow.Cells[3].Value != null && !String.IsNullOrEmpty(GridRow.Cells[3].Value.ToString())) &&
                    (GridRow.Cells[4].Value != null && !String.IsNullOrEmpty(GridRow.Cells[4].Value.ToString()))) /*&&*/
                                                                                                                  //(GridRow.Cells[6].Value != null && !String.IsNullOrEmpty(GridRow.Cells[6].Value.ToString())))
                {
                    int replicate = Convert.ToInt32(numberCaptured.Matches(GridRow.Cells[3].Value.ToString())[0].Value);
                    if (GridRow.Cells[0].Value.ToString().ToLower().Equals("etchd"))
                    {
                        if (GridRow.Cells[6].Value != null && !String.IsNullOrEmpty(GridRow.Cells[6].Value.ToString()))
                        {
                            inputFileList.Add((GridRow.Cells[4].Value.ToString(),
                                "EThcD",
                                GridRow.Cells[1].Value.ToString(),
                                Convert.ToInt32(GridRow.Cells[2].Value.ToString()),
                                Convert.ToInt32(replicate),
                                GridRow.Cells[6].Value.ToString()));
                            myParams.HasIntensities = true;
                        }
                        else
                        {
                            inputFileList.Add((GridRow.Cells[4].Value.ToString(),
                                "EThcD",
                                GridRow.Cells[1].Value.ToString(),
                                Convert.ToInt32(GridRow.Cells[2].Value.ToString()),
                                Convert.ToInt32(replicate),
                                string.Empty));
                            myParams.HasIntensities = false;
                        }

                    }
                    else
                    {
                        if (GridRow.Cells[6].Value != null && !String.IsNullOrEmpty(GridRow.Cells[6].Value.ToString()))
                        {
                            inputFileList.Add((GridRow.Cells[4].Value.ToString(),
                            GridRow.Cells[0].Value.ToString(),
                            GridRow.Cells[1].Value.ToString(),
                            Convert.ToInt32(GridRow.Cells[2].Value.ToString()),
                            Convert.ToInt32(replicate),
                            GridRow.Cells[6].Value.ToString()));
                            myParams.HasIntensities = true;
                        }
                        else
                        {
                            inputFileList.Add((GridRow.Cells[4].Value.ToString(),
                            GridRow.Cells[0].Value.ToString(),
                            GridRow.Cells[1].Value.ToString(),
                            Convert.ToInt32(GridRow.Cells[2].Value.ToString()),
                            Convert.ToInt32(replicate),
                            string.Empty));
                            myParams.HasIntensities = false;
                        }
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

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            if (mainProgramGUI != null && mainProgramGUI.FinishProcessing)
            {
                mainThread = null;
                timerStatus.Enabled = false;

                #region Enabling some fields
                textBoxProteinSeq.Enabled = true;
                textBoxSeqInfo.Enabled = true;
                dataGridViewInputFiles.Enabled = true;
                buttonBrowseProteinSequence.Enabled = true;
                buttonAddInputFile.Enabled = true;
                buttonDisplay.Enabled = true;
                this.userControlFilterCondition1.Enabled = true;
                if (mainProgramGUI.programParams.HasIntensities)
                {
                    groupBoxIntensityNorm.Enabled = true;
                    buttonIntensity.Enabled = true;
                }
                #endregion

                buttonOK.Enabled = true;
                buttonOK.Text = "Next step";
                //change button icon
                buttonOK.Image = TDFragMapper.Properties.Resources.goBtn;

                this.userControlFilterCondition1.Setup(mainProgramGUI.mainCore);
                tabControlMainWindow.SelectedIndex = 1;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.Filter = "TDFragMapper results (*.tdfm)|*.tdfm"; // Filter files by extension
            dlg.Title = "Load results";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    mainProgramGUI = new Program();
                    mainProgramGUI.mainCore = new Controller.Core();
                    mainProgramGUI.mainCore = mainProgramGUI.mainCore.DeserializeResults(dlg.FileName);
                    this.userControlFilterCondition1.Setup(mainProgramGUI.mainCore, false);
                    buttonDisplay_Click(null, null);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Failed to load file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mainProgramGUI = null;
                }
            }
        }

        private void readMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Coming soon!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //HelpForm help = new HelpForm();
            //help.ShowDialog();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            #region open automatically a tdfm file
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

            #region Load software version
            try
            {
                versionNumberLabel.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch (Exception exception)
            {
                //Unable to retrieve version number
                Console.WriteLine("", exception.Message);
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
                    dlg.Filter = "Excel File(*.xlsx)|*.xlsx|Excel File(*.xls)|*.xls|CSV File (*.csv)|*.csv";
                    dlg.Title = "Open input files";

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();
                    if (result == true)
                    {
                        if (e.ColumnIndex == 5)
                            dataGridViewInputFiles[4, e.RowIndex].Value = dlg.FileName;
                        else if (e.ColumnIndex == 7)
                            dataGridViewInputFiles[6, e.RowIndex].Value = dlg.FileName;
                    }
                }
            }
        }

        private void dataGridViewInputFiles_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(ColumnOnlyNumbers_KeyPress);
            e.Control.KeyPress -= new KeyPressEventHandler(ColumnActivationLevel_KeyPress);
            e.Control.KeyPress -= new KeyPressEventHandler(ColumnFragMeth_KeyPress);
            e.Control.KeyPress -= new KeyPressEventHandler(ColumnReplicate_KeyPress);

            if (dataGridViewInputFiles.CurrentCell.ColumnIndex == 2) //Precursor Charge State
            {
                TextBox tb = e.Control as TextBox;
                tb.CharacterCasing = CharacterCasing.Upper;

                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(ColumnOnlyNumbers_KeyPress);
                }
            }

            if (dataGridViewInputFiles.CurrentCell.ColumnIndex == 3) //Replicate cols
            {
                TextBox tb = e.Control as TextBox;
                tb.CharacterCasing = CharacterCasing.Upper;

                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(ColumnReplicate_KeyPress);
                }
            }

            if (dataGridViewInputFiles.CurrentCell.ColumnIndex == 1) //Activation Level col
            {
                TextBox tb = e.Control as TextBox;
                tb.CharacterCasing = CharacterCasing.Lower;
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

        private void ColumnReplicate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)
                || e.KeyChar == 82 /* 'R' */
                || e.KeyChar == 114 /* 'r' */
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
                || e.KeyChar == 47 /* '/' */
                || e.KeyChar == 37 /* '%' */
                || e.KeyChar == 109 /* 'm' */
                || e.KeyChar == 115 /* 's' */
                || e.KeyChar == 77 /* 'M' */
                || e.KeyChar == 83 /* 'S' */
                || e.KeyChar == 8 /* 'backspace' */
                || e.KeyChar == 32 /* 'space' */
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
                                if (i == 5)
                                    oCell = dataGridViewInputFiles[iCol + (i + 1), iRow];
                                else
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
                if (row.Index + 1 < 100)
                    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
                else
                    row.HeaderCell.Value = String.Format("{00}", row.Index + 1);
                row.HeaderCell.Style.Font = new Font("Tahoma", 9.75F);
                if (dataGridViewInputFiles.Rows.Count < 100)
                    row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                else
                    row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                DataGridViewButtonCell btnCell = (DataGridViewButtonCell)row.Cells[5];
                btnCell.FlatStyle = FlatStyle.Standard;
                btnCell.Value = "Browse file";
            }
            if (dataGridViewInputFiles.Rows.Count < 100)
                dataGridViewInputFiles.RowHeadersWidth = 50;
            else
                dataGridViewInputFiles.RowHeadersWidth = 70;
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
            if (mainProgramGUI.mainCore.DictMaps.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                        "There is no Map. Please, create one before displaying results.",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                return;
            }
            Results results = new Results();
            results.Setup(mainProgramGUI.mainCore, this);
            results.ShowDialog();
        }

        private void checkBoxIntensityPerMap_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxIntensityGlobal.Checked && checkBoxIntensityPerMap.Checked)
                checkBoxIntensityGlobal.Checked = false;

            if (mainProgramGUI != null && mainProgramGUI.mainCore != null)
            {
                mainProgramGUI.mainCore.Has_And_LocalNormalization = checkBoxIntensityPerMap.Checked;
                mainProgramGUI.mainCore.GlobalNormalization = false;
            }
        }

        private void checkBoxIntensityGlobal_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxIntensityPerMap.Checked && checkBoxIntensityGlobal.Checked)
                checkBoxIntensityPerMap.Checked = false;

            if (mainProgramGUI != null && mainProgramGUI.mainCore != null)
            {
                mainProgramGUI.mainCore.GlobalNormalization = checkBoxIntensityGlobal.Checked;
                if (checkBoxIntensityGlobal.Checked)
                    mainProgramGUI.mainCore.Has_And_LocalNormalization = true;
                else
                    mainProgramGUI.mainCore.Has_And_LocalNormalization = false;
            }
        }

        private void buttonIntensity_Click(object sender, EventArgs e)
        {
            buttonDisplay_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            About aboutScreen = new About();
            aboutScreen.ShowDialog();
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
                                   TDFragMapper.Properties.Resources.load.GetHbitmap(),
                                   IntPtr.Zero,
                                   Int32Rect.Empty,
                                   BitmapSizeOptions.FromEmptyOptions());

            _buttonImageHot = (Image)TDFragMapper.Properties.Resources.load;
            _buttonImageNormal = (Image)TDFragMapper.Properties.Resources.load;
            _buttonImageDisabled = (Image)TDFragMapper.Properties.Resources.load;
        }
    }
}
