using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibSrv
{
    public class Client
    {
        public TcpClient client;
        public Client(TcpClient tcpClient)
        {
            client = tcpClient;
            Work.SendMSG("Подключен новый узел!");
        }

        public void Process()
        {
            NetworkStream stream = null;
            BinaryFormatter formatter = new BinaryFormatter();

        
            try
            {
                stream = client.GetStream();

                while (Work.enable & client.Connected)
                {
                    while (Work.enable & stream.DataAvailable)
                    {
                        LibHost.Host host = (LibHost.Host)formatter.Deserialize(stream);
                        Work.SendMSG("Приняты данные об узле: " + host.hostname);
                        Work.queue_to_db.Enqueue(host);                        
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}
