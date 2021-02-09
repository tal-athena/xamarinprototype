namespace DecisionsMobile.Models
{
    using DecisionsMobile.Models.FormService;
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    public partial class FlowExecutionStateInstruction
    {
        // this isn't getting serialized either way...
        [JsonProperty("__type")] // __type?
        public string Type { get; set; }

        [JsonProperty("StartupEvents")]
        public EventsReturn StartupEvents { get; set; }

        [JsonProperty("FormID")]
        public string FormId { get; set; }

        [JsonProperty("FlowId")]
        public string FlowId { get; set; }

        [JsonProperty("StepId")]
        public string StepId { get; set; }

        [JsonProperty("StepName")]
        public string StepName { get; set; }

        [JsonProperty("OverrideFormTitle")]
        public bool OverrideFormTitle { get; set; }

        [JsonProperty("FormTitle")]
        public string FormTitle { get; set; }

        [JsonProperty("TitleSettings")]
        public object TitleSettings { get; set; } // TODO FormTitleSettings

        [JsonProperty("SizeTypeInSubDialog")]
        public long SizeTypeInSubDialog { get; set; }

        [JsonProperty("SizeTypeInOwnWindow")]
        public long SizeTypeInOwnWindow { get; set; }

        [JsonProperty("OwnWindowSetSizeOnlyIfNotSpecified")]
        public bool OwnWindowSetSizeOnlyIfNotSpecified { get; set; }

        [JsonProperty("OwnWindowSetBackgroundColorOnlyIfNotSpecified")]
        public bool OwnWindowSetBackgroundColorOnlyIfNotSpecified { get; set; }

        [JsonProperty("OwnWindowBackgroundColor")]
        public DesignerColor OwnWindowBackgroundColor { get; set; }

        [JsonProperty("OwnWindowWidth")]
        public long OwnWindowWidth { get; set; }

        [JsonProperty("OwnWindowHeight")]
        public long OwnWindowHeight { get; set; }

        [JsonProperty("SubDialogSetSizeOnlyIfNotSpecified")]
        public bool SubDialogSetSizeOnlyIfNotSpecified { get; set; }

        [JsonProperty("SubDialogWidth")]
        public long SubDialogWidth { get; set; }

        [JsonProperty("SubDialogHeight")]
        public long SubDialogHeight { get; set; }

        [JsonProperty("StepTrackingId")]
        public string StepTrackingId { get; set; }

        [JsonProperty("DoNotAllowResize")]
        public bool DoNotAllowResize { get; set; }

        [JsonProperty("MessageIfFormOrFlowIsNotCurrent")]
        public string MessageIfFormOrFlowIsNotCurrent { get; set; }

        [JsonProperty("SubDialogPositionType")]
        public long SubDialogPositionType { get; set; }

        [JsonProperty("PreloadFormList")]
        public PreloadFormInformation[] PreloadFormList { get; set; }

        [JsonProperty("SubDialogUseHideTiltleBarFromHost")]
        public bool SubDialogUseHideTiltleBarFromHost { get; set; }

        [JsonProperty("HideTitleBar")]
        public bool HideTitleBar { get; set; }

        [JsonProperty("TimeStamp")]
        public double TimeStamp { get; set; }

        [JsonProperty("BackgroundFormTimeStamp")]
        public long BackgroundFormTimeStamp { get; set; }

        [JsonProperty("FormSessionId")]
        public string FormSessionId { get; set; }

        [JsonProperty("InstructionDateTime")]
        public DateTimeOffset InstructionDateTime { get; set; }

        [JsonProperty("BeingProcessed")]
        public bool BeingProcessed { get; set; }

        [JsonProperty("InstructionForUser")]
        public string InstructionForUser { get; set; }

        [JsonProperty("TopFlowTrackingId")]
        public string TopFlowTrackingId { get; set; }

        [JsonProperty("FlowTrackingId")]
        public string FlowTrackingId { get; set; }

        [JsonProperty("SetupInfo")]
        public object SetupInfo { get; set; } //BasicPortalSetupInfo

        [JsonProperty("FlowOutputs")]
        public DataPair[] FlowOutputs { get; set; }

        [JsonProperty("ExceptionMessage")]
        public string ExceptionMessage { get; internal set; }
    }

    public partial class PreloadFormInformation
    {
        [JsonProperty("$type")]
        public string Type { get; set; }

        [JsonProperty("FormId")]
        public string FormId { get; set; }

        [JsonProperty("TimeStamp")]
        public double TimeStamp { get; set; }
    }

    public partial class EventsReturn
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("Events")]
        public DataPair[] Events { get; set; }
    }

    /// 
    /// this is really a harmonization of NextFormInstructionEvent, NextInstructionEvent, and ValidationChangedEvent,
    /// for convenience, since those are the likely outcomes for FormService.SelectPath,
    /// and we don't have the robust type converters in this little app, that the DecisionsFramework has.
    ///
    public class DecisionsFormInfoEvent
    {
        static readonly string ValidationEventType = "ValidationChangedEvent:#DecisionsFramework.Design.Form";
        static readonly string NextFormInstructionEventType = "NextFormInstructionEvent:#DecisionsFramework.Design.Form";
        static readonly string FlowCompletedInstructionType = "FlowCompletedInstruction:#DecisionsFramework.Design.Flow.Service.Execution";

        [JsonProperty("__type")]
        public string ServerType { get; set; }

        [JsonProperty]
        public string ParentComponentId { get; set; }

        [JsonProperty]
        public string SurfaceId { get; set; }

        #region ValidationChangedEventProperties

        [JsonProperty]
        public FormValidations[] CurrentValidations { get; set; }

        #endregion

        #region NextFormInstructionProperties

        [JsonProperty]
        public string FlowTrackingId { get; set; }

        [JsonProperty]
        public string StepTrackingId { get; set; }

        #endregion

        #region NextInstructionEventProperties 

        [JsonProperty]
        public FlowExecutionStateInstruction NextInstruction { get; set; }
        #endregion

        #region Helpers
        public bool IsValidationEvent() => ServerType.Equals(ValidationEventType);

        public bool IsNextFormInstructionEvent() => ServerType.Equals(NextFormInstructionEventType);

        public bool IsFlowCompletedInstructionEvent() => NextInstruction != null && NextInstruction.Type.Equals(FlowCompletedInstructionType);

        public string[] GetValidationMessages()
        {
            if (CurrentValidations == null)
            {
                return new string[0];
            }
            var validationIssues = CurrentValidations
                .SelectMany(formValidation => formValidation.ValidationIssues)
                .Where(issue => issue.BreakLevel == BreakLevel.Fatal)
                .Select(issue => issue.Message)
                .Distinct().ToArray();

            return validationIssues;
        }

        #endregion
    }

    // Worth casting to FlowCompletedInstruction at the end of a set of forms?
    public class FlowCompletedInstruction : FlowExecutionStateInstruction
    {
        public DataPair[] ResultData { get; set; }
    }

    public class FormInfoServerEventWrapper
    {
        [JsonProperty]
        public DecisionsFormInfoEvent[] Events;
    }

}
