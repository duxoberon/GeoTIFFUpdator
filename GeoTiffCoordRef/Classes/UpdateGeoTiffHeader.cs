using System.IO;
using System.Text;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace GeoTiffCoordRef.Classes
{
    public class UpdateGeoTiffHeader
    {

        #region Fields

        private ThreadManager _threadmanager;
        private string _imagespath;
        private string _workingdirectory;
        private string _spatialreference;
        private string _applicationfolder;
        string[] _imagefiles = null;
        private List<string> _spatialreferences;
        private bool _updatemetadatatags = true;

        #endregion

        #region Constructor

        public UpdateGeoTiffHeader(ThreadManager threadManager, string[] imagefiles, string imagespath,
         string spatialreference, List<string> spatialreferences, bool updatemetadatatags, string workingdirectory, 
            string applicationfolder)
        {
            this._threadmanager = threadManager;
            this._imagefiles = imagefiles;
            this._imagespath = imagespath;
            this._workingdirectory = workingdirectory;
            this._applicationfolder = applicationfolder;
            this._spatialreference = spatialreference;
            this._spatialreferences = spatialreferences;
            this._updatemetadatatags = updatemetadatatags;
        }

        #endregion

        #region Method

        public void Start()
        {

            string osgeo4w = Path.Combine(_applicationfolder, Util.GDAL);
            osgeo4w = PathHelper.GetShortPathName(osgeo4w);
            string osgeo4wbat = Path.Combine(_applicationfolder, Util.OSGEO4W);
            string osgeo4wtext = File.ReadAllText(osgeo4wbat);
            string gdaltranslate = Path.Combine(_applicationfolder, Util.GDAL_TRANSLATE);
            string listgeo = Path.Combine(_applicationfolder, Util.LISTGEO);
            string geotifcp = Path.Combine(_applicationfolder, Util.GEOTIFCP);
            string geotiffmetadata = Path.Combine(_applicationfolder, Util.GEOMETADATA);

            bool custom = false;

            if (this._spatialreference.IndexOf('w') > -1)
            {
                string prjfilename = SpatialReferences.GetSpatialReferenceString(_spatialreference, _spatialreferences);
                if (prjfilename.IndexOf("[w") > -1)
                {
                    prjfilename = prjfilename.Substring(0, prjfilename.IndexOf("[w"));
                    prjfilename = prjfilename.Trim();
                }
                _spatialreference = Path.Combine(_applicationfolder, Util.DATA);
                _spatialreference = Path.Combine(_spatialreference, prjfilename);
                custom = true;
            }

            foreach (string imagefile in this._imagefiles)
            {
                Guid g = Guid.NewGuid();
                string tempfile = g.ToString().Substring(26);
                string tifftempfile = Path.Combine(_imagespath, tempfile + Util.TIFFEXT2);
                string metadatafile = Path.Combine(_imagespath, tempfile);
                string imagenamewithoutext = Path.GetFileNameWithoutExtension(imagefile);
                string imagename = Path.GetFileName(imagefile);

                string batchfilepath = Path.Combine(_workingdirectory, imagenamewithoutext + ".bat");
                StringBuilder batchfileentries = new StringBuilder();

                //Add OSGeo4W
                batchfileentries.AppendLine("set OSGEO4W_ROOT=" + osgeo4w + Path.DirectorySeparatorChar);
                batchfileentries.Append(osgeo4wtext);
                batchfileentries.AppendLine();
                if (custom)
                    batchfileentries.AppendLine("\"" + gdaltranslate + "\" -a_srs \"" + _spatialreference + "\" \"" + imagefile + "\" \"" + tifftempfile + "\"");
                else
                    batchfileentries.AppendLine("\"" + gdaltranslate + "\" -a_srs EPSG:" + _spatialreference + " \"" + imagefile + "\" \"" + tifftempfile + "\"");

                if (_updatemetadatatags)
                {
                    batchfileentries.AppendLine("\"" + listgeo + "\" \"" + tifftempfile + "\" >> \"" + metadatafile + "\"");
                    batchfileentries.AppendLine("\"" + geotiffmetadata + "\" \"" + metadatafile + "\"");
                    batchfileentries.AppendLine("del \"" + tifftempfile + "\"");
                    batchfileentries.AppendLine("ren \"" + imagefile + "\" " + tempfile + Util.TIFFEXT2);
                    batchfileentries.AppendLine("\"" + geotifcp + "\" -g \"" + metadatafile + "\" \"" + tifftempfile + "\" \"" + imagefile + "\"");
                    batchfileentries.AppendLine("del \"" + metadatafile + "\"");
                    batchfileentries.AppendLine("del \"" + tifftempfile + "\"");
                    batchfileentries.AppendLine("exit");
                }
                else
                {
                    batchfileentries.AppendLine("del \"" + imagefile + "\"");
                    batchfileentries.AppendLine("ren \"" + tifftempfile + "\" " + imagename);
                    batchfileentries.AppendLine("exit");
                }

                //batchfileentries.AppendLine("del \"" + imagefile + "\"");
                //batchfileentries.AppendLine("ren \"" + tifftempfile + "\" " + imagename);
                //batchfileentries.AppendLine("exit");

                //Write batch file
                if (File.Exists(batchfilepath))
                    File.Delete(batchfilepath);
                File.WriteAllText(batchfilepath, batchfileentries.ToString());

                _threadmanager.AddBatchFileToQueue(imagenamewithoutext);

            }

        }

        #endregion

    }

    public class PathHelper
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(
            [MarshalAs(UnmanagedType.LPTStr)] string path,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath,
            int shortPathLength);

        public static string GetShortPathName(string path)
        {
            StringBuilder shortPath = new StringBuilder(500);
            if (0 == GetShortPathName(path, shortPath, shortPath.Capacity))
            {
                if (Marshal.GetLastWin32Error() == 2)
                {
                    throw new Exception("Since the file/folder doesn't exist yet, the system can't generate short name.");
                }
                else
                {
                    throw new Exception("GetShortPathName Win32 error is " + Marshal.GetLastWin32Error());
                }
            }
            return shortPath.ToString();
        }
    }

}
