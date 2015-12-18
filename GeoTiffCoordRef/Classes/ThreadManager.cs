using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using GeoTiffCoordRef.Forms;
using System.Windows.Forms;

namespace GeoTiffCoordRef.Classes
{
    public class ThreadManager
    {

        #region Fields

        private Dictionary<string, string> _filestoprocess = new Dictionary<string, string>();
        private Queue<KeyValuePair<string, string>> _batchfilestoprocess = new Queue<KeyValuePair<string, string>>();

        private Object _controllock = new Object();
        private GeoTiffCoordRef_Form _frmgeotiffcoordref;
        private string _imagespath;
        private string _spatialreference;
        private int _numberofprocesses;
        private string _workingdirectory;
        private int _threadcount = 0;
        private bool _running = false;
        private System.Timers.Timer _timer;
        private System.Timers.Timer _timer2;
        private bool _forcestop = false;
        private string _logfile = string.Empty;
        private string _applicationfolder = string.Empty;
        private List<string> _spatialreferences;
        private bool _updatemetadatatags = true;
        private Dictionary<string, DateTime> _filesbeingprocessed;
        private DateTime _starttime;
        private string[] _imagefiles;
        private bool? _stuckswitch = false;
        private int _stuckilesbeingprocessed = 0;

        #endregion

        #region Properties

        public bool IsRunning
        {
            get
            {
                if (_threadcount == 0)
                {
                    this._timer.Enabled = false;
                    _running = false;
                }
                return _running;
            }
        }

        public int NumberOfProcess
        {
            set { _numberofprocesses = value; }
        }

        #endregion

        #region Constructor

        public ThreadManager(GeoTiffCoordRef_Form frmgeotiffcoordref, string imagespath,
            string spatialreference, List<string> spatialreferences, int numberofprocesses,
            bool updatemetadatatags, string applicationfolder, string workingdirectory, string logfile)
        {
            _frmgeotiffcoordref = frmgeotiffcoordref;
            _imagespath = imagespath;
            _spatialreference = spatialreference;
            _numberofprocesses = numberofprocesses;
            _workingdirectory = workingdirectory;
            _logfile = logfile;
            _applicationfolder = applicationfolder;
            _spatialreferences = spatialreferences;
            _updatemetadatatags = updatemetadatatags;
        }

        #endregion

        #region Methods

        public void Start()
        {
            try
            {
                Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), "Updating Tiffs in directory " + _imagespath + " started.");
                
                _running = true;
                _forcestop = false;
                _frmgeotiffcoordref.ClearDataGrid();
                _filesbeingprocessed = new Dictionary<string, DateTime>();
                _starttime = DateTime.Now;
                bool fileslocked = false;

