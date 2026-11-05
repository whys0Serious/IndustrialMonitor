using CommunityToolkit.Mvvm.ComponentModel;
using IndustrialMonitor.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Threading;

namespace IndustrialMonitor.Client.ViewModel
{
    public partial class MainViewModel:ObservableObject
    {
        //绑定到界面的数据集合
        public ObservableCollection<string> RealTimeData { get; } = new ();
        private readonly DispatcherTimer _timer;

        public MainViewModel() 
        {
            //每秒从PlcPollingService.DataQueue中取出数据并添加到RealTimeData集合中
            //定时器是WPF中常用的方式，这里使用DispatcherTimer
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            while (PlcPollingService.DataQueue.TryDequeue(out var data))
            {
                string displayStr = "";
                if (data.TryGetValue("Temperature",out var temp))
                {
                    displayStr += $"温度：{temp}℃";
                }
                if (data.TryGetValue("Pressure", out var pres))
                {
                    displayStr += $"压力: {pres}kPa  ";
                }
                // 加上时间戳
                displayStr = $"[{DateTime.Now:HH:mm:ss}] " + displayStr;
                RealTimeData.Add(displayStr);
            }
        }
    }
}
