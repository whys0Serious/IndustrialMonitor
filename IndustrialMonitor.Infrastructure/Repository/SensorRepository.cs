using IndustrialMonitor.Core.Entities;
using IndustrialMonitor.Core.Interfaces;
using IndustrialMonitor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IndustrialMonitor.Infrastructure.Repository
{
    public class SensorRepository : ISensorRepository
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public SensorRepository(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task AddSensorAsync(List<SensorRecord> sensors)
        {
            //使用工厂创建DbContext实例,保证线程安全
            using var db = _dbFactory.CreateDbContext();
            db.SensorRecords.AddRange(sensors);
            await db.SaveChangesAsync();
        }

        public async Task<List<SensorRecord>> GetSensorListAsync(int count)
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.SensorRecords.OrderByDescending(s => s.Timestamp).Take(count).ToListAsync();
        }
    }
}
