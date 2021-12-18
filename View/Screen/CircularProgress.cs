using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WwtbamOld.View;

public sealed class CircularProgress : Shape
{
    static CircularProgress()
    {
        Brush brush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 6, 176, 37));
        brush.Freeze();
        StrokeProperty.OverrideMetadata(typeof(CircularProgress), new FrameworkPropertyMetadata(brush));
        FillProperty.OverrideMetadata(typeof(CircularProgress), new FrameworkPropertyMetadata(Brushes.Transparent));
        StrokeThicknessProperty.OverrideMetadata(typeof(CircularProgress), new FrameworkPropertyMetadata(10.0));
    }

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private static object CoerceValue(DependencyObject depObj, object baseVal)
    {
        return Math.Max(Math.Min((double)baseVal, 99.999), 0.0);
    }

    protected override Geometry DefiningGeometry
    {
        get
        {
            double num = 90.0;
            double num2 = 90.0 - Value / 100.0 * 360.0;
            double num3 = Math.Max(0.0, RenderSize.Width - StrokeThickness);
            double num4 = Math.Max(0.0, RenderSize.Height - StrokeThickness);
            double num5 = num3 / 2.0 * Math.Cos(num * 3.1415926535897931 / 180.0);
            double num6 = num4 / 2.0 * Math.Sin(num * 3.1415926535897931 / 180.0);
            double num7 = num3 / 2.0 * Math.Cos(num2 * 3.1415926535897931 / 180.0);
            double num8 = num4 / 2.0 * Math.Sin(num2 * 3.1415926535897931 / 180.0);
            StreamGeometry streamGeometry = new();
            using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
            {
                streamGeometryContext.BeginFigure(new Point(RenderSize.Width / 2.0 + num5, RenderSize.Height / 2.0 - num6), true, false);
                streamGeometryContext.ArcTo(new Point(RenderSize.Width / 2.0 + num7, RenderSize.Height / 2.0 - num8), new Size(num3 / 2.0, num4 / 2.0), 0.0, num - num2 > 180.0, SweepDirection.Clockwise, true, false);
            }
            return streamGeometry;
        }
    }

    private static readonly FrameworkPropertyMetadata valueMetadata = new(0.0, FrameworkPropertyMetadataOptions.AffectsRender, null, new CoerceValueCallback(CoerceValue));
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(CircularProgress), valueMetadata);
}
