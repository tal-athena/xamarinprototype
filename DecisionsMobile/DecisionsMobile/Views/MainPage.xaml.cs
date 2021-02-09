using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.Services;
using DecisionsMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DecisionsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : NavigationPage
    {
        WelcomePage WelcomePage { get; set; }
        LoginPage LoginPage { get; set; }

        HomePage HomePage { get; set; }

        ServerConfigPage ServerConfigPage { get; set; }

        BusyPage BusyPage { get; set; }

        public MainPage()
        {
            //ShowServerConfigPage(AuthService.Instance.CurrentSession);
            ShowBusyPage("Checking Session");

            InitializeComponent();

            MessagingCenter.Subscribe<WelcomePage, Session>(this, Message.SHOW_LOGIN, (obj, session) => ShowLogin(session));
            MessagingCenter.Subscribe<App, Session>(this, Message.SHOW_LOGIN, (obj, session) => ShowLogin(session));
            MessagingCenter.Subscribe<App, Session>(this, Message.SHOW_WELCOME, (obj, session) => ShowWelcome(session));
            MessagingCenter.Subscribe<App>(this, Message.CLOSE_LOGIN, (obj) => ShowHomePageAsync());
            MessagingCenter.Subscribe<BaseViewModel, Session>(this, Message.SHOW_LOGIN, (obj, session) => ShowLogin(session));
            MessagingCenter.Subscribe<ServerConfigPage, Session>(this, Message.SHOW_LOGIN,
                (obj, session) => ShowLoginOverServerConfig(session));
            MessagingCenter.Subscribe<Views.LoginPage, Session>(this, Message.CREATE_SESSION, CreateSession);
            MessagingCenter.Subscribe<BaseViewModel>(this, Message.SHOW_SETTINGS, ShowSettings);
            MessagingCenter.Subscribe<WorkflowsViewModel>(this, Message.SHOW_FORM_SUBMISSION, ShowFormSubmissionPage);

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            NotificationService.Instance.NotificationTapped += OnPushNotificationReceived;
        }

        ~MainPage()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        private async void OnPushNotificationReceived(object sender, EventArgs e)
        {   
            if (!(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1] is OfflineFormSubmissionsPage))
                await Navigation.PushAsync(new OfflineFormSubmissionsPage());
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await OfflineService.Instance.SubmitPendingSubmissionsAsync();
                MessagingCenter.Instance.Send(this, Message.OFFLINE_FORMS_SUBMITTED);
            }
        }

        public Page PageAtTopOfStack
        {
            get => Navigation.NavigationStack[Navigation.NavigationStack.Count - 1];
        }

        private async void ShowServerConfigPageAsync(Session session)
        {
            //HideBusyPage();

            if (PageIsAtTopOfStack(ServerConfigPage))
            {
                return;
            }

            ServerConfigPage = ServerConfigPage ?? new ServerConfigPage(session);
            await AddAtRootAndShow(ServerConfigPage);
        }

        private async void ShowWelcome(Session session)
        {   
            if (PageIsAtTopOfStack(WelcomePage))
            {
                return;
            }

            WelcomePage = WelcomePage ?? new WelcomePage(session);
            await AddAtRootAndShow(WelcomePage);
        }

        private async void ShowLogin(Session session, Page pageToHide)
        {
            if (PageIsAtTopOfStack(LoginPage))
            {
                return;
            }
            LoginPage = LoginPage ?? new LoginPage(session);
            await AddAtRootAndShow(LoginPage);
        }

        private void ShowBusyPage(string title)
        {
            BusyPage = BusyPage ?? new BusyPage(title);
            Navigation.PushAsync(BusyPage);
        }

        private void RemoveBusyPage()
        {
            RemovePage(BusyPage);
            BusyPage = null;
        }

        private void RemovePage(Page page)
        {
            if (ContainsPage(BusyPage))
            {
                Navigation.RemovePage(BusyPage);
            }
            BusyPage = null;
        }

        private void ShowLogin(Session session)
        {
            ShowLogin(session, HomePage);
        }

        private void ShowLoginOverServerConfig(Session session)
        {
            ShowLogin(session, ServerConfigPage);
        }

        private async void ShowHomePageAsync()
        {
            if (PageIsAtTopOfStack(HomePage))
            {
                // NOOP
                return; // login is probably not actually showing.
            }

            HomePage = HomePage ?? new HomePage(); // make sure we have a HomePage instance.
            await AddAtRootAndShow(HomePage);

            // clear pointers to login pages, we might not need them for a long time.
            WelcomePage = null;
            LoginPage = null;
            ServerConfigPage = null;
            HomePage = null;
        }

        private async void CreateSession(Views.LoginPage loginPage, Session session)
        {
            var newSession = await AuthService.Instance.CreateSession(session);
            if (newSession.IsValid)
            {
                ShowHomePageAsync();
            }
            else
            {
                MessagingCenter.Send(this, Message.SHOW_LOGIN_ERROR, session);
            }
        }

        private async void ShowSettings(ViewModels.BaseViewModel viewModel)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
        private async void ShowFormSubmissionPage(WorkflowsViewModel viewModel)
        {
            await Navigation.PushAsync(new OfflineFormSubmissionsPage());
        }
        /// <summary>
        /// Returns true if page is not null and is in the NavigationStack
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        private bool ContainsPage(Page page)
        {
            return StackContainsPage(page, Navigation.NavigationStack);
        }

        private bool ContainsModal(Page page)
        {
            return StackContainsPage(page, Navigation.ModalStack);
        }

        /// <summary>
        /// returns true if page is not null and is in the NavigationStack
        /// </summary>
        /// <param name="Page"> to check for </param>
        /// <param name="stack"> to check </param>
        /// <returns>true if page is not null and is in the NavigationStack</returns>
        private bool StackContainsPage(Page Page, IReadOnlyList<Page> stack)
        {
            if (Page == null) return false;

            foreach (Page page in stack)
            {
                if (page == Page)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if page is not null and at top of navigation stack.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public bool PageIsAtTopOfStack(Page page)
        {
            if (page == null)
            {
                return false;
            }

            Type typeofPageAppearing = page.GetType();
            var stack = Navigation.NavigationStack;
            return (PageAtTopOfStack.GetType() == typeofPageAppearing);
        }



        /// <summary>
        /// Pushes a page under current root page, then removes all others.
        /// </summary>
        /// <param name="pageToShow">page to add to the navigation stack</param>
        private async Task AddAtRootAndShow(Page pageToShow)
        {
            if (pageToShow == null || PageIsAtTopOfStack(pageToShow) || ContainsPage(pageToShow))
            {
                return;
            }
            var rootPage = Navigation.NavigationStack[0];
            Navigation.InsertPageBefore(pageToShow, rootPage);
            await PopToRootAsync();
            return;
        }
    }
}