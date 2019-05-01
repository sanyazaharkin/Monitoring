using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost.Devices
{
    [Serializable]
    public struct Device_CPU : IDevice
    {
        public int hash;
        public string manufacturer;
        public string name;
        public int threads;
        public int cores;
        public int clock_speed;

        public Device_CPU(string manufacturer, string name, int threads, int cores, int clock_speed)
        {
            
            this.manufacturer = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.threads = threads;
            this.cores = cores;
            this.clock_speed = clock_speed;
            string temp = (this.manufacturer + this.name + this.threads.ToString() + this.cores.ToString() + this.clock_speed.ToString());

            this.hash = temp.GetHashCode();
            
        }
    }
}
