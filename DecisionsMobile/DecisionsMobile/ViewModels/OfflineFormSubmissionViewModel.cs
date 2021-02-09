using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.Services;
using DecisionsMobile.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DecisionsMobile.ViewModels
{

    public class OfflineFormSubmissionViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; set; }

        public ObservableCollection<FormSubmissionModel> Items { get; set; }

        public bool ShowPlaceHolder => !IsBusy && (Items == null || Items.Count < 1);

        public OfflineFormSubmissionViewModel()
        {
            Title = "Form submissions";
            Items = new ObservableCollection<FormSubmissionModel>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<MainPage>(this, Message.OFFLINE_FORMS_SUBMITTED, new Action<MainPage>((MainPage p) =>
            {
                LoadItemsCommand.Execute(null);
            }));
        }
        
        async protected Task ExecuteLoadItemsCommand()
        {
            if (!AuthService.Instance.CurrentSession.IsValid)
            {
                ShowLogin(AuthService.Instance.CurrentSession);
                return;
            }
            if (IsBusy) return;

            SetIsBusy(true);

            try
            {
                Items.Clear();

                var items = await OfflineService.Instance.GetStoredOfflineFormSubmissions();
                foreach (var item in items)
                {
                    Workflow workflow = OfflineService.Instance.GetWorkflowByServiceCategoryId(item.ServiceCategoryId);
                    if (workflow != null)
                    {
                        Items.Add(new FormSubmissionModel
                        {
                            StoredFormSubmissionId = item.Id.Value,
                            CreatedAt = item.CreatedAt,
                            WorkFlowName = workflow.EntityName,
                            IsFailed = (item.IsFailed == 1)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                SetIsBusy(false);
                OnPropertyChanged(nameof(ShowPlaceHolder));
            }
        }

        public async Task RetrySubmitAsync(FormSubmissionModel formSubmission)
        {
            try
            {
                SetIsBusy(true);
                if (await OfflineService.Instance.SubmitOfflineFormAsync(formSubmission.StoredFormSubmissionId))
                {
                    Items.Remove(formSubmission);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            finally
            {
                SetIsBusy(false);
            }
        }

        public async Task DiscardAsync(FormSubmissionModel formSubmission)
        {
            try
            {
                SetIsBusy(true);
                if (await OfflineService.Instance.DiscardStoredFormSubmission(formSubmission.StoredFormSubmissionId))
                {
                    Items.Remove(formSubmission);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            finally
            {
                SetIsBusy(false);
            }

        }
    }
}