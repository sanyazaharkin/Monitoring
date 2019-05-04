using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.Specialized;

namespace LibSrv
{
    public class QueueHandler
    {
        Queue<LibHost.Host> queue = null;
        NameValueCollection config;
        public QueueHandler(Queue<LibHost.Host> queue, NameValueCollection conf)
        {
            this.queue = queue;
            config = conf;
        }



        public void Main()
        {
            while (Work.enable)
            {
                while (this.queue.Count > 0)
                {
                    LibHost.Host host = queue.Dequeue();                    
                    Task write = new Task(() => DB_Writer.start_write(host, Get_db_conn_string_from_config(config)));
                    write.Start();                    
                }
                Thread.Sleep(100);
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
    }
}
