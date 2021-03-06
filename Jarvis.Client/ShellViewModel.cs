﻿using System;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using Jarvis.Core.Events;
using Jarvis.Core.Infrastructure;
using ManagedWinapi;
using NLog;
using Application = System.Windows.Application;
using LogManager = NLog.LogManager;

namespace Jarvis.Client
{
    public class ShellViewModel : Conductor<LaunchViewModel>, IShell, IHandle<CloseLaunchWindowEvent>, IHandle<CloseJarvisEvent>
    {
        readonly LaunchViewModel _launchViewModel;
        readonly Logger _logger = LogManager.GetCurrentClassLogger();
        readonly IWindowManager _windowManager;
        readonly INotificationManager _notificationManager;
        IDisposable _hotKeySubscriber;
        Hotkey _hotkey;
        NotifyIcon _icon;

        public ShellViewModel(LaunchViewModel launchViewModel, IWindowManager windowManager, INotificationManager notificationManager) {
            _launchViewModel = launchViewModel;
            _windowManager = windowManager;
            _notificationManager = notificationManager;
        }

        public void Handle(CloseJarvisEvent message) {
            Shutdown();
        }

        public void Handle(CloseLaunchWindowEvent message) {
            CloseLaunchWindow();
        }

        protected override void OnInitialize() {
            _icon = new NotifyIcon {
                Text = "Jarvis",
                Visible = true,
                Icon = new Icon("SysTray.ico"),
                ContextMenu =
                    new ContextMenu(
                        new[] {
                            new MenuItem("Open launcher", (s, e) => OpenLaunchWindow()), new MenuItem("Initialize hotkey", (s, e) => InitializeGlobalHotkey()),
                            new MenuItem("Close", (s, e) => Shutdown())
                        })
            };

            _notificationManager.Initialize(_icon);

            InitializeGlobalHotkey();
            base.OnInitialize();
        }

        void Shutdown() {
            Application.Current.Shutdown();
        }

        void InitializeGlobalHotkey() {
            if(_hotKeySubscriber != null) {
                _hotkey.Enabled = false;
                _hotKeySubscriber.Dispose();
            }

            _hotkey = new Hotkey { Alt = true, KeyCode = Keys.Space };

            _hotKeySubscriber = Observable.FromEventPattern(_hotkey, "HotkeyPressed").Subscribe(k => ToggleLaunchWindow());

            _hotkey.Enabled = true;

            _logger.Debug("Hotkey.Enabled = '{0}'".Fmt(_hotkey.Enabled));
        }

        void ToggleLaunchWindow() {
            if(ActiveItem == _launchViewModel)
                CloseLaunchWindow();
            else
                OpenLaunchWindow();
        }

        public void OpenLaunchWindow() {
            if(ActiveItem == _launchViewModel)
                return;

            ActivateItem(_launchViewModel);

            _windowManager.ShowWindow(_launchViewModel);
        }

        public void CloseLaunchWindow() {
            if(ActiveItem != _launchViewModel)
                return;

            DeactivateItem(_launchViewModel, true);
        }
    }
}