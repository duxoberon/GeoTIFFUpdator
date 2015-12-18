using System;
using System.Diagnostics;

namespace GeoTiffCoordRef.Classes
{
    public class RunBatchThread
    {

        #region Fields

        private ThreadManager _threadmanager;
        private string _batchfile;
        private string _threadID;
        private string _imagenamewithoutext;
        private string _workingdirectory;
        
        #endregion

        #region Constructor

        public RunBatchThread(ThreadManager threadmanager, string threadID, string imagenamewithoutext, string batchfile, string workingdirectory)
        {
            _threadmanager = threadmanager;
            _batchfile = batchfile;
            _threadID = threadID;
            _imagenamewithoutext = imagenamewithoutext;
            _workingdirectory = workingdirectory;
        }

        #endregion

        #region Methods

        public void Start()
        {
            try
            {

                //Process proc = new Process();
                //proc.StartInfo.FileName = _batchfile;
                //proc.StartInfo.WorkingDirectory = _workingdirectory;
                //proc.EnableRaisingEvents = true;
                //proc.StartInfo.UseShellExecute = true;
                //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //proc.StartInfo.CreateNoWindow = true;
                //proc.StartInfo.RedirectStandardOutput = true;
                //proc.OutputDataReceived += new DataReceivedEventHandler(proc_OutputDataReceived);
                //proc.StartInfo.RedirectStandardError = true;
                //proc.ErrorDataReceived += new DataReceivedEventHandler(proc_ErrorDataReceived);
                //proc.Start();
                //proc.WaitForExit();  // blocks until finished
                //_threadmanager.ThreadFinishCallback(_threadID, _imagenamewithoutext);

                ProcessStartInfo procStartInfo = new ProcessStartInfo(System.IO.Path.Combine(_workingdirectory, _batchfile));
                procStartInfo.WorkingDirectory = _workingdirectory;
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.RedirectStandardError = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                Process proc = new Process();
                proc.EnableRaisingEvents = true;
                proc.StartInfo = procStartInfo;
                proc.OutputDataReceived += new DataReceivedEventHandler(proc_OutputDataReceived);
                proc.ErrorDataReceived += new DataReceivedEventHandler(proc_ErrorDataReceived);
                proc.Exited += new EventHandler(proc_Exited);
                proc.Start();
                proc.BeginOutputReadLine();
                proc.WaitForExit();  // blocks until finished

            }
            catch (Exception ex)
            {
                _threadmanager.ThreadErrorCallback(_threadID, _imagenamewithoutext, ex.Message);
            }
        }

        void proc_Exited(object sender, EventArgs e)
        {
            _threadmanager.ThreadFinishCallback(_threadID, _imagenamewithoutext);
        }

        void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            _threadmanager.ThreadErrorCallback(_threadID, _imagenamewithoutext, e.Data);
        }

        void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _threadmanager.ThreadOutputCallback(_threadID, _imagenamewithoutext, e.Data);
        }

        #endregion

    }
}
