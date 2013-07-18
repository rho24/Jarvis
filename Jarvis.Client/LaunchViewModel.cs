using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using Caliburn.Micro;
using Jarvis.Core;
using Jarvis.Core.Options;

namespace Jarvis.Client
{
    public class LaunchViewModel : Screen
    {
        readonly IEventAggregator _eventAggregator;
        readonly IJarvisService _jarvis;
        IEnumerable<IOption> _results;
        int _resultsSelectedInput;
        string _text;

        public string UserInput {
            get { return _text; }
            set {
                if(value == _text) return;
                _text = value;
                NotifyOfPropertyChange(() => UserInput);
            }
        }

        public IEnumerable<IOption> Results {
            get { return _results; }
            set {
                if(Equals(value, _results)) return;
                _results = value;
                NotifyOfPropertyChange(() => Results);
            }
        }

        public int ResultsSelectedInput {
            get { return _resultsSelectedInput; }
            set {
                if(value == _resultsSelectedInput) return;
                _resultsSelectedInput = value;
                NotifyOfPropertyChange(() => ResultsSelectedInput);
            }
        }

        IOption SelectedOption {
            get { return Results.Skip(Math.Max(0, ResultsSelectedInput)).FirstOrDefault(); }
        }

        public string UserInputSelectedText { get; set; }
        public int UserInputSelectionLength { get; set; }

        public int UserInputSelectionStart { get; set; }

        public LaunchViewModel(IJarvisService jarvis, IEventAggregator eventAggregator) {
            _jarvis = jarvis;
            _eventAggregator = eventAggregator;

            InitialiseSearch();
        }

        void InitialiseSearch() {
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
            if(_jarvis.ExecuteOption(SelectedOption))
                CloseWindow();
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

        protected override void OnActivate() {
            if(UserInput != null) {
                UserInputSelectionStart = 0;
                UserInputSelectionLength = UserInput.Length;
            }
        }

        protected override void OnViewLoaded(object view) {
            base.OnViewLoaded(view);

            var window = (Window)GetView();
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = (SystemParameters.PrimaryScreenWidth / 2) - (window.ActualWidth / 2);
            window.Top = (SystemParameters.PrimaryScreenHeight / 2) - (window.ActualHeight / 2);
        }
    }
}