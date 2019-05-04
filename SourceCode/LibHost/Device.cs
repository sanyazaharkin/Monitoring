using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost
{
    [Serializable]
    public class Device
    {
        public int id;
        public string device_type;
        public int hash;

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
