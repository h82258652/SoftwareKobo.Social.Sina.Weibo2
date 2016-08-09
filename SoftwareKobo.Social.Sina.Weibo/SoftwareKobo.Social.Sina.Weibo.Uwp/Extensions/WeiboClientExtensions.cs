using Newtonsoft.Json;
using SoftwareKobo.Social.Sina.Weibo.Core;
using SoftwareKobo.Social.Sina.Weibo.Core.Extensions;
using SoftwareKobo.Social.Sina.Weibo.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Security.Authentication.Web;

namespace SoftwareKobo.Social.Sina.Weibo.Extensions
{
    public static class WeiboClientExtensions
    {
        public static async Task AuthorizeAsync(this WeiboClient client, string appKey, string appSecret, string redirectUri, string scope = null)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            if (appKey == null)
            {
                throw new ArgumentNullException(nameof(appKey));
            }
            if (appSecret == null)
            {
                throw new ArgumentNullException(nameof(appSecret));
            }
            if (redirectUri == null)
            {
                throw new ArgumentNullException(nameof(redirectUri));
            }

            var requestUri = new Uri("https://api.weibo.com/oauth2/authorize", UriKind.Absolute);
            var builder = new UriBuilder(requestUri);
            var requestUriDecoder = new WwwFormUrlDecoder(requestUri.Query);
            var query = new Dictionary<string, string>();
            foreach (var entry in requestUriDecoder)
            {
                query[entry.Name] = entry.Value;
            }
            query["client_id"] = appKey;
            query["redirect_uri"] = redirectUri;
            if (scope != null)
            {
                query["scope"] = scope;
            }
            var qualifiers = ResourceContext.GetForCurrentView().QualifierValues;
            if (qualifiers.ContainsKey("DeviceFamily") && qualifiers["DeviceFamily"] == "Mobile")
            {
                query["display"] = "mobile";
            }
            if (CultureInfo.CurrentUICulture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase) == false)
            {
                query["language"] = "en";
            }
            builder.Query = query.ToUriQuery();
            requestUri = builder.Uri;

            var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, new Uri(redirectUri));
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    var responseUrl = result.ResponseData;
                    var responseUri = new Uri(responseUrl);
                    var decoder = new WwwFormUrlDecoder(responseUri.Query);
                    var authorizeCode = decoder.GetFirstValueByName("code");

                    var parameters = new Dictionary<string, string>()
                    {
                        ["client_id"] = appKey,
                        ["client_secret"] = appSecret,
                        ["grant_type"] = "authorization_code",
                        ["code"] = authorizeCode,
                        ["redirect_uri"] = redirectUri
                    };
                    var json = await client.HttpPostAsync("https://api.weibo.com/oauth2/access_token", parameters);
                    var accessToken = JsonConvert.DeserializeObject<AccessToken>(json);

                    client.AppKey = appKey;
                    client.AppSecret = appSecret;
                    client.RedirectUri = redirectUri;
                    client.AccessToken = accessToken.Value;
                    client.Uid = accessToken.Uid;
                    break;

                case WebAuthenticationStatus.UserCancel:
                    // TODO
                    break;

                case WebAuthenticationStatus.ErrorHttp:
                    // TODO
                    break;
            }
        }
    }
}