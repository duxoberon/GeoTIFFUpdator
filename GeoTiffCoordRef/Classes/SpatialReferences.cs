using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeoTiffCoordRef.Classes
{
    internal class SpatialReferences
    {

        internal static string DATAFOLDER = "data";
        internal static string COORDFILE = "data\\pcs_gcs.wcoord";
        internal static string PCSFILE = "gdal\\share\\gdal\\pcs.csv";
        internal static string GCSFILE = "gdal\\share\\gdal\\gcs.csv";

        static internal List<string> GetSpatialReferenceStrings(out List<string> epsgcodes)
        {

            List<string> spatialrefs = new List<string>();
            epsgcodes = new List<string>();

            string executingassembly = System.Reflection.Assembly.GetEntryAssembly().Location;
            string applicationfolder = Path.GetDirectoryName(executingassembly);

            string spatialreferencefile = Path.Combine(applicationfolder, COORDFILE);

            if (File.Exists(spatialreferencefile))
            {
                using (StreamReader sr = new StreamReader(spatialreferencefile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] split = line.Split(',');
                        spatialrefs.Add(split[1] + " [" + split[0] + "]");
                        epsgcodes.Add(split[0]);
                    }
                }
            }

            //Get All prjs
            int fakeepsgcode = 7000001;
            string[] prjfiles = Directory.GetFiles(Path.Combine(applicationfolder, DATAFOLDER), "*.prj");
            foreach (string prj in prjfiles)
            {
                spatialrefs.Add(Path.GetFileName(prj) + " [w" + fakeepsgcode.ToString() + "]");
                epsgcodes.Add("w" + fakeepsgcode.ToString());
                fakeepsgcode++;
            }


            return spatialrefs;
        }

        static internal string GetSpatialReferenceCode(string spatialreferencestring)
        {
            string[] split = spatialreferencestring.Split('[');
            if (split.Length == 2)
                return split[1].Replace("]", "");
            else return string.Empty;
        }

        static internal string GetSpatialReferenceString(string spatialreferencecode, List<string> spatialreferences)
        {
            foreach (string sref in spatialreferences)
            {
                string code = GetSpatialReferenceCode(sref);
                if (string.Compare(code, spatialreferencecode, true) == 0)
                    return sref;
            }
            return string.Empty;
        }

        static internal string GetDetails(string spatialreferencecode)
        {
            string executingassembly = System.Reflection.Assembly.GetEntryAssembly().Location;
            string applicationfolder = Path.GetDirectoryName(executingassembly);

            string spatialreferencefile = Path.Combine(applicationfolder, PCSFILE);

            StringBuilder sb = new StringBuilder();
            if (File.Exists(spatialreferencefile))
            {
                using (StreamReader sr = new StreamReader(spatialreferencefile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] split = line.Split(',');
                        if (string.Compare(spatialreferencecode, split[0]) == 0)
                        {
                            switch (split[2])
                            {
                                case "9001":
                                    sb.AppendLine("UNITS: Meters");
                                    break;
                                case "9002":
                                    sb.AppendLine("UNITS: International Feet");
                                    break;
                                case "9003":
                                    sb.AppendLine("UNITS: US Feet");
                                    break;
                            }
                            sb.AppendLine("DATUM: " + GetGeog(split[3]));

                            return sb.ToString();
                        }
                    }
                }
            }

            sb.AppendLine(GetGeog(spatialreferencecode));
            return sb.ToString();
        }

        static private string GetGeog(string spatialreferencecode)
        {

            string executingassembly = System.Reflection.Assembly.GetEntryAssembly().Location;
            string applicationfolder = Path.GetDirectoryName(executingassembly);

            string spatialreferencefile = Path.Combine(applicationfolder, GCSFILE);

            StringBuilder sb = new StringBuilder();
            if (File.Exists(spatialreferencefile))
            {
                using (StreamReader sr = new StreamReader(spatialreferencefile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] split = line.Split(',');
                        if (string.Compare(spatialreferencecode, split[0]) == 0)
                        {
                            sb.AppendLine(split[1]);
                        }
                    }
                }
            }
            return sb.ToString();
        }

    }
}
