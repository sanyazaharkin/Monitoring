using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;


namespace LibSrv
{
    public class Work
    {
        public delegate void Message(string str);
        public static event Message DebugInfoSend;


        public static bool enable;
        private static bool debug;
        private static int Listen_port;
        private static IPAddress Listen_address;
        private static int timeout;
        private static TcpListener listener = null;

        public  static Queue<LibHost.Host> queue_to_db = new Queue<LibHost.Host>();
        private static QueueHandler queueHandler;
        private static Thread queueHandlerThread;


        public static void Main(object sAll)
        {
            NameValueCollection config = (NameValueCollection)sAll;

           

            enable = true;
            debug = config["enable_log"].ToLower() == "yes" ? true : false;
            Listen_address = IPAddress.Parse(config["Listen_ip"]);
            Listen_port = int.Parse(config["Listen_port"]);
            timeout = int.Parse(config["timeout"]);


            queueHandler = new QueueHandler(queue_to_db, config);
            queueHandlerThread = new Thread(new ThreadStart(queueHandler.Main));
            
            queueHandlerThread.Start();



            try
            {
                listener = new TcpListener(Listen_address, Listen_port);
                listener.AllowNatTraversal(true);
                listener.Start();
                SendMSG("Выполнен запуск с параметрами: Listen_address="+ Listen_address + ", Listen_port="+ Listen_port);


                

                while (enable)
                {
                    if (listener.Pending())
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Client clientObject = new Client(client);
                        Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                        clientThread.Start();
                    }


                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                SendMSG(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }

        }



        public static void SendMSG(string str)
        {
            if (debug & DebugInfoSend != null) DebugInfoSend(str);
        }
    }
}
