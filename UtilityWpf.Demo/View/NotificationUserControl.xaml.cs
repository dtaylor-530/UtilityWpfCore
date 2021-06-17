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
using UtilityWpf.View;

namespace UtilityWpf.DemoAppCore.View
{
   /// <summary>
   /// Interaction logic for DialogUserControl.xaml
   /// </summary>
   public partial class NotificationUserControl : UserControl
   {
      public NotificationUserControl()
      {
         InitializeComponent();
      }

      private void NotificationUserControl_OnTap(object sender, UtilityWpf.View.NotificationUserControl.DialogResultEventArgs args)
      {
         MessageBox.Show(args.DialogResult.ToString());
      }
   }
}
