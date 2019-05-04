using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost.Devices
{
    [Serializable]
    public class Device_GPU : Device
    {
        
        public string name;
        public uint memory_size;



        public Device_GPU(string name, uint memory_size)
        {
            this.device_type = "GPU";

            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.memory_size = memory_size;
            string temp = (this.name + this.memory_size.ToString());

            this.hash = temp.GetHashCode();
        }

        public override string ToString()
        {
            string result = string.Empty;
            result += "\n-----------------------GPU------------------------";
            result += "\nhash = " + this.hash;
            result += "\nname = " + this.name;
            result += "\nmemory_size = " + this.memory_size;
            result += "\ndevice_type = " + this.device_type;

            result += "\n--------------------------------------------------\n";


            return result;
        }
    }
}
