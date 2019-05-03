﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;


namespace ServerConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            LibSrv.Work.DebugInfoSend += ShowMessage;

            NameValueCollection sAll = ConfigurationManager.AppSettings;
            LibSrv.Work.Main(sAll);




            Console.ReadLine();
        }


        public static void ShowMessage(string str)
        {
            Console.WriteLine(str);
        }
    }
}
