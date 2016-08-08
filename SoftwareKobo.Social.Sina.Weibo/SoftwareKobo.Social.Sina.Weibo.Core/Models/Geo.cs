using Newtonsoft.Json;

namespace SoftwareKobo.Social.Sina.Weibo.Core.Models
{
    [JsonObject]
    public class Geo
    {
        /// <summary>
        /// 经度坐标
        /// </summary>
        [JsonProperty("longitude")]
        public string Longitude
        {
            get;
            set;
        }

        /// <summary>
        /// 维度坐标
        /// </summary>
        [JsonProperty("latitude")]
        public string Latitude
        {
            get;
            set;
        }

        /// <summary>
        /// 所在城市的城市代码
        /// </summary>
        [JsonProperty("city")]
        public string City
        {
            get;
            set;
        }

        /// <summary>
        /// 所在省份的省份代码
        /// </summary>
        [JsonProperty("province")]
        public string Province
        {
            get;
            set;
        }

        /// <summary>
        /// 所在城市的城市名称
        /// </summary>
        [JsonProperty("city_name")]
        public string CityName
        {
            get;
            set;
        }

        /// <summary>
        /// 所在省份的省份名称
        /// </summary>
        [JsonProperty("province_name")]
        public string ProvinceName
        {
            get;
            set;
        }

        /// <summary>
        /// 所在的实际地址，可以为空
        /// </summary>
        [JsonProperty("address")]
        public string Address
        {
            get; set;
        }

        /// <summary>
        /// 地址的汉语拼音，不是所有情况都会返回该字段
        /// </summary>
        [JsonProperty("pinyin")]
        public string Pinyin
        {
            get; set;
        }

        /// <summary>
        /// 更多信息，不是所有情况都会返回该字段
        /// </summary>
        [JsonProperty("more")]
        public string More
        {
            get; set;
        }
    }
}