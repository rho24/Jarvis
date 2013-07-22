using System;
using System.Windows.Forms;

namespace Jarvis.Client
{
    public interface INotificationManager
    {
        void Initialize(NotifyIcon icon);
    }
}