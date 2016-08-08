using Newtonsoft.Json;
using SoftwareKobo.Social.Sina.Weibo.Core.Extensions;
using SoftwareKobo.Social.Sina.Weibo.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SoftwareKobo.Social.Sina.Weibo.Core
{
    public class WeiboClient
    {
        internal string AccessToken;

        internal string AppKey;

        internal string AppSecret;

        internal string RedirectUri;

        internal string Uid;

        public Task<string> HttpGetAsync(string requestUrl, IDictionary<string, string> parameters)
        {
            return HttpGetAsync(new Uri(requestUrl, UriKind.Absolute), parameters);
        }

        public async Task<string> HttpGetAsync(Uri requestUri, IDictionary<string, string> parameters)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException(nameof(requestUri));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var builder = new UriBuilder(requestUri)
            {
                Query = PrepareParameter(parameters).ToUriQuery()
            };
            requestUri = builder.Uri;
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(requestUri);
            }
        }

        public Task<string> HttpPostAsync(string requestUrl, IDictionary<string, string> parameters)
        {
            return HttpPostAsync(new Uri(requestUrl, UriKind.Absolute), parameters);
        }

        public async Task<string> HttpPostAsync(Uri requestUri, IDictionary<string, string> parameters)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException(nameof(requestUri));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var content = new FormUrlEncodedContent(PrepareParameter(parameters));
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(requestUri, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<User> ShowAsync(long uid)
        {
            var parameters = new Dictionary<string, string>()
            {
                ["uid"] = uid.ToString()
            };
            var json = await HttpGetAsync("https://api.weibo.com/2/users/show.json", parameters);
            return JsonConvert.DeserializeObject<User>(json);
        }

        public async Task<Status> UpdateAsync(string status)
        {
            if (status == null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            var parameters = new Dictionary<string, string>()
            {
                ["status"] = status
            };
            var json = await HttpPostAsync("https://api.weibo.com/2/statuses/update.json", parameters);
            return JsonConvert.DeserializeObject<Status>(json);
        }

        private IDictionary<string, string> PrepareParameter(IDictionary<string, string> parameters)
        {
            if (parameters.ContainsKey("client_id") == false)
            {
                parameters["client_id"] = AppKey;
            }
            if (parameters.ContainsKey("client_secret") == false)
            {
                parameters["client_secret"] = AppSecret;
            }
            if (parameters.ContainsKey("redirect_uri") == false)
            {
                parameters["redirect_uri"] = RedirectUri;
            }
            if (parameters.ContainsKey("code") == false)
            {
                parameters["access_token"] = AccessToken;
            }
            return parameters;
        }
    }
}