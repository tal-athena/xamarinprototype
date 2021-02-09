using Newtonsoft.Json;

namespace DecisionsMobile.Models
{
    /// <summary>
    /// Action type
    /// </summary>
    /// <remarks>
    /// This is derivative of DecisionsFramework.ServiceLayer.Actions.BaseActionType,
    /// which could be consumed at some point in the future instead, but only
    /// a subset of functions are currently necessary, and none of the type
    /// heirarchy may ever be necessary here.
    /// </remarks>
    public class BaseActionType
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of action
        /// </summary>
        [JsonProperty("Nm")]
        public string Nm { get; set; }

        /// <summary>
        /// ID of original element (e.g. flow) backing the action item, if it has one.
        /// </summary>
        [JsonProperty("ElementId")]
        public string ElementId { get; set; }

        /// <summary>
        /// Description of Action
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Action handler definition, which unfortunately has MVC implementation leaked into its name.
        /// </summary>
        [JsonProperty("MvcActionHandler")]
        public MvcActionHandler MvcActionHandler { get; set; }

        /// <summary>
        /// URL of Open URL type actions.
        /// </summary>
        [JsonProperty("Url")]
        public string Url { get; set; }
    }

    public class HandlerNames
    {
        public static string RUN_FLOW = "RunFlowActionHandler";
        public static string OPEN_URL = "OpenUrlActionHandler";
    }

    public class MvcActionHandler
    {
        [JsonProperty("HandlerName")]
        public string HandlerName { get; set; }
    }
}
