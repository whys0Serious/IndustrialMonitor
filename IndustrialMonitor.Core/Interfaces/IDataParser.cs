using IndustrialMonitor.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IndustrialMonitor.Core.Interfaces
{
    public interface IDataParser
    {
      
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="registers"></param>
        /// <param name="dataType"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        public object Parse(ushort[] registers, PlcDataType dataType, bool isLittleEndian = false);

        /// <summary>
        /// 将单个寄存器的某一位解析为bool值
        /// </summary>
        /// <param name="register"></param>
        /// <param name="bitPosition"></param>
        /// <returns></returns>
        bool ParseBool(ushort register, int bitPosition);

    }
}
