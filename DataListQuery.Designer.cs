namespace BatteryMonitor
{
    partial class DataListQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataListQuery));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.shipmentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.specs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.batteryCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.r = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.v = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workShift = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalMeasureMent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measureMentStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpk_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpk_V = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type_V = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.std_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.std_V = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ave_R = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ave_V = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp_begin = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_maslh = new System.Windows.Forms.ComboBox();
            this.btn_export = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cb_CaLamViec = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp_end = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.shipmentId,
            this.specs,
            this.batteryCode,
            this.r,
            this.v,
            this.rMax,
            this.vMax,
            this.rMin,
            this.vMin,
            this.workShift,
            this.totalMeasureMent,
            this.date,
            this.userId,
            this.measureMentStatus,
            this.quality,
            this.cpk_R,
            this.cpk_V,
            this.type_R,
            this.type_V,
            this.std_R,
            this.std_V,
            this.ave_R,
            this.ave_V});
            this.dataGridView1.Location = new System.Drawing.Point(2, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1147, 366);
            this.dataGridView1.TabIndex = 0;
            // 
            // shipmentId
            // 
            this.shipmentId.DataPropertyName = "shipmentId";
            this.shipmentId.HeaderText = "Mã số lô hàng";
            this.shipmentId.Name = "shipmentId";
            this.shipmentId.ReadOnly = true;
            // 
            // specs
            // 
            this.specs.DataPropertyName = "specs";
            this.specs.HeaderText = "Quy cách";
            this.specs.Name = "specs";
            this.specs.ReadOnly = true;
            // 
            // batteryCode
            // 
            this.batteryCode.DataPropertyName = "batteryCode";
            this.batteryCode.HeaderText = "Mã Code Pin";
            this.batteryCode.Name = "batteryCode";
            this.batteryCode.ReadOnly = true;
            // 
            // r
            // 
            this.r.DataPropertyName = "r";
            this.r.HeaderText = "Điện trở";
            this.r.Name = "r";
            this.r.ReadOnly = true;
            // 
            // v
            // 
            this.v.DataPropertyName = "v";
            this.v.HeaderText = "Điện áp";
            this.v.Name = "v";
            this.v.ReadOnly = true;
            // 
            // rMax
            // 
            this.rMax.DataPropertyName = "rMax";
            this.rMax.HeaderText = "Điện trở Max";
            this.rMax.Name = "rMax";
            this.rMax.ReadOnly = true;
            // 
            // vMax
            // 
            this.vMax.DataPropertyName = "vMax";
            this.vMax.HeaderText = "Điện áp Max";
            this.vMax.Name = "vMax";
            this.vMax.ReadOnly = true;
            // 
            // rMin
            // 
            this.rMin.DataPropertyName = "rMin";
            this.rMin.HeaderText = "Điện trở Min";
            this.rMin.Name = "rMin";
            this.rMin.ReadOnly = true;
            // 
            // vMin
            // 
            this.vMin.DataPropertyName = "vMin";
            this.vMin.HeaderText = "Điện áp Min";
            this.vMin.Name = "vMin";
            this.vMin.ReadOnly = true;
            // 
            // workShift
            // 
            this.workShift.DataPropertyName = "workShift";
            this.workShift.HeaderText = "Ca Làm việc";
            this.workShift.Name = "workShift";
            this.workShift.ReadOnly = true;
            // 
            // totalMeasureMent
            // 
            this.totalMeasureMent.DataPropertyName = "totalMeasureMent";
            this.totalMeasureMent.HeaderText = "Tổng lần đo";
            this.totalMeasureMent.Name = "totalMeasureMent";
            this.totalMeasureMent.ReadOnly = true;
            // 
            // date
            // 
            this.date.DataPropertyName = "date";
            this.date.HeaderText = "Ngày";
            this.date.Name = "date";
            this.date.ReadOnly = true;
            // 
            // userId
            // 
            this.userId.DataPropertyName = "userId";
            this.userId.HeaderText = "Người TT";
            this.userId.Name = "userId";
            this.userId.ReadOnly = true;
            // 
            // measureMentStatus
            // 
            this.measureMentStatus.DataPropertyName = "measureMentStatus";
            this.measureMentStatus.HeaderText = "Kết quả đo";
            this.measureMentStatus.Name = "measureMentStatus";
            this.measureMentStatus.ReadOnly = true;
            // 
            // quality
            // 
            this.quality.HeaderText = "Chất lượng";
            this.quality.Name = "quality";
            this.quality.ReadOnly = true;
            // 
            // cpk_R
            // 
            this.cpk_R.DataPropertyName = "cpk_R";
            this.cpk_R.HeaderText = "Điện trở CPK";
            this.cpk_R.Name = "cpk_R";
            this.cpk_R.ReadOnly = true;
            // 
            // cpk_V
            // 
            this.cpk_V.DataPropertyName = "cpk_V";
            this.cpk_V.HeaderText = "Điện áp CPK";
            this.cpk_V.Name = "cpk_V";
            this.cpk_V.ReadOnly = true;
            // 
            // type_R
            // 
            this.type_R.DataPropertyName = "type_R";
            this.type_R.HeaderText = "Phân loại Điện trở";
            this.type_R.Name = "type_R";
            this.type_R.ReadOnly = true;
            // 
            // type_V
            // 
            this.type_V.DataPropertyName = "type_V";
            this.type_V.HeaderText = "Phân loại Điện áp";
            this.type_V.Name = "type_V";
            this.type_V.ReadOnly = true;
            // 
            // std_R
            // 
            this.std_R.DataPropertyName = "std_R";
            this.std_R.HeaderText = "Điện trở STD";
            this.std_R.Name = "std_R";
            this.std_R.ReadOnly = true;
            // 
            // std_V
            // 
            this.std_V.DataPropertyName = "std_V";
            this.std_V.HeaderText = "Điện áp STD";
            this.std_V.Name = "std_V";
            this.std_V.ReadOnly = true;
            // 
            // ave_R
            // 
            this.ave_R.DataPropertyName = "ave_R";
            this.ave_R.HeaderText = "Điện trở AVE";
            this.ave_R.Name = "ave_R";
            this.ave_R.ReadOnly = true;
            // 
            // ave_V
            // 
            this.ave_V.DataPropertyName = "ave_V";
            this.ave_V.HeaderText = "Điện áp AVE";
            this.ave_V.Name = "ave_V";
            this.ave_V.ReadOnly = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.Location = new System.Drawing.Point(64, 413);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 416);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Số dòng";
            // 
            // dtp_begin
            // 
            this.dtp_begin.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dtp_begin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_begin.Location = new System.Drawing.Point(48, 9);
            this.dtp_begin.Name = "dtp_begin";
            this.dtp_begin.Size = new System.Drawing.Size(142, 20);
            this.dtp_begin.TabIndex = 4;
            this.dtp_begin.ValueChanged += new System.EventHandler(this.dtp_begin_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cb_maslh);
            this.panel1.Controls.Add(this.btn_export);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.cb_CaLamViec);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtp_end);
            this.panel1.Controls.Add(this.dtp_begin);
            this.panel1.Location = new System.Drawing.Point(2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1144, 39);
            this.panel1.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(570, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Mã số lô hàng";
            // 
            // cb_maslh
            // 
            this.cb_maslh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_maslh.FormattingEnabled = true;
            this.cb_maslh.Items.AddRange(new object[] {
            "ALL"});
            this.cb_maslh.Location = new System.Drawing.Point(650, 9);
            this.cb_maslh.Name = "cb_maslh";
            this.cb_maslh.Size = new System.Drawing.Size(183, 21);
            this.cb_maslh.TabIndex = 10;
            // 
            // btn_export
            // 
            this.btn_export.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_export.Image = ((System.Drawing.Image)(resources.GetObject("btn_export.Image")));
            this.btn_export.Location = new System.Drawing.Point(1083, 1);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(53, 38);
            this.btn_export.TabIndex = 9;
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(1030, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 37);
            this.button1.TabIndex = 8;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.toolStripbtn_qry_Click);
            // 
            // cb_CaLamViec
            // 
            this.cb_CaLamViec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_CaLamViec.FormattingEnabled = true;
            this.cb_CaLamViec.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cb_CaLamViec.Location = new System.Drawing.Point(489, 9);
            this.cb_CaLamViec.Name = "cb_CaLamViec";
            this.cb_CaLamViec.Size = new System.Drawing.Size(65, 21);
            this.cb_CaLamViec.TabIndex = 7;
            this.cb_CaLamViec.SelectedIndexChanged += new System.EventHandler(this.cb_CaLamViec_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(414, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Ca Làm Việc";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(207, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "To";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "From";
            // 
            // dtp_end
            // 
            this.dtp_end.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dtp_end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_end.Location = new System.Drawing.Point(244, 9);
            this.dtp_end.Name = "dtp_end";
            this.dtp_end.Size = new System.Drawing.Size(142, 20);
            this.dtp_end.TabIndex = 4;
            this.dtp_end.ValueChanged += new System.EventHandler(this.dtp_end_ValueChanged);
            // 
            // DataListQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 438);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DataListQuery";
            this.Text = "DataListQuery";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DataListQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp_begin;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtp_end;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_CaLamViec;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipmentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn specs;
        private System.Windows.Forms.DataGridViewTextBoxColumn batteryCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn r;
        private System.Windows.Forms.DataGridViewTextBoxColumn v;
        private System.Windows.Forms.DataGridViewTextBoxColumn rMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn vMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn rMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn vMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn workShift;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalMeasureMent;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn userId;
        private System.Windows.Forms.DataGridViewTextBoxColumn measureMentStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn quality;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpk_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpk_V;
        private System.Windows.Forms.DataGridViewTextBoxColumn type_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn type_V;
        private System.Windows.Forms.DataGridViewTextBoxColumn std_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn std_V;
        private System.Windows.Forms.DataGridViewTextBoxColumn ave_R;
        private System.Windows.Forms.DataGridViewTextBoxColumn ave_V;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cb_maslh;
    }
}