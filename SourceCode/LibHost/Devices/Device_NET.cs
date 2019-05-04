using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace LibHost.Devices
{
    [Serializable]
    public class Device_NET : Device
    {
        
        public string mac;
        public string description;
        public List<System.Net.IPAddress> Gateway;
        public List<System.Net.IPAddress> iPAddresses;



        public Device_NET( string mac, string description, List<System.Net.IPAddress> gateway, List<System.Net.IPAddress> iPAddresses)
        {
            this.device_type = "NET";

            this.mac = mac ?? throw new ArgumentNullException(nameof(mac));
            this.description = description ?? throw new ArgumentNullException(nameof(description));
            Gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
            this.iPAddresses = iPAddresses ?? throw new ArgumentNullException(nameof(iPAddresses));

            string temp = (this.mac + this.description);

            this.hash = temp.GetHashCode();
        }

        public override string ToString()
        {
            string result = string.Empty;
            result += "\n-----------------------NET------------------------";
            result += "\nhash = " + this.hash;
            result += "\nmac = " + this.mac;
            result += "\ndescription = " + this.description;
            result += "\ngateways = ";
            foreach (var item in Gateway)
            {
                result += item.ToString() + " ";
            }
            result += "\nIPAddresses = " + this.iPAddresses;
            foreach (var item in iPAddresses)
            {
                result += item.ToString() + " ";
            }


            result += "\ndevice_type = " + this.device_type;

            result += "\n--------------------------------------------------\n";


            return result;
        }

    }
}
