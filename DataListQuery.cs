using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BatteryMonitor.SQLlite;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace BatteryMonitor
{

    public partial class DataListQuery : Form
    {
       
        public DataListQuery()
        {
            InitializeComponent();
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripbtn_qry_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            if( cb_maslh.SelectedItem == null )
            {
                MessageBox.Show("Vui lòng chọn mã số lô hàng");
                cb_maslh.Focus();
                return;
            }


            DateTime startDate = dtp_begin.Value;
            DateTime endDate = dtp_end.Value;
            string connectionString = "Data Source=SCDB.db;Version=3;";
            string query = @"SELECT shipmentId, specs, batteryCode, r, v, rMax, vMax, rMin, vMin, workShift, totalMeasureMent, date, userId, measureMentStatus, quality, cpk_R, cpk_V, type_R, type_V, std_R, std_V, ave_R, ave_V " +
                           " FROM batteryList WHERE  workShift = @workShift AND date BETWEEN  @begin AND @end  AND shipmentId LIKE @shipmentId";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(query, connection);
   
                command.Parameters.AddWithValue("@begin", startDate.ToString("yyyy/MM/dd HH:mm:ss"));
                command.Parameters.AddWithValue("@end", endDate.ToString("yyyy/MM/dd HH:mm:ss"));
                command.Parameters.AddWithValue("@workShift", cb_CaLamViec.SelectedItem);
                string shipmentQuery = cb_maslh.SelectedItem.ToString();

                if ( cb_maslh.SelectedItem.Equals("ALL"))
                {
                    shipmentQuery = "%";
                }

                command.Parameters.AddWithValue("@shipmentId", shipmentQuery);


                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);

                    dataGridView1.DataSource = dataTable;
                }
            }
        }

       

        private void btn_export_Click(object sender, EventArgs e)
        {
            var exporter = new ExportToExcel();
            exporter.ExportDataGridViewToExcel(dataGridView1, Properties.Settings.Default.excelPath);
        }

        private void DataListQuery_Load(object sender, EventArgs e)
        {
            cb_CaLamViec.SelectedIndex = 0;

            cb_maslh.SelectedIndex = 0;
            DateTime startDate = dtp_begin.Value;
            DateTime endDate = dtp_end.Value;

            string connectionString = "Data Source=SCDB.db;Version=3;";
            string query = @"SELECT DISTINCT shipmentId" +
                           " FROM batteryList WHERE  workShift = @workShift AND date BETWEEN  @begin AND @end ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.Parameters.AddWithValue("@begin", startDate.ToString("yyyy/MM/dd HH:mm:ss"));
                command.Parameters.AddWithValue("@end", endDate.ToString("yyyy/MM/dd HH:mm:ss"));
                command.Parameters.AddWithValue("@workShift", cb_CaLamViec.SelectedItem);
                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetValue(0);
                        cb_maslh.Items.Add(id);    
                    }
                }
            }




        }

        private void dtp_begin_ValueChanged(object sender, EventArgs e)
        {
            cb_maslh.Items.Clear();
            cb_maslh.Items.Add("ALL");

            DateTime startDate = dtp_begin.Value;
            DateTime endDate = dtp_end.Value;

            string connectionString = "Data Source=SCDB.db;Version=3;";
            string query = @"SELECT DISTINCT shipmentId" +
                           " FROM batteryList WHERE  workShift = @workShift AND date BETWEEN  @begin AND @end ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.Parameters.AddWithValue("@begin", startDate.ToString("yyyy/MM/dd HH:mm:ss"));
                command.Parameters.AddWithValue("@end", endDate.ToString("yyyy/MM/dd HH:mm:ss"));
                command.Parameters.AddWithValue("@workShift", cb_CaLamViec.SelectedItem);
                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetValue(0);
                        cb_maslh.Items.Add(id);
                    }
                }
                cb_maslh.SelectedIndex = 0;
            }
        }

        private void dtp_end_ValueChanged(object sender, EventArgs e)
        {
            cb_maslh.Items.Clear();
            cb_maslh.Items.Add("ALL");

            DateTime startDate = dtp_begin.Value;
            DateTime endDate = dtp_end.Value;

            string connectionString = "Data Source=SCDB.db;Version=3;";
            string query = @"SELECT DISTINCT shipmentId" +
                           " FROM batteryList WHERE  workShift = @workShift AND date BETWEEN  @begin AND @end ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.Parameters.AddWithValue("@begin", startDate.ToString("yyyy/MM/dd HH:mm:ss"));
                command.Parameters.AddWithValue("@end", endDate.ToString("yyyy/MM/dd HH:mm:ss"));
                command.Parameters.AddWithValue("@workShift", cb_CaLamViec.SelectedItem);
                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetValue(0);
                        cb_maslh.Items.Add(id);
                    }
                }
                cb_maslh.SelectedIndex = 0;
            }
        }

        private void cb_CaLamViec_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_maslh.Items.Clear();
            cb_maslh.Items.Add("ALL");

            DateTime startDate = dtp_begin.Value;
            DateTime endDate = dtp_end.Value;

            string connectionString = "Data Source=SCDB.db;Version=3;";
            string query = @"SELECT DISTINCT shipmentId" +
                           " FROM batteryList WHERE  workShift = @workShift AND date BETWEEN  @begin AND @end ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.Parameters.AddWithValue("@begin", startDate.ToString("yyyy/MM/dd HH:mm:ss"));
                command.Parameters.AddWithValue("@end", endDate.ToString("yyyy/MM/dd HH:mm:ss"));
                command.Parameters.AddWithValue("@workShift", cb_CaLamViec.SelectedItem);
                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetValue(0);
                        cb_maslh.Items.Add(id);
                    }
                }
                cb_maslh.SelectedIndex = 0;

            }
        }

    }
}
