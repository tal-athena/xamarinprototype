using DecisionsMobile.Constants;
using DecisionsMobile.Services;
using DecisionsMobile.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PortalPage : ContentPage
    {
        BaseWebViewViewModel viewModel = new BaseWebViewViewModel();

        public bool WebViewIsLoading { get; set; }

        public string PortalUrl { get; set; }

        public PortalPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new BaseWebViewViewModel();
            viewModel.Title = this.Title = "The Portal";

            WebView.Navigating += viewModel.OnNavigating;
            WebView.Navigated += viewModel.OnNavigated;

            MessagingCenter.Subscribe<MainPage>(this, Message.CLOSE_LOGIN, (obj) =>
            {
                // subscribe to CLOSE_LOGIN as a proxy for the server URL changing, for now
                // as a workaround for the fact that this page currently bootstraps before login,
                // and OnAppearing doesn't seem to fire consistently on tab clicks :(
                UpdatePortalUrl();
            });

            
        }

        void Settings_Clicked(object sender, EventArgs e)
        {
            viewModel.ShowSettingsPage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdatePortalUrl();
            //NavigationPage.SetTitleView = this.Con;
        }

        void UpdatePortalUrl()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                viewModel.IsWebViewSuccess = false;
            }
            else
            {
                var url = AuthService.Instance.CurrentSession.ServerBaseUrl +
                    RestConstants.MOBILE_API_ROOT;
                url = AuthService.Instance.AppendSessionId(url);

                viewModel.Url = url;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            UpdatePortalUrl();
        }
    }

}