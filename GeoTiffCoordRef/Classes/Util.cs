using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoTiffCoordRef.Classes
{
    public class Util
    {

        public static string APPFOLDER = "SmartGTiff";
        public static string TIFFEXT = "*.tif";
        public static string TIFFEXT2 = ".tif";

        internal static string GDAL = "gdal";
        internal static string DATA = "data";
        internal static string OSGEO4W = "gdal" + Path.DirectorySeparatorChar + "OSGeo4W.bat";
        internal static string GDAL_TRANSLATE = "gdal" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "gdal_translate.exe";
        internal static string LISTGEO = "gdal" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "listgeo.exe";
        internal static string GEOTIFCP = "gdal" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "geotifcp.exe";
        internal static string GEOMETADATA = "GeoTiffMetadata.exe";

        private static class SETTINGSTAG
        {
            public const string WATCHFOLDER = "inputfolder";
            public const string SPATIALREF = "spatialref";
            public const string PROCESSINGTHREADS = "processingthreads";
        }

        protected string userFolder = string.Empty;

        protected string watchFolder = string.Empty;
        protected string spatialreference = string.Empty;
        protected decimal processingthreads = 3;
        private const string settings = ".settings";
        private string logfile = string.Empty;

        public string SPATIALREFERENCE
        {
            set { spatialreference = value; }
            get { return spatialreference; }
        }
        public string WATCHFOLDER
        {
            set { watchFolder = value; }
            get { return watchFolder; }
        }
        public decimal PROCESSINGTHREADS
        {
            set { processingthreads = value; }
            get { return processingthreads; }
        }

        public Util()
        {
            userFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APPFOLDER);
            if (!Directory.Exists(userFolder)) Directory.CreateDirectory(userFolder);
            GetFolderPath();
        }

        internal void SaveSettings()
        {
            setSetting(settings, watchFolder, SETTINGSTAG.WATCHFOLDER);
            setSetting(settings, processingthreads.ToString(), SETTINGSTAG.PROCESSINGTHREADS);
            setSetting(settings, spatialreference, SETTINGSTAG.SPATIALREF);
        }

        protected void GetFolderPath()
        {
            watchFolder = getSetting(settings, SETTINGSTAG.WATCHFOLDER);
            spatialreference = getSetting(settings, SETTINGSTAG.SPATIALREF);

            string s = getSetting(settings, SETTINGSTAG.PROCESSINGTHREADS);
            if (s.Length > 0)
                decimal.TryParse(s, out processingthreads);
        }

        private string getSetting(string filename, string tag)
        {
            string filepath = System.IO.Path.Combine(userFolder, filename);
            if (File.Exists(filepath))
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] split = line.Split(';');
                        if (split.Length > 1)
                        {
                            if (string.Compare(split[0], tag, true) == 0)
                                return split[1];
                        }
                    }
                }
            }
            return string.Empty;
        }

        private void setSetting(string filename, string setting, string tag)
        {
            string tempfilepath = System.IO.Path.Combine(userFolder, "x_" + filename);
            string filepath = System.IO.Path.Combine(userFolder, filename);
            using (StreamWriter sw = new StreamWriter(tempfilepath, false))
            {
                bool found = false;
                if (System.IO.File.Exists(filepath))
                {
                    using (StreamReader sr = new StreamReader(filepath))
                    {
                        string line = string.Empty;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] split = line.Split(';');
                            if (split.Length > 1)
                            {
                                if (string.Compare(split[0], tag, true) == 0)
                                {
                                    sw.WriteLine(tag + ";" + setting);
                                    found = true;
                                }
                                else
                                    sw.WriteLine(line);
                            }
                        }
                    }
                }
                if (!found)
                    sw.WriteLine(tag + ";" + setting);
            }
            System.IO.File.Copy(tempfilepath, filepath, true);
        }

    }
}
