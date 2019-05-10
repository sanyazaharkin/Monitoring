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
        public delegate void Message(string str); //объявляем делегат кторый будет использоваться для вывода дебаг информации в консоль или лог (куда угодно)
        public static event Message DebugInfoSend; // событие которое будет вызываться при необходимости вывода информации


        public  static bool enable; //объявление переменных с настроиками
        private static bool debug; 
        private static int port;
        private static int timeout;
        private static IPAddress address;

        private static NetworkStream stream;

        public static void Main(object sAll) //главный метод программы, именно он вызывается службой или консолью
        {
            NameValueCollection config = (NameValueCollection)sAll;  //даункастим настройки которые были переданы аргументом

            enable = true;  //взводим флаг, который сигнализирует о том, что программа запущена

            // читаем настройки из коллекции переданной аргументом к методу
            debug = config["enable_log"].ToLower()=="yes"?true:false; // здесь использован тернарный оператор
            address = IPAddress.Parse(config["server_ip"]);
            port    = int.Parse(config["server_port"]);
            timeout = int.Parse(config["timeout"]);
            
            

            BinaryFormatter formatter = new BinaryFormatter(); //создаем экземпляр обьекта кторый будет выполнять сериализацию
            while (enable) //запускаем главный цикл программы, он будет выполняться пока enable==true
            {
                try  //ловим исключения в куске кода между {}
                {
                    using (TcpClient client = new TcpClient()) //используем новй объект клиента
                    {
                        if (ServerIsAvailible(address, port) & !client.Connected) // если сервер доступен и клиент не подключен
                        {
                            client.Connect(address, port); //коннектимся на необходимый адрес и порт
                            stream = client.GetStream(); //получаем поток куда будем писать информацию
                        }

                        while (enable & client.Connected) //и пока программа включена и соединение активно
                        {   
                            SendMSG("Выполняется сериализация и отправка");
                            formatter.Serialize(stream, GetHost()); //получаем новый объект (GetHost()) класса HOST, сериализуем его в поток байт и пишем в поток stream
                            SendMSG("Отправленно!!! Сон " + (double)(timeout / 1000) + "сек.");
                            System.Threading.Thread.Sleep(timeout); //ожидаем некоторое заданное время
                        }
                    }
                    System.Threading.Thread.Sleep(timeout);//ожидаем некоторое заданное время
                }
                catch (Exception ex) //ловим исключение, если оно есть 
                {
                    SendMSG(ex.Message); //генерируем событие
                }
                //возвращаемся к началу цикла
            }
        }

        public static LibHost.Host GetHost()  //метод генерирующий объекты класса Host
        {
            
            string hostname = Environment.MachineName;  //получаем имя компьютера
            string os_version = Execute_WMI_Query("SELECT * FROM Win32_OperatingSystem", "Caption")[0]; //получаем версию ОС

            string bios_version = string.Empty; //строковая переменная для хранения информации о БИОС
            foreach (string item in Execute_WMI_QueryArray("SELECT * FROM Win32_BIOS", "BIOSVersion")[0])//выдергиваем информацию о БИОС
            {
                bios_version += item + " "; // и добавляем все к переменной через пробел
            }
                        
            int state = 0; //устанавливаем состояние хоста

            List<LibHost.Device> Devices = GetDevices(); //наполняем список устройств
            List<LibHost.Process> Processes = GetProcesses();//наполняем список процессов
            List<LibHost.Program> Programs = GetPrograms();//наполняем список программ



            return new LibHost.Host(hostname,os_version,bios_version, state,Devices,Processes,Programs ); //возращаем новый объект класса Host которому в конструктор передана вся полученная информация
        }

        private static List<LibHost.Device> GetDevices()  //наполенние списка устройств
        {
            List<LibHost.Device> result = new List<LibHost.Device>(); //создаем пустой односвязный список объектов класса Device

            foreach (LibHost.Devices.Device_MB item in Search_MB()) { result.Add(item); } // получаем список материнсских плат и поштучно добавляем в список устройств
            foreach (LibHost.Devices.Device_CPU item in Search_CPU()) {result.Add(item); }// получаем список процессоров и поштучно добавляем в список устройств
            foreach (LibHost.Devices.Device_RAM item in Search_RAM()) {result.Add(item); }// получаем список планок памяти и поштучно добавляем в список устройств
            foreach (LibHost.Devices.Device_HDD item in Search_HDD()) {result.Add(item); }// получаем список логических дисков и поштучно добавляем в список устройств
            foreach (LibHost.Devices.Device_NET item in Search_NET()) {result.Add(item); }// получаем список сетевых адаптеров и поштучно добавляем в список устройств
            foreach (LibHost.Devices.Device_GPU item in Search_GPU()) {result.Add(item); }// получаем список видеокарт и поштучно добавляем в список устройств


            return result; //возвращаем наполненный список устройств
        }
        private static List<LibHost.Program> GetPrograms() // наполнение списка программ из реестра Windows
        {
            List<LibHost.Program> result = new List<LibHost.Program>(); //создаем пустой список устройств (односвязный список)

            try //ловим исключения
            {
                string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registry_key))
                {
                    foreach (string subkey_name in key.GetSubKeyNames())
                    {
                        using (Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                            string name = (string)subkey.GetValue("DisplayName");// выдергиваем имя программы
                            string version = (string)subkey.GetValue("DisplayVersion"); //выдергиваем версию
                            string vendor = (string)subkey.GetValue("Publisher"); // выдергиваем производителя

                            if ((name != null) & (version != null) & (vendor != null)) // если ни одно из полей неравно NULL 
                            {
                                result.Add(new LibHost.Program(name, version, vendor));// то добавляем в список новый объект программы
                            }
                        }
                    }
                }

                if (Environment.Is64BitOperatingSystem) //если система 64-бит то читаем еще одну ветку реестра
                {
                    registry_key = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
                    using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registry_key))
                    {
                        foreach (string subkey_name in key.GetSubKeyNames())
                        {
                            using (Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(subkey_name))
                            {
                                string name = (string)subkey.GetValue("DisplayName"); // выдергиваем имя программы
                                string version = (string)subkey.GetValue("DisplayVersion");//выдергиваем версию
                                string vendor = (string)subkey.GetValue("Publisher");// выдергиваем производителя

                                if ((name != null) & (version != null) & (vendor != null)) // если ни одно из полей неравно NULL 
                                {
                                    result.Add(new LibHost.Program(name, version, vendor)); // то добавляем в список новый объект программы
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex) //обрабатываем исключения
            {
                SendMSG(ex.Message); //генерируем сообщение
            }


            return result; //возвращаем наполненный список программ
        }
        private static List<LibHost.Process> GetProcesses()// наполнение списка процессов
        {
            List<LibHost.Process> result = new List<LibHost.Process>(); // создаем пустой список процессов

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Process"); //создаем объект который будет выполнять указанный запрос WMI
            try
            {
                foreach (ManagementObject obj in searcher.Get())//получаем список процессов
                {
                    string name = (string)obj["Name"] ?? "-1"; //проверка на NULL
                    result.Add(new LibHost.Process(name)); //поочереди пишем в список процессов
                }
            }
            catch (Exception ex) //если есть ошибки
            {
                SendMSG(ex.Message); //генерируем сообщение
            }

            return result; // возвращаем наполенный список
        }

        #region методы для поиска устройств
        private static List<LibHost.Devices.Device_MB> Search_MB() //метод для обнаружения матринок
        {
            List<LibHost.Devices.Device_MB> result = new List<LibHost.Devices.Device_MB>(); //пустой список материнок
                        
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_BaseBoard");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get()) //выполняем запрос и получаем список обектов, затем из каждого объекта получаем необходимую информацию
                {
                    string name         = (string)obj["Name"] ?? "-1"; //приводим каждое поле к типу string и проверяем на NULL, если поле NULL то присваиваем значение "-1"
                    string manufacturer = (string)obj["Manufacturer"] ?? "-1";
                    string model        = (string)obj["Model"] ?? "-1";
                    string product      = (string)obj["Product"] ?? "-1";
                    string serial_nomer = (string)obj["SerialNumber"] ?? "-1";

                    result.Add(new LibHost.Devices.Device_MB(manufacturer, model, name, product, serial_nomer)); // добавляем в список новый объект материнки, полученные параметры передаем как аргументы в конструктор
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }

            return result; //возвращаем наполенный список
        }  
        private static List<LibHost.Devices.Device_CPU> Search_CPU() //метод для обнаружения процессоров
        {
            List<LibHost.Devices.Device_CPU> result = new List<LibHost.Devices.Device_CPU>(); //пустой список процессоров

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get()) //выполняем запрос и получаем список обектов, затем из каждого объекта получаем необходимую информацию
                {
                    string name         = (string)obj["Name"] ?? "-1"; //приводим каждое поле к типу string и проверяем на NULL, если поле NULL то присваиваем значение "-1"
                    string manufacturer = (string)obj["Manufacturer"] ?? "-1";                    
                    uint cores          = (uint)obj["NumberOfCores"];//здесь downcast к типу беззнаковое целое
                    uint clock_speed    = (uint)obj["MaxClockSpeed"];

                    result.Add(new LibHost.Devices.Device_CPU(manufacturer,name, (int)cores, (int)clock_speed));// пишем каждый в список
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }

            return result; //возвращаем наполненный список процессоров
        }
        private static List<LibHost.Devices.Device_RAM> Search_RAM() //метод для обнаружения модулей памяти
        {
            List<LibHost.Devices.Device_RAM> result = new List<LibHost.Devices.Device_RAM>(); //создаем пустой список 

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM win32_PhysicalMemory");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get()) //выполняем запрос и получаем список обектов, затем из каждого объекта получаем необходимую информацию
                {
                    
                    string manufacturer = (string)obj["Manufacturer"] ?? "-1"; //приводим поле к типу string и проверяем на NULL, если поле NULL то присваиваем значение "-1"
                    uint clock_speed = obj["Speed"] != null ? (uint)obj["Speed"]:0;
                    ushort memory_type = obj["MemoryType"] != null ? (ushort)obj["MemoryType"] : (ushort)0; //проверяем на NULL, если так то присваиваем значение 0, в БД есть расшифровка этих значений, в отдельной таблице
                    ushort form_factor = obj["FormFactor"] != null ? (ushort)obj["FormFactor"] : (ushort)0; //проверяем на NULL, если так то присваиваем значение 0, в БД есть расшифровка этих значений, в отдельной таблице
                    string DeviceLocator = (string)obj["DeviceLocator"] ?? "-1";
                    ulong size = (ulong)obj["Capacity"];


                    result.Add(new LibHost.Devices.Device_RAM(manufacturer, (int)clock_speed,  (int)memory_type, (int)form_factor, size, DeviceLocator)); //добавляем в список новый объект 
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);               
            }


            return result; // возвращаем наполненный список
        }
        private static List<LibHost.Devices.Device_HDD> Search_HDD() //метод для обнаружения логицеских дисков 
        {
            List<LibHost.Devices.Device_HDD> result = new List<LibHost.Devices.Device_HDD>(); //создаем пустой список 

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk WHERE DriveType=3");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get()) //выполняем запрос и получаем список обектов, затем из каждого объекта получаем необходимую информацию
                {
                    string caption = (string)obj["Caption"] ?? "-1"; //приводим поле к типу string и проверяем на NULL, если поле NULL то присваиваем значение "-1"
                    string description = (string)obj["Description"] ?? "-1";//приводим поле к типу string и проверяем на NULL, если поле NULL то присваиваем значение "-1"
                    ulong size = (ulong)obj["Size"];
                    ulong free_space = (ulong)obj["FreeSpace"];
                    string file_system = (string)obj["FileSystem"] ?? "-1"; ;//приводим поле к типу string и проверяем на NULL, если поле NULL то присваиваем значение "-1"



                    result.Add(new LibHost.Devices.Device_HDD(description,caption,size,free_space,file_system)); //добавляем в список новый объект, параметры передаекм в конструктор
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }

            return result;//возвращаем наполненный список логических дисков
        }
        private static List<LibHost.Devices.Device_NET> Search_NET() //метод для обнаружения сетевых адаптеров
        {
            List<LibHost.Devices.Device_NET> result = new List<LibHost.Devices.Device_NET>(); // создаем пустой список сетевых адаптеров

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPenabled = true");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get()) //выполняем запрос и получаем список обектов, затем из каждого объекта получаем необходимую информацию
                {
                    string macaddress = (string)obj["macaddress"] ?? "-1"; //приводим поле к типу string и проверяем на NULL, если поле NULL то присваиваем значение "-1"
                    string description = (string)obj["description"] ?? "-1"; //приводим поле к типу string и проверяем на NULL, если поле NULL то присваиваем значение "-1"

                    List<IPAddress> ipaddress = new List<IPAddress>(); //создаем пустой список IP адресов
                    foreach (string ip in ((string[])obj["ipaddress"] ?? new string[1]{"0.0.0.0"})) // получаем массив адресов 
                    {
                        ipaddress.Add(IPAddress.Parse(ip)); //по очереди добавляем в список
                    }

                    List<IPAddress> DefaultIPGateway = new List<IPAddress>(); //создаем пустой список шлюзов
                    foreach (string ip in ((string[])obj["DefaultIPGateway"] ?? new string[1] { "0.0.0.0" })) //получаем массив шлюзов
                    {
                        DefaultIPGateway.Add(IPAddress.Parse(ip)); //по очереди добавляем в список
                    }

                    result.Add(new LibHost.Devices.Device_NET(macaddress,description,DefaultIPGateway,ipaddress)); //создаем новый обьект сетевой карточки, полученные данные передаем аргументом в конструктор
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }

            return result; //возвращаем наполненный список сетевых карточек
        }
        private static List<LibHost.Devices.Device_GPU> Search_GPU() //метод для обнаружения видео ускорителей
        {
            List<LibHost.Devices.Device_GPU> result = new List<LibHost.Devices.Device_GPU>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get())//выполняем запрос и получаем список обектов, затем из каждого объекта получаем необходимую информацию
                {
                    string name = (string)obj["Name"] ?? "-1"; //получаем имя, проверяем на null, если null то присваиваем "-1" 
                    uint memory_size = (uint)obj["AdapterRAM"];

                    result.Add(new LibHost.Devices.Device_GPU(name, memory_size)); //добавляем адаптер в список
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }

            return result; //возвращаем наполненный список видеоадаптеров 
        }
        #endregion


        private static List<string> Execute_WMI_Query(string query, string ClassItemAdd) //метод для получения списка значений конкретного поля у конкретного класса WMI, на вход получаем запрос и имя поля
        {
            List<string> result = new List<string>(); //создаем пустой список сипа "строка"
            string temp;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query); //создаем объект который будет выполнять запрос

            try
            {
                SendMSG("Выполняется запрос: " + searcher.Query.QueryString);
                foreach (ManagementObject obj in searcher.Get()) //выполняем запрос 
                {
                    temp = obj[ClassItemAdd].ToString().Trim(); //каждое полученое значение конвертируем в строку и обрезаем пробелы по краям
                    result.Add(temp ?? "-1"); //если не NULL то добавляем в список результатов
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }
            return result; //возвращаем резултьтирующий список
        }

        private static List<string[]> Execute_WMI_QueryArray(string query, string ClassItemAdd)//работает аналогично предыдущеуму методу , только возвращает не список строк, а список массивов строк
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

        private static bool ServerIsAvailible(IPAddress ip, int port) //метод проверки сервера ан доступность, на вход получаем адрес и порт
        {
            bool result; 
            try
            {
                TcpClient newClient = new TcpClient();
                IPEndPoint endPoint = new IPEndPoint(ip, port);
                newClient.Connect(endPoint); //пробуем установить соединение с удаленным компьютером
                result = true; // если удачно то результат истина
            }
            catch (Exception ex) //если не удачно то будет выброшено исключение
            {
                result = false; //результат ложь
                SendMSG(ex.Message);   //генерируем сообщение 
            }

            return result; //возвращаем рузультат
        } 


        public static void SendMSG(string str) //метод который дергает событие, для отправки сообщений в лог или консоль
        {
            if (debug & DebugInfoSend != null) DebugInfoSend(str); //если дебаг включен и в событии есть обработчики то выполнить их
        }
    }
}
