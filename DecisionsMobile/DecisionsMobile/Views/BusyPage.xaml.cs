using DecisionsMobile.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    /// <summary>
    /// A page to show between showing other pages, e.g. render this page
    /// while checking session, before showing login unecessarily, or 
    /// rendering home page with bad session ID.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusyPage : ContentPage
    {
        public BusyPage(string title)
        {
            InitializeComponent();
            BindingContext = new BusyPageViewModel(title);
        }
    }
}