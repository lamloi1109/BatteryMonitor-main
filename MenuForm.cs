using BatteryMonitor.HIOKI;
using EasyModbus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimersTimer = System.Timers.Timer;
namespace BatteryMonitor
{
    public partial class MenuForm : Form
    {
        static public ModbusClient modbustcp = new ModbusClient();
        //static string hiokiIp = Properties.Settings.Default.hiokiIP;
        //static string hiokiPort = Properties.Settings.Default.hiokiPort;
        //static public TcpClient LanSocket;

        // Tạo đối tương tcp socket
        //static public hioki hiokiSocket = new hioki(hiokiIp, hiokiPort, LanSocket);

        // Timer kiểm tra kết nối mắt quét
        TimersTimer checkKeygenceConnectTimer = new TimersTimer();

        // Timer kiểm tra kết nối tới modbust
        TimersTimer checkModbustConnectTimer = new TimersTimer();

        // Timer kiểm tra kết nối tới HIOKI
        TimersTimer checkHiokiConnectTimer = new TimersTimer();

        // Timer nhận giá trị từ HIOKI
        TimersTimer getMeasurementValueHiokiTimer = new TimersTimer();

        // Timer ghi giá trị lên modbust
        TimersTimer writeToModbustTimer = new TimersTimer();

        // Timer quản lý trạng thái
        TimersTimer statusManagermentTimer = new TimersTimer();


        // BackgroundWorker
        BackgroundWorker bgwork_check_keygence_connect, bgwork_check_modbust_connect, bgwork_check_hioki_connect, bgwork_getMeasurementValueHioki, bgwork_writeToModbust;
        BackgroundWorker bgwork_status_managerment;

        //List<string> convertToInt16List = new List<string>();

        // status 
        Boolean onLineStatus, SetScannerStatus, chkprtconnted;

        // Kiểm tra xem đã nhận được biến bằng 1 hãy chưa
        static public bool isMeasureMentSucceed = false;
        // Kiểm tra xem đã hết ca 1 hay chưa
        static public bool isEndShift1 = false;
        // Kiểm tra xem đã hết ca 2 hay chưa
        static public bool isEndShift2 = false;
        // Đã hết ca 1 chưa
        static public int endShift1 = 0;
        // Đã hết ca 2 chưa
        static public int endShift2 = 0;
        // Đã đo bình xong chưa
        static public int hiokiMeasureMentSuceed = 0;
        // Ca hiện tại
        static public int currentShift = 0;
        // Xóa Chart hay không
        static public int isDeleteChart = 0;
        // Xóa CPK hay không
        static public int isDeleteCPK = 0;
        // Xuất báo cáo hay chưa
        static public int isExportDataReport = 0;

        static public bool isConnectedToHioki = false;

        static public bool isConnectedToModbust = false;

        static public bool isConnectedToKeygence = false;


        List<string> convertToInt16List = new List<string>();

        // NLog
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public MenuForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Chuyển sang form test
            this.Hide();
            test test = new test();
            test.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // chuyển sang form main
                Main main = new Main();
                //main.FormClosed += new FormClosedEventHandler(MenuFormClosed); // Đăng ký sự kiện FormClosed
                //this.Hide();
                main.Show();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            //if (!isConnectedToHioki)
            //{
            //    MessageBox.Show("Mất kết nối với HIOKI!");
            //    return;
            //}
            //if (!isConnectedToModbust)
            //{
            //    MessageBox.Show("Mất kết nối với MODBUST!");
            //    return;
            //}

            // Kiểm tra xem đã nhận time số max min hay chưa
            //if ((Functions.Display != 1 || Functions.dientromax == 0 ? true : Functions.dienapmax == 0))
            //{
            //    MessageBox.Show("Chưa nhập giá trị tham số. Kiểm tra lại giá trị điện trở max, điện áp max!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //}


        }

        private void button5_Click(object sender, EventArgs e)
        {
            ConfigIP_PORT cfg = new ConfigIP_PORT();
            cfg.FormClosed += new FormClosedEventHandler(MenuFormClosed); // Đăng ký sự kiện FormClosed
            this.Hide();
            cfg.Show();
        }

