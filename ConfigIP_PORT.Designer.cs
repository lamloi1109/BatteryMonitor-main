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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "hiokiIp";
            // 
            // hiokiIp
            // 
            this.hiokiIp.Location = new System.Drawing.Point(84, 6);
            this.hiokiIp.Name = "hiokiIp";
            this.hiokiIp.Size = new System.Drawing.Size(121, 20);
            this.hiokiIp.TabIndex = 1;
            this.hiokiIp.Text = "192.168.1.1";
            this.hiokiIp.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "hiokiPort";
            // 
            // hiokiPort
            // 
            this.hiokiPort.Location = new System.Drawing.Point(84, 35);
            this.hiokiPort.Name = "hiokiPort";
            this.hiokiPort.Size = new System.Drawing.Size(121, 20);
            this.hiokiPort.TabIndex = 3;
            this.hiokiPort.Text = "23";
            // 
            // modbustIp
            // 
            this.modbustIp.Location = new System.Drawing.Point(84, 71);
            this.modbustIp.Name = "modbustIp";
            this.modbustIp.Size = new System.Drawing.Size(121, 20);
            this.modbustIp.TabIndex = 4;
            this.modbustIp.Text = "192.168.1.57";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "modbustIp";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // modbustPort
            // 
            this.modbustPort.Location = new System.Drawing.Point(84, 97);
            this.modbustPort.Name = "modbustPort";
            this.modbustPort.Size = new System.Drawing.Size(121, 20);
            this.modbustPort.TabIndex = 6;
            this.modbustPort.Text = "502";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "modbustPort";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Orange;
            this.button1.Location = new System.Drawing.Point(18, 293);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.PaleGreen;
            this.button2.Location = new System.Drawing.Point(130, 293);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // keygenceIp
            // 
            this.keygenceIp.Location = new System.Drawing.Point(84, 134);
            this.keygenceIp.Name = "keygenceIp";
            this.keygenceIp.Size = new System.Drawing.Size(121, 20);
            this.keygenceIp.TabIndex = 10;
            this.keygenceIp.TextChanged += new System.EventHandler(this.keygenceIp_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "keygenceIp";
            // 
            // keygencePort
            // 
            this.keygencePort.Location = new System.Drawing.Point(84, 160);
            this.keygencePort.Name = "keygencePort";
            this.keygencePort.ReadOnly = true;
            this.keygencePort.Size = new System.Drawing.Size(121, 20);
            this.keygencePort.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 163);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "keygencePort";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 200);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Đặt số pin";
            // 
            // pinModes
            // 
            this.pinModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pinModes.FormattingEnabled = true;
            this.pinModes.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.pinModes.Location = new System.Drawing.Point(84, 197);
            this.pinModes.Name = "pinModes";
            this.pinModes.Size = new System.Drawing.Size(121, 21);
            this.pinModes.TabIndex = 19;
            this.pinModes.SelectedIndexChanged += new System.EventHandler(this.pinModes_SelectedIndexChanged);
            // 
            // export_setting_local
            // 
            this.export_setting_local.Location = new System.Drawing.Point(84, 239);
            this.export_setting_local.Name = "export_setting_local";
            this.export_setting_local.Size = new System.Drawing.Size(121, 23);
            this.export_setting_local.TabIndex = 20;
            this.export_setting_local.Text = "...";
            this.export_setting_local.UseVisualStyleBackColor = true;
            this.export_setting_local.Click += new System.EventHandler(this.export_setting_local_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 244);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Export_Local";
            // 
            // ConfigIP_PORT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 328);
            this.Controls.Add(this.export_setting_local);
            this.Controls.Add(this.pinModes);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
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
    }
}