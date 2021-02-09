using Newtonsoft.Json;

namespace DecisionsMobile.Models
{
    public enum BreakLevel
    {
        Warning, Fatal, Ignored
    }

    public class ValidationIssue
    {

        public ValidationIssue()
        { }

        [JsonProperty]
        public BreakLevel BreakLevel { get; set; } //: 1,
        [JsonProperty]
        public object ForData { get; set; } //: null,
        [JsonProperty]
        public string Message { get; set; } //: "'Drop Down List' must be specified",
        [JsonProperty]
        public string RuleID { get; set; } //: null,
        [JsonProperty]
        public DecisionsValidationIssueType ValidationType { get; set; } //: 2
    }

    public enum DecisionsValidationIssueType
    {
        Unknown, Custom, RequiredDataMissing, Runtime
    }
}
