using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDevice.Wattmeter.Services.Calculator.Models
{
    public class WattModel : EventArgs
    {
        public double Amperage { get; set; } = default!;
        public double Voltage { get; set; } = default!;
    }
}
