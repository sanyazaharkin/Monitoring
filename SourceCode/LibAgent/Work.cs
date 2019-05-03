using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.Management;
using System.Configuration;
using System.Collections.Specialized;

namespace LibAgent
{


    public class Work
    {   
        public delegate void Message(string str);
        public static event Message DebugInfoSend;


        public  static bool enable;
        private static bool debug;
        private static int port;
        private static int timeout;
        private static IPAddress address;
        private static NetworkStream stream;

        public static void Main(object sAll)
        {
            NameValueCollection config = (NameValueCollection)sAll;

            enable = true;
            debug   = config["enable_log"].ToLower()=="yes"?true:false;
            address = IPAddress.Parse(config["server_ip"]);
            port    = int.Parse(config["server_port"]);
            timeout = int.Parse(config["timeout"]);
            
            

            BinaryFormatter formatter = new BinaryFormatter();
            while (enable)
            {
                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        if (ServerIsAvailible(address, port) & !client.Connected)
                        {
                            client.Connect(address, port);
                            stream = client.GetStream();
                        }

                        while (enable & client.Connected)
                        {   
                            SendMSG("Выполняется сбор метрик");
                            LibHost.Host host = GetHost();

                            SendMSG("Выполняется сериализация и отправка");
                            formatter.Serialize(stream, host);
                            SendMSG("Отправленно!!! Сон " + (double)(timeout / 1000) + "сек.");
                            System.Threading.Thread.Sleep(timeout);
                        }
                    }
                    System.Threading.Thread.Sleep(timeout);
                }