                _imagefiles = GetUnlockedFiles(_imagespath, Util.TIFFEXT, out fileslocked);
                if (fileslocked)
                {
                    DialogResult results = MessageBox.Show("Some files are locked and cannot be updated.  These files could cause the application to hang.\nThe log file located in the folder being processed contains a list of locked files.\nDo you wish to proceed?", "GeoTiff Coordinate Reference", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (results == DialogResult.No)
                    {
                        _running = false;
                        _frmgeotiffcoordref.EnableControls();
                        return;
                    }
                }

                foreach (string imagefile in _imagefiles)
                {
                    string ext = Util.TIFFEXT.Replace("*", string.Empty);
                    string imagenamewithoutext = Path.GetFileNameWithoutExtension(imagefile);
                    Guid id = Guid.NewGuid();
                    _filestoprocess.Add(imagenamewithoutext, id.ToString());
                    _frmgeotiffcoordref.AddNewFileStatus(id.ToString(), imagenamewithoutext);
                }

                UpdateGeoTiffHeader updategeotiffheader = new UpdateGeoTiffHeader(this, _imagefiles, _imagespath, _spatialreference, _spatialreferences, _updatemetadatatags, _workingdirectory, _applicationfolder);
                Thread thread = new Thread(updategeotiffheader.Start);
                thread.Start();

                _timer = new System.Timers.Timer();
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
                _timer.Interval = 4000;
                _timer.Enabled = true;

                _timer2 = new System.Timers.Timer();
                _timer2.Elapsed += new System.Timers.ElapsedEventHandler(_timer2_Elapsed);
                _timer2.Interval = 300000;
                _timer2.Enabled = true;

            }
            catch (Exception ex)
            {
                Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), ex.ToString());
            }
        }

        private string[] GetUnlockedFiles(string path, string extension, out bool fileslocked)
        {
            string[] imagefiles = Directory.GetFiles(path, extension);
            List<string> newimamgefilelist = new List<string>();
            fileslocked = false;
            foreach (string imagefile in imagefiles)
            {
                if (!IsFileLocked(path, imagefile))
                    newimamgefilelist.Add(imagefile);
                else fileslocked = true;
            }

            return newimamgefilelist.ToArray();
        }

        private bool IsFileLocked(string path, string imagefile)
        {
            bool islocked = false;

            FileStream stream = null;
            try
            {
                FileInfo fi = new FileInfo(Path.Combine(path, imagefile));
                stream = fi.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                islocked = true;
                Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), imagefile + " is locked and could not be updated.");
            }
            finally
            {
                if (stream != null) stream.Close();
            }

            return islocked;
        }

        private void _timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_threadcount == 0)
            {
                this._timer2.Enabled = false;
                _stuckswitch = null;
                _stuckilesbeingprocessed = 0;
                return;
            }

            if (_threadcount > 0)
            {
                if (_filesbeingprocessed.Count == _stuckilesbeingprocessed)
                {
                    if (_stuckswitch == null) _stuckswitch = false;
                    if (_stuckswitch == false) _stuckswitch = true;
                }
                else
                {
                    _stuckswitch = null;
                    _stuckilesbeingprocessed = _filesbeingprocessed.Count;
                }
            }

            if (_stuckswitch == true)
            {
                DialogResult result = MessageBox.Show("Oh dear, it seems I am stuck! I will attempt to get myself unstuck.\nDo you wish me to attempt to get unstuck?", "GeoTiff Coordinate Reference", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {

                    List<String> filenamelist = new List<string>();
                    lock (_controllock)
                        foreach (KeyValuePair<string, DateTime> kvp in _filesbeingprocessed) filenamelist.Add(kvp.Key);

                    foreach (string fileName in filenamelist)
                    {
                        DateTime dt;
                        if (_filesbeingprocessed.TryGetValue(fileName, out dt))
                        {
                            RemoveFile(fileName);
                            TimeSpan ts = DateTime.Now - dt;
                            Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), fileName + ": Completed in " + ts.ToString() + " (HH:mm:ss)");
                            
                            if (_threadcount == 0) _running = false;
                            this.DecreaseThreadCount();
                            if (_threadcount == 0 && _filesbeingprocessed.Count == 0 && _batchfilestoprocess.Count == 0)
                            {
                                ts = DateTime.Now - _starttime;
                                Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), "");
                                Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), "All processes completed in " + ts.ToString() + " (HH:mm:ss)");
                            }
                        }
                    }
                
                }
                else
                {
                    _stuckswitch = null;
                    _stuckilesbeingprocessed = 0;
                }
            }

        }

        public void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (_forcestop)
            {
                if (_threadcount == 0)
                {
                    this._timer.Enabled = false;
                    _running = false;
                }
                return;
            }

            while ((_batchfilestoprocess.Count > 0) && (_threadcount < _numberofprocesses))
            {
                KeyValuePair<string, string> batchtorun = _batchfilestoprocess.Dequeue();
                _frmgeotiffcoordref.UpdateProcessingStatus(batchtorun.Key, "Running...");

                string batchfile = Path.Combine(_workingdirectory, batchtorun.Value + ".bat");
                if (File.Exists(batchfile))
                {
                    if (IsFileLocked(_imagespath, batchtorun.Value + Util.TIFFEXT2))
                        _frmgeotiffcoordref.UpdateProcessingStatus(batchtorun.Key, "File is locked...");
                    else
                    {
                        RunBatchThread runbatchthread = new RunBatchThread(this, batchtorun.Key, batchtorun.Value, batchtorun.Value + ".bat", _workingdirectory);
                        Thread thread2 = new Thread(runbatchthread.Start);
                        this.IncreaseThreadCount();
                        thread2.Start();
                    }
                }
            }

            if (_threadcount == 0)
            {
                this._timer.Enabled = false;
                _running = false;
                _frmgeotiffcoordref.EnableControls();
            }

        }

        public void AddBatchFileToQueue(string imagenamewithoutext)
        {
            string id = string.Empty;
            _filestoprocess.TryGetValue(imagenamewithoutext, out id);
            _frmgeotiffcoordref.UpdateProcessingStatus(id, "Ready");
            KeyValuePair<string, string> batchfile = new KeyValuePair<string, string>(id, imagenamewithoutext);
            _batchfilestoprocess.Enqueue(batchfile);
        }

        public void RunListGeo(string imagenamewithoutext)
        {
            if (!IsFileLocked(_imagespath, imagenamewithoutext + Util.TIFFEXT2))
            {
                RunBatchThread runbatchthread = new RunBatchThread(this, "", imagenamewithoutext, imagenamewithoutext + ".bat", _workingdirectory);
                Thread thread2 = new Thread(runbatchthread.Start);
                thread2.Start();
            }
        }

        public void StopRunning()
        {
            _forcestop = true;
        }

        public void IncreaseThreadCount()
        {
            lock (_controllock)
                _threadcount++;
        }

        public void DecreaseThreadCount()
        {
            lock (_controllock)
            {
                if (_threadcount > 0)
                    _threadcount--;
            }
        }

        public void RemoveFile(string filename)
        {
            lock (_controllock)
                _filesbeingprocessed.Remove(filename);
        }

        public void AddFile(string filename, DateTime dt)
        {
            lock (_controllock)
                _filesbeingprocessed.Add(filename, dt);
        }

        public void ThreadFinishCallback(string threadID, string fileName)
        {
            _frmgeotiffcoordref.UpdateProcessingStatus(threadID, "Finished");
            //if (_threadcount == 0) _running = false;
            //this.DecreaseThreadCount();
            //if (_threadcount == 0 && _filesbeingprocessed.Count == 0 && _batchfilestoprocess.Count == 0)
            //{
            //    TimeSpan ts = DateTime.Now - _starttime;
            //    Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), "");
            //    Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), "All processes completed in " + ts.ToString() + " (HH:mm:ss)");
            //}
        }

        public void ThreadOutputCallback(string threadID, string fileName, string outputMessage)
        {
            if (outputMessage != null)
            {
                if (outputMessage.Contains("Input file size is"))
                {
                    AddFile(fileName, DateTime.Now);
                    //Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), fileName + ": Started");
                }
                if (outputMessage.Contains(">exit"))
                {
                    if (_filesbeingprocessed.ContainsKey(fileName))
                    {
                        RunListGeo runlistgeo = new RunListGeo(this, fileName, _imagespath, _workingdirectory, _applicationfolder);
                        runlistgeo.Start();
                    }
                }
                if (outputMessage.Contains("Lower Right"))
                {
                    DateTime dt;
                    if (_filesbeingprocessed.TryGetValue(fileName, out dt))
                    {
                        RemoveFile(fileName);
                        TimeSpan ts = DateTime.Now - dt;
                        Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), fileName + ": Completed in " + ts.ToString() + " (HH:mm:ss)");

                        if (_threadcount == 0) _running = false;
                        this.DecreaseThreadCount();
                        if (_threadcount == 0 && _filesbeingprocessed.Count == 0 && _batchfilestoprocess.Count == 0)
                        {
                            ts = DateTime.Now - _starttime;
                            Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), "");
                            Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), "All processes completed in " + ts.ToString() + " (HH:mm:ss)");
                        }
                    }
                }
                if (outputMessage.Contains("... unable"))
                {
                    if (_filesbeingprocessed.ContainsKey(fileName))
                        RemoveFile(fileName);
                    Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), fileName + " !Error!:  Reported a problem");
                }
            }
        }

        public void ThreadErrorCallback(string threadID, string fileName, string errorMessage)
        {
            if (errorMessage != null)
            {
                Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), fileName + " !Error!: " + errorMessage);
                _frmgeotiffcoordref.UpdateProcessingStatus(threadID, "Failed - check log");
                this.DecreaseThreadCount();
            }
        }

        #endregion

        #region Destructor

        ~ThreadManager()
        {
            if (_filesbeingprocessed.Count > 0)
            {
                foreach (KeyValuePair<string, DateTime> kvp in _filesbeingprocessed)
                {
                    if (kvp.Key.Length > 0)
                        Logger.WriteLogLine(Path.Combine(_imagespath, _logfile), kvp.Key + ": !Started but not completed!");
                }
            }
        }

        #endregion

    }
}
