using System;
using System.Collections.Generic;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Collections.Specialized;

namespace LibSrv
{
    public class QueueHandler
    {
        readonly Queue<LibHost.Host> queue; //ссылка на очередь
        readonly NameValueCollection config; //ссылка на конфиг
        public QueueHandler(Queue<LibHost.Host> queue, NameValueCollection conf) //конструктор в параметры принимает ссылки на очередь и конфиг
        {
            this.queue = queue;
            config = conf;

            Work.SendMSG("Запущен обработчик очереди"); //шлем сообщение
        }



        public void Main() //метод который выполняется в отдельном потоке
        {
            while (Work.enable) //пока программа запущенна, выполняется этот цикл
            {
                while (this.queue.Count > 0) //если в очереди есть элементы
                {
                    LibHost.Host host = queue.Dequeue(); //вытаскиваем элемент из очереди 
                    Work.SendMSG("ПИшем в базу информацию об узле: " + host.hostname);
                    DB_Writer.Start_write(host, Get_db_conn_string_from_config(config)); //и вызываем метод который выполнит запись в БД, в параметры передаем хост который достали из очереди и подключение к БД, которое мы сгенерировали методом Get_db_conn_string_from_config(config) 

                    Thread.Sleep(50); //между выполняемыми запросами делаем паузу 50мс
                }
                Thread.Sleep(1000); //отправляем поток спать на 1 секунду
            }

        }

        private static MySqlConnection Get_db_conn_string_from_config(NameValueCollection config) //метод возвращающий подключение к БД, в аргументы принимает коллекцию настроек
        {

            string db_server_port = config["db_server_port"]; //читаем конфиг
            string db_server_ip = config["db_server_ip"];
            string db_name = config["db_name"];
            string db_user = config["db_user"];
            string db_pass = config["db_pass"];

            return new MySqlConnection("Server=" + db_server_ip + ";Database=" + db_name + ";port=" + db_server_port + ";User Id=" + db_user + ";password=" + db_pass + ";CharSet=utf8"); //и возвращаем объект подключения которому в конструктор передали строку подключения
        }
    }
}
