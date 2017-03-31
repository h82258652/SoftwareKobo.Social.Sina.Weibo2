using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using SoftwareKobo.Social.SinaWeibo.Models;
using SoftwareKobo.Social.SinaWeibo.Properties;
using Newtonsoft.Json;
using SoftwareKobo.Social.SinaWeibo.Extensions;

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
                var authorizeUri = new Uri("https://api.weibo.com/oauth2/authorize");
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
                if (CultureInfo.CurrentUICulture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase) == false)
                {
                    authorizeQuery["language"] = "en";
                }
                authorizeUriBuilder.Query = ToUriQuery(authorizeQuery);
                authorizeUri = authorizeUriBuilder.Uri;

                var authorizeDialog = new AuthorizeDialog(authorizeUri);
                var authorizeResult = authorizeDialog.ShowDialog();
                if (authorizeResult == DialogResult.OK)
                {
                    var getAccessTokenQuery = new Dictionary<string, string>()
                    {
                        ["client_id"] = AppKey,
                        ["client_secret"] = AppSecret,
                        ["grant_type"] = "authorization_code",
                        ["code"] = authorizeDialog.AuthorizeCode,
                        ["redirect_uri"] = RedirectUri
                    };

                    var requestTime = DateTime.Now;
                    var json = await HttpPostAsync("https://api.weibo.com/oauth2/access_token", getAccessTokenQuery, false);
                    var accessToken = JsonConvert.DeserializeObject<AccessToken>(json);

                    AccessToken = accessToken.Value;
                    Uid = accessToken.Uid;

                    LocalAccessToken.Value = accessToken.Value;
                    LocalAccessToken.Uid = accessToken.Uid;
                    LocalAccessToken.ExpiresAt = requestTime.AddSeconds(accessToken.ExpiresIn);
                }
                else if (authorizeResult == DialogResult.Cancel)
                {
                    throw new UserCancelAuthorizeException();
                }
                else
                {
                    throw new AuthorizationException();
                }
            }
        }

        public override void ClearAuthorization()
        {
            Settings.Default.Reset();
        }
    }
}