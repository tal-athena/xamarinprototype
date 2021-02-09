using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using System.Collections.Generic;

namespace DecisionsMobile.Services
{
    public static class WorkflowUtil
    {
        public static GetEntityActionsBody GetWorkflowActionBody(Workflow workflow, string sessionId)
        {
            GetEntityActionsBody body = new GetEntityActionsBody();
            body.actionTypes = new List<EntityActionType>();
            body.actionTypes.Add(RestConstants.GetActionType());
            body.sessionid = sessionId;
            body.entityID = workflow.ServiceCatalogId;

            // TODO would be better for this to be provided by dependency, not hard-coded:
            body.typeName = "DecisionsFramework.ServiceLayer.Services.ServiceCatalog.ServiceCatalogItem";
            return body;

        }
    }
}
