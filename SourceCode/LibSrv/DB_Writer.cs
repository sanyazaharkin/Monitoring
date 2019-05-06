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

        public static void start_write(LibHost.Host host, MySqlConnection connection)
        {

            connection.Open();

            try
            {
                sql_SELECT_Execute("START TRANSACTION;", connection);

                #region запись в таблицу hosts
                if (sql_SELECT_Execute("SELECT EXISTS(SELECT id FROM hosts WHERE hostname='" + host.hostname + "');", connection) == "1")
                {
                    host.host_id = int.Parse(sql_SELECT_Execute("UPDATE hosts SET last_update_time='" + DateTime.Now + "';SELECT id FROM hosts WHERE hostname='" + host.hostname + "';", connection));
                }
                else
                {
                    host.host_id = int.Parse(sql_SELECT_Execute("INSERT INTO hosts (hostname, operating_system, bios_version, state, last_update_time) VALUES ('" + host.hostname + "', " + GetOperatingSystemID(host.os_version,connection) + ", '" + host.bios_version + "', " + host.state + ", '" + DateTime.Now + "') ON DUPLICATE KEY UPDATE last_update_time=VALUES(last_update_time); SELECT LAST_INSERT_ID();", connection));
                }
                #endregion


                #region запись информации о железе
                foreach (LibHost.Device item in host.Devices)
                {
                    switch (item.device_type.ToUpper())
                    {
                        case ("MB"):
                            Write_Device_to_DB((LibHost.Devices.Device_MB)item, connection);
                            break;
                        case ("CPU"):
                            Write_Device_to_DB((LibHost.Devices.Device_CPU)item, connection);
                            break;
                        case ("RAM"):
                            Write_Device_to_DB((LibHost.Devices.Device_RAM)item, connection);
                            break;
                        case ("HDD"):
                            Write_Device_to_DB((LibHost.Devices.Device_HDD)item, connection);
                            break;
                        case ("NET"):
                            Write_Device_to_DB((LibHost.Devices.Device_NET)item, connection);
                            break;
                        case ("GPU"):
                            Write_Device_to_DB((LibHost.Devices.Device_GPU)item, connection);
                            break;
                    }
                }

                SearchChangeDevices(host,connection);

                sql_SELECT_Execute("DELETE FROM host_devices WHERE host_id=" + host.host_id + ";", connection); // вынести потом это в другое место

                foreach (LibHost.Device item in host.Devices)
                {
                    sql_SELECT_Execute("INSERT INTO host_devices (device_id,host_id) VALUES (" + item.id + "," + host.host_id + "); SELECT LAST_INSERT_ID();", connection); // вынести потом это в другое место
                }





                #endregion

 
                #region запись информации о процессах

                sql_SELECT_Execute("DELETE FROM host_processes WHERE host_id=" + host.host_id + ";", connection);
                foreach (LibHost.Process process in host.Processes)
                {
                    Write_Process_to_DB(process, connection);
                    sql_SELECT_Execute("INSERT INTO host_processes (host_id, process_id) VALUES (" + host.host_id + ", " + process.process_id + ");", connection);
                }

                #endregion

    
                #region запись информации о программах

                foreach (LibHost.Program program in host.Programs)
                {
                    Write_Programm_to_DB(program, connection);                    
                }


                SearchChangePrograms(host, connection);

                sql_SELECT_Execute("DELETE FROM host_programs WHERE host_id=" + host.host_id + ";", connection);
                foreach (LibHost.Program program in host.Programs)
                {                    
                    sql_SELECT_Execute("INSERT INTO host_programs (host_id, program_id) VALUES (" + host.host_id + ", " + program.program_id + ");", connection);
                }




                #endregion

                UpdateHostState(host, connection);

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

        private static MySqlDataReader Get_Table_From_DB(string query,MySqlConnection connection)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            MySqlDataReader reader = command.ExecuteReader();
            return reader;
        }


        private static void Write_Device_to_DB(LibHost.Devices.Device_MB device, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM device_mb WHERE device_name_hash=" + device.hash + ");", connection)=="0")
            {                                   
                sql_SELECT_Execute("INSERT INTO device_mb (device_name_hash, manufacturer_id,model, name, product,serial_number) VALUES (" + device.hash + ", " + GetManufacturerID(device.manufacturer, connection) + ",'" + device.model + "','" + device.name + "','" + device.product + "','" + device.serial_number + "'); SELECT LAST_INSERT_ID();", connection);
            }
        }

        private static void Write_Device_to_DB(LibHost.Devices.Device_CPU device, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM device_cpu WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {                
                sql_SELECT_Execute("INSERT INTO device_cpu (device_name_hash, manufacturer_id, name, cores, clock_speed) VALUES (" + device.hash + "," + GetManufacturerID(device.manufacturer, connection) + ",'" + device.name + "'," + device.cores + "," + device.clock_speed + "); SELECT LAST_INSERT_ID();", connection);
            }
        }

        private static void Write_Device_to_DB(LibHost.Devices.Device_RAM device, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM device_ram WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {                
                sql_SELECT_Execute("INSERT IGNORE INTO device_ram (device_name_hash, manufacturer_id, clock_speed, memory_type, form_factor, size) VALUES (" + device.hash + "," + GetManufacturerID(device.manufacturer, connection) + "," + device.clock_speed + "," + device.memory_type + "," + device.form_factor + ", " + device.size + "); SELECT LAST_INSERT_ID();", connection);
            }
        }

        private static void Write_Device_to_DB(LibHost.Devices.Device_HDD device, MySqlConnection connection)
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

        private static void Write_Device_to_DB(LibHost.Devices.Device_NET device, MySqlConnection connection)
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

        private static void Write_Device_to_DB(LibHost.Devices.Device_GPU device, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM device_gpu WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {
                
                sql_SELECT_Execute("INSERT IGNORE INTO device_gpu (device_name_hash, name, memory_size) VALUES (" + device.hash + ",'" + device.name + "'," + device.memory_size + "); SELECT LAST_INSERT_ID();", connection);
            }
        }

        private static void Write_Process_to_DB(LibHost.Process process, MySqlConnection connection)
        {
            
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT id FROM processes WHERE name='" + process.name + "');", connection) == "1")
            {
                process.process_id = int.Parse(sql_SELECT_Execute("SELECT id FROM processes WHERE name='" + process.name + "';", connection));
            }
            else
            {
                process.process_id = int.Parse(sql_SELECT_Execute("INSERT INTO processes (name) VALUES ('" + process.name + "'); SELECT LAST_INSERT_ID();", connection));
            }

        }
        
        private static void Write_Programm_to_DB(LibHost.Program programm, MySqlConnection connection)
        {
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT id FROM programs WHERE name_version_hash=" + programm.hash + ");", connection) == "1")
            {
                programm.program_id = int.Parse(sql_SELECT_Execute("SELECT id FROM programs WHERE name_version_hash=" + programm.hash + ";", connection));
            }
            else
            {
                programm.program_id = int.Parse(sql_SELECT_Execute("INSERT INTO programs (name_version_hash, name, version, vendor_id) VALUES (" + programm.hash + ",'" + programm.name + "','" + programm.version + "', " + GetVendorID(programm.vendor, connection) + " ); SELECT LAST_INSERT_ID();", connection));
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

        private static int GetVendorID(string vendor, MySqlConnection connection)
        {

            if (sql_SELECT_Execute("SELECT EXISTS(SELECT id FROM vendors WHERE vendor='" + vendor + "');", connection) == "1")
            {
                return int.Parse(sql_SELECT_Execute("SELECT id FROM vendors WHERE vendor='" + vendor + "';", connection));
            }
            else
            {
                return int.Parse(sql_SELECT_Execute("INSERT INTO vendors (vendor) VALUES ('" + vendor + "'); SELECT LAST_INSERT_ID();", connection));
            }
        }

        private static int GetOperatingSystemID(string os_version, MySqlConnection connection)
        {

            if (sql_SELECT_Execute("SELECT EXISTS (SELECT id FROM operating_systems WHERE system='" + os_version + "');", connection) == "1")
            {
                return int.Parse(sql_SELECT_Execute("SELECT id FROM operating_systems WHERE system='" + os_version + "';", connection));
            }
            else
            {
                return int.Parse(sql_SELECT_Execute("INSERT INTO operating_systems(system) VALUES ('" + os_version + "'); SELECT LAST_INSERT_ID();", connection));
            }
        }



        private static void SearchChangeDevices(LibHost.Host host, MySqlConnection connection)
        {
            List<int> oldDeviceHash = new List<int>();
            List<int> newDeviceHash = new List<int>();

            MySqlDataReader reader = Get_Table_From_DB("SELECT device_name_hash FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id=" + host.host_id + ");", connection);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    oldDeviceHash.Add(reader.GetInt32("device_name_hash"));
                }
            }

            reader.Close();


            foreach (LibHost.Device item in host.Devices)
            {
                newDeviceHash.Add(item.hash);
            }

            List<int> mountedDevices = newDeviceHash.Except(oldDeviceHash).ToList();
            List<int> unmountedDevices = oldDeviceHash.Except(newDeviceHash).ToList();

            foreach (int item in mountedDevices)
            {
                sql_SELECT_Execute("INSERT INTO host_device_history (host_id, device_id,action,looked,date) VALUES (" + host.host_id + "," + sql_SELECT_Execute("SELECT id FROM devices WHERE device_name_hash=" + item + "; ", connection).ToString() + ", 1, 0, '" + DateTime.Now + "');", connection);
            }

            foreach (int item in unmountedDevices)
            {
                sql_SELECT_Execute("INSERT INTO host_device_history (host_id, device_id,action,looked,date) VALUES (" + host.host_id + "," + sql_SELECT_Execute("SELECT id FROM devices WHERE device_name_hash=" + item + "; ", connection).ToString() + ", 0, 0, '" + DateTime.Now + "');", connection);
            }

        }

        private static void SearchChangePrograms(LibHost.Host host, MySqlConnection connection)
        {
            List<int> oldProgramHash = new List<int>();
            List<int> newProgramHash = new List<int>();

            MySqlDataReader reader = Get_Table_From_DB("SELECT name_version_hash FROM programs WHERE id IN (SELECT program_id FROM host_programs WHERE host_id=" + host.host_id + ");", connection);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    oldProgramHash.Add(reader.GetInt32("name_version_hash"));
                }
            }

            reader.Close();


            foreach (LibHost.Program item in host.Programs)
            {
                newProgramHash.Add(item.hash);
            }

            List<int> installedPrograms = newProgramHash.Except(oldProgramHash).ToList();
            List<int> uninstalledPrograms = oldProgramHash.Except(newProgramHash).ToList();

            foreach (int item in installedPrograms)
            {
                string program_id = "0";
                if (sql_SELECT_Execute("SELECT EXISTS(SELECT id FROM programs WHERE name_version_hash=" + item + "); ", connection) == "1")
                {
                    program_id = sql_SELECT_Execute("SELECT id FROM programs WHERE name_version_hash=" + item + "; ", connection);
                }
                else
                {

                        Write_Programm_to_DB(host.Programs.Find(x => x.hash == item), connection);
                    
                }

                sql_SELECT_Execute("INSERT INTO host_program_history (host_id, program_id,action,looked,date) VALUES (" + host.host_id + "," + program_id + ", 1, 0, '" + DateTime.Now + "');", connection);


            }

            foreach (int item in uninstalledPrograms)
            {
                sql_SELECT_Execute("INSERT INTO host_program_history (host_id, program_id,action,looked,date) VALUES (" + host.host_id + "," + sql_SELECT_Execute("SELECT id FROM programs WHERE name_version_hash=" + item + "; ", connection).ToString() + ", 0, 0, '" + DateTime.Now + "');", connection);
            }

        }


        private static void UpdateHostState(LibHost.Host host, MySqlConnection connection)
        {
            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM host_device_history WHERE looked = 0 AND host_id=" + host.host_id + "); ", connection) == "1")
            {
                sql_SELECT_Execute("UPDATE hosts SET state=2 WHERE id = " + host.host_id + "; ", connection);
            }

            if (sql_SELECT_Execute("SELECT EXISTS(SELECT * FROM host_program_history WHERE looked = 0 AND host_id=" + host.host_id + "); ", connection) == "1")
            {
                sql_SELECT_Execute("UPDATE hosts SET state=3 WHERE id = " + host.host_id + "; ", connection);
            }
        }

    }
}
