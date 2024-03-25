using Newtonsoft.Json;
using System;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechDevice.Wattmeter.Services.Calculator;
using TechDevice.Wattmeter.Services.Calculator.Models;
using TechDevice.Wattmeter.Services.Socket;
using Websocket.Client;

namespace TechDevice.Wattmeter.Views
{
    public partial class MainWindow : Window
    {
        protected readonly ISocketClient<WattModel> socketClient = default!;
        protected readonly ICalculatorWatt calculatorWatt = default!;

        public static readonly Color CustomBackground = (Color)ColorConverter.ConvertFromString("#1b1b1f");
        public bool IsGereratorOn { get; private set; } = default!;
        public MainWindow(ICalculatorWatt calculator, ISocketClient<WattModel> socketClient): base()
        {
            this.InitializeComponent();
            this.Background = new SolidColorBrush(CustomBackground);
            (this.calculatorWatt, this.socketClient) = (calculator, socketClient);

            this.socketClient.DataReceived += this.SocketDataReceived;
            this.socketClient.Connected += this.SocketConnected;
            this.socketClient.Disconnected += this.SocketDisconnected;
        }

        private void SocketConnected(object? sender, EventArgs args) => this.Dispatcher.Invoke(() =>
        {
            this.connectionButton.Background = Brushes.GreenYellow;
            this.IsGereratorOn = true;
        });
        private void SocketDisconnected(object? sender, EventArgs args) => this.Dispatcher.Invoke(() =>
        {
            this.connectionButton.Background = Brushes.White;

            MessageBox.Show(this.IsGereratorOn ? "Генератор был отключен" : 
                "Не удалось подключить генератор");
            this.IsGereratorOn = false;
        });
        private async void SocketDataReceived(object? sender, WattModel args)
        {
            await this.Dispatcher.Invoke(async () =>
            {
                this.wattmeterGauge.WattValue = await this.calculatorWatt.CalculateWatt(args);
            });
        }
        private async void Button_Click(object sender, RoutedEventArgs args)
        {
            if(this.socketClient.IsConnected)
            {
                await this.socketClient.DisconnectAsync();
                this.SocketDisconnected(this, EventArgs.Empty);
                return;
            }
            var connectionWindow = new ConnectionWindow();
            connectionWindow.ShowDialog();

            var connectionUri = connectionWindow.ConnectionUrl;
            if (connectionUri != null && connectionUri.Length > 0)
            {
                try { await this.socketClient.ConnectAsync(new Uri($"ws://{connectionUri}")); }
                catch (Exception errorInfo) { MessageBox.Show($"Ошибка: {errorInfo.Message}"); }
            }
        }
    }
}