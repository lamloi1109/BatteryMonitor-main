using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using NPOI.SS.Formula.Functions;

namespace BatteryMonitor.HIOKI
{
    public class hioki
    {
        private string hiokiIp;
        private string hiokiPort;
        private TcpClient hiokiTcpSocket;
        private string msgBuf = "";
        private string hiokiConnectStatus;

        // NLog
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public hioki(string hiokiIp, string hiokiPort, TcpClient hiokiTcpSocket)
        {
            this.hiokiIp = hiokiIp;
            this.hiokiPort = hiokiPort;
            this.hiokiTcpSocket = hiokiTcpSocket;
        }

        public string HiokiIp
        {
            get { return hiokiIp; }
            set { hiokiIp = value; }
        }

        public string HiokiPort
        {
            get { return hiokiPort; }
            set { hiokiPort = value; }
        }

        public TcpClient HiokiTcpSocket
        {
            get { return hiokiTcpSocket; }
            set { hiokiTcpSocket = value; }
        }

        public string MsgBuf
        {
            get { return msgBuf; }
            set { msgBuf = value; }
        }

        public string HiokiConnectStatus
        {
            get { return hiokiConnectStatus; }
            set { hiokiConnectStatus = value; }
        }

        // method connect
        public bool TryConnectToTcpServer()
        {
            string ipaddress = this.hiokiIp;
            string port = this.hiokiPort;

            if (IPAddress.TryParse(ipaddress, out IPAddress pAddress) && int.TryParse(port, out int portNumber))
            {
                try
                {
                    this.hiokiTcpSocket = new TcpClient()
                    {
                        NoDelay = true
                    };
                    this.hiokiTcpSocket.Connect(pAddress, portNumber);
                    this.hiokiConnectStatus = "Connected";
                    return true; // Thành công, trả về true
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Không thể kết nối: {ex.Message}");
                    // Ở đây bạn có thể xử lý hoặc log lỗi theo mong muốn
                    return false; // Kết nối thất bại, trả về false
                }
            }
            else
            {
                Console.WriteLine("Địa chỉ IP hoặc cổng không hợp lệ.");
                return false; // Địa chỉ IP hoặc cổng không hợp lệ, trả về false
            }
        }
        // method closeConnect
        public bool TryCloseConnectToTcpServer()
        {
            try
            {
                this.hiokiTcpSocket.Close();
                this.hiokiConnectStatus = "DisConnected";
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        // method check connection to tcp server

        public bool IsTcpSocketConnected()
        {
            // Kiểm tra đối tượng TcpClient có null hay không
            if (this.hiokiTcpSocket == null) return false;

            try
            {
                // Lấy thông tin socket
                Socket socket = this.hiokiTcpSocket.Client;

                if (socket == null) return false;

                // Kiểm tra xem socket có bị đóng hay không
                if (!socket.Connected) return false;

                // Ngoài ra kiểm tra xem có thể gửi và nhận dữ liệu trong trạng thái phi chặn hoặc có lỗi trên socket
                bool part1 = socket.Poll(1000, SelectMode.SelectRead);
                bool part2 = (socket.Available == 0);

                if (part1 && part2)
                    return false; // Cổng đọc có dữ liệu nhưng không đọc được, có nghĩa là kết nối đã bị đóng
                

                return true;
            }
            catch
            {
                // Trường hợp có lỗi, coi như kết nối không thành công
                return false;
            }
        }


        // method ReceiveMsg
        public async Task<List<string>> ReceiveMsg(long timeout_ms)
        {
            List<string> resultList = new List<string>();
            //TEST
            byte[] buffer = new byte[65536];
            Stopwatch stopwatch = new Stopwatch();
            //TEST
            NetworkStream stream = this.hiokiTcpSocket.GetStream();
            StringBuilder messageBuilder = new StringBuilder();

            try
            {
                while (true)
                {
                    if (stopwatch.ElapsedMilliseconds > timeout_ms)
                    {
                        logger.Info("Request timeout!");
                        break;
                    }

                    //TEST
                    if (!stream.CanRead)
                    {
                        throw new InvalidOperationException("Cannot read from network stream.");
                    }

                    //TEST
                    //// Đọc dữ liệu từ network stream
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        throw new IOException("No bytes read from network stream; connection may be lost.");
                    }

                    //TEST
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    //string data = randomValue();
                    messageBuilder.Append(data);

                    // Xử lý chuỗi dữ liệu nhận được
                    if (data.Length > 0)
                    {
                        string receivedMessage = messageBuilder.ToString();
                        string[] messages = receivedMessage.Split(new char[] { ',' });

                        if (messages.Length >= 2)
                        {

                            if (float.TryParse(messages[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float resistanceValue) &&
                                float.TryParse(messages[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float voltageValue))
                            {
                                resultList.Add((resistanceValue*1000f).ToString(CultureInfo.InvariantCulture));
                                resultList.Add(voltageValue.ToString(CultureInfo.InvariantCulture));
                                stopwatch.Stop();
                                return resultList;
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
                    else
                    {
                        throw new InvalidOperationException("Received data is empty.");
                    }  
                }
               
            }
            catch (Exception ex)
            {
                // Bạn có thể thêm xử lý lỗi chi tiết ở đây, ví dụ ghi lỗi vào file log hoặc thông báo lỗi
                Console.WriteLine($"An error occurred: {ex.Message}");
                logger.Error(ex.Message);

            }

            return resultList;
        }



        static string randomValue()
        {
            Random R = new Random();
            double rMin = 5;
            double rMax = 8;
            double vMin = 12.86;
            double vMax = 13.05;

            double random1 = rMin + (rMax + 3 - (rMin - 3)) * R.NextDouble();
            double random2 = vMin + (vMax + 1 - (vMin - 3)) * R.NextDouble();

            // Định dạng thành chuỗi số mũ
            string result1 = random1.ToString("#.###E-3", System.Globalization.CultureInfo.InvariantCulture);
            string result2 = random2.ToString("0.###E+0", System.Globalization.CultureInfo.InvariantCulture);

            // Chuỗi kết quả
            string finalResult = $"{result1},{result2}";

            //Console.WriteLine(finalResult); // Ví dụ: "3.31E-3,1.23E+0"
            return finalResult;
        }



        // method SendMsg
        public bool SendMsg(string strMsg)
        {
            try
            {
                if (this.hiokiTcpSocket != null && this.hiokiTcpSocket.Connected)
                {
                    NetworkStream stream = this.hiokiTcpSocket.GetStream();
                    if (stream.CanWrite)
                    {
                        strMsg += "\r\n";
                        byte[] bytes = Encoding.ASCII.GetBytes(strMsg);
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush(); // Đảm bảo dữ liệu được gửi ngay lập tức
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            return false;
        }
        // method SendQueryMsg
        public bool SendQueryMsg(string strMsg, long timeoutFms)
        {
            try
            {
                // Kiểm tra xem strMsg có phải là một truy vấn không.
                if (!strMsg.Contains("?"))
                {
                    // Nếu không phải là truy vấn, không cần thực hiện nhận tin nhắn.
                    return SendMsg(strMsg);
                }

                if (this.hiokiTcpSocket == null || !this.hiokiTcpSocket.Connected)
                {
                    return false; // Nếu LanSocket không kết nối, trả về false.
                }

                NetworkStream stream = this.hiokiTcpSocket.GetStream();
                // Dọn dẹp bất kỳ dữ liệu đang chờ sẵn nếu có.
                if (stream.DataAvailable)
                {
                    byte[] cleanupBuffer = new byte[65536];
                    stream.Read(cleanupBuffer, 0, cleanupBuffer.Length);
                }

                // Gửi tin nhắn.
                if (SendMsg(strMsg))
                {
                    // Nhận phản hồi chỉ khi tin nhắn được gửi thành công
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework or method)
                logger.Error(ex.Message);
            }

            return false;
        }
    }
}
