using DecisionsMobile.Models;
using DecisionsMobile.Services;

namespace DecisionsMobile.ViewModels
{
    public class AssignmentDetailViewModel : StoreViewModel<Assignment>
    {
        public Assignment Item { get; set; }

        public string DoAssignmentUrl { get; set; }

        public string CreatedOnDate => DateUtils.ToUiString(Item.CreatedOnDate);

        public string DueDate => DateUtils.ToUiString(Item.ExpirationDate);

        public bool HasDueDate => DateUtils.Exists(Item.ExpirationDate);

        public bool HasNotes => !string.IsNullOrEmpty(Item.Notes);

        public bool HasStep => !string.IsNullOrEmpty(Item.StepName);

        public AssignmentDetailViewModel(Assignment item = null)
        {
            Title = "Assignment Details";
            Item = item;
        }
    }
}
