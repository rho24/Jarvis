using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Caliburn.Micro;
using Jarvis.Core;

namespace Jarvis.Client
{
    public class ShellViewModel : PropertyChangedBase, IShell
    {
        private readonly IJarvisService _jarvis;
        private IEnumerable<IItem> _results;
        private int _resultsSelectedInput;
        private string _text;

        public string UserInput {
            get { return _text; }
            set {
                if (value == _text) return;
                _text = value;
                NotifyOfPropertyChange(() => UserInput);
            }
        }

        public IEnumerable<IItem> Results {
            get { return _results; }
            set {
                if (Equals(value, _results)) return;
                _results = value;
                NotifyOfPropertyChange(() => Results);
            }
        }

        public int ResultsSelectedInput {
            get { return _resultsSelectedInput; }
            set {
                if (value == _resultsSelectedInput) return;
                _resultsSelectedInput = value;
                NotifyOfPropertyChange(() => ResultsSelectedInput);
            }
        }

        public ShellViewModel(IJarvisService jarvis) {
            _jarvis = jarvis;

            Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                .Where(e => e.EventArgs.PropertyName == "UserInput")
                .Select(e => UserInput)
                .DistinctUntilChanged()
                .StartWith("")
                .ObserveOn(TaskPoolScheduler.Default)
                .Select(s => _jarvis.Items(s).Fetch())
                .Subscribe(r => Results = r);
        }

        public void UpInput() {
            ResultsSelectedInput--;
        }

        public void DownInput() {
            ResultsSelectedInput++;
        }

        public void EnterInput() {
            var selectedFile = Results.Skip(Math.Max(0, ResultsSelectedInput)).FirstOrDefault() as FileItem;
            if (selectedFile == null)
                return;

            var executer = new ProcessStarter(selectedFile.FullPath);
            executer.Execute();
        }
    }
}