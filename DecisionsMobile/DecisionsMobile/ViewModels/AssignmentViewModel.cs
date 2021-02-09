using DecisionsMobile.Models;
using DecisionsMobile.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace DecisionsMobile.ViewModels
{
    public class AssignmentViewModel : StoreViewModel<Assignment>
    {

        public AssignmentViewModel()
        {
            DataStore = DependencyService.Get<IDataStore<Assignment>>() ?? new AssignmentStore();
            Title = "Assignments";
            Items = new ObservableCollection<Models.Assignment>();
        }
    }
}