using DecisionsMobile.Constants;
using DecisionsMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DecisionsMobile.Services
{
    public class AssignmentStore : IDataStore<Models.Assignment>
    {
        List<Models.Assignment> assignments;
        Session Session = new Session();
        HttpClient httpClient;
        Dictionary<string, string> assignmentUrls;

        public AssignmentStore()
        {
            assignments = new List<Models.Assignment>();
            httpClient = new HttpClient();
            assignmentUrls = new Dictionary<string, string>();
        }

        public async Task<bool> AddItemAsync(Models.Assignment item)
        {
            assignments.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Models.Assignment item)
        {
            var oldItem = assignments.Where((Assignment arg) => arg.AssignmentId == item.AssignmentId).FirstOrDefault();
            assignments.Remove(oldItem);
            assignments.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = assignments.Where((Assignment arg) => arg.AssignmentId == id).FirstOrDefault();
            assignments.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Assignment> GetItemAsync(string id)
        {
            return await Task.FromResult(assignments.FirstOrDefault(s => s.AssignmentId == id));
        }

        public async Task<IEnumerable<Assignment>> GetItemsAsync(bool forceRefresh = false)
        {
            Session = await AuthService.Instance.CheckSessionAsync();

            if (!Session.IsValid)
            {
                return await Task.FromResult(assignments);
            }
            
            var queryParams = $"sessionId={Session.SessionId}&{RestConstants.OUTPUT_TYPE_JSON}";
            var uri = new Uri($"{Session.ServerBaseUrl}{RestConstants.REST_ROOT}/Assignment/GetMyCurrentAssignments?{queryParams}");

            try
            {
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var converterSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    var resultWrapper = JsonConvert.DeserializeObject<GetMyCurrentAssignmentsResultWrapper>(body, converterSettings);

                    var result = resultWrapper.GetMyCurrentAssignmentsResult;


                    assignments = new List<Assignment>(result);
                    // sort by created date :
                    assignments.Sort((a, b) => a.CreatedOnDate.CompareTo(b.CreatedOnDate));

                    return assignments;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return await Task.FromResult(assignments);
        }

        /// Added this thinking I couldn't know what the URLs would be, client side, but
        /// upon realizing it was really just an assignment ID query param, decided this
        /// overhead wasn't worth it. Leaving this in, in case something else does require
        /// reliance on the server for such things.
        public async Task<string> GetItemActionUrlAsync(Assignment assignment)
        {
            var id = assignment.AssignmentId;
            // return it from map if we have it:
            if (assignmentUrls.TryGetValue(id, out string url))
            {
                return await Task.FromResult(url);
            }

            Session = await AuthService.Instance.CheckSessionAsync();
            if (!Session.IsValid)
            {
                return await Task.FromResult("");
            }

            var queryParams = $"assignmentId={id.ToString()}&sessionId={Session.SessionId}&{RestConstants.OUTPUT_TYPE_JSON}";
            var uri = new Uri($"{Session.ServerBaseUrl}{RestConstants.REST_ROOT}/Assignment/GetDoAssignmentUrl?{queryParams}");

            try
            {
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var resultWrapper = JsonConvert.DeserializeObject<GetDoAssignmentUrlResultWrapper>(body);
                    url = resultWrapper.GetDoAssignmentUrlResult;

                    assignmentUrls.Add(id, body);
                    return url;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return await Task.FromResult("");
        }
    }
}