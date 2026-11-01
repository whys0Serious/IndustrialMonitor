using IndustrialMonitor.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IndustrialMonitor.Core.Interfaces
{
    public interface ISensorRepository
    {
        /// <summary>
        /// 获取N条数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<List<SensorRecord>> GetSensorListAsync(int count);

        /// <summary>
        /// 批量添加传感器数据
        /// </summary>
        /// <param name="sensors"></param>
        /// <returns></returns>
        Task AddSensorAsync(List<SensorRecord> sensors);
    }
}
