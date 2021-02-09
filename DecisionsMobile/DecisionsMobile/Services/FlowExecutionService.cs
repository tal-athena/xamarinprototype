using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace DecisionsMobile.Services
{
    public class FlowExecutionService
    {

        public static readonly string SERVICE_URL = "FlowExecutionService";

        public static async Task<DataDescription[]> GetFlowInputsOnly(string flowId)
        {
            Uri uri = await RestConstants.GetUriAsync(SERVICE_URL, "GetFlowInputsOnly");

            GetFlowInputsBody body = new GetFlowInputsBody
            {
                FlowId = flowId,
                Context = await AuthService.Instance.GetUserContextAsync(),
            };

            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsync(uri, RestConstants.SerializeBody(body));

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var resultWrapper = JsonConvert.DeserializeObject<GetFlowInputsOnlyWrapper>(responseBody);
                    return resultWrapper.GetFlowInputsOnlyResult;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return new DataDescription[0];
        }

        public static async Task<FlowExecutionStateInstruction> StartFlowWithData(string flowId, DataPair[] data, bool useNamedSession = false)
        {
            if (string.IsNullOrEmpty(flowId))
            {
                Debug.WriteLine("flowId cannot be null or empty");
                return null;
            }

            Uri uri = await RestConstants.GetUriAsync(SERVICE_URL, "StartFlowWithData");
            if (!useNamedSession)
            {
                Session session = await AuthService.Instance.CheckSessionAsync();
            }

            try
            {

                StartFlowWithDataBody requestBody = new StartFlowWithDataBody
                {
                    Data = data,
                    FlowId = flowId,
                    Context = useNamedSession ? new UserContext(RestConstants.NamedSessionId) : await AuthService.Instance.GetUserContextAsync(),
                };

                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsync(uri, RestConstants.SerializeBody(requestBody));
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    //Debug.Write(responseBody);
                    var resultWrapper = JsonConvert.DeserializeObject<StartFlowWithDataWrapper>(responseBody);

                    //var task = OfflineService.Instance.SaveDataAsync($"{uri.AbsolutePath}/{flowId}", resultWrapper);

                    if (!string.IsNullOrEmpty(resultWrapper.StartFlowWithDataResult.ExceptionMessage))
                    {
                        throw new Exception(resultWrapper.StartFlowWithDataResult.ExceptionMessage);
                    }

                    //var task = await OfflineService.Instance.SaveInstructionAsync(flowId, 0, resultWrapper.StartFlowWithDataResult);

                    return resultWrapper.StartFlowWithDataResult;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return null;
        }

        public static async Task<FlowExecutionStateInstruction> GetInstructionsForStep(string flowTrackingId, string stepTrackingId, bool useNamedSession = false)
        {
            Uri uri = await RestConstants.GetUriAsync(SERVICE_URL, "GetInstructionsForStep");

            GetInstructionsForStepBody requestBody = new GetInstructionsForStepBody
            {
                Context = useNamedSession ? new UserContext(RestConstants.NamedSessionId) : await AuthService.Instance.GetUserContextAsync(),
                FlowTrackingId = flowTrackingId,
                StepTrackingId = stepTrackingId
            };

            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsync(uri, RestConstants.SerializeBody(requestBody));

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var resultWrapper = JsonConvert.DeserializeObject<GetInstructionsForStepWrapper>(responseBody);

                    //var task = OfflineService.Instance.SaveDataAsync($"{uri.AbsolutePath}/{flowTrackingId}/{stepTrackingId}", resultWrapper);
                    //var task = OfflineService.Instance.SaveInstructionAsync(resultWrapper.GetInstructionsForStepResult.FlowId, 1, resultWrapper.GetInstructionsForStepResult);
                    return resultWrapper.GetInstructionsForStepResult;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return null;
        }

        public static bool IsShowFormType(FlowExecutionStateInstruction instruction)
        {
            return !String.IsNullOrEmpty(instruction.FormId);
        }
    }

    public class GetFlowInputsBody : ServiceMethodBody
    {
        [JsonProperty("flowId")]
        public string FlowId { get; set; }
    }

    public class DataDescription // TODO
    {

    }

    public class GetFlowInputsOnlyWrapper
    {
        public DataDescription[] GetFlowInputsOnlyResult { get; set; }
    }

    public class StartFlowWithDataBody : ServiceMethodBody
    {

        [JsonProperty("flowID")]
        public string FlowId { get; set; }

        [JsonProperty("data")]
        public DataPair[] Data { get; set; }
    }

    public class StartFlowWithDataWrapper
    {
        public FlowExecutionStateInstruction StartFlowWithDataResult { get; set; }
    }

    public class GetInstructionsForStepBody : ServiceMethodBody
    {
        [JsonProperty("flowTrackingId")]
        public string FlowTrackingId { get; set; }

        [JsonProperty("stepTrackingId")]
        public string StepTrackingId { get; set; }

    }

    class GetInstructionsForStepWrapper
    {
        public FlowExecutionStateInstruction GetInstructionsForStepResult { get; set; }
    }
}