        private void MenuFormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            try
            {
                // Phiên bản
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string productName = Application.ProductName;
                this.Text = productName + " Menu " + version.ToString();
                this.userLabel.Text = Properties.Settings.Default.userId;
                this.userLabel.ForeColor = Color.White;
                //modbustcp.IPAddress = Properties.Settings.Default.modbustIP;
                //modbustcp.Port = Properties.Settings.Default.modbustPort;
                //modbustcp.Connect();

                //Kiểm tra kết nối tới modbust
                //if (!modbustcp.Connected)
                //{
                //    //Nofitication nofiForm = new Nofitication("Kết nối tới modubust thất bại!");
                //    //nofiForm.Show();
                //    return;
                //}

                // Kết nối đến HIOKI
                //bool isConnectHioki = hiokiSocket.TryConnectToTcpServer();

                //// Kết nối đến HIOKI
                //if (!isConnectHioki)
                //{
                //    //Nofitication nofiForm = new Nofitication("Kết nối tới Hioki thất bại!");
                //    //nofiForm.Show();
                //    return;
                //}

                //// Timer nhận giá trị từ HIOKI
                //bgwork_getMeasurementValueHioki = new BackgroundWorker();
                //bgwork_getMeasurementValueHioki.DoWork += new DoWorkEventHandler(bgwork_getMeasurementValueHioki_DoWork);
                //getMeasurementValueHiokiTimer.Interval = 50;
                //getMeasurementValueHiokiTimer.Elapsed += new System.Timers.ElapsedEventHandler(_getMeasurementValueHioki_Elapsed);
                //getMeasurementValueHiokiTimer.Start();

                //// Timer ghi giá tri lên modbust
                //bgwork_writeToModbust = new BackgroundWorker();
                //bgwork_writeToModbust.DoWork += new DoWorkEventHandler(bgwork_writeToModbust_DoWork);
                //writeToModbustTimer.Interval = 50;
                //writeToModbustTimer.Elapsed += new System.Timers.ElapsedEventHandler(_writeToModbust_Elapsed);

                //// Timer kiểm tra mắt quét
                // Ping tới keygence để kiểm tra kết nối
                //bgwork_check_keygence_connect = new BackgroundWorker();
                //bgwork_check_keygence_connect.DoWork += new DoWorkEventHandler(bgwork_check_keygence_connect_DoWork);
                //checkKeygenceConnectTimer.Interval = 50;
                //checkKeygenceConnectTimer.Elapsed += new System.Timers.ElapsedEventHandler(_check_keygence_connect_DoWork_Elapsed);
                //checkKeygenceConnectTimer.Start();

                //// Timer kiểm tra kết nối tới modbust
                //bgwork_check_modbust_connect = new BackgroundWorker();
                //bgwork_check_modbust_connect.DoWork += new DoWorkEventHandler(bgwork_check_modbust_connect_DoWork);
                //checkKeygenceConnectTimer.Interval = 50;
                //checkKeygenceConnectTimer.Elapsed += new System.Timers.ElapsedEventHandler(_check_modbust_connect_DoWork_Elapsed);
                //checkKeygenceConnectTimer.Start();
                //// Timer kiểm tra kết nối đến HIOKI
                //bgwork_check_hioki_connect = new BackgroundWorker();
                //bgwork_check_hioki_connect.DoWork += new DoWorkEventHandler(bgwork_check_hioki_connect_DoWork);
                //checkHiokiConnectTimer.Interval = 50;
                //checkHiokiConnectTimer.Elapsed += new System.Timers.ElapsedEventHandler(_check_hioki_connect_DoWork_Elapsed);
                //checkHiokiConnectTimer.Start();
                //// Timer quản lý trạng thái cho các chức năng trong ứng dụng
                //bgwork_status_managerment = new BackgroundWorker();
                //bgwork_status_managerment.DoWork += new DoWorkEventHandler(bgwork_status_managerment_DoWork);
                //statusManagermentTimer.Interval = 100;
                //statusManagermentTimer.Elapsed += new System.Timers.ElapsedEventHandler(_status_managerment_DoWork_Elapsed);
                //statusManagermentTimer.Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show(ex.Message);
                // reConnect
                // Application.Restart();
            }

        }

        void _getMeasurementValueHioki_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!bgwork_getMeasurementValueHioki.IsBusy)
            {
                bgwork_getMeasurementValueHioki.RunWorkerAsync();
            }
        }

        void _writeToModbust_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!bgwork_writeToModbust.IsBusy)
            {
                bgwork_writeToModbust.RunWorkerAsync();
            }
        }

        void _check_modbust_connect_DoWork_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!bgwork_check_modbust_connect.IsBusy)
            {
                bgwork_check_modbust_connect.RunWorkerAsync();
            }
        }

        void _status_managerment_DoWork_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!bgwork_check_hioki_connect.IsBusy)
            {
                bgwork_status_managerment.RunWorkerAsync();
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {

        }

      

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //ExcelConfig excelConfig = new ExcelConfig();
            //excelConfig.FormClosed += new FormClosedEventHandler(MenuFormClosed); // Đăng ký sự kiện FormClosed
            //this.Hide();
            //excelConfig.Show();

            DataListQuery datalistqry = new DataListQuery();
            datalistqry.FormClosed += Datalistqry_FormClosed;
            this.Hide();
            datalistqry.Show();

        }

        private void Datalistqry_FormClosed(object sender, FormClosedEventArgs e)
        {
           this.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.userId = "";
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            // chuyển sang form main
            test main = new test();
            //main.FormClosed += new FormClosedEventHandler(MenuFormClosed); // Đăng ký sự kiện FormClosed
            //this.Hide();
            main.Show();
        }

        void _check_hioki_connect_DoWork_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!bgwork_check_hioki_connect.IsBusy)
            {
                bgwork_check_hioki_connect.RunWorkerAsync();
            }
        }

        void _check_keygence_connect_DoWork_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!bgwork_check_keygence_connect.IsBusy)
            {
                bgwork_check_keygence_connect.RunWorkerAsync();
            }
        }


        // Nhận giá trị từ HIOKI
        //private async void bgwork_getMeasurementValueHioki_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        List<string> results = new List<string>();

        //        // Kiểm tra kết nối tới HIOKI 192.168.1.1:23
        //        if (!isConnectedToHioki)
        //        {
        //            // Xử lý lỗi
        //            // ...
        //        }

        //        // SendMsg
        //        string hiokiCommand = ":FETCH?";
        //        long timerOut = Convert.ToInt64("1") * (long)1000;
        //        bool isSendMsgSucceed = hiokiSocket.SendQueryMsg(hiokiCommand, timerOut);
        //        // ReceiveMsg
        //        if (isSendMsgSucceed)
        //        {
        //            results = await hiokiSocket.ReceiveMsg(timerOut);
        //        }
        //        // Start Timer Write to modbust
        //        // Gửi msg thành công và nhận msg thành công
        //        if (!isConnectedToHioki && results.Count > 1)
        //        {
        //            convertToInt16List = results;
        //            writeToModbustTimer.Start();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //        getMeasurementValueHiokiTimer.Stop();
        //        Thread.Sleep(5000);
        //        getMeasurementValueHiokiTimer.Start();

        //    }
        //}

        private void button4_Click(object sender, EventArgs e)
        {
            ExcelConfig excfg = new ExcelConfig();
            excfg.FormClosed += new FormClosedEventHandler(MenuFormClosed); // Đăng ký sự kiện FormClosed
            this.Hide();
            excfg.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void bgwork_writeToModbust_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

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

                float r = float.Parse(string.Format("{0:0.00}", single * 1000f));


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
                    modbustcp.WriteSingleRegister(39, num[0]);
                    modbustcp.WriteSingleRegister(40, num[1]);

                }
                else
                {
                    // Nếu là giá trị R không hợp lệ thì write 0

                    modbustcp.WriteSingleRegister(39, 0);
                    modbustcp.WriteSingleRegister(40, 0);
                }

                // Xử lý giá trị V nhận về từ hioki
                // Các bước xử lý số khi chuyển lên modbust
                // Do modbust proface sử dụng 2 thanh ghi 16 bit để biểu diễn số thực

                int[] num1 = new int[2];
                ushort[] voltRegisters = FloatToModbusRegisters(v);
                num1[0] = Convert.ToInt32(voltRegisters[0]);
                num1[1] = Convert.ToInt32(voltRegisters[1]);
                modbustcp.WriteSingleRegister(45, num1[0]);
                modbustcp.WriteSingleRegister(46, num1[1]);

                // Ngừng timer
                writeToModbustTimer.Stop();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Thread.Sleep(10000);
                writeToModbustTimer.Stop();
            }
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

        //private void bgwork_check_keygence_connect_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        Ping pingSender = new Ping();
        //        string keyGenceIp = Properties.Settings.Default.keyGenceIP;
        //        PingReply reply = pingSender.Send(keyGenceIp, 1000);

        //        if (reply.Status != IPStatus.Success)
        //        {
        //            onLineStatus = false;
        //            //Nofitication nofitication = new Nofitication("Mất kết nối với mắt quét keygence!!!");
        //            //nofitication.Show();
        //            modbustcp.WriteSingleRegister(97, 0);
        //            modbustcp.WriteSingleRegister(95, 0);
        //        }
        //        else
        //        {
        //            onLineStatus = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //        //Nofitication nofitication = new Nofitication(ex.Message);
        //        //nofitication.Show();
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //private void bgwork_check_modbust_connect_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if (!modbustcp.Connected)
        //        {
        //            isConnectedToModbust = false;
        //            modbustcp.WriteSingleRegister(97, 0);
        //            modbustcp.WriteSingleRegister(95, 0);
        //            return;
        //        }
        //        isConnectedToModbust = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //        //Nofitication nofitication = new Nofitication(ex.Message);
        //        //nofitication.Show();
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //private void bgwork_check_hioki_connect_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if (!hiokiSocket.IsTcpSocketConnected())
        //        {
        //            isConnectedToHioki = false;
        //            modbustcp.WriteSingleRegister(97, 0);
        //            modbustcp.WriteSingleRegister(95, 0);
        //            return;
        //        }
        //        isConnectedToHioki = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //        //Nofitication nofitication = new Nofitication("Mất kết mới với HIOKI!");
        //        //nofitication.Show();
        //        Console.WriteLine(ex.Message);

        //    }
        //}

        //private void bgwork_status_managerment_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        // Kiểm tra kết nối đến hioki
        //        // Nếu như thất bại thì
        //        // Ngừng timer
        //        // Ngắt kết nối
        //        // Thử kết nối lại

        //        if (!isConnectedToHioki)
        //        {
        //            statusManagermentTimer.Stop();
        //            hiokiSocket.TryCloseConnectToTcpServer();
        //            Thread.Sleep(10);
        //            hiokiSocket.TryConnectToTcpServer();
        //        }
        //        else
        //        {
        //            // Nhận các biến liên quan đến trạng thái từ modbust
        //            int[] numArray = modbustcp.ReadHoldingRegisters(0, 122);

        //            // Báo cáo
        //            isExportDataReport = numArray[93];

        //            // Số ca
        //            currentShift = numArray[33];

        //            // Xóa CPK
        //            isDeleteCPK = numArray[69];

        //            // Xóa đồ thị
        //            isDeleteChart = numArray[67];

        //            // Hết ca 1
        //            endShift1 = numArray[71];

        //            // Hết ca 2
        //            endShift2 = numArray[73];

        //            // Cân đầu vào hoàn thành
        //            hiokiMeasureMentSuceed = numArray[75];
        //            modbustcp.WriteSingleRegister(97, 1);
        //            if (isConnectedToHioki)
        //            {
        //                modbustcp.WriteSingleRegister(95, 1);
        //            }
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //        Console.WriteLine(ex.Message);
        //    }
        //}

    }
}
