
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using DecisionsMobile.Services;
using Xamarin.Forms;

namespace DecisionsMobile.Droid
{
    [Activity(Label = "Decisions", 
        Icon = "@mipmap/icon", Theme = "@style/splashscreen", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            CreateNotificationFromIntent(Intent);
        }
        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }
        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {                
                string title = intent.Extras.GetString(AndroidNotificationManager.TitleKey);
                string message = intent.Extras.GetString(AndroidNotificationManager.MessageKey);
                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(message))
                    DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);                
            }
        }
    }
}