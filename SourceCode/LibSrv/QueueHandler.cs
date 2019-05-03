using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LibSrv
{
    public class QueueHandler
    {
        Queue<LibHost.Host> queue = null;
        MySqlConnection connection = null;
        public QueueHandler(Queue<LibHost.Host> queue, MySqlConnection connection)
        {
            this.queue = queue;
            this.connection = connection;
        }



        public void Main()
        {
            while (Work.enable)
            {
                while (this.queue.Count > 0)
                {
                    LibHost.Host host = queue.Dequeue();
                    Task write = new Task(() => DB_Writer.start_write(host,connection));
                    write.Start();                    
                }
                Thread.Sleep(100);
            }

        }
    }
}
