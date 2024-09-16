using BatteryMonitor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatteryMonitor
{
    public partial class ConfigIP_PORT : Form
    {
        static public int pinMode;

        public ConfigIP_PORT()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ConfigIP_PORT_Load(object sender, EventArgs e)
        {
            hiokiIp.Text = Properties.Settings.Default.hiokiIP;
            hiokiPort.Text = Properties.Settings.Default.hiokiPort;
            modbustIp.Text = Properties.Settings.Default.modbustIP;
            modbustPort.Text = Properties.Settings.Default.modbustPort.ToString();
            keygenceIp.Text = Properties.Settings.Default.keyGenceIP;
            pinModes.SelectedIndex = Properties.Settings.Default.CountPin;
            begindate.Value = String.IsNullOrEmpty(Properties.Settings.Default.begindate)?DateTime.Now: DateTime.Parse(Properties.Settings.Default.begindate);
            enddate.Value = String.IsNullOrEmpty(Properties.Settings.Default.enddate) ? DateTime.Now : DateTime.Parse(Properties.Settings.Default.enddate);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CountPin = pinModes.SelectedIndex;
            Properties.Settings.Default.Save();
            pinMode = pinModes.SelectedIndex == 0 ? 4 : 8;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Check quyền

            // Hỏi xem có muốn tiếp tục hay không

            // Lưu lại các thay đổi

            if (hiokiIp.Text != Properties.Settings.Default.hiokiIP)
            {
                Properties.Settings.Default.hiokiIP = hiokiIp.Text;
            }

            if (hiokiPort.Text != Properties.Settings.Default.hiokiPort)
            {
                Properties.Settings.Default.hiokiPort = hiokiPort.Text;
            }

            if (modbustIp.Text != Properties.Settings.Default.modbustIP)
            {
                Properties.Settings.Default.modbustIP = modbustIp.Text;
            }

            if (modbustPort.Text != Properties.Settings.Default.modbustPort.ToString() && int.TryParse(modbustPort.Text, out int modbustPortInt))
            {
                Properties.Settings.Default.modbustPort = modbustPortInt;
            }

            if (keygenceIp.Text != Properties.Settings.Default.keyGenceIP)
            {
                Properties.Settings.Default.keyGenceIP = keygenceIp.Text;
            }

            Properties.Settings.Default.CountPin = pinModes.SelectedIndex;

            Properties.Settings.Default.begindate = begindate.Value.ToString("yyyy/MM/dd HH:mm:ss");
            Properties.Settings.Default.enddate = enddate.Value.ToString("yyyy/MM/dd HH:mm:ss");
            Properties.Settings.Default.Save();
            // Restart lại ứng dụng

            Application.Restart();

        }

        private void keygenceIp_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            hiokiIp.Text = Properties.Settings.Default.hiokiIP;
            hiokiPort.Text = Properties.Settings.Default.hiokiPort;
            modbustIp.Text = Properties.Settings.Default.modbustIP;
            modbustPort.Text = Properties.Settings.Default.modbustPort.ToString();
            keygenceIp.Text = Properties.Settings.Default.keyGenceIP;
            this.Close();
        }

        private void export_setting_local_Click(object sender, EventArgs e)
        {
            ExcelConfig excelConfig = new ExcelConfig();
            excelConfig.FormClosed += new FormClosedEventHandler(MenuFormClosed); // Đăng ký sự kiện FormClosed
            this.Hide();
            excelConfig.Show();
        }

        private void MenuFormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void pinModes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
