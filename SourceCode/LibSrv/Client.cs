using System;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibSrv
{
    public class Client
    {
        public TcpClient client; //переменная для хранения ссылки на используемое подключение
        public Client(TcpClient tcpClient) //конструктор который в параметры принимает подключение, уже открытое и готовое к чтению/записи информации в/из него
        {
            client = tcpClient;
            Work.SendMSG("Подключен новый узел!");
        }

        public void Process() //метод который запускается в отдельном потоке при подключении клиента
        {
            NetworkStream stream = null; //ссылка на объект потока
            BinaryFormatter formatter = new BinaryFormatter();  //объект который будет выполнять десериализацию

        
            try
            {
                stream = client.GetStream(); //получаем поток из подключения 

                while (Work.enable & client.Connected) //и пока программа включена и подключение активно
                {
                    while (Work.enable & stream.DataAvailable) // пока в потоке доступны данные для чтения 
                    {
                        LibHost.Host host = (LibHost.Host)formatter.Deserialize(stream); //читаем данные из потока, десериализуем и выполняем приведение к типу (LibHost.Host), на выходе получаем объект класса LibHost.Host
                        Work.SendMSG("Приняты данные об узле: " + host.hostname); //шлем сообщение
                        Work.queue_to_db.Enqueue(host);      //складываем полученный объект в очередь                  
                    }
                    Thread.Sleep(500);//пока в потоке(stream) нет данных отправляем поток(Thread) спать на полсекунды
                }
            }
            catch (Exception ex) //ловим возможные исключения
            {
                Console.WriteLine(ex.Message); //шлем сообщение в консоль
            }
            finally
            {
                if (stream != null) //после завершения цикла закрываем поток и подключение
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}
