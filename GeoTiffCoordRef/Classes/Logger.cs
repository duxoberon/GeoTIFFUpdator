using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoTiffCoordRef.Classes
{
    public static class Logger
    {

        private static readonly object sync = new object();

        /// <summary>
        /// Writes log message to log file instance.
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="message"></param>
        public static void WriteLogLine(string logPath, string message)
        {
            try
            {
                lock (sync)
                {
                    using (StreamWriter writer = File.AppendText(logPath))
                    {
                        string fullMessage = DateTime.Now.ToString() + " - " + message;
                        writer.WriteLine(fullMessage);
                        writer.Flush();
                        writer.Close();
                    }
                }

            }
            catch (Exception)
            {
            }

        }

    }
}
