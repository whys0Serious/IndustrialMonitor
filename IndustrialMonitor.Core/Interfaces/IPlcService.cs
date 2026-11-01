using System;
using System.Collections.Generic;
using System.Text;

namespace IndustrialMonitor.Core.Interfaces
{
    public interface IPlcService
    {
        /// <summary>
        /// 当前连接状态
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 断开连接
        /// </summary>
        void DisConnect();
        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <returns></returns>
        Task<bool> ConnectAsync();
        /// <summary>
        /// 读取寄存器数据
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="startAddress"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<ushort[]> ReadAsync(byte slaveId,ushort startAddress,ushort count);
    }
}
