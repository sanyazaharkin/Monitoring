using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.Threading;

namespace AgentService
{
    public partial class ServiceAgent : ServiceBase
    {
        public ServiceAgent()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) // метод выполняемый при запуске службы
        {
            LibAgent.Work.DebugInfoSend += WriteToEventLog; // добавляем обработчик события
            
            Thread work = new Thread(new ParameterizedThreadStart(LibAgent.Work.Main));  // создаем новый поток для главного метода программы
            work.Start(ConfigurationManager.AppSettings); // запускаем поток и в параметры передаем коллекцию с настроиками из файла
        }

        protected override void OnStop() //метод вызываемый при остановке службы
        {
            LibAgent.Work.enable = false; // останавливаем все циклы
        }

        public static void WriteToEventLog(string str)  // метод для записи информации в EventLog
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "ServiceAgent";
                eventLog.WriteEntry(str, EventLogEntryType.Information, 101, 1);
            }
        }
    }
}
