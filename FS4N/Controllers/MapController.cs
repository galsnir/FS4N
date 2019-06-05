using FS4N.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace FS4N.Controllers
{
    public class MapController : Controller
    {
        public static readonly string LON_DEG = "/position/longitude-deg";
        public static readonly string LAT_DEG = "/position/latitude-deg";
        public static readonly string RUDDER_PATH = "/controls/flight/rudder";
        public static readonly string THROTTLE_PATH = "/controls/engines/engine/throttle";


        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult display(string ip, int port)
        {
            IPAddress ipTemp;
            /* We check if the ip paramerter is an ip, if it is an ip we run 
            the display else we run the displaySavedFlight*/
            if (IPAddress.TryParse(ip,out ipTemp) == false)
            {
                displaySavedFlight(ip, port);
                return View("displaySavedFlight");
            }

            Client.Instance.Connect(ip,port);
            ViewBag.lon = Client.Instance.getInfo("get " + LON_DEG);
            ViewBag.lat = Client.Instance.getInfo("get " + LAT_DEG);
            return View();
        }

        public ActionResult displayTime(string ip, int port,int time)
        {
            Client.Instance.Connect(ip, port);
            Session["time"] = time;
            return View();
        }

        // The method creates a point and returns it
        public KeyValuePair<string,string> GetNewPoint()
        {
            string lon = Client.Instance.getInfo("get " + LON_DEG);
            string lat = Client.Instance.getInfo("get " + LAT_DEG);
            return new KeyValuePair<string, string>(lon, lat);
        }

        // This method creates Xml with a point and returns it in a string form
        public string ToXml()
        {
            //Initiate XML stuff
            KeyValuePair<string, string> point = GetNewPoint();
            string lon = point.Key;
            string lat = point.Value;
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Point");
            writer.WriteElementString("lon",lon);
            writer.WriteElementString("lat", lat);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }


        public ActionResult displayAndSave(string ip, int port, int frequency, int duration, string file)
        {
            Client.Instance.Connect(ip, port);
            Session["frequency"] = frequency;
            Session["duration"] = duration;
            Session["fileName"] = file;
            return View();
        }

        // This method creates Xml with a point write it to  a file and returns it in a string form
        public string ToXmlSave()
        {
            // getting all needed values from FlightGear
            string lon = Client.Instance.getInfo("get " + LON_DEG);
            string lat = Client.Instance.getInfo("get " + LAT_DEG);
            string rudder = Client.Instance.getInfo("get " + RUDDER_PATH);
            string throttle = Client.Instance.getInfo("get " + THROTTLE_PATH);

            // creating object pointEntry to send to function that saves data to file
            PointEntry pointEntry = new PointEntry(lat, lon,
                rudder, throttle);
            // calling function that writes data of specific point to file
            Models.DataWriter.Instance.WriteData(pointEntry,(string)Session["fileName"]);

            // after saving point to xml file, send it to view
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Point");
            writer.WriteElementString("lon", lon);
            writer.WriteElementString("lat", lat);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }


        public ActionResult displaySavedFlight(string file, int frequency)
        {
            DataReader.Instance.LoadFile(file);
            Session["frequency"] = frequency;
            return View();
        }

        // This method creates Xml read a point from the file and returns it in a string form
        public string ToXmlSavedFlight()
        {
            // reading data point from object that 
            PointEntry nextPoint = DataReader.Instance.ReadData();

            if (nextPoint == null)
                return null;

            // after saving point to xml file, send it to view
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Point");
            writer.WriteElementString("lon", nextPoint.lon);
            writer.WriteElementString("lat", nextPoint.lat);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }



    }
}