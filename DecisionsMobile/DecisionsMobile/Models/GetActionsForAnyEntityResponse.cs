using System.Collections.Generic;

namespace DecisionsMobile.Models
{
    public class GetActionsForAnyEntityResponse
    {
        public IEnumerable<BaseActionType> GetActionsForAnyEntityResult { get; set; }
    }
}
