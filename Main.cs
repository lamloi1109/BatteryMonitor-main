﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using BatteryMonitor.Chart;
using EasyModbus;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using LiveCharts.Definitions.Charts;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Microsoft.Win32;
using System.Windows.Forms.VisualStyles;
using System.Windows.Shell;
using System.Windows.Media;
using LiveCharts.Geared;
using BatteryMonitor.Data;
using System.Windows.Ink;
using Keyence.AutoID.SDK;
using Oracle.ManagedDataAccess.Client;
using TimersTimer = System.Timers.Timer;
using System.Net.NetworkInformation;
using BatteryMonitor.SQLlite;
using BatteryMonitor.HIOKI;
using System.Reflection;
using System.Security.Cryptography;

using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Formula.Functions;
//using OPCAutomation;
using System.ArrayExtensions;
using Krypton.Toolkit;
using System.Data.SQLite;
//using ActUtlTypeLib;

namespace BatteryMonitor
{

    public partial class Main : Form
    {



        private ReaderAccessor m_reader = new ReaderAccessor();

        // NLog
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // QUEUE 
        Queue<BatteryMonitor.Data.battery> queueBattery = new Queue<BatteryMonitor.Data.battery>();

        // QUEUE UI
        Queue<BatteryMonitor.Data.battery> queueUI = new Queue<BatteryMonitor.Data.battery>();

        // tạo một mảng đổi tượng battery
        List<BatteryMonitor.Data.battery> batteryList = new List<BatteryMonitor.Data.battery>();

        // timer reset về đèn vàng (Chờ)
        TimersTimer _LightTimer = new TimersTimer();

        //// Timer kiểm tra kết nối mắt quét
        TimersTimer checkKeygenceConnectTimer = new TimersTimer();

        //// Timer kiểm tra kết nối tới modbust
        //TimersTimer checkModbustConnectTimer = new TimersTimer();

        //// Timer kiểm tra kết nối tới HIOKI
        //TimersTimer checkHiokiConnectTimer = new TimersTimer();

        //// Timer nhận giá trị từ HIOKI
        TimersTimer getMeasurementValueHiokiTimer = new TimersTimer();

        //// Timer ghi giá trị lên modbust
        TimersTimer writeToModbustTimer = new TimersTimer();
        TimersTimer writeTypePinTimer = new TimersTimer();


        //// Timer xuất báo cáo
        TimersTimer exportExcelTimer = new TimersTimer();

        // timer dùng để ghi nhận các biến trên thanh ghi của modbus
        System.Windows.Forms.Timer monitorModbusTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timerShowText = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timerShowphanloaiV = new System.Windows.Forms.Timer();


        // timer kiểm tra xem đã đo bình xong chưa để đưa các bình trong hàng đợi lên UI
        System.Windows.Forms.Timer uiTimer = new System.Windows.Forms.Timer();

        // timer dùng để cập nhật các UI của chart
        System.Windows.Forms.Timer chartTimer = new System.Windows.Forms.Timer();

        // timer dùng để cập nhật các UI của chart
        //System.Windows.Forms.Timer chartTimer_V = new System.Windows.Forms.Timer();



        // timer dùng để cập nhật các UI của chart
        System.Windows.Forms.Timer pinGroupUITimer = new System.Windows.Forms.Timer();

        //timer giả lập quét qrcode
        //System.Windows.Forms.Timer scanQrcodeTimer = new System.Windows.Forms.Timer();

        // Timer reset đồ thị khi hết ca
        System.Windows.Forms.Timer resetChartTimer = new System.Windows.Forms.Timer();

        // BackgroundWorker
        BackgroundWorker bgwork_check_keygence_connect, bgwork_check_modbust_connect, bgwork_check_hioki_connect, bgwork_light, bgwork_getMeasurementValueHioki, bgwork_writeToModbust, bgwork_exportExcelTimer, bgwork_writeTypePin;

        // Kiểm tra xem đã nhận được biến bằng 1 hãy chưa
        public bool isMeasureMentSucceed = false;
        // Kiểm tra xem đã hết ca 1 hay chưa
        public bool isEndShift1 = false;
        // Kiểm tra xem đã hết ca 2 hay chưa
        public bool isEndShift2 = false;
        // Đã hết ca 1 chưa
        public int endShift1 = 0;
        // Đã hết ca 2 chưa
        public int endShift2 = 0;
        // Đã đo bình xong chưa
        public int hiokiMeasureMentSuceed = 0;

        // Xóa Chart hay không
        public int isDeleteChart = 0;
        // Xóa CPK hay không
        public int isDeleteCPK = 0;
        // Xuất báo cáo hay chưa
        public int isExportDataReport = 0;
        // Flag xuất báo cáo
        bool isExportDataFlag = false;

        public int isResetLamp = 0;

        // HIOKI SOCKET

        static string hiokiIp = Properties.Settings.Default.hiokiIP;
        static string hiokiPort = Properties.Settings.Default.hiokiPort;
        static public TcpClient LanSocket;
        static public hioki hiokiSocket = new hioki(hiokiIp, hiokiPort, LanSocket);

        // status 
        Boolean onLineStatus, SetScannerStatus, chkprtconnted;

        //List<string> uiListPrepare = new List<string>();

        //
        Nofitication nofiticationForm = new Nofitication("");

        public const int MaxDataPoints = 16;
        public const int MinAxisValue = 0;
        sqlitedbofff sqlLite = new sqlitedbofff();


        bool scanError = false;

        bool isScannedQrcode = false;

        bool isConnectedToModbust = false;

        //string receivedData = "";

        public Main()
        {
            InitializeComponent();
        }
        public static ChartValues<MeasureModel> ChartValuesR { get; set; } = new ChartValues<MeasureModel>();
        public static ChartValues<MeasureModel> ChartValuesV { get; set; } = new ChartValues<MeasureModel>();
        public LineSeries rMaxSeries { get; set; } = new LineSeries
        {
            Values = new ChartValues<MeasureModel>
        {
            new MeasureModel
            {
                times = 0,
                Value = 0,
            },
            new MeasureModel
            {
                times = int.MaxValue,
                Value = 0,
            }
        }
        };

        public LineSeries rMinSeries { get; set; } = new LineSeries
        {
            Values = new ChartValues<MeasureModel>
        {
            new MeasureModel
            {
                times = 0,
                Value = 0,
            },
            new MeasureModel
            {
                times = int.MaxValue,
                Value = 0,
            }
        }
        };
        public LineSeries vMaxSeries { get; set; } = new LineSeries
        {
            Values = new ChartValues<MeasureModel>
        {
            new MeasureModel
            {
                times = 0,
                Value = 0,
            },
            new MeasureModel
            {
                times = int.MaxValue,
                Value = 0,
            }
        }
        };
        public LineSeries vMinSeries { get; set; } = new LineSeries
        {
            Values = new ChartValues<MeasureModel>
        {
            new MeasureModel
            {
                times = 0,
                Value = 0,
            },
            new MeasureModel
            {
                times = int.MaxValue,
                Value = 0,
            }
        }
        };

        //public Random R { get; set; }

        //ModbusClient MenuForm.modbustMainForm = new ModbusClient();

        // Giá trị đọc R đọc từ HIOKI
        //double rHioki = 0.0;
        // Giá trị đọc V đọc từ HIOKI
        //double vHioki = 0.0;

        // Mã số lô hàng
        string batchString = "";
        // Người thao tác
        string userString = "";
        // Quy cách
        string specsString = "";
        // Số ca
        string currentShiftString = "";

        // Số bình tối đa trong một cụm bình 
        int maxGroupPin = 4;

        int pinCountSetting = Properties.Settings.Default.CountPin + 1;

        // Tổng bình ca 1`
        int totalBatteryShift1 = Properties.Settings.Default.totalPinShift1;
        // Tổng bình ca 2
        int totalBatteryShift2 = Properties.Settings.Default.totalPinShift2;
        // Ca hiện tại
        int currentShift = 1;
        // Xóa Chart hay không
        //int isDeleteChart = 0;
        // Xóa CPK hay không
        //int isDeleteCPK = 0;
        // Đã hết ca 1 chưa
        //int MenuForm.endShift1 = 0;
        // Đã hết ca 2 chưa
        //int MenuForm.endShift2 = 0;
        // Đã đo bình xong chưa
        //int MenuForm.hiokiMeasureMentSuceed = 0;
        // Có thêm dữ liệu vào đồ thị không
        int isAddDataToChart = 0;
        // R của bình hiện tại
        double currentR = 0;
        // V của bình hiện tại
        double currentV = 0;
        // Rmax
        double rMax = Properties.Settings.Default.rMax_setting;
        // Rmin
        double rMin = Properties.Settings.Default.rMin_setting;
        // Vmax
        double vMax = Properties.Settings.Default.vMax_setting;
        // Vmin
        double vMin = Properties.Settings.Default.vMin_setting;
        // Số bình đạt R trong ca 1
        int totalGoodRBatteryShift1 = 0;
        // Số bình đạt R trong ca 2
        int totalGoodRBatteryShift2 = 0;
        // Số bình cao R trong ca 1
        int totalHightRBatteryShift1 = 0;
        // Số bình cao R trong ca 2
        int totalHightRBatteryShift2 = 0;
        // Số bình thấp R trong ca 1
        int totalLowRBatteryShift1 = 0;
        // Số bình thấp R trong ca 2
        int totalLowRBatteryShift2 = 0;

        // Số bình đạt V trong ca 1
        int totalGoodVBatteryShift1 = 0;
        // Số bình đạt V trong ca 2
        int totalGoodVBatteryShift2 = 0;
        // Số bình cao V trong ca 1
        int totalHightVBatteryShift1 = 0;

        int vA_Ca1 = 0;
        int vB_Ca1 = 0;
        int vC_Ca1 = 0;
        int vD_Ca1 = 0;
        int vNG_Ca1 = 0;

        // Số bình cao V trong ca 2
        int totalHightVBatteryShift2 = 0;
        // Số bình thấp V trong ca 1
        int totalLowVBatteryShift1 = 0;
        // Số bình thấp V trong ca 2
        int totalLowVBatteryShift2 = 0;

        int vA_Ca2 = 0;
        int vB_Ca2 = 0;
        int vC_Ca2 = 0;
        int vD_Ca2 = 0;
        int vNG_Ca2 = 0;

        public static long deccc1;
        bool isShowedUIList = false;
        bool isShowedChart = true;
        // Kiểm tra lần đầu có load lại giới hạn hay chưa
        bool isSetLimitedRChart = false;
        bool isSetLimitedVChart = false;
        // Tính khoảng cách trên trục Y của hai đồ thị
        double stepR = 1;
        double stepV = 1;
        // Kiểm tra xem đã thêm điểm vào đồ thị hay chưa
        bool isDrawedChartR = false;
        bool isDrawedChartV = false;

        // RmaxOld
        double rMaxOld = -1;
        // Rmin
        double rMinOld = -1;
        // Vmax
        double vMaxOld = -1;
        // Vmin
        double vMinOld = -1;
        // Set standard
        bool isSetStandardRLine = false;
        bool isSetStandardVLine = false;

        int pinCurrentColor = 0;
        // Tổng giá trị điện trở của ca hiện tại
        double totalR = 0.0;
        // Tổng giá trị điện áp của ca hiện tại
        double totalV = 0.0;
        // ZI_R
        double ZI_R = 0.0;
        double ZI_V = 0.0;
        // isDequeue
        bool isDeQueue = false;

        // Danh sách chứa các biến dùng để chuyển thành int16 và ghi lên modbust
        List<string> convertToInt16List = new List<string>();

        // Danh sách chứa các chuỗi đọc được từ mắt quét
        List<List<string>> stringScanList = new List<List<string>>();

        ModbusClient modbustReadValue = new ModbusClient();
        //ModbusClient modbustWriteTypePin = new ModbusClient();

        ModbusClient modbustMainForm = new ModbusClient();
        ModbusClient modbustWrite = new ModbusClient();

        static public bool isConnectedToHioki = false;

        const double DefaultMeasurementValue = 0.0;
        const int DefaultTotalMeasurement = 0;
        const int DefaultWorkShift = 0;
        static string DefaultMeasurementStatus = "Pending";
        const string EmptyString = "";

        //TEST
        int[] testArray = new int[122];
        int[] numArray = new int[122];

        BatteryMonitor.Data.battery currentPin = new battery(
                                 R: DefaultMeasurementValue,
                                 V: DefaultMeasurementValue,
                                 RMax: DefaultMeasurementValue,
                                 RMin: DefaultMeasurementValue,
                                 VMax: DefaultMeasurementValue,
                                 Vmin: DefaultMeasurementValue,
                                 totalMeasureMent: DefaultTotalMeasurement,
                                 BatteryCode: EmptyString,
                                 UserId: EmptyString,
                                 ShipmentId: EmptyString,
                                 Specs: EmptyString,
                                 WorkShift: DefaultWorkShift,
                                 MeasureMentStatus: DefaultMeasurementStatus,
                                 Date: EmptyString,
                                 quality: EmptyString,
                                 type_R: EmptyString,
                                 type_V: EmptyString,
                                 cpk_R: DefaultMeasurementValue,
                                 cpk_V: DefaultMeasurementValue,
                                 std_R: DefaultMeasurementValue,
                                 std_V: DefaultMeasurementValue,
                                 ave_R: DefaultMeasurementValue,
                                 ave_V: DefaultMeasurementValue
                             );

