using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomeApp.Models
{
    public partial class CommandRequest
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "LaunchUri";

        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }

    public partial class Payload
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
