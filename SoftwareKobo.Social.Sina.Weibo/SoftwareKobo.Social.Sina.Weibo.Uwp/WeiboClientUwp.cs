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
using Windows.Storage;

namespace SoftwareKobo.Social.Sina.Weibo
{
    public class WeiboClientUwp : WeiboClientBase
    {
        public WeiboClientUwp(string appKey, string appSecret, string redirectUri, string scope = null) : base(appKey, appSecret, redirectUri, scope)
        {
        }

        public override bool IsAuthorized => LocalAccessToken.IsUseable;

        public override async Task AuthorizeAsync()
        {
            if (LocalAccessToken.IsUseable)
            {
                AccessToken = LocalAccessToken.Value;
                Uid = LocalAccessToken.Uid;
            }
            else
            {
                var requestUri = new Uri("https://api.weibo.com/oauth2/authorize", UriKind.Absolute);
                var builder = new UriBuilder(requestUri);
                var query = new Dictionary<string, string>()
                {
                    ["client_id"] = AppKey,
                    ["redirect_uri"] = RedirectUri
                };
                if (Scope != null)
                {
                    query["scope"] = Scope;
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

                var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, new Uri(RedirectUri));
                switch (result.ResponseStatus)
                {
                    case WebAuthenticationStatus.Success:
                        var responseUrl = result.ResponseData;
                        var responseUri = new Uri(responseUrl);
                        var decoder = new WwwFormUrlDecoder(responseUri.Query);
                        var authorizeCode = decoder.GetFirstValueByName("code");

                        var parameters = new Dictionary<string, string>()
                        {
                            ["client_id"] = AppKey,
                            ["client_secret"] = AppSecret,
                            ["grant_type"] = "authorization_code",
                            ["code"] = authorizeCode,
                            ["redirect_uri"] = RedirectUri
                        };

                        var requestTime = DateTime.Now;
                        var json = await HttpPostAsync("https://api.weibo.com/oauth2/access_token", parameters, false);
                        var accessToken = JsonConvert.DeserializeObject<AccessToken>(json);

                        AccessToken = accessToken.Value;
                        Uid = accessToken.Uid;

                        LocalAccessToken.Value = accessToken.Value;
                        LocalAccessToken.Uid = accessToken.Uid;
                        LocalAccessToken.ExpiresAt = requestTime.AddSeconds(accessToken.ExpiresIn);
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

        public override void ClearAuthorize()
        {
            // TODO to const
            ApplicationData.Current.LocalSettings.DeleteContainer("SinaWeibo");
        }
    }
}