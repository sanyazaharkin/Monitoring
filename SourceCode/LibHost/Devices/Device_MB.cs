using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost.Devices
{
    [Serializable]
    public class Device_MB : Device
    {
       
        public string manufacturer;
        public string model;
        public string name;
        public string product;
        public string serial_number;


        public Device_MB( string manufacturer, string model, string name, string product, string serial_number)
        {
            this.device_type = "MB";

            this.manufacturer = manufacturer ?? "-1";
            this.model = model ?? "-1";
            this.name = name ?? "-1";
            this.product = product ?? "-1";
            this.serial_number = serial_number ?? "-1";

            string temp = (this.manufacturer + this.model + this.name + this.product + this.serial_number);

            this.hash = temp.GetHashCode();
        }


        public override string ToString()
        {
            string result = string.Empty;
            result += "\n-----------------------MB------------------------";
            result += "\nhash = " + this.hash;
            result += "\nmanufacturer = " + this.manufacturer;
            result += "\nmodel = " + this.model;
            result += "\nname = " + this.name;
            result += "\nproduct = " + this.product;
            result += "\nserial_number = " + this.serial_number;
            result += "\ndevice_type = " + this.device_type;

            result += "\n--------------------------------------------------\n";


            return result;
        }
    }
}
