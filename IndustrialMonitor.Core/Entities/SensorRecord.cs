using System;
using System.Collections.Generic;
using System.Text;

namespace IndustrialMonitor.Core.Entities
{
    public class SensorRecord
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }
}
