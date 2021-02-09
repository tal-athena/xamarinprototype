using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.Models.FormService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DecisionsMobile.Services
{
    public class ServiceCatalogService
    {   
        private static readonly Lazy<ServiceCatalogService> lazy = new Lazy<ServiceCatalogService>
          (() => new ServiceCatalogService());

        public static ServiceCatalogService Instance => lazy.Value;

        public async Task<List<Workflow>> GetAllOfflineCatalogItemsAsync()
        {
            
            var session = await AuthService.Instance.CheckSessionAsync();

            if (!session.IsValid)
            {
                return null;
            }
            using (var httpClient = new HttpClient())
            {
                var uri = await RestConstants.GetUriAsync(RestConstants.WORKFLOW_CATALOG, "GetAllOfflineCatalogItems");

                GetAllOfflineCatalogItemsRequest requestBody = new GetAllOfflineCatalogItemsRequest
                {
                    UserContext = await AuthService.Instance.GetUserContextAsync()
                };

                try
                {
                    List<Workflow> workflows = new List<Workflow>();

                    var response = await httpClient.PostAsync(uri, RestConstants.SerializeBody(requestBody));
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();
                        var resultWrapper = JsonConvert.DeserializeObject<GetAllOfflineCatalogItemsResultWrapper>(body, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        workflows = resultWrapper.GetAllOfflineCatalogItemsResult;
                    }

                    return workflows;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"GetAllOfflineCatalogItemsAsync - {ex.Message}");
                }

            }            
            return null;
        }

        public async Task<IEnumerable<Workflow>> GetAllMobileServiceCatalogItemsAsync()
        {
            IEnumerable<Workflow> workflows = new List<Workflow>();

            var session = await AuthService.Instance.CheckSessionAsync();

            if (!session.IsValid)
            {
                return workflows;
            }
            
            using (HttpClient httpClient = new HttpClient())
            {   
                var uri = await RestConstants.GetUriAsync(RestConstants.WORKFLOW_CATALOG, "GetAllMobileServiceCatalogItems");


                GetAllRequestBody requestBody = new GetAllRequestBody
                {
                    UserContext = await AuthService.Instance.GetUserContextAsync(),
                };

                var postBody = RestConstants.SerializeBody(requestBody);

                var response = await httpClient.PostAsync(uri, postBody);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var converterSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    var resultWrapper = JsonConvert.DeserializeObject<GetAllWorkflowsResult>(body, converterSettings);

                    workflows = resultWrapper.GetAllMobileServiceCatalogItemsResult;
                }
            }
            return workflows;
        }
        public async Task<Dictionary<string, IEnumerable<string>>> GetAllOfflineFormSurfacesAsync()
        {
            var session = await AuthService.Instance.CheckSessionAsync();

            if (!session.IsValid)
            {
                return null;
            }
            /*
             // for now we use REST end-point, but we can use JS end-point in the future
            using (var httpClient = new HttpClient())
            {   
                Dictionary<string, IEnumerable<string>> formData = new Dictionary<string, IEnumerable<string>>();

                //var queryParams = $"sessionId={session.SessionId}&{RestConstants.OUTPUT_TYPE_JSON}";                

                var uri = await RestConstants.GetUriAsync(RestConstants.WORKFLOW_CATALOG, "GetAllOfflineFormSurfaces");

                GetAllOfflineFormSurfacesRequest request = new GetAllOfflineFormSurfacesRequest
                {
                    UserContext = await AuthService.Instance.GetUserContextAsync()
                };

                try
                {
                  
                    var response = await httpClient.PostAsync(uri, RestConstants.SerializeBody(request));
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();
                       
                        var resultWrapper = JsonConvert.DeserializeObject<GetAllOfflineFormSurfacesResultWrapper>(body, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        formData = resultWrapper.GetAllOfflineFormSurfacesResult;
                    }

                    return formData;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"GetAllOfflineFormSurfaces - {ex.Message}");
                }
            }
            
            */
           
            using (var httpClient = new HttpClient())
            {
                Dictionary<string, IEnumerable<string>> formData = new Dictionary<string, IEnumerable<string>>();

                var queryParams = $"sessionId={session.SessionId}&{RestConstants.OUTPUT_TYPE_JSON}";
                var relativeUri = $"{RestConstants.REST_ROOT}/{RestConstants.WORKFLOW_CATALOG}/GetAllOfflineFormSurfaces?{queryParams}";

                var uri = new Uri($"{session.ServerBaseUrl}{relativeUri}");

                try
                {

                    var response = await httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(body);
                        var resultWrapper = JsonConvert.DeserializeObject<GetAllOfflineFormSurfacesResultWrapper>(body, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                        formData = resultWrapper.GetAllOfflineFormSurfacesResult;
                    }

                    return formData;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"GetAllOfflineFormSurfaces - {ex.Message}");
                }
            }

            return null;
        }

        public async Task<DecisionsFormInfoEvent> SubmitOfflineForms(string categoryItemId, List<OfflineFormSubmission> formSubmissions)
        {
            var session = await AuthService.Instance.CheckSessionAsync();

            if (!session.IsValid)
            {
                return null;
            }
            
            using (var httpClient = new HttpClient())
            {

                var uri = await RestConstants.GetUriAsync(RestConstants.WORKFLOW_CATALOG, "SubmitOfflineForms");

                SubmitOfflineFormsRequestBody requestBody = new SubmitOfflineFormsRequestBody
                {
                    UserContext = await AuthService.Instance.GetUserContextAsync(),
                    FormSubmissions = formSubmissions.ToArray(),
                    CatalogItemId = categoryItemId
                };

                try
                {
                    var response = await httpClient.PostAsync(uri, RestConstants.SerializeBody(requestBody));

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var submitOfflineFormsResponse = JsonConvert.DeserializeObject<SubmitOfflineFormsResponse>(responseBody);
                        return FormService.GetNextFormInstruction(submitOfflineFormsResponse.SubmitOfflineFormsResult);
                    }
                    else
                    {
                        Debug.Write(response);
                    }
                } catch (Exception ex)
                {
                    Debug.WriteLine(@"             ERROR {0}", ex.Message);
                }                
            }
            return null;
        }
        
    }
    class GetAllOfflineCatalogItemsRequest
    {
        [JsonProperty("userContext")]
        public UserContext UserContext { get; set; }
    }
    class GetAllOfflineFormSurfacesRequest
    {
        [JsonProperty("userContext")]
        public UserContext UserContext { get; set; }
    }
    class GetAllRequestBody
    {
        [JsonProperty("userContext")]
        public UserContext UserContext { get; set; }
    }
    class SubmitOfflineFormsRequestBody
    {
        [JsonProperty("userContext")]
        public UserContext UserContext { get; set; }

        [JsonProperty("catalogItemId")]
        public string CatalogItemId { get; set; }

        [JsonProperty("formSubmissions")]
        public object[] FormSubmissions { get; set; } // TODO rules
    }
    class SubmitOfflineFormsResponse
    {
        public EventsReturn SubmitOfflineFormsResult { get; set; }
    }
}
