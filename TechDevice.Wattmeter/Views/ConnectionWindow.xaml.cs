using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TechDevice.Wattmeter.Views
{
    public partial class ConnectionWindow : Window
    {
        public string? ConnectionUrl { get; private set; } = default!;
        public ConnectionWindow() : base() => this.InitializeComponent();
        private void connectionButton_Click(object sender, RoutedEventArgs args)
        {
            this.ConnectionUrl = this.urlTextBlock.Text;
            this.Close();
        }
    }
}
