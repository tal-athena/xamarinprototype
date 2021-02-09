using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionsMobile.Services
{
    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();

        int ScheduleNotification(string title, string message);

        void ReceiveNotification(string title, string message);
    }
}
