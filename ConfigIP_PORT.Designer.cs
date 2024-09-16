namespace BatteryMonitor
{
    partial class ConfigIP_PORT
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
            this.label1 = new System.Windows.Forms.Label();
            this.hiokiIp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.hiokiPort = new System.Windows.Forms.TextBox();
            this.modbustIp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.modbustPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.keygenceIp = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.keygencePort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pinModes = new System.Windows.Forms.ComboBox();
            this.export_setting_local = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.begindate = new System.Windows.Forms.DateTimePicker();
            this.enddate = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Địa chỉ HIOKI";
            // 
            // hiokiIp
            // 
            this.hiokiIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hiokiIp.Location = new System.Drawing.Point(230, 11);
            this.hiokiIp.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.hiokiIp.Name = "hiokiIp";
            this.hiokiIp.Size = new System.Drawing.Size(238, 30);
            this.hiokiIp.TabIndex = 1;
            this.hiokiIp.Text = "192.168.1.1";
            this.hiokiIp.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port HIOKI";
            // 
            // hiokiPort
            // 
            this.hiokiPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hiokiPort.Location = new System.Drawing.Point(230, 53);
            this.hiokiPort.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.hiokiPort.Name = "hiokiPort";
            this.hiokiPort.Size = new System.Drawing.Size(238, 30);
            this.hiokiPort.TabIndex = 3;
            this.hiokiPort.Text = "23";
            // 
            // modbustIp
            // 
            this.modbustIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modbustIp.Location = new System.Drawing.Point(230, 95);
            this.modbustIp.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.modbustIp.Name = "modbustIp";
            this.modbustIp.Size = new System.Drawing.Size(238, 30);
            this.modbustIp.TabIndex = 4;
            this.modbustIp.Text = "192.168.1.57";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 98);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Địa chỉ Proface";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // modbustPort
            // 
            this.modbustPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modbustPort.Location = new System.Drawing.Point(230, 137);
            this.modbustPort.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.modbustPort.Name = "modbustPort";
            this.modbustPort.Size = new System.Drawing.Size(238, 30);
            this.modbustPort.TabIndex = 6;
            this.modbustPort.Text = "502";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 140);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "Port Proface";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Orange;
            this.button1.Location = new System.Drawing.Point(61, 564);
            this.button1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 44);
            this.button1.TabIndex = 8;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.PaleGreen;
            this.button2.Location = new System.Drawing.Point(260, 564);
            this.button2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(150, 44);
            this.button2.TabIndex = 9;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // keygenceIp
            // 
            this.keygenceIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.keygenceIp.Location = new System.Drawing.Point(230, 185);
            this.keygenceIp.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.keygenceIp.Name = "keygenceIp";
            this.keygenceIp.Size = new System.Drawing.Size(238, 30);
            this.keygenceIp.TabIndex = 10;
            this.keygenceIp.TextChanged += new System.EventHandler(this.keygenceIp_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 190);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 25);
            this.label5.TabIndex = 11;
            this.label5.Text = "Địa chỉ Keyence";
            // 
            // keygencePort
            // 
            this.keygencePort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.keygencePort.Location = new System.Drawing.Point(230, 232);
            this.keygencePort.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.keygencePort.Name = "keygencePort";
            this.keygencePort.ReadOnly = true;
            this.keygencePort.Size = new System.Drawing.Size(238, 30);
            this.keygencePort.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 235);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 25);
            this.label6.TabIndex = 13;
            this.label6.Text = "Port Keyence";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 289);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 25);
            this.label7.TabIndex = 14;
            this.label7.Text = "Đặt số pin";
            // 
            // pinModes
            // 
            this.pinModes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pinModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pinModes.Enabled = false;
            this.pinModes.FormattingEnabled = true;
            this.pinModes.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.pinModes.Location = new System.Drawing.Point(230, 277);
            this.pinModes.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pinModes.Name = "pinModes";
            this.pinModes.Size = new System.Drawing.Size(238, 33);
            this.pinModes.TabIndex = 19;
            this.pinModes.SelectedIndexChanged += new System.EventHandler(this.pinModes_SelectedIndexChanged);
            // 
            // export_setting_local
            // 
            this.export_setting_local.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.export_setting_local.Location = new System.Drawing.Point(230, 459);
            this.export_setting_local.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.export_setting_local.Name = "export_setting_local";
            this.export_setting_local.Size = new System.Drawing.Size(242, 44);
            this.export_setting_local.TabIndex = 20;
            this.export_setting_local.Text = "...";
            this.export_setting_local.UseVisualStyleBackColor = true;
            this.export_setting_local.Click += new System.EventHandler(this.export_setting_local_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 469);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(126, 25);
            this.label8.TabIndex = 14;
            this.label8.Text = "Export_Local";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 331);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(205, 25);
            this.label9.TabIndex = 13;
            this.label9.Text = "Thời gian bắt đầu ca 1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 382);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(208, 25);
            this.label10.TabIndex = 13;
            this.label10.Text = "Thời gian kết thúc ca 1";
            // 
            // begindate
            // 
            this.begindate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.begindate.CustomFormat = "HH:mm";
            this.begindate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.begindate.Location = new System.Drawing.Point(230, 331);
            this.begindate.Name = "begindate";
            this.begindate.Size = new System.Drawing.Size(114, 30);
            this.begindate.TabIndex = 21;
            // 
            // enddate
            // 
            this.enddate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.enddate.CustomFormat = "HH:mm";
            this.enddate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.enddate.Location = new System.Drawing.Point(230, 377);
            this.enddate.Name = "enddate";
            this.enddate.Size = new System.Drawing.Size(114, 30);
            this.enddate.TabIndex = 21;
            // 
            // ConfigIP_PORT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(496, 631);
            this.Controls.Add(this.enddate);
            this.Controls.Add(this.begindate);
            this.Controls.Add(this.export_setting_local);
            this.Controls.Add(this.pinModes);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.keygencePort);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.keygenceIp);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.modbustPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.modbustIp);
            this.Controls.Add(this.hiokiPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hiokiIp);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "ConfigIP_PORT";
            this.Text = "ConfigIP_PORT";
            this.Load += new System.EventHandler(this.ConfigIP_PORT_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hiokiIp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox hiokiPort;
        private System.Windows.Forms.TextBox modbustIp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox modbustPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox keygenceIp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox keygencePort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox pinModes;
        private System.Windows.Forms.Button export_setting_local;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker begindate;
        private System.Windows.Forms.DateTimePicker enddate;
    }
}