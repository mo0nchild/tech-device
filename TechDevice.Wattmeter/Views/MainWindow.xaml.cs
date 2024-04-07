using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        protected readonly ISocketClient<WattModel> socketClient = default!;
        protected readonly ICalculatorWatt calculatorWatt = default!;
        public bool IsGereratorOn { get; private set; } = default!;

        private bool _isPowering = default!;
        public bool IsPowering { get => this._isPowering && this._connectionWires == Brushes.YellowGreen; }
        public double MaxWattValue { get; set; } = 150;
        
        private Brush _connectionWires = Brushes.Gray, _connection = Brushes.Transparent;
        public Brush ConnectionWires
        {
            set { this._connectionWires = value; this.OnPropertyChanged(); }
            get { return this._connectionWires; }
        }
        public Brush Connection
        {
            set { this._connection = value; this.OnPropertyChanged(); }
            get { return this._connection; }
        }
        private double _voltageLimit = 30, _amperageLimit = 0.5, _currentWatt = 0;
        private bool _isLoaded = default!;
        private MenuItem? connectionButtonRef = default;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public MainWindow(ICalculatorWatt calculator, ISocketClient<WattModel> socketClient): base()
        {
            this.InitializeComponent();
            (this.calculatorWatt, this.socketClient) = (calculator, socketClient);
            this.Loaded += (sender, args) => this._isLoaded = true;

            this.socketClient.DataReceived += this.SocketDataReceived;
            this.socketClient.Connected += this.SocketConnected;
            this.socketClient.Disconnected += this.SocketDisconnected;
        }
        private void SocketConnected(object? sender, EventArgs args) => this.Dispatcher.Invoke(() =>
        {
            this.Connection = Brushes.YellowGreen;
            this.IsGereratorOn = true;
            if (this.connectionButtonRef != null) connectionButtonRef.IsChecked = true;
            if (this.IsPowering) this.socketClient.SendMessage<string>("alive");
        });
        private void SocketDisconnected(object? sender, EventArgs args) => this.Dispatcher.Invoke(() =>
        {
            MessageBox.Show(this.IsGereratorOn
                ? "Генератор был отключен" : "Не удалось подключить генератор");
            this.Connection = Brushes.Transparent;
            this.IsGereratorOn = false;
            this.wattmeterGauge.Value = default!;
            this.SetLabelState(true);
            if (this.connectionButtonRef != null) connectionButtonRef.IsChecked = false;
        });
        private async void SocketDataReceived(object? sender, WattModel args)
        {
            if (!this.IsPowering) return;
            this.SetWattmeterValue(this._currentWatt = await this.calculatorWatt.CalculateWatt(args));
        }
        protected virtual void SetWattmeterValue(double watt) => this.Dispatcher.Invoke(() =>
        {
            var converter = this.MaxWattValue / this._voltageLimit / this._amperageLimit;
            this.SetLabelState(this._amperageLimit * this._voltageLimit >= watt);
            this.wattmeterGauge.Value = watt * converter;
        });
        private void SetLabelState(bool isNormal) => this.Dispatcher.Invoke(() =>
        {
            this.stateBlock.Background = isNormal ? Brushes.YellowGreen : Brushes.Crimson;
        });
        private async void ConnectionButton_Click(object sender, RoutedEventArgs args)
        {
            this.connectionButtonRef = sender as MenuItem;
            if (this.socketClient.IsConnected)
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
        private void SetDevicePower(bool powering, Brush connection)
        {
            (this._isPowering, this.ConnectionWires) = (powering, connection);
            if (!this.IsPowering)
            {
                this.wattmeterGauge.Value = 0;
                this.SetLabelState(true);
            }
            else this.socketClient.SendMessage<string>("alive");
        }
        private void Power_Checked(object sender, RoutedEventArgs args)
        {
            this.SetDevicePower(!_isPowering, this._connectionWires);
        }
        private void ConnectWiresButton_Click(object sender, RoutedEventArgs args)
        {
            this.SetDevicePower(_isPowering, this.ConnectionWires == Brushes.YellowGreen ? Brushes.Gray : Brushes.YellowGreen);
            if (sender is MenuItem menuItem) menuItem.IsChecked = !menuItem.IsChecked;
        }
        private void RadioButtonVoltage_Checked(object sender, RoutedEventArgs args)
        {
            if (!this._isLoaded) return;
            if (sender is RadioButton radioButton)
            {
                var content = radioButton.Content.ToString()!.Replace('.', ',');
                this._voltageLimit = double.Parse(content.Split(' ')[0]);
            }
            if (!this.IsPowering || !this.IsGereratorOn) return;
            this.SetWattmeterValue(this._currentWatt);
        }
        private void RadioButtonAmperage_Checked(object sender, RoutedEventArgs args)
        {
            if (!this._isLoaded) return;
            if (sender is RadioButton radioButton)
            {
                var content = radioButton.Content.ToString()!.Replace('.', ',');
                this._amperageLimit = double.Parse(content.Split(' ')[0]);
            }
            if (!this.IsPowering || !this.IsGereratorOn) return;
            this.SetWattmeterValue(this._currentWatt);
        }
    }
}