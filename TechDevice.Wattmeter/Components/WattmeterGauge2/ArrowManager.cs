using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace TechDevice.Wattmeter.Components.WattmeterGauge2
{
    public class ArrowManager : object
    {
        public static readonly int MaxValue = 150;
        private RotateTransform angle { get; set; } = default!;

        private double _value = default!;
        public ArrowManager(RotateTransform angle) => this.angle = angle;
        public void setArrowRotateTransform(RotateTransform arrow) => this.angle = arrow;
        public double Value {  
            get { return this._value; }
            set {
                if (this._value > 150) this._value = 150;
                this.RotateToAnim(value);
                this._value = value;
            } 
        }
        private void RotateToAnim(double val, double multiply = 1.0)
        {
            var new_val = val * multiply;
            if(new_val > 150) new_val = 150;
            var rotateAnim = new DoubleAnimation();
            var fix_ = 1 + new_val / 1000.0f;
            var fix_2 = 0;
            if(new_val > 35 && new_val < 101)
            {
                fix_2 += 1;
            }
            if (new_val > 100)
            { 
                fix_ *= 0.9f; 
                fix_2 += 7;
                if (new_val > 100 && new_val < 140) fix_2 += 5;
                else fix_2 += 3;
            }
            rotateAnim.From = (((this._value + fix_2 - 1) * fix_) - 80);
            rotateAnim.To = (((new_val + fix_2 - 1) * fix_) - 80);
            rotateAnim.Duration = TimeSpan.FromSeconds(Math.Abs(this._value - new_val) / 120.0);

            this.angle.BeginAnimation(RotateTransform.AngleProperty, rotateAnim);
        }
    }
}
