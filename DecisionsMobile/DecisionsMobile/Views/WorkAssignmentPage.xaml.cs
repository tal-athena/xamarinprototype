using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.Services;
using DecisionsMobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkAssignmentPage : ContentPage
    {
        BaseWebViewViewModel viewModel;
        Assignment assignment;
        //public string AssignmentUrl { get; set; }
        //public string AssignmentName { get; set; }

        public WorkAssignmentPage(Assignment assignment)
        {
            InitializeComponent();
            BindingContext = viewModel = new BaseWebViewViewModel();
            viewModel.Title = assignment.EntityName;
            this.assignment = assignment;            
            // TODO should add a decorator for this boilerplate:
            WebView.Navigating += viewModel.OnNavigating;
            WebView.Navigated += viewModel.OnNavigated;

            UpdateAssignmentUrl();
        }

        // Idealist in me wants this elsewhere, as it's not "display logic," strictly speaking.
        public void UpdateAssignmentUrl()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                viewModel.IsWebViewSuccess = false;
            }
            else
            {
                string url = $"{AuthService.Instance.CurrentSession.ServerBaseUrl}{RestConstants.MOBILE_API_ROOT}/" +
                $"?{RestConstants.ASSIGNMENT_ID}={assignment.AssignmentId}";
                url = AuthService.Instance.AppendSessionId(url);
                url = RestConstants.AppendChromeless(url);
                url = RestConstants.AppendLocationCenter(url);

                viewModel.Url = url;
            }
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            UpdateAssignmentUrl();
        }
    }
}