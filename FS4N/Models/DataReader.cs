using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FS4N.Models
{
    public class DataReader
    {
        private static DataReader s_instace = null;
        List<PointEntry> pointEntries;

        public static DataReader Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new DataReader();
                }
                return s_instace;
            }
        }

        public DataReader()
        {
            pointEntries = new List<PointEntry>();
        }

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        public void LoadFile(string name)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, name));
            if (!File.Exists(path))
            {
                return;
            }
            else
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string lat = "";
                    string lon;
                    string rudder;
                    string throttle;
                    while ((lat = sr.ReadLine()) != null)
                    {
                        lon = sr.ReadLine();
                        rudder = sr.ReadLine();
                        throttle = sr.ReadLine();
                        pointEntries.Add(new PointEntry(lat, lon, rudder, throttle));
                    }
                }
            }
        }


        public PointEntry ReadData()
        {
            PointEntry result = null;
            if (pointEntries.Any())
            {
                result = pointEntries[0];
                pointEntries.RemoveAt(0);
            }

            return result;
        }
    }
}