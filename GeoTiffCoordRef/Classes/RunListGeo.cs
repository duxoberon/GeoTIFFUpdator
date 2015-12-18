using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoTiffCoordRef.Classes
{
    public class RunListGeo
    {

        #region Fields

        private ThreadManager _threadmanager;
        private string _imagespath;
        private string _workingdirectory;
        private string _applicationfolder;
        private string _imagefile = null;

        #endregion

        #region Constructor

        public RunListGeo(ThreadManager threadManager, string imagefile, string imagespath, string workingdirectory, string applicationfolder)
        {
            this._threadmanager = threadManager;
            this._imagefile = imagefile;
            this._imagespath = imagespath;
            this._workingdirectory = workingdirectory;
            this._applicationfolder = applicationfolder;
        }

        #endregion

        #region Methods

        public void Start()
        {
            string osgeo4w = Path.Combine(_applicationfolder, Util.GDAL);
            osgeo4w = PathHelper.GetShortPathName(osgeo4w);
            string osgeo4wbat = Path.Combine(_applicationfolder, Util.OSGEO4W);
            string osgeo4wtext = File.ReadAllText(osgeo4wbat);
            string listgeo = Path.Combine(_applicationfolder, Util.LISTGEO);
            string imagefilename = System.IO.Path.Combine(_imagespath, _imagefile + Util.TIFFEXT2);

            string imagenamewithoutext = Path.GetFileNameWithoutExtension(_imagefile);

            string batchfilepath = Path.Combine(_workingdirectory, imagenamewithoutext + ".bat");
            StringBuilder batchfileentries = new StringBuilder();

            //Add OSGeo4W
            batchfileentries.AppendLine("set OSGEO4W_ROOT=" + osgeo4w + Path.DirectorySeparatorChar);
            batchfileentries.Append(osgeo4wtext);
            batchfileentries.AppendLine();

            batchfileentries.AppendLine("\"" + listgeo + "\" \"" + imagefilename + "\"");
            batchfileentries.AppendLine("exit");

            //Write batch file
            if (File.Exists(batchfilepath))
                File.Delete(batchfilepath);
            File.WriteAllText(batchfilepath, batchfileentries.ToString());

            _threadmanager.RunListGeo(imagenamewithoutext);
        }

        #endregion

    }
}
