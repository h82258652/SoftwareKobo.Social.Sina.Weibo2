using Newtonsoft.Json;

namespace SoftwareKobo.Social.SinaWeibo.Models
{
    [JsonObject]
    public class ModelBase
    {
        [JsonProperty("error")]
        public string ErrorMessage
        {
            get;
            set;
        }

        [JsonProperty("error_code")]
        public int ErrorCode
        {
            get;
            set;
        }

        [JsonProperty("request")]
        public string UrlBase
        {
            get;
            set;
        }
    }
}