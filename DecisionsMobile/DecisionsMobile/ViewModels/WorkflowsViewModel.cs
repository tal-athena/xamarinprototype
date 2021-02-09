using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DecisionsMobile.ViewModels
{

    public class WorkflowsViewModel : StoreViewModel<Workflow>
    {
        public WorkflowsViewModel()
        {
            DataStore = DependencyService.Get<IDataStore<Workflow>>() ?? new WorkflowStore();
            Title = "Workflows";
            Items = new ObservableCollection<Workflow>();
        }
        public void ShowFormSubmissionsPage()
        {
            MessagingCenter.Send(this, Message.SHOW_FORM_SUBMISSION);
        }
    }
}