        private void SetAxisLimits(LiveCharts.WinForms.CartesianChart chart, int number)
        {

            chart.Invoke((MethodInvoker)delegate
            {
                try
                {
                    // Kiểm tra xem các giá trị max và min có thay đổi hay không
                    // Lấy last value trong list battery
                    //queueBattery.Count > 0 && batteryList.Count > 0

                    if (queueBattery.Count > 0 && batteryList.Count > 0)
                    {
                        // lấy trong batteryList ra lần đo gần nhất
                        BatteryMonitor.Data.battery battery = batteryList.Last();

                        // Cập nhật STD, CPK, AVE
                        // std ave cpk 3 4 5

                        if (chart == cartesianChart1)
                        {
                            if (chart.Series.Count >= 3 && chart.Series.Count <= 6 && chart.Series[3] is LineSeries lineSeriesSTDR)
                            {
                                lineSeriesSTDR.Title = "STD: " + battery.STD_V.ToString("N3");
                            }

                            if (chart.Series.Count >= 3 && chart.Series.Count <= 6 && chart.Series[4] is LineSeries lineSeriesCPKR)
                            {
                                lineSeriesCPKR.Title = "CPK: " + battery.CPK_V.ToString("N3");
                            }

                            if (chart.Series.Count >= 3 && chart.Series.Count <= 6 && chart.Series[5] is LineSeries lineSeriesAVER)
                            {
                                lineSeriesAVER.Title = "AVE: " + battery.AVE_V.ToString("N3");
                            }
                        }



                        if (chart == cartesianChart2)
                        {
                            if (chart.Series.Count >= 3 && chart.Series.Count <= 6 && chart.Series[3] is LineSeries lineSeriesSTDV)
                            {
                                lineSeriesSTDV.Title = "STD: " + battery.STD_R.ToString("N3");
                            }

                            if (chart.Series.Count >= 3 && chart.Series.Count <= 6 && chart.Series[4] is LineSeries lineSeriesCPKV)
                            {
                                lineSeriesCPKV.Title = "CPK: " + battery.CPK_R.ToString("N3");
                            }

                            if (chart.Series.Count >= 3 && chart.Series.Count <= 6 && chart.Series[5] is LineSeries lineSeriesAVEV)
                            {
                                lineSeriesAVEV.Title = "AVE: " + battery.AVE_R.ToString("N3");
                            }
                        }

                        //// Vẽ lại khi giá trị min max thay đổi
                        //if ((battery.RMAX != rMax || battery.RMIN != rMin) || (battery.VMAX != vMax || battery.VMIN != vMin) || (!isSetLimitedRChart || !isSetLimitedVChart))
                        //{

                        //    if (chart == cartesianChart1)
                        //    {

                        //        UpdateChart(cartesianChart1, rMaxSeries, rMinSeries, rMax, rMin);
                        //        isSetLimitedRChart = true;

                        //        if (!double.IsNaN(rMax) && !double.IsNaN(rMin))
                        //        {
                        //            stepR = (rMax - rMin) / 8;
                        //        }

                        //        Separator separator = new Separator
                        //        {
                        //            //Thiết lập các thuộc tính cho Separator
                        //            Step = stepR,
                        //        };

                        //        chart.AxisY[0].Separator = separator;
                        //        // Xét giới hạn cho trục Y
                        //        double minValue = rMin;
                        //        double maxValue = rMax;

                        //        if (!double.IsNaN(stepR) && !double.IsNaN(rMin) && !double.IsNaN(rMax))
                        //        {
                        //            minValue = rMin - (2 * stepR);
                        //            maxValue = rMax + (2 * stepR);
                        //        }

                        //        chart.AxisY[0].MinValue = minValue;
                        //        chart.AxisY[0].MaxValue = maxValue;
                        //    }
                        //    else
                        //    {
                        //        UpdateChart(cartesianChart2, vMaxSeries, vMinSeries, vMax, vMin);
                        //        isSetLimitedVChart = true;
                        //        if (!double.IsNaN(vMax) && !double.IsNaN(vMin))
                        //        {
                        //            stepV = (vMax - vMin) / 8;
                        //        }

                        //        Separator separator = new Separator
                        //        {
                        //            // Thiết lập các thuộc tính cho Separator
                        //            Step = stepV, // Khoảng cách giữa các Separator
                        //                          //StrokeThickness = 1, // Độ dày của Separator
                        //            IsEnabled = true

                        //        };
                        //        chart.AxisY[0].Separator = separator;
                        //        // Xét giới hạn cho trục Y

                        //        double minValue = vMin;
                        //        double maxValue = vMax;

                        //        if (!double.IsNaN(stepV) && !double.IsNaN(vMin) && !double.IsNaN(vMax))
                        //        {
                        //            minValue = vMin - (2 * stepV);
                        //            maxValue = vMax + (2 * stepV);
                        //        }

                        //        chart.AxisY[0].MinValue = minValue;
                        //        chart.AxisY[0].MaxValue = maxValue;
                        //        // chart.AxisY[0].LabelFormatter = value => (value % stepV == 0) ? value.ToString() : "";

                        //    }
                        //}



                    }

                    chart.AxisX[0].MaxValue = number + 1;

                    // khi chưa đặt minPoint
                    if (Double.IsNaN(chart.AxisX[0].MinValue))
                    {
                        chart.AxisX[0].MinValue = number;
                    }

                    // Xử lý khi số point quá 16 điểmF
                    if (number - chart.AxisX[0].MinValue > MaxDataPoints && chart.Series[0].Values.Count > MaxDataPoints)
                    {
                        chart.AxisX[0].MinValue = number - MaxDataPoints + 1;
                    }

                    // Nếu min trên trục X là 0 thì gán về cho tổng bình
                    if (chart.AxisX[0].MinValue == 0)
                    {
                        chart.AxisX[0].MinValue = number;
                    }

                    // khi số bình là 0
                    if (number == 0)
                    {
                        chart.AxisX[0].MinValue = number;
                    }

                }
                catch (Exception ex)
                {
                    // Log the exception or notify the user
                    logger.Error(ex);
                    Console.WriteLine(ex.Message);
                    //chartTimer.Stop();
                    //monitorModbusTimer.Stop();
                }
            });
        }



        private void monitorChartR(LiveCharts.WinForms.CartesianChart chart)
        {

            chart.Invoke((MethodInvoker)delegate
            {
                try
                {

                    if (chart == cartesianChart1 && rMaxOld == rMax && rMinOld == rMin && isSetStandardRLine)
                    {
                        return;
                    }

                    isSetLimitedRChart = false;

                    if (chart == cartesianChart1)
                    {
                        UpdateChart(cartesianChart1, rMaxSeries, rMinSeries, rMax, rMin);

                        if (!double.IsNaN(rMax) && !double.IsNaN(rMin))
                        {
                            stepR = (rMax - rMin) / 8;
                        }

                        Separator separator = new Separator
                        {
                            //Thiết lập các thuộc tính cho Separator
                            Step = stepR,
                        };

                        chart.AxisY[0].Separator = separator;
                        // Xét giới hạn cho trục Y
                        double minValue = rMin;
                        double maxValue = rMax;

                        if (!double.IsNaN(stepR) && !double.IsNaN(rMin) && !double.IsNaN(rMax))
                        {
                            minValue = rMin - (2 * stepR);
                            maxValue = rMax + (2 * stepR);
                        }

                        chart.AxisY[0].MinValue = minValue;
                        chart.AxisY[0].MaxValue = maxValue;

                        rMaxOld = rMax;

                        rMinOld = rMin;
                        isSetLimitedRChart = true;

                    }

                }



                catch (Exception ex)
                {
                    // Log the exception or notify the user
                    logger.Error(ex);
                    Console.WriteLine(ex.Message);
                    //chartTimer.Stop();
                    //monitorModbusTimer.Stop();
                }
            });



        }


        private void monitorChartV(LiveCharts.WinForms.CartesianChart chart)
        {

            chart.Invoke((MethodInvoker)delegate
            {
                try
                {

                    if (chart == cartesianChart2 && vMaxOld == vMax && vMinOld == vMin && isSetStandardVLine)
                    {
                        return;
                    }

                    isSetLimitedVChart = false;


                    if (chart == cartesianChart2)
                    {
                        UpdateChart(cartesianChart2, vMaxSeries, vMinSeries, vMax, vMin);
                        if (!double.IsNaN(vMax) && !double.IsNaN(vMin))
                        {
                            stepV = (vMax - vMin) / 8;
                        }

                        Separator separator = new Separator
                        {
                            // Thiết lập các thuộc tính cho Separator
                            Step = stepV, // Khoảng cách giữa các Separator
                                          //StrokeThickness = 1, // Độ dày của Separator
                            IsEnabled = true

                        };
                        chart.AxisY[0].Separator = separator;
                        // Xét giới hạn cho trục Y

                        double minValue = vMin;
                        double maxValue = vMax;

                        if (!double.IsNaN(stepV) && !double.IsNaN(vMin) && !double.IsNaN(vMax))
                        {
                            minValue = vMin - (2 * stepV);
                            maxValue = vMax + (2 * stepV);
                        }

                        chart.AxisY[0].MinValue = minValue;
                        chart.AxisY[0].MaxValue = maxValue;

                        vMaxOld = vMax;

                        vMinOld = vMin;

                        isSetLimitedVChart = true;
                    }
                }

                catch (Exception ex)
                {
                    // Log the exception or notify the user
                    logger.Error(ex);
                    Console.WriteLine(ex.Message);
                }
            });



        }

        private void tongtheophanloai()
        {
            string connection = "Data Source=SCDB.db;Version=3;";
            using (var conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string max_timeCa1 = "";
                string sqlite_string = "SELECT MAX(date)  "+
                                        "FROM batteryList WHERE workShift=1 ;";

                SQLiteCommand cmd_maxtime = new SQLiteCommand(sqlite_string, conn);
                SQLiteDataReader reader_maxtime = cmd_maxtime.ExecuteReader();
                while (reader_maxtime.Read())
                {
                    var value = reader_maxtime.GetValue(0);
                    max_timeCa1 = value != DBNull.Value ? value.ToString() : DateTime.Now.ToString("yyyy/MM/dd 00:00:00");

                }

                sqlite_string = "SELECT SUM(CASE WHEN type_V='A' THEN 1 ELSE 0 END ) AS A," +
                                        " SUM(CASE WHEN type_V='B' THEN 1 ELSE 0 END ) AS B," +
                                        " SUM(CASE WHEN type_V='C' THEN 1 ELSE 0 END ) AS C," +
                                        " SUM(CASE WHEN type_V='D' THEN 1 ELSE 0 END ) AS D," +
                                        " SUM(CASE WHEN type_V='NG' THEN 1 ELSE 0 END ) AS NG "+
                                        "FROM batteryList WHERE date >= '"+DateTime.Now.ToString("yyyy/MM/dd 00:00:00")+ "' AND workShift = 1  ;";

                SQLiteCommand cmdca1 = new SQLiteCommand(sqlite_string, conn);
                SQLiteDataReader readerca1 = cmdca1.ExecuteReader();
                while (readerca1.Read())
                {
                    vA_Ca1 = readerca1.IsDBNull(0) ? 0 : readerca1.GetInt32(0);
                    vB_Ca1 = readerca1.IsDBNull(1) ? 0 : readerca1.GetInt32(1);
                    vC_Ca1 = readerca1.IsDBNull(2) ? 0 : readerca1.GetInt32(2);
                    vD_Ca1 = readerca1.IsDBNull(3) ? 0 : readerca1.GetInt32(3);
                    vNG_Ca1 = readerca1.IsDBNull(4) ? 0 : readerca1.GetInt32(4);

                }
                sqlite_string = "SELECT SUM(CASE WHEN type_V='A' THEN 1 ELSE 0 END ) AS A," +
                                       " SUM(CASE WHEN type_V='B' THEN 1 ELSE 0 END ) AS B," +
                                       " SUM(CASE WHEN type_V='C' THEN 1 ELSE 0 END ) AS C," +
                                       " SUM(CASE WHEN type_V='D' THEN 1 ELSE 0 END ) AS D," +
                                       " SUM(CASE WHEN type_V='NG' THEN 1 ELSE 0 END ) AS NG " +
                                       "FROM batteryList WHERE date >= '" + max_timeCa1 + "' AND workShift = 2  ;";

                SQLiteCommand cmd = new SQLiteCommand(sqlite_string, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vA_Ca2 = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    vB_Ca2 = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                    vC_Ca2 = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    vD_Ca2 = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    vNG_Ca2 = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);

                }
            }
            // Tổng Volt loai A
            UpdateLabelText(V_ACa1, vA_Ca1.ToString());
            //Ca 1
            // Tổng Volt loai B
            UpdateLabelText(V_BCa1, vB_Ca1.ToString());

            // Tổng Volt loai C
            UpdateLabelText(V_CCa1, vC_Ca1.ToString());

            // Tổng Volt loai D
            UpdateLabelText(V_DCa1, vD_Ca1.ToString());

            // Tổng Volt loai NG
            UpdateLabelText(V_NGCa1, vNG_Ca1.ToString());


            //Ca 2
            // Tổng Volt loai A
            UpdateLabelText(V_ACa2, vA_Ca2.ToString());

            // Tổng Volt loai B
            UpdateLabelText(V_BCa2, vB_Ca2.ToString());

            // Tổng Volt loai C
            UpdateLabelText(V_CCa2, vC_Ca2.ToString());

            // Tổng Volt loai D
            UpdateLabelText(V_DCa2, vD_Ca2.ToString());

            // Tổng Volt loai NG
            UpdateLabelText(V_NGCa2, vNG_Ca2.ToString());

        }


        private void UpdateChart(LiveCharts.WinForms.CartesianChart chart, LineSeries maxSeries, LineSeries minSeries, double max, double min)
        {
            try
            {

                if (chart == cartesianChart1 && rMaxOld == rMax && rMinOld == rMin && isSetStandardRLine)
                {
                    return;
                }

                if (chart == cartesianChart2 && vMaxOld == vMax && vMinOld == vMin && isSetStandardVLine)
                {
                    return;
                }

                ChartValues<MeasureModel> maxValues = new ChartValues<MeasureModel>
                {
                    new MeasureModel
                    {
                        times = 0,
                        Value = max,
                    },
                    new MeasureModel
                    {
                        times = int.MaxValue,
                        Value = min,
                    }
                };

                ChartValues<MeasureModel> minValues = new ChartValues<MeasureModel>
                {
                    new MeasureModel
                    {
                        times = 0,
                        Value = min,
                    },
                    new MeasureModel
                    {
                        times = int.MaxValue,
                        Value = min,
                    }
                };

                if (maxSeries == null || minSeries == null || maxValues == null || minValues == null)
                {
                    return;
                }

                maxSeries.Values = maxValues;
                minSeries.Values = minValues;

                if (chart == cartesianChart1)
                {
                    isSetStandardRLine = true;
                }

                if (chart == cartesianChart2)
                {
                    isSetStandardVLine = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex);
            }

        }

        public float Twoint16ConverttoFloat(int Val1, int Val2)
        {
            byte[] bytes = BitConverter.GetBytes(Val1);
            byte[] numArray = BitConverter.GetBytes(Val2);
            byte[] numArray1 = new byte[] { bytes[0], bytes[1], numArray[0], numArray[1] };
            return BitConverter.ToSingle(numArray1, 0);
        }

