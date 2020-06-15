﻿using AutoBogus;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UtilityWpf.DemoApp
{
    /// <summary>
    /// Interaction logic for SliderItemsUserControl.xaml
    /// </summary>
    public partial class SliderItemsUserControl : UserControl
    {
        public SliderItemsUserControl()
        {
            InitializeComponent();
            Init();
        }

        private async void Init()
        {
            await Task.Delay(20000).ContinueWith(_ =>
            {
                // Configure globally
                //AutoFaker.Configure(builder =>
                //{
                //    builder
                //      .WithLocale()         // Configures the locale to use
                //      .WithRepeatCount()    // Configures the number of items in a collection
                //      .WithRecursiveDepth() // Configures how deep nested types should recurse
                //      .WithBinder()         // Configures the binder to use
                //      .WithSkip()           // Configures members to be skipped for a type
                //      .WithOverride();      // Configures the generator overrides to use - can be called multiple times
                //});
                var personFaker = new AutoFaker<Character>()
  .RuleFor(fake => fake.First, fake => fake.Random.Word())
    .RuleFor(fake => fake.Last, fake => fake.Random.Word())
        .RuleFor(fake => fake.Location, fake => new Point(fake.Random.Int(), fake.Random.Int()))
                   .RuleFor(fake => fake.Age, fake => fake.Random.Int(0, 10000));
                this.Dispatcher.InvokeAsync(() => sic.Data = personFaker.Generate(10));
            });
        }

        private bool b;

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            b = !b;
            sic.ShowKeyValuePanel = b;
        }
    }

    public class ValueChangedCommand : UtilityWpf.View.ValueChangedCommand, INotifyPropertyChanged
    {
        private KeyValuePair<string, double> keyValuePair;

        public string Key => keyValuePair.Key;
        public double Value => keyValuePair.Value;

        public ValueChangedCommand()
        {
            base.Event += ValueChangedCommand_Event;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ValueChangedCommand_Event(KeyValuePair<string, double> obj)
        {
            this.keyValuePair = obj;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Key)));
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}