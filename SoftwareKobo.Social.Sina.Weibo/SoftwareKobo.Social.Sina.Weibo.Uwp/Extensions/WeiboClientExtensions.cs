using SoftwareKobo.Social.Sina.Weibo.Core;
using System;
using System.Collections.Specialized;
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
            var requestUri = new Uri("https://api.weibo.com/oauth2/authorize", UriKind.Absolute);
            var builder = new UriBuilder(requestUri);
            var requestUriDecoder = new WwwFormUrlDecoder(requestUri.Query);
            var query = new NameValueCollection();
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
            builder.Query = query.ToString();
            requestUri = builder.Uri;

            var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, new Uri(redirectUri));
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    var responseUrl = result.ResponseData;
                    var responseUri = new Uri(responseUrl);
                    var decoder = new WwwFormUrlDecoder(responseUri.Query);
                    var authorizeCode = decoder.GetFirstValueByName("code");

                    client.HttpPostAsync(new Uri(""), )

                    break;

                case WebAuthenticationStatus.UserCancel:
                    // TODO
                    break;

                case WebAuthenticationStatus.ErrorHttp:
                    // TODO
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}