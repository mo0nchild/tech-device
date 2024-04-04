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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TechDevice.Wattmeter.Components.WattmeterGauge2
{
    public partial class WattmeterGauge2 : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(double), typeof(WattmeterGauge2),
            new PropertyMetadata(0.0, static (@object, value) =>
            {
                if (@object is WattmeterGauge2 wattmeter) 
                {
                    wattmeter.wattmeterEmu.SetWattValue((double)value.NewValue);
                }
            }));
        public double Value 
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        private WattmeterEmu wattmeterEmu = default!;
        public WattmeterGauge2() : base()
        {
            this.InitializeComponent();
            this.wattmeterEmu = new WattmeterEmu(this.canvas,
                arrow_img: "Images/arrow.png",
                component_img: "Images/wattmeter.jpg");

            this.wattmeterEmu.SetWattValue(this.Value);
        }
    }
}
