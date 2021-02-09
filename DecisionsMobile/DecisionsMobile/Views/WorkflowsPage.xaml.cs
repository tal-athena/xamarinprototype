using DecisionsMobile.Models;
using DecisionsMobile.Models.FormService;
using DecisionsMobile.Services;
using DecisionsMobile.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static DecisionsMobile.Services.WorkflowStore;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkflowsPage : ContentPage
    {
        WorkflowsViewModel viewModel = new WorkflowsViewModel();

        public WorkflowsPage()
        {
            InitializeComponent();
            BindingContext = viewModel;

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }
        ~WorkflowsPage()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            viewModel.LoadItemsCommand.Execute(null);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Models.Workflow workflow))
            {
                return;
            }

            viewModel.SetIsBusy(true); // show loader in case fetching action takes time

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {

                WorkFlowAction action = await (viewModel.DataStore as WorkflowStore).GetWorkflowActionAsync(workflow);

                Debug.WriteLine(action);

                // see if it's a flow we can run:
                if (action?.RunFlowId != null)
                {
                    FlowExecutionStateInstruction instruction = await FlowExecutionService.StartFlowWithData(action.RunFlowId, new DataPair[0]);

                    if (instruction == null)
                    {
                        Debug.WriteLine("Flow Execution Instruction was null");
                        DoneLaunching();
                        return;
                    }

                    if (FlowExecutionService.IsShowFormType(instruction))
                    {
                        // get the form JSON
                        StandAloneFormSessionInfo formModel = await FormService.GetFormSessionSurfaceJson(instruction);
                        // launch the view to render the form:
                        FormViewModel formViewModel;
                        if (workflow.CanRunOffline)
                            formViewModel = new FormViewModel(formModel, instruction, true, workflow.ServiceCatalogId);
                        else
                            formViewModel = new FormViewModel(formModel, instruction);
                        if (formModel == null)
                        {
                            await DisplayAlert("Error", "Problem loading form data.", "OK");
                            DoneLaunching();
                            return;
                        }
                        DoneLaunching();
                        await Navigation.PushAsync(new FormPage(formViewModel));
                    }
                    else
                    {
                        // throw a modal that the flow has been started
                        DoneLaunching();
                        await DisplayAlert("Workflow", $"Flow for {workflow.EntityName} has been started", "OK");
                    }

                    return;
                }
                // Otherwise, try to embed the result in a web-view

                var actionuUrl = action.WebViewUrl;

                viewModel.SetIsBusy(false);

                if (string.IsNullOrEmpty(actionuUrl))
                {
                    await DisplayAlert("Alert", "No valid action was found for this item", "OK");
                    return;
                }
                // Manually deselect item.
                WorkflowListView.SelectedItem = null;

                await Navigation.PushAsync(new DoWorkflowPage(workflow, actionuUrl));

            }
            else
            {
                // get the form JSON

                var formInfos = await OfflineService.Instance.GetStandAloneFormSessionInfoAsync(workflow.ServiceCatalogId);

                if (formInfos == null || formInfos.Count <= 0)
                {
                    await DisplayAlert("Error", "Problem loading form data.", "OK");
                    DoneLaunching();
                    return;
                }
                StandAloneFormSessionInfo formModel = formInfos[0];

                FlowExecutionStateInstruction instruction = new FlowExecutionStateInstruction
                {
                    FormTitle = workflow.EntityName
                };
                FormViewModel formViewModel;
                if (workflow.CanRunOffline)
                    formViewModel = new FormViewModel(formModel, instruction, true, workflow.ServiceCatalogId);
                else
                    formViewModel = new FormViewModel(formModel, instruction);

                DoneLaunching();
                await Navigation.PushAsync(new FormPage(formViewModel));
            }

        }

        private void DoneLaunching()
        {
            viewModel.SetIsBusy(false);
            WorkflowListView.SelectedItem = null;
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

        private void FormSubmissions_Clicked(object sender, EventArgs e)
        {
            viewModel.ShowFormSubmissionsPage();
        }
    }

}