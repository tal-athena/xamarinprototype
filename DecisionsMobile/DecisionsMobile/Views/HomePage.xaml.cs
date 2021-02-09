using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : Xamarin.Forms.TabbedPage
    {
        public HomePage()
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }

        protected override void OnCurrentPageChanged()
        {
            // Don't know why this is necessary... I guess because the
            // TabbedPage has content children and is itself inside a
            // Navigation page, instead of containing a NavigationPage,
            // but this solves the missing title problem.
            base.OnCurrentPageChanged();
            this.Title = this.CurrentPage.Title;
        }

    }
}
