using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost.Devices
{
    [Serializable]
    public struct Device_GPU : IDevice
    {
        public int hash;
        public string name;
        public long memory_size;

        public Device_GPU(string name, long memory_size)
        {

            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.memory_size = memory_size;
            string temp = (this.name + this.memory_size.ToString());
            this.hash = temp.GetHashCode();
        }
    }
}
