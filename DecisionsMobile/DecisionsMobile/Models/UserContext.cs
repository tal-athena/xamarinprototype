using Newtonsoft.Json;
using System;

namespace DecisionsMobile.Models
{

    /// <summary>
    /// A user context which many service endpoints require (beyond just as sessionId)
    /// </summary>
    public class UserContext
    {
        public UserContext(string sessionValue)
        {
            SessionValue = sessionValue;
            ClientEventSessionId = Guid.NewGuid().ToString();
            DisplayType = "0";
            StudioPortal = false;
            BrowserUserAgent = null;
        }

        [JsonProperty("__type")]
        public string Type = "SessionUserContext:#DecisionsFramework.ServiceLayer.Utilities";

        [JsonProperty]
        public string SessionValue { get; set; }

        [JsonProperty]
        public string ClientEventSessionId { get; set; }

        [JsonProperty]
        public string DisplayType { get; set; }

        [JsonProperty]
        public bool StudioPortal { get; set; }

        [JsonProperty]
        public string BrowserUserAgent { get; set; }
    }
}
