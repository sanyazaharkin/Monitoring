using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Specialized;



namespace MonitoringInterface
{
    public partial class HostsForm : Form
    {
        private MySqlConnection db_conn;
        private int GridColumns = 6;


        public HostsForm()
        {
            InitializeComponent();
        }
        public HostsForm(object sAll)
        {
            InitializeComponent();
            db_conn = Get_db_conn((NameValueCollection)sAll);
            timer1.Interval = (int)SetTimerPeriod.Value*1000;
            timer1.Enabled = true;
        }



        private void UpdateHostsGrid()
        {
            HostsGrid.Rows.Clear();

            foreach (string[] row in GetTableFromDB("SELECT id,hostname,bios_version,operating_system,last_update_time,state FROM hosts"))
            {
                string[] columns = row;

                HostsGrid.Rows.Add(columns);
            }



            HostsGrid.Update();
        }

        private MySqlConnection Get_db_conn(NameValueCollection config)
        {

            string db_server_port = config["db_server_port"];
            string db_server_ip = config["db_server_ip"];
            string db_name = config["db_name"];
            string db_user = config["db_user"];
            string db_pass = config["db_pass"];
            MySqlConnection connection = new MySqlConnection("Server=" + db_server_ip + ";Database=" + db_name + ";port=" + db_server_port + ";User Id=" + db_user + ";password=" + db_pass + ";CharSet=utf8");
            return connection;
        }
        private List<string[]> GetTableFromDB(string query)
        {
            db_conn.Open();
            MySqlCommand command = db_conn.CreateCommand();
            command.CommandText = query;
            MySqlDataReader reader = command.ExecuteReader();

            List<string[]> result = new List<string[]>();
            
            while (reader.Read())
            {
                result.Add(new string[GridColumns]);
                for (int i = 0; i < GridColumns; i++)
                {
                    result[result.Count - 1][i] = reader[i].ToString();
                }

            }
            reader.Close();
            db_conn.Close();

            return result;
        }

        private void SetTimerPeriod_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)SetTimerPeriod.Value*1000;
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateHostsGrid();
        }

        private void HostsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
