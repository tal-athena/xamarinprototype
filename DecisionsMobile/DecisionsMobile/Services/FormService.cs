using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using DecisionsMobile.Models.FormService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DecisionsMobile.Services
{
    public class FormService
    {

        public static readonly string SERVICE_URL = "FormService";

        public FormService()
        {

        }

        // TODO dry these methods up with a good generic...

        public static async Task<StandAloneFormSessionInfo> GetFormSessionSurfaceJson(FlowExecutionStateInstruction instruction, bool useNamedSession = false)
        {
            Uri uri = await RestConstants.GetUriAsync(SERVICE_URL, "GetFormSessionSurfaceJson");

            try
            {
                FormServiceRequestBody requestBody = new FormServiceRequestBody
                {
                    UserContext = useNamedSession ? new UserContext(RestConstants.NamedSessionId) : await AuthService.Instance.GetUserContextAsync(),
                    FormSessionInfoId = instruction.FormSessionId
                };

                var serializedBody = RestConstants.SerializeBody(requestBody);
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsync(uri, serializedBody);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var resultWrapper = JsonConvert.DeserializeObject<GetFormSessionSurfaceJsonResponse>(responseBody);
                    // we need $type field to specify component type in form data
                    // NewtonJson can't deserialize field which starts with '$', so we needed to replace it to get correct deserialization.
                    resultWrapper.GetFormSessionSurfaceJsonResult = resultWrapper.GetFormSessionSurfaceJsonResult.Replace("$type", "__type");
                    StandAloneFormSessionInfo info = JsonConvert.DeserializeObject<StandAloneFormSessionInfo>(
                        resultWrapper.GetFormSessionSurfaceJsonResult);

                    return info;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return null;
        }

        public static async Task<DecisionsFormInfoEvent> SelectPath(string formSessionInfoId, string outcomeName, DataPair[] controlsData, bool useNamedSession = false)
        {
            Uri uri = await RestConstants.GetUriAsync(SERVICE_URL, "SelectPath");

            SelectPathRequestBody requestBody = new SelectPathRequestBody
            {
                UserContext = useNamedSession ? new UserContext(RestConstants.NamedSessionId) : await AuthService.Instance.GetUserContextAsync(),
                FormSessionInfoId = formSessionInfoId,
                OutcomeName = outcomeName,
                ControlsData = controlsData,
                RuleSessionInfos = new object[0] // TODO rules
            };

            var postBody = RestConstants.SerializeBody(requestBody);
            Debug.Write(postBody);

            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsync(uri, postBody);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var resultWrapper = JsonConvert.DeserializeObject<SelectPathResponse>(responseBody);
                    return GetNextFormInstruction(resultWrapper.SelectPathResult);
                }
                else
                {
                    Debug.Write(response);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return null;
        }

        public static async Task<object> KeepFormAlive(string formSessionInfoId)
        {
            Uri uri = await RestConstants.GetUriAsync(SERVICE_URL, "KeepFormAlive");

            FormServiceRequestBody requestBody = new FormServiceRequestBody
            {
                UserContext = await AuthService.Instance.GetUserContextAsync(),
                FormSessionInfoId = formSessionInfoId
            };

            var postBody = RestConstants.SerializeBody(requestBody);

            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsync(uri, postBody);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var resultWrapper = JsonConvert.DeserializeObject<KeepFormAliveResponse>(responseBody);
                    return resultWrapper.KeepFormAliveResult;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return null;
        }

        public static async Task<object> FormLoadComplete(string formSessionInfoId)
        {
            Uri uri = await RestConstants.GetUriAsync(SERVICE_URL, "FormLoadComplete");

            FormServiceRequestBody requestBody = new FormServiceRequestBody
            {
                UserContext = await AuthService.Instance.GetUserContextAsync(),
                FormSessionInfoId = formSessionInfoId
            };

            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsync(uri, RestConstants.SerializeBody(requestBody));

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var resultWrapper = JsonConvert.DeserializeObject<FormLoadCompleteResponse>(responseBody);
                    return resultWrapper.FormLoadCompleteResult;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return null;
        }

        public static DecisionsFormInfoEvent GetNextFormInstruction(EventsReturn events)
        {
            var first = events.Events[0];
            // some more JSON deserialization of "object" to do here...
            var valueString = JsonConvert.SerializeObject(first.OutputValue);
            Debug.WriteLine(valueString);
            var inner = JsonConvert.DeserializeObject<FormInfoServerEventWrapper>(valueString);
            return inner.Events[0] as DecisionsFormInfoEvent;
        }

        public static async void FormClosed(string flowTrackingId, string stepTrackingId)
        {
            Uri uri = await RestConstants.GetUriAsync(SERVICE_URL, "FormClosed");

            FormClosedBody requestBody = new FormClosedBody
            {
                UserContext = await AuthService.Instance.GetUserContextAsync(),
                FlowTrackingId = flowTrackingId,
                StepTrackingId = stepTrackingId
            };

            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsync(uri, RestConstants.SerializeBody(requestBody));

                if (response.IsSuccessStatusCode)
                {
                    // TODO logging
                    Debug.WriteLine("Problem closing form on server:", response);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
        }
        public static bool Validate(Dictionary<string, FormControlWrapper> controls, string outcomePathName, out string validationMessage)
        {
            validationMessage = "";

            bool isValidated = true;
            foreach (var control in controls)
            {
                string controlDataName;
                if (control.Value.IsEmpty(out controlDataName) && control.Value.ElementData.Child.RequiredOnOutputs.Contains(outcomePathName))
                {
                    isValidated = false;
                    control.Value.SetValidation(BreakLevel.Warning);
                    if (control.Value.ElementData.Child.OverrideRequiredMessage)
                    {
                        validationMessage += $"'{controlDataName}' {control.Value.ElementData.Child.RequiredMessage}.\n\n";
                    } else
                    {
                        validationMessage += $"'{controlDataName}' must be specified.\n\n";
                    }
                    
                }
            }
            return isValidated;
        }

        class FormServiceRequestBody
        {
            [JsonProperty("userContext")]
            public UserContext UserContext { get; set; }

            [JsonProperty("formSessionInfoId")]
            public string FormSessionInfoId { get; set; }
        }

        class SelectPathRequestBody : FormServiceRequestBody
        {
            [JsonProperty("controlsData")]
            public DataPair[] ControlsData { get; set; }

            [JsonProperty("ruleSessionInfos")]
            public object[] RuleSessionInfos { get; set; } // TODO rules

            [JsonProperty("outcomeName")]
            public string OutcomeName { get; set; }
        }

        class GetFormSessionSurfaceJsonResponse
        {
            public string GetFormSessionSurfaceJsonResult { get; set; }
        }

        class SelectPathResponse
        {
            public EventsReturn SelectPathResult { get; set; }
        }

        class KeepFormAliveResponse
        {
            public object KeepFormAliveResult { get; set; }
        }

        class FormLoadCompleteResponse
        {
            public object FormLoadCompleteResult { get; set; }
        }

        class FormClosedBody
        {
            [JsonProperty("userContext")]
            public UserContext UserContext { get; set; }

            [JsonProperty("flowTrackingId")]
            public string FlowTrackingId { get; set; }
            [JsonProperty("stepTrackingId")]
            public string StepTrackingId { get; set; }

        }
    }
}
