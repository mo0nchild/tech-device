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
using Websocket.Client;

namespace TechDevice.Wattmeter.Views
{
    public partial class MainWindow : Window
    {
        protected readonly ICalculatorWatt calculatorWatt = default!;
        public MainWindow(ICalculatorWatt calculator): base()
        {
            this.InitializeComponent();
            this.calculatorWatt = calculator;
        }
        private void Button_Click(object sender, RoutedEventArgs args)
        {
            var connectionWindow = new ConnectionWindow();
            var exitEvent = new ManualResetEvent(false);
            
            connectionWindow.ShowDialog();
            if (connectionWindow.ConnectionUrl == null) return;
            
            Task.Run(async () =>
            {
                using (var client = new WebsocketClient(new Uri(@"ws://localhost:8000")))
                {
                    client.ReconnectTimeout = TimeSpan.FromSeconds(10);
                    client.MessageReceived.Subscribe(async msg =>
                    {
                        var wattData = JsonConvert.DeserializeObject<WattModel>(msg.Text!);
                        var wattValue = await this.calculatorWatt.CalculateWatt(wattData!);
                    });
                    client.DisconnectionHappened.Subscribe((info) =>
                    {
                        Console.WriteLine();
                    });
                    await client.Start();
                    exitEvent.WaitOne();
                }
            });
            
        }
    }
}