using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost
{
    [Serializable]
    public class Program
    {
        public int program_id;
        public string name;
        public string version;
        public string vendor;

        public Program(string name, string version, string vendor)
        {
            this.program_id = 0;
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.version = version ?? throw new ArgumentNullException(nameof(version));
            this.vendor = vendor ?? throw new ArgumentNullException(nameof(vendor));
        }



        public override string ToString()
        {
            return ("name: " + name + " version: " + vendor + " vendor: " + version + " \n");
        }
    }
}
