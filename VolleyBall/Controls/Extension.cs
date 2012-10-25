using System.Windows;
using System.Windows.Controls;

namespace VolleyBall.Controls
{
    public static class Extension
    {
        public static double Left(this FrameworkElement element)
        {
            return (double)element.GetValue(Canvas.LeftProperty);
        }
        public static double Top(this FrameworkElement element)
        {
            return (double)element.GetValue(Canvas.TopProperty);
        }
        public static double Right(this FrameworkElement element)
        {
            return (double)element.GetValue(Canvas.LeftProperty) + element.RenderSize.Width;
        }
        public static double Bottom(this FrameworkElement element)
        {
            return (double)element.GetValue(Canvas.TopProperty) + element.RenderSize.Height;
        }

        public static double RealWidth(this FrameworkElement element)
        {
            return element.RenderSize.Width;
        }
        public static double RealHeight(this FrameworkElement element)
        {
            return element.RenderSize.Height;
        }

        public static void SetLeft(this FrameworkElement element, double value)
        {
            element.SetValue(Canvas.LeftProperty, value);
        }
        public static void SetTop(this FrameworkElement element, double value)
        {
            element.SetValue(Canvas.TopProperty, value);
        }
    }
}