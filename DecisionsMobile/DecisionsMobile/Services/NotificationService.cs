using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DecisionsMobile.Services
{
    public class NotificationService
    {
        readonly INotificationManager _notificationManager = null;

        private static readonly Lazy<NotificationService> lazy = new Lazy<NotificationService>
           (() => new NotificationService());

        public static NotificationService Instance => lazy.Value;

        public event EventHandler NotificationTapped;

        public NotificationService()
        {
            _notificationManager = DependencyService.Get<INotificationManager>();
            _notificationManager.Initialize();
            _notificationManager.NotificationReceived += OnNotificationReceived;
        }

        private void OnNotificationReceived(object sender, EventArgs e)
        {
            var args = e as NotificationEventArgs;
            if (args.Title.Contains("Submission Failed"))
                NotificationTapped?.Invoke(this, e);
        }

        public void ScheduleNotification(string title, string msg)
        {
            _notificationManager.ScheduleNotification(title, msg);
        }
    }
}
