using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost
{
    [Serializable]
    public class Host
    {
        public int host_id;
        public string hostname;
        public string os_version;
        public string bios_version;
        public int state;
        public List<Device> Devices;
        public List<Process> Processes;
        public List<Program> Programs;

        public Host( string hostname, string os_version, string bios_version, int state, List<Device> devices, List<Process> processes, List<Program> programs)
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

        public override string ToString()
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
