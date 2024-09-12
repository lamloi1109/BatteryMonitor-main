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
    public partial class Nofitication : Form
    {
        public string message = "";


        public Nofitication(string message)
        {
            InitializeComponent();
            this.message = message;
        }

        public void setMessage(string msg)
        {
            this.message = msg;
        }


        private void Nofitication_Load(object sender, EventArgs e)
        {
            messageLabel.Text = message;
            if (message.ToLower().Contains("thiết bị đo"))
            {
                pictureBox4.Visible = false;
                pictureBox3.Visible = false;
                pictureBox1.Visible = true;
                pictureBox5.Visible = false;
                return;
            }
            if (message.ToLower().Contains("scanner"))
            {
                pictureBox4.Visible = false;
                pictureBox3.Visible = true;
                pictureBox1.Visible = false;
                pictureBox5.Visible = false;
                return;

            }
            if (message.ToLower().Contains("proface"))
            {
                pictureBox4.Visible = true;
                pictureBox3.Visible = false;
                pictureBox1.Visible = false;
                pictureBox5.Visible = false;
                return;

            }
            pictureBox4.Visible = false;
            pictureBox3.Visible = false;
            pictureBox1.Visible = false;
            pictureBox5.Visible = true;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void messageLabel_Click(object sender, EventArgs e)
        {

        }

        private void Nofitication_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ConfigIP_PORT cfg = new ConfigIP_PORT();
            //cfg.FormClosed += new FormClosedEventHandler(MenuFormClosed); // Đăng ký sự kiện FormClosed
            //this.Hide();
            cfg.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void messageLabel_Click_1(object sender, EventArgs e)
        {

        }
    }
}
