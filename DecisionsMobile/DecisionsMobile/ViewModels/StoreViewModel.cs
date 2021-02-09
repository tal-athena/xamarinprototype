using DecisionsMobile.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DecisionsMobile.ViewModels
{
    /// <summary>
    /// A View Model that includes a data-store object
    /// </summary>
    public class StoreViewModel<T> : BaseViewModel
    {

        public IDataStore<T> DataStore { get; set; }

        // TODO learn about DependencyService.Get<IDataStore<T>>() ?? new T();
        // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/dependency-service/

        private ObservableCollection<T> items;

        public ObservableCollection<T> Items
        {
            get => items;
            set
            {
                SetProperty(ref items, value);
                OnPropertyChanged(nameof(ShowPlaceHolder));
            }
        }

        public override void SetIsBusy(bool value)
        {
            base.SetIsBusy(value);
            OnPropertyChanged(nameof(ShowPlaceHolder));
        }

        public bool ShowPlaceHolder => !IsBusy && (Items == null || Items.Count < 1);

        public Command LoadItemsCommand { get; set; }

        public StoreViewModel()
        {
            LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand() );
        }

        protected void ExecuteLoadItemsCommand()
        {
            Device.BeginInvokeOnMainThread(async () =>
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
                    var items = await DataStore.GetItemsAsync(true);
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    SetIsBusy(false);
                }
            });
        }
    }
}
