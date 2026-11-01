using IndustrialMonitor.Core.Interfaces;
using NModbus;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace IndustrialMonitor.Infrastructure.Services
{
    public class NModbusService : IPlcService,IDisposable
    {
        private readonly string _ip;
        private readonly int _port;
        private TcpClient? _tcpClient;
        private IModbusMaster? _master;

        public bool IsConnected => _tcpClient?.Connected ?? false;

        public NModbusService(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                //如果已连接，先断开连接
                DisConnect();
                _tcpClient = new TcpClient();
                //带3秒超时的连接
                var connectTask = _tcpClient.ConnectAsync(_ip, _port);
                if(await Task.WhenAny(connectTask, Task.Delay(3000)) == connectTask)
                {
                    var factory = new ModbusFactory();
                    _master = factory.CreateMaster(_tcpClient);
                    return true;
                }
                else
                {
                    //连接超时
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public void DisConnect()
        {
            _master = null;
            _tcpClient?.Close();
            _tcpClient?.Dispose();
            _tcpClient = null;
        }

        public Task<ushort[]> ReadAsync(byte slaveId, ushort startAddress, ushort count)
        {
            if (_master == null || !IsConnected) throw new InvalidOperationException("PLC尚未连接");
            return _master.ReadHoldingRegistersAsync(slaveId, startAddress, count);
        }

        public void Dispose()=>DisConnect();

    }
}
