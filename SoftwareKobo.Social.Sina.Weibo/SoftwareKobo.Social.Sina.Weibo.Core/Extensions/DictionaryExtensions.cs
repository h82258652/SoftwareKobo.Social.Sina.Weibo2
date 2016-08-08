using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SoftwareKobo.Social.Sina.Weibo.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static string ToUriQuery(this IDictionary<string, string> dictionary)
        {
            return string.Join("&", from temp in dictionary
                                    select WebUtility.UrlEncode(temp.Key) + "=" + WebUtility.UrlEncode(temp.Value));
        }
    }
}