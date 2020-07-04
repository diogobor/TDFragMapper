using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace TDFragMapper.Utils
{
    public class ListBoxStreamWriter : TextWriter
    {
        ListBox output = null;
        Mutex bufferAccess = new Mutex();

        public ListBoxStreamWriter(ListBox output)
        {
            this.output = output;
        }

        public override void Write(string value)
        {
            base.Write(value);
            try
            {
                bufferAccess.WaitOne();
                if (value.Contains("%") ||
                    value.Contains("spectra skipped (0 isotopic envelopes)") ||
                    value.Contains("Plot time"))
                    output.Invoke(new Action(() => output.Items.RemoveAt(output.Items.Count - 1)));
                output.Invoke(new Action(() => output.Items.Add(value.ToString())));// When character data is written, append it to the text box.
                output.Invoke(new Action(() => output.SelectedIndex = output.Items.Count - 1));
            }
            catch (Exception) { }
            finally
            {
                bufferAccess.ReleaseMutex();
            }
        }

        public override void WriteLine(string value)
        {
            base.Write(value);
            try
            {
                bufferAccess.WaitOne();
                output.Invoke(new Action(() => output.Items.Add(value.ToString())));// When character data is written, append it to the text box.

                if (String.IsNullOrEmpty(value) ||
                    value.Contains("%") ||
                    value.Contains("lines") ||
                    value.Contains("} not found in") || //Warning when a residue modification is inserted (homodimer analyses)
                    value.Contains("~ not found in") || //Warning when a residue modification is inserted (homodimer analyses)
                    value.Contains("Processing :") ||   //Comet string
                    value.Contains("Setting current") ||//Comet string
                    value.Contains("Post processing") ||//Comet string
                    value.Contains("Temp/comet") ||     //Comet string
                    value.Contains("Scan ") ||          //Quantitation
                    value.Contains("parsed in") ||            //FastaReaser
                    value.Contains("spectra skipped (0 isotopic envelopes)") || //Determine precursor charge state
                    value.Contains("Plot time"))//SpectrumViewer2
                    output.Invoke(new Action(() => output.Items.RemoveAt(output.Items.Count - 1)));
                output.Invoke(new Action(() => output.SelectedIndex = output.Items.Count - 1));
            }
            catch (Exception) { }
            finally
            {
                bufferAccess.ReleaseMutex();
            }
        }

        public string GetString
        {
            get
            {
                string retVal = "";
                try
                {
                    bufferAccess.WaitOne();
                    retVal = output.Items[output.Items.Count - 1].ToString();
                    output.Items.Clear();
                }
                catch (Exception)
                {

                }
                finally
                {
                    bufferAccess.ReleaseMutex();
                }
                return retVal;
            }
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
