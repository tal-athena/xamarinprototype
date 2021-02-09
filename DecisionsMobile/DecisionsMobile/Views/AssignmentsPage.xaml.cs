using DecisionsMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssignmentsPage : ContentPage
    {
        AssignmentViewModel viewModel;

        public AssignmentsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new AssignmentViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Models.Assignment item))
                return;

            // var url = viewModel.DataStore.GetItemActionUrlAsync(item); // DEBUG

            await Navigation.PushAsync(new ItemDetailPage(new AssignmentDetailViewModel(item)));

            // Manually deselect item.
            AssignmentsListView.SelectedItem = null;
        }

        void Settings_Clicked(object sender, EventArgs e)
        {
            viewModel.ShowSettingsPage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}