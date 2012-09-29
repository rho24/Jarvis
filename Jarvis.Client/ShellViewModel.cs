using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Jarvis.Core;

namespace Jarvis.Client
{
    public class ShellViewModel : PropertyChangedBase, IShell
    {
        private readonly DirectoryInfo _dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        private readonly IJarvisService _jarvis;
        private string _previousText;
        private IEnumerable<string> _results;
        private string _text;

        public string Text {
            get { return _text; }
            set {
                if (value == _text) return;
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public IEnumerable<string> Results {
            get { return _results; }
            set {
                if (Equals(value, _results)) return;
                _results = value;
                NotifyOfPropertyChange(() => Results);
            }
        }

        public ShellViewModel(IJarvisService jarvis) {
            _jarvis = jarvis;
            SelectResults("");
        }

        public void ValueChanged() {
            var text = Text;
            if (text == _previousText)
                return;
            _previousText = text;
            SelectResults(text);
        }

        private void SelectResults(string text) {
            Task.Factory.StartNew(() => _dir.EnumerateFiles(text + "*"))
                .ContinueWith(files => { Results = files.Result.Select(f => f.Name); },
                              TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}