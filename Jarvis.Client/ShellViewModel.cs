using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Forms;
using Caliburn.Micro;
using Jarvis.Core;
using ManagedWinapi;
using Application = System.Windows.Application;

namespace Jarvis.Client
{
    public class ShellViewModel : PropertyChangedBase, IShell
    {
        private readonly IJarvisService _jarvis;
        private readonly Application _application;
        private IEnumerable<IItem> _results;
        private int _resultsSelectedInput;
        private string _text;
        private WindowState _winState = WindowState.Normal;

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

        public WindowState WinState {
            get { return _winState; }
            set {
                if (value == _winState) return;
                _winState = value;
                NotifyOfPropertyChange(() => WinState);
            }
        }

        public ShellViewModel(IJarvisService jarvis, Application application) {
            _jarvis = jarvis;
            _application = application;

            InitialiseSearch();

            InitializeGlobalHotkey();
        }

        private void InitializeGlobalHotkey() {
            var hotkey = new Hotkey {Ctrl = true, KeyCode = Keys.Space};

            Observable.FromEventPattern(hotkey, "HotkeyPressed")
                      .Subscribe(k => ToggleVisibility());

            hotkey.Enabled = true;
        }

        public void HideWindow() {
            WinState = WindowState.Minimized;
        }

        public void Close() {
            _application.MainWindow.Close();
        }

        public void ToggleVisibility() {
            WinState = WinState == WindowState.Normal ? WindowState.Minimized : WindowState.Normal;
        }

        private void InitialiseSearch() {
            Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                      .Where(e => e.EventArgs.PropertyName == "UserInput")
                      .Select(e => UserInput)
                      .DistinctUntilChanged()
                      .StartWith("")
                      .ObserveOn(TaskPoolScheduler.Default)
                      .Select(s => _jarvis.Items(s))
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

        public void OpenDbStudio() {
            Process.Start(_jarvis.StudioUrl);
        }
    }
}