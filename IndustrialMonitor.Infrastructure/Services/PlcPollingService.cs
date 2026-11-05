using IndustrialMonitor.Core.Enums;
using IndustrialMonitor.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace IndustrialMonitor.Infrastructure.Services
{
    public class PlcPollingService : BackgroundService
    {
        private readonly IPlcService _plcService;
        private readonly ILogger<PlcPollingService> _logger;

        private readonly IDataParser _dataParser;

        private readonly IConfiguration _configuration;


        //静态属性给UI层，方便直接访问
        public static readonly ConcurrentQueue<Dictionary<string, object>> DataQueue = new();

        public PlcPollingService(IPlcService plcService, ILogger<PlcPollingService> logger, IDataParser dataParser,IConfiguration configuration)
        {
            _plcService = plcService;
            _logger = logger;
            _dataParser = dataParser;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("后台PLC轮询服务启动");
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //如果没连接，尝试重新连接
                    if (!_plcService.IsConnected)
                    {
                        _logger.LogInformation("PLC未连接，尝试重新连接");
                       var connect=  await _plcService.ConnectAsync();
                        if(!connect)
                        {
                            _logger.LogWarning("PLC连接失败，3秒后重连");
                            await Task.Delay(3000, stoppingToken);
                            continue;
                        }
                    }
                    _logger.LogInformation("PLC连接成功");

                    //采集数据（这里假设采集的数据地址0读取2个，实际情况根据配置文件读取）
                    //读取PLC数据
                    var tags = _configuration.GetSection("Plc:Tags").Get<List<TagConfig>>();
                    var data = new Dictionary<string, object>();
                    //遍历解析
                    foreach (var item in tags)
                    {
                        ushort count = (item.DataType == PlcDataType.Float32 || item.DataType == PlcDataType.Int32) ? (ushort)2 : (ushort)1;
                        var registers = await _plcService.ReadAsync(1, item.Address, count);

                        var parsedValue = _dataParser.Parse(registers, item.DataType, item.IsLittleEndian);
                        data.Add(item.Name,parsedValue);
                    }

                    if (data.Count > 0)
                    {
                        DataQueue.Enqueue(data);
                    }
                    //轮询间隔（每秒记录一次数据）
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "轮询PLC数据时发生异常,即将断开重连");
                    _plcService.DisConnect();
                    await Task.Delay(3000, stoppingToken);
                }
            }
            _logger.LogInformation("后台PLC轮询服务安全停止");
        }
    }
}
