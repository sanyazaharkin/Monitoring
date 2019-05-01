using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost.Devices
{
    [Serializable]
    public struct Device_HDD : IDevice
    {
        public int hash;
        public string description;
        public string caption;
        public ulong size;
        public ulong free_space;
        public string file_system;

        public Device_HDD( string description, string caption, ulong size, ulong free_space, string file_system)
        {
            
            this.description = description ?? throw new ArgumentNullException(nameof(description));
            this.caption = caption ?? throw new ArgumentNullException(nameof(caption));
            this.size = size;
            this.free_space = free_space;
            this.file_system = file_system ?? throw new ArgumentNullException(nameof(file_system));

            string temp = (this.description + this.caption + this.size.ToString() + this.free_space.ToString() + this.file_system.ToString());

            this.hash = temp.GetHashCode();
        }
    }
}
