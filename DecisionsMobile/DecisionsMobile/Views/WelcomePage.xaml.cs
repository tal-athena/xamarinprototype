using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.Models.FormService;
using DecisionsMobile.Services;
using DecisionsMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {   
        BaseViewModel _viewModel;
        Session _session;
        public WelcomePage(Session session)
        {
            InitializeComponent();
            _session = session;
            BindingContext = _viewModel = new BaseViewModel();

            MessagingCenter.Subscribe<FormPage>(this, Message.ACCOUNT_CREATED, AccountCreated);
        }
        public void AccountCreated(FormPage page)
        {   
            MessagingCenter.Send(this, Message.SHOW_LOGIN, _session);
        }
        private async void CreateAccount_Clicked(object sender, EventArgs e)
        {
            _viewModel.SetIsBusy(true);
            btnCreateAccount.IsEnabled = false;

            FlowExecutionStateInstruction instruction = await FlowExecutionService.StartFlowWithData(RestConstants.CreateAccountFlowId, new DataPair[0], true);

            if (instruction == null)
            {
                _viewModel.SetIsBusy(false);
                btnCreateAccount.IsEnabled = true;
                Debug.WriteLine("Flow Execution Instruction was null");
                
                return;
            }

            if (FlowExecutionService.IsShowFormType(instruction))
            {
                // get the form JSON
                StandAloneFormSessionInfo formModel = await FormService.GetFormSessionSurfaceJson(instruction, true);
                // launch the view to render the form:
                FormViewModel formViewModel;
                formViewModel = new FormViewModel(formModel, instruction);
                if (formModel == null)
                {
                    _viewModel.SetIsBusy(false);
                    btnCreateAccount.IsEnabled = true;
                    await DisplayAlert("Error", "Problem loading form data.", "OK");
                    return;
                }
                await Navigation.PushAsync(new FormPage(formViewModel, true));
            }
            else
            {
                // throw a modal that the flow has been started
                await DisplayAlert("Workflow", $"Flow for Account creation has been started", "OK");
            }
            btnCreateAccount.IsEnabled = true;
            _viewModel.SetIsBusy(false);
        }

        private void Login_Clicked(object sender, EventArgs e)
        {
            var loginPage = new LoginPage(_session);
            Navigation.PushAsync(loginPage);
        }

        private void Preview_Clicked(object sender, EventArgs e)
        {

        }
    }
}