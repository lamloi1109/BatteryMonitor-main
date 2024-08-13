using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatteryMonitor.Data
{
    public class battery
    {
        // Số bình
        private int totalMeasureMent;
        // Điện trở Min
        private double rMin;
        // Điện trở Max
        private double rMax;
        // Điện áp min
        private double vMin;
        // Điện áp max
        private double vMax;
        // Điện áp
        private double v;
        // Điện trở
        private double r;
        // Mã pin
        private string batteryCode;
        // Quy cách
        private string specs;
        // Mã nhân viên thao tác
        private string userId;
        // Số ca hiện tại
        private int workShift;
        // Trạng thái pin
        private string measureMentStatus;
        // Số lô hàng
        private string shipmentId;
        private string date;

        // Phân loại của R hoặc V
        private string type_R;
        private string type_V;

        // Chất lượng bình
        private string quality;


        // Chỉ số đánh giá khả năng sản xuất
        private double cpk_R;
        private double cpk_V;

        // Độ lệch chuẩn
        private double std_V;
        private double std_R;

        // Trung bình
        private double ave_R;
        private double ave_V;

        public battery(double R, double V, double RMax, double RMin, double VMax, double Vmin, int totalMeasureMent, string BatteryCode, string UserId, string ShipmentId, string Specs, int WorkShift, string MeasureMentStatus, string Date,string quality ,string type_R, string type_V, double cpk_R, double cpk_V, double std_R, double std_V, double ave_R, double ave_V)
        {
            this.r = R;
            this.v = V;
            this.rMax = RMax;
            this.rMin = RMin;
            this.vMax = VMax;
            this.vMin = Vmin;
            this.totalMeasureMent = totalMeasureMent;
            this.batteryCode = BatteryCode;

            this.specs = Specs;
            this.userId = UserId;
            this.workShift = WorkShift;
            this.measureMentStatus = MeasureMentStatus;
            this.shipmentId = ShipmentId;
            this.date = Date;
            this.type_R = type_R;
            this.type_V = type_V;
            this.cpk_R = cpk_R;
            this.cpk_V = cpk_V;
            this.std_R = std_R;
            this.std_V = std_V;
            this.ave_R = ave_R;
            this.ave_V = ave_V;
            this.quality = quality;
        }

        // SETTER & GETTER
        public int TotalMeasureMent
        {
            get { return totalMeasureMent; }
            set { totalMeasureMent = value; }
        }

        public double RMAX
        {
            get { return rMax; }
            set { rMax = value; }
        }
        public double RMIN
        {
            get { return rMin; }
            set { rMin = value; }
        }
        public double VMAX
        {
            get { return vMax; }
            set { vMax = value; }
        }
        public double VMIN
        {
            get { return vMin; }
            set { vMin = value; }
        }

        public double R
        {
            get { return r; }
            set { r = value; }
        }

        public double V
        {
            get { return v; }
            set { v = value; }
        }

        public string BatteryCode
        {
            get { return batteryCode; }
            set { batteryCode = value; }
        }

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string ShipmentId
        {
            get { return shipmentId; }
            set { shipmentId = value; }
        }

        public int WorkShift
        {
            get { return workShift; }
            set { workShift = value; }
        }

        public string Specs
        {
            get { return specs; }
            set { specs = value; }
        }

        public string MeasureMentStatus
        {
            get { return measureMentStatus; }
            set { measureMentStatus = value; }
        }
        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public string QUALITY
        {
            get { return quality; }
            set { quality = value; }
        }

        public string TYPE_R
        {
            get { return type_R; }
            set { type_R = value; }
        }
        public string TYPE_V
        {
            get { return type_V; }
            set { type_V = value; }
        }

        public double CPK_V
        {
            get { return cpk_V; }
            set { cpk_V = value; }
        }

        public double CPK_R
        {
            get { return cpk_R; }
            set { cpk_R = value; }
        }
        public double STD_R
        {
            get { return std_R; }
            set { std_R = value; }
        }


        public double STD_V
        {
            get { return std_V; }
            set { std_V = value; }
        }

        public double AVE_V
        {
            get { return ave_V; }
            set { ave_V = value; }
        }

        public double AVE_R
        {
            get { return ave_R; }
            set { ave_R = value; }
        }

    }
}
