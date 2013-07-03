using System;
using System.Drawing;
using System.Reactive.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using ManagedWinapi;
using Application = System.Windows.Application;

namespace Jarvis.Client
{
    public class ShellViewModel : Conductor<LaunchViewModel>, IShell, IHandle<CloseLaunchWindowEvent>, IHandle<CloseJarvisEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly LaunchViewModel _launchViewModel;
        private readonly IWindowManager _windowManager;

        public ShellViewModel(LaunchViewModel launchViewModel, IWindowManager windowManager, IEventAggregator eventAggregator) {
            _launchViewModel = launchViewModel;
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
        }

        public void Handle(CloseJarvisEvent message) {
            Shutdown();
        }

        public void Handle(CloseLaunchWindowEvent message) {
            CloseLaunchWindow();
        }

        protected override void OnInitialize() {
            _eventAggregator.Subscribe(this);

            var icon = new NotifyIcon {Text = "Jarvis", Visible = true, Icon = new Icon("SysTray.ico"), ContextMenu = new ContextMenu(new[] {new MenuItem("Close", (s, e) => Shutdown())})};

            InitializeGlobalHotkey();
            base.OnInitialize();
        }
        
        private void Shutdown() {
            Application.Current.Shutdown();
        }

        private void InitializeGlobalHotkey() {
            var hotkey = new Hotkey {Alt = true, KeyCode = Keys.Space};

            Observable.FromEventPattern(hotkey, "HotkeyPressed")
                      .Subscribe(k => ToggleLaunchWindow());

            hotkey.Enabled = true;
        }

        private void ToggleLaunchWindow() {
            if (ActiveItem == _launchViewModel)
                CloseLaunchWindow();
            else
                OpenLaunchWindow();
        }

        public void OpenLaunchWindow() {
            if (ActiveItem == _launchViewModel)
                return;

            ActivateItem(_launchViewModel);
            _windowManager.ShowWindow(_launchViewModel);
        }

        public void CloseLaunchWindow() {
            if (ActiveItem != _launchViewModel)
                return;

            DeactivateItem(_launchViewModel, true);
        }
    }
}