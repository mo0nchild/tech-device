using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechDevice.Wattmeter.Services.Calculator;
using TechDevice.Wattmeter.Services.Calculator.Models;
using TechDevice.Wattmeter.Services.Socket;

namespace TechDevice.Wattmeter.Services
{
    public static class Bootstrapper : object
    {
        public static IServiceCollection AddServices(this IServiceCollection collection)
        {
            return collection
                .AddTransient<ISocketClient<WattModel>, SocketClient<WattModel>>()
                .AddTransient<ICalculatorWatt, CalculatorWatt>();
        }
    }
}
