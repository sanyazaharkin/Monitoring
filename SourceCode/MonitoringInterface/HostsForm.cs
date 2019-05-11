using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Specialized;



namespace MonitoringInterface
{
    public partial class HostsForm : Form
    {
        readonly MySqlConnection db_conn; //ссылка на объект подключения к БД
        readonly int GridColumns = 6; //количество колонок в таблице 


        public HostsForm()
        {
            InitializeComponent();
        }
        public HostsForm(object sAll) //конструктор принимающий в аргументы коллекцию с настройками
        {
            InitializeComponent();
            db_conn = Get_db_conn((NameValueCollection)sAll);  //создание объекта с помощью метода  Get_db_conn которому в параметры передается коллекция с настройками (NameValueCollection)sAll
            checkBox1.Checked = true; //присвоение переменным начальных значений
            timer1.Enabled = checkBox1.Checked;
            timer1.Interval = (int)SetTimerPeriod.Value * 1000;            
        }



        public void UpdateHostsGrid()//метод который обновляет данные в таблице
        {
            HostsGrid.Rows.Clear(); //очищаем таблицу

            foreach (string[] row in GetTableFromDB("SELECT id,hostname,bios_version,operating_system,last_update_time,state FROM hosts", GridColumns)) //делаем запрос в БД
            {


                row[3] = GetTableFromDB("SELECT system FROM operating_systems WHERE id=" + row[3] + ";", 1)[0][0]; //запрашиваем данные из "вспомогательных" таблиц

                row[row.Length - 1] = GetTableFromDB("SELECT description FROM host_states WHERE id=" + row[row.Length - 1] + ";", 1)[0][0]; //запрашиваем данные из "вспомогательных" таблиц

                HostsGrid.Rows.Add(row); //добавляем строки в таблицу
            }

            for (int i = 0; i < HostsGrid.Rows.Count; i++) //перекрашиваем колонку "состояние" в нужный нам цвет
            {
                if (HostsGrid.Rows[i].Cells[HostsGrid.Rows[i].Cells.Count - 1].Value.ToString().ToLower() == "без изменений")
                {
                    HostsGrid.Rows[i].Cells[HostsGrid.Rows[i].Cells.Count - 1].Style.BackColor = System.Drawing.Color.ForestGreen; //зеленый
                }
                else if (HostsGrid.Rows[i].Cells[HostsGrid.Rows[i].Cells.Count - 1].Value.ToString().ToLower() == "Ошибка")
                {
                    HostsGrid.Rows[i].Cells[HostsGrid.Rows[i].Cells.Count - 1].Style.BackColor = System.Drawing.Color.OrangeRed; //оранжево-красный
                }
                else
                {
                    HostsGrid.Rows[i].Cells[HostsGrid.Rows[i].Cells.Count - 1].Style.BackColor = System.Drawing.Color.Yellow; //желтый
                }
            }


            HostsGrid.Update(); //просим таблицу отрисоваться заново
        }

        private MySqlConnection Get_db_conn(NameValueCollection config) //метод создающий объект подключения
        {

            string db_server_port = config["db_server_port"];
            string db_server_ip = config["db_server_ip"];
            string db_name = config["db_name"];
            string db_user = config["db_user"];
            string db_pass = config["db_pass"];
           
            return new MySqlConnection("Server=" + db_server_ip + ";Database=" + db_name + ";port=" + db_server_port + ";User Id=" + db_user + ";password=" + db_pass + ";CharSet=utf8");
        }

        public List<string[]> GetTableFromDB(string query,int Columns) //метод для получения целой таблицы
        {
            db_conn.Open();
            MySqlCommand command = db_conn.CreateCommand();
            command.CommandText = query;
            MySqlDataReader reader = command.ExecuteReader();

            List<string[]> result = new List<string[]>();
            
            while (reader.Read())
            {
                result.Add(new string[Columns]);
                for (int i = 0; i < Columns; i++)
                {
                    result[result.Count - 1][i] = reader[i].ToString();
                }
            }
            reader.Close();
            db_conn.Close();

            return result;
        }

        private void SetTimerPeriod_ValueChanged(object sender, EventArgs e) //событие изменения значения объекта  SetTimerPeriod
        {
            timer1.Interval = (int)SetTimerPeriod.Value*1000;
        }
        private void Timer1_Tick(object sender, EventArgs e) //событие возникающее в конце каждого периода таймера
        {
            UpdateHostsGrid(); //обновляем таблицу
        }

        private void HostsForm_Load(object sender, EventArgs e) //событие которое возникает посде загрузки формы
        {
            UpdateHostsGrid();  //обновляем таблицу          
        }

        private void HostsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e) //обработка события нажатия на строку в таблице
        {
            DataGridView obj = (DataGridView)sender; //downcast к необходимому типу

            HostForm hostForm = new HostForm(int.Parse(obj.Rows[e.RowIndex].Cells[0].Value.ToString()), obj.Rows[e.RowIndex].Cells[1].Value.ToString(), this); //создаем дочернюю форму , ей в конструктор передаем значения id и hostname, которые берем прямо из таблицы, и адрес на объект главной формы 
            hostForm.Show(); //просим дочернюю форму отрисоваться
            this.Hide(); //главную скрываем
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e) //событие изменения состояния чекбокса
        {
            timer1.Enabled = checkBox1.Checked; //включаем или выключаем таймер в зависимости от состояния чекбокса
            SetTimerPeriod.Enabled = checkBox1.Checked; //активируем или деактивируем поле ввода  в зависимости от состояния чекбокса
            checkBox1.Update(); //обновляем чекбокс
        }

        private void Update_Button1_Click(object sender, EventArgs e)
        {
            UpdateHostsGrid(); //обновляем таблицу
        }
    }
}
