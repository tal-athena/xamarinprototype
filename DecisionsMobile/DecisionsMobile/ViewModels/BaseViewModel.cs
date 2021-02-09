using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace DecisionsMobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        bool isBusy = false;

        public bool IsBusy => isBusy;

        public virtual void SetIsBusy(bool value)
        {
            SetProperty(ref isBusy, value, nameof(IsBusy));
            OnPropertyChanged(nameof(IsReady));
        }

        public bool IsReady => !isBusy;

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public BaseViewModel() { }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Imperative method for showing the settings page,
        /// as a temporary step towards having a true app-wide
        /// hamburger menu, etc.
        /// 
        /// The goal here is for any view to be able to pass the same message
        /// for the MainPage to handle by showing settings as a modal.
        /// </summary>
        public void ShowSettingsPage()
        {
            MessagingCenter.Send(this, Message.SHOW_SETTINGS);
        }

        public void ShowLogin(Session session)
        {
            MessagingCenter.Send(this, Message.SHOW_LOGIN, session);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }
}
