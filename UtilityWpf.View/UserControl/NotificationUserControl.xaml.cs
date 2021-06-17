using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UtilityWpf.View
{
   /// <summary>
   /// Interaction logic for NotificationUserControl.xaml
   /// </summary>
   public partial class NotificationUserControl : UserControl
   {


      public string Message
      {
         get { return (string)GetValue(MessageProperty); }
         set { SetValue(MessageProperty, value); }
      }

      // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty MessageProperty =
          DependencyProperty.Register("Message", typeof(string), typeof(NotificationUserControl), new PropertyMetadata("Message", Changed));

      private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         (d as NotificationUserControl).MessageBox1.Text = (string)e.NewValue;//(d athrow new NotImplementedException();))
      }


      public NotificationUserControl()
      {
         InitializeComponent();

      }

      private void ButtonBase_OnClickConfirm(object sender, RoutedEventArgs e)
      {
         this.RaiseTapEvent(DialogResult.Ok );
      }

      private void ButtonBase_OnClickCancel(object sender, RoutedEventArgs e)
      {
         this.RaiseTapEvent(DialogResult.Cancel);

      }

      // Create a custom routed event by first registering a RoutedEventID
      // This event uses the bubbling routing strategy
      public static readonly RoutedEvent TapEvent = EventManager.RegisterRoutedEvent(
         "Tap", RoutingStrategy.Bubble, typeof(DialogRoutedEventHandler), typeof(NotificationUserControl));

      // Provide CLR accessors for the event
      public event DialogRoutedEventHandler Tap
      {
         add => AddHandler(TapEvent, value);
         remove => RemoveHandler(TapEvent, value);
      }

      // This method raises the Tap event
      void RaiseTapEvent(DialogResult dialogResult)
      {
         DialogResultEventArgs newEventArgs = new DialogResultEventArgs(NotificationUserControl.TapEvent, dialogResult);
         RaiseEvent(newEventArgs);
      }

      public class DialogResultEventArgs : RoutedEventArgs
      {
         public DialogResultEventArgs(RoutedEvent routedEvent, DialogResult dialogResult) : base(routedEvent)
         {
            DialogResult = dialogResult;
         }

         public DialogResult DialogResult { get; }


      }

      public enum DialogResult
      {
         None,
         Ok = 1,
         Cancel,
      }

      public delegate void DialogRoutedEventHandler(object sender, DialogResultEventArgs args);
   }



}
