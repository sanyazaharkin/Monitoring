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
            LibAgent.Work.MSG += ShowMessage;
            LibHost.Host host = LibAgent.Work.GetHost();
            



            Console.ReadLine();
        }


        public static void ShowMessage(string str)
        {
            Console.WriteLine(str);
        }
    }
}
