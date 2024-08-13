using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyModbus;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Remoting.Messaging;


namespace BatteryMonitor
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
        }
        ModbusClient modbustcp = new ModbusClient();
        TcpClient LanSocket;
        public event EventHandler<string> MessageReceived;
        string receiveData = null;
        int tongBinhCa1 = 0;
        System.Windows.Forms.Timer testTimer = new System.Windows.Forms.Timer();
        public static long deccc1;
        // tạo một List()
        public List<int> list;
        int lastTotal = 0;
        float r = 0;
        float v = 0;
        string rStr = "";
        string vStr = "";


        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                IPAddress pAddress = IPAddress.Parse(textBox2.Text);


                this.LanSocket = new TcpClient()
                {
                    NoDelay = true
                };
                this.LanSocket.Connect(pAddress, Convert.ToInt32("23"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }



        }

        private void test_Load(object sender, EventArgs e)
        {
            testTimer.Tick += new System.EventHandler(this.testTimer_Tick);
            //modbustcp.ReceiveDataChanged += new EasyModbus.ModbusClient.ReceiveDataChangedHandler(UpdateReceiveData);
            //modbustcp.ConnectedChanged += new EasyModbus.ModbusClient.ConnectedChangedHandler(UpdateConnectedChanged);

        }

        private void testTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // kiểm tra xem đã kết nối chưa
                if (!modbustcp.Connected)
                {
                    testTimer.Stop();
                    MessageBox.Show("Timer OFF");
                    return;
                }

                int[] numArray = modbustcp.ReadHoldingRegisters(0, 122);
                // convert number to string
                //convert number to float

                float regsisterdisplay = this.Twoint16ConverttoFloat(numArray[99], numArray[100]);

                float voltdisplay = this.Twoint16ConverttoFloat(numArray[103], numArray[104]);
                double num = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(35, 2));
                double dientromax = Convert.ToDouble(string.Format("{0:0.00}", num / 100));

                double num1 = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(37, 2));
                double dientromin = Convert.ToDouble(string.Format("{0:0.00}", num1 / 100));
                double num2 = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(41, 2));
                double dienapmax = Convert.ToDouble(string.Format("{0:0.00}", num2 / 100));
                double num3 = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(43, 2));
                double dienapmin = Convert.ToDouble(string.Format("{0:0.00}", num3 / 100));


                float SOCA = numArray[33];
                float OK_R_ca1 = numArray[49];
                float Thap_R_ca1 = numArray[51];
                float Cao_R_ca1 = numArray[53];
                float OK_R_ca2 = numArray[57];
                float Thap_R_ca2 = numArray[59];
                float Cao_R_ca2 = numArray[61];
                float XoaDothi = numArray[67];
                float XoaCPK = numArray[69];
                float Hetca1 = numArray[71];
                float Hetca2 = numArray[73];
                int Can_dau_vao_hoan_thanh = numArray[75];
                float ve = numArray[113];
                float OK_V_ca1 = numArray[77];
                float Thap_V_ca1 = numArray[79];
                float Cao_V_ca1 = numArray[81];
                float OK_V_ca2 = numArray[83];
                float Thap_V_ca2 = numArray[85];
                float Cao_V_ca2 = numArray[87];
                float v1 = numArray[39];

                float v2 = numArray[40];

                float v3 = numArray[45];

                float v4 = numArray[46];

                // xóa hết dữ liệu trong dataGridview1

                // Khai báo và gán giá trị cho các biến nhưng không có class Functions
                // tạo cột tên cho gridview

                // Nếu cần hoàn thành thì thêm dữ liệu vào đồ thị và tổng số lần cân trước nhỏ hơn số lần ca hiện tại
                // Lấy tổng bình
                int tong_binh_ca1 = numArray[47];
                int tong_binh_ca2 = numArray[55];
                //int Hetca1 = numArray[71];
                //int Hetca2 = numArray[73];

                // cần lần đầu tiên cân hoàn thành
                this.dataGridView1.ClearSelection();
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                this.dataGridView1.ColumnCount = 3;
                this.dataGridView1.Columns[0].Name = "Tên";
                this.dataGridView1.Columns[1].Name = "Địa chỉ thanh ghi";
                this.dataGridView1.Columns[2].Name = "Giá trị";

                this.dataGridView1.Rows.Add(new object[] { "Điện trở", "(99 , 100)", regsisterdisplay });
                this.dataGridView1.Rows.Add(new object[] { "Điện trở Max", " (35 , 2) ", dientromax });
                this.dataGridView1.Rows.Add(new object[] { "Điện trở Min", " (37 , 2) ", dientromin });
                //this.dataGridView1.Rows.Add(new object[] { "Điện áp", " 103, 104 ", voltdisplay });
                //this.dataGridView1.Rows.Add(new object[] { "Điện áp Max", " (41 , 2) ", dienapmax });
                //this.dataGridView1.Rows.Add(new object[] { "Điện áp Min", " (43 , 2) ", dienapmin });
                //this.dataGridView1.Rows.Add(new object[] { "Số Cả", 33, SOCA });
                //this.dataGridView1.Rows.Add(new object[] { "Tổng Bình Cả 1", 47, tong_binh_ca1 });
                //this.dataGridView1.Rows.Add(new object[] { "OK R Cả 1", 49, OK_R_ca1 });
                //this.dataGridView1.Rows.Add(new object[] { "Thấp R Cả 1", 51, Thap_R_ca1 });
                //this.dataGridView1.Rows.Add(new object[] { "Cao R Cả 1", 53, Cao_R_ca1 });
                //this.dataGridView1.Rows.Add(new object[] { "Tổng Bình Cả 2", 55, tong_binh_ca2 });
                //this.dataGridView1.Rows.Add(new object[] { "OK R Cả 2", 57, OK_R_ca2 });
                //this.dataGridView1.Rows.Add(new object[] { "Thấp R Cả 2", 59, Thap_R_ca2 });
                this.dataGridView1.Rows.Add(new object[] { "Cao R Cả 2", 61, Cao_R_ca2 });
                this.dataGridView1.Rows.Add(new object[] { "Về", 113, ve });
                this.dataGridView1.Rows.Add(new object[] { "OK V Cả 1", 77, OK_V_ca1 });
                //this.dataGridView1.Rows.Add(new object[] { "Thấp V Cả 1", 79, Thap_V_ca1 });
                //this.dataGridView1.Rows.Add(new object[] { "Cao V Cả 1", 81, Cao_V_ca1 });
                //this.dataGridView1.Rows.Add(new object[] { "OK V Cả 2", 83, OK_V_ca2 });
                //this.dataGridView1.Rows.Add(new object[] { "Thấp V Cả 2", 85, Thap_V_ca2 });
                //this.dataGridView1.Rows.Add(new object[] { "Cao V Cả 2", 87, Cao_V_ca2 });
                //this.dataGridView1.Rows.Add(new object[] { "Xóa Đồ Thị", 67, XoaDothi });
                //this.dataGridView1.Rows.Add(new object[] { "Xóa CPK", 69, XoaCPK });
                //this.dataGridView1.Rows.Add(new object[] { "Hết Cả 1", 71, Hetca1 });
                //this.dataGridView1.Rows.Add(new object[] { "Hết Cả 2", 73, Hetca2 });

                this.dataGridView1.Rows.Add(new object[] { "? R1", 39, numArray[39] });

                this.dataGridView1.Rows.Add(new object[] { "? R2", 40, numArray[40] });

                this.dataGridView1.Rows.Add(new object[] { "? V1", 45, numArray[45] });

                this.dataGridView1.Rows.Add(new object[] { "? V2", 46, numArray[46] });

                this.dataGridView1.Rows.Add(new object[] { "Về", 113, ve });

                this.dataGridView1.Rows.Add(new object[] { "Can", 113, Can_dau_vao_hoan_thanh });

                //this.dataGridView1.Rows.Add(new object[] { "Điện trở Max", " (35 , 2) ", test });
                //this.dataGridView1.Rows.Add(new object[] { "Điện trở Min", " (37 , 2) ", test });
                //this.dataGridView1.Rows.Add(new object[] { "Điện áp", " 103, 104 ", test });
                //this.dataGridView1.Rows.Add(new object[] { "Điện áp Max", " (41 , 2) ", test });
                //this.dataGridView1.Rows.Add(new object[] { "Điện áp Min", " (43 , 2) ", test });
                //    Random R = new Random();
                //int test = R.Next(0, 10);
                // Initialize an empty string to hold the ASCII representation




                string asciiString = "";

                // Loop through the elements of numArray that you want to convert
                for (int i = 0; i <= 9; i++)
                {
                    if (numArray[i] != 0)
                    {
                        asciiString += READHODINGREGISTER_to_ASCII(numArray[i]);
                    }
                }

                //Add the new row to dataGridView1
                this.dataGridView1.Rows.Add(new object[] { "Mã số lô hàng", "1 - 9", asciiString.Trim() });
                asciiString = "";
                // index 11 - 19
                for (int i = 11; i <= 19; i++)
                {
                    if (numArray[i] != 0)
                    {
                        asciiString += READHODINGREGISTER_to_ASCII(numArray[i]);
                    }
                }

                // Add the new row to dataGridView1
                this.dataGridView1.Rows.Add(new object[] { "Quy cách", "11 - 19", asciiString.Trim() });
                // index 22 - 31
                asciiString = "";
                for (int i = 22; i <= 31; i++)
                {
                    if (numArray[i] != 0)
                    {
                        asciiString += READHODINGREGISTER_to_ASCII(numArray[i]);
                    }
                }

                // Add the new row to dataGridView1
                this.dataGridView1.Rows.Add(new object[] { "Người thao tác", "22 - 31", asciiString.Trim() });

            }
            catch (Exception ex)
            {
                testTimer.Stop();

                //MessageBox.Show(ex.Message);
            }
        }



        private void ClassifyAndCompare(int[] registerValues)
        {

            double num = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(35, 2));
            double resistanceMax = Convert.ToDouble(string.Format("{0:0.00}", num / 100));
            double num1 = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(37, 2));
            double resistanceMin = Convert.ToDouble(string.Format("{0:0.00}", num1 / 100));
            double num2 = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(41, 2));
            double voltageMax = Convert.ToDouble(string.Format("{0:0.00}", num2 / 100));
            double num3 = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(43, 2));
            double voltageMin = Convert.ToDouble(string.Format("{0:0.00}", num3 / 100));

            // Convert the register values to float for comparison
            float registerDisplay = Twoint16ConverttoFloat(registerValues[99], registerValues[100]);
            float voltageDisplay = Twoint16ConverttoFloat(registerValues[103], registerValues[104]);


            // Classify register display value
            if (registerDisplay <= 0f)
            {
                this.modbustcp.WriteSingleRegister(106, 1);
                //label8.Text = "FAULT";
            }
            else if (registerDisplay >= resistanceMin && registerDisplay <= resistanceMax)
            {
                this.modbustcp.WriteSingleRegister(106, 2);
                //label8.Text = "OK";
            }
            else if (registerDisplay < resistanceMin)
            {
                this.modbustcp.WriteSingleRegister(106, 3);
                //label8.Text = "LOW";
            }
            else if (registerDisplay > resistanceMax)
            {
                this.modbustcp.WriteSingleRegister(106, 4);
                //label8.Text = "HIGH";
            }

            // Classify voltage display value
            if (voltageDisplay <= 0f)
            {
                this.modbustcp.WriteSingleRegister(108, 1);
                //label9.Text = "FAULT";
            }
            else if (voltageDisplay >= voltageMin && voltageDisplay <= voltageMax)
            {
                this.modbustcp.WriteSingleRegister(108, 2);
                //label9.Text = "OK";
            }
            else if (voltageDisplay < voltageMin)
            {
                this.modbustcp.WriteSingleRegister(108, 3);
                //label9.Text = "LOW";
            }
            else if (voltageDisplay > voltageMax)
            {
                this.modbustcp.WriteSingleRegister(108, 4);
                //label9.Text = "HIGH";
            }
        }

        //private void UpdateConnectedChanged(object sender)
        //{
        //    if (modbustcp.Connected)
        //    {
        //        label3.Text = "Connected to Server";
        //        label3.BackColor = Color.Green;
        //    }
        //    else
        //    {
        //        label3.Text = "Not Connected to Server";
        //        label3.BackColor = Color.Red;
        //    }
        //}


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        void UpdateReceiveData(object sender)
        {
            //MessageBox.Show("Đã nhận dữ liệu");

            //receiveData = "Rx: " + BitConverter.ToString(modbustcp.receiveData).Replace("-", " ") + System.Environment.NewLine;
            //MessageBox.Show(receiveData);
            Thread thread = new Thread(updateReceiveTextBox);
            thread.Start();
        }



        delegate void UpdateReceiveDataCallback();
        void updateReceiveTextBox()
        {
            if (richTextBox1.InvokeRequired)
            {
                UpdateReceiveDataCallback d = new UpdateReceiveDataCallback(updateReceiveTextBox);
                this.Invoke(d, new object[] { });
            }
            else
            {
                richTextBox1.AppendText(receiveData);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                modbustcp.IPAddress = textBox1.Text;
                modbustcp.Port = 502;
                modbustcp.Connect();
                if (!modbustcp.Connected)
                {
                    //MessageBox.Show("Connect fail");
                }
                else
                {
                    MessageBox.Show("Connect success");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void SendMessage(string message)
        {
            try
            {
                message += "\r\n";
                NetworkStream stream = LanSocket.GetStream();
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SendMessage(":FETch?");
                await ReceiveDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private async Task ReceiveDataAsync()
        {
            try
            {
                float single;
                float single1;
                NetworkStream stream = LanSocket.GetStream();
                byte[] buffer = new byte[65536];
                StringBuilder messageBuilder = new StringBuilder();
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        MessageBox.Show("Không có dữ liệu trả về!");
                        break;
                    }

                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    messageBuilder.Append(data);

                    if (data.Length > 0)
                    {
                        string receivedMessage = messageBuilder.ToString();
                        richTextBox2.Text = receivedMessage;

                        //this.label3.Text = this.comm.MsgBuf.ToString();
                        string[] strArrays = receivedMessage.Split(new char[] { ',' });
                        string tmp1 = strArrays[0].ToString();

                        string[] strArrays1 = tmp1.Split(new char[] { 'E' });

                        string tmp2 = strArrays1[0].ToString();

                        float.TryParse(tmp2, out single);
                        rStr = single.ToString();
                        MessageBox.Show(rStr);

                        string tmp3 = strArrays[1].ToString();
                        float.TryParse(tmp3, out single1);
                        vStr = single1.ToString();
                        MessageBox.Show(vStr);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }





        protected virtual void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(this, message);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // kiểm tra xem đã kết nối chưa
            if (!modbustcp.Connected)
            {
                MessageBox.Show("Chưa kết nối");
                return;
            }

            int[] numArray = modbustcp.ReadHoldingRegisters(0, 122);
            // convert number to string
            string str = string.Join(" ", numArray);
            // convert number to float

            float regsisterdisplay = this.Twoint16ConverttoFloat(numArray[99], numArray[100]);

            float voltdisplay = this.Twoint16ConverttoFloat(numArray[103], numArray[104]);
            double num = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(35, 2));
            double dientromax = Convert.ToDouble(string.Format("{0:0.00}", num / 100));
            double num1 = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(37, 2));
            double dientromin = Convert.ToDouble(string.Format("{0:0.00}", num1 / 100));
            double num2 = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(41, 2));
            double dienapmax = Convert.ToDouble(string.Format("{0:0.00}", num2 / 100));
            double num3 = (double)ModbusClient.ConvertRegistersToInt(modbustcp.ReadHoldingRegisters(43, 2));
            double dienapmin = Convert.ToDouble(string.Format("{0:0.00}", num3 / 100));

            float SOCA = numArray[33];
            float tong_binh_ca1 = numArray[47];
            float OK_R_ca1 = numArray[49];
            float Thap_R_ca1 = numArray[51];
            float Cao_R_ca1 = numArray[53];
            float tong_binh_ca2 = numArray[55];
            float OK_R_ca2 = numArray[57];
            float Thap_R_ca2 = numArray[59];
            float Cao_R_ca2 = numArray[61];
            float XoaDothi = numArray[67];
            float XoaCPK = numArray[69];
            float Hetca1 = numArray[71];
            float Hetca2 = numArray[73];
            float Can_dau_vao_hoan_thanh = numArray[75];
            float ve = numArray[113];
            float OK_V_ca1 = numArray[77];
            float Thap_V_ca1 = numArray[79];
            float Cao_V_ca1 = numArray[81];
            float OK_V_ca2 = numArray[83];
            float Thap_V_ca2 = numArray[85];
            float Cao_V_ca2 = numArray[87];

            float v1 = numArray[39];

            float v2 = numArray[40];

            float v3 = numArray[45];

            float v4 = numArray[46];


            // Khai báo và gán giá trị cho các biến nhưng không có class Functions
            // tạo cột tên cho gridview
            this.dataGridView1.ColumnCount = 3;
            this.dataGridView1.Columns[0].Name = "Tên";
            this.dataGridView1.Columns[1].Name = "Địa chỉ thanh ghi";
            this.dataGridView1.Columns[2].Name = "Giá trị";


            this.dataGridView1.Rows.Add(new object[] { "Điện trở", "(99 , 100)", regsisterdisplay });
            this.dataGridView1.Rows.Add(new object[] { "Điện trở Max", " (35 , 2) ", dientromax });
            this.dataGridView1.Rows.Add(new object[] { "Điện trở Min", " (37 , 2) ", dientromin });
            this.dataGridView1.Rows.Add(new object[] { "Điện áp", " 103, 104 ", voltdisplay });
            this.dataGridView1.Rows.Add(new object[] { "Điện áp Max", " (41 , 2) ", dienapmax });
            this.dataGridView1.Rows.Add(new object[] { "Điện áp Min", " (43 , 2) ", dienapmin });
            this.dataGridView1.Rows.Add(new object[] { "Số Cả", 33, SOCA });
            this.dataGridView1.Rows.Add(new object[] { "Tổng Bình Cả 1", 47, tong_binh_ca1 });
            this.dataGridView1.Rows.Add(new object[] { "OK R Cả 1", 49, OK_R_ca1 });
            this.dataGridView1.Rows.Add(new object[] { "Thấp R Cả 1", 51, Thap_R_ca1 });
            this.dataGridView1.Rows.Add(new object[] { "Cao R Cả 1", 53, Cao_R_ca1 });
            this.dataGridView1.Rows.Add(new object[] { "Tổng Bình Cả 2", 55, tong_binh_ca2 });
            this.dataGridView1.Rows.Add(new object[] { "OK R Cả 2", 57, OK_R_ca2 });
            this.dataGridView1.Rows.Add(new object[] { "Thấp R Cả 2", 59, Thap_R_ca2 });
            this.dataGridView1.Rows.Add(new object[] { "Cao R Cả 2", 61, Cao_R_ca2 });
            this.dataGridView1.Rows.Add(new object[] { "Cân Đầu Vào Hoàn Thành", 75, Can_dau_vao_hoan_thanh });
            this.dataGridView1.Rows.Add(new object[] { "Về", 113, ve });
            this.dataGridView1.Rows.Add(new object[] { "OK V Cả 1", 77, OK_V_ca1 });
            this.dataGridView1.Rows.Add(new object[] { "Thấp V Cả 1", 79, Thap_V_ca1 });
            this.dataGridView1.Rows.Add(new object[] { "Cao V Cả 1", 81, Cao_V_ca1 });
            this.dataGridView1.Rows.Add(new object[] { "OK V Cả 2", 83, OK_V_ca2 });
            this.dataGridView1.Rows.Add(new object[] { "Thấp V Cả 2", 85, Thap_V_ca2 });
            this.dataGridView1.Rows.Add(new object[] { "Cao V Cả 2", 87, Cao_V_ca2 });
            this.dataGridView1.Rows.Add(new object[] { "Xóa Đồ Thị", 67, XoaDothi });
            this.dataGridView1.Rows.Add(new object[] { "Xóa CPK", 69, XoaCPK });
            this.dataGridView1.Rows.Add(new object[] { "Hết Cả 1", 71, Hetca1 });
            this.dataGridView1.Rows.Add(new object[] { "Hết Cả 2", 73, Hetca2 });

            this.dataGridView1.Rows.Add(new object[] { "Hết Cả 1", 39, v1 });
            this.dataGridView1.Rows.Add(new object[] { "Hết Cả 2", 40, v2 });

            this.dataGridView1.Rows.Add(new object[] { "Hết Cả 1", 45, v3 });
            this.dataGridView1.Rows.Add(new object[] { "Hết Cả 2", 46, v4 });


            // Initialize an empty string to hold the ASCII representation
            string asciiString = "";

            // Loop through the elements of numArray that you want to convert
            for (int i = 1; i <= 9; i++)
            {
                if (numArray[i] != 0)
                {
                    asciiString += READHODINGREGISTER_to_ASCII(numArray[i]);
                }
            }

            // Add the new row to dataGridView1
            this.dataGridView1.Rows.Add(new object[] { "Mã số lô hàng", "1 - 9", asciiString.Trim() });
            asciiString = "";
            // index 11 - 19
            for (int i = 11; i <= 19; i++)
            {
                if (numArray[i] != 0)
                {
                    asciiString += READHODINGREGISTER_to_ASCII(numArray[i]);
                }
            }

            // Add the new row to dataGridView1
            this.dataGridView1.Rows.Add(new object[] { "Quy cách", "11 - 19", asciiString.Trim() });
            // index 22 - 31
            asciiString = "";
            for (int i = 22; i <= 31; i++)
            {
                if (numArray[i] != 0)
                {
                    asciiString += READHODINGREGISTER_to_ASCII(numArray[i]);
                }
            }

            // Add the new row to dataGridView1
            this.dataGridView1.Rows.Add(new object[] { "Người thao tác", "22 - 31", asciiString.Trim() });


        }
        public float Twoint16ConverttoFloat(int Val1, int Val2)
        {
            byte[] bytes = BitConverter.GetBytes(Val1);
            byte[] numArray = BitConverter.GetBytes(Val2);
            byte[] numArray1 = new byte[] { bytes[0], bytes[1], numArray[0], numArray[1] };
            return BitConverter.ToSingle(numArray1, 0);
        }

        public static int ConvertRegistersToInt(int[] registers)
        {
            if (registers.Length != 2)
            {
                throw new ArgumentException("Input Array length invalid - Array langth must be '2'");
            }

            int value = registers[1];
            int value2 = registers[0];
            byte[] bytes = BitConverter.GetBytes(value);
            byte[] bytes2 = BitConverter.GetBytes(value2);
            byte[] value3 = new byte[4]
            {
            bytes2[0],
            bytes2[1],
            bytes[0],
            bytes[1]
            };
            return BitConverter.ToInt32(value3, 0);
        }

        public static string ConvertRegisterValueToAscii(int registerValue)
        {
            // Convert the register value to a hexadecimal string
            string hexValue = registerValue.ToString("X");

            // Split the hexadecimal string into two parts
            string firstPart = hexValue.Substring(0, 2);
            string secondPart = hexValue.Substring(2);

            // Convert the first part to a long integer
            long firstPartAsLong = Convert.ToInt64(firstPart, 16);

            // Convert the second part to a long integer, if it's not an empty string
            long secondPartAsLong = 0;
            if (!string.IsNullOrEmpty(secondPart))
            {
                secondPartAsLong = Convert.ToInt64(secondPart, 16);
            }

            // Convert the long integers to their corresponding ASCII characters
            char firstChar = Convert.ToChar(firstPartAsLong);
            char secondChar = Convert.ToChar(secondPartAsLong);

            // Return the concatenated string
            return string.Concat(firstChar, secondChar);
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






        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // nêu chưa kết nối thì thông báo
                if (!LanSocket.Connected)
                {
                    MessageBox.Show("Chưa kết nối");
                    return;
                }
                LanSocket?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // nếu chưa kết nối thì thông báo
                if (!modbustcp.Connected)
                {
                    MessageBox.Show("Chưa kết nối");
                    return;
                }

                modbustcp?.Disconnect();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show("Đã ngắt kết nối");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            richTextBox1.Clear();
            dataGridView1.ClearSelection();
            dataGridView1.Rows.Clear();
        }

        private void button9_Click(object sender, EventArgs e)
        {

            // nếu đang start thì thông báo
            if (testTimer.Enabled)
            {
                return;
            }

            testTimer.Interval = 50;
            testTimer.Start();
        }




        private void button8_Click(object sender, EventArgs e)
        {
            // xóa các cột và hàng trong gridview
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            // tạo cột tên cho gridview

            this.dataGridView1.ColumnCount = 3;
            this.dataGridView1.Columns[0].Name = "Tên";
            this.dataGridView1.Columns[1].Name = "Địa chỉ thanh ghi";
            this.dataGridView1.Columns[2].Name = "Giá trị";

            this.dataGridView1.Rows.Add(new object[] { "Điện trở", 1, 1 });


            this.dataGridView1.Rows.Add(new object[] { "Điện trở", "(99 , 100)", 11 });

            this.dataGridView1.Rows.Add(new object[] { "Điện trở Max", " (35 , 2) ", 1 });
            this.dataGridView1.Rows.Add(new object[] { "Điện trở Min", " (37 , 2) ", 1 });
            this.dataGridView1.Rows.Add(new object[] { "Điện áp", " 103, 104 ", 1 });
            this.dataGridView1.Rows.Add(new object[] { "Điện áp Max", " (41 , 2) ", 1 });
            this.dataGridView1.Rows.Add(new object[] { "Điện áp Min", " (43 , 2) ", 1 });
            this.dataGridView1.Rows.Add(new object[] { "Số Cả", 33, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Tổng Bình Cả 1", 47, 1 });
            this.dataGridView1.Rows.Add(new object[] { "OK R Cả 1", 49, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Thấp R Cả 1", 51, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Cao R Cả 1", 53, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Tổng Bình Cả 2", 55, 1 });
            this.dataGridView1.Rows.Add(new object[] { "OK R Cả 2", 57, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Thấp R Cả 2", 59, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Cao R Cả 2", 61, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Cân Đầu Vào Hoàn Thành", 75, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Về", 113, 1 });
            this.dataGridView1.Rows.Add(new object[] { "OK V Cả 1", 77, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Thấp V Cả 1", 79, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Cao V Cả 1", 81, 1 });
            this.dataGridView1.Rows.Add(new object[] { "OK V Cả 2", 83, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Thấp V Cả 2", 85, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Cao V Cả 2", 87, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Xóa Đồ Thị", 67, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Xóa CPK", 69, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Hết Cả 1", 71, 1 });
            this.dataGridView1.Rows.Add(new object[] { "Hết Cả 2", 73, 1 });
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // nếu đang start thì dừng
            if (testTimer.Enabled)
            {
                testTimer.Stop();
            }
        }

        private void test_FormClosing(object sender, FormClosingEventArgs e)
        {
            testTimer.Stop();
            Application.Exit();
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            modbustcp.WriteSingleRegister(39, 0);
            modbustcp.WriteSingleRegister(40, 0);
            modbustcp.WriteSingleRegister(45, 0);
            modbustcp.WriteSingleRegister(46, 0);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                if (!float.TryParse(rStr, out float single))
                {
                    // Xử lý lỗi ở đây nếu cần thiết
                    single = 0; // Set giá trị mặc định nếu parse thất bại
                }

                r = float.Parse(string.Format("{0:0.00}", single * 1000f));


                if (!float.TryParse(vStr, out float single2))
                {
                    // Xử lý lỗi ở đây nếu cần thiết
                    single2 = 0; // Set giá trị mặc định nếu parse thất bại
                }

                v = float.Parse(string.Format("{0:0.00}", single2));

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
                    modbustcp.WriteSingleRegister(39, 0);
                    modbustcp.WriteSingleRegister(40, 0);
                }
                int[] num1 = new int[2];
                ushort[] voltRegisters = FloatToModbusRegisters(v);
                num1[0] = Convert.ToInt32(voltRegisters[0]);
                num1[1] = Convert.ToInt32(voltRegisters[1]);
                modbustcp.WriteSingleRegister(45, num1[0]);
                modbustcp.WriteSingleRegister(46, num1[1]);

            }
            catch (Exception exception)
            {
            }
        }


        private ushort[] FloatToModbusRegisters(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            //if (!BitConverter.IsLittleEndian)
            //{
            //    Array.Reverse(bytes); // Đảm bảo byte order đúng định dạng Little Endian
            //}

            ushort lowOrderValue = BitConverter.ToUInt16(bytes, 0);
            ushort highOrderValue = BitConverter.ToUInt16(bytes, 2);

            // Trong Modbus, đôi khi các giá trị cao (high-order) được gửi trước giá trị thấp (low-order)
            // Ví dụ: return new ushort[] { highOrderValue, lowOrderValue };
            return new ushort[] { lowOrderValue, highOrderValue };
        }

        private void button13_Click(object sender, EventArgs e)
        {

            List<string> resultList = new List<string>();

            string receivedMessage = "1.578E-3,1.23E+0";
            string[] messages = receivedMessage.Split(new char[] { ',' });

            if (messages.Length >= 2)
            {

                if (float.TryParse(messages[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float resistanceValue) &&
                    float.TryParse(messages[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float voltageValue))
                {
                    resultList.Add((resistanceValue*1000f).ToString(CultureInfo.InvariantCulture ));
                    resultList.Add(voltageValue.ToString(CultureInfo.InvariantCulture));


                    if (resultList.Count == 0)
                    {
                        return;
                    }

                    if (!float.TryParse(resultList[0], out float single))
                    {
                        // Xử lý lỗi ở đây nếu cần thiết
                        single = 0; // Set giá trị mặc định nếu parse thất bại
                    }

                    float r = float.Parse(string.Format("{0:0.00}", single * 1000f));


                    if (!float.TryParse(resultList[1], out float single2))
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

                        MessageBox.Show(num[0].ToString());
                        MessageBox.Show(num[1].ToString());


                        //modbustcp.WriteSingleRegister(39, num[0]);
                        //modbustcp.WriteSingleRegister(40, num[1]);

                    }
                    else
                    {
                        // Nếu là giá trị R không hợp lệ thì write 0

                        //modbustcp.WriteSingleRegister(39, 0);
                        //modbustcp.WriteSingleRegister(40, 0);
                    }
                }
                else
                {
                    throw new FormatException("Received data is not in the expected format.");
                }
            }
            else
            {
                throw new FormatException("Insufficient data received.");
            }
        }
                    
    } 
}




