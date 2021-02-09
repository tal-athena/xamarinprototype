using DecisionsMobile.Services;
using DecisionsMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsServersPage : ContentPage
    {
        LoginViewModel viewModel;

        public SettingsServersPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new LoginViewModel(AuthService.Instance.CurrentSession)
            {
                Title = "Session"
            };
            BindingContext = viewModel;
        }

        void Logout_Clicked(object sender, EventArgs e)
        {
            viewModel.ShowLogin(AuthService.Instance.LogOut());
        }
    }
}