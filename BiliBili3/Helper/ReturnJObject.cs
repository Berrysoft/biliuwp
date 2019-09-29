using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BiliBili3
{
    public class ReturnJObject
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("msg")]
        public string Msg { get; set; }
        [JsonIgnore]
        public JToken Json { get; set; }
    }
}
