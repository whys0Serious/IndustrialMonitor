using System;
using System.Collections.Generic;
using System.Text;

namespace IndustrialMonitor.Core.Enums
{
    public enum PlcDataType
    {
        Int16,      // 16位有符号整数（常用）
        UInt16,     // 16位无符号整数
        Int32,      // 32位有符号整数（占2个寄存器）
        Float32,    // 32位浮点数（占2个寄存器）
        Bool        // 布尔开关（需要位索引）
    }
}
