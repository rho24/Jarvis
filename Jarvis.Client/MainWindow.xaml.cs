using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Jarvis.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DirectoryInfo _dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

        public MainWindow() {
            InitializeComponent();
        }

        private void SelectResults() {
            var text = input.Text;

            Task.Factory.StartNew(() => _dir.EnumerateFiles(text + "*"))
                .ContinueWith(files => {
                    var results = files.Result.Select(f => new {f.Name}).ToArray();
                    Results.ItemsSource = results;
                },
                              TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Input_OnKeyUp(object sender, KeyEventArgs e) {
            SelectResults();
        }
    }
}