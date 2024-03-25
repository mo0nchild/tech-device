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
        public bool IsGereratorOn { get; private set; } = default!;
        public bool IsPowering { get; private set; } = default!;
        public double MaxWattValue { get; set; } = 150;

        private double _voltageLimit = 0, _amperageLimit = 0, _currentWatt = 0; 
        public MainWindow(ICalculatorWatt calculator, ISocketClient<WattModel> socketClient): base()
        {
            this.InitializeComponent();
            (this.calculatorWatt, this.socketClient) = (calculator, socketClient);

            this.socketClient.DataReceived += this.SocketDataReceived;
            this.socketClient.Connected += this.SocketConnected;
            this.socketClient.Disconnected += this.SocketDisconnected;
        }
        private void SocketConnected(object? sender, EventArgs args) => this.Dispatcher.Invoke(() =>
        {
            this.connectionButton.Background = Brushes.GreenYellow;
            this.powerButton.IsEnabled = this.IsGereratorOn = true;
        });
        private void SocketDisconnected(object? sender, EventArgs args) => this.Dispatcher.Invoke(() =>
        {
            this.connectionButton.Background = Brushes.White;
            this.SetDevicePower(this.powerButton.IsEnabled = this.IsGereratorOn = false);
            MessageBox.Show(!this.IsGereratorOn 
                ? "Генератор был отключен" : "Не удалось подключить генератор");
        });
        private async void SocketDataReceived(object? sender, WattModel args)
        {
            if (!this.IsPowering) return;
            this.SetLabelState(this._amperageLimit >= args.Amperage && this._voltageLimit >= args.Voltage);
            this.SetWattmeterValue(this._currentWatt = await this.calculatorWatt.CalculateWatt(args));
        }
        protected virtual void SetWattmeterValue(double watt) => this.Dispatcher.Invoke(() =>
        {
            var converter = this.MaxWattValue / this._voltageLimit / this._amperageLimit;
            this.wattmeterGauge.WattValue = watt * converter;
        });
        private void SetLabelState(bool isNormal) => this.Dispatcher.Invoke(() =>
        {
            this.stateTextBlock.Text = isNormal ? "С устройством все хорошо"
                : "Случилась перегрузка";
            this.stateTextBlock.Foreground = isNormal ? Brushes.YellowGreen : Brushes.Crimson;
        });
        private async void ConnectionButton_Click(object sender, RoutedEventArgs args)
        {
            if(this.socketClient.IsConnected)
            {
                await this.socketClient.DisconnectAsync();
                return;
            }
            var connectionWindow = new ConnectionWindow();
            connectionWindow.ShowDialog();

            var connectionUri = connectionWindow.ConnectionUrl;
            if (connectionUri != null && connectionUri.Length > 0)
            {
                try { await this.socketClient.ConnectAsync(new Uri($"ws://{connectionUri}")); }
                catch (Exception errorInfo) 
                {
                    MessageBox.Show($"Ошибка: {errorInfo.Message}");
                }
            }
        }
        private void SetDevicePower(bool powering)
        {
            this.powerButton.Background = !powering ? Brushes.White : Brushes.GreenYellow;
            this.powerButton.Content = !powering ? "Включить" : "Отключить";

            this.IsPowering = powering;
            if (!powering) this.wattmeterGauge.WattValue = 0;
        }
        private void PowerButton_Click(object sender, RoutedEventArgs args)
        {
            this.SetDevicePower(!this.IsPowering);
            if (this.IsPowering) this.socketClient.SendMessage<string>("alive");
            else this.SetLabelState(true);
        }
        private void RadioButtonVoltage_Checked(object sender, RoutedEventArgs args)
        {
            if (sender is RadioButton radioButton)
            {
                var content = radioButton.Content.ToString()!.Replace('.', ',');
                this._voltageLimit = double.Parse(content.Split(' ')[0]);
            }
            if (!this.IsPowering) return;
            this.SetWattmeterValue(this._currentWatt);
        }
        private void RadioButtonAmperage_Checked(object sender, RoutedEventArgs args)
        {
            if (sender is RadioButton radioButton)
            {
                var content = radioButton.Content.ToString()!.Replace('.', ',');
                this._amperageLimit = double.Parse(content.Split(' ')[0]);
            }
            if (!this.IsPowering) return;
            this.SetWattmeterValue(this._currentWatt);
        }
    }
}