using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Caliburn.Micro;
using Jarvis.Core;

namespace Jarvis.Client
{
    public class LaunchViewModel : Screen
    {
        private readonly IJarvisService _jarvis;
        private readonly IEventAggregator _eventAggregator;
        private IEnumerable<IOption> _results;
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

        public IEnumerable<IOption> Results {
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

        public LaunchViewModel(IJarvisService jarvis, IEventAggregator eventAggregator) {
            _jarvis = jarvis;
            _eventAggregator = eventAggregator;

            InitialiseSearch();
        }

        private void InitialiseSearch() {
            Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                      .Where(e => e.EventArgs.PropertyName == "UserInput")
                      .Select(e => UserInput)
                      .DistinctUntilChanged()
                      .StartWith("")
                      .ObserveOn(TaskPoolScheduler.Default)
                      .Select(s => _jarvis.GetOptions(s))
                      .Subscribe(r => Results = r);
        }

        public void UpInput() {
            ResultsSelectedInput--;
        }

        public void DownInput() {
            ResultsSelectedInput++;
        }

        public void EnterInput() {
            var selectedFile = Results.Skip(Math.Max(0, ResultsSelectedInput)).FirstOrDefault() as FileOption;
            if (selectedFile == null)
                return;

            var executer = new ProcessStarter(selectedFile.FullPath);
            executer.Execute();
        }

        public void OpenDbStudio() {
            Process.Start(_jarvis.StudioUrl);
        }

        public void CloseWindow() {
            _eventAggregator.Publish(new CloseLaunchWindowEvent());
        }
    }
}