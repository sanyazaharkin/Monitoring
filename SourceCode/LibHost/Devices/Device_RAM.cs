using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost.Devices
{
    [Serializable]
    public struct Device_RAM : IDevice
    {
        public int hash;
        public string manufacturer;
        public int clock_speed;
        public int voltage;
        public int memory_type;
        public int form_factor;
        public ulong size;

        public Device_RAM(string manufacturer, int clock_speed, int voltage, int memory_type, int form_factor, ulong size)
        {

            this.manufacturer = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer));
            this.clock_speed = clock_speed;
            this.voltage = voltage;
            this.memory_type = memory_type;
            this.form_factor = form_factor;
            this.size = size;

            string temp = (this.manufacturer + this.clock_speed.ToString() + this.voltage.ToString() + this.memory_type.ToString() + this.form_factor.ToString() + this.size.ToString());

            this.hash = temp.GetHashCode();
        }
    }
}
