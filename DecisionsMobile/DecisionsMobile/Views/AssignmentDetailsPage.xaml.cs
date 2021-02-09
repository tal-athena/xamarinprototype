using DecisionsMobile.Models;
using DecisionsMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        AssignmentDetailViewModel viewModel;

        public ItemDetailPage(AssignmentDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Assignment
            {
                EntityName = "Item 1",
                StepName = "This is an item description."
            };

            viewModel = new AssignmentDetailViewModel(item);
            BindingContext = viewModel;
        }

        protected void WorkAssignment_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WorkAssignmentPage(viewModel.Item));
        }
    }
}
