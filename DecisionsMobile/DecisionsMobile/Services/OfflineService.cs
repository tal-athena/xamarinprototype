using DecisionsMobile.Constants;
using DecisionsMobile.Helper;
using DecisionsMobile.Models;
using DecisionsMobile.Models.FormService;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DecisionsMobile.Services
{
    class OfflineService
    {

        readonly SQLiteAsyncConnection _database;

        private static readonly Lazy<OfflineService> lazy = new Lazy<OfflineService>
           (() => new OfflineService());

        public static OfflineService Instance => lazy.Value;

        IFileService fileService = DependencyService.Get<IFileService>();

        object locker = new object();
        public OfflineService()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fileName = Path.Combine(basePath, DatabaseConstants.DatabaseFilename);

            _database = new SQLiteAsyncConnection(fileName);

            _database.CreateTableAsync<StoredWorkflow>().Wait();
            _database.CreateTableAsync<StoredOfflineFormSubmission>().Wait();

        }

        public async Task<List<StoredOfflineFormSubmission>> GetStoredOfflineFormSubmissions()
        {
            return await _database.Table<StoredOfflineFormSubmission>().ToListAsync();
        }

        public Workflow GetWorkflowByServiceCategoryId(string serviceCategoryId)
        {
            lock (locker)
            {
                var storedWorkFlowTask = _database.Table<StoredWorkflow>().FirstOrDefaultAsync(x => x.ServiceCategoryId == serviceCategoryId);
                storedWorkFlowTask.Wait();
                var storedWorkFlow = storedWorkFlowTask.Result;
                if (storedWorkFlow == null)
                    return null;
                return JsonConvert.DeserializeObject<Workflow>(storedWorkFlow.WorkFlow);
            }
        }

        public async Task SubmitPendingSubmissionsAsync()
        {
            try
            {
                var pendingList = await _database.Table<StoredOfflineFormSubmission>().ToListAsync();
                Debug.WriteLine($"SubmitPendingSubmissionsAsync - {pendingList.Count} forms are submitting");
                foreach (var pendingSubmission in pendingList)
                {
                    if (pendingSubmission.IsFailed == 0)
                        await SubmitOfflineFormAsync(pendingSubmission.Id.Value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"               SubmitPendingSubmissionsAsync exception - {ex.Message}");
            }
        }

        public async Task<bool> SubmitOfflineFormAsync(int submissionId)
        {
            var offlineSubmission = await _database.Table<StoredOfflineFormSubmission>().Where(x => x.Id == submissionId).FirstOrDefaultAsync();
            
            if (offlineSubmission == null)
                return false;

            //TODO submit

            var categoryItemId = offlineSubmission.ServiceCategoryId;
            List<OfflineFormSubmission> offlineForms = JsonConvert.DeserializeObject<List<OfflineFormSubmission>>(offlineSubmission.OfflineFormSubmissions);

            Workflow workflow = GetWorkflowByServiceCategoryId(categoryItemId);

            DecisionsFormInfoEvent formEvent = await ServiceCatalogService.Instance.SubmitOfflineForms(categoryItemId, offlineForms);

            if (formEvent != null && formEvent.IsFlowCompletedInstructionEvent())
            {
                NotificationService.Instance.ScheduleNotification("Form Submitted", $"{workflow.EntityName} submitted successfully");
                await _database.DeleteAsync(offlineSubmission);
                return true;
            }
            else
            {
                NotificationService.Instance.ScheduleNotification("Submission Failed", $"Problem submitting {workflow.EntityName}");
                //for failed case
                offlineSubmission.IsFailed = 1;
                var cnt = await _database.UpdateAsync(offlineSubmission);
                return false;
            }
        }

        public async Task<List<StandAloneFormSessionInfo>> GetStandAloneFormSessionInfoAsync(string serviceCatalogId)
        {
            var storedWorkFlow = await _database.Table<StoredWorkflow>().FirstOrDefaultAsync(x => x.ServiceCategoryId == serviceCatalogId);

            if (storedWorkFlow == null)
                return null;
            var infos = JsonConvert.DeserializeObject<List<string>>(storedWorkFlow.StandAloneFormSessionInfo);

            List<StandAloneFormSessionInfo> ret = new List<StandAloneFormSessionInfo>();
            foreach (var info in infos)
            {
                // we need $type field to specify component type in form data
                // NewtonJson can't deserialize field which starts with '$', so we needed to replace it to get correct deserialization.
                var replacedInfo = info.Replace("$type", "__type");
                ret.Add(JsonConvert.DeserializeObject<StandAloneFormSessionInfo>(replacedInfo, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            }
            return ret;
        }

        public async Task<bool> DiscardStoredFormSubmission(int storedFormSubmissionId)
        {
            return await _database.Table<StoredOfflineFormSubmission>().Where(x => x.Id == storedFormSubmissionId).DeleteAsync() > 0;
        }

        public void SaveAllWorkflowsAsync(List<Workflow> workflows, Dictionary<string, IEnumerable<string>> formData)
        {
            lock (locker)
            {
                _database.DeleteAllAsync<StoredWorkflow>().Wait();

                List<StoredWorkflow> list = new List<StoredWorkflow>();
                workflows.ForEach(x =>
                {
                    IEnumerable<string> standFormInfos = new List<string>();
                    if (formData.TryGetValue(x.ServiceCatalogId, out standFormInfos))
                    {
                        list.Add(new StoredWorkflow
                        {
                            WorkFlow = JsonConvert.SerializeObject(x, Formatting.None, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }),
                            ServiceCategoryId = x.ServiceCatalogId,
                            StandAloneFormSessionInfo = JsonConvert.SerializeObject(standFormInfos, Formatting.None, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            })
                        });
                    }
                });
                _database.InsertAllAsync(list).Wait();
            }
        }
        public async Task<List<Workflow>> GetAllworkflowsAsync()
        {
            var storedWorkflows = await _database.Table<StoredWorkflow>().ToListAsync();
            List<Workflow> workflows = new List<Workflow>();
            storedWorkflows.ForEach(x =>
            {
                workflows.Add(JsonConvert.DeserializeObject<Workflow>(x.WorkFlow));
            });
            return workflows;
        }

        public async Task LoadAllWorkflowsFromServerAsync()
        {
            var workflowsTask = ServiceCatalogService.Instance.GetAllOfflineCatalogItemsAsync();
            var formDataTask = ServiceCatalogService.Instance.GetAllOfflineFormSurfacesAsync();

            await Task.WhenAll(workflowsTask, formDataTask);

            if (workflowsTask.Result != null && formDataTask.Result != null)
            { 
                SaveAllWorkflowsAsync(workflowsTask.Result, formDataTask.Result);
            }
        }
        private async Task CacheImageAsync(ImageInfo imageInfo)
        {
            string imageUrl = ImageHelper.GetImageInfoUrl(imageInfo);
            HttpClient httpClient = new HttpClient();

            if (!imageUrl.Trim().StartsWith("https", StringComparison.OrdinalIgnoreCase))
                return;

            try
            {
                byte[] data = await httpClient.GetByteArrayAsync(imageUrl);

                var filePath = fileService.SaveFile(generateNameFromUrl(imageUrl), data);
                imageInfo.ImageUrl = filePath;
                imageInfo.ImageType = (long)ImageInfoType.Url;

            } catch (Exception ex)
            {
                Debug.WriteLine($"               CacheImageAsync exception - {ex.Message}");
            }
        }
        private string generateNameFromUrl(string url)
        {
            // Replace useless chareacters with UNDERSCORE
            string uniqueName = url.Replace("://", "_").Replace(".", "_").Replace("/", "_");
            // Replace last UNDERSCORE with a DOT
            uniqueName = uniqueName.Substring(0, uniqueName.LastIndexOf('_'))
                    + "." + uniqueName.Substring(uniqueName.LastIndexOf('_') + 1, uniqueName.Length);
            return uniqueName;
        }

        public async Task<int> SaveOfflineFormSubmission(string serviceCategoryId, DataPair[] data, string outcomeName, object[] formRules)
        {
            List<OfflineFormSubmission> formSubmissions = new List<OfflineFormSubmission>();

            formSubmissions.Add (new OfflineFormSubmission
            {
                ControlsData = data,
                OutcomeName = outcomeName,
                RuleSessionInfos = formRules
            });
            StoredOfflineFormSubmission entity = new StoredOfflineFormSubmission
            {
                ServiceCategoryId = serviceCategoryId,
                OfflineFormSubmissions = JsonConvert.SerializeObject(formSubmissions, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }),
                CreatedAt = DateTime.Now,
                IsFailed = 0
            };
            return await _database.InsertAsync(entity);
        }
    }

}
