using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatteryMonitor
{
    public partial class ExcelConfig : Form
    {
        public ExcelConfig()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // Cài đặt mô tả và các thuộc tính khác cho FolderBrowserDialog.
                folderDialog.Description = "Chọn thư mục để lưu";

                // Hiển thị hộp thoại và kiểm tra kết quả.
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // Lấy đường dẫn của thư mục được chọn.
                    string selectedFolderPath = folderDialog.SelectedPath;

                    // Bây giờ bạn có thể sử dụng 'selectedFolderPath' để lưu file hoặc thư mục.

                    // Thông báo cho người dùng biết thư mục đã được chọn.
                    excelPath.Text = selectedFolderPath;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // check null
            if (String.IsNullOrEmpty(excelPath.Text) || String.IsNullOrWhiteSpace(excelPath.Text))
            {
                MessageBox.Show("Chọn đường dẫn lưu file!");
                return;
            }

            // kiểm tra đường dẫn có tồn tại hay không
            if (!Directory.Exists(excelPath.Text))
            {
                MessageBox.Show("Đường dẫn không tồn tại!");
                return;
            }

            // Kiểm tra xem đã chọn định dạng file chưa
            List<string> listExtension = new List<string>() {"xls","xlsx" };
            
            bool isValidExtension = false;

            for (int i = 0; i < listExtension.Count; i++)
            {
                if (extension.Text == listExtension[i])
                {
                    isValidExtension = true;
                }
            }

            if (!isValidExtension)
            {
                MessageBox.Show("Sai định dạng! [xls, xlsx]!");
                return;
            }


            Properties.Settings.Default.excelPath = excelPath.Text;
            Properties.Settings.Default.Save();
            this.Close();

        }

        private void excelPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void ExcelConfig_Load(object sender, EventArgs e)
        {
            excelPath.Text = Properties.Settings.Default.excelPath;
            extension.Text = Properties.Settings.Default.extension;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
