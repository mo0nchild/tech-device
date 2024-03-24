using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TechDevice.Wattmeter.Services.Calculator.Models;

namespace TechDevice.Wattmeter.Services.Calculator
{
    public class CalculatorWatt : ICalculatorWatt
    {
        public CalculatorWatt(): base() { }
        public Task<double> CalculateWatt(WattModel data) => 
            Task.FromResult(data.Voltage * data.Amperage);
    }
}
