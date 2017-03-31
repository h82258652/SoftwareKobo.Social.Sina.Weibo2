using Newtonsoft.Json;
using SoftwareKobo.Social.SinaWeibo.Helpers;
using SoftwareKobo.Social.SinaWeibo.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Authentication.Web;
using Windows.Storage;

namespace SoftwareKobo.Social.SinaWeibo
{
    public class SinaWeiboClient : SinaWeiboClientBase
    {
        public SinaWeiboClient(string appKey, string appSecret, string redirectUri, string scope = null) : base(appKey, appSecret, redirectUri, scope)
        {
            if (LocalAccessToken.IsUseable)
            {
                AccessToken = LocalAccessToken.Value;
                Uid = LocalAccessToken.Uid;
            }
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
                var authorizeUri = new Uri("https://api.weibo.com/oauth2/authorize", UriKind.Absolute);
                var authorizeUriBuilder = new UriBuilder(authorizeUri);
                var authorizeQuery = new Dictionary<string, string>()
                {
                    ["client_id"] = AppKey,
                    ["redirect_uri"] = RedirectUri
                };
                if (Scope != null)
                {
                    authorizeQuery["scope"] = Scope;
                }
                if (DeviceFamilyHelper.IsMobile)
                {
                    authorizeQuery["display"] = "mobile";
                }
                if (CultureInfo.CurrentUICulture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase) == false)
                {
                    authorizeQuery["language"] = "en";
                }
                authorizeUriBuilder.Query = ToUriQuery(authorizeQuery);
                authorizeUri = authorizeUriBuilder.Uri;

                var authorizeResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, authorizeUri, new Uri(RedirectUri, UriKind.Absolute));
                switch (authorizeResult.ResponseStatus)
                {
                    case WebAuthenticationStatus.Success:
                        var responseUrl = authorizeResult.ResponseData;
                        var responseUri = new Uri(responseUrl, UriKind.Absolute);
                        var responseUriDecoder = new WwwFormUrlDecoder(responseUri.Query);
                        var authorizeCode = responseUriDecoder.GetFirstValueByName("code");

                        var getAccessTokenQuery = new Dictionary<string, string>()
                        {
                            ["client_id"] = AppKey,
                            ["client_secret"] = AppSecret,
                            ["grant_type"] = "authorization_code",
                            ["code"] = authorizeCode,
                            ["redirect_uri"] = RedirectUri
                        };

                        var requestTime = DateTimeOffset.Now;
                        var json = await HttpPostAsync("https://api.weibo.com/oauth2/access_token", getAccessTokenQuery, false);
                        var accessToken = JsonConvert.DeserializeObject<AccessToken>(json);

                        AccessToken = accessToken.Value;
                        Uid = accessToken.Uid;

                        LocalAccessToken.Value = accessToken.Value;
                        LocalAccessToken.Uid = accessToken.Uid;
                        LocalAccessToken.ExpiresAt = requestTime.AddSeconds(accessToken.ExpiresIn);

                        break;

                    case WebAuthenticationStatus.UserCancel:
                        throw new UserCancelAuthorizeException(authorizeResult);

                    case WebAuthenticationStatus.ErrorHttp:
                        throw new AuthorizeErrorHttpException(authorizeResult);
                }
            }
        }

        public override void ClearAuthorization()
        {
            AccessToken = null;
            ApplicationData.Current.LocalSettings.DeleteContainer(Constants.LocalAccessTokenDataContainerName);
        }
    }
}