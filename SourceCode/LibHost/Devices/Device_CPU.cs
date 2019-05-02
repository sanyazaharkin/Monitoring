using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost.Devices
{
    [Serializable]
    public class Device_CPU : Device
    {
        public int hash;
        public string manufacturer;
        public string name;
        public int cores;
        public int clock_speed;
        public string device_type;

        public Device_CPU(string manufacturer, string name,int cores, int clock_speed)
        {
            this.device_type = "CPU";

            this.manufacturer = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.cores = cores;
            this.clock_speed = clock_speed;
            string temp = (this.manufacturer + this.name + this.cores.ToString() + this.clock_speed.ToString());
            
            this.hash = temp.GetHashCode();            
        }


        public override string ToString()
        {
            string result = string.Empty;
            result += "\n-----------------------CPU------------------------";
            result += "\nhash = " + this.hash;
            result += "\nmanufacturer = " + this.manufacturer;
            result += "\nname = " + this.name;
            result += "\ncores = " + this.cores;
            result += "\nclock_speed = " + this.clock_speed;
            result += "\ndevice_type = " + this.device_type;
            result += "\n--------------------------------------------------\n";


            return result;
        }
    }
}
