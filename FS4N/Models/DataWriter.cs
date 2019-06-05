using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FS4N.Models
{
    public class DataWriter
    {
        private static DataWriter s_instace = null;

        public static DataWriter Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new DataWriter();
                }
                return s_instace;
            }
        }

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        public void WriteData(PointEntry point, string file)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, file));
            if (!File.Exists(path))
            {
                using (StreamWriter newFile = File.CreateText(path))
                {
                    newFile.WriteLine(point.lat);
                    newFile.WriteLine(point.lon);
                    newFile.WriteLine(point.rudder);
                    newFile.WriteLine(point.throttle);
                }
            }
            else
            {
                using (StreamWriter oldFile = File.AppendText(path))
                {
                    oldFile.WriteLine(point.lat);
                    oldFile.WriteLine(point.lon);
                    oldFile.WriteLine(point.rudder);
                    oldFile.WriteLine(point.throttle);
                }
            }
        }
    }
}