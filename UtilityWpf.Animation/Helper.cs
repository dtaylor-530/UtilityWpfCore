using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using UtilityWpf.Animation;

namespace UtilityWpf.AnimationCore {
   public static class Helper {


      public static PathGeometry ToPathGeometry(this Point[] points)
      {
         return new PathGeometry {
            Figures = new PathFigureCollection
            {
               CreatePathFigure(points)
            }
         }; 
         
         static PathFigure CreatePathFigure(Point[] points) {
            PathFigure pathFigure = new PathFigure { StartPoint = points[0] };

            for (int i = 1; i < points.Length; i++) {
               pathFigure.Segments.Add(new LineSegment { Point = points[i] });
            }

            return pathFigure;
         }
      }

      public static void ProcessAnimationsQueue<T>(Queue<T> list, Animatable geometry, DependencyProperty property) where T : AnimationTimeline {
         if (list.Any() == false)
            return;

         var pointAnimation = list.Dequeue();
         pointAnimation.Completed += (s, e) => ProcessAnimationsQueue(list, geometry, property);
         geometry.BeginAnimation(property, pointAnimation);
      }

      public static PathGeometry ConvertToPathGeometry(params Point[] points) {
         return ToPathGeometry(points);
      }

        public static IObservable<EventArgs> SelectCompletions(this Storyboard storyboard) =>

     Observable
     .FromEventPattern<EventHandler, EventArgs>
     (a => storyboard.Completed += a, a => storyboard.Completed -= a)
     .Select(a => a.EventArgs);

        public static ReplaySubject<T> ToReplaySubject<T>(this IObservable<T> source, int save = 1)
        {
            var replaySubject = new ReplaySubject<T>(save);
            source.Subscribe(replaySubject);
            return replaySubject;
        }
    }

}
