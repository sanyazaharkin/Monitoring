using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace LibSrv
{
    public class DB_Writer
    {
        LibHost.Host host;
        MySqlConnection connection;
        int os_id = 0;

        public DB_Writer(LibHost.Host host, MySqlConnection connection)
        {
            this.host = host;
            this.connection = connection;
        }

        public void start_write()
        {
            
            try
            {
                connection.Open();
                sql_SELECT_Execute("START TRANSACTION;");


                if (sql_SELECT_Execute("SELECT EXIST (SELECT host_id FROM hosts WHERE hostname='" + host.hostname + "');") == "1")
                {
                    host.host_id = int.Parse(sql_SELECT_Execute("SELECT host_id FROM hosts WHERE hostname='" + host.hostname + "';"));
                }
                else
                {
                    if (sql_SELECT_Execute("SELECT EXIST (SELECT id FROM operating_systems WHERE system='" + host.os_version + "');") == "1")
                    {
                        os_id = int.Parse(sql_SELECT_Execute("SELECT id FROM operating_systems WHERE system='" + host.os_version + "';"));
                    }
                    else
                    {
                        os_id = int.Parse(sql_SELECT_Execute("INSERT INTO operating_systems(system) VALUES ('" + host.os_version + "'); SELECT LAST_INSERT_ID();"));
                    }

                    host.host_id = int.Parse(sql_SELECT_Execute("INSERT INTO hosts (hostname, operating_system, bios_version, state) VALUES ('" + host.hostname + "', " + os_id + ", '" + host.bios_version + "', " + host.state + "); SELECT LAST_INSERT_ID();"));
                }


                sql_SELECT_Execute("COMMIT;");
                connection.Close();
            }
            catch (Exception ex)
            {
                Work.SendMSG(ex.Message);
            }

        }


        private string sql_SELECT_Execute(string query)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;

            object answer = command.ExecuteScalar();

            return answer!=null?answer.ToString():"-1";
        }



    }
}
