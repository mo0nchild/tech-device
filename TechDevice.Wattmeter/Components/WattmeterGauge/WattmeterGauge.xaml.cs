using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TechDevice.Wattmeter.Components.WattmeterGauge
{
    /// <summary>
    /// Логика взаимодействия для WattmeterGauge.xaml
    /// </summary>
    public partial class WattmeterGauge : UserControl
    {
        public static readonly DependencyProperty WattValueProperty = DependencyProperty.Register(
            nameof(WattValue), typeof(double), typeof(WattmeterGauge), 
            new PropertyMetadata(default(double), static (@object, args) =>
            {
                if(@object is WattmeterGauge component) { }
            }));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            nameof(MaxValue), typeof(double), typeof(WattmeterGauge),
            new PropertyMetadata(100.0, static (@object, args) =>
            {
                if (@object is WattmeterGauge component) { }
            }));
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            nameof(Angle), typeof(double), typeof(WattmeterGauge),
            new PropertyMetadata(180.0, static (@object, args) =>
            {
                if (@object is WattmeterGauge component) { }
            }));
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            nameof(Radius), typeof(double), typeof(WattmeterGauge),
            new PropertyMetadata(50.0, static (@object, args) =>
            {
                if (@object is WattmeterGauge component) { }
            }));
        public double WattValue
        {
            get { return (double)this.GetValue(WattValueProperty); }
            set { this.SetValue(WattValueProperty, value); }
        }
        public double MaxValue
        {
            get { return (double)this.GetValue(MaxValueProperty); }
            set { this.SetValue(MaxValueProperty, value); }
        }
        public double Angle
        {
            get { return (double)this.GetValue(AngleProperty); }
            set { this.SetValue(AngleProperty, value); }
        }
        public double Radius
        {
            get { return (double)this.GetValue(RadiusProperty); }
            set { this.SetValue(RadiusProperty, value); }
        }
        public static readonly TimeSpan RenderInterval = TimeSpan.FromMilliseconds(10);
        protected Image? ComponentBackground { get; private set; } = default!;
        protected virtual Point Center 
        {
            get {
                int width = (int)this.myCanvas.Width, height = (int)this.myCanvas.Height;
                return new Point(width / 2, height / 6 * 5);
            }
        }
        public Pen GaugeColor { get => new Pen(new SolidColorBrush(Colors.Black), 2); }
        public Pen ArrowColor { get => new Pen(new SolidColorBrush(Colors.Black), 2); }
        public WattmeterGauge() : base()
        {
            this.InitializeComponent();
            this.Loaded += (@object, args) => this.SetComponent();

            var renderTimer = new DispatcherTimer() { Interval = RenderInterval };
            renderTimer.Tick += (@object, args) => this.UpdateComponent();
            renderTimer.Start();
        }
        protected virtual void UpdateComponent()
        {
            int width = (int)this.myCanvas.Width, height = (int)this.myCanvas.Height;
            this.myCanvas.Children.Clear();

            this.myCanvas.Children.Add(this.ComponentBackground);
            this.myCanvas.Children.Add(this.RenderComponent(context =>
            {
                var convertToAngle = this.WattValue * (this.Angle / this.MaxValue);
                var arrowSize = this.Radius - 80.0;
                var currentPoint = new Point(
                    x: -(Math.Cos(Math.PI / 180.0 * (convertToAngle - this.Angle / 2 + 90)) * arrowSize) + this.Center.X,
                    y: -(Math.Sin(Math.PI / 180.0 * (convertToAngle - this.Angle / 2 + 90)) * arrowSize) + this.Center.Y
                );
                context.DrawLine(this.ArrowColor, this.Center, currentPoint);
            }));
        }
        protected virtual void SetComponent() => this.ComponentBackground
            = this.RenderComponent(ctx => this.DrawComponent(ctx));
        protected Image RenderComponent(Action<DrawingContext> renderer)
        {
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen()) renderer(drawingContext);
            
            var renderBitmap = new RenderTargetBitmap(
                (int)this.myCanvas.Width, (int)this.myCanvas.Height,
                dpiX: 0, dpiY: 0, pixelFormat: PixelFormats.Pbgra32);

            renderBitmap.Render(drawingVisual);
            return new Image() { Source = renderBitmap };
        }
        protected void DrawComponent(DrawingContext context)
        {
            int width = (int)this.myCanvas.Width, height = (int)this.myCanvas.Height;

#pragma warning disable CS0618 // Тип или член устарел
            var formatText = (string text) => new FormattedText($"{text}",
              CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
              new Typeface("Verdana"), 12, new SolidColorBrush(Colors.Black));
#pragma warning restore CS0618 // Тип или член устарел

            var formatTick = (double angle, double size, double radius) => new Point(
                x: -(Math.Cos(Math.PI / 180.0 * (angle - this.Angle / 2 + 90)) * (radius - size)) + this.Center.X,
                y: -(Math.Sin(Math.PI / 180.0 * (angle - this.Angle / 2 + 90)) * (radius - size)) + this.Center.Y
            );
            context.DrawEllipse(new SolidColorBrush(Colors.Transparent), this.GaugeColor, this.Center, 10, 10);
            for(var index = 0; index <= this.MaxValue; index++)
            {
                var angle = index * (this.Angle / this.MaxValue);
                if (index % 10 == 0)
                {
                    var textPoint = new Point(
                        x: -(Math.Cos(Math.PI / 180.0 * (angle - this.Angle / 2 + 90)) * (this.Radius - 50)) + this.Center.X - 6,
                        y: -(Math.Sin(Math.PI / 180.0 * (angle - this.Angle / 2 + 90)) * (this.Radius - 50)) + this.Center.Y - 6
                    );
                    context.DrawText(formatText($"{index}"), textPoint);
                }
                var currentPoint = new Point(
                    x: -(Math.Cos(Math.PI / 180.0 * (angle - this.Angle / 2 + 90)) * (this.Radius - 100)) + this.Center.X,
                    y: -(Math.Sin(Math.PI / 180.0 * (angle - this.Angle / 2 + 90)) * (this.Radius - 100)) + this.Center.Y
                );
                var tickSize = index % 10 == 0 ? 10 : (index % 5 == 0 ? 5 : 2);
                context.DrawLine(this.GaugeColor, formatTick(angle, 20, this.Radius - 40), 
                    formatTick(angle, tickSize, this.Radius - 60));
            }
        }
    }
}
