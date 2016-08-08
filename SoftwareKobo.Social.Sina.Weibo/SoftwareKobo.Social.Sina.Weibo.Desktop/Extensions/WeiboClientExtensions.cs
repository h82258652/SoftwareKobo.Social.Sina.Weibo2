using SoftwareKobo.Social.Sina.Weibo.Core;
using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;

namespace SoftwareKobo.Social.Sina.Weibo.Extensions
{
    public static class WeiboClientExtensions
    {
        public static void AuthorizeAsync(this WeiboClient client, string appKey, string appSecret, string redirectUri)
        {
            Uri requestUri = new Uri("", UriKind.Absolute);
            var uriBuilder = new UriBuilder(requestUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["xx"] = appKey;
            uriBuilder.Query = query.ToString();
            requestUri = uriBuilder.Uri;

            AuthorizeDialog dialog = new AuthorizeDialog();
            var result = dialog.ShowDialog();
        }
    }
}