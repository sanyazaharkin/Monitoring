namespace MonitoringInterface
{
    partial class HostForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProgramHistoryGrid = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.expand_devices_button = new System.Windows.Forms.Button();
            this.colapse_devices_button = new System.Windows.Forms.Button();
            this.DevicesTree = new System.Windows.Forms.TreeView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.expand_programs_button = new System.Windows.Forms.Button();
            this.ProgramTree = new System.Windows.Forms.TreeView();
            this.collapse_programs_button = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.SetLookedButton1 = new System.Windows.Forms.Button();
            this.DevicesHistoryGrid = new System.Windows.Forms.DataGridView();
            this.nostname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Device = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Looked = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.SetLookedButton2 = new System.Windows.Forms.Button();
            this.ProgramsHistoryGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.DeleteHostButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.Upload_Report_button = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Accept_Button1 = new System.Windows.Forms.Button();
            this.ProgramHistoryGrid.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DevicesHistoryGrid)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProgramsHistoryGrid)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProgramHistoryGrid
            // 
            this.ProgramHistoryGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramHistoryGrid.Controls.Add(this.tabPage1);
            this.ProgramHistoryGrid.Controls.Add(this.tabPage5);
            this.ProgramHistoryGrid.Controls.Add(this.tabPage2);
            this.ProgramHistoryGrid.Controls.Add(this.tabPage3);
            this.ProgramHistoryGrid.Controls.Add(this.tabPage4);
            this.ProgramHistoryGrid.Location = new System.Drawing.Point(12, 43);
            this.ProgramHistoryGrid.Name = "ProgramHistoryGrid";
            this.ProgramHistoryGrid.SelectedIndex = 0;
            this.ProgramHistoryGrid.Size = new System.Drawing.Size(1192, 372);
            this.ProgramHistoryGrid.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.expand_devices_button);
            this.tabPage1.Controls.Add(this.colapse_devices_button);
            this.tabPage1.Controls.Add(this.DevicesTree);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(723, 292);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Установленное оборудование";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // expand_devices_button
            // 
            this.expand_devices_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.expand_devices_button.Location = new System.Drawing.Point(0, 268);
            this.expand_devices_button.Name = "expand_devices_button";
            this.expand_devices_button.Size = new System.Drawing.Size(154, 23);
            this.expand_devices_button.TabIndex = 1;
            this.expand_devices_button.Text = "развернуть";
            this.expand_devices_button.UseVisualStyleBackColor = true;
            this.expand_devices_button.Click += new System.EventHandler(this.expand_devices_button_Click);
            // 
            // colapse_devices_button
            // 
            this.colapse_devices_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.colapse_devices_button.Location = new System.Drawing.Point(160, 268);
            this.colapse_devices_button.Name = "colapse_devices_button";
            this.colapse_devices_button.Size = new System.Drawing.Size(154, 23);
            this.colapse_devices_button.TabIndex = 1;
            this.colapse_devices_button.Text = "свернуть";
            this.colapse_devices_button.UseVisualStyleBackColor = true;
            this.colapse_devices_button.Click += new System.EventHandler(this.colapse_devices_button_Click);
            // 
            // DevicesTree
            // 
            this.DevicesTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DevicesTree.Location = new System.Drawing.Point(0, 0);
            this.DevicesTree.Name = "DevicesTree";
            this.DevicesTree.Size = new System.Drawing.Size(723, 262);
            this.DevicesTree.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.expand_programs_button);
            this.tabPage5.Controls.Add(this.ProgramTree);
            this.tabPage5.Controls.Add(this.collapse_programs_button);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(723, 292);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Установленное ПО";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // expand_programs_button
            // 
            this.expand_programs_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.expand_programs_button.Location = new System.Drawing.Point(0, 269);
            this.expand_programs_button.Name = "expand_programs_button";
            this.expand_programs_button.Size = new System.Drawing.Size(154, 23);
            this.expand_programs_button.TabIndex = 1;
            this.expand_programs_button.Text = "развернуть";
            this.expand_programs_button.UseVisualStyleBackColor = true;
            this.expand_programs_button.Click += new System.EventHandler(this.expand_programs_button_Click);
            // 
            // ProgramTree
            // 
            this.ProgramTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramTree.Location = new System.Drawing.Point(0, 0);
            this.ProgramTree.Name = "ProgramTree";
            this.ProgramTree.Size = new System.Drawing.Size(723, 241);
            this.ProgramTree.TabIndex = 0;
            // 
            // collapse_programs_button
            // 
            this.collapse_programs_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.collapse_programs_button.Location = new System.Drawing.Point(160, 269);
            this.collapse_programs_button.Name = "collapse_programs_button";
            this.collapse_programs_button.Size = new System.Drawing.Size(154, 23);
            this.collapse_programs_button.TabIndex = 1;
            this.collapse_programs_button.Text = "свернуть";
            this.collapse_programs_button.UseVisualStyleBackColor = true;
            this.collapse_programs_button.Click += new System.EventHandler(this.collapse_programs_button_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.SetLookedButton1);
            this.tabPage2.Controls.Add(this.DevicesHistoryGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1184, 346);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Изменения оборудования";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SetLookedButton1
            // 
            this.SetLookedButton1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetLookedButton1.Location = new System.Drawing.Point(0, 324);
            this.SetLookedButton1.Name = "SetLookedButton1";
            this.SetLookedButton1.Size = new System.Drawing.Size(1184, 22);
            this.SetLookedButton1.TabIndex = 1;
            this.SetLookedButton1.Text = "Пометить все как просмотрено";
            this.SetLookedButton1.UseVisualStyleBackColor = true;
            this.SetLookedButton1.Click += new System.EventHandler(this.SetLookedButton1_Click);
            // 
            // DevicesHistoryGrid
            // 
            this.DevicesHistoryGrid.AllowUserToAddRows = false;
            this.DevicesHistoryGrid.AllowUserToDeleteRows = false;
            this.DevicesHistoryGrid.AllowUserToOrderColumns = true;
            this.DevicesHistoryGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DevicesHistoryGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DevicesHistoryGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DevicesHistoryGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nostname,
            this.Device,
            this.Action,
            this.Looked,
            this.date});
            this.DevicesHistoryGrid.Location = new System.Drawing.Point(6, 0);
            this.DevicesHistoryGrid.Name = "DevicesHistoryGrid";
            this.DevicesHistoryGrid.ReadOnly = true;
            this.DevicesHistoryGrid.Size = new System.Drawing.Size(1178, 318);
            this.DevicesHistoryGrid.TabIndex = 0;
            // 
            // nostname
            // 
            this.nostname.HeaderText = "Компьютер";
            this.nostname.Name = "nostname";
            this.nostname.ReadOnly = true;
            // 
            // Device
            // 
            this.Device.HeaderText = "Устройство";
            this.Device.Name = "Device";
            this.Device.ReadOnly = true;
            // 
            // Action
            // 
            this.Action.HeaderText = "Действие";
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            // 
            // Looked
            // 
            this.Looked.HeaderText = "Просмотрено";
            this.Looked.Name = "Looked";
            this.Looked.ReadOnly = true;
            // 
            // date
            // 
            this.date.HeaderText = "дата и время";
            this.date.Name = "date";
            this.date.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.SetLookedButton2);
            this.tabPage3.Controls.Add(this.ProgramsHistoryGrid);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1184, 346);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Изменения ПО";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // SetLookedButton2
            // 
            this.SetLookedButton2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetLookedButton2.Location = new System.Drawing.Point(-1, 320);
            this.SetLookedButton2.Name = "SetLookedButton2";
            this.SetLookedButton2.Size = new System.Drawing.Size(1185, 22);
            this.SetLookedButton2.TabIndex = 2;
            this.SetLookedButton2.Text = "Пометить все как просмотрено";
            this.SetLookedButton2.UseVisualStyleBackColor = true;
            this.SetLookedButton2.Click += new System.EventHandler(this.SetLookedButton2_Click);
            // 
            // ProgramsHistoryGrid
            // 
            this.ProgramsHistoryGrid.AllowUserToAddRows = false;
            this.ProgramsHistoryGrid.AllowUserToDeleteRows = false;
            this.ProgramsHistoryGrid.AllowUserToOrderColumns = true;
            this.ProgramsHistoryGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramsHistoryGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ProgramsHistoryGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProgramsHistoryGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.ProgramsHistoryGrid.Location = new System.Drawing.Point(-1, 0);
            this.ProgramsHistoryGrid.Name = "ProgramsHistoryGrid";
            this.ProgramsHistoryGrid.ReadOnly = true;
            this.ProgramsHistoryGrid.Size = new System.Drawing.Size(1185, 314);
            this.ProgramsHistoryGrid.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Компьютер";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Программа";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Действие";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Просмотрено";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "дата и время";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.listView1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(723, 292);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Запущенные процессы";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(723, 292);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(12, 9);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(148, 23);
            this.UpdateButton.TabIndex = 1;
            this.UpdateButton.Text = "Обновить информацию";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // DeleteHostButton
            // 
            this.DeleteHostButton.Location = new System.Drawing.Point(166, 9);
            this.DeleteHostButton.Name = "DeleteHostButton";
            this.DeleteHostButton.Size = new System.Drawing.Size(90, 23);
            this.DeleteHostButton.TabIndex = 2;
            this.DeleteHostButton.Text = "Удалить хост";
            this.DeleteHostButton.UseVisualStyleBackColor = true;
            this.DeleteHostButton.Click += new System.EventHandler(this.DeleteHostButton_Click);
            // 
            // Upload_Report_button
            // 
            this.Upload_Report_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Upload_Report_button.Location = new System.Drawing.Point(1085, 9);
            this.Upload_Report_button.Name = "Upload_Report_button";
            this.Upload_Report_button.Size = new System.Drawing.Size(119, 23);
            this.Upload_Report_button.TabIndex = 3;
            this.Upload_Report_button.Text = "Выгрузить отчет";
            this.Upload_Report_button.UseVisualStyleBackColor = true;
            this.Upload_Report_button.Click += new System.EventHandler(this.Upload_Report_button_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(262, 9);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(694, 21);
            this.comboBox1.TabIndex = 4;
            // 
            // Accept_Button1
            // 
            this.Accept_Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Accept_Button1.Location = new System.Drawing.Point(962, 9);
            this.Accept_Button1.Name = "Accept_Button1";
            this.Accept_Button1.Size = new System.Drawing.Size(117, 23);
            this.Accept_Button1.TabIndex = 5;
            this.Accept_Button1.Text = "Подтвердить";
            this.Accept_Button1.UseVisualStyleBackColor = true;
            this.Accept_Button1.Click += new System.EventHandler(this.Accept_Button1_Click);
            // 
            // HostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1216, 427);
            this.Controls.Add(this.Accept_Button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Upload_Report_button);
            this.Controls.Add(this.DeleteHostButton);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.ProgramHistoryGrid);
            this.MinimumSize = new System.Drawing.Size(750, 400);
            this.Name = "HostForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "HostForm";
            this.Load += new System.EventHandler(this.HostForm_Load);
            this.ProgramHistoryGrid.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DevicesHistoryGrid)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProgramsHistoryGrid)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl ProgramHistoryGrid;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TreeView DevicesTree;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TreeView ProgramTree;
        private System.Windows.Forms.DataGridView DevicesHistoryGrid;
        private System.Windows.Forms.Button SetLookedButton1;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button DeleteHostButton;
        private System.Windows.Forms.DataGridView ProgramsHistoryGrid;
        private System.Windows.Forms.Button SetLookedButton2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nostname;
        private System.Windows.Forms.DataGridViewTextBoxColumn Device;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn Looked;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button Upload_Report_button;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button Accept_Button1;
        private System.Windows.Forms.Button expand_devices_button;
        private System.Windows.Forms.Button colapse_devices_button;
        private System.Windows.Forms.Button expand_programs_button;
        private System.Windows.Forms.Button collapse_programs_button;
    }
}