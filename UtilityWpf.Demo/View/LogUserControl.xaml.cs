using System.Linq;
using ReactiveUI;
using Splat;
using System.Windows.Controls;
using ArxOne.MrAdvice.Advice;
using UtilityWpf.Command;

namespace UtilityWpf.DemoApp
{
    /// <summary>
    /// Interaction logic for LogUserControl.xaml
    /// </summary>
    public partial class LogUserControl : UserControl
    {
        public LogUserControl()
        {
            InitializeComponent();
        }
    }

    internal class LogViewModel : ReactiveObject, IEnableLogger
    {
        private string species;
        private string location;

        public LogViewModel()
        {
            this.LogCommand = new Command.RelayCommand(() =>
            {
                if (string.IsNullOrEmpty(this.Species))
                {
                    this
                        .Log()
                        .Error("No species was specified!");
                }
                this
                    .Log()
                    .Info("Species {0} was discovered at {1}.", this.Species, "Home");
            });

            this.RunAspectMethodCommand = new RelayCommand(() => AddLengths(species, location));
        }

        public Command.RelayCommand LogCommand { get; }

        public RelayCommand RunAspectMethodCommand { get; }

        public string Species
        {
            get => this.species;
            set => this.RaiseAndSetIfChanged(ref this.species, value);
        }

        public string Location
        {
            get => this.location;
            set => this.RaiseAndSetIfChanged(ref this.location, value);
        }

        [LogAdvice]
        public int AddLengths(string a, string b)
        {
            if (a == null || b == null)
                this.Log().Error("args are null");
            return a?.Length ?? 0 + b?.Length ?? 0;

        }

        public class LogAdvice : System.Attribute, IMethodAdvice, IEnableLogger
        {
            public void Advise(MethodAdviceContext context)
            {
                // do things you want here
                this.Log().Info("Method Name="+context.TargetName + 
                                      "| Arguments= " + string.Join(", ", context.Arguments.Select(a => a?.ToString() ?? "null")));

                context.Proceed(); // this calls the original method
                // do other things here
                this.Log().Info("Return Value= " + context.ReturnValue);
            }
        }
    }
}