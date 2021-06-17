using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace UtilityWpf.Animation {

   public class PointControl : ContentControl {

      private EllipseGeometry myEllipseGeometry;
      public static readonly DependencyProperty PointProperty = DependencyProperty.Register("Point", typeof(Point), typeof(PointControl), new PropertyMetadata(default(Point), Changed));
      public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(PointControl), new PropertyMetadata(TimeSpan.FromSeconds(2)));
      public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register("RadiusY", typeof(double), typeof(PointControl), new PropertyMetadata(5d));
      public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register("RadiusX", typeof(double), typeof(PointControl), new PropertyMetadata(5d));



      static PointControl() {
      }

      public PointControl() { 
         myEllipseGeometry = new EllipseGeometry { RadiusX = RadiusX, RadiusY = RadiusY };
      }

      public override void OnApplyTemplate()
      {
         myEllipseGeometry.RadiusX = RadiusX;
         myEllipseGeometry.RadiusY = RadiusY;
         var myPath = new Path { Fill = Foreground, Data = myEllipseGeometry };
         this.Content = myPath;
      }

      public Point Point {
         get => (Point)GetValue(PointProperty);
         set => SetValue(PointProperty, value);
      }

      public TimeSpan Duration {
         get => (TimeSpan)GetValue(DurationProperty);
         set => SetValue(DurationProperty, value);
      }

      public double RadiusX {
         get => (double)GetValue(RadiusXProperty);
         set => SetValue(RadiusXProperty, value);
      }

      public double RadiusY {
         get => (double)GetValue(RadiusYProperty);
         set => SetValue(RadiusYProperty, value);
      }

      private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e) {

         if (!(d is PointControl pointControl))
            return;

         PointAnimation myPointAnimation = new PointAnimation {
            Duration = pointControl.Duration,
            From = (Point)e.OldValue,
            To = (Point)e.NewValue
         };

         pointControl.myEllipseGeometry.BeginAnimation(EllipseGeometry.CenterProperty, myPointAnimation);
      }
   }
}

