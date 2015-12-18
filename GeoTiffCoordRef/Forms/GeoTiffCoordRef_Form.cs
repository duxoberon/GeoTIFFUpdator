using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GeoTiffCoordRef.Classes;
using System.Threading;

namespace GeoTiffCoordRef.Forms
{
    public partial class GeoTiffCoordRef_Form : Form
    {

        #region Fields

        private const string LockFileExtension = ".sgtif";

        private string _logFilename = String.Empty;
        private string _machinename = String.Empty;
        private Util _util;

        private ThreadManager _threadmanager;
        private List<string> _spatialreferences;
        private string _tempid = string.Empty;

        #endregion

        #region Constructor

        public GeoTiffCoordRef_Form()
        {
            InitializeComponent();

            _machinename = System.Environment.MachineName;

            DateTime startTime = DateTime.Now;

            _logFilename = "_GTIFFHeader_" + _machinename + "-" + startTime.Month.ToString() + "-" + startTime.Date.Day.ToString() + "-" + startTime.Year.ToString() + ".log";

            _util = new Util();

        }

        #endregion

        #region Events

        private void GeoTiffCoordRef_Form_Load(object sender, EventArgs e)
        {
            this.tbxTiffFolder.Text = _util.WATCHFOLDER;

            List<string> epsgcodes;
            _spatialreferences = SpatialReferences.GetSpatialReferenceStrings(out epsgcodes);
            this.cboProjection.Items.AddRange(_spatialreferences.ToArray());
            this.cboEPSG.Items.AddRange(epsgcodes.ToArray());
            
            if (this.cboProjection.Items.Count > 0)
                this.cboProjection.SelectedIndex = 0;

            int index = this.cboProjection.Items.IndexOf(SpatialReferences.GetSpatialReferenceString(_util.SPATIALREFERENCE, _spatialreferences));
            if (index > -1)
                this.cboProjection.SelectedIndex = index;
            
            this.numOfProcesses.Value = _util.PROCESSINGTHREADS;
            Guid g = Guid.NewGuid();
            _tempid = g.ToString();
        }

