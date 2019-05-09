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
        public int hash;
        public string name;
        public string version;
        public string vendor;
        

        public Program(string name, string version, string vendor)
        {
            this.program_id = 0;
            this.name = name ?? "-1";
            this.version = version ?? "-1";
            this.vendor = vendor ?? "-1";
            string temp = (this.name + this.version + this.vendor );

            this.hash = temp.GetHashCode();
        }



        public override string ToString()
        {
            return ("hash: " + this.hash + " name: " + name + " version: " + vendor + " vendor: " + version + " \n");
        }
    }
}
