using SoftwareKobo.Social.Sina.Weibo.Core;
using System;
using System.Collections.Specialized;
using System.Net;
using Windows.Foundation;
using Windows.Security.Authentication.Web;

namespace SoftwareKobo.Social.Sina.Weibo.Extensions
{
    public static class WeiboClientExtensions
    {
        public static void AuthorizeAsync(this WeiboClient client, string appKey, string appSecret, string redirectUri)
        {
            Uri requestUri = new Uri("https://api.weibo.com/oauth2/authorize", UriKind.Absolute);

            var decoder = new WwwFormUrlDecoder("http");
            var c = new NameValueCollection();
            foreach (var f in decoder)
            {
                c.Add(f.Name, f.Value);
            }

            WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, )
        }
    }
}