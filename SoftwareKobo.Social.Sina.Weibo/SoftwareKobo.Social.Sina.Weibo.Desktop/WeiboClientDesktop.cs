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
        public override void ClearAuthorize()
        {
            Settings.Default.Reset();
        }

        public override async Task AuthorizeAsync(string appKey, string appSecret, string redirectUri, string scope = null)
        {
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

            var authorizeUri = new Uri("https://api.weibo.com/oauth2/authorize", UriKind.Absolute);
            var authorizeUriBuilder = new UriBuilder(authorizeUri);
            Dictionary<string, string> authorizeQuery = new Dictionary<string, string>();
            authorizeQuery["client_id"] = appKey;
            authorizeQuery["redirect_uri"] = redirectUri;
            if (scope != null)
            {
                authorizeQuery["scope"] = scope;
            }
            if (CultureInfo.CurrentUICulture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase) == false)
            {
                authorizeQuery["language"] = "en";
            }
            authorizeUriBuilder.Query = authorizeQuery.ToUriQuery();
            authorizeUri = authorizeUriBuilder.Uri;

            AuthorizeDialog authorizeDialog = new AuthorizeDialog(authorizeUri);
            var authorizeResult = authorizeDialog.ShowDialog();
            if (authorizeResult == DialogResult.OK)
            {
                var getAccessTokenQuery = new Dictionary<string, string>()
                {
                    ["client_id"] = appKey,
                    ["client_secret"] = appSecret,
                    ["grant_type"] = "authorization_code",
                    ["code"] = authorizeDialog.AuthorizeCode,
                    ["redirect_uri"] = redirectUri
                };

                var json = await HttpPostAsync("https://api.weibo.com/oauth2/access_token", getAccessTokenQuery);
                var accessToken = JsonConvert.DeserializeObject<AccessToken>(json);
            }
        }
    }
}