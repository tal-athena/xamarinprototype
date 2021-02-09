using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using DecisionsMobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(DecisionsMobile.Droid.AndroidSnackbar))]
namespace DecisionsMobile.Droid
{
    public class AndroidSnackbar : ISnackbar
    {
        public void LongAlert(string message)
        {
            var activity = (Activity)Forms.Context;
            var view = activity.FindViewById(Android.Resource.Id.Content);
            Snackbar.Make(view, message, Snackbar.LengthLong).Show();
        }

        public void ShortAlert(string message)
        {
            var activity = (Activity)Forms.Context;
            var view = activity.FindViewById(Android.Resource.Id.Content);
            Snackbar.Make(view, message, Snackbar.LengthShort).Show();
        }
    }
}