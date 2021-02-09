using DecisionsMobile.Constants;
using System.Collections.Generic;

namespace DecisionsMobile.Models
{
    public class GetEntityActionsBody
    {
        public List<EntityActionType> actionTypes;
        public string sessionid;
        public string entityID;
        public string outputtype = "Json";
        public string typeName;
    }
}
