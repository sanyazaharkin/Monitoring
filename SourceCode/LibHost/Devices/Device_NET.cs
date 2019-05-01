using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace LibHost.Devices
{
    [Serializable]
    public struct Device_NET : IDevice
    {
        public int hash;
        public string mac;
        public string description;
        public System.Net.IPAddress Gateway;
        public List<System.Net.IPAddress> iPAddresses;

        public Device_NET( string mac, string description, IPAddress gateway, List<System.Net.IPAddress> iPAddresses)
        {

            this.mac = mac ?? throw new ArgumentNullException(nameof(mac));
            this.description = description ?? throw new ArgumentNullException(nameof(description));
            Gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
            this.iPAddresses = iPAddresses ?? throw new ArgumentNullException(nameof(iPAddresses));

            string temp = (this.mac + this.description);

            this.hash = temp.GetHashCode();
        }
    }
}
