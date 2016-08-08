using Newtonsoft.Json;

namespace SoftwareKobo.Social.Sina.Weibo.Core.Models
{
    [JsonObject]
    public class StatusVisible
    {
        [JsonProperty("type")]
        public int Type
        {
            get;
            set;
        }

        [JsonProperty("list_id")]
        public long ListId
        {
            get;
            set;
        }
    }
}