using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;
using System.Data.SQLite;
using static System.Net.WebRequestMethods;
using System.Drawing;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using BatteryMonitor.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BatteryMonitor.SQLlite
{
    class sqlitedbofff
    {
        private string _connectionString = "Data Source=SCDB.db;Version=3;";

        public void CreateDatabaseFile()
        {
            if (System.IO.File.Exists("SCDB.db")) return;
            using (var conn = new SQLiteConnection(_connectionString))
            {
                // Tạo bảng lưu dữ liệu ofline
                // Bảng pin

                // Mã Số lô hàng TEXT
                // Quy cách TEXT
                // pinCode TEXT
                // r number
                // v number
                // rMax number
                // vMax number
                // rMin number
                // vMin number
                // totalMeasureMent number
                // workShift number
                // Ngày đo date 
                // Nhân viên đo


                conn.Open();
                var command = conn.CreateCommand();

                command.CommandText = @"CREATE TABLE batteryList (
                shipmentId TEXT,
                specs TEXT,
                batteryCode TEXT ,
                r REAL,
                v REAL,
                rMax REAL,
                vMax REAL,
                rMin REAL,
                vMin REAL,
                workShift INTEGER,
                totalMeasureMent INTEGER,
                date TEXT,
                userId TEXT,
                measurementStatus TEXT,
                quality TEXT,
                cpk_R REAL,
                cpk_V REAL,
                type_R TEXT,
                type_V TEXT,
                std_R REAL,
                std_V REAL,
                ave_R REAL,
                ave_V REAL,

                CONSTRAINT constraint_pk PRIMARY KEY (batteryCode, date)
    
                 );";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE errorScanQrList (
                batteryCodeList TEXT ,
                date TEXT,

                CONSTRAINT constraint_errorScanQrList_pk PRIMARY KEY (batteryCodeList, date));";
                command.ExecuteNonQuery();


                command.CommandText = @"CREATE TABLE user(
                name TEXT ,
                password TEXT,
                CONSTRAINT constraint_user_pk PRIMARY KEY (name));";
                command.ExecuteNonQuery();

            }
        }

        public void insertBatteryList(
                    BatteryMonitor.Data.battery battery
            )
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @"
    INSERT INTO batteryList(
        shipmentId,
        specs,
        batteryCode,
        r,
        v,
        rMax,
        vMax,
        rMin,
        vMin,
        workShift,
        totalMeasureMent,
        date,
        userId,
        measureMentStatus,
        cpk_R ,
        cpk_V ,
        quality,
        type_R ,
        type_V ,
        std_R ,
        std_V ,
        ave_R ,
        ave_V 
    ) VALUES(
        $shipmentId,
        $specs,
        $batteryCode,
        $r,
        $v,
        $rMax,
        $vMax,
        $rMin,
        $vMin,
        $workShift,
        $totalMeasureMent,
        $date,
        $userId,
        $measureMentStatus,
        $quality,
        $cpk_R,
        $cpk_V,
        $type_R,
        $type_V,
        $std_R,
        $std_V,
        $ave_R,
        $ave_V
    );";

                command.Parameters.AddWithValue("$shipmentId", battery.ShipmentId);
                command.Parameters.AddWithValue("$specs", battery.Specs);
                command.Parameters.AddWithValue("$batteryCode", battery.BatteryCode);
                command.Parameters.AddWithValue("$r", Convert.ToDouble(battery.R));
                command.Parameters.AddWithValue("$v", Convert.ToDouble(battery.V));
                command.Parameters.AddWithValue("$rMax", Convert.ToDouble(battery.RMAX));
                command.Parameters.AddWithValue("$vMax", Convert.ToDouble(battery.VMAX));
                command.Parameters.AddWithValue("$rMin", Convert.ToDouble(battery.RMIN));
                command.Parameters.AddWithValue("$vMin", Convert.ToDouble(battery.VMIN));
                command.Parameters.AddWithValue("$workShift", battery.WorkShift);
                command.Parameters.AddWithValue("$totalMeasureMent", battery.TotalMeasureMent);
                command.Parameters.AddWithValue("$date", battery.Date);
                command.Parameters.AddWithValue("$userId", battery.UserId);
                command.Parameters.AddWithValue("$measureMentStatus", battery.MeasureMentStatus);
                command.Parameters.AddWithValue("$cpk_R", battery.CPK_R);
                command.Parameters.AddWithValue("$cpk_V", battery.CPK_V);
                command.Parameters.AddWithValue("$quality", battery.QUALITY);
                command.Parameters.AddWithValue("$type_R", battery.TYPE_R);
                command.Parameters.AddWithValue("$type_V", battery.TYPE_V);
                command.Parameters.AddWithValue("$std_R", battery.STD_R);
                command.Parameters.AddWithValue("$std_V", battery.STD_V);
                command.Parameters.AddWithValue("$ave_R", battery.AVE_R);
                command.Parameters.AddWithValue("$ave_V", battery.AVE_V);
                command.ExecuteNonQuery();
            }
        }

        public void insertErrorScanQrList(
                string batteryCodeList, string date
         )
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @"
    INSERT INTO errorScanQrList(       
        batteryCodeList, 
        date
    ) VALUES(     
        $batteryCodeList,
        $date
    );";

                command.Parameters.AddWithValue("$batteryCodeList", batteryCodeList);
                command.Parameters.AddWithValue("$date", date);
                command.ExecuteNonQuery();
            }
        }

        public void insertUser(
               string name, string password
        )
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @"
    INSERT INTO user(       
        name, 
        password
    ) VALUES(     
        $name,
        $password
    );";

                command.Parameters.AddWithValue("$name", name);
                command.Parameters.AddWithValue("$password", password);
                command.ExecuteNonQuery();
            }
        }

        public bool login(string name, string password)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @"SELECT COUNT(name) as isLogin WHERE name = $name AND password = $password";
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("password", password);
                int checkgiatri = Convert.ToInt32(command.ExecuteScalar());

                if (checkgiatri > 0)
                {
                    Properties.Settings.Default.userId = name;
                    Properties.Settings.Default.Save();
                    return true;
                }
                return false;
            }
        }

        public void logout()
        {
            Properties.Settings.Default.userId = "";
            Properties.Settings.Default.Save();
        }


        public DataTable getAllErrorScanBatteryList()
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @"SELECT 
                    batteryCodeList,
                    date  FROM errorScanQrList;";
                using (var reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    return dt;

                }
            }
        }

        public DataTable getErrorScanBatteryListWithCondition(string condition = null)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @"SELECT * FROM errorScanQrList;";
                if (condition != null)
                {
                    command.CommandText = @"SELECT * FROM errorScanQrList WHERE " + condition + ";";
                }
                using (var reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    return dt;

                }
            }
        }
        public void deleteErrorScanBatteryList(string condition)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();

                if (condition.Length == 0 || condition == null)
                {
                    condition = " ";
                }

                command.CommandText = @"DELETE FROM errorScanQrList WHERE " + condition + ";";
                command.ExecuteNonQuery();
            }
        }

        public DataTable getAllBatterry()
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @"SELECT * FROM batteryList;";
                using (var reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    return dt;

                }
            }
        }
        public DataTable getBatteryListWithCondition(string condition = null)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = @"SELECT * FROM batteryList;";
                if (condition != null)
                {
                    command.CommandText = @"SELECT * FROM batteryList WHERE " + condition + ";";
                }
                using (var reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    return dt;

                }
            }
        }

        public void deleteBatterryList(string condition)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();

                if (condition.Length == 0 || condition == null)
                {
                    condition = " ";
                }

                command.CommandText = @"DELETE FROM batteryList WHERE " + condition + ";";
                command.ExecuteNonQuery();
            }
        }

        public bool isExits(string condition)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var command = conn.CreateCommand();

                command.CommandText = @"SELECT count(*) FROM batteryList WHERE " + condition + ";";

                int checkgiatri = Convert.ToInt32(command.ExecuteScalar());

                if (checkgiatri > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public void updateBatteryList(BatteryMonitor.Data.battery battery, string condition)
        {

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                if (condition.Length == 0 || condition == "" || condition == null)
                {
                    return;
                }

                var command = conn.CreateCommand();

                command.CommandText = @"
    UPDATE batteryList
    SET
        shipmentId = $shipmentId,
        specs = $specs,
        r = $r,
        v = $v,
        rMax = $rMax,
        vMax = $vMax,
        rMin = $rMin,
        vMin = $vMin,
        workShift = $workShift,
        totalMeasureMent = $totalMeasureMent,
        date = $date,
        userId =  $userId,
        measureMentStatus  = $measureMentStatus,
        cpk_R = $cpk_R,
        cpk_V = $cpk_V,
        quality = $quality,
        type_R = $type_R,
        type_V = $type_V,
        std_R = $std_R,
        std_V = $std_V,
        ave_R = $ave_R,
        ave_V = $ave_V
    WHERE  date = (SELECT MAX(date) FROM batteryList WHERE " + condition + ") AND "+ condition +";";
                
                command.Parameters.AddWithValue("$shipmentId", battery.ShipmentId);
                command.Parameters.AddWithValue("$specs", battery.Specs);
                command.Parameters.AddWithValue("$r", Convert.ToDouble(battery.R));
                command.Parameters.AddWithValue("$v", Convert.ToDouble(battery.V));
                command.Parameters.AddWithValue("$rMax", Convert.ToDouble(battery.RMAX));
                command.Parameters.AddWithValue("$vMax", Convert.ToDouble(battery.VMAX));
                command.Parameters.AddWithValue("$rMin", Convert.ToDouble(battery.RMIN));
                command.Parameters.AddWithValue("$vMin", Convert.ToDouble(battery.VMIN));
                command.Parameters.AddWithValue("$workShift", battery.WorkShift);
                command.Parameters.AddWithValue("$totalMeasureMent", battery.TotalMeasureMent);
                command.Parameters.AddWithValue("$date", battery.Date);
                command.Parameters.AddWithValue("$userId", battery.UserId);
                command.Parameters.AddWithValue("$measureMentStatus", battery.MeasureMentStatus);
                command.Parameters.AddWithValue("$cpk_R", battery.CPK_R);
                command.Parameters.AddWithValue("$cpk_V", battery.CPK_V);
                command.Parameters.AddWithValue("$quality", battery.QUALITY);
                command.Parameters.AddWithValue("$type_R", battery.TYPE_R);
                command.Parameters.AddWithValue("$type_V", battery.TYPE_V);
                command.Parameters.AddWithValue("$std_R", battery.STD_R);
                command.Parameters.AddWithValue("$std_V", battery.STD_V);
                command.Parameters.AddWithValue("$ave_R", battery.AVE_R);
                command.Parameters.AddWithValue("$ave_V", battery.AVE_V);

                command.ExecuteNonQuery();
            }
        }

        public void ExportSqliteDataToExcel(string excelForder ,string condition)
        {
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                // Truy vấn dữ liệu từ bảng
                string query = "SELECT * FROM batteryList WHERE " + condition;
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataReader dr = cmd.ExecuteReader();

                // Tạo workbook mới cho tệp Excel
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Sheet1");

                // Tạo tiêu đề cột
                IRow headerRow = sheet.CreateRow(0);
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(dr.GetName(i));
                }

                // Ghi dữ liệu từ SQLite vào tệp Excel
                int rowIndex = 1;
                while (dr.Read())
                {
                    IRow row = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        var cell = row.CreateCell(i);
                        var val = dr[i];
                        if (val is int)
                        {
                            cell.SetCellValue((int)val);
                        }
                        else if (val is double)
                        {
                            cell.SetCellValue((double)val);
                        }
                        else if (val is bool)
                        {
                            cell.SetCellValue((bool)val);
                        }
                        else // assume it's a string for this example
                        {
                            cell.SetCellValue(val.ToString());
                        }
                    }
                    rowIndex++;
                }

                string extension = Properties.Settings.Default.extension;

                string excelFileName = System.DateTime.Now.ToString("yyyyMMddHH24miss") + "." + extension;

                // Lưu workbook vào đường dẫn cụ thể
                using (var fileData = new FileStream(excelForder + "\\" + excelFileName, FileMode.Create))
                {
                    workbook.Write(fileData);
                }
            }
        }

    }
}
