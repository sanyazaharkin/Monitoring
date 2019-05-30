using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonitoringInterface
{
    public partial class CabinetsForm : Form
    {

        readonly HostsForm parentForm;
        

        public CabinetsForm()
        {
            InitializeComponent();

        }
        public CabinetsForm(HostsForm parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            Text = "Редактирование списка кабинетов";
            UpdateCabinets();
        }

        private void UpdateCabinets()
        {            
            listBox1.DataSource = GetCabinets();
            listBox1.Update();    
        }

        private List<Cabinet> GetCabinets()
        {
            List<Cabinet> result = new List<Cabinet>();

            foreach (string[] row in parentForm.GetTableFromDB("SELECT id, cabinet FROM cabinets;", 2))
            {
                result.Add(new Cabinet(int.Parse(row[0]), row[1]));
            }
            return result;
        }

        private void insert_button_Click(object sender, EventArgs e)
        {
            if(insert_cabinet_textBox1.Text != "")
                parentForm.GetTableFromDB("INSERT INTO cabinets (cabinet) VALUES ('" + insert_cabinet_textBox1.Text + "');", 1);
            insert_cabinet_textBox1.Clear();
            parentForm.UpdateHostsGrid();
            UpdateCabinets();

        }

        private void delete_cabinet_Click(object sender, EventArgs e)
        {
            Cabinet temp = (listBox1.SelectedItem as Cabinet);
            parentForm.GetTableFromDB("DELETE FROM cabinets WHERE id = " + temp.id + ";", 1);
            parentForm.UpdateHostsGrid();
            UpdateCabinets();
        }
    }
}