                catch (Exception ex)
                {
                    SendMSG(ex.Message);
                }

            }
        }

        public static LibHost.Host GetHost()
        {
            
            string hostname = Environment.MachineName;
            string os_version = Execute_WMI_Query("SELECT * FROM Win32_OperatingSystem", "Caption")[0];

            string bios_version = string.Empty;
            foreach (string item in Execute_WMI_QueryArray("SELECT * FROM Win32_BIOS", "BIOSVersion")[0])
            {
                bios_version += item + " ";
            }
                        
            int state = 0;

            List<LibHost.Device> Devices = GetDevices();
            List<LibHost.Process> Processes = GetProcesses();
            List<LibHost.Program> Programs = GetPrograms();



            return new LibHost.Host(hostname,os_version,bios_version, state,Devices,Processes,Programs );
        }

        private static List<LibHost.Device> GetDevices()  
        {
            List<LibHost.Device> result = new List<LibHost.Device>();

            foreach (LibHost.Devices.Device_MB  item in Search_MB())  {result.Add(item);}
            foreach (LibHost.Devices.Device_CPU item in Search_CPU()) {result.Add(item);}
            foreach (LibHost.Devices.Device_RAM item in Search_RAM()) {result.Add(item);}
            foreach (LibHost.Devices.Device_HDD item in Search_HDD()) {result.Add(item);}
            foreach (LibHost.Devices.Device_NET item in Search_NET()) {result.Add(item);}
            foreach (LibHost.Devices.Device_GPU item in Search_GPU()) {result.Add(item);}


            return result;
        }//наполенние списка устройств
        private static List<LibHost.Program> GetPrograms()
        {
            List<LibHost.Program> result = new List<LibHost.Program>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    string name     = (string)obj["Name"] ?? "-1";
                    string version  = (string)obj["Version"] ?? "-1";
                    string vendor   = (string)obj["Vendor"] ?? "-1";

                    result.Add(new LibHost.Program(name , version, vendor));
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }


            return result;
        }// наполнение списка программ
        private static List<LibHost.Process> GetProcesses()
        {
            List<LibHost.Process> result = new List<LibHost.Process>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Process");
            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    string name = (string)obj["Name"] ?? "-1";
                    result.Add(new LibHost.Process(name));
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }

            return result;
        }// наполнение списка процессов

        #region методы для поиска устройств
        private static List<LibHost.Devices.Device_MB> Search_MB()
        {
            List<LibHost.Devices.Device_MB> result = new List<LibHost.Devices.Device_MB>();
                       
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_BaseBoard");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get())
                {
                    string name         = (string)obj["Name"] ?? "-1";
                    string manufacturer = (string)obj["Manufacturer"] ?? "-1";
                    string model        = (string)obj["Model"] ?? "-1";
                    string product      = (string)obj["Product"] ?? "-1";
                    string serial_nomer = (string)obj["SerialNumber"] ?? "-1";

                    result.Add(new LibHost.Devices.Device_MB(manufacturer, model, name, product, serial_nomer));
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }

            return result;
        }
        private static List<LibHost.Devices.Device_CPU> Search_CPU()
        {
            List<LibHost.Devices.Device_CPU> result = new List<LibHost.Devices.Device_CPU>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get())
                {
                    string name         = (string)obj["Name"] ?? "-1";
                    string manufacturer = (string)obj["Manufacturer"] ?? "-1";                    
                    uint cores          = (uint)obj["NumberOfCores"];
                    uint clock_speed    = (uint)obj["MaxClockSpeed"];

                    result.Add(new LibHost.Devices.Device_CPU(manufacturer,name, (int)cores, (int)clock_speed));
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }

            return result;
        }
        private static List<LibHost.Devices.Device_RAM> Search_RAM()
        {
            List<LibHost.Devices.Device_RAM> result = new List<LibHost.Devices.Device_RAM>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM win32_PhysicalMemory");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get())
                {
                    
                    string manufacturer = (string)obj["Manufacturer"] ?? "-1";
                    uint clock_speed = obj["Speed"] != null ? (uint)obj["Speed"]:0;
                    UInt16 memory_type = obj["MemoryType"] != null ? (UInt16)obj["MemoryType"] : (ushort)0;
                    UInt16 form_factor = obj["FormFactor"] != null ? (UInt16)obj["FormFactor"] : (ushort)0; 
                    string DeviceLocator = (string)obj["DeviceLocator"] ?? "-1";
                    ulong size = (ulong)obj["Capacity"];


                    result.Add(new LibHost.Devices.Device_RAM(manufacturer, (int)clock_speed,  (int)memory_type, (int)form_factor, size, DeviceLocator));
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
                SendMSG(ex.Source.ToString());
            }


            return result;
        }
        private static List<LibHost.Devices.Device_HDD> Search_HDD()
        {
            List<LibHost.Devices.Device_HDD> result = new List<LibHost.Devices.Device_HDD>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk WHERE DriveType=3");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get())
                {
                    string caption = (string)obj["Caption"] ?? "-1";
                    string description = (string)obj["Description"] ?? "-1";
                    ulong size = (ulong)obj["Size"];
                    ulong free_space = (ulong)obj["FreeSpace"];
                    string file_system = (string)obj["FileSystem"] ?? "-1"; ;



                    result.Add(new LibHost.Devices.Device_HDD(description,caption,size,free_space,file_system));
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }

            return result;
        }
        private static List<LibHost.Devices.Device_NET> Search_NET()
        {
            List<LibHost.Devices.Device_NET> result = new List<LibHost.Devices.Device_NET>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPenabled = true");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get())
                {
                    string macaddress = (string)obj["macaddress"] ?? "-1";
                    string description = (string)obj["description"] ?? "-1";

                    List<IPAddress> ipaddress = new List<IPAddress>();
                    foreach (string ip in ((string[])obj["ipaddress"] ?? new string[1]{"0.0.0.0"}))
                    {
                        ipaddress.Add(IPAddress.Parse(ip));
                    }

                    List<IPAddress> DefaultIPGateway = new List<IPAddress>();
                    foreach (string ip in ((string[])obj["DefaultIPGateway"] ?? new string[1] { "0.0.0.0" }))
                    {
                        DefaultIPGateway.Add(IPAddress.Parse(ip));
                    }

                    result.Add(new LibHost.Devices.Device_NET(macaddress,description,DefaultIPGateway,ipaddress));
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }

            return result;
        }
        private static List<LibHost.Devices.Device_GPU> Search_GPU()
        {
            List<LibHost.Devices.Device_GPU> result = new List<LibHost.Devices.Device_GPU>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get())
                {
                    string name = (string)obj["Name"] ?? "-1";
                    uint memory_size = (uint)obj["AdapterRAM"];

                    result.Add(new LibHost.Devices.Device_GPU(name, memory_size));
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }

            return result;
        }
        #endregion


        private static List<string> Execute_WMI_Query(string query, string ClassItemAdd)
        {
            List<string> result = new List<string>();
            string temp;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
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
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
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

        private static bool ServerIsAvailible(IPAddress ip, int port)
        {
            bool result;
            try
            {
                TcpClient newClient = new TcpClient();
                IPEndPoint endPoint = new IPEndPoint(ip, port);
                newClient.Connect(endPoint);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                SendMSG(ex.Message);                
            }

            return result;
        }


        public static void SendMSG(string str)
        {
            if (debug & DebugInfoSend != null) DebugInfoSend(str);
        }
    }
}
