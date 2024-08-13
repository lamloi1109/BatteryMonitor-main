using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BatteryMonitor.SQLlite;
using Oracle.ManagedDataAccess.Client;

namespace BatteryMonitor
{
    public partial class Login : Form
    {
        private string userIdText;
        private string passwordText;
        private OracleSQL.OracleSQL oracleSQL;

        public Login()
        {
            InitializeComponent();
        }

        sqlitedbofff sqlLite = new sqlitedbofff();

        private void LoginButtonClick(object sender, EventArgs e)
        {
            bool isLogin = true;

            // check null or empty
            if (string.IsNullOrEmpty(userIdText))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!");

                return;
            }

            if (string.IsNullOrEmpty(passwordText))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
                return;
            }

            // vadation for userid and password
            // userid need to be 6 characters
            if (userIdText.Length < 6)
            {
                MessageBox.Show("Tên đăng nhập phải có ít nhất 6 ký tự!");
                return;
            }
            // userid don't have specical characters
            if (userIdText.Any(char.IsPunctuation))
            {
                MessageBox.Show("Tên đăng nhập không được chứa ký tự đặc biệt!");
                return;
            }
            // one character in userid need to be uppercase and it is a character
            if (!userIdText.Any(char.IsUpper))
            {
                MessageBox.Show("Ký tự đầu tiên của tên đăng nhập phải viết hoa!");
                return;
            }
            // any character without one character is number
            if (!userIdText.Any(char.IsDigit))
            {
                MessageBox.Show("Tên đăng nhập chỉ chứa 1 ký tự viết hoa và các số");
                return;
            }

            //sqlLite.insertUser(userIdText, passwordText);
            Properties.Settings.Default.userId = userIdText;
            Properties.Settings.Default.Save();
            MenuForm menu = new MenuForm();
            menu.FormClosed += new FormClosedEventHandler(LoginClosed); // Đăng ký sự kiện FormClosed
            this.Hide();
            menu.Show();

            //// connection string sql
            //string connectionString = oracleSQL.GetConnectionString();
            //if (string.IsNullOrEmpty(connectionString))
            //{

            //}

            //using (OracleConnection oraConnect = new OracleConnection(connectionString))
            //{
            //    try
            //    {
            //        oraConnect.Open();
            //        // kiểm tra tên đăng nhập có tồn tại trong bảng tc_qrh_file hay không
            //        List<string> result = oracleSQL.getStringListData("SELECT tc_qrh001 FROM tc_qrh_file WHERE tc_qrh001 = '" + userIdText + "'", oraConnect);
            //        if (result.Count == 0)
            //        {
            //            // tên đăng nhập không tồn tại
            //            MessageBox.Show("Tên đăng nhập không tồn tại!");
            //            return;
            //        }

            //        // kiểm tra mật khẩu có đúng không
            //        result = oracleSQL.getStringListData("SELECT tc_qrh001 FROM tc_qrh_file WHERE tc_qrh001 = '" + userIdText + "' AND tc_qrh002 = '" + passwordText + "'", oraConnect);
            //        if (result.Count == 0)
            //        {
            //            // tên đăng nhập không tồn tại
            //            MessageBox.Show("Sai mật khẩu!");
            //            return;
            //        }

            //        // Lấy thông tin user từ bảng cpf_file
            //        // Set thông tin user vào biến class user

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //        Console.WriteLine(ex.Message);
            //    }
            //}

            //if (isLogin)
            //{
            //    MenuForm menu = new MenuForm();
            //    menu.FormClosed += new FormClosedEventHandler(LoginClosed); // Đăng ký sự kiện FormClosed
            //    this.Hide();
            //    menu.Show();
            //}
        }

        private void LoginClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }


        private void Login_Load(object sender, EventArgs e)
        {
            userIdText = userId.Text;
            passwordText = password.Text;
            oracleSQL = new OracleSQL.OracleSQL("PRO", "lelong", "lelong");
            oracleSQL.SetConnectionString();
        }

        private void userId_TextChanged(object sender, EventArgs e)
        {
            userIdText = userId.Text;
        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            passwordText = password.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            registerForm rForm = new registerForm();
            rForm.FormClosed += new FormClosedEventHandler(LoginClosed); // Đăng ký sự kiện FormClosed
            this.Hide();
            rForm.Show();
        }
    }
}
