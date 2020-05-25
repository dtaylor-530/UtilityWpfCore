﻿# nullable enable
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using static LambdaConverters.ValueConverter;
using static LambdaConverters.TemplateSelector;


namespace UtilityWpf.View
{
    /// <summary>
    /// Interaction logic for Json.xaml
    /// <a href="https://github.com/catsgotmytongue/JsonControls-WPF">JSON controls</a>
    /// </summary>
    public partial class JsonView : UserControl
    {
        private const GeneratorStatus Generated = GeneratorStatus.ContainersGenerated;

        public JsonView()
        {
            InitializeComponent();
        }

        public string Json
        {
            get { return (string)GetValue(JsonProperty); }
            set { SetValue(JsonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Json.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JsonProperty = DependencyProperty.Register("Json", typeof(string), typeof(JsonView), new PropertyMetadata(null, JsonChanged));

        public object Object
        {
            get { return (object)GetValue(ObjectProperty); }
            set { SetValue(ObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Object.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ObjectProperty = DependencyProperty.Register("Object", typeof(object), typeof(JsonView), new PropertyMetadata(null, ObjectChanged));

        private static void ObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is JsonView jsonView))
            {
                return;
            }

            jsonView.Load(Newtonsoft.Json.JsonConvert.SerializeObject(e.NewValue));
        }

        private static void JsonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is JsonView jsonView && e.NewValue is string json))
            {
                return;
            }

            jsonView.Load(json);
        }


        void Load(string json)
        {
            JsonTreeView.ItemsSource = null;
            JsonTreeView.Items.Clear();

            var children = new List<JToken>();

            try
            {
                var token = JToken.Parse(json);

                if (token != null)
                {
                    children.Add(token);
                }

                JsonTreeView.ItemsSource = children;
                JsonTreeView.ItemsSource = token.Children();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the JSON string:\r\n" + ex.Message);
            }
        }

        private void JValue_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock tb && e.ClickCount == 2)
            {
                Clipboard.SetText(tb.Text);
            }
        }

        private void ExpandAll(object sender, RoutedEventArgs e)
        {
            ToggleItems(true);
        }

        private void CollapseAll(object sender, RoutedEventArgs e)
        {
            ToggleItems(false);
        }

        private void ToggleItems(bool isExpanded)
        {
            if (JsonTreeView.Items.IsEmpty)
                return;

            var prevCursor = Cursor;
            //System.Windows.Controls.DockPanel.Opacity = 0.2;
            //System.Windows.Controls.DockPanel.IsEnabled = false;
            Cursor = Cursors.Wait;

            var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.Normal, (s,e)=>
            {
                ToggleItems(JsonTreeView, JsonTreeView.Items, isExpanded);
                //System.Windows.Controls.DockPanel.Opacity = 1.0;
                //System.Windows.Controls.DockPanel.IsEnabled = true;
                (s as DispatcherTimer)?.Stop();
                Cursor = prevCursor;
            }, Application.Current.Dispatcher);
            timer.Start();

            static void ToggleItems(ItemsControl parentContainer, ItemCollection items, bool isExpanded)
            {
                var itemGen = parentContainer.ItemContainerGenerator;
                if (itemGen.Status == Generated)
                {
                    Recurse(items, isExpanded, itemGen);
                }
                else
                {
                    itemGen.StatusChanged += delegate
                    {
                        Recurse(items, isExpanded, itemGen);
                    };
                }

                static void Recurse(ItemCollection items, bool isExpanded, ItemContainerGenerator itemGen)
                {
                    if (itemGen.Status != Generated)
                        return;

                    foreach (var item in items)
                    {
                        var tvi = itemGen.ContainerFromItem(item) as TreeViewItem;
                        tvi.IsExpanded = isExpanded;
                        ToggleItems(tvi, tvi.Items, isExpanded);
                    }
                }
            }
        }
    }


    internal static class TemplateSelector
    {
        public static DataTemplateSelector JPropertyDataTemplateSelector =
           Create<object>(
                e =>
                    (e.Item, e.Container) switch
                    {
                        (JProperty property, FrameworkElement frameworkElement) => property.Value.Type switch
                        {
                            JTokenType.Object => frameworkElement.FindResource("ObjectPropertyTemplate"),
                            JTokenType.Array => frameworkElement.FindResource("ArrayPropertyTemplate"),
                            _ => frameworkElement.FindResource("PrimitivePropertyTemplate"),
                        },
                        ({ } property, FrameworkElement frameworkElement) =>
                        frameworkElement.FindResource(new DataTemplateKey(e.Item.GetType())),
                        _ => null
                    } as DataTemplate
                );
    }

    internal static class ColorStore
    {
        public static readonly Dictionary<string, string> Collection = new Dictionary<string, string> {
            { "navy", "#001F3F"} ,
             { "blue", "#0074D9"} ,
              { "aqua", "#7FDBFF"} ,
                                  { "teal", "#39CCCC"} ,
                                  { "olive", "#3D9970"} ,
                                  { "green", "#2ECC40"} ,
                                  { "d", "#d59aea"} ,
                                  {  "yellow", "#FFDC00"} ,
                         { "black", "#111111"},
                                  { "red", "#FF4136"} ,
               { "fuchsia", "#F012BE"} ,

             { "purple", "#B10DC9"} ,

             { "maroon", "#85144B"} ,
                                  { "gray", "#AAAAAA"} ,
                                                 { "silver", "#DDDDDD"} ,
                                           {  "orange", "#FF851B"} ,
                                  { "a", "#ff035c"},
                                  { "b", "#9eb4cc"},
                                  { "c", "#fbead3"},
                                  };
    }
}
