using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Specialized;


namespace LibSrv
{
    public class Work
    {
        public delegate void Message(string str); //объявляем делегат кторый будет использоваться для вывода дебаг информации в консоль или лог (куда угодно)
        public static event Message DebugInfoSend;  // событие которое будет вызываться при необходимости вывода информации


        public static bool enable; //булевая переменная которая отвечает за работу главного цикла
        private static bool debug; //переменная которая включает, выключает вывод отладочной информации 
        private static int Listen_port;
        private static IPAddress Listen_address;        
        private static TcpListener listener = null;

        public  static Queue<LibHost.Host> queue_to_db = new Queue<LibHost.Host>();//объявляем очередь в которую будут складываться принятые объекты
        private static QueueHandler queueHandler; //объявляем ссылку на  обьект обработчика очереди
        private static Thread queueHandlerThread; //объявляем ссылку на  объект потока для обработчика


        public static void Main(object sAll) //главный метод, точка входа в программу, в аргументы принимает коллекцию с настройками
        {
            NameValueCollection config = (NameValueCollection)sAll; //принятые настройки приводим к типу NameValueCollection (downcast)



            enable = true;
            debug = config["enable_log"].ToLower() == "yes" ? true : false;  //читаем полученый конфиг
            Listen_address = IPAddress.Parse(config["Listen_ip"]);
            Listen_port = int.Parse(config["Listen_port"]);
            


            queueHandler = new QueueHandler(queue_to_db, config); //создаем новый объект обработчика в параметры передаем ссылку на очередь и конфигурацию
            queueHandlerThread = new Thread(new ThreadStart(queueHandler.Main)); //создаем новый поток
            
            queueHandlerThread.Start(); //запускаем поток с обработчиком очереди



            try
            {
                listener = new TcpListener(Listen_address, Listen_port); //создаем TCP сервер, в аргументы передаем значения из конфига
                listener.AllowNatTraversal(true); // включаем NatTraversal, необходимая настроийка если сервер находится за NAT
                listener.Start(); //запускаем сервер
                SendMSG("Выполнен запуск с параметрами: Listen_address="+ Listen_address + ", Listen_port="+ Listen_port); //шлем сообщение 


                

                while (enable) //запускаем главный цикл программы, по сути вечный цикл (пока enable == true) 
                {
                    while (enable & listener.Pending()) //пока enable == true и есть запросы на подключение 
                    {
                        TcpClient client = listener.AcceptTcpClient(); //создаем объект клиента приняв запрос на подключение
                        Client clientObject = new Client(client); 
                        Thread clientThread = new Thread(new ThreadStart(clientObject.Process)); //и передаем обрабатываться в новый поток
                        clientThread.Start(); //запускаем поток с клиентом
                    }


                    Thread.Sleep(500);//если запросов на подключение не осталось приостанавливаем поток на полсекунды
                }
            }
            catch (Exception ex) //ловим возможные исключения 
            {
                SendMSG(ex.Message); //отправляем их в сообщении
            }
            finally
            {
                if (listener != null)  //после завершения цикла останавливаем сервер 
                    listener.Stop();
            }

        }



        public static void SendMSG(string str) //метод для отправки отладочной информации
        {
            if (debug & DebugInfoSend != null) DebugInfoSend(str); //если декбаг включен и событие не пусто то генерируем событие
        }
    }
}
