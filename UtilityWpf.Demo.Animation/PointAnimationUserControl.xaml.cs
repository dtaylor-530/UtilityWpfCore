using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace UtilityWpf.DemoAnimation {
   /// <summary>
   /// Interaction logic for PointAnimationUserControl.xaml
   /// </summary>
   public partial class PointAnimationUserControl : UserControl {
      Random r = new Random();
      Path myPath = null; EllipseGeometry myEllipseGeometry = null; Geometry myEllipseGeometry2 = null;

      public PointAnimationUserControl() {
         InitializeComponent();
      }

      private void Button_Click(object sender, RoutedEventArgs e)
      {
         NewMethod();
         NewMethod1();
         NewMethod2();
      }

      private void NewMethod()
      {
         int amt = 100;

         var point = RandomNext(r,
            () => new Point(PointControl1.Point.X + amt, PointControl1.Point.Y + amt),
            () => new Point(PointControl1.Point.X + amt, PointControl1.Point.Y - amt),
            () => new Point(PointControl1.Point.X - amt, PointControl1.Point.Y + amt),
            () => new Point(PointControl1.Point.X - amt, PointControl1.Point.Y - amt)).Invoke();

         point = new Point(Math.Min(Math.Max(point.X, 0), Canvas1.Width),
            Math.Min(Math.Max(point.Y, 0), Canvas1.Height));

         PointControl1.Point = point;
      }     
      
      private void NewMethod1()
      {
         int amt = 50;

         var point = RandomNext(r,
            () => new Point(TravellerControl1.LastPoint.X + amt, TravellerControl1.LastPoint.Y + amt),
            () => new Point(TravellerControl1.LastPoint.X + amt, TravellerControl1.LastPoint.Y - amt),
            () => new Point(TravellerControl1.LastPoint.X - amt, TravellerControl1.LastPoint.Y + amt),
            () => new Point(TravellerControl1.LastPoint.X - amt, TravellerControl1.LastPoint.Y - amt)).Invoke();

         point = new Point(Math.Min(Math.Max(point.X, 0), Canvas1.Width),
            Math.Min(Math.Max(point.Y, 0), Canvas1.Height));

         TravellerControl1.Point = point;
      }

      private void NewMethod2() {
         int amt = 50;

         var point = RandomNext(r,
            () => new Point(TravellerControl2.LastPoint.X + amt, TravellerControl2.LastPoint.Y + amt),
            () => new Point(TravellerControl2.LastPoint.X + amt, TravellerControl2.LastPoint.Y - amt),
            () => new Point(TravellerControl2.LastPoint.X - amt, TravellerControl2.LastPoint.Y + amt),
            () => new Point(TravellerControl2.LastPoint.X - amt, TravellerControl2.LastPoint.Y - amt)).Invoke();

         point = new Point(Math.Min(Math.Max(point.X, 0), Canvas1.Width),
            Math.Min(Math.Max(point.Y, 0), Canvas1.Height));

         TravellerControl2.Point = point;
      }



      static Func<T> RandomNext<T>(Random random, params Func<T>[] actions) {
         return actions[random.Next(0, actions.Length)];
      }


   }

}
