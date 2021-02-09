using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DecisionsMobile.Models
{
    // do we end up importing an assignment .dll from Decisions backend for this?
    // if so, we can deserialize it
    public partial class Assignment
    {
        [JsonProperty("AssignmentId")]
        public string AssignmentId { get; set; }

        [JsonProperty("ValidOnlyInFolderState")]
        public object ValidOnlyInFolderState { get; set; }

        [JsonProperty("Completed")]
        public bool Completed { get; set; }

        [JsonProperty("IsAssigned")]
        public bool IsAssigned { get; set; }

        [JsonProperty("BringUpOnLogin")]
        public bool BringUpOnLogin { get; set; }

        [JsonProperty("UrlTokens")]
        public string UrlTokens { get; set; }

        [JsonProperty("NextCheckTime")]
        public object NextCheckTime { get; set; }

        [JsonProperty("NextCheckType")]
        public long NextCheckType { get; set; }

        [JsonProperty("NextCheckId")]
        public object NextCheckId { get; set; }

        [JsonProperty("StateChangedDate")]
        public DateTimeOffset StateChangedDate { get; set; }

        [JsonProperty("WarnDate")]
        public DateTimeOffset WarnDate { get; set; }

        [JsonProperty("LateDate")]
        public DateTimeOffset LateDate { get; set; }

        [JsonProperty("EscalateDate")]
        public DateTimeOffset EscalateDate { get; set; }

        [JsonProperty("ExpirationDate")]
        public DateTimeOffset ExpirationDate { get; set; }

        [JsonProperty("StartDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty("AssignmentInteractionStarted")]
        public DateTimeOffset AssignmentInteractionStarted { get; set; }

        [JsonProperty("AssignmentStarted")]
        public DateTimeOffset AssignmentStarted { get; set; }

        [JsonProperty("Priority")]
        public string Priority { get; set; }

        [JsonProperty("Notes")]
        public string Notes { get; set; }

        [JsonProperty("HandlerData")]
        public object HandlerData { get; set; }

        [JsonProperty("FlowTrackingId")]
        public string FlowTrackingId { get; set; }

        [JsonProperty("StepTrackingId")]
        public string StepTrackingId { get; set; }

        [JsonProperty("PrimaryFlowId")]
        public string PrimaryFlowId { get; set; }

        [JsonProperty("PrimaryFlowName")]
        public string PrimaryFlowName { get; set; }

        [JsonProperty("FlowId")]
        public string FlowId { get; set; }

        [JsonProperty("FlowName")]
        public string FlowName { get; set; }

        [JsonProperty("StepId")]
        public string StepId { get; set; }

        [JsonProperty("StepName")]
        public string StepName { get; set; }

        [JsonProperty("CompletedDateTime")]
        public object CompletedDateTime { get; set; }

        [JsonProperty("AssignmentTimeInSeconds")]
        public long AssignmentTimeInSeconds { get; set; }

        [JsonProperty("AssignmentInteractionTimeInSeconds")]
        public long AssignmentInteractionTimeInSeconds { get; set; }

        [JsonProperty("CompletedResult")]
        public object CompletedResult { get; set; }

        [JsonProperty("CompletedBy")]
        public object CompletedBy { get; set; }

        [JsonProperty("CompletedNotes")]
        public object CompletedNotes { get; set; }

        [JsonProperty("AssignmentType")]
        public string AssignmentType { get; set; }

        [JsonProperty("JobScheduleId")]
        public string JobScheduleId { get; set; }

        [JsonProperty("HasStateChanged")]
        public bool HasStateChanged { get; set; }

        [JsonProperty("StateChangedBy")]
        public string StateChangedBy { get; set; }

        [JsonProperty("AllAssignments")]
        public string AllAssignments { get; set; }

        [JsonProperty("AssignmentStartFlowId")]
        public string AssignmentStartFlowId { get; set; }

        [JsonProperty("AssignmentWarnFlowId")]
        public string AssignmentWarnFlowId { get; set; }

        [JsonProperty("AssignmentLateFlowId")]
        public string AssignmentLateFlowId { get; set; }

        [JsonProperty("AssignmentEscalteFlowId")]
        public string AssignmentEscalteFlowId { get; set; }

        [JsonProperty("AssignedUsers")]
        public string[] AssignedUsers { get; set; }

        [JsonProperty("AssignedGroups")]
        public object[] AssignedGroups { get; set; } // FIXME what is it?

        [JsonProperty("NotificationSubject")]
        public string NotificationSubject { get; set; }

        [JsonProperty("NotificationMessage")]
        public string NotificationMessage { get; set; }

        [JsonProperty("DoNotSendDefaultNotification")]
        public bool DoNotSendDefaultNotification { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("Hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("AdministratorViewOnly")]
        public bool AdministratorViewOnly { get; set; }

        [JsonProperty("EntityFolderID")]
        public string EntityFolderId { get; set; }

        [JsonProperty("HistoryFolderID")]
        public string HistoryFolderId { get; set; }

        [JsonProperty("AllTagsData")]
        public string AllTagsData { get; set; }

        [JsonProperty("EntityName")]
        public string EntityName { get; set; }

        [JsonProperty("EntityDescription")]
        public string EntityDescription { get; set; }

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

    public class GetMyCurrentAssignmentsResultWrapper
    {
        public IEnumerable<Assignment> GetMyCurrentAssignmentsResult { get; set; }
    }

    public class GetDoAssignmentUrlResultWrapper
    {
        public string GetDoAssignmentUrlResult { get; set; }
    }
}