using Newtonsoft.Json;

namespace DecisionsMobile.Models
{
    public partial class ImageInfo
    {
        [JsonProperty("ImageType")]
        public long ImageType { get; set; }

        [JsonProperty("DocumentId")]
        public string DocumentId { get; set; }

        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("ImageId")]
        public string ImageId { get; set; }

        [JsonProperty("RawData")]
        public object RawData { get; set; }

        [JsonProperty("ImageName")]
        public string ImageName { get; set; }

        [JsonProperty("ImageFileReferenceId")]
        public object ImageFileReferenceId { get; set; }
        [JsonProperty("UrlEncodedImage", NullValueHandling = NullValueHandling.Ignore)]
        public string UrlEncodedImage { get; set; }
    }
}
