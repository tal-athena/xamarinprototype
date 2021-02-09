using DecisionsMobile.Models.FormService;
using DecisionsMobile.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace DecisionsMobile.ViewModels
{
    /// <summary>
    /// A view-model for managing a series of forms. Currently unused, but will
    /// likely be necessary when implementing offline forms functionality.
    /// </summary>
    public class FormsViewModel : StoreViewModel<StandAloneFormSessionInfo>
    {
        public FormsViewModel()
        {
            DataStore = DependencyService.Get<IDataStore<StandAloneFormSessionInfo>>() ?? new FormDataStore();
            Title = "Form Test";
            Items = new ObservableCollection<StandAloneFormSessionInfo>();
        }
    }
}
