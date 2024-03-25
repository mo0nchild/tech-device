using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDevice.Wattmeter.Services.Calculator.Models;

namespace TechDevice.Wattmeter.Services.Calculator
{
    public interface ICalculatorWatt
    {
        public Task<double> CalculateWatt(WattModel model);
    }
}
