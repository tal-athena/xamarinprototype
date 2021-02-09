using DecisionsMobile.Models;
using DecisionsMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DoWorkflowPage : ContentPage
    {
        BaseWebViewViewModel viewModel;

        //public string AssignmentUrl { get; set; }
        //public string AssignmentName { get; set; }

        public DoWorkflowPage(Workflow workflow, string actionuUrl)
        {
            InitializeComponent();
            BindingContext = viewModel = new BaseWebViewViewModel();
            viewModel.Title = workflow.EntityName;
            viewModel.Url = actionuUrl;
            // TODO should add a decorator for this boilerplate:
            WebView.Navigating += viewModel.OnNavigating;
            WebView.Navigated += viewModel.OnNavigated;
        }
    }
}