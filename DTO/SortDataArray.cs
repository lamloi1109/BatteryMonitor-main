using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatteryMonitor
{
    public class SortDataArray
    {
        private static SortDataArray instance;
        public static SortDataArray Instance
        {
            get
            {
                if (instance == null) instance = new SortDataArray();
                return instance;
            }
            private set => instance = value;
        }
        private SortDataArray() { }

        public DataTable Sort(int numID,Array IDArray, Array ValueArray, Array Qualities=null,Array TimeArray = null)
        {
            DataTable data = new DataTable();
            if (numID > 0)
            {
               
                data.Columns.Add("ID", typeof(int));
                data.Columns.Add("Value", typeof(string));
                data.Columns.Add("Quality", typeof(string));
                data.Columns.Add("Time", typeof(string));

                for (int i = 1; i <= numID; i++) //Giá trị Handle bắt đầu từ 1
                {
                    int id = Convert.ToInt32(IDArray.GetValue(i));
                    object itemValue = ValueArray.GetValue(i);
                    object Quality = Qualities.GetValue(i);
                    object Time = TimeArray.GetValue(i);
                    data.Rows.Add(id, itemValue.ToString(), Quality.ToString(), Time.ToString());
                }

                DataView dataView = new DataView(data);
                dataView.Sort = "ID ASC";  
                data = dataView.ToTable();
            }
            return data;
        }

    }

}
