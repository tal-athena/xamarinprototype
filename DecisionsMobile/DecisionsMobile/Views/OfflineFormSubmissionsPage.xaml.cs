using DecisionsMobile.Models;
using DecisionsMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfflineFormSubmissionsPage : ContentPage
    {
        OfflineFormSubmissionViewModel viewModel = new OfflineFormSubmissionViewModel();

        public OfflineFormSubmissionsPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void DoneLaunching()
        {
            viewModel.SetIsBusy(false);
            FormSubmissionListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void OnDiscard(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            var formSubmission = btn.BindingContext as FormSubmissionModel;
            await viewModel.DiscardAsync(formSubmission);
        }

        private async void OnRetry(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            var formSubmission = btn.BindingContext as FormSubmissionModel;
            await viewModel.RetrySubmitAsync(formSubmission);
        }
    }

}