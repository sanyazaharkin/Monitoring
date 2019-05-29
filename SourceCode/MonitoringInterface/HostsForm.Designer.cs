namespace MonitoringInterface
{
    partial class HostsForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.HostsGrid = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SetTimerPeriod = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Update_Button1 = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hostname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cabinet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.HostsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SetTimerPeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // HostsGrid
            // 
            this.HostsGrid.AllowUserToAddRows = false;
            this.HostsGrid.AllowUserToDeleteRows = false;
            this.HostsGrid.AllowUserToOrderColumns = true;
            this.HostsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HostsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.HostsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.HostsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.hostname,
            this.bios,
            this.OS,
            this.Cabinet,
            this.UpdateDate,
            this.state});
            this.HostsGrid.Location = new System.Drawing.Point(12, 12);
            this.HostsGrid.Margin = new System.Windows.Forms.Padding(1);
            this.HostsGrid.Name = "HostsGrid";
            this.HostsGrid.ReadOnly = true;
            this.HostsGrid.Size = new System.Drawing.Size(862, 316);
            this.HostsGrid.TabIndex = 0;
            this.HostsGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HostsGrid_CellContentClick);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // SetTimerPeriod
            // 
            this.SetTimerPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SetTimerPeriod.Location = new System.Drawing.Point(271, 332);
            this.SetTimerPeriod.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.SetTimerPeriod.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SetTimerPeriod.Name = "SetTimerPeriod";
            this.SetTimerPeriod.Size = new System.Drawing.Size(50, 20);
            this.SetTimerPeriod.TabIndex = 1;
            this.SetTimerPeriod.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SetTimerPeriod.ValueChanged += new System.EventHandler(this.SetTimerPeriod_ValueChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(155, 332);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(110, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Автообновление";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(327, 336);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "период (сек.)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Update_Button1
            // 
            this.Update_Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Update_Button1.Location = new System.Drawing.Point(12, 329);
            this.Update_Button1.Name = "Update_Button1";
            this.Update_Button1.Size = new System.Drawing.Size(137, 23);
            this.Update_Button1.TabIndex = 4;
            this.Update_Button1.Text = "Обновить";
            this.Update_Button1.UseVisualStyleBackColor = true;
            this.Update_Button1.Click += new System.EventHandler(this.Update_Button1_Click);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // hostname
            // 
            this.hostname.HeaderText = "Имя хоста";
            this.hostname.Name = "hostname";
            this.hostname.ReadOnly = true;
            // 
            // bios
            // 
            this.bios.HeaderText = "BIOS";
            this.bios.Name = "bios";
            this.bios.ReadOnly = true;
            // 
            // OS
            // 
            this.OS.HeaderText = "OS";
            this.OS.Name = "OS";
            this.OS.ReadOnly = true;
            // 
            // Cabinet
            // 
            this.Cabinet.HeaderText = "Кабинет";
            this.Cabinet.Name = "Cabinet";
            this.Cabinet.ReadOnly = true;
            // 
            // UpdateDate
            // 
            this.UpdateDate.HeaderText = "время последнего обновления";
            this.UpdateDate.Name = "UpdateDate";
            this.UpdateDate.ReadOnly = true;
            // 
            // state
            // 
            this.state.HeaderText = "состояние";
            this.state.Name = "state";
            this.state.ReadOnly = true;
            // 
            // HostsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 361);
            this.Controls.Add(this.Update_Button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.SetTimerPeriod);
            this.Controls.Add(this.HostsGrid);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(900, 400);
            this.Name = "HostsForm";
            this.Text = "Список подключенных компьютеров";
            this.Load += new System.EventHandler(this.HostsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.HostsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SetTimerPeriod)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView HostsGrid;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NumericUpDown SetTimerPeriod;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Update_Button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostname;
        private System.Windows.Forms.DataGridViewTextBoxColumn bios;
        private System.Windows.Forms.DataGridViewTextBoxColumn OS;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cabinet;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn state;
    }
}

