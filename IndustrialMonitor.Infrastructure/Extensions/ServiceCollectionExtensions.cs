using IndustrialMonitor.Core.Interfaces;
using IndustrialMonitor.Infrastructure.Data;
using IndustrialMonitor.Infrastructure.Repository;
using IndustrialMonitor.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IndustrialMonitor.Infrastructure.Extensions
{
    /// <summary>
    /// 扩展方法类，用于向 IServiceCollection 添加自定义服务
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            //在这里注册基础设施层的服务
            //注册Modbus服务
            var modbusIp = configuration["Plc:Ip"]??"127.0.0.1";
            var modbusPort = int.TryParse(configuration["Plc:Port"], out var port) ? port : 502;
            services.AddSingleton<IPlcService>(new NModbusService(modbusIp, modbusPort));

            //注册sqlite数据库上下文工厂
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=industrial_monitor.db";
            services.AddDbContextFactory<AppDbContext>(options=>options.UseSqlite(connectionString));

            //注册传感器数据仓储
            services.AddTransient<ISensorRepository, SensorRepository>();

            return services;
        }
    }
}
