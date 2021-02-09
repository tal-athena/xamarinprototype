using DecisionsMobile.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DecisionsMobile.Models
{
    public class Workflow
    {
        [JsonProperty("ImageInfo")]
        public ImageInfo ImageInfo { get; set; }

        [JsonIgnore]
        public ImageSource ImageSource => ImageHelper.GetImageSourceFromBase64EncodedUrl(ImageInfo.UrlEncodedImage);

        [JsonProperty("ShowOnMobile")]
        public bool ShowOnMobile { get; set; }

        [JsonProperty("CanRunOffline")]
        public bool CanRunOffline { get; set; }

        [JsonProperty("CanSuggestToUser")]
        public bool CanSuggestToUser { get; set; }

        [JsonProperty("HandlerData")]
        public object HandlerData { get; set; }

        [JsonProperty("IconName")]
        public object IconName { get; set; }

        [JsonProperty("CatalogItemOrder")]
        public long CatalogItemOrder { get; set; }

        [JsonProperty("Color")]
        public object Color { get; set; }

        [JsonProperty("ReportFolderId")]
        public object ReportFolderId { get; set; }

        [JsonProperty("ServiceCatalogId")]
        public string ServiceCatalogId { get; set; }

        [JsonProperty("ServiceCost")]
        public long ServiceCost { get; set; }

        [JsonProperty("IsCostAproximate")]
        public bool IsCostAproximate { get; set; }

        [JsonProperty("ChargeModel")]
        public long ChargeModel { get; set; }

        [JsonProperty("ExpectedDaysToComplete")]
        public long ExpectedDaysToComplete { get; set; }

        [JsonProperty("Hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("AdministratorViewOnly")]
        public bool AdministratorViewOnly { get; set; }

        [JsonProperty("EntityFolderID")]
        public string EntityFolderId { get; set; }

        [JsonProperty("HistoryFolderID")]
        public object HistoryFolderId { get; set; }

        [JsonProperty("AllTagsData")]
        public string AllTagsData { get; set; }

        [JsonProperty("EntityName")]
        public string EntityName { get; set; }

        [JsonProperty("EntityDescription")]
        public object EntityDescription { get; set; }

        [JsonProperty("State")]
        public object State { get; set; }

        [JsonProperty("CreatedOnDate")]
        public DateTimeOffset CreatedOnDate { get; set; }

        [JsonProperty("ModifiedDate")]
        public DateTimeOffset ModifiedDate { get; set; }

        [JsonProperty("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("Archived")]
        public bool Archived { get; set; }

        [JsonProperty("ArchivedBy")]
        public object ArchivedBy { get; set; }

        [JsonProperty("ArchivedDate")]
        public DateTimeOffset ArchivedDate { get; set; }

        [JsonProperty("Deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("DeletedBy")]
        public object DeletedBy { get; set; }

        [JsonProperty("DeletedOn")]
        public DateTimeOffset DeletedOn { get; set; }        
    }

    public class GetAllWorkflowsResult
    {
        public IEnumerable<Workflow> GetAllMobileServiceCatalogItemsResult { get; set; }
    }

    public class GetAllOfflineCatalogItemsResultWrapper
    {
        public List<Workflow> GetAllOfflineCatalogItemsResult { get; set; } = new List<Workflow>();
    }
}
