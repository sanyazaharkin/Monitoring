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

        protected override void OnStart(string[] args)
        {
            LibAgent.Work.DebugInfoSend += WriteToEventLog;
            
            Thread work = new Thread(new ParameterizedThreadStart(LibAgent.Work.Main));
            work.Start(ConfigurationManager.AppSettings);
        }

        protected override void OnStop()
        {
            LibAgent.Work.enable = true;
        }

        public static void WriteToEventLog(string str)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "ServiceAgent";
                eventLog.WriteEntry(str, EventLogEntryType.Information, 101, 1);
            }
        }
    }
}
