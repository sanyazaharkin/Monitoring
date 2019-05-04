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

        private static int os_id = 0;

        public static void start_write(LibHost.Host host, MySqlConnection connection)
        {

            connection.Open();
            try
            {
                //if (connection != null & connection.State == System.Data.ConnectionState.Closed) connection.Open();

                sql_SELECT_Execute("START TRANSACTION;", connection);

                #region запись в таблицу hosts
                if (sql_SELECT_Execute("SELECT EXISTS(SELECT id FROM hosts WHERE hostname='" + host.hostname + "');", connection) == "1")
                {
                    host.host_id = int.Parse(sql_SELECT_Execute("UPDATE hosts SET last_update_time='" + DateTime.Now + "';SELECT id FROM hosts WHERE hostname='" + host.hostname + "';", connection));
                }
                else
                {
                    if (sql_SELECT_Execute("SELECT EXISTS (SELECT id FROM operating_systems WHERE system='" + host.os_version + "');", connection) == "1")
                    {
                        os_id = int.Parse(sql_SELECT_Execute("SELECT id FROM operating_systems WHERE system='" + host.os_version + "';", connection));
                    }
                    else
                    {
                        os_id = int.Parse(sql_SELECT_Execute("INSERT INTO operating_systems(system) VALUES ('" + host.os_version + "'); SELECT LAST_INSERT_ID();", connection));
                    }

                    host.host_id = int.Parse(sql_SELECT_Execute("INSERT INTO hosts (hostname, operating_system, bios_version, state, last_update_time) VALUES ('" + host.hostname + "', " + os_id + ", '" + host.bios_version + "', " + host.state + ", '" + DateTime.Now + "') ON DUPLICATE KEY UPDATE last_update_time=VALUES(last_update_time); SELECT LAST_INSERT_ID();", connection));
                }
                #endregion

                #region запись информации о железе
                foreach (LibHost.Device item in host.Devices)
                {


                    switch (item.device_type.ToUpper())
                    {
                        case ("MB"):
                            Write_MB_to_DB((LibHost.Devices.Device_MB)item, host.host_id, connection);
                            break;
                        case ("CPU"):
                            Write_CPU_to_DB((LibHost.Devices.Device_CPU)item, host.host_id, connection);
                            break;
                        case ("RAM"):
                            Write_RAM_to_DB((LibHost.Devices.Device_RAM)item, host.host_id, connection);
                            break;
                        case ("HDD"):
                            Write_HDD_to_DB((LibHost.Devices.Device_HDD)item, host.host_id, connection);
                            break;
                        case ("NET"):
                            Write_NET_to_DB((LibHost.Devices.Device_NET)item, host.host_id, connection);
                            break;
                        case ("GPU"):
                            Write_GPU_to_DB((LibHost.Devices.Device_GPU)item, host.host_id, connection);
                            break;
                    }


                }
                #endregion


                sql_SELECT_Execute("COMMIT;", connection);
            }
            catch (Exception ex)
            {
                
                Work.SendMSG(ex.Message);
            }
            finally
            {
                if (connection != null & connection.State == System.Data.ConnectionState.Open) connection.Close();
            }

        }


        private static string sql_SELECT_Execute(string query, MySqlConnection conn)
        {
            Work.SendMSG("Выполняется запрос: " + query);
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = query;

            object answer = command.ExecuteScalar();
            Work.SendMSG("Получен ответ:" + (answer != null ? answer.ToString() : "-1"));
            return answer!=null?answer.ToString():"-1";
        }


        private static void Write_MB_to_DB(LibHost.Devices.Device_MB device_MB, int host_id, MySqlConnection connection)
        {
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT id FROM devices WHERE device_name_hash='" + device_MB.hash + "');", connection) == "1")
            {

            }
            else
            {
                sql_SELECT_Execute("INSERT IGNORE INTO devices (device_name_hash, device_type)='" + device_MB.hash + "', '" + device_MB.device_type + "';", connection);
            }
            //{
            //    host.host_id = int.Parse(sql_SELECT_Execute("UPDATE hosts SET last_update_time='" + DateTime.Now + "';SELECT id FROM hosts WHERE hostname='" + host.hostname + "';", connection));
            //}
            //else
            //{
            //    if (sql_SELECT_Execute("SELECT EXISTS (SELECT id FROM operating_systems WHERE system='" + host.os_version + "');", connection) == "1")
            //    {
            //        os_id = int.Parse(sql_SELECT_Execute("SELECT id FROM operating_systems WHERE system='" + host.os_version + "';", connection));
            //    }
            //    else
            //    {
            //        os_id = int.Parse(sql_SELECT_Execute("INSERT INTO operating_systems(system) VALUES ('" + host.os_version + "'); SELECT LAST_INSERT_ID();", connection));
            //    }

            //    host.host_id = int.Parse(sql_SELECT_Execute("INSERT INTO hosts (hostname, operating_system, bios_version, state, last_update_time) VALUES ('" + host.hostname + "', " + os_id + ", '" + host.bios_version + "', " + host.state + ", '" + DateTime.Now + "') ON DUPLICATE KEY UPDATE last_update_time=VALUES(last_update_time); SELECT LAST_INSERT_ID();", connection));
            //}

        }

        private static void Write_CPU_to_DB(LibHost.Devices.Device_CPU device_CPU, int host_id, MySqlConnection connection)
        {

        }

        private static void Write_RAM_to_DB(LibHost.Devices.Device_RAM device_RAM, int host_id, MySqlConnection connection)
        {

        }

        private static void Write_HDD_to_DB(LibHost.Devices.Device_HDD device_HDD, int host_id, MySqlConnection connection)
        {

        }

        private static void Write_NET_to_DB(LibHost.Devices.Device_NET device_NET, int host_id, MySqlConnection connection)
        {

        }

        private static void Write_GPU_to_DB(LibHost.Devices.Device_GPU device_GPU, int host_id, MySqlConnection connection)
        {

        }



    }
}
