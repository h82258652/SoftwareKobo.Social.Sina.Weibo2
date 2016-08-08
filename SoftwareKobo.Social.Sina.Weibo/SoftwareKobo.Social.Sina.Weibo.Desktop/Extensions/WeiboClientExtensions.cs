using Newtonsoft.Json;
using SoftwareKobo.Social.Sina.Weibo.Core;
using SoftwareKobo.Social.Sina.Weibo.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace SoftwareKobo.Social.Sina.Weibo.Extensions
{
    public static class WeiboClientExtensions
    {
        public static async Task AuthorizeAsync(this WeiboClient client, string appKey, string appSecret, string redirectUri, string scope = null)
        {
            var requestUri = new Uri("https://api.weibo.com/oauth2/authorize", UriKind.Absolute);
            var uriBuilder = new UriBuilder(requestUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["client_id"] = appKey;
            query["redirect_uri"] = redirectUri;
            if (scope != null)
            {
                query["scope"] = scope;
            }
            if (CultureInfo.CurrentUICulture.Name.StartsWith("zh", StringComparison.OrdinalIgnoreCase) == false)
            {
                query["language"] = "en";
            }
            uriBuilder.Query = query.ToString();
            requestUri = uriBuilder.Uri;

            var authorizeDialog = new AuthorizeDialog(requestUri);

            var rrf = authorizeDialog.Width;
            var size = authorizeDialog.Size;

            var result = authorizeDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var parameters = new Dictionary<string, string>()
                {
                    ["client_id"] = appKey,
                    ["client_secret"] = appSecret,
                    ["grant_type"] = "authorization_code",
                    ["code"] = authorizeDialog.AuthorizeCode,
                    ["redirect_uri"] = redirectUri
                };
                var json = await client.HttpPostAsync("https://api.weibo.com/oauth2/access_token", parameters);
                var accessToken = JsonConvert.DeserializeObject<AccessToken>(json);

                client.AppKey = appKey;
                client.AppSecret = appSecret;
                client.RedirectUri = redirectUri;
                client.AccessToken = accessToken.Value;
                client.Uid = accessToken.Uid;
            }
        }
    }
}