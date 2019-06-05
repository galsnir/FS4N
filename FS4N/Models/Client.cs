using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace FS4N.Models
{
    public class Client
    {
        TcpClient client;
        BinaryWriter writer;
        BinaryReader reader;
        private TcpListener listener;
        public bool isConnected = false;

        #region Singleton
        private static Client m_Instance = null;
        public static Client Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Client();
                }
                return m_Instance;
            }
        }
        #endregion

        // This method reset the singelton value to be null
        public void Clear()
        {
            m_Instance = null;
        }

        // This method connects the program to the simulator server
        public void Connect(string ip, int port)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            client = new TcpClient();

            // We try to connect again and again util the connection is made
            while (!client.Connected)
            {
                try { client.Connect(ep); }
                catch (Exception) { }
            }

            Console.WriteLine("You are connected");
            isConnected = true;
            writer = new BinaryWriter(client.GetStream());
            reader = new BinaryReader(client.GetStream());
        }

        // This method sends a get requset to the simulator and return it's result
        public string getInfo(string command)
        {
            if (string.IsNullOrEmpty(command)) return "0";
            string buffer = command + "\r\n";
            writer.Write(Encoding.ASCII.GetBytes(buffer));

            char c;
            string line = "";
            while ((c = reader.ReadChar()) != '\n') line += c;
            return Parse(line);
        }

        // This method parse the return string from the simulator' extract the relevent value and returns it
        public string Parse(string rawString)
        {
            string parsedString = "";
            string[] values = rawString.Split('\'');
            parsedString = values[1];
            return parsedString;
        }
    }
}