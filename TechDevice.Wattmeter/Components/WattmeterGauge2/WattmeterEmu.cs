using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.IO.Pipes;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;

namespace TechDevice.Wattmeter.Components.WattmeterGauge2
{
    public class WattmeterEmu: object
    {
        private ArrowManager arrow { get; set; } = default!;
        public bool Repeat { get; set; } = true;

        private bool isConnected = default!;
        public WattmeterEmu(RotateTransform angle) => arrow = new ArrowManager(angle);
        public WattmeterEmu(Canvas canvas, string component_img, string arrow_img)
        {
            this.CreateMeter(canvas, component_img, arrow_img);
        }
        public List<UIElement> CreateMeter(Canvas canvas, string component_img, string arrow_img)
        {
            const double base_height = 320.0;
            var mult = canvas.Height / base_height;
            var elements = new List<UIElement>();

            // Создаем изображение вольтметра
            var wattmeterImage = new Image();
            wattmeterImage.Source = new BitmapImage(new Uri(component_img, UriKind.Relative));
            wattmeterImage.Width = 453 * mult;
            wattmeterImage.Height = 327 * mult;
            Canvas.SetLeft(wattmeterImage, -10 * mult);
            wattmeterImage.HorizontalAlignment = HorizontalAlignment.Left;
            wattmeterImage.VerticalAlignment = VerticalAlignment.Center;
            elements.Add(wattmeterImage);

            // Создаем изображение стрелки
            var arrowImage = new Image();
            arrowImage.Source = new BitmapImage(new Uri(arrow_img, UriKind.Relative));
            arrowImage.Width = 48 * mult;
            arrowImage.Height = 380 * mult;
            Canvas.SetLeft(arrowImage, 190 * mult);
            Canvas.SetTop(arrowImage, 30 * mult);
            arrowImage.HorizontalAlignment = HorizontalAlignment.Center;
            arrowImage.VerticalAlignment = VerticalAlignment.Top;

            // Создаем и настраиваем RenderTransform для поворота стрелки
            var rotateTransform = new RotateTransform();
            rotateTransform.Angle = -81;
            rotateTransform.CenterX = arrowImage.Width / 2;
            rotateTransform.CenterY = arrowImage.Height / 2;
            arrowImage.RenderTransform = rotateTransform;

            elements.Add(arrowImage);
            // Добавляем созданные элементы на холст
            foreach (UIElement element in elements) canvas.Children.Add(element);

            this.arrow = new ArrowManager(rotateTransform);
            return elements;
        }
        public void SetWattValue(double watt) => this.arrow.Value = watt;
    }
}
