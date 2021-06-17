using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using UtilityWpf.AnimationCore;

namespace UtilityWpf.Animation {

   public class TravellerControl : Canvas {

      private readonly EllipseGeometry particlePath;
      private Path ellipsePath;
      private readonly Path journeyPath;
      public static readonly DependencyProperty PointProperty = DependencyProperty.Register("Point", typeof(object), typeof(TravellerControl), new PropertyMetadata(default(Point), Changed));
      public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(TravellerControl), new PropertyMetadata(TimeSpan.FromSeconds(2)));
      public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register("RadiusY", typeof(double), typeof(TravellerControl), new PropertyMetadata(5d));
      public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register("RadiusX", typeof(double), typeof(TravellerControl), new PropertyMetadata(5d));
      public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(TravellerControl), new PropertyMetadata(Brushes.Black));
      public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(TravellerControl), new PropertyMetadata(Brushes.White));

      public TravellerControl() {

         journeyPath = new Path {
            Stroke = Stroke,
            StrokeThickness = Math.Max(RadiusX, RadiusY) + 1,
            Data = new[] { new Point(0, 0), new Point(00, 00) }
               .ToArray().ToPathGeometry()
         };

         this.Children.Add(journeyPath);

         particlePath = new EllipseGeometry { RadiusX = RadiusX, RadiusY = RadiusY };
         ellipsePath = new Path { Fill = Foreground, Data = particlePath };

         this.Children.Add(ellipsePath);
      }

      public Point LastPoint => Point switch {

         Point[] points => points.LastOrDefault(),
         Point point => point,
         _ => throw new Exception("sdf  dd")

      };


      public object Point {
         get => (object)GetValue(PointProperty);
         set => SetValue(PointProperty, value);
      }


      public Brush Foreground {
         get => (Brush)GetValue(ForegroundProperty);
         set => SetValue(ForegroundProperty, value);
      }

      public Brush Stroke {
         get => (Brush)GetValue(StrokeProperty);
         set => SetValue(StrokeProperty, value);
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

         if (!(d is TravellerControl control))
            return;

         SetChildProperties(control);
         GetPoints(e, out var oldPoint, out var points);
         control.journeyPath.Data = new[] { oldPoint }.Concat(points).ToArray().ToPathGeometry();

         Queue<PointAnimation> list = new Queue<PointAnimation>();
         foreach (var point in points) {
            var pointAnimation = new PointAnimation {
               Duration = control.Duration,
               From = oldPoint,
               To = point
            };
            oldPoint = point;
            list.Enqueue(pointAnimation);
         }

         Helper.ProcessAnimationsQueue(list, control.particlePath, EllipseGeometry.CenterProperty);

         static void GetPoints(DependencyPropertyChangedEventArgs e, out Point oldPoint, out Point[] points) {

            oldPoint = default;

            if (e.NewValue is Point[] pts) {
               points = pts;
            }
            else if (e.NewValue is Point pnt) {
               points = new[] { pnt };
            }
            else {
               throw new Exception("Points must be a Point or array of points");
            }

            if (e.OldValue is Point[] opts) {
               oldPoint = opts.LastOrDefault();
            }

            if (e.OldValue is Point opnt) {
               oldPoint = opnt;
            }
         }
      }

      private static void SetChildProperties(TravellerControl pointControl) {
         pointControl.particlePath.RadiusX = pointControl.RadiusX;
         pointControl.particlePath.RadiusY = pointControl.RadiusY;
         pointControl.ellipsePath.Fill = pointControl.Foreground;
         pointControl.journeyPath.Stroke = pointControl.Stroke;
         pointControl.journeyPath.StrokeThickness = Math.Max(pointControl.RadiusX, pointControl.RadiusY) + 1;
      }
   }
}

