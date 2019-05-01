using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost.Devices
{
    [Serializable]
    public struct Device_MB : IDevice
    {
        public int hash;
        public string manufacturer;
        public string model;
        public string name;
        public string product;
        public string serial_number;

        public Device_MB( string manufacturer, string model, string name, string product, string serial_number)
        {

            this.manufacturer = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer));
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.product = product ?? throw new ArgumentNullException(nameof(product));
            this.serial_number = serial_number ?? throw new ArgumentNullException(nameof(serial_number));

            string temp = (this.manufacturer + this.model + this.name + this.product + this.serial_number);

            this.hash = temp.GetHashCode();
        }
    }
}
