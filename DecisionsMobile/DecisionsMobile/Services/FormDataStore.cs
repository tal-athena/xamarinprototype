using DecisionsMobile.Models.FormService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DecisionsMobile.Services
{
    public class FormDataStore : IDataStore<StandAloneFormSessionInfo>
    {

        List<StandAloneFormSessionInfo> infoList;
        Dictionary<string, StandAloneFormSessionInfo> infoDict;

        public FormDataStore()
        {
            infoList = new List<StandAloneFormSessionInfo>();
            infoDict = new Dictionary<string, StandAloneFormSessionInfo>();
        }

        public Task<bool> AddItemAsync(StandAloneFormSessionInfo item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetItemActionUrlAsync(StandAloneFormSessionInfo item)
        {
            throw new NotImplementedException();
        }

        public async Task<StandAloneFormSessionInfo> GetItemAsync(string id)
        {
            infoDict.TryGetValue(id, out StandAloneFormSessionInfo value);
            return await Task.FromResult(value);
        }

        public StandAloneFormSessionInfo GetItemByIndex(int index = 0)
        {
            return infoList[index];
        }

        public StandAloneFormSessionInfo GetGridSample()
        {
            StandAloneFormSessionInfo info;
            try
            {
                string body = "TODO";
                var converterSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                info = JsonConvert.DeserializeObject<StandAloneFormSessionInfo>(body, converterSettings);

                return info;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return null;
        }

        public async Task<IEnumerable<StandAloneFormSessionInfo>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                string body = "TODO";
                var converterSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                var info = JsonConvert.DeserializeObject<StandAloneFormSessionInfo>(body, converterSettings);

                if (!infoList.Contains(info))
                {
                    infoList.Add(info);
                }
                if (!infoDict.ContainsKey(info.FormSessionInfoId))
                {
                    infoDict.Add(info.FormSessionInfoId, info);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return await Task.FromResult(infoList);
        }

        public Task<bool> UpdateItemAsync(StandAloneFormSessionInfo item)
        {
            throw new NotImplementedException();
        }

    }

}


