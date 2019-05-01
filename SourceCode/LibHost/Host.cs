using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibHost
{
    [Serializable]
    public struct Host
    {
        public int host_id;
        public string hostname;
        public string os_version;
        public string bios_version;
        public int state;
        public List<IDevice> Devices;
        public List<Process> Processes;
        public List<Program> Programs;

        public Host( string hostname, string os_version, string bios_version, int state, List<IDevice> devices, List<Process> processes, List<Program> programs)
        {
            this.host_id = 0;
            this.hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));
            this.os_version = os_version ?? throw new ArgumentNullException(nameof(os_version));
            this.bios_version = bios_version ?? throw new ArgumentNullException(nameof(bios_version));
            this.state = state;
            Devices = devices ?? throw new ArgumentNullException(nameof(devices));
            Processes = processes ?? throw new ArgumentNullException(nameof(processes));
            Programs = programs ?? throw new ArgumentNullException(nameof(programs));
        }
    }
}
