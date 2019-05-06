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
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hostname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SetTimerPeriod = new System.Windows.Forms.NumericUpDown();
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
            this.UpdateDate,
            this.state});
            this.HostsGrid.Location = new System.Drawing.Point(12, 12);
            this.HostsGrid.Margin = new System.Windows.Forms.Padding(1);
            this.HostsGrid.Name = "HostsGrid";
            this.HostsGrid.ReadOnly = true;
            this.HostsGrid.Size = new System.Drawing.Size(700, 337);
            this.HostsGrid.TabIndex = 0;
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
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // SetTimerPeriod
            // 
            this.SetTimerPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SetTimerPeriod.Location = new System.Drawing.Point(822, 329);
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
            // HostsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 361);
            this.Controls.Add(this.SetTimerPeriod);
            this.Controls.Add(this.HostsGrid);
            this.MinimumSize = new System.Drawing.Size(900, 400);
            this.Name = "HostsForm";
            this.Text = "Hosts";
            this.Load += new System.EventHandler(this.HostsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.HostsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SetTimerPeriod)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView HostsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn hostname;
        private System.Windows.Forms.DataGridViewTextBoxColumn bios;
        private System.Windows.Forms.DataGridViewTextBoxColumn OS;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn state;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NumericUpDown SetTimerPeriod;
    }
}

