using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DecisionsMobile.Services
{
    class WorkflowStore : IDataStore<Models.Workflow>
    {
        List<Models.Workflow> workflows = new List<Models.Workflow>();
        HttpClient httpClient = new HttpClient();
        Dictionary<string, WorkFlowAction> workflowActions = new Dictionary<string, WorkFlowAction>();

        public Task<bool> AddItemAsync(Workflow item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Workflow> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        // TODO DRY this up between this store and the one for assignments.
        public async Task<IEnumerable<Workflow>> GetItemsAsync(bool forceRefresh = false)
        {  
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    Task offlineFomLoadTask = OfflineService.Instance.LoadAllWorkflowsFromServerAsync();

                    IEnumerable<Workflow> workflows = await ServiceCatalogService.Instance.GetAllMobileServiceCatalogItemsAsync();

                    Task task = LoadWorkflowActionsAsync(workflows); // use another thread, but don't wait for this.

                    return workflows;
                  
                }
                else
                {
                    workflows = await OfflineService.Instance.GetAllworkflowsAsync();
                    if (workflows == null)
                        workflows = new List<Workflow>();
                    return workflows;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }

            return await Task.FromResult(workflows);
        }

        private async Task LoadWorkflowActionsAsync(IEnumerable<Workflow> workflows)
        {
            foreach (var workflow in workflows)
            {
                await GetWorkflowActionAsync(workflow);
            }
        }

        public async Task<WorkFlowAction> GetWorkflowActionAsync(Workflow workflow)
        {
            if (workflowActions.TryGetValue(workflow.ServiceCatalogId, out WorkFlowAction action))
            {
                Debug.WriteLine("returning cached workflow action");
                return action;
            }

            Session session = await AuthService.Instance.CheckSessionAsync();
            if (!session.IsValid)
            {
                return null;
            }

            Debug.WriteLine("fetching workflow action");            
            var relativePath = $"{RestConstants.REST_ROOT}/{RestConstants.GET_ACTIONS_FOR_ENTITY}";
            var uri = new Uri($"{session.ServerBaseUrl}{relativePath}");

            try
            {
                GetEntityActionsBody body = WorkflowUtil.GetWorkflowActionBody(workflow, session.SessionId);
                var json = JsonConvert.SerializeObject(body);
                StringContent content = new StringContent(json, Encoding.UTF8, RestConstants.JSON_HEADER);

                var response = await httpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var resultWrapper = JsonConvert.DeserializeObject<GetActionsForAnyEntityResponse>(responseBody);
                    var actions = resultWrapper.GetActionsForAnyEntityResult;
                    WorkFlowAction newAction = GetActionForWorkflow(actions, session);
                    Debug.WriteLine("fetched workflow action");
                    Debug.Write(action);
                    workflowActions.Add(workflow.ServiceCatalogId, newAction);
                    return newAction;
                } else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseBody);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return null;
        }

        private static WorkFlowAction GetActionForWorkflow(IEnumerable<BaseActionType> actions, Session session)
        {
            WorkFlowAction workflowAction = new WorkFlowAction();
            // try for flow:
            IEnumerable<BaseActionType> mainActions = actions.Where((action, i) => action.MvcActionHandler.HandlerName.Equals(HandlerNames.RUN_FLOW));
            if (mainActions.Count() > 0)
            {
                var strings = from mainAction in mainActions select mainAction.ElementId;
                string flowId = strings.First();
                workflowAction.RunFlowId = flowId;
                // TODO may not need the below, but leaving for now just in case:
                workflowAction.WebViewUrl = GetWorkflowActionFlowUrl(flowId, session);
            }

            // try for URL:
            IEnumerable<BaseActionType> urlActions = actions.Where((action, i) => action.MvcActionHandler.HandlerName.Equals(HandlerNames.OPEN_URL));
            workflowAction.WebViewUrl = urlActions.Count() > 0 ? urlActions.First().Url : "";

            return workflowAction;
        }

        public static string GetWorkflowActionFlowUrl(string flowId, Session session)
        {
            // e.g. 
            // http://localhost/decisions/Primary/H/?FlowId=ad129170-5fc1-11e9-b51f-802bf99b288c&ForceFormat=true&Location=Maximized&Chrome=Off
            var url = $"{session.ServerBaseUrl}{RestConstants.MOBILE_API_ROOT}/"
                + $"?{RestConstants.FLOW_ID}={flowId}";
            url = AuthService.Instance.AppendSessionId(url);
            url = RestConstants.AppendEmbedFlowParams(url);
            return url;
        }

        public Task<bool> UpdateItemAsync(Workflow item)
        {
            throw new NotImplementedException();
        }

        async Task<string> IDataStore<Workflow>.GetItemActionUrlAsync(Workflow item)
        {
            WorkFlowAction action = await GetWorkflowActionAsync(item);
            return action.WebViewUrl;
        }

        public class WorkFlowAction
        {
            public string WebViewUrl { get; set; }

            public string RunFlowId { get; set; }
        }

        // Start Flow
        // Check flow instruction
        // if it's a form, get FormSurface JSON
        // TODO if it's not a form, show modal that the flow was started 
        // 
    }
}
