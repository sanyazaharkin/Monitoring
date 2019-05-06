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

namespace ServerService
{
    public partial class ServerMonitoringService : ServiceBase
    {
        public ServerMonitoringService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            LibSrv.Work.DebugInfoSend += WriteToEventLog;

            Thread work = new Thread(new ParameterizedThreadStart(LibSrv.Work.Main));
            work.Start(ConfigurationManager.AppSettings);
        }

        protected override void OnStop()
        {
            LibSrv.Work.enable = false;
        }

        public static void WriteToEventLog(string str)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "ServerMonitoringService";
                eventLog.WriteEntry(str, EventLogEntryType.Information, 101, 1);
            }
        }
    }
}
