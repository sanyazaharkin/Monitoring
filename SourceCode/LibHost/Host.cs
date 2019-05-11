using System;
using System.Collections.Generic;

namespace LibHost
{
    [Serializable] //параметр указывает что класс будет сериализовываться
    public class Host
    {
        public int host_id; //переменная для хранения ID устройства, пудет запрашиваться из БД
        public string hostname; //хранение имя устройства
        public string os_version; //верссии ОС
        public string bios_version; //версии BIOS
        public int state; // хранение состояния устройства
        public List<Device> Devices; //список устройств
        public List<Process> Processes; //список процессов
        public List<Program> Programs; //список программ

        public Host( string hostname, string os_version, string bios_version, int state, List<Device> devices, List<Process> processes, List<Program> programs)  //конструктор который в аргументы принимает информацию об узле и списки программ, процессов и устройств
        {
            this.host_id = 0;
            this.hostname = hostname ?? "-1";
            this.os_version = os_version ?? "-1";
            this.bios_version = bios_version ?? "-1";
            this.state = state;
            Devices = devices ?? new List<Device>();
            Processes = processes ?? new List<Process>();
            Programs = programs ?? new List<Program>();
        }

        public override string ToString() //переопределенный метод возвращающий строку, нужен был только для отладки, не используется сейчас
        {
            string result = string.Empty;
            result += "\n-----------------------Host------------------------";
            result += "\nhost_id = " + this.host_id;
            result += "\nhostname = " + this.hostname;
            result += "\nos_version = " + this.os_version;
            result += "\nbios_version = " + this.bios_version;
            result += "\nstate = " + this.state;
            result += "\n--------------------------------------------------\n";

            foreach (var item in Devices)
            {
                result += item.ToString();
            }

            foreach (var item in Programs)
            {
                result += item.ToString();
            }

            foreach (var item in Processes)
            {
                result += item.ToString();
            }

            return result;
        }
    }
}
