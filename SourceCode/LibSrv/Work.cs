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

            queueHandlerThread.Start();

            enable = true;
            debug = config["enable_log"].ToLower() == "yes" ? true : false;
            Listen_address = IPAddress.Parse(config["Listen_ip"]);
            Listen_port = int.Parse(config["Listen_port"]);
            timeout = int.Parse(config["timeout"]);


            queueHandler = new QueueHandler(queue_to_db, Get_db_conn_string_from_config(config));
            queueHandlerThread = new Thread(new ThreadStart(queueHandler.Main));


        

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


        private static MySqlConnection Get_db_conn_string_from_config(NameValueCollection config)
        {

            string db_server_port = config["db_server_port"];
            string db_server_ip = config["db_server_ip"];
            string db_name = config["db_name"];
            string db_user = config["db_user"];
            string db_pass = config["db_pass"];

            return new MySqlConnection("Server=" + db_server_ip + ";Database=" + db_name + ";port=" + db_server_port + ";User Id=" + db_user + ";password=" + db_pass); 

        }
        public static void SendMSG(string str)
        {
            if (debug & DebugInfoSend != null) DebugInfoSend(str);
        }
    }
}
