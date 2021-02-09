using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        private Session Session { get; set; }
        LoginViewModel viewModel;

        public LoginPage(Session session)
        {
            InitializeComponent();

            Session = session;
            BindingContext = viewModel = new LoginViewModel(Session);

            // subscribe to close login message
            MessagingCenter.Subscribe<MainPage>(this, Message.CLOSE_LOGIN, Close);
            MessagingCenter.Subscribe<MainPage, Session>(this, Message.SHOW_LOGIN_ERROR, ShowLoginError);
            MessagingCenter.Subscribe<ServerConfigPage, string>(this, Message.CHANGE_SERVER_URI, ChangeSeverUri);
        }

        async void Close(MainPage obj)
        {
            await Navigation.PopAsync();
        }

        private void ShowLoginError(MainPage obj, Session session)
        {
            viewModel.Session = session;
        }

        private void ChangeSeverUri(ServerConfigPage obj, string uri)
        {
            uri = uri.TrimEnd('/');
            viewModel.Session.ServerUri = uri;
        }

        void Login_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, Message.CREATE_SESSION, Session);
        }

        public void Credential_Text_Changed(object sender, TextChangedEventArgs e)
        {
            viewModel.HideErrorMessage();
        }

        // FIXME this might be totally unnecessary now that login isn't in a popup.
        // TODO disable back button on android:
        override protected bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed(); // delete this line to disable back when we're ready.
            // return false;
        }
    }
}