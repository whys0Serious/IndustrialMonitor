using System;
using System.Collections.Generic;
using System.Text;

namespace IndustrialMonitor.Core.Enums
{
    public class TagConfig
    {
        public string Name { get; set; } = string.Empty;
        public ushort Address { get; set; }
        public PlcDataType DataType { get; set; } = PlcDataType.Int16;

        public bool IsLittleEndian { get; set; } = false;
        public string Unit { get; set; } = string.Empty;
    }
}
