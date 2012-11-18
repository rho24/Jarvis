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
        private readonly IEventAggregator _eventAggregator;
        private readonly IJarvisService _jarvis;
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

        private IOption SelectedOption {
            get { return Results.Skip(Math.Max(0, ResultsSelectedInput)).FirstOrDefault(); }
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
            var executableOption = SelectedOption as IHasDefaultAction;

            if (executableOption != null) {
                executableOption.Execute();
                CloseWindow();
            }
        }

        public void OpenDbStudio() {
            Process.Start(_jarvis.StudioUrl);
        }

        public void CloseWindow() {
            _eventAggregator.Publish(new CloseLaunchWindowEvent());
        }

        public void CloseJarvis() {
            _eventAggregator.Publish(new CloseJarvisEvent());
        }

        public void SelectSubOption() {
            Results = _jarvis.GetSubOptions(SelectedOption);
        }
    }
}