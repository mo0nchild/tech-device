using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using TechDevice.Wattmeter.Services.Calculator.Models;
using Websocket.Client;

namespace TechDevice.Wattmeter.Services.Socket
{
    public class SocketClient<TData> : ISocketClient<TData>
    {
        protected CancellationTokenSource tokenSourse = new();
        public bool IsConnected { get; private set; } = default!;
        public string SocketUri { get; private set; } = default!;

        private WebsocketClient? socketClient = default!;
        public SocketClient() : base()
        {
            this.Disconnected += (@object, args) => this.IsConnected = false;
            this.Connected += (@object, args) => this.IsConnected = true;
        }
        public event EventHandler<TData> DataReceived = delegate { };

        public event EventHandler Connected = delegate { };
        public event EventHandler Disconnected = delegate { };

        protected virtual void DisconnectionHandler(DisconnectionInfo info)
        {
            if (this.IsConnected) this.tokenSourse.Cancel();
        }
        protected virtual void MessageHandler(ResponseMessage message)
        {
            var socketData = JsonConvert.DeserializeObject<TData>(message.Text!);
            if (socketData != null) this.DataReceived.Invoke(this, socketData);
        }
        public virtual Task ConnectAsync(Uri socketUri)
        {
            this.tokenSourse = new CancellationTokenSource();
            var cancellationToken = this.tokenSourse.Token;
            Task.Run(new Action(async() =>
            {
                this.SocketUri = socketUri.AbsoluteUri;
                using (this.socketClient = new WebsocketClient(socketUri) { IsReconnectionEnabled = false })
                {
                    this.socketClient.DisconnectionHappened.Subscribe(this.DisconnectionHandler);
                    this.socketClient.MessageReceived.Subscribe(this.MessageHandler);

                    await this.socketClient.Start();
                    if(this.socketClient.IsRunning) this.Connected.Invoke(this, EventArgs.Empty);

                    while (!cancellationToken.IsCancellationRequested && this.socketClient.IsRunning)
                        continue;
                    this.Disconnected.Invoke(this, EventArgs.Empty);
                }
                this.socketClient = null;
                this.tokenSourse.Dispose();
            }));
            return Task.CompletedTask;
        }
        public virtual Task DisconnectAsync() => this.tokenSourse.CancelAsync();

        public virtual async Task SendMessage<TMessage>(TMessage message)
        {
            if (this.socketClient == null || !this.IsConnected) return;
            await Task.Run(() => this.socketClient.Send(JsonConvert.SerializeObject(message)));
        }
    }
}
