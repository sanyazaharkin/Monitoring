using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.Management;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace LibAgent
{


    public class Work
    {
        public delegate void Message(string str);
        public static event Message MSG;

        public static bool eneble = true;
        public static int timeout = 10000;


        public static void Main(string[] Args)
        {
            const int port = 8888;
            const string address = "127.0.0.1";
            TcpClient client = new TcpClient(address, port);
            NetworkStream stream = client.GetStream();

            BinaryFormatter formatter = new BinaryFormatter();

            while (eneble)
            {
                formatter.Serialize(stream, GetHost());
                System.Threading.Thread.Sleep(timeout);
            }
        }

        public static LibHost.Host GetHost()
        {
            
            string hostname = Environment.MachineName;
            string os_version = Execute_WMI_Query("SELECT Caption FROM Win32_OperatingSystem", "Caption")[0];

            string bios_version = string.Empty;
            foreach (string item in Execute_WMI_QueryArray("SELECT BIOSVersion FROM Win32_BIOS", "BIOSVersion")[0])
            {
                bios_version += item + " ";
            }

            
            int state = 0;
            List<LibHost.IDevice> Devices = GetDevices();
            List<LibHost.Process> Processes = GetProcesses();
            List<LibHost.Program> Programs = GetPrograms();



            return new LibHost.Host(hostname,os_version,bios_version, state,Devices,Processes,Programs );
        }

        private static List<LibHost.IDevice> GetDevices()
        {
            return new List<LibHost.IDevice>();
        }
        private static List<LibHost.Program> GetPrograms()
        {
            return new List<LibHost.Program>();
        }

        private static List<LibHost.Process> GetProcesses()
        {
            return new List<LibHost.Process>();
        }


        private static List<string> Execute_WMI_Query(string query, string ClassItemAdd)
        {
            List<string> result = new List<string>();
            string temp;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    temp = obj[ClassItemAdd].ToString().Trim();
                    result.Add(temp ?? "-1");
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }
            return result;
        }

        private static List<string[]> Execute_WMI_QueryArray(string query, string ClassItemAdd)
        {
            List<string[]> result = new List<string[]>();
            string[] temp;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    temp = (string[])obj[ClassItemAdd];
                    result.Add(temp ?? new string[1]{"-1"} );
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }
            return result;
        }


        public static void SendMSG(string str)
        {
            if (MSG != null) MSG(str);
        }
    }
}
