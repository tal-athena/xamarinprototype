using DecisionsMobile.Models;
using DecisionsMobile.Services;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DecisionsMobile.Constants
{
    // TODO rename as "helpers" or "utils" since that's how it's being used, now.
    public static class RestConstants
    {
        public static readonly string DEFAULT_SERVER_URI = "mobile-app.decisions.com";
        public static readonly string JSON_HEADER = "application/json";
        // TODO MT URLs?
        public static readonly string API_ROOT = "/Primary";
        public static string MOBILE_API_ROOT
        {
            get
            {
                // If Idiom isn't phone, it's presumed to be tablet.
                // var deviceSizePath = Device.Idiom == TargetIdiom.Phone ? "M" : "T";
                // per Khuzema's input, I'm uncertain whether 
                return $"";
            }
        }
        public static readonly string REST_ROOT = $"{API_ROOT}/REST";

        /// <summary>
        /// Query parameter for JSON output type.
        /// </summary>
        public static readonly string OUTPUT_TYPE_JSON = "outputType=JSON";
        public static readonly string CHROME = "Chrome=";
        public static readonly string CHROMELESS = $"{CHROME}Off";
        public static readonly string LOCATION_CENTER = "Location=Center";
        public static readonly string WORKFLOW_CATALOG = "ServiceCatalogService";
        public static readonly string FOLDER_SERVICE = "FolderService";
        public static readonly string GET_ACTIONS_FOR_ENTITY = $"{FOLDER_SERVICE}/GetActionsForAnyEntity";

        public static readonly string FLOW_ID = "FlowId";

        public static readonly string ASSIGNMENT_ID = "assignmentId";


        public static readonly string CreateAccountFlowId = "030a1414-2eac-11eb-bbfe-0022481c891e";
        public static readonly string NamedSessionId = "NS-f97cd8dc-332b-11eb-bbfe-0022481c891e";

        public static string AddJsonOutputTypeQueryParam(string uri)
        {
            return uri + "&" + OUTPUT_TYPE_JSON;
        }

        public static string AppendChromeless(string uri)
        {
            return $"{uri}&{CHROMELESS}";
        }

        public static string AppendLocationCenter(string uri)
        {
            return $"{uri}&{LOCATION_CENTER}";
        }

        public static string AppendEmbedFlowParams(string uri)
        {
            return AppendChromeless($"{uri}&ForceFormat=true&Location=Maximized");
        }

        public static EntityActionType GetActionType()
        {
            return Device.Idiom == TargetIdiom.Phone ? EntityActionType.MvcMobile : EntityActionType.MvcTablet;
        }

        public static string GetServiceUri(string serviceUrl, string endPoint)
        {
            // "js" here for JSON, vs XML:
            return $"{API_ROOT}/API/{serviceUrl}/js/{endPoint}";
        }

        public static async Task<Uri> GetUriAsync(string serviceUrl, string endPoint)
        {  
            var session = await AuthService.Instance.CheckSessionAsync();
            string uriString = RestConstants.GetServiceUri(serviceUrl, endPoint);
            Debug.WriteLine($"service URI String: {uriString}");
            return new Uri($"{session.ServerBaseUrl}{uriString}");
        }


        public static StringContent SerializeBody(object value)
        {
            var json = JsonConvert.SerializeObject(value, Formatting.None);
            return new StringContent(json, Encoding.UTF8, RestConstants.JSON_HEADER);
        }

        // TODO dry up service method calls if possible... if they all serialize "sessionId" with different 
        // casing then might not be possible, though.
        // public static async Task<T> MakeServiceCall<T, B, R>(string serviceUrl, string endPoint, B requestBody) {
        //     Uri uri = await RestConstants.GetUriAsync(serviceUrl, "GetFormSessionSurfaceJson");
        //     Session session = await AuthService.Instance.CheckSessionAsync();

        //     FormServiceRequestBody requestBody = new FormServiceRequestBody
        //     {
        //         SessionId = session.SessionId,
        //         FormSessionInfoId = formSessionInfoId
        //     };

        //     try
        //     {
        //         HttpClient httpClient = new HttpClient();
        //         var response = await httpClient.PostAsync(uri, RestConstants.SerializeBody(requestBody));

        //         if (response.IsSuccessStatusCode)
        //         {
        //             var responseBody = await response.Content.ReadAsStringAsync();
        //             var resultWrapper = JsonConvert.DeserializeObject<StartFlowWithDataWrapper>(responseBody);
        //             return resultWrapper.StartFlowWithDataResult;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Debug.WriteLine(@"             ERROR {0}", ex.Message);
        //     }
        //     return null;
        // }

    }

    public class ServiceMethodBody
    {
        // this might not make sense. seems services are inconsistent about what they call this...
        [JsonProperty("context")]
        public UserContext Context { get; set; }

        [JsonProperty("outputtype")]
        public string OutputType { get; set; } = "Json";
    }
}
