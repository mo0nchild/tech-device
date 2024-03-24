using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDevice.Wattmeter.Services.Calculator;

namespace TechDevice.Wattmeter.Services
{
    public static class Bootstrapper : object
    {
        public static IServiceCollection AddCalculators(this IServiceCollection collection)
        {
            return collection.AddTransient<ICalculatorWatt, CalculatorWatt>();
        }
    }
}
