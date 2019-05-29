namespace MonitoringInterface
{
    partial class CabinetsForm
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.insert_button = new System.Windows.Forms.Button();
            this.insert_cabinet_textBox1 = new System.Windows.Forms.TextBox();
            this.delete_cabinet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(13, 39);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(775, 394);
            this.listBox1.TabIndex = 0;
            // 
            // insert_button
            // 
            this.insert_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.insert_button.Location = new System.Drawing.Point(632, 10);
            this.insert_button.Name = "insert_button";
            this.insert_button.Size = new System.Drawing.Size(75, 23);
            this.insert_button.TabIndex = 1;
            this.insert_button.Text = "Добавить";
            this.insert_button.UseVisualStyleBackColor = true;
            this.insert_button.Click += new System.EventHandler(this.insert_button_Click);
            // 
            // insert_cabinet_textBox1
            // 
            this.insert_cabinet_textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insert_cabinet_textBox1.Location = new System.Drawing.Point(12, 12);
            this.insert_cabinet_textBox1.Name = "insert_cabinet_textBox1";
            this.insert_cabinet_textBox1.Size = new System.Drawing.Size(614, 20);
            this.insert_cabinet_textBox1.TabIndex = 2;
            // 
            // delete_cabinet
            // 
            this.delete_cabinet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delete_cabinet.Location = new System.Drawing.Point(713, 10);
            this.delete_cabinet.Name = "delete_cabinet";
            this.delete_cabinet.Size = new System.Drawing.Size(75, 23);
            this.delete_cabinet.TabIndex = 3;
            this.delete_cabinet.Text = "Удалить";
            this.delete_cabinet.UseVisualStyleBackColor = true;
            this.delete_cabinet.Click += new System.EventHandler(this.delete_cabinet_Click);
            // 
            // CabinetsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.delete_cabinet);
            this.Controls.Add(this.insert_cabinet_textBox1);
            this.Controls.Add(this.insert_button);
            this.Controls.Add(this.listBox1);
            this.Name = "CabinetsForm";
            this.Text = "CabinetsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button insert_button;
        private System.Windows.Forms.TextBox insert_cabinet_textBox1;
        private System.Windows.Forms.Button delete_cabinet;
    }
}