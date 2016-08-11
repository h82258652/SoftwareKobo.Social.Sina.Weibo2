using Newtonsoft.Json;
using SoftwareKobo.Social.Sina.Weibo.Core;
using SoftwareKobo.Social.Sina.Weibo.Core.Extensions;
using SoftwareKobo.Social.Sina.Weibo.Core.Models;
using SoftwareKobo.Social.Sina.Weibo.Extensions;
using SoftwareKobo.Social.Sina.Weibo.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftwareKobo.Social.Sina.Weibo
{
    public class WeiboClientDesktop : WeiboClientBase
    {
        public WeiboClientDesktop(string appKey, string appSecret, string redirectUri, string scope = null) : base(appKey, appSecret, redirectUri, scope)
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
                if (CultureInfo.CurrentUICulture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase) == false)
                {
                    authorizeQuery["language"] = "en";
                }
                authorizeUriBuilder.Query = authorizeQuery.ToUriQuery();
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
            }
        }

        public override void ClearAuthorize()
        {
            Settings.Default.Reset();
        }
    }
}