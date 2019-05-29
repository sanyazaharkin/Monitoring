using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace LibSrv
{
    public class DB_Writer 
    {       

        public static void Start_write(LibHost.Host host, MySqlConnection connection) //статический метод который выполняет запись информации о хосте в БД
        {

            connection.Open(); //открываем подключение к БД

            try
            {
                Sql_Query_Execute("START TRANSACTION;", connection); //начинаем тразакцию

                #region запись в таблицу hosts
                if (Sql_Query_Execute("SELECT EXISTS(SELECT * FROM hosts WHERE hostname='" + host.hostname + "');", connection) == "1")  //вот здесь проверям наличие записи об узле в БД
                {
                    //если есть то запрашиваем ID и обновляем время последнего обновления
                    host.host_id = int.Parse(Sql_Query_Execute("UPDATE hosts SET last_update_time='" + DateTime.Now + "' WHERE hostname='" + host.hostname + "' ;SELECT id FROM hosts WHERE hostname='" + host.hostname + "';", connection));
                }
                else
                {
                    //если нет то добавляем запись в БД
                    host.host_id = int.Parse(Sql_Query_Execute("INSERT INTO hosts (hostname, operating_system, bios_version, state, last_update_time) VALUES ('" + host.hostname + "', '" + host.os_version + "', '" + host.bios_version + "', 'Без изменений', '" + DateTime.Now + "'); SELECT LAST_INSERT_ID();", connection));
                }
                #endregion


                #region запись информации о железе
                foreach (LibHost.Device item in host.Devices) //перебираем список устройств на узле
                {
                    switch (item.device_type.ToUpper())//смотрим на тип устройства
                    {
                        case ("MB"):
                            Write_Device_to_DB((LibHost.Devices.Device_MB)item, connection); //если материнка то вызываем метод который пишет информацию о материнке в БД
                            break;
                        case ("CPU"):
                            Write_Device_to_DB((LibHost.Devices.Device_CPU)item, connection);//все остальное аналогично
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

                SearchChangeDevices(host,connection); // метод который ищет разницу в установленных железках 

                Sql_Query_Execute("DELETE FROM host_devices WHERE host_id=" + host.host_id + ";", connection); //очищаем список устройств у хоста

                foreach (LibHost.Device item in host.Devices) //наполняем его заново
                {
                    Sql_Query_Execute("INSERT INTO host_devices (device_id,host_id) VALUES (" + item.id + "," + host.host_id + "); SELECT LAST_INSERT_ID();", connection); 
                }





                #endregion

 
                #region запись информации о процессах

                Sql_Query_Execute("DELETE FROM host_processes WHERE host_id=" + host.host_id + ";", connection); //очищаем список процессов у хоста
                foreach (LibHost.Process process in host.Processes)// наполняем заново
                {
                    Write_Process_to_DB(process, connection);
                    Sql_Query_Execute("INSERT INTO host_processes (host_id, process_id) VALUES (" + host.host_id + ", " + process.process_id + ");", connection);
                }

                #endregion

    
                #region запись информации о программах

                foreach (LibHost.Program program in host.Programs) //перебираем список программ
                {
                    Write_Programm_to_DB(program, connection);//каждую программу пишеи в БД                    
                }


                SearchChangePrograms(host, connection); //ищем разницу в программах

                
                foreach (LibHost.Program program in host.Programs)//пишем информацию о программах на устройстве
                {                    
                    Sql_Query_Execute("INSERT INTO host_programs (host_id, program_id) VALUES (" + host.host_id + ", " + program.program_id + ");", connection);
                }




                #endregion

                UpdateHostState(host, connection); //меняем состояние узла

                Sql_Query_Execute("COMMIT;", connection); //завершаем транзакцию
            } 
            catch (Exception ex) //ловим возможное исключение
            {                
                Work.SendMSG(ex.Message); //шлем сообшение с текстом исключения
            }
            finally
            {
                if (connection != null & connection.State == System.Data.ConnectionState.Open) connection.Close(); // закрываем подключение 
            }

        }
        private static string Sql_Query_Execute(string query, MySqlConnection conn) //метод который выполняет запросы в БД
        {
            Work.SendMSG("Выполняется запрос: " + query);
            MySqlCommand command = conn.CreateCommand();
            
            command.CommandText = query;

            object answer = command.ExecuteScalar();
            Work.SendMSG("Получен ответ:" + (answer != null ? answer.ToString() : "-1"));
            return answer!=null?answer.ToString():"-1";
        }
        private static MySqlDataReader Get_Table_From_DB(string query,MySqlConnection connection) //метод для получения целой таблицы из БД
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            MySqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        private static void Write_Device_to_DB(LibHost.Devices.Device_MB device, MySqlConnection connection) //перегрузка метода для записи информации об устройстве в БД  , все перегрузки этого метода работают одинаково, только пишут в разные таблицы 
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection); //получение ID
            if (Sql_Query_Execute("SELECT EXISTS(SELECT * FROM device_mb WHERE device_name_hash=" + device.hash + ");", connection)=="0")//если нет информации о данной железке 
            {      
                //то пишем ее в БД
                Sql_Query_Execute("INSERT INTO device_mb (device_name_hash, manufacturer, model, name, product,serial_number) VALUES (" + device.hash + ", '" + device.manufacturer + "','" + device.model + "','" + device.name + "','" + device.product + "','" + device.serial_number + "'); SELECT LAST_INSERT_ID();", connection);
            }
        }
        private static void Write_Device_to_DB(LibHost.Devices.Device_CPU device, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (Sql_Query_Execute("SELECT EXISTS(SELECT * FROM device_cpu WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {                
                Sql_Query_Execute("INSERT INTO device_cpu (device_name_hash, manufacturer, name, cores, clock_speed) VALUES (" + device.hash + ",'" + device.manufacturer + "','" + device.name + "'," + device.cores + "," + device.clock_speed + "); SELECT LAST_INSERT_ID();", connection);
            }
        }
        private static void Write_Device_to_DB(LibHost.Devices.Device_RAM device, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (Sql_Query_Execute("SELECT EXISTS(SELECT * FROM device_ram WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {                
                Sql_Query_Execute("INSERT IGNORE INTO device_ram (device_name_hash, manufacturer, clock_speed, memory_type, form_factor, size) VALUES (" + device.hash + ",'" + device.manufacturer + "'," + device.clock_speed + "," + device.memory_type + "," + device.form_factor + ", " + device.size + "); SELECT LAST_INSERT_ID();", connection);
            }
        }
        private static void Write_Device_to_DB(LibHost.Devices.Device_HDD device, MySqlConnection connection)
        {

            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (Sql_Query_Execute("SELECT EXISTS(SELECT * FROM device_hdd WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {
                Sql_Query_Execute("INSERT INTO device_hdd (device_name_hash, description, caption, size, free_space, file_system) VALUES (" + device.hash + ",'" + device.description + "','" + device.caption + "'," + device.size + "," + device.free_space + ", '" + device.file_system + "') ON DUPLICATE KEY UPDATE free_space=VALUES(free_space); SELECT LAST_INSERT_ID();", connection);
            }
            else
            {
                Sql_Query_Execute("UPDATE device_hdd SET free_space=" + device.free_space + " WHERE device_name_hash=" + device.hash + ";", connection);
            }
        }
        private static void Write_Device_to_DB(LibHost.Devices.Device_NET device, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);           

            if (Sql_Query_Execute("SELECT EXISTS(SELECT * FROM device_net WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {   
                Sql_Query_Execute("INSERT IGNORE INTO device_net (device_name_hash, mac, description, gateway) VALUES (" + device.hash + ",'" + device.mac + "','" + device.description + "','" + device.Gateway[0] + "'); SELECT LAST_INSERT_ID();", connection);
            }

            Sql_Query_Execute("DELETE FROM net_ip_addresses WHERE mac='" + device.mac + "';", connection);
            foreach (System.Net.IPAddress iP in device.iPAddresses)
            {
                Sql_Query_Execute("INSERT IGNORE INTO net_ip_addresses (mac, ip) VALUES ('" + device.mac + "','" + iP.ToString() + "'); SELECT LAST_INSERT_ID();", connection);
            }
            
        }
        private static void Write_Device_to_DB(LibHost.Devices.Device_GPU device, MySqlConnection connection)
        {
            device.id = GetDeviceID(device.hash, device.device_type, connection);
            if (Sql_Query_Execute("SELECT EXISTS(SELECT * FROM device_gpu WHERE device_name_hash=" + device.hash + ");", connection) == "0")
            {
                
                Sql_Query_Execute("INSERT IGNORE INTO device_gpu (device_name_hash, name, memory_size) VALUES (" + device.hash + ",'" + device.name + "'," + device.memory_size + "); SELECT LAST_INSERT_ID();", connection);
            }
        }
        private static void Write_Process_to_DB(LibHost.Process process, MySqlConnection connection) //метод который пишет информацию о процессахз в БД
        {
            
            if (Sql_Query_Execute("SELECT EXISTS(SELECT id FROM processes WHERE name='" + process.name + "');", connection) == "1")
            {
                process.process_id = int.Parse(Sql_Query_Execute("SELECT id FROM processes WHERE name='" + process.name + "';", connection));
            }
            else
            {
                process.process_id = int.Parse(Sql_Query_Execute("INSERT INTO processes (name) VALUES ('" + process.name + "'); SELECT LAST_INSERT_ID();", connection));
            }

        }       
        private static void Write_Programm_to_DB(LibHost.Program programm, MySqlConnection connection) //метод который пишет информацию о программах в БД
        {
            if (Sql_Query_Execute("SELECT EXISTS(SELECT id FROM programs WHERE name_version_hash=" + programm.hash + ");", connection) == "1")
            {
                programm.program_id = int.Parse(Sql_Query_Execute("SELECT id FROM programs WHERE name_version_hash=" + programm.hash + ";", connection));
            }
            else
            {
                programm.program_id = int.Parse(Sql_Query_Execute("INSERT INTO programs (name_version_hash, name, version, vendor) VALUES (" + programm.hash + ",'" + programm.name + "','" + programm.version + "', '" + programm.vendor + "' ); SELECT LAST_INSERT_ID();", connection));
            }
        }
        //private static int GetManufacturerID(string name, MySqlConnection connection) //метод который пишет информацию о производителе в таблицу и возвращает полученный ID
        //{
        //    if (Sql_Query_Execute("SELECT EXISTS(SELECT id FROM manufacturers WHERE name='" + name + "');", connection) == "1")
        //    {
        //        return int.Parse(Sql_Query_Execute("SELECT id FROM manufacturers WHERE name='" + name + "';", connection));
        //    }
        //    else
        //    {
        //        return int.Parse(Sql_Query_Execute("INSERT IGNORE INTO manufacturers (name) VALUES ('" + name + "'); SELECT LAST_INSERT_ID();", connection));
        //    }

        //}
        private static int GetDeviceID(int hash,string type, MySqlConnection connection) //метода добавляюший запись в таблицу Devices и возвращающий ID 
        {
            if (Sql_Query_Execute("SELECT EXISTS(SELECT id FROM devices WHERE device_name_hash=" + hash + ");", connection) == "1")
            {
                return int.Parse(Sql_Query_Execute("SELECT id FROM devices WHERE device_name_hash=" + hash + ";", connection));
            }
            else
            {
                return int.Parse(Sql_Query_Execute("INSERT IGNORE INTO devices (device_name_hash, device_type) VALUES (" + hash + ", '" + type + "'); SELECT LAST_INSERT_ID();", connection));
            }

        }
        //private static int GetGatewayID(System.Net.IPAddress gw, MySqlConnection connection) //запись в таблицу gateways  и получение id 
        //{

        //    if (Sql_Query_Execute("SELECT EXISTS(SELECT id FROM net_gateways WHERE gateway='" + gw.ToString() + "');", connection) == "1")
        //    {
        //        return int.Parse(Sql_Query_Execute("SELECT id FROM net_gateways WHERE gateway='" + gw.ToString() + "';", connection));
        //    }
        //    else
        //    {
        //        return int.Parse(Sql_Query_Execute("INSERT IGNORE INTO net_gateways (gateway) VALUES ('" + gw.ToString() + "'); SELECT LAST_INSERT_ID();", connection));
        //    }
        //}

        //private static int GetVendorID(string vendor, MySqlConnection connection) //запись в таблицу vendors  и получение id
        //{

        //    if (Sql_Query_Execute("SELECT EXISTS(SELECT id FROM vendors WHERE vendor='" + vendor + "');", connection) == "1")
        //    {
        //        return int.Parse(Sql_Query_Execute("SELECT id FROM vendors WHERE vendor='" + vendor + "';", connection));
        //    }
        //    else
        //    {
        //        return int.Parse(Sql_Query_Execute("INSERT INTO vendors (vendor) VALUES ('" + vendor + "'); SELECT LAST_INSERT_ID();", connection));
        //    }
        //}

        //private static int GetOperatingSystemID(string os_version, MySqlConnection connection) //запись в таблицу operating_systems  и получение id
        //{

        //    if (Sql_Query_Execute("SELECT EXISTS (SELECT id FROM operating_systems WHERE system='" + os_version + "');", connection) == "1")
        //    {
        //        return int.Parse(Sql_Query_Execute("SELECT id FROM operating_systems WHERE system='" + os_version + "';", connection));
        //    }
        //    else
        //    {
        //        return int.Parse(Sql_Query_Execute("INSERT INTO operating_systems(system) VALUES ('" + os_version + "'); SELECT LAST_INSERT_ID();", connection));
        //    }
        //}
        private static void SearchChangeDevices(LibHost.Host host, MySqlConnection connection) //поиск изменений в составе устройств
        {
            List<int> oldDeviceHash = new List<int>(); //пустой список старых устройств
            List<int> newDeviceHash = new List<int>(); //пустой список новых устройств

            MySqlDataReader reader = Get_Table_From_DB("SELECT device_name_hash FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id=" + host.host_id + ");", connection); //делаем запрос старых устройств из БД

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    oldDeviceHash.Add(reader.GetInt32("device_name_hash")); //наполняем список старых устройств хешами получеными из БД
                }
            }

            reader.Close();


            foreach (LibHost.Device item in host.Devices)
            {
                newDeviceHash.Add(item.hash); //наполняем список новых устройств хешами , которые выдергиваем из списка устройств, полученного от узла
            }

            List<int> mountedDevices = newDeviceHash.Except(oldDeviceHash).ToList(); //получаем список новых устройств , путем получения разности множеств, из списка хешей новых устройств мы вычитаем список хешей старых устройств результат присваиваем переменной
            List<int> unmountedDevices = oldDeviceHash.Except(newDeviceHash).ToList(); //то же самое только в обратную сторону

            foreach (int item in mountedDevices) //перебираем список новых устройств
            {
                //о каждом пишем в БД
                Sql_Query_Execute("INSERT INTO host_device_history (host_id, device_id,action,looked,date) VALUES (" + host.host_id + "," + Sql_Query_Execute("SELECT id FROM devices WHERE device_name_hash=" + item + "; ", connection).ToString() + ", 1, 0, '" + DateTime.Now + "');", connection);
            }

            foreach (int item in unmountedDevices)  //перебираем список старых устройств
            {
                //о каждом пишем в БД
                Sql_Query_Execute("INSERT INTO host_device_history (host_id, device_id,action,looked,date) VALUES (" + host.host_id + "," + Sql_Query_Execute("SELECT id FROM devices WHERE device_name_hash=" + item + "; ", connection).ToString() + ", 0, 0, '" + DateTime.Now + "');", connection);
            }

        }
        private static void SearchChangePrograms(LibHost.Host host, MySqlConnection connection) //поиск изменений в составе программ, работает аналогично предыдущему, с небольшой разницей
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

            Sql_Query_Execute("DELETE FROM host_programs WHERE host_id=" + host.host_id + ";", connection);

            foreach (LibHost.Program item in host.Programs)
            {
                newProgramHash.Add(item.hash);
            }

            List<int> installedPrograms = newProgramHash.Except(oldProgramHash).ToList();
            List<int> uninstalledPrograms = oldProgramHash.Except(newProgramHash).ToList();

            foreach (int item in installedPrograms)
            {
                int program_id ;
                if (Sql_Query_Execute("SELECT EXISTS(SELECT id FROM programs WHERE name_version_hash=" + item + "); ", connection) == "1") //проверяем есть ли запись о такой программе в БД
                {
                    //если есть то получаем id
                    program_id = int.Parse(Sql_Query_Execute("SELECT id FROM programs WHERE name_version_hash=" + item + "; ", connection)); 
                }
                else
                {
                    //если енет то добавляем запись и получаем id
                    LibHost.Program temp = host.Programs.Find(x => x.hash == item);
                    program_id = int.Parse(Sql_Query_Execute("INSERT INTO programs (name_version_hash, name, version, vendor) VALUES (" + temp.hash + ",'" + temp.name + "','" + temp.version + "', '" + temp.vendor + "' ); SELECT LAST_INSERT_ID();", connection));
                }




                Sql_Query_Execute("INSERT INTO host_program_history (host_id, program_id,action,looked,date) VALUES (" + host.host_id + "," + program_id + ", 1, 0, '" + DateTime.Now + "');", connection);


            }

            foreach (int item in uninstalledPrograms)
            {
                Sql_Query_Execute("INSERT INTO host_program_history (host_id, program_id,action,looked,date) VALUES (" + host.host_id + "," + Sql_Query_Execute("SELECT id FROM programs WHERE name_version_hash=" + item + "; ", connection).ToString() + ", 0, 0, '" + DateTime.Now + "');", connection);
            }

        }
        private static void UpdateHostState(LibHost.Host host, MySqlConnection connection) //метод обновляющий статус устройства
        {
            if (Sql_Query_Execute("SELECT EXISTS(SELECT * FROM host_device_history WHERE looked = 0 AND host_id=" + host.host_id + "); ", connection) == "1") //если есть непрочтеные записи об изменении в списке устройств
            {
                Sql_Query_Execute("UPDATE hosts SET state='изменен набор устройств' WHERE id = " + host.host_id + "; ", connection); //то обновляем состояние на 2
            }

            if (Sql_Query_Execute("SELECT EXISTS(SELECT * FROM host_program_history WHERE looked = 0 AND host_id=" + host.host_id + "); ", connection) == "1") //если есть непрочтеные записи об изменении в списке программ
            {
                Sql_Query_Execute("UPDATE hosts SET state='изменен список программ' WHERE id = " + host.host_id + "; ", connection);//то обновляем состояние на 3
            }
        }

    }
}
