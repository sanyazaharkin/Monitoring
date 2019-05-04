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
                            Write_to_DB((LibHost.Devices.Device_MB)item, host.host_id, connection);
                            break;
                        case ("CPU"):
                            Write_to_DB((LibHost.Devices.Device_CPU)item, host.host_id, connection);
                            break;
                        case ("RAM"):
                            Write_to_DB((LibHost.Devices.Device_RAM)item, host.host_id, connection);
                            break;
                        case ("HDD"):
                            Write_to_DB((LibHost.Devices.Device_HDD)item, host.host_id, connection);
                            break;
                        case ("NET"):
                            Write_to_DB((LibHost.Devices.Device_NET)item, host.host_id, connection);
                            break;
                        case ("GPU"):
                            Write_to_DB((LibHost.Devices.Device_GPU)item, host.host_id, connection);
                            break;
                    }

                    if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM host_devices WHERE device_id=" + item.id + " AND host_id=" + host.host_id + ");", connection) == "0")
                    {
                        sql_SELECT_Execute("INSERT INTO host_devices (device_id,host_id) VALUES (" + item.id + "," + host.host_id + "); SELECT LAST_INSERT_ID();", connection); // вынести потом это в другое место
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


        private static void Write_to_DB(LibHost.Devices.Device_MB device, int host_id, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM device_mb WHERE device_name_hash=" + device.hash + ");", connection)=="0")
            {                                   
                sql_SELECT_Execute("INSERT INTO device_mb (device_name_hash, manufacturer_id,model, name, product,serial_number) VALUES (" + device.hash + ", " + GetManufacturerID(device.manufacturer, connection) + ",'" + device.model + "','" + device.name + "','" + device.product + "','" + device.serial_number + "'); SELECT LAST_INSERT_ID();", connection);
            }
        }

        private static void Write_to_DB(LibHost.Devices.Device_CPU device, int host_id, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM device_cpu WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {                
                sql_SELECT_Execute("INSERT INTO device_cpu (device_name_hash, manufacturer_id, name, cores, clock_speed) VALUES (" + device.hash + "," + GetManufacturerID(device.manufacturer, connection) + ",'" + device.name + "'," + device.cores + "," + device.clock_speed + "); SELECT LAST_INSERT_ID();", connection);
            }
        }

        private static void Write_to_DB(LibHost.Devices.Device_RAM device, int host_id, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM device_ram WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {                
                sql_SELECT_Execute("INSERT IGNORE INTO device_ram (device_name_hash, manufacturer_id, clock_speed, memory_type, form_factor, size) VALUES (" + device.hash + "," + GetManufacturerID(device.manufacturer, connection) + "," + device.clock_speed + "," + device.memory_type + "," + device.form_factor + ", " + device.size + "); SELECT LAST_INSERT_ID();", connection);
            }
        }

        private static void Write_to_DB(LibHost.Devices.Device_HDD device, int host_id, MySqlConnection connection)
        {

            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM device_hdd WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {
                sql_SELECT_Execute("INSERT INTO device_hdd (device_name_hash, description, caption, size, free_space, file_system) VALUES (" + device.hash + ",'" + device.description + "','" + device.caption + "'," + device.size + "," + device.free_space + ", '" + device.file_system + "') ON DUPLICATE KEY UPDATE free_space=VALUES(free_space); SELECT LAST_INSERT_ID();", connection);
            }
            else
            {
                sql_SELECT_Execute("UPDATE device_hdd SET free_space=" + device.free_space + " WHERE device_name_hash=" + device.hash + ";", connection);
            }
        }

        private static void Write_to_DB(LibHost.Devices.Device_NET device, int host_id, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            int gateway_id = GetGatewayID(device.Gateway[0], connection);

            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM device_net WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {   
                sql_SELECT_Execute("INSERT IGNORE INTO device_net (device_name_hash, mac, description, gateway_id) VALUES (" + device.hash + ",'" + device.mac + "','" + device.description + "'," + gateway_id + "); SELECT LAST_INSERT_ID();", connection);
            }

            sql_SELECT_Execute("DELETE FROM net_ip_addresses WHERE mac='" + device.mac + "';", connection);
            foreach (System.Net.IPAddress iP in device.iPAddresses)
            {
                sql_SELECT_Execute("INSERT IGNORE INTO net_ip_addresses (mac, ip) VALUES ('" + device.mac + "','" + iP.ToString() + "'); SELECT LAST_INSERT_ID();", connection);
            }
            
        }

        private static void Write_to_DB(LibHost.Devices.Device_GPU device, int host_id, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM device_gpu WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {
                
                sql_SELECT_Execute("INSERT IGNORE INTO device_gpu (device_name_hash, name, memory_size) VALUES (" + device.hash + ",'" + device.name + "'," + device.memory_size + "); SELECT LAST_INSERT_ID();", connection);
            }
        }




        private static int GetManufacturerID(string name, MySqlConnection connection)
        {
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT id FROM manufacturers WHERE name='" + name + "');", connection) == "1")
            {
                return int.Parse(sql_SELECT_Execute("SELECT id FROM manufacturers WHERE name='" + name + "';", connection));
            }
            else
            {
                return int.Parse(sql_SELECT_Execute("INSERT IGNORE INTO manufacturers (name) VALUES ('" + name + "'); SELECT LAST_INSERT_ID();", connection));
            }

        }

        private static int GetDeviceID(int hash,string type, MySqlConnection connection)
        {
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT id FROM devices WHERE device_name_hash=" + hash + ");", connection) == "1")
            {
                return int.Parse(sql_SELECT_Execute("SELECT id FROM devices WHERE device_name_hash=" + hash + ";", connection));
            }
            else
            {
                return int.Parse(sql_SELECT_Execute("INSERT IGNORE INTO devices (device_name_hash, device_type) VALUES (" + hash + ", '" + type + "'); SELECT LAST_INSERT_ID();", connection));
            }

        }

        private static int GetGatewayID(System.Net.IPAddress gw, MySqlConnection connection)
        {

            if (sql_SELECT_Execute("SELECT EXISTS(SELECT id FROM net_gateways WHERE gateway='" + gw.ToString() + "');", connection) == "1")
            {
                return int.Parse(sql_SELECT_Execute("SELECT id FROM net_gateways WHERE gateway='" + gw.ToString() + "';", connection));
            }
            else
            {
                return int.Parse(sql_SELECT_Execute("INSERT IGNORE INTO net_gateways (gateway) VALUES ('" + gw.ToString() + "'); SELECT LAST_INSERT_ID();", connection));
            }
        }
    }
}
