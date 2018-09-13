using Newtonsoft.Json;

namespace RomeApp.Models
{
    public partial class DeviceInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }
    }
}
