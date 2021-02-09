using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPageMaster : ContentPage
    {
        public ListView ListView;

        public MainMenuPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainMenuPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainMenuPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainMenuPageMenuItem> MenuItems { get; set; }

            public MainMenuPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainMenuPageMenuItem>(new[]
                {
                    new MainMenuPageMenuItem { Id = 0, Title = "Home" },
                    new MainMenuPageMenuItem { Id = 1, Title = "Settings" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}