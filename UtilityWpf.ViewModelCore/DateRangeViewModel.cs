﻿
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using UtilityModel;

namespace UtilityWpf.ViewModel
{
    public class DataRangeViewModel : ReactiveObject
    {
        DateTime from = new DateTime(2018, 6, 14);
        DateTime to = new DateTime(2018, 7, 15);
        private readonly ObservableAsPropertyHelper<Range<DateTime>> output;

        public DateTime From
        {
            get => from;
            set => this.RaiseAndSetIfChanged(ref from, value);
        }

        public DateTime To
        {
            get => to;
            set => this.RaiseAndSetIfChanged(ref to, value);
        }


        public UtilityModel.Range<DateTime> Output => output.Value;

        public DataRangeViewModel()
        {
            output = this.WhenAnyValue(a => a.From).CombineLatest(this.WhenAnyValue(a => a.To), (a, b) => new { a, b })
                 .Where(g => g.a < g.b)
                 .Select(_ => new UtilityModel.Range<DateTime> { Minimum = _.a, Maximum = _.b })
                 .Throttle(TimeSpan.FromSeconds(1))
                 .ToProperty(this, a => a.Output, deferSubscription: true);
        }
    }
}