using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LibSrv
{
    public class QueueHandler
    {
        Queue<LibHost.Host> queue = null;
        public QueueHandler(Queue<LibHost.Host> queue)
        {
            this.queue = queue;
        }



        public void Main()
        {
            while (Work.enable)
            {
                while (this.queue.Count > 0)
                {
                    LibHost.Host host = queue.Dequeue();
                    Work.SendMSG("Обрабатывается узел");
                    Work.SendMSG(host.ToString());
                }
                Thread.Sleep(100);
            }

        }
    }
}
