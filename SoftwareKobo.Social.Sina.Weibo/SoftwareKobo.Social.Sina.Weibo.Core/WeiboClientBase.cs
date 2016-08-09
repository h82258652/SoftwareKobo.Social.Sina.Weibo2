using SoftwareKobo.Social.Sina.Weibo.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SoftwareKobo.Social.Sina.Weibo.Core
{
    public abstract class WeiboClientBase
    {
        public abstract Task AuthorizeAsync(string appKey, string appSecret, string redirectUri, string scope = null);

        public abstract void ClearAuthorize();

        public Task<string> HttpGetAsync(string api, IDictionary<string, string> parameters)
        {
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }

            return HttpGetAsync(new Uri(api, UriKind.Absolute), parameters);
        }

        public async Task<string> HttpGetAsync(Uri api, IDictionary<string, string> parameters)
        {
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            // TODO check authorize

            var uriBuilder = new UriBuilder(api)
            {
                Query = parameters.ToUriQuery()
            };
            api = uriBuilder.Uri;

            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(api);
            }
        }

        public Task<string> HttpPostAsync(string api, IDictionary<string, string> parameters)
        {
            return HttpPostAsync(new Uri(api, UriKind.Absolute), parameters);
        }

        public Task<string> HttpPostAsync(Uri api, IDictionary<string, string> parameters)
        {
            return HttpPostAsync(api, parameters.ToDictionary(temp => temp.Key, temp => (object)temp.Value));
        }

        public Task<string> HttpPostAsync(string api, IDictionary<string, object> parameters)
        {
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }

            return HttpPostAsync(new Uri(api, UriKind.Absolute), parameters);
        }

        public async Task<string> HttpPostAsync(Uri api, IDictionary<string, object> parameters)
        {
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            // TODO check authorize

            HttpContent content;
            if (parameters.Any(temp => temp.Value != null && temp.Value is string == false))
            {
                var postContent = new MultipartFormDataContent();
                foreach (var temp in parameters)
                {
                    var value = temp.Value;
                    var bytes = value as byte[];
                    if (bytes != null)
                    {
                        postContent.Add(new ByteArrayContent(bytes), temp.Key);
                    }
                    else
                    {
                        postContent.Add(new StringContent(temp.Value.ToString()), temp.Key);
                    }
                }
                content = postContent;
            }
            else
            {
                content = new FormUrlEncodedContent(parameters.ToDictionary(temp => temp.Key, temp => (string)temp.Value));
            }

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(api, content);
                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
        }
    }
}