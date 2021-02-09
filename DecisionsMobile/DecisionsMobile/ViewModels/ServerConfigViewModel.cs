using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionsMobile.ViewModels
{
    public class ServerConfigViewModel : BaseViewModel
    {
        string serverUri;
        public string ServerUri
        {
            get => serverUri;
            set { serverUri = value; OnPropertyChanged(nameof(ServerUri)); }
        }
    }
}
