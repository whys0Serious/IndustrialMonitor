using IndustrialMonitor.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IndustrialMonitor.Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        //SensorRecords表
        public DbSet<SensorRecord> SensorRecords => Set<SensorRecord>();
    }
}
