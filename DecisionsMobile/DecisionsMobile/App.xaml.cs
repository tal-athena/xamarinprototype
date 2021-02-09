using DecisionsMobile.Constants;
using DecisionsMobile.Services;
using DecisionsMobile.Themes;
using DecisionsMobile.Views;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DecisionsMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Device.SetFlags(new[] { "RadioButton_Experimental" });

            Resources.MergedDictionaries.Add(new DefaultTheme());

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            CheckSession();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Task.Run(async () =>
                {   
                    await OfflineService.Instance.SubmitPendingSubmissionsAsync();
                });
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            CheckSession();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Task.Run(async () =>
                {
                    await OfflineService.Instance.SubmitPendingSubmissionsAsync();

                });
            }
        }

        protected async void CheckSession()
        {
            // TODO use dependency injection for this, so we just have a ref to the _interface_.

            Models.Session session = await AuthService.Instance.CheckSessionAsync();
            if (session.IsValid)
            {
                // This is safe, because if it isn't showing, this is a n00p
                MessagingCenter.Send(this, Message.CLOSE_LOGIN);
            }
            else
            {
                var haveEverLoggedIn = AuthService.Instance.HaveEverLoggedIn();
                if (haveEverLoggedIn)
                    MessagingCenter.Send(this, Message.SHOW_LOGIN, session);
                else MessagingCenter.Send(this, Message.SHOW_WELCOME, session);
            }

        }
    }
}

// TODO global way to handle 401s and show login

// Hook up the Mac
// https://docs.microsoft.com/en-us/xamarin/ios/get-started/installation/windows/connecting-to-mac/

// TODO Login Error feedback

// TODO loading indicator for Web Views
// https://stackoverflow.com/questions/31748076/xamarin-forms-webview-initial-loading-indicator

// TODO folder view(s)
// https://github.com/AdaptSolutions/Xamarin.Forms-TreeView

// TODO be good to debounce certain inputs, etc. or have observable data in general
// Reactive Extensions FTW!
// https://forums.xamarin.com/discussion/42876/add-delay-and-cancelation-to-a-command

// TODO debug login in Xamarin Live - had to add an extra check in the Assignments page to get it to show,
// but the network request doesn't seem to complete. Not sure why...
// TODO make sure keyboard gets closed on hide login 
// https://forums.xamarin.com/discussion/comment/172077#Comment_172077
