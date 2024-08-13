using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatteryMonitor.Chart
{
    public class MeasureModel
    {
        public int times { get; set; }
        public double Value { get; set; }
        public double min { get; set; }
        public double max { get; set; }
    }
}
