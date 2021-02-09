using DecisionsMobile.Models;
using DecisionsMobile.Models.FormService;

namespace DecisionsMobile.ViewModels
{
    public class FormViewModel : BaseViewModel
    {
        /// <summary>
        /// View Model for showing a single form or page.
        /// </summary>
        public FormViewModel(StandAloneFormSessionInfo formInfo, FlowExecutionStateInstruction instruction, bool canRunOffline = false, string serviceCategoryId = "")
        {
            FormInfo = formInfo;
            Title = instruction.FormTitle ?? instruction.StepName;
            FlowTrackingId = instruction.FlowTrackingId;
            StepTrackingId = instruction.StepTrackingId;
            FlowId = instruction.FlowId;
            CanRunOffline = canRunOffline;
            ServiceCategoryId = serviceCategoryId;
        }

        public StandAloneFormSessionInfo FormInfo { get; set; }

        public string FlowTrackingId { get; set; }

        public string StepTrackingId { get; set; }
        public bool Submitted { get; set; }
        public string FlowId { get; set; }
        public bool CanRunOffline { get; set; }
        public string ServiceCategoryId { get; set; }
    }
}