        private void tbxTiffFolder_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string s in files)
                {
                    if (System.IO.Directory.Exists(s))
                    {
                        this.tbxTiffFolder.Text = s;
                        break;
                    }
                    if (File.Exists(s))
                    {
                        this.tbxTiffFolder.Text = Path.GetDirectoryName(s);
                        break;
                    }
                }
            }
        }

        private void tbxTiffFolder_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string s in files)
                {
                    if (System.IO.Directory.Exists(s))
                    {
                        this.tbxTiffFolder.Text = s;
                        break;
                    }
                    if (File.Exists(s))
                    {
                        this.tbxTiffFolder.Text = Path.GetDirectoryName(s);
                        break;  
                    }
                }
            }
        }

        private void cboProjection_SelectedIndexChanged(object sender, EventArgs e)
        {
            string epsg = SpatialReferences.GetSpatialReferenceCode(this.cboProjection.Text);
            int index = this.cboEPSG.Items.IndexOf(epsg);
            if (index > -1)
            {
                this.cboEPSG.SelectedIndex = index;
                this.toolTip1.ToolTipTitle = this.cboProjection.Text;
                string description = SpatialReferences.GetDetails(epsg);
                this.toolTip1.SetToolTip(this.btnHoverDam, description);
            }
        }

        private void cboEPSG_TextUpdate(object sender, EventArgs e)
        {
            string epsg = this.cboEPSG.Text;
            if (epsg.Length >= 4)
            {
                string spatialreference = SpatialReferences.GetSpatialReferenceString(epsg, _spatialreferences);
                if (spatialreference.Length > 0)
                {
                    int index = this.cboProjection.Items.IndexOf(spatialreference);
                    if (index > -1)
                        this.cboProjection.SelectedIndex = index;
                }
            }
        }

        private void cboEPSG_SelectedIndexChanged(object sender, EventArgs e)
        {
            string epsg = this.cboEPSG.Text;
            string spatialreference = SpatialReferences.GetSpatialReferenceString(epsg, _spatialreferences);
            if (spatialreference.Length == 0)
            {
                MessageBox.Show(this.cboEPSG.Text + " cannot be found in the EPSG database.", "GeoTiff Header Update Tool", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int index = this.cboProjection.Items.IndexOf(spatialreference);
                if (index > -1)
                    this.cboProjection.SelectedIndex = index;
            }
        }

        private void cboEPSG_Validating(object sender, CancelEventArgs e)
        {
            string epsg = this.cboEPSG.Text;
            if (epsg.Length >= 4)
            {
                string spatialreference = SpatialReferences.GetSpatialReferenceString(epsg, _spatialreferences);
                if (spatialreference.Length == 0)
                {
                    MessageBox.Show(this.cboEPSG.Text + " cannot be found in the EPSG database.", "GeoTiff Header Update Tool", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
                else
                {
                    int index = this.cboProjection.Items.IndexOf(spatialreference);
                    if (index > -1)
                        this.cboProjection.SelectedIndex = index;
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            DialogResult dialogresult = MessageBox.Show(this, "Are you sure you want to set the projection of the TIFF files to \n" + this.cboProjection.Text + "?\nThis action cannot be undone.", "GeoTiff Coordinate Reference", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogresult == DialogResult.No) return;


            FileInfo executingassembly = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
            string applicationfolder = executingassembly.DirectoryName;
            
            try
            {

                string imagespath = this.tbxTiffFolder.Text;
                if (!Directory.Exists(imagespath)) return;
                _util.WATCHFOLDER = imagespath;
                string log = Path.Combine(imagespath, _logFilename);
                if (File.Exists(log)) File.Delete(log);

                int numberofprocesses = (int)this.numOfProcesses.Value;
                _util.PROCESSINGTHREADS = this.numOfProcesses.Value;
                bool updatemetadatatags = this.chkUpdateMetadataTags.Checked;

                _util.SPATIALREFERENCE = SpatialReferences.GetSpatialReferenceCode(this.cboProjection.Text);

                string workingdirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Util.APPFOLDER);
                workingdirectory = System.IO.Path.Combine(workingdirectory, _tempid);
                if (!Directory.Exists(workingdirectory)) Directory.CreateDirectory(workingdirectory);
                else RecursiveDelete(workingdirectory);

                DisableControls();
                _threadmanager = new ThreadManager(this, _util.WATCHFOLDER, _util.SPATIALREFERENCE, _spatialreferences, numberofprocesses, updatemetadatatags, applicationfolder,
                    workingdirectory, _logFilename);
                Thread thread = new Thread(_threadmanager.Start);
                _threadmanager.Start();

            }
            catch (Exception ex)
            {
                EnableControls();
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _threadmanager.StopRunning();
            EnableControls();
            this.btnStop.Enabled = false;
            this.lblNetworkOffline.Visible = false;

            // Set the status value for any files in the queue to canceled
            for (int i = 0; i < dgvStatus.Rows.Count; i++)
            {
                if (dgvStatus.Rows[i].Cells[2].Value.ToString() == "Queued" || dgvStatus.Rows[i].Cells[2].Value.ToString() == "Ready")
                {
                    dgvStatus.Rows[i].Cells[2].Value = "Canceled";
                }
            }

            //Logger.WriteLogLine(Path.Combine(_util.WATCHFOLDER, _logFilename), "Ended by user");
        
        }

        private void btnTiffFolder_Click(object sender, EventArgs e)
        {
            if (this.tbxTiffFolder.Text.Length > 0)
            {
                if (Directory.Exists(this.tbxTiffFolder.Text))
                    this.folderBrowserDialog1.SelectedPath = this.tbxTiffFolder.Text;
            }
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.tbxTiffFolder.Text = this.folderBrowserDialog1.SelectedPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not find folder from disk. Original error: " + ex.Message);
                }
            }
        }

        private void numOfProcesses_ValueChanged(object sender, EventArgs e)
        {
            int numberofprocesses = (int)this.numOfProcesses.Value;
            if (_threadmanager != null)
                _threadmanager.NumberOfProcess = numberofprocesses;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GeoTiffCoordRef_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_threadmanager != null)
            {
                try
                {
                    _threadmanager.StopRunning(); ;
                    _util.SaveSettings();
                }
                catch (Exception) { }
            }
            try
            {
                string workingdirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Util.APPFOLDER);
                workingdirectory = System.IO.Path.Combine(workingdirectory, _tempid);
                if (Directory.Exists(workingdirectory))
                    System.IO.Directory.Delete(workingdirectory, true);
            }
            catch (Exception) { }
        }

        #endregion

        #region Methods

        private delegate void EnableControlsDelegate();
        public void EnableControls()
        {
            if (this.tbxTiffFolder.InvokeRequired)
                this.tbxTiffFolder.Invoke(new EnableControlsDelegate(this.EnableControls));
            else if (this.btnTiffFolder.InvokeRequired)
                this.btnTiffFolder.Invoke(new EnableControlsDelegate(this.EnableControls));
            else if(this.cboProjection.InvokeRequired)
                this.cboProjection.Invoke(new EnableControlsDelegate(this.EnableControls));
            else if(this.btnStart.InvokeRequired)
                this.btnStart.Invoke(new EnableControlsDelegate(this.EnableControls));
            else if(this.btnStop.InvokeRequired)
                this.btnStop.Invoke(new EnableControlsDelegate(this.EnableControls));
            else
            {
                this.tbxTiffFolder.Enabled = true;
                this.btnTiffFolder.Enabled = true;
                this.cboProjection.Enabled = true;
                this.btnStart.Enabled = true;
                this.btnStop.Enabled = false;
                this.cboEPSG.Enabled = true;
                this.btnHoverDam.Enabled = true;
            }
        }

        private void DisableControls()
        {
            this.tbxTiffFolder.Enabled = false;
            this.btnTiffFolder.Enabled = false;
            this.cboProjection.Enabled = false;
            //this.numOfProcesses.Enabled = false;
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            this.cboEPSG.Enabled = false;
            this.btnHoverDam.Enabled = false;
        }

        private delegate void AddNewFileStatusDelegate(string threadID, string ID);
        public void AddNewFileStatus(string threadID, string fileName)
        {
            if (this.dgvStatus.InvokeRequired)
                this.dgvStatus.Invoke(new AddNewFileStatusDelegate(this.AddNewFileStatus), threadID, fileName);
            else
            {
                DateTime timeStamp = DateTime.Now;
                dgvStatus.Rows.Add(threadID, fileName, "Queued", timeStamp.ToShortTimeString());
            }
        }

        private delegate void UpdateProcessingStatusDelegate(string threadID, string statusMessage);
        public void UpdateProcessingStatus(string threadID, string statusMessage)
        {

            if (this.dgvStatus.InvokeRequired)
            {
                this.dgvStatus.Invoke(new UpdateProcessingStatusDelegate(this.UpdateProcessingStatus), threadID, statusMessage);
            }
            else
            {
                DateTime timeStamp = DateTime.Now;

                for (int i = 0; i < dgvStatus.Rows.Count; i++)
                {
                    if (dgvStatus.Rows[i].Cells[0].Value.ToString() == threadID)
                    {
                        dgvStatus.Rows[i].Cells[2].Value = statusMessage;
                        dgvStatus.Rows[i].Cells[3].Value = timeStamp.ToShortTimeString();
                    }
                }
            }
        }

        private delegate void ClearDataGridDelegate();
        public void ClearDataGrid()
        {
            if (this.dgvStatus.InvokeRequired)
            {
                this.dgvStatus.Invoke(new ClearDataGridDelegate(this.ClearDataGrid));
            }
            else
            {
                this.dgvStatus.Rows.Clear();
            }
        }

        private bool RecursiveDelete(string dir)
        {
            bool status = false;
            try
            {
                string[] dirs = Directory.GetDirectories(dir);
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                    File.Delete(file);
                foreach (string sub in dirs)
                    RecursiveDelete(sub);
                status = true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return status;
        }

        //private bool RecursiveDelete(string dir, string ext)
        //{
        //    bool status = false;
        //    try
        //    {
        //        string[] dirs = Directory.GetFiles(dir);
        //        string[] files = Directory.GetFiles(dir, ext);
        //        foreach (string file in files)
        //            File.Delete(file);
        //        foreach (string sub in dirs)
        //            RecursiveDelete(sub);
        //        status = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //    }
        //    return status;
        //}

        #endregion


    }
}
