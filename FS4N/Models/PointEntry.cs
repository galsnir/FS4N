using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace FS4N.Models
{
    public class PointEntry
    {
        public PointEntry(string iLat, string iLon, string iRudder, string iThrottle)
        {
            lat = iLat;
            lon = iLon;
            rudder = iRudder;
            throttle = iThrottle;
        }

        public string lat { get; set; }
        public string lon { get; set; }
        public string rudder { get; set; }
        public string throttle { get; set; }


        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("PointEntry");
            writer.WriteElementString("Lat", this.lat);
            writer.WriteElementString("Lon", this.lon);
            writer.WriteElementString("Rudder", this.rudder);
            writer.WriteElementString("Throttle", this.throttle);
            writer.WriteEndElement();
        }

    }
}