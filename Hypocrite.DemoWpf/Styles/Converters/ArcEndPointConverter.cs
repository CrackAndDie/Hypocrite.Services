using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Hypocrite.DemoWpf.Styles.Converters
{
    public class ArcEndPointConverter : IMultiValueConverter
    {
        /// <summary>
        /// CircularProgressBar draws two arcs to support a full circle at 100 %.
        /// With one arc at 100 % the start point is identical the end point, so nothing is drawn.
        /// Midpoint at half of current percentage is the endpoint of the first arc
        /// and the start point of the second arc.
        /// </summary>
        public const string ParameterMidPoint = "MidPoint";

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var actualWidth = ExtractDouble(values[0]);
            var value = ExtractDouble(values[1]);
            var minimum = ExtractDouble(values[2]);
            var maximum = ExtractDouble(values[3]);

            if (new[] { actualWidth, value, minimum, maximum }.Any(double.IsNaN))
                return Binding.DoNothing;

            if (values.Length == 5)
            {
                var fullIndeterminateScaling = ExtractDouble(values[4]);
                if (!double.IsNaN(fullIndeterminateScaling) && fullIndeterminateScaling > 0.0)
                {
                    value = (maximum - minimum) * fullIndeterminateScaling;
                }
            }

            var percent = maximum <= minimum ? 1.0 : (value - minimum) / (maximum - minimum);
            if (Equals(parameter, ParameterMidPoint))
                percent /= 2;

            var degrees = 360 * percent;
            var radians = degrees * (Math.PI / 180);

            var centre = new Point(actualWidth / 2, actualWidth / 2);
            var hypotenuseRadius = (actualWidth / 2);

            var adjacent = Math.Cos(radians) * hypotenuseRadius;
            var opposite = Math.Sin(radians) * hypotenuseRadius;

            return new Point(centre.X + opposite, centre.Y - adjacent);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static double ExtractDouble(object value)
        {
            var d = value as double? ?? double.NaN;
            return double.IsInfinity(d) ? double.NaN : d;
        }
    }
}
