using IndustrialMonitor.Core.Enums;
using IndustrialMonitor.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace IndustrialMonitor.Infrastructure.Services
{
    public class DataParser : IDataParser
    {
        public bool ParseBool(ushort register, int bitPosition)
        {
            //通过位移与运算获取指定位置的位值
            return ((register >> bitPosition) & 1) == 1;
        }

        /// <summary>
        /// 泛型解析方法
        /// </summary>
        /// <param name="registers">从 PLC 读到的寄存器数组</param>
        /// <param name="dataType">要解析成的数据类型</param>
        /// <param name="isLittleEndian">是否是字交换/小端序（现场经常需要微调这个）</param>
        public object Parse(ushort[] registers, PlcDataType dataType, bool isLittleEndian = false)
        {
            if (registers == null || registers.Length == 0) return 0;
            if (dataType == PlcDataType.Int16) return (short)registers[0];
            if (dataType == PlcDataType.UInt16) return registers[0];
            if (dataType == PlcDataType.Bool) return registers[0] > 0;

            if (registers.Length < 2) throw new ArgumentException("32位数据至少需要2个寄存器");

            // 将两个 16 位寄存器拆成 4 个独立字节
            byte b0 = (byte)(registers[0] >> 8);   // A
            byte b1 = (byte)(registers[0] & 0xFF); // B
            byte b2 = (byte)(registers[1] >> 8);   // C
            byte b3 = (byte)(registers[1] & 0xFF); // D

            byte[] bytes = new byte[4];


            // 【方案 1：标准大端 ABCD】
           // bytes[0] = b0; bytes[1] = b1; bytes[2] = b2; bytes[3] = b3;

            // 【方案 2：字交换 CDAB】（西门子/部分模拟器最爱用，概率极高）
             //bytes[0] = b2; bytes[1] = b3; bytes[2] = b0; bytes[3] = b1;

            // 【方案 3：字节交换 BADC】
             //bytes[0] = b1; bytes[1] = b0; bytes[2] = b3; bytes[3] = b2;

            // 【方案 4：完全小端 DCBA】
            bytes[0] = b3; bytes[1] = b2; bytes[2] = b1; bytes[3] = b0;

            // 注意：这里去掉了 Array.Reverse()，完全按照你选择的顺序拼装
            uint combined = BitConverter.ToUInt32(bytes, 0);

            return dataType switch
            {
                PlcDataType.Int32 => (int)combined,
                PlcDataType.Float32 => BitConverter.ToSingle(bytes, 0),
                _ => throw new NotSupportedException($"不支持的数据类型: {dataType}")
            };
        }
    }
}
