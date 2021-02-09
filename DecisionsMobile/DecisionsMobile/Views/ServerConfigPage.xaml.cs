using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.Services;
using DecisionsMobile.Themes;
using DecisionsMobile.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServerConfigPage : ContentPage
    {
        Task<bool> canPingServer;
        CancellationTokenSource cts;

        private ServerConfigViewModel viewModel;

        public ServerConfigPage(Session session)
        {
            InitializeComponent();

            BindingContext = viewModel = new ServerConfigViewModel()
            {
                ServerUri = session.ServerUri
            };
        }

        void Next_Clicked(object sender, EventArgs e)
        {   
            MessagingCenter.Send(this, Message.CHANGE_SERVER_URI, viewModel.ServerUri);
            Navigation.PopAsync();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            viewModel.ServerUri = RestConstants.DEFAULT_SERVER_URI;
        }
    }
}