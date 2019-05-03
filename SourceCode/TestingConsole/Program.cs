using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace TestingConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LibAgent.Work.DebugInfoSend += ShowMessage;
            //LibHost.Host host = LibAgent.Work.GetHost();
            //Console.WriteLine(host);
            NameValueCollection sAll = ConfigurationManager.AppSettings;
            LibAgent.Work.Main(sAll);



            Console.ReadLine();
        }


        public static void ShowMessage(string str)
        {
            Console.WriteLine(str);
        }
    }
}