        private ushort[] FloatToModbusRegisters(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes); // Đảm bảo byte order đúng định dạng Little Endian
            }

            ushort lowOrderValue = BitConverter.ToUInt16(bytes, 0);
            ushort highOrderValue = BitConverter.ToUInt16(bytes, 2);

            // Trong Modbus, đôi khi các giá trị cao (high-order) được gửi trước giá trị thấp (low-order)
            // Ví dụ: return new ushort[] { highOrderValue, lowOrderValue };
            return new ushort[] { lowOrderValue, highOrderValue };
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
               // ChartConfig(cartesianChart1);
               // ChartConfig(cartesianChart2);
                tongtheophanloai();
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string productName = Application.ProductName;
                this.Text = productName + " Chart " + version.ToString() + "Version: 24.05.22.01";
                this.Resize += MainForm_Resize;

                // Set PinGroup List layout
                if (maxGroupPin == 4)
                {
                    for (int i = maxGroupPin + 4; i > 3; i--)
                    {
                        if (i!=8)
                        {
                            RemoveColumn(tableLayout_measure, i);
                            RemoveColumn(tableLayout_QR, i);
                        }
                       
                    }
                }


                //TEST
                //modbustReadValue.IPAddress = Properties.Settings.Default.modbustIP;
                //modbustReadValue.Port = Properties.Settings.Default.modbustPort;
                //modbustReadValue.Connect();

                //modbustWriteTypePin.IPAddress = Properties.Settings.Default.modbustIP;
                //modbustWriteTypePin.Port = Properties.Settings.Default.modbustPort;
                //modbustWriteTypePin.Connect();



                modbustMainForm.IPAddress = Properties.Settings.Default.modbustIP;
                modbustMainForm.Port = Properties.Settings.Default.modbustPort;
                modbustMainForm.Connect();

                modbustWrite.IPAddress = Properties.Settings.Default.modbustIP;
                modbustWrite.Port = Properties.Settings.Default.modbustPort;
                modbustWrite.Connect();

                //TEST
                if (!modbustMainForm.Connected || !modbustWrite.Connected)
                {
                    modbustWrite.WriteSingleRegister(97, 1);

                    (new Nofitication("Kết nối tới proface thất bại!")).ShowDialog();
                    return;
                }

                // Kết nối đến HIOKI               
                isConnectedToHioki = hiokiSocket.TryConnectToTcpServer();

                // Kết nối đến HIOKI
                if (!isConnectedToHioki)
                {
                    modbustWrite.WriteSingleRegister(95, 1);

                    (new Nofitication("Kết nối tới thiết bị đo thất bại!")).ShowDialog();

                    return;
                }

                // Timer nhận giá trị từ HIOKI
                bgwork_getMeasurementValueHioki = new BackgroundWorker();
                bgwork_getMeasurementValueHioki.DoWork += new DoWorkEventHandler(bgwork_getMeasurementValueHioki_DoWork);
                getMeasurementValueHiokiTimer.Interval = 50;
                getMeasurementValueHiokiTimer.Elapsed += new System.Timers.ElapsedEventHandler(RetrieveHiokiMeasurement);
                getMeasurementValueHiokiTimer.Start();

                // Timer ghi giá tri lên modbust
                bgwork_writeToModbust = new BackgroundWorker();
                bgwork_writeToModbust.DoWork += new DoWorkEventHandler(bgwork_writeToModbust_DoWork);
                writeToModbustTimer.Interval = 50;
                writeToModbustTimer.Elapsed += new System.Timers.ElapsedEventHandler(_writeToModbust_Elapsed);

                // Timer ghi giá tri lên modbust
                //bgwork_writeTypePin = new BackgroundWorker();
                //bgwork_writeTypePin.DoWork += new DoWorkEventHandler(bgwork_writeTypePin_DoWork);
                //writeTypePinTimer.Interval = 10;
                //writeTypePinTimer.Elapsed += new System.Timers.ElapsedEventHandler(_writeTypePin_Elapsed);
                //writeTypePinTimer.Start();


                // Thiết lập kết nối máy quét
                //TEST
                setscanner();
                //TEST
                //if (maxGroupPin == 4)
                //{
                //    m_reader.ExecCommand("WP,250,4");
                //}
                //else
                //{
                //    m_reader.ExecCommand("WP,250,8");
                //}

                uiTimer.Interval = 50;
                uiTimer.Tick += uiTimer_Tick;
                uiTimer.Start();

                monitorModbusTimer.Interval = 50;
                monitorModbusTimer.Tick += MonitorModbusTimer_Tick;
                monitorModbusTimer.Start();

                timerShowText.Interval = 50;
                timerShowText.Tick += timerShowText_Tick;
                timerShowText.Start();

                timerShowphanloaiV.Interval = 1000;
                timerShowphanloaiV.Tick += timerShowphanloaiV_Tick;
                timerShowphanloaiV.Start();

                ChartConfig(cartesianChart1);
                ChartConfig(cartesianChart2);

                chartTimer.Interval = 50;
                chartTimer.Tick += ChartTimer_Tick;
                chartTimer.Start();

                pinGroupUITimer.Interval = 50;
                pinGroupUITimer.Tick += pinGroupUI_Tick;
                pinGroupUITimer.Start();

                bgwork_light = new BackgroundWorker();
                bgwork_light.DoWork += new DoWorkEventHandler(bgwork_light_DoWork);

                // Timer Export Excel
                bgwork_exportExcelTimer = new BackgroundWorker();
                bgwork_exportExcelTimer.DoWork += new DoWorkEventHandler(bgwork_exportExcelTimer_DoWork);
                exportExcelTimer.Interval = 50;
                exportExcelTimer.Elapsed += new System.Timers.ElapsedEventHandler(_exportExcelTimer_Elapsed);


                ////Timer đèn
                _LightTimer.Interval = 1000;
                _LightTimer.Elapsed += new System.Timers.ElapsedEventHandler(_LightTimer_Elapsed);

                // Ping tới keygence để kiểm tra kết nối
                bgwork_check_keygence_connect = new BackgroundWorker();
                bgwork_check_keygence_connect.DoWork += new DoWorkEventHandler(checkConnectDevices);

                // Timer kiểm tra mắt quét
                checkKeygenceConnectTimer.Interval = 3000;
                checkKeygenceConnectTimer.Elapsed += new System.Timers.ElapsedEventHandler(_check_keygence_connect_DoWork_Elapsed);
                checkKeygenceConnectTimer.Start();

                resetChartTimer.Interval = 50;
                resetChartTimer.Tick += ResetChartTimer_Tick;
                resetChartTimer.Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                //if (fnoti == null)
                //{               
                //    fnoti = new Nofitication("Kết nối thất bại!");
                //    fnoti.Owner = this;              
                //    fnoti.Show();
                //}
            }
        }

        private void timerShowphanloaiV_Tick(object sender, EventArgs e)
        {
            tongtheophanloai();
        }

        private Nofitication fnoti = null;
        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Kiểm tra nếu form chính (MainForm) được minimize
            if (this.WindowState == FormWindowState.Minimized && nofiticationForm != null)
            {
                if (nofiticationForm.InvokeRequired)
                {
                    nofiticationForm.Invoke(new MethodInvoker(delegate
                    {
                        nofiticationForm.WindowState = FormWindowState.Minimized;

                    }));
                }

            }

            if (this.WindowState == FormWindowState.Maximized && nofiticationForm != null)
            {
                if (nofiticationForm.InvokeRequired)
                {
                    nofiticationForm.Invoke(new MethodInvoker(delegate
                    {
                        nofiticationForm.WindowState = FormWindowState.Normal;

                    }));
                }

            }
        }

        public void RemoveColumn(TableLayoutPanel tableLayoutPanel, int columnToRemove)
        {
            if (columnToRemove < tableLayoutPanel.ColumnCount)
            {
                for (int row = 0; row < tableLayoutPanel.RowCount; row++)
                {
                    // Bước 1: Xóa các Control trong cột
                    var control = tableLayoutPanel.GetControlFromPosition(columnToRemove, row);
                    tableLayoutPanel.Controls.Remove(control);
                }

                // Bước 2: Di chuyển các Control ở các cột bên phải sang cột trái
                for (int col = columnToRemove + 1; col < tableLayoutPanel.ColumnCount; col++)
                {
                    for (int row = 0; row < tableLayoutPanel.RowCount; row++)
                    {
                        var control = tableLayoutPanel.GetControlFromPosition(col, row);
                        if (control != null)
                        {
                            tableLayoutPanel.SetColumn(control, col - 1);
                        }
                    }
                }

                // Bước 3: Giảm ColumnCount
                tableLayoutPanel.ColumnStyles.RemoveAt(tableLayoutPanel.ColumnCount - 1);
                tableLayoutPanel.ColumnCount--;
            }
        }

        void _check_keygence_connect_DoWork_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!bgwork_check_keygence_connect.IsBusy)
            {
                bgwork_check_keygence_connect.RunWorkerAsync();
            }
        }



        private void ResetChartTimer_Tick(object sender, EventArgs e)
        {

            try
            {
                if (endShift1 == 1 && !isEndShift1)
                {
                    // reset chart 1
                    ResetChart(cartesianChart1);
                    isEndShift1 = true;
                    totalBatteryShift1 = 1;
                    isSetStandardRLine = false;
                }

                if (endShift2 == 1 && !isEndShift2)
                {
                    // reset chart 2
                    ResetChart(cartesianChart2);
                    isEndShift2 = true;
                    totalBatteryShift2 = 1;
                    isSetStandardVLine = false;

                }

                if (endShift1 == 0)
                {
                    isEndShift1 = false;
                }

                if (endShift2 == 0)
                {
                    isEndShift2 = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                resetChartTimer.Stop();
                Thread.Sleep(5000);
                resetChartTimer.Start();
            }
        }

        private void ResetChart(LiveCharts.WinForms.CartesianChart chart)
        {
            try
            {
                // clear các điểm trên trục Y trừ min max
                chart.Series.Clear();
                chart.AxisY.Clear();
                chart.AxisX.Clear();
                Thread.Sleep(5000);

                if (chart == cartesianChart1)
                {
                    endShift1 = 0;
                }
                else
                {
                    endShift2 = 0;
                }

                batteryList.Clear();

                ChartConfig(chart);
            }

            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                resetChartTimer.Stop();
                Thread.Sleep(5000);
                resetChartTimer.Start();
            }
        }

        bool IsLabelPresent(GroupBox groupBox)
        {
            var labels = groupBox.Controls.OfType<Label>();
            return labels.Any(); // Trả về true nếu có bất kỳ Label nào tồn tại
        }

        private void uiTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị lên UI các bình -> isShowedUIList false
                // Đồ thị hiển thị đủ 8 điểm (Hàng đợi đo bình rỗng) isShowedChart -> true

                // Đảm bảo rằng QR đã quét
                // Và đã đo bình đầu tiên

                if (!isShowedUIList && isShowedChart && queueUI.Count > 0 && queueBattery.Count == 0 && isScannedQrcode && hiokiMeasureMentSuceed == 1)
                {

                    List<string> uiList = new List<string>();

                    bool isError = false;
                    // Lấy ra 8 bình trong hàng đợi
                    for (int i = 0; i < maxGroupPin; i++)
                    {
                        if (queueUI.Count > 0)
                        {
                            BatteryMonitor.Data.battery battery = queueUI.Dequeue();

                            uiList.Add(battery.BatteryCode);


                            if (battery.BatteryCode == "ERROR")
                            {
                                isError = true;

                                // Phát hiện một bình bị lỗi phải nhã các bình trong lô nó ra
                                // Do nó sẽ bị lấy ra khỏi truyển
                                if (i > 0)
                                {
                                    for (int errorIndex = 0; errorIndex < i; errorIndex++)
                                    {
                                        if (queueBattery.Count > 0)
                                        {
                                            queueBattery.Dequeue();
                                        }
                                    }
                                }
                            }
                            if (!isError)
                            {
                                queueBattery.Enqueue(battery);
                            }
                        }
                    }

                    // Nếu như trong 8 code có 1 code bị lỗi thì không lưu nó vào hàng chờ
                    // Được đo bình

                    // Render lên UI
                    renderCodeList(uiList);
                    resetPinGroupsQR();
                    isShowedUIList = true;
                    isShowedChart = false;
                    isScannedQrcode = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void timerShowText_Tick(object sender, EventArgs e)
        {
            try
            {
                // batch
                UpdateLabelText(batchNumber, batchString.ToString());

                // specs
                UpdateLabelText(specs, specsString.ToString());

                // user
                UpdateLabelText(userLabel, userString.ToString());

                UpdateLabelText(rMaxLabel, rMax.ToString("0.000"));
                UpdateLabelText(rMinLabel, rMin.ToString("0.000"));
                UpdateLabelText(vMinLabel, vMin.ToString("0.000"));
                UpdateLabelText(vMaxLabel, vMax.ToString("0.000"));

                UpdateLabelText(currentRLabel, currentR.ToString("N3"));
                UpdateLabelText(currentVLabel, currentV.ToString("N3"));

                // Số ca
                UpdateLabelText(currentWorkShift, currentShiftString);

                UpdateLabelText(totalBatteryShift1Label, totalBatteryShift1.ToString());

                UpdateLabelText(totalBatteryShift2Label, totalBatteryShift2.ToString());

                // Tổng bình R đạt ca 1

                UpdateLabelText(rGoodShift1Label, totalGoodRBatteryShift1.ToString());

                // Tổng bình R thấp ca 1
                UpdateLabelText(rLowShift1Label, totalLowRBatteryShift1.ToString());

                // Tổng bình R cao ca 1
                UpdateLabelText(rHightShift1Label, totalHightRBatteryShift1.ToString());

                // Tổng bình R đật ca 2
                UpdateLabelText(rGoodShift2Label, totalGoodRBatteryShift2.ToString());

                // Tổng bình R thấp ca 2  
                UpdateLabelText(rLowShift2Label, totalLowRBatteryShift2.ToString());

                // Tổng bình R cao ca 2
                UpdateLabelText(rHightShift2Label, totalHightRBatteryShift2.ToString());

                //// Tổng bình V Đạt ca 1
                //UpdateLabelText(V_ACa1, totalGoodVBatteryShift1.ToString());

                //// Tổng bình V thấp ca 1
                //UpdateLabelText(V_BCa1, totalLowVBatteryShift1.ToString());

                //// Tổng bình V cao ca 1
                //UpdateLabelText(V_CCa1, totalHightVBatteryShift1.ToString());

                //// Tổng bình V đạt ca 2
                //UpdateLabelText(V_CCa2, totalGoodVBatteryShift2.ToString());

                //// Tổng bình V thấp ca 2
                //UpdateLabelText(V_BCa2, totalLowVBatteryShift2.ToString());

                //// Tổng bình V cao ca 2 
                //UpdateLabelText(V_CCa2, totalHightVBatteryShift2.ToString());               
            }
            catch (Exception ex)
            {

            }
        }



        private void MonitorModbusTimer_Tick(object sender, EventArgs e)
        {

            try
            {

                if (modbustMainForm.Connected)
                {
                    string asciiString = "";

                    //// Kiểm tra kết nối tới modbus
                    if (!modbustMainForm.Connected)
                    {
                        monitorModbusTimer.Stop();
                        //chartTimer.Stop();
                        return;
                    }

                    // Timer dùng để ghi nhận thông tin của modbus

                    int[] numArray = modbustMainForm.ReadHoldingRegisters(0, 122);
                    int[] numArray2  = modbustMainForm.ReadHoldingRegisters(139, 1);
                    if (numArray[120] < 5 && numArray[120] > 0)
                    {
                        pinCountSetting = numArray[120];
                        UpdateLabelText(numPin, pinCountSetting.ToString());
                    }

                    // Mã số lô hàng
                    asciiString = "";

                    for (int i = 0; i <= 9; i++)
                    {
                        if (numArray[i] != 0)
                        {
                            asciiString += READHODINGREGISTER_to_ASCII(numArray[i]);
                        }
                    }

                    specsString = asciiString;

                    // Quy cách

                    asciiString = "";

                    for (int i = 11; i <= 19; i++)
                    {
                        if (numArray[i] != 0)
                        {
                            asciiString += READHODINGREGISTER_to_ASCII(numArray[i]);
                        }
                    }

                     batchString = asciiString;

                    // Người thao tác
                    asciiString = "";
                    for (int i = 22; i <= 31; i++)
                    {
                        if (numArray[i] != 0)
                        {
                            asciiString += READHODINGREGISTER_to_ASCII(numArray[i]);
                        }
                    }
                    userString = asciiString;

                    // Số ca
                    currentShiftString = numArray[33].ToString();
                    this.currentWorkShift.Text = currentShiftString;

                    // Nếu cần hoàn thành thì thêm dữ liệu vào đồ thị và tổng số lần cân trước nhỏ hơn số lần ca hiện tại
                    // Lấy tổng bình

                    totalBatteryShift1 = numArray[47];
                    totalBatteryShift2 = numArray[55];

                    MenuForm.endShift1 = numArray[71];
                    MenuForm.endShift2 = numArray[73];

                    // số ca hiện tại
                    currentShift = numArray[33];
                    // Tổng bình R đạt ca 1
                    totalGoodRBatteryShift1 = numArray[49];
                    // Tổng bình R thấp ca 1
                    totalLowRBatteryShift1 = numArray[51];
                    // Tổng bình R cao ca 1
                    totalHightRBatteryShift1 = numArray[53];
                    // Tổng bình R đật ca 2
                    totalGoodRBatteryShift2 = numArray[57];
                    // Tổng bình R thấp ca 2  
                    totalLowRBatteryShift2 = numArray[59];
                    // Tổng bình R cao ca 2
                    totalHightRBatteryShift2 = numArray[61];


                    //// Tổng bình V Đạt ca 1
                    //totalGoodVBatteryShift1 = numArray[77];
                    //// Tổng bình V thấp ca 1
                    //totalLowVBatteryShift1 = numArray[79];
                    //// Tổng bình V cao ca 1
                    //totalHightVBatteryShift1 = numArray[81];
                    //// Tổng bình V đạt ca 2
                    //totalGoodVBatteryShift2 = numArray[83];
                    //// Tổng bình V thấp ca 2
                    //totalLowVBatteryShift2 = numArray[85];
                    //// Tổng bình V cao ca 2 
                    //totalHightVBatteryShift2 = numArray[87];
                    //vA_Ca1 = numArray[139];
                    //vB_Ca1 = numArray[141];
                    //vC_Ca1 = numArray[143];
                    //vD_Ca1 = numArray[145];
                    //vNG_Ca1 = numArray[147];

                    //vA_Ca2 = numArray[153];
                    //vB_Ca2 = numArray[155];
                    //vC_Ca2 = numArray[157];
                    //vD_Ca2 = numArray[159];
                    //vNG_Ca2 = numArray[161];
                    //get rMax
                    rMax = getRMaxFromModbus(modbustMainForm);
                    rMin = getRMinFromModbus(modbustMainForm);

                    // get vMin
                    vMin = getVminFromModbus(modbustMainForm);
                    // get vMax
                    vMax = getVMaxFromModbus(modbustMainForm);

                    //TEST demo mark
                    currentR = Twoint16ConverttoFloat(numArray[99], numArray[100]);
                    currentR = Math.Round(currentR, 3);
                    //// currentV demo mark
                    currentV = Twoint16ConverttoFloat(numArray[103], numArray[104]);
                    currentV = Math.Round(currentV, 3);
                    // label10.Text = currentR.ToString();
                    // Báo cáo
                    isExportDataReport = numArray[93];

                    // Số ca
                    currentShift = numArray[33];

                    // Xóa CPK
                    isDeleteCPK = numArray[69];

                    // Xóa đồ thị
                    isDeleteChart = numArray[67];

                    // Hết ca 1
                    endShift1 = numArray[71];

                    // Hết ca 2
                    endShift2 = numArray[73];

                    // Đo đầu vào hoàn thành
                    hiokiMeasureMentSuceed = numArray[75];

                    // Kiểm tra xem có thể thêm dữ liệu vào đồ thị hay chưa
                    isAddDataToChart = numArray[113];
                    //if (hiokiMeasureMentSuceed == 1 && !isMeasureMentSucceed && isShowedUIList && !isShowedChart && queueBattery.Count > 0)
                    isResetLamp = numArray2[0];
                    //if (isResetLamp ==1)
                    //{ 
                    //   // modbustWrite.WriteSingleRegister(149, 0);
                    //}
                    UpdateLabelText(lb_isResetLamp, isResetLamp.ToString());

                    if (isAddDataToChart == 1 && !isMeasureMentSucceed && isShowedUIList && !isShowedChart && queueBattery.Count > 0)
                    {
                        //modbustMainForm.WriteSingleRegister(106, 0);
                        //modbustMainForm.WriteSingleRegister(108, 0);
                        var watch = new System.Diagnostics.Stopwatch();
                        watch.Start();
                        vMax = Math.Round(vMax,3);
                        Properties.Settings.Default.totalPinShift1 = totalBatteryShift1 + 1;
                        Properties.Settings.Default.totalPinShift2 = totalBatteryShift2 + 1;
                        Properties.Settings.Default.vMax_setting = vMax;
                        Properties.Settings.Default.vMin_setting = vMin;
                        Properties.Settings.Default.rMax_setting = rMax;
                        Properties.Settings.Default.rMin_setting = rMin;

                        Properties.Settings.Default.Save();

                        string date = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        int totalMeasurement = currentShift == 1 ? totalBatteryShift1 : totalBatteryShift2;

                        BatteryMonitor.Data.battery battery = queueBattery.Dequeue();
                        batteryList.Add(battery);
                        isDeQueue = true;


                        // Lấy các thông số từ OPC SERVER
                        // USR124 <= A <= vMax
                        // USR126 <= B <= USR128
                        // USR130 <= C <= USR132
                        // vMin <= D <= USR134

                        //cập nhật thông số đo cho các pin
                        battery.ShipmentId = this.batchNumber.Text;
                        battery.Specs = this.specs.Text;
                        battery.R = currentR;
                        battery.V = currentV;
                        battery.RMAX = rMax;
                        battery.VMAX = vMax;
                        battery.RMIN = rMin;
                        battery.VMIN = vMin;
                        battery.WorkShift = currentShift;
                        battery.TotalMeasureMent = currentShift == 1 ? totalBatteryShift1 : totalBatteryShift2;
                        battery.Date = date;
                        battery.UserId = this.userLabel.Text;
                        battery.MeasureMentStatus = numArray[39] == 0 && numArray[40] == 0 ? "WrongMeasurement" : "Measured";
                        battery.QUALITY = "";
                        battery.TYPE_R =  ClassifyR(battery.R, rMax, rMin);

                        battery.TYPE_V = ClassifyV(battery.V, vMax, vMin);

                        if (battery.TYPE_R == "FAULT")
                        {
                            currentR = 0;
                            currentV = 0;
                        }

                        totalR += currentR;
                        totalV += currentV;

                        // demo mark
                        double AVE_R = calc_AVE(totalR, totalMeasurement);
                        double AVE_V = calc_AVE(totalV, totalMeasurement);
                        ZI_R = ZI_R + Math.Pow((currentR - AVE_R), 2);
                        ZI_V = ZI_V + Math.Pow((currentV - AVE_V), 2);
                        double std_R = calc_STD(totalR, totalMeasurement);
                        double std_V = calc_STD(totalV, totalMeasurement);

                        // demo mark
                        battery.AVE_R = AVE_R;
                        battery.AVE_V = AVE_V;
                        battery.STD_R = std_R;
                        battery.STD_V = std_V;

                        // demo mark
                        battery.CPK_R = calc_CPK(totalR, totalMeasurement, currentR, rMin, rMax, ZI_R, AVE_R, std_R);
                        battery.CPK_V = calc_CPK(totalV, totalMeasurement, currentV, vMin, vMax, ZI_V, AVE_V, std_V);

                        string condition = " batteryCode = '" + battery.BatteryCode + "'";

                        sqlLite.updateBatteryList(battery, condition);

                        //// Nếu kết quả đo lỗi -> báo đèn đỏ => đá bình ra
                        if (numArray[39] == 0 && numArray[40] == 0)
                        {
                            m_reader.ExecCommand("OUTOFF,1");
                            m_reader.ExecCommand("OUTOFF,3");
                            m_reader.ExecCommand("OUTON,2");
                            _LightTimer.Start();
                        }

                        // reset
                        isMeasureMentSucceed = true;
                        //modbustWrite.WriteSingleRegister(76, 0);

                        watch.Stop();
                        //currentR = 0;
                        //currentV = 0;
                       
                    }

                    // nếu việc cân đầu vào hoàn thành
                    //if (hiokiMeasureMentSuceed == 0)

                    if (isAddDataToChart == 0)
                    {
                        //if (modbustWrite.Connected && isMeasureMentSucceed == true)
                        //{
                        //modbustWrite.WriteSingleRegister(39, 0);
                        //modbustWrite.WriteSingleRegister(40, 0);

                        //}

                        isMeasureMentSucceed = false;
                        isDeQueue = false;



                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                modbustMainForm.Disconnect();
                monitorModbusTimer.Stop();
                Thread.Sleep(5000);
                monitorModbusTimer.Start();
            }
        }

        private string createForder(string shipmentId)
        {
            string filepath = Properties.Settings.Default.excelPath;
            string fullPath = Path.GetFullPath(filepath);
            string path = Path.Combine(new string[] { fullPath, "EXCEL", "OhmVolt", "Report", shipmentId });
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            // Lấy thông tin bảo mật từ thư mục đã tạo
            DirectorySecurity security = Directory.GetAccessControl(path);

            // Thêm quyền truy cập full cho tất cả người dùng
            security.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));

            // Áp dụng các cài đặt bảo mật
            Directory.SetAccessControl(path, security);

            return path;
        }

        private double calc_CPK(double totalValue, int totalMeasurement, double currentValue, double min, double max, double zi, double AVE, double std)
        {
            // chỉ số quá trình khả năng sản xuất (CPK)
            // độ lệch chuẩn STD
            // Zi: Tổng bình phương của sự chênh lệch giữa mỗi giá trị đo lường và giá trị trung bình của chúng.
            // Zi = Zi + (x1 - x2)^2
            // Số bình đã đo hiện tại 

            // STD = Math.sqrt(Zi / n - 1)
            // Trong đó n là tổng bình đã đo

            // CPK1 = DST - DSD / 3 * STD
            // CPK2 = AVE - CDTC - DSD / 3 * STD
            // Trong đó:
            // DST: Giá trị trên (Trong TH này là R max)
            // DSD: Giá trị dưới (Trong TH này là R min)
            // AVE: Giá trị trung bình của các giá trị đo lường
            // CDTC: Trung bình của điện trở min và max

            // Tính CPK1
            double cpk1 = (max - min) / (3 * std);
            // Tính CPK2
            // CDTC giá trị trung bình của max và min
            double CDTC = (max + min) / 2;

            double cpk2 = (AVE - CDTC - max) / (3 * std);

            // Chọn CPK1 hoặc CPK2 làm giá trị của CPK
            double cpk = cpk1 < cpk2 ? cpk1 : cpk2;

            // Tinh chỉnh CPK

            // Nếu CPK lớn hơn 1.6, tình chỉnh CPK:
            if (cpk < 1.6)
            {
                cpk = 1.607 - (0.1 / std);
            }

            // Nếu CPK nhỏ hơn 1.3, tình chỉnh CPK:
            if (cpk < 1.3)
            {
                cpk = 1.333 + (0.1 / std);
            }
            return cpk;
        }

        private double calc_STD(double zi, int totalMeasurement)
        {
            try
            {
                if (zi == 0 || totalMeasurement == 0)
                {
                    return -1;
                }

                double std = Math.Sqrt(zi / totalMeasurement - 1);

                if (double.IsNaN(std))
                {
                    return -1;
                }

                return std;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return -1;
        }

        private double calc_AVE(double totalValue, int totalMeasurement)
        {

            try
            {
                if (totalValue == 0 || totalMeasurement == 0)
                {
                    return -1;
                }

                double AVE = totalValue / totalMeasurement;

                if (double.IsNaN(AVE))
                {
                    return -1;
                }

                return AVE;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return -1;
        }

        private void pinGroupUI_Tick(object sender, EventArgs e)
        {

            try
            {
                int groupIndex = 0;
                List<GroupBox> pinGroupBoxes = GetAllGroupBoxesInTableLayoutPanel(tableLayout_measure);

                if (isMeasureMentSucceed && isShowedUIList && queueBattery.Count >= 0 && isDeQueue)
                {
                    //Xử lý hàng đợi
                    //Tính index của bình
                    //Suy ra đc đang ở pin số mấy trong 8 pin

                    //int batteryIndex = Math.Abs(maxGroupPin - (queueBattery.Count));
                    // Chuyển đổi thành nhận số pin cần đo từ modbust
                    int batteryIndex = Math.Abs(pinCountSetting - (queueBattery.Count));

                    groupIndex = batteryIndex == 0 ? 0 : batteryIndex - 1;
                    //Cập nhật UI
                    this.Invoke((MethodInvoker)delegate
                    {
                        var tableLayout = pinGroupBoxes[groupIndex].Controls.OfType<TableLayoutPanel>().FirstOrDefault();

                        //Kiểm tra xem giá trị đo có vượt ngưỡng hay không

                        if (tableLayout != null)
                        {

                            var labels = tableLayout.Controls.OfType<Label>();
                          
                            //switch (pinCurrentColor)
                            //{
                            //    case 1:
                            //        pinGroupBoxes[groupIndex].BackColor = System.Drawing.Color.White;
                            //        break;
                            //    case 2:
                            //        pinGroupBoxes[groupIndex].BackColor = System.Drawing.Color.Blue;
                            //        break;
                            //    case 3:
                            //        pinGroupBoxes[groupIndex].BackColor = System.Drawing.Color.Lime;
                            //        break;
                            //    case 4:
                            //        pinGroupBoxes[groupIndex].BackColor = System.Drawing.Color.Gold;
                            //        break;
                            //    case 5:
                            //        pinGroupBoxes[groupIndex].BackColor = System.Drawing.Color.Red;
                            //        break;
                            //}

                           
                            foreach (var label in labels)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    string message = label.Name.Contains("V") ? "V: " + (Math.Round(currentV, 3).ToString()) : "mΩ: " + (Math.Round(currentR, 3).ToString());
                                    label.Text = message;
                                   
                                });
                            }
                        }
                    });

                }
                //pinGroupUITimer.Stop();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                //monitorModbusTimer.Stop();
                //chartTimer.Stop();
            }
        }

        private List<GroupBox> GetAllGroupBoxesInTableLayoutPanel(TableLayoutPanel tableLayoutPanel)
        {
            List<GroupBox> groupBoxes = new List<GroupBox>();

            foreach (Control control in tableLayoutPanel.Controls)
            {
                if (control is GroupBox)
                {
                    groupBoxes.Add((GroupBox)control);
                }
            }

            groupBoxes.Reverse();
            return groupBoxes;
        }

        private void ChartTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Biến vẽ đồ thì của modbust trả về 1
                // Cờ hiệu vẽ đồ thị thêm 1 điểm vào đồ thị 1 lần isDrawedChartR và isDrawedChartV
                // Đã show hết chưa 8 điểm lên chart isShowedChart
                // Đã show hến lên list đo pin chưa isShowedUIList
                // currentR != 0 && currentV != 0 giá trị đo là một giá trị hợp lệ
                //isAddDataToChart == 1
                //if (isAddDataToChart == 1 && (!isDrawedChartR || !isDrawedChartV) && queueBattery.Count >= 0 && !isShowedChart && isShowedUIList && isDeQueue)

                monitorChartR(cartesianChart1);
                monitorChartV(cartesianChart2);


                if (isAddDataToChart == 1 && (!isDrawedChartR) && queueBattery.Count >= 0 && !isShowedChart && isShowedUIList && isDeQueue)
                {
                    int totalBatteryShift = currentShift == 1 ? totalBatteryShift1 : totalBatteryShift2;
                    RunMonitorTask(totalBatteryShift, currentR, ChartValuesR, cartesianChart1);
                    RunMonitorTask(totalBatteryShift, currentV, ChartValuesV, cartesianChart2);
                    isDrawedChartR = true;
                    //isDrawedChartV = true;

                    if (queueBattery.Count == 0)
                    {
                        // Nếu đã đo hết 8 bình thì hàng đo là 0
                        // Cần phải reset lại => Đã show hết lên chart và chưa show list đo pin của pin tiếp theo
                        isShowedChart = true;
                        isShowedUIList = false;
                    }
                }

                // Cờ hiệu ngăn chặn vẽ quá nhiều điểm do timer chạy quá nhanh
                if (isAddDataToChart == 0)
                {
                    isDrawedChartR = false;
                    //isDrawedChartV = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);

            }
        }


        private void ChartTimer_V_Tick(object sender, EventArgs e)
        {
            try
            {
                // Biến vẽ đồ thì của modbust trả về 1
                // Cờ hiệu vẽ đồ thị thêm 1 điểm vào đồ thị 1 lần isDrawedChartR và isDrawedChartV
                // Đã show hết chưa 8 điểm lên chart isShowedChart
                // Đã show hến lên list đo pin chưa isShowedUIList
                // currentR != 0 && currentV != 0 giá trị đo là một giá trị hợp lệ
                //isAddDataToChart == 1
                if (isAddDataToChart == 1 && (!isDrawedChartV) && queueBattery.Count >= 0 && !isShowedChart && isShowedUIList && isDeQueue)
                {
                    int totalBatteryShift = currentShift == 1 ? totalBatteryShift1 : totalBatteryShift2;
                    //RunMonitorTask(totalBatteryShift, currentR, ChartValuesR, cartesianChart1);
                    RunMonitorTask(totalBatteryShift, currentV, ChartValuesV, cartesianChart2);
                    isDrawedChartV = true;
                    //isAddDataToChart = 0;

                    // Tăng số lượng điểm lên (trục X)
                    if (currentShift == 1)
                    {
                        totalBatteryShift1++;
                    }
                    else
                    {
                        totalBatteryShift2++;
                    }

                    if (queueBattery.Count == 0)
                    {
                        // Nếu đã đo hết 8 bình thì hàng đo là 0
                        // Cần phải reset lại => Đã show hết lên chart và chưa show list đo pin của pin tiếp theo
                        isShowedChart = true;
                        isShowedUIList = false;
                        //isAddDataToChart = 0;
                    }

                    //isDeQueue = false;
                }

                // Cờ hiệu ngăn chặn vẽ quá nhiều điểm do timer chạy quá nhanh
                if (isAddDataToChart == 0)
                {
                    isDrawedChartV = false;
                }

                //chartTimer.Stop();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);

            }
        }

        private void scanQrcodeTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (queueUI.Count == 0 && !isShowedUIList && isShowedChart && isDeQueue)
                {
                    // Nhận dữ liệu từ mắt quét
                    string qrcodes = ScanQRCode();

                    // Lấy thời gian thực
                    string date = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    // validate
                    // Xử lý dữ liệu từ qrcodes -> List string
                    List<string> qrcodeList = getBatteryCodeList(qrcodes);
                    // validate 
                    if (qrcodeList.Count == 0)
                    {
                        // error
                        // ...
                    }
                    // Xử lý khi có mã bị lỗi
                    // Thêm các mã pin đã quét vào battery List
                    int index = 0;
                    bool isError = false;
                    string errorScanQrList = "";
                    qrcodeList.ForEach((code) =>
                    {
                        // Trường hợp đã ở code cuối thì không cần dùng ','
                        string errorCode = code + ",";
                        if (index == 7)
                        {
                            errorCode = code;
                        }

                        errorScanQrList += errorCode;
                        // Check error
                        if (code == "ERROR")
                        {
                            isError = true;
                            // Nếu lõi thì nhã hàng đợi ra 
                            // Chỉ cần phát hiện 1 bình lỗi thì
                            // Xóa các bình đã thêm trong cụm
                            // Loại ra khỏi hàng đợi
                            if (index > 0)
                            {
                                for (int i = 0; i < index; i++)
                                {
                                    if (queueUI.Count > 0)
                                    {
                                        BatteryMonitor.Data.battery battery = queueUI.Dequeue();
                                        string condition = "batteryCode = '" + battery.BatteryCode + "' ";
                                        sqlLite.deleteBatterryList(condition);
                                    }

                                }
                            }
                        }

                        // Ghi nhận mã code nhưng chưa đo bình

                        // Các giá trị khởi tạo mặc định


                        // Khởi tạo đối tượng battery với trạng thái ban đầu là "Pending"
                        // Pending
                        // Wrong
                        // Fullfill

                        // Đưa vào hàng đợi
                        // Trường hợp các pin trong hàng đợi chưa đo hết mà các pin khác đã vào
                        // Phát hiện lỗi thì ngừng các thao tác thêm vào sqllite và hàng đợi
                        if (isError == false)
                        {
                            BatteryMonitor.Data.battery battery = new BatteryMonitor.Data.battery
                             (
                                 R: DefaultMeasurementValue,
                                 V: DefaultMeasurementValue,
                                 RMax: DefaultMeasurementValue,
                                 RMin: DefaultMeasurementValue,
                                 VMax: DefaultMeasurementValue,
                                 Vmin: DefaultMeasurementValue,
                                 totalMeasureMent: DefaultTotalMeasurement,
                                 BatteryCode: code,
                                 UserId: EmptyString,
                                 ShipmentId: EmptyString,
                                 Specs: EmptyString,
                                 WorkShift: DefaultWorkShift,
                                 MeasureMentStatus: DefaultMeasurementStatus,
                                 Date: date,
                                 quality: EmptyString,
                                 type_R: EmptyString,
                                 type_V: EmptyString,
                                 cpk_R: DefaultTotalMeasurement,
                                 cpk_V: DefaultTotalMeasurement,
                                 std_R: DefaultTotalMeasurement,
                                 std_V: DefaultTotalMeasurement,
                                 ave_R: DefaultTotalMeasurement,
                                 ave_V: DefaultTotalMeasurement
                             );

                            sqlLite.insertBatteryList(battery);
                            queueUI.Enqueue(battery);
                        }
                        index++;
                    });

                    if (errorScanQrList.Contains("ERROR"))
                    {
                        // Nếu như trong errorScanQrList có chứa ERROR thì save lại

                        sqlLite.insertErrorScanQrList(errorScanQrList, date);
                    }


                    // Render lên UI
                    renderCodeList(qrcodeList);

                    if (queueUI.Count == 0)
                    {
                        return;
                    }


                    for (int i = 0; i < maxGroupPin; i++)
                    {
                        if (queueUI.Count > 0)
                        {
                            queueBattery.Enqueue(queueUI.Dequeue());
                        }
                    }

                    if (queueBattery.Count == 0)
                    {

                    }

                    isShowedUIList = true;
                    isShowedChart = false;

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
            }
        }

        private void RunMonitorTask(int totalBatteryShift, double currentValue, ChartValues<MeasureModel> chartValues, LiveCharts.WinForms.CartesianChart chart)
        {
            Task.Run(() =>
            {
                try
                {
                    currentValue = Math.Round(currentValue, 3);
                    int totalMeasurement = totalBatteryShift;
                    AddDataInChartAsync(currentValue, chartValues, chart, totalMeasurement);
                }
                catch (Exception ex)
                {
                    // Log the exception or notify the user
                    Console.WriteLine(ex.Message);
                    logger.Error(ex);

                }
            });
        }
        public ChartValues<MeasureModel> setValuesForLine(LiveCharts.WinForms.CartesianChart chart, double r, double v)
        {
            return new ChartValues<MeasureModel>
        {
            new MeasureModel
            {
                times = currentShift == 1 ? totalBatteryShift1 : totalBatteryShift2,
                Value = getLimitDouble(chart, r, v),
            },
            new MeasureModel
            {
                times = int.MaxValue,
                Value = getLimitDouble(chart, r, v),
            }
        };
        }

        // Tạo hàm cấu hình cho chart để dùng chung cho nhiều chart. Truyền chart vào hàm
        // async
        public double getRMaxFromModbus(ModbusClient modbus)
        {
            double num = (double)ModbusClient.ConvertRegistersToInt(modbus.ReadHoldingRegisters(35, 2));

            double rMax = Convert.ToDouble(string.Format("{0:0.00}", num / 1000));


            return rMax;
        }

        public double getRMinFromModbus(ModbusClient modbus)
        {
            try
            {
                int[] register = modbus.ReadHoldingRegisters(37, 2);
                double num = (double)ModbusClient.ConvertRegistersToInt(register);

                double rMin = Math.Round(num / 1000.0, 3);

                return rMin;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return 0.0;
        }

        public double getVMaxFromModbus(ModbusClient modbus)
        {
            double num = (double)ModbusClient.ConvertRegistersToInt(modbus.ReadHoldingRegisters(41, 2));
            double vMax = Convert.ToDouble(string.Format("{0:0.00}", num / 1000));
            return vMax;
        }

        public double getVminFromModbus(ModbusClient modbus)
        {
            double num = (double)ModbusClient.ConvertRegistersToInt(modbus.ReadHoldingRegisters(43, 2));
            double vMin = Convert.ToDouble(string.Format("{0:0.00}", num / 1000));
            return vMin;
        }

        public double getLimitDouble(LiveCharts.WinForms.CartesianChart chart, double rValue, double vValue)
        {
            return chart == cartesianChart1 ? rValue : vValue;
        }

        //public int getLimitInt(LiveCharts.WinForms.CartesianChart chart, int rValue, int vValue)
        //{
        //    return chart == cartesianChart1 ? rValue : vValue;
        //}

        // Mã số lô hàng 1 - 9
        // Quy cách 11 - 19
        // Người thao tác 22 - 31
        // ca hiện tại 33 - 33

        public string getInfo(int[] values, int start, int end)
        {
            string result = "";
            if (end == start)
            {
                result = values[end].ToString();
                return result;
            }

            for (int i = start; i <= end; i++)
            {
                result += READHODINGREGISTER_to_ASCII(values[i]);
            }
            return result.Trim();
        }

        public void ChartConfig(LiveCharts.WinForms.CartesianChart chart)
        {
            try
            {
                // TEST
                //// Kiểm tra kết nối với modbus

                var mapper = Mappers.Xy<MeasureModel>()
                    .X(model => model.times)
                    .Y(model => model.Value);

                Charting.For<MeasureModel>(mapper);

                var chartValues = chart == cartesianChart1
                    ? ChartValuesR = new ChartValues<MeasureModel>()
                    : ChartValuesV = new ChartValues<MeasureModel>();

                chart.BackColor = System.Drawing.Color.White;
                chart.LegendLocation = LegendLocation.Right;
                chart.DisableAnimations = true;
                chart.Hoverable = false;
                chart.DataTooltip = null;

                chart.DefaultLegend.FontFamily = new System.Windows.Media.FontFamily("Times New Roman");

                if (chart == cartesianChart1)
                {
                    rMaxSeries = CreateLimitLineSeries(rMax, vMax, "Giới hạn trên", 5, chart, "Max");
                    rMinSeries = CreateLimitLineSeries(rMin, vMin, "Giới hạn dưới", 5, chart, "Min");

                    chart.Series = new SeriesCollection
                    {
                        CreateLineSeries(chartValues, "times", chart == cartesianChart1 ? "Điện trở (R)" : "Điện áp (V)", 13, 4, true, 18),
                        rMinSeries,
                        rMaxSeries ,
                        CreateStatLineSeries("STD: 0"),
                        CreateStatLineSeries("AVE: 0"),
                        CreateStatLineSeries("CPK: 0")
                    };
                }
                else
                {
                    vMaxSeries = CreateLimitLineSeries(rMax, vMax, "Giới hạn trên", 5, chart, "Max");
                    vMinSeries = CreateLimitLineSeries(rMin, vMin, "Giới hạn dưới", 5, chart, "Min");
                    chart.Series = new SeriesCollection
                    {
                        CreateLineSeries(chartValues, "times", chart == cartesianChart1 ? "Điện trở (R)" : "Điện áp (V)", 13, 4, true, 18),
                        vMinSeries,
                        vMaxSeries,
                        CreateStatLineSeries("STD: 0"),
                        CreateStatLineSeries("AVE: 0"),
                        CreateStatLineSeries("CPK: 0")
                    };
                }

                chart.AxisY.Add(new Axis
                {
                    LabelFormatter = value => (chart == cartesianChart1 ? value.ToString("N2") : value.ToString("N3")),
                    Separator = new Separator
                    {
                        Step = chart == cartesianChart1 ? stepR : stepV,
                    },
                    FontSize = 12
                });

                chart.AxisX.Add(new Axis
                {
                    DisableAnimations = true,
                    LabelFormatter = value => value.ToString(),
                    Separator = new Separator
                    {
                        Step = 1,
                    },
                    FontSize = 12,

                });

                int totalBattery = currentShift == 1 ? totalBatteryShift1 : totalBatteryShift2;

                SetAxisLimits(chart, totalBattery);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
            }
        }

        private LineSeries CreateLineSeries(ChartValues<MeasureModel> values, string dataContext, string title, int pointGeometrySize, int strokeThickness, bool dataLabels, int fontSize)
        {

            return new LineSeries
            {
                Values = values,
                PointGeometrySize = pointGeometrySize,
                PointGeometry = DefaultGeometries.Circle,
                StrokeThickness = strokeThickness,
                DataLabels = dataLabels,
                DataContext = dataContext,
                Title = title,
                FontSize = fontSize,
                Fill = System.Windows.Media.Brushes.Transparent,
                LineSmoothness = 0,
                Stroke = System.Windows.Media.Brushes.Blue,

            };
        }

        private LineSeries CreateLimitLineSeries(double rLimit, double vLimit, string title, int strokeThickness, LiveCharts.WinForms.CartesianChart chart, string status)
        {
            return new LineSeries
            {
                Values = new ChartValues<MeasureModel>

                {
                    new MeasureModel
            {
                times = currentShift == 1 ? totalBatteryShift1 : totalBatteryShift2,
                Value = getLimitDouble(chart, rLimit, vLimit),
            },
            new MeasureModel
            {
                times = int.MaxValue,
                Value = getLimitDouble(chart, rLimit, vLimit),
            }
        },
                PointGeometry = null,
                StrokeThickness = strokeThickness,
                Fill = System.Windows.Media.Brushes.Transparent,
                Title = title,
                FontSize = 20,
                Stroke = status == "Max" ? System.Windows.Media.Brushes.Yellow : System.Windows.Media.Brushes.Red,

            };
        }

        private LineSeries CreateStatLineSeries(string title)
        {
            return new LineSeries
            {
                Title = title,
                Values = new ChartValues<double> { },
                PointGeometry = DefaultGeometries.Square,
                Stroke = System.Windows.Media.Brushes.Blue,
            };
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Tẳt hết các đèn
            if (SetScannerStatus)
            {
                m_reader.ExecCommand("ALLOFF");
                m_reader.Disconnect();
            }

            // Clear các timer
            chartTimer.Stop();
            monitorModbusTimer.Stop();
            resetChartTimer.Stop();
            getMeasurementValueHiokiTimer.Stop();
            uiTimer.Stop();
            checkKeygenceConnectTimer.Stop();
            writeToModbustTimer.Stop();
            exportExcelTimer.Stop();
            timerShowText.Stop();

            if (nofiticationForm.InvokeRequired)
            {
                nofiticationForm.Invoke(new MethodInvoker(delegate
                {
                    nofiticationForm.Close();
                }));
            }

        }


        public void UpdateAverageValue(double newAverageValue, SeriesCollection seriesCollection, LiveCharts.WinForms.CartesianChart chart, string title)
        {
            // If this method is not run on the UI thread, BeginInvoke will be used to run it on the UI thread
            if (chart.InvokeRequired)
            {
                chart.BeginInvoke((Action)(() => UpdateAverageValue(newAverageValue, seriesCollection, chart, title)));
                return;
            }

            // Find the series with the title starting with "AVE"
            var series = seriesCollection.FirstOrDefault(s => s.Title.StartsWith(title));

            // If the series was found, create a new series with the new title and the same values
            if (series != null)
            {
                var newSeries = new LineSeries
                {
                    Title = $"{title}: {newAverageValue}",
                    Values = series.Values,
                    // thêm phầm point hình vuông và cố định màu
                    PointGeometry = DefaultGeometries.Square,
                    Stroke = System.Windows.Media.Brushes.Blue,

                };

                // Replace the old series with the new one in the SeriesCollection
                var index = seriesCollection.IndexOf(series);
                seriesCollection[index] = newSeries;
            }
        }

        public void UpdateLabelText(Label label, string text)
        {
            try
            {
                if (label.InvokeRequired)
                {
                    label.Invoke(new Action(() => label.Text = text));
                }
                else
                {
                    label.Text = text;
                }
                //label.Text = text;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
            }
        }
        public void UpdateLabelColor(Label label, System.Drawing.Color color)
        {
            try
            {
                if (label.InvokeRequired)
                {
                    label.Invoke(new Action(() => label.BackColor = color));
                }
                else
                {
                    label.BackColor = color;
                }
                //label.Text = text;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
            }
        }
        public Task AddDataInChartAsync(double value, ChartValues<MeasureModel> chartValues, LiveCharts.WinForms.CartesianChart chart, int totalMeasurement)
        {
            return Task.Run(() =>
            {
                try
                {

                    // Add data to the chart
                    chartValues.Add(new MeasureModel
                    {
                        times = totalMeasurement,
                        Value = value,
                    });


                    // Set the limits for the X axis
                    SetAxisLimits(chart, totalMeasurement);

                    // If the number of data points exceeds MaxDataPoints, remove the first one
                    if (chartValues.Count > MaxDataPoints) chartValues.RemoveAt(0);

                }
                catch (Exception ex)
                {
                    // Log the exception or notify the user
                    Console.WriteLine(ex.Message);
                    logger.Error(ex);
                }
            });
        }

        public static string READHODINGREGISTER_to_ASCII(int Value)
        {
            string str = Value.ToString("X").Substring(0, 2);
            string str1 = Value.ToString("X").Substring(2);
            string[] strArrays = str1.Split(new char[] { ' ' });
            int num = 0;
            while (num < (int)strArrays.Length)
            {
                if (string.IsNullOrEmpty(strArrays[num]))
                {
                    num++;
                }
                else
                {
                    deccc1 = Convert.ToInt64(str1, 16);
                    break;
                }
            }
            long num1 = Convert.ToInt64(str, 16);
            string str2 = Convert.ToChar(num1).ToString();
            char chr = Convert.ToChar(deccc1);
            return string.Concat(chr.ToString(), str2);
        }

        // Hàm xử lý dữ liệu từ máy quét
        // input: 1 chuỗi chứa 8 mã code của pin
        // output: mảng chứa các code
        private List<String> getBatteryCodeList(string codes)
        {
            try
            {

                if (codes.Length == 0 || string.IsNullOrEmpty(codes))
                {
                    return new List<string>();
                }

                List<String> batteryCodes = codes.Split(',').ToList();

                return batteryCodes;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex);

            }

            return new List<string>();
        }

        // Hàm nhận dữ liệu sau khi đã xử lý thành một code list
        // Nhận vào một List string
        // Duyệt qua code list để render UI phù hợp
        // Nếu Error thì pinGroup tương ứng sẽ đổi màu đỏ và gửi tín hiệu ngừng truyền
        // Đổi màu đèn ...

        private void renderCodeList(List<string> codeList)
        {
            try
            {
                List<GroupBox> groupBoxes = GetAllGroupBoxesInTableLayoutPanel(tableLayout_measure);
                groupBoxes.Reverse();

                if (codeList.Count == 0)
                {
                    // Show error
                    // messsage 
                    // ...
                    return;
                }

                // Duyệt sang codeList
                for (int index = 0; index < codeList.Count; index++)
                {
                    string e = codeList[index];

                    // Tạo một luống mới để xử lý UI
                    this.Invoke((MethodInvoker)delegate
                    {
                        if (index >= 0 && index < codeList.Count)
                        {
                            groupBoxes[index].BackColor = e == "ERROR" ? System.Drawing.Color.Red : System.Drawing.Color.Lime;

                            var tableLayout = groupBoxes[index].Controls.OfType<TableLayoutPanel>().FirstOrDefault();
                            if (tableLayout != null)
                            {
                                var labels = tableLayout.Controls.OfType<Label>();
                                foreach (var label in labels)
                                {
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        label.Text = "";
                                    });
                                }
                            }


                        }
                    });

                };
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void renderCodeQRList(List<string> codeList)
        {
            try
            {
                List<GroupBox> groupBoxes = GetAllGroupBoxesInTableLayoutPanel(tableLayout_QR);
                if (codeList.Count == 0)
                {
                    // Show error
                    // messsage 
                    // ...
                    return;
                }

                // Duyệt sang codeList
                for (int index = 0; index < codeList.Count; index++)
                {
                    if (index == pinCountSetting && pinCountSetting != codeList.Count)
                    {
                        break;
                    }

                    string e = codeList[index];

                    // Tạo một luống mới để xử lý UI
                    this.Invoke((MethodInvoker)delegate
                    {
                        if (index >= 0 && index < codeList.Count)
                        {
                            //var labels = groupBoxes[index].Controls.OfType<Label>();
                            groupBoxes[index].BackColor = e == "ERROR" ? System.Drawing.Color.Red : System.Drawing.Color.Lime;
                        }
                    });

                };
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }



        private void changeToScannedUI()
        {
            List<GroupBox> groupBoxes = new List<GroupBox>
    {
        pinGroup1, pinGroup2, pinGroup3, pinGroup4,
        pinGroup5, pinGroup6, pinGroup7, pinGroup8
    };
            // Duyệt sang codeList
            for (int index = 0; index < maxGroupPin; index++)
            {

                // Tạo một luống mới để xử lý UI
                this.Invoke((MethodInvoker)delegate
                {
                    groupBoxes[index].BackColor = System.Drawing.Color.Lime;
                });

            };
        }




        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            HashSet<char> uniqueChars = new HashSet<char>();
            while (uniqueChars.Count < length)
            {
                uniqueChars.Add(chars[random.Next(chars.Length)]);
            }
            return new string(Array.AsReadOnly(uniqueChars.ToArray()).ToArray());
        }

        // Hàm giả lập quét QRcode 

        public string ScanQRCode()
        {
            Random rand = new Random();
            List<string> scannedCodes = new List<string>();

            // Giả lập việc quét 8 QRCode
            for (int i = 1; i <= 8; i++)
            {
                // Xác định ngẫu nhiên xem liệu có lỗi xảy ra không
                bool hasError = rand.NextDouble() < 0.1; // Giả định có 10% khả năng lỗi xảy ra

                if (hasError)
                {
                    // Sinh lỗi ngẫu nhiên
                    scannedCodes.Add("ERROR");
                }
                else
                {
                    // Nếu không lỗi, sinh mã QRCode dựa theo mẫu

                    // String 
                    string randomString = RandomString(10);

                    scannedCodes.Add($"{randomString}{i.ToString().PadLeft(3, '0')}"); // Thêm số 0 vào trước để đạt đủ 3 chữ số
                }
            }

            String.Join(",", scannedCodes);

            return String.Join(",", scannedCodes);
        }

        // Hàm Clear đi label và màu sắc trên các pinGroup sau khi đã xử lý 8 pin
        private void resetPinGroups()
        {
            List<GroupBox> groupBoxes = new List<GroupBox>
    {
        pinGroup1, pinGroup2, pinGroup3, pinGroup4,
        pinGroup5, pinGroup6, pinGroup7, pinGroup8
    };
            groupBoxes.ForEach((pinGroup) =>
            {
                pinGroup.BackColor = System.Drawing.Color.White;

                var tableLayout = pinGroup.Controls.OfType<TableLayoutPanel>().FirstOrDefault();
                if (tableLayout != null)
                {
                    var labels = tableLayout.Controls.OfType<Label>();
                    foreach (var label in labels)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            label.Text = "";
                        });
                    }
                }

            });
        }

        private void resetPinGroupsQR()
        {
            try
            {

                List<GroupBox> groupBoxes = new List<GroupBox>
    {
        pinGroupQR1, pinGroupQR2, pinGroupQR3, pinGroupQR4,
        pinGroupQR5, pinGroupQR6, pinGroupQR7, pinGroupQR8
    };
                groupBoxes.ForEach((pinGroup) =>
                {
                    pinGroup.BackColor = System.Drawing.Color.White;

                    var tableLayout = pinGroup.Controls.OfType<TableLayoutPanel>().FirstOrDefault();
                    if (tableLayout != null)
                    {
                        var labels = tableLayout.Controls.OfType<Label>();
                        foreach (var label in labels)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                label.Text = "";
                            });
                        }
                    }

                });
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }



        // Hàm kết nối đến mắt quét keygen
        public void setscanner()
        {
            try
            {
                m_reader.IpAddress = Properties.Settings.Default.keyGenceIP;
                m_reader.Connect((data) =>
                {
                    BeginInvoke(new delegateUserControl(ReceivedDataWrite), Encoding.ASCII.GetString(data));
                });
                // Bật đèn vàng
                m_reader.ExecCommand("OUTON,3");
                SetScannerStatus = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                //Nofitication nofitication = new Nofitication("Set Scanner Error..!Bấm OK để tiếp tục... ");
                //nofitication.Show();
            }
        }

        // Hàm xử lý output (Nhận output)
        private delegate void delegateUserControl(string str);

        private void ReceivedDataWrite(string receivedData)
        {
            try
            {

                if (receivedData.Length > 0)
                {

                    // Nhận dữ liệu từ mắt quét
                    receivedData = receivedData.Replace("\r", "");
                    if (receivedData.Contains("OUTON") || receivedData.Contains("OUTOFF") || receivedData.Contains("ALLOFF"))
                    {
                        return;
                    }

                    // Ghi nhận trong khuôn có bao nhiêu bình
                    // đặt pinMaxGroup lại

                    // Xử lý qrcodestr hợp lệ
                    // Xử lý dữ liệu từ qrcodes -> List string
                    List<string> qrcodeList = getBatteryCodeList(receivedData);

                    int numberOfSucces = 0;
                    int numberOfError = 0;
                    int index = 0;
                    bool isOverScanned = true;


                    // Mỗi lần quét đều phải lấy lại số pin trên PLC Fx3u để đảm bảo tính chính xác của dữ liệu
                    //pinCountSetting = getMaxPinScanned();


                    for (; index < qrcodeList.Count; index++)
                    {
                        if ((numberOfSucces + numberOfError) == pinCountSetting)
                        {
                            isOverScanned = false;
                        }


                        if ((isOverScanned == false && (numberOfSucces + numberOfError) > pinCountSetting))
                        {
                            if (!qrcodeList[index - 1].Equals("ERROR"))
                            {
                                m_reader.ExecCommand("OUTOFF,1");
                                m_reader.ExecCommand("OUTOFF,3");
                                m_reader.ExecCommand("OUTON,2");
                                _LightTimer.Start();
                                logger.Info(receivedData + "Số lượng pin quét được không đúng với số lượng cài đặt!");
                                MessageBox.Show("Số lượng pin quét được không đúng với số lượng cài đặt!");
                                return;
                            }
                        }

                        if (!qrcodeList[index].Equals("ERROR"))
                        {
                            numberOfSucces++;
                        }
                        else
                        {
                            numberOfError++;
                        }
                    }

                    if (pinCountSetting >= 3)
                    {

                        if ((isOverScanned == false && (numberOfSucces + numberOfError) > pinCountSetting))
                        {
                            if (!qrcodeList[index - 1].Equals("ERROR"))
                            {
                                m_reader.ExecCommand("OUTOFF,1");
                                m_reader.ExecCommand("OUTOFF,3");
                                m_reader.ExecCommand("OUTON,2");
                                _LightTimer.Start();
                                logger.Info(receivedData + "Số lượng pin quét được không đúng với số lượng cài đặt!");
                                MessageBox.Show("Số lượng pin quét được không đúng với số lượng cài đặt!");
                                return;
                            }
                        }

                    }



                    // 1 pin -> 1 ERROR OR 1 QRCODE
                    // 2 pin -> 2 ERROR OR 2 QRCODE
                    // 3 pin -> 3 ERROR OR 3 QRCODE




                    // trường hợp quét thấy đủ 8 code nhưng có lỗi
                    // Trường hợp số pin vượt quá số lượng thiêt lập
                    // + Quét đủ 4 pin

                    // + Quét có pin bị lỗi

                    // Xử lý hàng đợi
                    // Lấy thời gian thực
                    string date = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    // Thêm các mã pin đã quét vào battery List
                    index = 0;
                    bool isError = false;
                    string errorScanQrList = "";
                    int startErrorIndex = 0;
                    bool isDeQueuErrorCode = false;
                    for (; index < qrcodeList.Count; index++)
                    {

                        if (index == pinCountSetting && pinCountSetting != qrcodeList.Count)
                        {
                            break;
                        }


                        string code = qrcodeList[index];
                        // Trường hợp đã ở code cuối thì không cần dùng ','
                        string errorCode = code + ",";
                        if (index == qrcodeList.Count - 1)
                        {
                            errorCode = code;
                        }

                        errorScanQrList += errorCode;
                        // Check error
                        if (code == "ERROR")
                        {
                            isError = true;
                            // Nếu lõi thì nhã hàng đợi ra 
                            // Chỉ cần phát hiện 1 bình lỗi thì
                            // Xóa các bình đã thêm trong cụm
                            // Loại ra khỏi hàng đợi
                            // Nếu không phải bình đầu tiên bị lỗi
                            if (index > 0 && !isDeQueuErrorCode)
                            {
                                int i = startErrorIndex == 0 ? 0 : startErrorIndex;

                                for (; i < index; i++)
                                {
                                    if (queueUI.Count > 0)
                                    {
                                        BatteryMonitor.Data.battery batteryError = queueUI.Dequeue();
                                        string condition = "batteryCode = '" + batteryError.BatteryCode + "' ";
                                        sqlLite.deleteBatterryList(condition);
                                        isDeQueuErrorCode = true;
                                    }

                                }
                                startErrorIndex = i;
                            }
                        }

                        // Ghi nhận mã code nhưng chưa đo bình

                        // Các giá trị khởi tạo mặc định
                        const double DefaultMeasurementValue = 0.0;
                        const int DefaultTotalMeasurement = 0;
                        const int DefaultWorkShift = 0;
                        string DefaultMeasurementStatus = "Pending";
                        const string EmptyString = "";

                        // Khởi tạo đối tượng battery với trạng thái ban đầu là "Pending"
                        // Pending
                        // Wrong
                        // Fullfill

                        // Đưa vào hàng đợi
                        // Trường hợp các pin trong hàng đợi chưa đo hết mà các pin khác đã vào
                        // Phát hiện lỗi thì ngừng các thao tác thêm vào sqllite và hàng đợi

                        BatteryMonitor.Data.battery battery = new BatteryMonitor.Data.battery
                        (
                            R: DefaultMeasurementValue,
                            V: DefaultMeasurementValue,
                            RMax: DefaultMeasurementValue,
                            RMin: DefaultMeasurementValue,
                            VMax: DefaultMeasurementValue,
                            Vmin: DefaultMeasurementValue,
                            totalMeasureMent: DefaultTotalMeasurement,
                            BatteryCode: code,
                            UserId: EmptyString,
                            ShipmentId: EmptyString,
                            Specs: EmptyString,
                            WorkShift: DefaultWorkShift,
                            MeasureMentStatus: DefaultMeasurementStatus,
                            Date: date,
                            quality: EmptyString,
                            type_R: EmptyString,
                            type_V: EmptyString,
                            cpk_R: DefaultMeasurementValue,
                            cpk_V: DefaultMeasurementValue,
                            std_R: DefaultMeasurementValue,
                            std_V: DefaultMeasurementValue,
                            ave_R: DefaultMeasurementValue,
                            ave_V: DefaultMeasurementValue
                        );
                        // Nếu mã code ERROR thì không thêm vào CSDL
                        if (isError == false)
                        {
                            // Hàng đợi xử lý UI
                            sqlLite.insertBatteryList(battery);
                            queueUI.Enqueue(battery);
                        }

                    }

                    renderCodeQRList(qrcodeList);

                    if (errorScanQrList.Contains("ERROR"))
                    {
                        // Hiển thị danh sách bình lỗi
                        scanError = true;
                        sqlLite.insertErrorScanQrList(errorScanQrList, date);
                        m_reader.ExecCommand("OUTOFF,1");
                        m_reader.ExecCommand("OUTOFF,3");
                        m_reader.ExecCommand("OUTON,2");
                        _LightTimer.Start();
                        Thread.Sleep(1000);
                        return;
                    }

                    isScannedQrcode = true;

                    m_reader.ExecCommand("OUTOFF,2");
                    m_reader.ExecCommand("OUTOFF,3");
                    m_reader.ExecCommand("OUTON,1");
                    _LightTimer.Start();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
                m_reader.ExecCommand("OUTOFF,1");
                m_reader.ExecCommand("OUTOFF,3");
                m_reader.ExecCommand("OUTON,2");
                _LightTimer.Start();
            }






            // Xử lý dữ liệu nhận về
            // Thứ tự từ phải sang trái 8 7 6 5 4 3 2 1
            // Thể hiện thành  1 2 3 4 5 6 7 8 
            //receivedData = new string(receivedData.Reverse().ToArray());
            // Nhận dữ liệu từ mắt quét
            //string qrcodes = ScanQRCode();
            // validate

            // Xử lý dữ liệu từ qrcodes -> List string
            //List<string> qrcodeList = getBatteryCodeList(receivedData);
            //stringScanList.Add(qrcodeList);


            // validate 
            //if (qrcodeList.Count == 0)
            //{
            // error
            // ...
            //}
            // Xử lý khi có mã bị lỗi

            // Thêm các mã pin đã quét vào battery List
            //int index = 0;
            //bool isError = false;

            //qrcodeList.ForEach((code) =>
            //{
            // Check error
            //if (code == "ERROR")
            //{
            //isError = true;
            // Nếu lõi thì nhã hàng đợi ra 
            //    if (index > 0)
            //    {
            //        for (int i = 0; i < index; i++)
            //        {
            //            queueBattery.Dequeue();
            //        }
            //    }

            //}

            // Ghi nhận mã code nhưng chưa đo bình

            // Các giá trị khởi tạo mặc định
            //const double DefaultMeasurementValue = 0.0;
            //const int DefaultTotalMeasurement = 0;
            //const int DefaultWorkShift = 0;
            //string DefaultMeasurementStatus = "Pending";
            //const string EmptyString = "";

            // Khởi tạo đối tượng battery với trạng thái ban đầu là "Pending"
            // Pending
            // Wrong
            // Fullfill

            // Đưa vào hàng đợi
            // Trường hợp các pin trong hàng đợi chưa đo hết mà các pin khác đã vào


            // Nếu hàng đợi rỗng thì show ui lô bình tiếp theo

            //while (true)
            //{
            //    if (queueBattery.Count == 0)
            //    {
            //        break;
            //    }f
            //}

            // Render lên UI
            //renderCodeList(qrcodeList);

            // Xử lý ERROR từ mắt quét
            // Bật đèn đỏ
            //if (receivedData.Contains("ERROR"))
            //{
            //m_reader.ExecCommand("OUTOFF,1");
            //m_reader.ExecCommand("OUTOFF,3");
            //m_reader.ExecCommand("OUTON,2");

            // Trả cụm bình ra khỏi truyền

            // Timer trả về đèn vàng
            //_LightTimer.Start();

            // Clear UI

            // Xóa Cụm bình lỗi ra khỏi DB


            //    return;
            //}

            // Bật đèn xanh
            //m_reader.ExecCommand("OUTOFF,2");
            //m_reader.ExecCommand("OUTOFF,3");
            //m_reader.ExecCommand("OUTON,1");

            //    }
            //}
            //catch (Exception ex)
            //{

            //    logger.Error(ex);
            //}
        }

        // Hàm xử lý tín hiệu đèn
        // m_reader.ExecCommand("ALLOFF"); Tắt hết đèn
        // m_reader.ExecCommand("OUTON,1"); Đèn xanh
        // m_reader.ExecCommand("OUTON,2"); Đèn đỏ
        // m_reader.ExecCommand("OUTON,3"); Đèn vàng

        void _TimersTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

        }

        void _LightTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (!bgwork_light.IsBusy)
                {
                    bgwork_light.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"{ex.Message}");
                Console.WriteLine(ex.Message);
            }
        }
        private void RetrieveHiokiMeasurement(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (!bgwork_getMeasurementValueHioki.IsBusy)
                {
                    // Start the asynchronous operation to retrieve the measurement value.
                    bgwork_getMeasurementValueHioki.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                // Use string interpolation for better readability and to avoid unnecessary string concatenations.
                logger.Error($"An error occurred while attempting to retrieve Hioki measurement: {ex.Message}");
            }
        }


        void _exportExcelTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (!bgwork_exportExcelTimer.IsBusy)
                {
                    bgwork_exportExcelTimer.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }


        void _writeToModbust_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (!bgwork_writeToModbust.IsBusy)
                {
                    bgwork_writeToModbust.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }


        void _writeTypePin_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (!bgwork_writeTypePin.IsBusy)
                {
                    bgwork_writeTypePin.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }




        private void bgwork_light_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                m_reader.ExecCommand("OUTOFF,1");
                m_reader.ExecCommand("OUTOFF,2");
                m_reader.ExecCommand("OUTON,3");
                _LightTimer.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex.Message);
            }

        }



        private void ShowChartNotification(string message)
        {
            Panel notificationPanel = new Panel();
            notificationPanel.BackColor = System.Drawing.Color.Transparent;
            notificationPanel.Size = new System.Drawing.Size(tableLayout_measure.ClientSize.Width, 68);
            notificationPanel.Location = new System.Drawing.Point(0, 86);

            Label notificationLabel = new Label();
            notificationLabel.Text = message;
            notificationLabel.BackColor = System.Drawing.Color.Yellow;
            notificationLabel.Font = new System.Drawing.Font("Arial", 40, FontStyle.Bold);
            notificationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            notificationLabel.Dock = DockStyle.Fill;
            notificationPanel.Controls.Add(notificationLabel);
            tableLayout_measure.Controls.Add(notificationPanel);
            System.Drawing.Color myColor = System.Drawing.Color.FromArgb(229, 115, 115);
            tableLayout_measure.BackColor = myColor;
            //cartesianChart1.ChartAreas["ChartArea1"].BackColor = myColor;

        }





        private void checkConnectDevices(object sender, DoWorkEventArgs e)
        {
            try
            {
                Ping pingSender = new Ping();
                string keyGenceIp = Properties.Settings.Default.keyGenceIP;
                PingReply reply = pingSender.Send(keyGenceIp, 1000);

                if (reply.Status != IPStatus.Success)
                {
                    onLineStatus = false;
                    // thông báo
                    if (fnoti == null)
                    {
                        fnoti = new Nofitication("Mất kết nối với thiết bị quét scanner!!!");
                        fnoti.Owner = this;
                        fnoti.Show();
                    }
                    modbustMainForm.WriteSingleRegister(97, 1);
                    modbustMainForm.WriteSingleRegister(95, 1);
                    checkKeygenceConnectTimer.Stop();
                    m_reader.Disconnect();
                    Thread.Sleep(10000);
                    setscanner();
                    checkKeygenceConnectTimer.Start();
                    return;
                }

                onLineStatus = true;

                if (modbustMainForm != null && !modbustMainForm.Connected)
                {
                    isConnectedToModbust = false;
                    modbustMainForm.WriteSingleRegister(97, 1);
                    modbustMainForm.WriteSingleRegister(95, 1);
                    // thông báo
                    if (fnoti == null)
                    {
                        fnoti = new Nofitication("Mất kết nối với HMI proface!!!");
                        fnoti.Owner = this;
                        fnoti.Show();
                    }
                    getMeasurementValueHiokiTimer.Stop();
                    writeToModbustTimer.Stop();
                    monitorModbusTimer.Stop();
                    uiTimer.Stop();
                    chartTimer.Stop();
                    modbustMainForm.Disconnect();
                    Thread.Sleep(10000);
                    modbustMainForm.Connect();
                    getMeasurementValueHiokiTimer.Start();
                    writeToModbustTimer.Start();
                    monitorModbusTimer.Start();
                    uiTimer.Start();
                    chartTimer.Start();
                    return;
                }

                isConnectedToModbust = true;


                string hioikiIp = Properties.Settings.Default.hiokiIP;
                reply = pingSender.Send(hiokiIp, 1000);

                //if (hiokiSocket != null && !hiokiSocket.IsTcpSocketConnected())


                if (reply.Status != IPStatus.Success)
                {
                    modbustMainForm.WriteSingleRegister(97, 1);
                    modbustMainForm.WriteSingleRegister(95, 1);
                    // thông báo                
                    if (fnoti == null)
                    {
                        fnoti = new Nofitication("Mất kết nối với thiết bị đo hioki!!!");
                        fnoti.Owner = this;
                        fnoti.Show();
                    }
                    isConnectedToHioki = false;
                    getMeasurementValueHiokiTimer.Stop();
                    writeToModbustTimer.Stop();
                    monitorModbusTimer.Stop();
                    uiTimer.Stop();
                    chartTimer.Stop();
                    hiokiSocket.TryCloseConnectToTcpServer();
                    Thread.Sleep(10000);
                    hiokiSocket.TryConnectToTcpServer();
                    getMeasurementValueHiokiTimer.Start();
                    writeToModbustTimer.Start();
                    monitorModbusTimer.Start();
                    uiTimer.Start();
                    chartTimer.Start();
                    return;
                } 
                isConnectedToHioki = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                if (fnoti == null)
                {    
                    fnoti = new Nofitication("Có lỗi xảy ra!!");
                    fnoti.Owner = this;
                    fnoti.Show();
                }
                Console.WriteLine(ex.Message);
            }
        }

        private void bgwork_check_modbust_connect_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (modbustMainForm.Connected)
                {
                    modbustMainForm.WriteSingleRegister(97, 0);
                    modbustMainForm.WriteSingleRegister(95, 0);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                //Nofitication nofitication = new Nofitication(ex.Message);
                //nofitication.Show();
                Console.WriteLine(ex.Message);
            }
        }
        private void bgwork_check_hioki_connect_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (modbustMainForm.Connected)
                {
                    modbustMainForm.WriteSingleRegister(97, 0);
                    modbustMainForm.WriteSingleRegister(95, 0);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                //Nofitication nofitication = new Nofitication(ex.Message);
                //nofitication.Show();
                Console.WriteLine(ex.Message);
            }
        }

        private void pinGroupQR2_Enter(object sender, EventArgs e)
        {

        }

        private void pinGroupQR3_Enter(object sender, EventArgs e)
        {
            //ReceivedDataWrite("22YCB65320000J8BV8704192,22YCB65320000J8BV8704207,22YCB65320000J8BV8704208,22YCB65320000J8BV8704209");

        }

        private void specs_Click(object sender, EventArgs e)
        {
            //ReceivedDataWrite("22YCB65320000J8BV8704192,22YCB65320000J8BV8704207,22YCB65320000J8BV8704208,22YCB65320000J8BV8704209");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //sqlLite.deleteBatterryList(" 1=1");
        }

        private void userLabel_Click(object sender, EventArgs e)
        {

        }

        private void batchNumber_Click(object sender, EventArgs e)
        {

        }

        private void Main_MinimumSizeChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void currentVLabel_Click(object sender, EventArgs e)
        {

        }

        private void vMinLabel_Click(object sender, EventArgs e)
        {

        }

        private void pinGroupQR1_Enter(object sender, EventArgs e)
        {
            ReceivedDataWrite("02YCB65320000J8BV8704192,02YCB65320000J8BV8704193,02YCB65320000J8BV8704194,02YCB65320000J8BV8704195,02YCB65320000J8BV8704196,02YCB65320000J8BV8704197,02YCB65320000J8BV8704198,02YCB65320000J8BV870419");

        }

        private string ClassifyR(double registerDisplay, double resistanceMax, double resistanceMin)
        {
            try
            {
                //if (registerValues.Length == 0)
                //{
                //    // Báo lỗi
                //    return "ERROR";
                //}

                //double num = (double)ModbusClient.ConvertRegistersToInt(modbustMainForm.ReadHoldingRegisters(35, 2));
                //double resistanceMax = Convert.ToDouble(string.Format("{0:0.00}", num / 100));
                //double num1 = (double)ModbusClient.ConvertRegistersToInt(modbustMainForm.ReadHoldingRegisters(37, 2));
                //double resistanceMin = Convert.ToDouble(string.Format("{0:0.00}", num1 / 100));

                //Convert the register values to float for comparison

                //float registerDisplay = Twoint16ConverttoFloat(registerValues[99], registerValues[100]);

                // Classify register display value
                if (registerDisplay <= 0f)
                {
                    modbustMainForm.WriteSingleRegister(106, 1);
                    UpdateLabelText(rCompare, "FAULT");
                    UpdateLabelColor(rCompare, System.Drawing.Color.Red);
                    return "FAULT";
                }

                if (registerDisplay >= resistanceMin && registerDisplay <= resistanceMax)
                {
                    modbustMainForm.WriteSingleRegister(106, 2);
                    UpdateLabelText(rCompare, "OK");
                    UpdateLabelColor(rCompare, System.Drawing.Color.Lime);
                    return "OK";
                }

                if (registerDisplay < resistanceMin)
                {
                    modbustMainForm.WriteSingleRegister(106, 3);
                    UpdateLabelText(rCompare, "LOW");
                    UpdateLabelColor(rCompare, System.Drawing.Color.Red);
                    return "LOW";
                }

                if (registerDisplay > resistanceMax)
                {
                    modbustMainForm.WriteSingleRegister(106, 4);
                    UpdateLabelText(rCompare, "HIGH");
                    UpdateLabelColor(rCompare, System.Drawing.Color.Red);
                    return "HIGH";
                }



                // Tính phân loại A B C D
                // ...
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex);
            }
            return "ERROR";

        }

        private string ClassifyV(double voltageDisplay, double voltageMax, double voltageMin)
        {
            try
            {

                //if (registerValues.Length == 0)
                //{
                //    // Báo lỗi
                //    return "ERROR";
                //}

                //double num2 = (double)ModbusClient.ConvertRegistersToInt(modbustMainForm.ReadHoldingRegisters(41, 2));
                //double voltageMax = Convert.ToDouble(string.Format("{0:0.00}", num2 / 100));
                //double num3 = (double)ModbusClient.ConvertRegistersToInt(modbustMainForm.ReadHoldingRegisters(43, 2));
                //double voltageMin = Convert.ToDouble(string.Format("{0:0.00}", num3 / 100));

                //float voltageDisplay = Twoint16ConverttoFloat(registerValues[103], registerValues[104]);


                if (voltageDisplay <= 0f)
                {
                    //  modbustMainForm.WriteSingleRegister(108, 1);
                    modbustMainForm.WriteSingleRegister(149, 5);
                    pinCurrentColor = 5;
                    UpdateLabelText(vCompare, "NG");
                    UpdateLabelColor(vCompare, System.Drawing.Color.Red);
                    return "NG";
                }

                if (voltageDisplay > voltageMin && voltageDisplay <= voltageMax)
                {
                    // Tính ra vị trí hiện tại của pin trong khuôn
                    //List<int> lightList = new List<int> { 101, 102, 103, 104 };

                    int batteryIndex = Math.Abs(pinCountSetting - (queueBattery.Count));
                    int lightIndex = batteryIndex == 0 ? 0 : batteryIndex - 1;


                    double USR124 = getUSR(modbustMainForm, 122);
                    double USR126 = getUSR(modbustMainForm, 124);
                    double USR128 = getUSR(modbustMainForm, 126);
                    double USR130 = getUSR(modbustMainForm, 128);
                    double USR132 = getUSR(modbustMainForm, 130);
                    double USR134 = getUSR(modbustMainForm, 132);

                    string quality = getQuality(currentV, vMax, vMin, USR124, USR126, USR130, USR132, USR134);
                    // Cụm đèn cần bật
                    //int turnOnLight = lightList[lightIndex];

                    int lightColor = getLightColor(quality);
                    pinCurrentColor = lightColor;
                    // 0 Đỏ
                    // 1 Trắng
                    // 2 Xanh dương
                    // 3 Xanh lá cây
                    // 4 Cam
                    //  modbustMainForm.WriteSingleRegister(150, 2);

                    modbustMainForm.WriteSingleRegister(149, lightColor);
                    switch ( lightColor)
                    {
                        case 1:
                            UpdateLabelColor(vCompare, System.Drawing.Color.White);
                            break;
                        case 2:
                            UpdateLabelColor(vCompare, System.Drawing.Color.Blue);
                            break;
                        case 3:
                            UpdateLabelColor(vCompare, System.Drawing.Color.Lime);
                            break;
                        case 4:
                            UpdateLabelColor(vCompare, System.Drawing.Color.Gold);
                            break;
                    }    
                        
                    UpdateLabelText(vCompare, quality);
                    return quality;
                }

                if (voltageDisplay < voltageMin)
                {
                    modbustMainForm.WriteSingleRegister(149, 5);
                    pinCurrentColor = 5;
                    UpdateLabelText(vCompare, "NG");
                    UpdateLabelColor(vCompare, System.Drawing.Color.Red);
                    return "NG";
                }

                if (voltageDisplay > voltageMax)
                {
                    modbustMainForm.WriteSingleRegister(149, 5);
                    pinCurrentColor = 5;
                    UpdateLabelText(vCompare, "NG");
                    UpdateLabelColor(vCompare, System.Drawing.Color.Red);
                    return "NG";
                }
            }
            catch (Exception ex)
            {
                pinCurrentColor = 5;
                Console.WriteLine(ex.Message);
                logger.Error(ex);
            }
            return "ERROR";

        }
        private string getQuality(double Value, double vMax, double vMin, double USR124, double USR126, double USR130, double USR132, double USR134)
        {
            //// A
            if (Value > vMax || Value < USR134)
            {
                return "";
            }
          //  if (Value >= USR124)
                if (Value > USR124)
                {
                return "A";
            }
            // B
            if (Value > USR126)
            {
                return "B";
            }
            // C
            if (Value > USR130)
            {
                return "C";
            }
            // D
            if (Value > USR132)
            {
                return "D";
            }
            return "";
        }
        int i = 1;      
        private void label13_Click(object sender, EventArgs e)
        {
            //Random rand = new Random();
            //double data = Math.Round(rand.NextDouble() * (3.3 - 1) + 1, 2);
            //i++;
            //cartesianChart2.AxisY[0].MinValue = 1;
            //cartesianChart2.AxisY[0].MaxValue = 3.3;
            //AddDataInChartAsync(data, data, cartesianChart2, i);
        }

        private void currentWorkShift_Click(object sender, EventArgs e)
        {
            //Random R = new Random();

            //double random1 = rMin + (rMax + 3 - (rMin - 3)) * R.NextDouble();
            //double random2 = vMin + (vMax + 1 - (vMin - 3)) * R.NextDouble();

            //currentR = random1;
            //currentV = random2;

            //numArray[99] = numArray[39];
            //numArray[100] = numArray[40];
            //numArray[103] = numArray[45];
            //numArray[104] = numArray[46];
            //// currentR
            //currentR = Twoint16ConverttoFloat(numArray[99], numArray[100]);
            //this.currentRLabel.Text = currentR.ToString("N2");

            //// currentV
            //currentV = Twoint16ConverttoFloat(numArray[103], numArray[104]);
            //this.currentVLabel.Text = currentV.ToString("N2");

            //hiokiMeasureMentSuceed = 1;
            //isAddDataToChart = 1;
        }

        private void totalBatteryShift1Label_Click(object sender, EventArgs e)
        {

            // Tạo forder
            string shipmentId = this.batchNumber.Text;
            string forder = createForder(shipmentId);
            // Xuất báo cáo
            string dateCondition = System.DateTime.Now.ToString("yyyy/mm/dd");
            string condition = " workShift = " + currentShift.ToString() + " AND shipmentId = '" + shipmentId + "' AND date LIKE '%" + dateCondition + "%' ;";
            sqlLite.ExportSqliteDataToExcel(forder, condition);
            // reset flag
            isExportDataFlag = true;

        }

        private void bgwork_writeTypePin_DoWork(object sender, DoWorkEventArgs e)
        {
            //if (hiokiMeasureMentSuceed == 1 && !isMeasureMentSucceed && isShowedUIList && !isShowedChart && queueBattery.Count > 0)
            //{
            //    //modbustMainForm.WriteSingleRegister(106, 0);
            //    //modbustMainForm.WriteSingleRegister(108, 0);


            //    Properties.Settings.Default.totalPinShift1 = totalBatteryShift1 + 1;
            //    Properties.Settings.Default.totalPinShift2 = totalBatteryShift2 + 1;
            //    Properties.Settings.Default.vMax_setting = vMax;
            //    Properties.Settings.Default.vMin_setting = vMin;
            //    Properties.Settings.Default.rMax_setting = rMax;
            //    Properties.Settings.Default.rMin_setting = rMin;

            //    Properties.Settings.Default.Save();

            //    string date = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss:ii");
            //    int totalMeasurement = currentShift == 1 ? totalBatteryShift1 : totalBatteryShift2;

            //    BatteryMonitor.Data.battery battery = queueBattery.Dequeue();
            //    batteryList.Add(battery);
            //    isDeQueue = true;


            //    // Lấy các thông số từ OPC SERVER
            //    // USR124 <= A <= vMax
            //    // USR126 <= B <= USR128
            //    // USR130 <= C <= USR132
            //    // vMin <= D <= USR134

            //    //cập nhật thông số đo cho các pin
            //    battery.ShipmentId = this.batchNumber.Text;
            //    battery.Specs = this.specs.Text;
            //    battery.R = currentR;
            //    battery.V = currentV;
            //    battery.RMAX = rMax;
            //    battery.VMAX = vMax;
            //    battery.RMIN = rMin;
            //    battery.VMIN = vMin;
            //    battery.WorkShift = currentShift;
            //    battery.TotalMeasureMent = currentShift == 1 ? totalBatteryShift1 : totalBatteryShift2;
            //    battery.Date = date;
            //    battery.UserId = this.userLabel.Text;
            //    battery.MeasureMentStatus = numArray[39] == 0 && numArray[40] == 0 ? "WrongMeasurement" : "Measured";
            //    battery.QUALITY = "";



            //    battery.TYPE_R = ClassifyR(currentR, rMax, rMin);


            //    battery.TYPE_V = ClassifyV(currentV, vMax, vMin);

            //    if (battery.TYPE_R == "FAULT")
            //    {
            //        currentR = 0;
            //        currentV = 0;
            //    }

            //    totalR += currentR;
            //    totalV += currentV;

            //    // demo mark
            //    double AVE_R = calc_AVE(totalR, totalMeasurement);
            //    double AVE_V = calc_AVE(totalV, totalMeasurement);
            //    ZI_R = ZI_R + Math.Pow((currentR - AVE_R), 2);
            //    ZI_V = ZI_V + Math.Pow((currentV - AVE_V), 2);
            //    double std_R = calc_STD(totalR, totalMeasurement);
            //    double std_V = calc_STD(totalV, totalMeasurement);

            //    // demo mark
            //    battery.AVE_R = AVE_R;
            //    battery.AVE_V = AVE_V;
            //    battery.STD_R = std_R;
            //    battery.STD_V = std_V;

            //    // demo mark
            //    battery.CPK_R = calc_CPK(totalR, totalMeasurement, currentR, rMin, rMax, ZI_R, AVE_R, std_R);
            //    battery.CPK_V = calc_CPK(totalV, totalMeasurement, currentV, vMin, vMax, ZI_V, AVE_V, std_V);

            //    string condition = " batteryCode = '" + battery.BatteryCode + "'";

            //    sqlLite.updateBatteryList(battery, condition);

            //    //// Nếu kết quả đo lỗi -> báo đèn đỏ => đá bình ra
            //    //if (numArray[39] == 0 && numArray[40] == 0)
            //    //{
            //    //    m_reader.ExecCommand("OUTOFF,1");
            //    //    m_reader.ExecCommand("OUTOFF,3");
            //    //    m_reader.ExecCommand("OUTON,2");
            //    //    _LightTimer.Start();
            //    //}

            //    // reset
            //    isMeasureMentSucceed = true;

            //    //currentR = 0;
            //    //currentV = 0;
            //}

            //// nếu việc cân đầu vào hoàn thành
            //if (hiokiMeasureMentSuceed == 0)
            //{
            //    isMeasureMentSucceed = false;
            //    isDeQueue = false;

            //}

        }



        private void bgwork_writeToModbust_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                if (!modbustWrite.Connected)
                {
                    return;
                }

                // Xử lý giá trị nhận về từ hioki
                if (convertToInt16List.Count == 0)
                {
                    return;
                }

                if (!float.TryParse(convertToInt16List[0], out float single))
                {
                    // Xử lý lỗi ở đây nếu cần thiết
                    single = 0; // Set giá trị mặc định nếu parse thất bại
                }

                float r = float.Parse(string.Format("{0:0.00}", single));
                if (!float.TryParse(convertToInt16List[1], out float single2))
                {
                    // Xử lý lỗi ở đây nếu cần thiết
                    single2 = 0; // Set giá trị mặc định nếu parse thất bại
                }

                float v = float.Parse(string.Format("{0:0.00}", single2));
                // Kiểm tra xem có giá trị R OK hay không

                if ((double)r < 10000000000 || (double)r > 10000000000000)
                {
                    ushort[] registers = FloatToModbusRegisters(r);
                    int[] num = new int[2];
                    num[0] = Convert.ToInt32(registers[0]);
                    num[1] = Convert.ToInt32(registers[1]);
                    //TEST

                    if (num[0] == 0 && num[1] == 0)
                    {
                        string test = "dsad";

                    }

                    modbustWrite.WriteSingleRegister(39, num[0]);
                    modbustWrite.WriteSingleRegister(40, num[1]);

                    //numArray[39] = num[0];
                    //numArray[40] = num[1];

                }
                else
                {
                    // Nếu là giá trị R không hợp lệ thì write 0
                    //TEST
                    if (isAddDataToChart == 0)
                    {
                        modbustWrite.WriteSingleRegister(39, 0);
                        modbustWrite.WriteSingleRegister(40, 0);
                    }
                }

                // Xử lý giá trị V nhận về từ hioki
                // Các bước xử lý số khi chuyển lên modbust
                // Do modbust proface sử dụng 2 thanh ghi 16 bit để biểu diễn số thực

                int[] num1 = new int[2];
                ushort[] voltRegisters = FloatToModbusRegisters(v);
                num1[0] = Convert.ToInt32(voltRegisters[0]);
                num1[1] = Convert.ToInt32(voltRegisters[1]);
                //TEST
                modbustWrite.WriteSingleRegister(45, num1[0]);
                modbustWrite.WriteSingleRegister(46, num1[1]);
                if (isResetLamp != 0)
                {
                    modbustWrite.WriteSingleRegister(149, 0); //Reset Den ve
                }
                //numArray[45] = num1[0];
                //numArray[46] = num1[1];
                // Ngừng timer
                writeToModbustTimer.Stop();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                writeToModbustTimer.Stop();

                Thread.Sleep(10000);
                writeToModbustTimer.Start();
            }
        }

        private async void bgwork_getMeasurementValueHioki_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<string> results = new List<string>();

                // Kiểm tra kết nối tới HIOKI 192.168.1.1:23
                if (!isConnectedToHioki)
                {
                    // Xử lý lỗi
                    // ...
                    return;
                }

                // SendMsg
                string hiokiCommand = ":FETCH?";
                long timerOut = Convert.ToInt64("1") * (long)1000;
                //TEST
                bool isSendMsgSucceed = hiokiSocket.SendQueryMsg(hiokiCommand, timerOut);
                //bool isSendMsgSucceed = true;
                // ReceiveMsg
                if (isSendMsgSucceed)
                {
                    results = await hiokiSocket.ReceiveMsg(timerOut);
                }
                // Start Timer Write to modbust
                // Gửi msg thành công và nhận msg thành công
                if (isConnectedToHioki && results.Count > 1)
                {
                    convertToInt16List = results;
                    writeToModbustTimer.Start();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                getMeasurementValueHiokiTimer.Stop();
                Thread.Sleep(5000);
                getMeasurementValueHiokiTimer.Start();
            }
        }

        // Kiểm tra chất lượng bình ắc quy 
        // A
        // B
        // C
        // D
        // maxV
        // minV

        private void bgwork_exportExcelTimer_DoWork(object sender, DoWorkEventArgs e)
        {
            if (modbustMainForm.Connected)
            {
                int[] array = modbustMainForm.ReadHoldingRegisters(0, 124);
                isExportDataReport = array[93];

                if (isExportDataReport == 1 && !isExportDataFlag)
                {
                    string shipmentId = this.batchNumber.Text;
                    string forder = createForder(shipmentId);
                    // Xuất báo cáo
                    string dateCondition = System.DateTime.Now.ToString("yyyy/mm/dd");
                    string condition = " workShift = " + currentShift.ToString() + " AND shipmentId = '" + shipmentId + "' AND date LIKE '%" + dateCondition + "%' ;";
                    sqlLite.ExportSqliteDataToExcel(forder, condition);
                    // reset flag
                    isExportDataFlag = true;
                }

                if (isExportDataReport == 0)
                {
                    isExportDataFlag = false;
                }

            }
        }

       


        // Tạm thời chưa áp dụng
        // Đợi anh Thuẩn code PLC đèn
        //private void MyGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        //{

        //    for (int i = 0; i < ItemValues.Length; i++)
        //    {
        //        var value = (double)ItemValues.GetValue(i);
        //        MessageBox.Show(value.ToString());
        //        opcValues.Add(value);
        //    }

        //}

        public double getUSR(ModbusClient modbus, int startAddress)
        {
            double num = (double)ModbusClient.ConvertRegistersToInt(modbus.ReadHoldingRegisters(startAddress, 2).Reverse().ToArray());

            double usrValue = Convert.ToDouble(string.Format("{0:0.00}", num / 1000));


            return usrValue;
        }

        // Xử lý tìn hiệu lên đèn

        public int getLightColor(String quality)
        {
            List<String> qualities = new List<string> { "", "A", "B", "C", "D" };
            return qualities.IndexOf(quality);

        }

    }
}
