using System;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace MonitoringInterface
{
    public partial class HostForm : Form
    {
        readonly int host_id;
        readonly string hostname;
        readonly HostsForm parentForm;
        public HostForm()
        {
            InitializeComponent();
        }

        public HostForm(int id, string name, HostsForm parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            host_id = id;
            hostname = name;
            this.FormClosed += Form_closed;
            
            Text = "Информация об узле " + hostname;
        }

        private void Form_closed(object sender, EventArgs e) //при закрытии формы
        {
            parentForm.Show(); //отрисовать родительскую форму
        }

        private void HostForm_Load(object sender, EventArgs e) //после загрузки формы 
        {
            UpdateForm(); //обновляем ее
        }

        private void UpdateForm() //метод обновляющий все элементы на форме
        {
            UpdateDeviceTree();
            UpdateProgramTree();
            UpdateDeviceHistorysGrid();
            UpdateProgramHistorysGrid();
            UpdateProcessLabel();
        }

        private void UpdateDeviceTree()//обновление дерева с устройствами
        {
            DevicesTree.Nodes.Clear(); //очистка дерева

            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'MB');", 1)[0][0] == "1") // делаем запрос в базу, если есть материнки
            {
                DevicesTree.Nodes.Add("MB", "Материнская плата"); // и если есть устройства соответствующекго типа добавляем ветку

                //и в добавленную ветку поштучно добавляем все устройства такого типа
                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, manufacturer, model ,name ,product, serial_number FROM device_mb WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='MB' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "));", 6))
                {

                    string result = string.Empty;

                    string manufaturer = row[1] != "-1" ? row[1] : "Не известно";
                    string model = row[2] != "-1" ? row[2] : "Не известно";
                    string name = row[3] != "-1" ? row[3] : "Не известно";
                    string product = row[4] != "-1" ? row[4] : "Не известно";
                    string serial_number = row[5] != "-1" ? row[5] : "Не известно";

                    result = string.Format("Производитель: {0}" +
                        "  Model: {1}" +
                        "  Name: {2}" +
                        "  Product: {3}" +
                        "  Serial number: {4}", manufaturer, model, name, product, serial_number);


                    DevicesTree.Nodes["MB"].Nodes.Add(row[0], result);

                }
            }
            //остальные куски кода работают аналогично
            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'CPU');", 1)[0][0] == "1")
            {
                DevicesTree.Nodes.Add("CPU", "Процессор");

                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, manufacturer, name ,cores ,clock_speed FROM device_cpu WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='CPU' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "))", 5))
                {
                    string result = string.Empty;

                    string manufaturer = row[1] != "-1" ? row[1] : "Не известно";
                    string name = row[2] != "-1" ? row[2] : "Не известно";
                    string cores = row[3] != "-1" ? row[3] : "Не известно";
                    string clock_speed = row[4] != "-1" ? row[4] : "Не известно";

                    result = string.Format("Производитель: {0}" +
                        "  Model: {1}" +
                        "  Cores: {2}" +
                        "  Clock Speed: {3} MHz", manufaturer, name, cores, clock_speed);


                    DevicesTree.Nodes["CPU"].Nodes.Add(row[0], result);

                }
            }
            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'RAM');", 1)[0][0] == "1")
            {
                DevicesTree.Nodes.Add("RAM", "Память");

                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, manufacturer, clock_speed, memory_type, form_factor,size FROM device_ram WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='RAM' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "))", 6))
                {
                    string result = string.Empty;

                    string manufaturer = row[1] != "-1" ? row[1] : "Не известно";
                    string clock_speed = row[2] != "-1" ? row[2] : "Не известно";
                    string memory_type = row[3] != "-1" ? row[3] : "Не известно";
                    string form_factor = row[4] != "-1" ? row[4] : "Не известно";
                    string size = row[5] != "-1" ? (ulong.Parse(row[5]) / 1024 / 1024).ToString() : "Не известно";



                    result = string.Format("Производитель: {0}" +
                        "  Clock_Speed: {1}MHz" +
                        "  Memory_Type: {2}" +
                        "  Form_Factor: {3}" +
                        "  Size: {4}MB", manufaturer, clock_speed, memory_type, form_factor, size);


                    DevicesTree.Nodes["RAM"].Nodes.Add(row[0], result);

                }
            }
            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'HDD');", 1)[0][0] == "1")
            {
                DevicesTree.Nodes.Add("HDD", "Хранилище");

                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, description, caption, size,free_space, file_system FROM device_hdd WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='HDD' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "))", 6))
                {
                    string result = string.Empty;

                    string description = row[1] != "-1" ? row[1] : "Не известно";
                    string caption = row[2] != "-1" ? row[2] : "Не известно";
                    double size = double.Parse(row[3]) / 1024 / 1024 / 1024;
                    double free_space = double.Parse(row[4]) / 1024 / 1024 / 1024;
                    string File_system = row[5] != "-1" ? row[5] : "Не известно";



                    result = string.Format("  description: {0}" +
                        "  caption: {1}" +
                        "  size: {2:#.##} GB" +
                        "  free_space: {3:#.##} GB" +
                        "  File_system: {4}", description, caption, size, free_space, File_system);


                    DevicesTree.Nodes["HDD"].Nodes.Add(row[0], result);

                }
            }
            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'NET');", 1)[0][0] == "1")
            {
                DevicesTree.Nodes.Add("NET", "Сетевые адаптеры");

                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, mac, description, gateway  FROM device_net WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='NET' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "))", 4))
                {
                    string result = string.Empty;

                    string mac = row[1] != "-1" ? row[1] : "Не известно";
                    string description = row[2] != "-1" ? row[2] : "Не известно";
                    string gateway_id = row[3] != "-1" ? row[3] : "Не известно";

                    result = string.Format("" +
                        "  mac: {0}" +
                        "  description: {1}" +
                        "  gateway: {2}", mac, description, gateway_id);

                    DevicesTree.Nodes["NET"].Nodes.Add(row[0], result);

                    foreach (string[] ip in parentForm.GetTableFromDB("SELECT ip FROM net_ip_addresses WHERE mac='" + row[1] + "';", 1))
                    {
                        DevicesTree.Nodes["NET"].Nodes[row[0]].Nodes.Add(ip[0], "Сетевой адресс: " + ip[0]);
                    }


                }
            }
            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'HDD');", 1)[0][0] == "1")
            {
                DevicesTree.Nodes.Add("GPU", "Видеоадаптер");

                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, name, memory_size FROM device_gpu WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='GPU' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "))", 3))
                {
                    string result = string.Empty;

                    string name = row[1] != "-1" ? row[1] : "Не известно";
                    double memory_size = double.Parse(row[2]) / 1024 / 1024;



                    result = string.Format("" +
                        "  name: {0}" +
                        "  memory_size: {1:#,##}MB", name, memory_size);


                    DevicesTree.Nodes["GPU"].Nodes.Add(row[0], result);

                }
            }

            DevicesTree.Update(); //обновляем дерево
        }
        private void UpdateProgramTree() //обновление дерева с установленными программами
        {
            ProgramTree.Nodes.Clear(); //очистка дерева

            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT * FROM host_programs WHERE host_id=" + host_id + ");", 1)[0][0] == "1") // если у узла есть программы
            {
                foreach (string[] vendors in parentForm.GetTableFromDB("SELECT DISTINCT vendor FROM programs WHERE id IN (SELECT program_id FROM host_programs WHERE host_id=" + host_id + ");", 1))
                {
                    //то поочереди добавляем вектки с производителями
                    ProgramTree.Nodes.Add(vendors[0], vendors[0]);

                    foreach (string[] programs in parentForm.GetTableFromDB("SELECT id, name, version FROM programs WHERE vendor='" + vendors[0] + "' AND id IN(SELECT program_id FROM host_programs WHERE host_id=" + host_id + ");", 3))
                    {
                        //а в эти ветки добавляем ветки с программами от этих производителей
                        ProgramTree.Nodes[vendors[0]].Nodes.Add(programs[0], programs[1] + " Version: " + programs[2]);
                    }
                }
            }

            ProgramTree.Update(); //перерисовка дерева
        }
        private void UpdateDeviceHistorysGrid() //метод который обновляет таблицу с изменениями в составе железок
        {
            DevicesHistoryGrid.Rows.Clear();

            foreach (string[] row in parentForm.GetTableFromDB("SELECT host_id, device_id, action, looked, date FROM host_device_history WHERE host_id=" + host_id + " ORDER BY id DESC;", 5))
            {
                row[0] = parentForm.GetTableFromDB("SELECT hostname FROM hosts WHERE id = " + row[0] + ";", 1)[0][0];


                string device_type = parentForm.GetTableFromDB("SELECT device_type FROM devices WHERE id = " + row[1] + ";", 1)[0][0];

                switch (device_type)
                {
                    case ("MB"):
                        row[1] = parentForm.GetTableFromDB("SELECT product FROM device_mb WHERE device_name_hash IN(SELECT device_name_hash FROM devices WHERE id = " + row[1] + ");", 1)[0][0];
                        break;

                    case ("CPU"):
                        row[1] = parentForm.GetTableFromDB("SELECT name FROM device_cpu WHERE device_name_hash IN(SELECT device_name_hash FROM devices WHERE id = " + row[1] + ");", 1)[0][0];

                        break;

                    case ("RAM"):
                        row[1] = "Модуль памяти";
                        break;

                    case ("HDD"):
                        row[1] = parentForm.GetTableFromDB("SELECT caption FROM device_hdd WHERE device_name_hash IN(SELECT device_name_hash FROM devices WHERE id = " + row[1] + ");", 1)[0][0];

                        break;

                    case ("NET"):
                        row[1] = parentForm.GetTableFromDB("SELECT description FROM device_net WHERE device_name_hash IN(SELECT device_name_hash FROM devices WHERE id = " + row[1] + ");", 1)[0][0];

                        break;

                    case ("GPU"):
                        row[1] = parentForm.GetTableFromDB("SELECT name FROM device_gpu WHERE device_name_hash IN(SELECT device_name_hash FROM devices WHERE id = " + row[1] + ");", 1)[0][0];

                        break;

                }

                row[2] = row[2] == "1" ? "Смонтированно" : "Демонтированно";
                row[3] = row[3] == "1" ? "Да" : "Нет";


                DevicesHistoryGrid.Rows.Add(row);
            }
            DevicesHistoryGrid.Update();
        }

        private void UpdateProgramHistorysGrid()//метод который обновляет таблицу с изменениями в составе ПО
        {
            ProgramsHistoryGrid.Rows.Clear();

            foreach (string[] row in parentForm.GetTableFromDB("SELECT host_id, program_id, action, looked, date FROM host_program_history WHERE host_id=" + host_id + " ORDER BY id DESC;", 5))
            {
                row[0] = parentForm.GetTableFromDB("SELECT hostname FROM hosts WHERE id = " + row[0] + ";", 1)[0][0];
                row[1] = parentForm.GetTableFromDB("SELECT name FROM programs WHERE id = " + row[1] + ";", 1)[0][0];


                row[2] = row[2] == "1" ? "Установленно" : "Удалено";
                row[3] = row[3] == "1" ? "Да" : "Нет";


                ProgramsHistoryGrid.Rows.Add(row);
            }
            ProgramsHistoryGrid.Update();
        }

        private void UpdateProcessLabel() //обновляем список процессов
        {            
            listView1.Clear();
            foreach (string[] process in  parentForm.GetTableFromDB("SELECT name FROM processes WHERE id IN (SELECT process_id FROM host_processes WHERE host_id=" + host_id + ");", 1))
            {
                listView1.Items.Add( process[0] + "\n");
            }


            listView1.Update();
        }



        private void UpdateButton_Click(object sender, EventArgs e) //обработчик нажатия на кнопку обновить
        {
            UpdateForm(); //обновляем форму
        }

        private void DeleteHostButton_Click(object sender, EventArgs e) //удаление информации об узле из БД
        {
            parentForm.GetTableFromDB("DELETE FROM hosts WHERE id = " + host_id + "",1); //запрос в БД
            parentForm.UpdateHostsGrid(); //обновление таблицы на главной форме
            Close(); //закрываем текущую форму            
        }

        private void SetLookedButton1_Click(object sender, EventArgs e) //нажатие на кнопуку пометить как просмотренное
        {
            parentForm.GetTableFromDB("UPDATE host_device_history SET looked = 1 WHERE host_id = " + host_id + "", 1);
            parentForm.GetTableFromDB("UPDATE hosts SET state = 'Без изменений' WHERE id = " + host_id + "", 1);
            parentForm.UpdateHostsGrid();
            UpdateForm();
            
        }

        private void SetLookedButton2_Click(object sender, EventArgs e)//нажатие на кнопуку пометить как просмотренное
        {
            parentForm.GetTableFromDB("UPDATE host_program_history SET looked = 1 WHERE host_id = " + host_id + "", 1);
            parentForm.GetTableFromDB("UPDATE hosts SET state = 'Без изменений' WHERE id = " + host_id + "", 1);
            parentForm.UpdateHostsGrid();
            UpdateForm();
        }

        private void UploadReport() //метод выгружающий отчет
        {
            string report_path = ChooseFolder(); //получаем путь к месту сохранения файла, с помошью диалогового окна
            List<string> report = GenerateReport(); // наполняем список(отчет) строками 
            try
            {
                File.WriteAllLines(Path.Combine(report_path, hostname + ".txt"), report); //сохраняем отчет в файл
            }
            catch (Exception ex)  //ловим исключение
            {
                MessageBox.Show(ex.Message); //вывод окна с текстом исключения
            }            
        }


        private string ChooseFolder() //метод открывающий окно для выбора пути сохранения
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog1.SelectedPath;
            }
            else
            {
                return @"C:\\";
            }
        }


        private void Upload_Report_button_Click(object sender, EventArgs e) //обработчик нажатия на кнопку
        {
            UploadReport();//вызываем метод
        }

        private List<string> GenerateReport() //метод собирающий отчет из БД
        {
            List<string> result = new List<string>
            {
                "===================Общая информация==================="
            };
            foreach (string[] item in parentForm.GetTableFromDB("SELECT hostname, operating_system, bios_version FROM hosts WHERE id=" + host_id + ";", 3))
            {
                result.Add("Узел: " + item[0]);
                result.Add("Операционная система: " + item[1]);
                result.Add("Версия BIOS: " + item[2]);
            }

            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'MB');", 1)[0][0] == "1")
            {
                result.Add("===================Материнская плата===================");
                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, manufacturer, model ,name ,product, serial_number FROM device_mb WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='MB' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "));", 6))
                {
                    string manufaturer = row[1] != "-1" ? row[1] : "Не известно";
                    string model = row[2] != "-1" ? row[2] : "Не известно";
                    string name = row[3] != "-1" ? row[3] : "Не известно";
                    string product = row[4] != "-1" ? row[4] : "Не известно";
                    string serial_number = row[5] != "-1" ? row[5] : "Не известно";

                    result.Add(string.Format("Производитель: {0}" +
                        "  Model: {1}" +
                        "  Name: {2}" +
                        "  Product: {3}" +
                        "  Serial number: {4}", manufaturer, model, name, product, serial_number));
                }
            }
            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'CPU');", 1)[0][0] == "1")
            {
                result.Add("===================Процессор===================");

                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, manufacturer, name ,cores ,clock_speed FROM device_cpu WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='CPU' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "))", 5))
                {
                    string manufaturer = row[1] != "-1" ? row[1] : "Не известно";
                    string name = row[2] != "-1" ? row[2] : "Не известно";
                    string cores = row[3] != "-1" ? row[3] : "Не известно";
                    string clock_speed = row[4] != "-1" ? row[4] : "Не известно";

                    result.Add(string.Format("Производитель: {0}" +
                        "  Model: {1}" +
                        "  Cores: {2}" +
                        "  Clock Speed: {3} MHz", manufaturer, name, cores, clock_speed));
                }
            }
            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'RAM');", 1)[0][0] == "1")
            {
                result.Add("===================Память===================");

                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, manufacturer, clock_speed, memory_type, form_factor,size FROM device_ram WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='RAM' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "))", 6))
                {
                    string manufaturer = row[1] != "-1" ? row[1] : "Не известно";
                    string clock_speed = row[2] != "-1" ? row[2] : "Не известно";
                    string memory_type = row[3] != "-1" ? row[3] : "Не известно";
                    string form_factor = row[4] != "-1" ? row[4] : "Не известно";
                    string size = row[5] != "-1" ? (ulong.Parse(row[5]) / 1024 / 1024).ToString() : "Не известно";



                    result.Add(string.Format("Производитель: {0}" +
                        "  Clock_Speed: {1}MHz" +
                        "  Memory_Type: {2}" +
                        "  Form_Factor: {3}" +
                        "  Size: {4}MB", manufaturer, clock_speed, memory_type, form_factor, size));
                }
            }
            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'HDD');", 1)[0][0] == "1")
            {
                result.Add("===================Хранилище===================");

                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, description, caption, size,free_space, file_system FROM device_hdd WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='HDD' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "))", 6))
                {
                    string description = row[1] != "-1" ? row[1] : "Не известно";
                    string caption = row[2] != "-1" ? row[2] : "Не известно";
                    double size = double.Parse(row[3]) / 1024 / 1024 / 1024;
                    double free_space = double.Parse(row[4]) / 1024 / 1024 / 1024;
                    string File_system = row[5] != "-1" ? row[5] : "Не известно";



                    result.Add(string.Format("  description: {0}" +
                        "  caption: {1}" +
                        "  size: {2:#.##} GB" +
                        "  free_space: {3:#.##} GB" +
                        "  File_system: {4}", description, caption, size, free_space, File_system));
                }
            }
            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'NET');", 1)[0][0] == "1")
            {
                result.Add("===================Сетевые адаптеры===================");

                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, mac, description, gateway  FROM device_net WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='NET' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "))", 4))
                {
                    string mac = row[1] != "-1" ? row[1] : "Не известно";
                    string description = row[2] != "-1" ? row[2] : "Не известно";
                    string gateway_id = row[3] != "-1" ? row[3] : "Не известно";

                    result.Add(string.Format("" +
                        "  mac: {0}" +
                        "  description: {1}" +
                        "  gateway: {2}", mac, description, gateway_id));

                    foreach (string[] ip in parentForm.GetTableFromDB("SELECT ip FROM net_ip_addresses WHERE mac='" + row[1] + "';", 1))
                    {
                        result.Add("Сетевой адресс: " + ip[0]);
                    }


                }
            }
            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT device_type FROM devices WHERE id IN (SELECT device_id FROM host_devices WHERE host_id = " + host_id + ") AND device_type = 'HDD');", 1)[0][0] == "1")
            {
                result.Add("===================Видеоадаптер===================");

                foreach (string[] row in parentForm.GetTableFromDB("SELECT device_name_hash, name, memory_size FROM device_gpu WHERE device_name_hash IN (SELECT device_name_hash FROM devices WHERE device_type='GPU' AND id IN(SELECT device_id FROM host_devices WHERE host_id=" + host_id + "))", 3))
                {
                    string name = row[1] != "-1" ? row[1] : "Не известно";
                    double memory_size = double.Parse(row[2]) / 1024 / 1024;


                    result.Add(string.Format("" +
                        "  name: {0}" +
                        "  memory_size: {1:#,##}MB", name, memory_size));
                }
            }

            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT * FROM host_programs WHERE host_id=" + host_id + ");", 1)[0][0] == "1")
            {
                result.Add("===================Установленное ПО===================");

                foreach (string[] vendors in parentForm.GetTableFromDB("SELECT DISTINCT vendor FROM programs WHERE id IN (SELECT program_id FROM host_programs WHERE host_id=" + host_id + ");", 1))
                {
                    foreach (string[] programs in parentForm.GetTableFromDB("SELECT id, name, version FROM programs WHERE vendor='" + vendors[0] + "' AND id IN(SELECT program_id FROM host_programs WHERE host_id=" + host_id + ");", 3))
                    {
                        result.Add(programs[1] + " Version: " + programs[2]);
                    }
                }
            }


            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT * FROM host_device_history WHERE host_id=" + host_id + ");", 1)[0][0] == "1")
            {
                result.Add(("===================История изменений списка оборудования==================="));
                foreach (string[] row in parentForm.GetTableFromDB("SELECT host_id, device_id, action, looked, date FROM host_device_history WHERE host_id=" + host_id + " ORDER BY id DESC;", 5))
                {
                    row[0] = parentForm.GetTableFromDB("SELECT hostname FROM hosts WHERE id = " + row[0] + ";", 1)[0][0];


                    string device_type = parentForm.GetTableFromDB("SELECT device_type FROM devices WHERE id = " + row[1] + ";", 1)[0][0];

                    switch (device_type)
                    {
                        case ("MB"):
                            row[1] = parentForm.GetTableFromDB("SELECT product FROM device_mb WHERE device_name_hash IN(SELECT device_name_hash FROM devices WHERE id = " + row[1] + ");", 1)[0][0];
                            break;

                        case ("CPU"):
                            row[1] = parentForm.GetTableFromDB("SELECT name FROM device_cpu WHERE device_name_hash IN(SELECT device_name_hash FROM devices WHERE id = " + row[1] + ");", 1)[0][0];

                            break;

                        case ("RAM"):
                            row[1] = "Модуль памяти";
                            break;

                        case ("HDD"):
                            row[1] = parentForm.GetTableFromDB("SELECT caption FROM device_hdd WHERE device_name_hash IN(SELECT device_name_hash FROM devices WHERE id = " + row[1] + ");", 1)[0][0];

                            break;

                        case ("NET"):
                            row[1] = parentForm.GetTableFromDB("SELECT description FROM device_net WHERE device_name_hash IN(SELECT device_name_hash FROM devices WHERE id = " + row[1] + ");", 1)[0][0];

                            break;

                        case ("GPU"):
                            row[1] = parentForm.GetTableFromDB("SELECT name FROM device_gpu WHERE device_name_hash IN(SELECT device_name_hash FROM devices WHERE id = " + row[1] + ");", 1)[0][0];

                            break;

                    }

                    row[2] = row[2] == "1" ? "Смонтированно" : "Демонтированно";
                    row[3] = row[3] == "1" ? "Да" : "Нет";


                   result.Add("host_id "+ row[0] +", device_id "+ row[1] +", Действие " + row[2] + ", date " + row[4] );
                }
            }



            if (parentForm.GetTableFromDB("SELECT EXISTS(SELECT * FROM host_program_history WHERE host_id=" + host_id + ");", 1)[0][0] == "1")
            {
                result.Add(("===================История изменений списка ПО==================="));

                foreach (string[] row in parentForm.GetTableFromDB("SELECT host_id, program_id, action, looked, date FROM host_program_history WHERE host_id=" + host_id + " ORDER BY id DESC;", 5))
                {
                    row[0] = parentForm.GetTableFromDB("SELECT hostname FROM hosts WHERE id = " + row[0] + ";", 1)[0][0];
                    row[1] = parentForm.GetTableFromDB("SELECT name FROM programs WHERE id = " + row[1] + ";", 1)[0][0];


                    row[2] = row[2] == "1" ? "Установленно" : "Удалено";
                    row[3] = row[3] == "1" ? "Да" : "Нет";


                    result.Add("host_id " + row[0] + ", program_id " + row[1] + ", Действие " + row[2] + ", date " + row[4]);
                }
                
            }




            return result;
        }


    }
}
