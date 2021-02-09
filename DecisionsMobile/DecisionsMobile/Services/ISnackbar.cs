using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionsMobile.Services
{
    public interface ISnackbar
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
