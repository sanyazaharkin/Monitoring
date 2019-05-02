using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibHost
{
    [Serializable]
    public class Process
    {
        public int process_id;
        public string name;

        public Process(string name)
        {
            this.process_id = 0;
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }


        public override string ToString()
        {
            return (name + "\n");
        }
    }
}
