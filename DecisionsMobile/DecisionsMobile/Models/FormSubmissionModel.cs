using System;
using Xamarin.Forms;

namespace DecisionsMobile.Models
{
    public class FormSubmissionModel
    {
        public int StoredFormSubmissionId { get; set; }
        public string FlowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string WorkFlowName { get; set; }
        public bool IsFailed { get; set; }

        public string StatusString
        {
            get
            {
                if (IsFailed) return "Failed";
                else return "Pending";
            }
        }
        public Color StatusColor
        {
            get
            {
                if (IsFailed == false)
                    return (Color)Application.Current.Resources["TextColor"];
                else return (Color)Application.Current.Resources["Warn"];
            }
        }
    }
}
