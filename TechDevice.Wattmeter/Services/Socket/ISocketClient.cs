using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDevice.Wattmeter.Services.Calculator.Models;

namespace TechDevice.Wattmeter.Services.Socket
{
    public interface ISocketClient<TData>
    {
        public event EventHandler<TData> DataReceived;
        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public string SocketUri { get; }
        public bool IsConnected { get; }

        public Task ConnectAsync(Uri socketUri);
        public Task DisconnectAsync();
        public Task SendMessage<TMessage>(TMessage message);
    }
}
