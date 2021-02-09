using System;
using Xamarin.Forms;

namespace DecisionsMobile.ViewModels
{

    class BaseWebViewViewModel : BaseViewModel
    {
        string url;
        bool isWebViewSuccess = true;
        public string Url
        {
            get => url;
            set
            {
                url = value;
                OnPropertyChanged(nameof(Url));
            }
        }
        public bool IsWebViewSuccess
        {
            get => isWebViewSuccess && !IsBusy;
            set
            {
                isWebViewSuccess = value;
                OnPropertyChanged(nameof(IsWebViewSuccess));
                OnPropertyChanged(nameof(IsWebViewFail));
            }
        }
        public bool IsWebViewFail
        {
            get => !isWebViewSuccess && !IsBusy;
        }
        public void OnNavigating(Object sender, WebNavigatingEventArgs e)
        {
            // tablet web views have an in-browser loader, so for a while:
            // SetIsBusy(Device.Idiom == TargetIdiom.Phone ? true : false);
            // but timing/responsiveness of above was lacking, so:
            SetIsBusy(true);
            OnPropertyChanged(nameof(IsWebViewSuccess));
            OnPropertyChanged(nameof(IsWebViewFail));
        }

        public void OnNavigated(Object sender, WebNavigatedEventArgs e)
        {
            SetIsBusy(false);
            
            if (e.Result != WebNavigationResult.Success)
            {
                IsWebViewSuccess = false;
            } else
            {
                IsWebViewSuccess = true;
            }
        }
    }
}
