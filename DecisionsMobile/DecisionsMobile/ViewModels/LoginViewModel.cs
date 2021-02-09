using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.Services;
using System.Collections.Generic;
using System.Drawing;

namespace DecisionsMobile.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        Session session;
        string feedbackMessage = "";
        Color feedbackColor;
        bool isValid = true;

        public LoginViewModel(Session session)
        {
            this.Session = session;
            session.ServerName = "";
        }

        public Session Session
        {
            get => session;
            set
            {
                SetProperty(ref session, value);
                FeedbackMessage = value.FeedbackMessage;
                OnPropertyChanged(nameof(Session));
                OnPropertyChanged(nameof(ShowFeedback));
            }
        }

        public string FeedbackMessage
        {
            get => feedbackMessage;
            set
            {
                SetProperty(ref feedbackMessage, value);
                OnPropertyChanged(nameof(ShowFeedback));
            }
        }

        public Color FeedbackColor
        {
            get => feedbackColor;
            set
            {
                SetProperty(ref feedbackColor, value);
                OnPropertyChanged(nameof(FeedbackColor));
            }
        }

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                SetProperty(ref isValid, value);
                OnPropertyChanged(nameof(IsReady)); // beacause it's a derived value.
                OnPropertyChanged(nameof(ReadyForLogin));
            }
        }


        public bool HasServerNameAndUrl
        {
            get
            {
                // original conception allowed configuration of multiple servers to log into
                // MVP supports only one. If concept changes, rename this method, for now
                // it simply ignores serverName
                return /* !string.IsNullOrEmpty(session.ServerName) && */ !session.ServerUriIsEmpty();
            }
        }

        public void SetServerName(string serverName)
        {
            session.ServerName = serverName;
            OnPropertyChanged(nameof(HasServerNameAndUrl));
        }

        public void SetServerUri(string serverUri)
        {
            
            session.ServerUri = serverUri.TrimEnd('/');
            
            OnPropertyChanged(nameof(HasServerNameAndUrl));
        }
        
        public string ServerUri
        {
            get
            {
                return session.ServerUri;
            }
            set
            {
                session.ServerUri = value;
                OnPropertyChanged(nameof(ServerUri));
            }
        }
        public string DefaultServerUri => RestConstants.DEFAULT_SERVER_URI;
        public ICollection<string> ServerNames
        {
            // to populate a picker of servers, if we actually need that.
            get => AuthService.Instance.Sessions.Keys;
        }

        public bool ReadyForLogin
        {
            get { return IsReady && IsValid; }
        }


        public void HideErrorMessage()
        {
            FeedbackMessage = null;
            OnPropertyChanged(nameof(ShowFeedback));
        }

        public bool ShowFeedback
        {
            get => !string.IsNullOrEmpty(FeedbackMessage);
        }
        public string PasswordConfirm { get; set; }

        public bool IsFirstTime => session.IsEmpty;
    }

}
