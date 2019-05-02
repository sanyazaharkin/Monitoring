using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestingConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LibAgent.Work.DebugInfoSend += ShowMessage;
            //LibHost.Host host = LibAgent.Work.GetHost();
            //Console.WriteLine(host);
            LibAgent.Work.Main(args);



            Console.ReadLine();
        }


        public static void ShowMessage(string str)
        {
            Console.WriteLine(str);
        }
    }
}
