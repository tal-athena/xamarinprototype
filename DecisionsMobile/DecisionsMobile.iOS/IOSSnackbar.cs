using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DecisionsMobile.Services;
using Foundation;
using TTGSnackBar;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DecisionsMobile.iOS.IOSSnackbar))]
namespace DecisionsMobile.iOS
{
    public class IOSSnackbar : ISnackbar
    {
        const double LONG_DELAY = 3.5;
        const double SHORT_DELAY = 1.5;

        public void LongAlert(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }

        public void ShortAlert(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        void ShowAlert(string message, double seconds)
        {

            var snackbar = new TTGSnackBar.TTGSnackbar(message)
            {
                Duration = TimeSpan.FromSeconds(seconds),
                MessageMarginLeft = 15
            };
            snackbar.Show();
        }
    }
}