using Newtonsoft.Json;

namespace DecisionsMobile.Models
{
    public class DataPair
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public object OutputValue { get; set; }
    }
}
