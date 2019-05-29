using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonitoringInterface
{
    public class Cabinet
    {


        public int id { get; set; }
        public string cabinet { get; set; }

        public Cabinet(int id, string cabinet)
        {
            this.id = id;
            this.cabinet = cabinet;
        }


        public override string ToString()
        {
            return cabinet;
        }
    }
}
