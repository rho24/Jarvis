using System;
using System.Linq;
using System.Windows.Forms;
using Caliburn.Micro;
using Jarvis.Core.Events;
using Jarvis.Core.Gmail;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;

namespace Jarvis.Client
{
    class SystemTrayNotificationManager : INotificationManager, IHandle<CurrentUnreadEmails>
    {
        readonly IEventAggregator _eventAggregator;
        NotifyIcon _icon;
        object _currentNotification;

        public SystemTrayNotificationManager(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;
        }

        public void Initialize(NotifyIcon icon) {
            _icon = icon;

            _icon.BalloonTipClicked += _icon_BalloonTipClicked;
            _icon.BalloonTipClosed += _icon_BalloonTipClosed;
        }

        void _icon_BalloonTipClicked(object sender, EventArgs e)
        {
            if(_currentNotification is CurrentUnreadEmails)
                _eventAggregator.Publish(new ExecuteOptionMessage{Option = new GmailOption()});
        }

        void _icon_BalloonTipClosed(object sender, EventArgs e) {
            _currentNotification = null;
        }

        public void Handle(CurrentUnreadEmails message)
        {
            if(message.Emails.Any()) {
                _currentNotification = message;
                _icon.ShowBalloonTip(30000, "Emails!", "You have mail:)\n{0} to be exact".Fmt(message.Emails.Count()), ToolTipIcon.Info);
            }
        }
    }
}