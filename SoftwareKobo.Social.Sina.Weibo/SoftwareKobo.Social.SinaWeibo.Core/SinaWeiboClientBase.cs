using Newtonsoft.Json;
using SoftwareKobo.Social.SinaWeibo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SoftwareKobo.Social.SinaWeibo
{
    public abstract class SinaWeiboClientBase
    {
        protected SinaWeiboClientBase(string appKey, string appSecret, string redirectUri, string scope = null)
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

            AppKey = appKey;
            AppSecret = appSecret;
            RedirectUri = redirectUri;
            Scope = scope;
        }

        public abstract bool IsAuthorized
        {
            get;
        }

        public string Uid
        {
            get;
            protected set;
        }

        protected string AccessToken
        {
            get;
            set;
        }

        protected string AppKey
        {
            get;
        }

        protected string AppSecret
        {
            get;
        }

        protected string RedirectUri
        {
            get;
        }

        protected string Scope
        {
            get;
        }

        public abstract Task AuthorizeAsync();

        public abstract void ClearAuthorization();

        public Task<string> HttpGetAsync(string api, IDictionary<string, string> parameters, bool requiredAuthorized = true)
        {
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }

            return HttpGetAsync(new Uri(api, UriKind.Absolute), parameters, requiredAuthorized);
        }

        public async Task<string> HttpGetAsync(Uri api, IDictionary<string, string> parameters, bool requiredAuthorized = true)
        {
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (requiredAuthorized && IsAuthorized == false)
            {
                await AuthorizeAsync();
            }

            var uriBuilder = new UriBuilder(api)
            {
                Query = ToUriQuery(PrepareParameters(parameters))
            };
            api = uriBuilder.Uri;

            using (var client = new HttpClient())
            {
                var repsonse = await client.GetAsync(api);
                return await repsonse.Content.ReadAsStringAsync();
            }
        }

        public Task<string> HttpPostAsync(string api, IDictionary<string, object> parameters, bool requiredAuthorized = true)
        {
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }

            return HttpPostAsync(new Uri(api, UriKind.Absolute), parameters, requiredAuthorized);
        }

        public Task<string> HttpPostAsync(string api, IDictionary<string, string> parameters, bool requiredAuthorized = true)
        {
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }

            return HttpPostAsync(new Uri(api, UriKind.Absolute), parameters, requiredAuthorized);
        }

        public async Task<string> HttpPostAsync(Uri api, IDictionary<string, object> parameters, bool requiredAuthorized = true)
        {
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (requiredAuthorized && IsAuthorized == false)
            {
                await AuthorizeAsync();
            }

            parameters = PrepareParameters(parameters);
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
                        postContent.Add(new ByteArrayContent(bytes), temp.Key, "pic.jpg");
                    }
                    else
                    {
                        postContent.Add(new StringContent(value.ToString()), temp.Key);
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

                var modelBase = JsonConvert.DeserializeObject<ModelBase>(json);
                if (modelBase.ErrorCode == Constants.UserRemoveAuthenticationErrorCode)
                {
                    ClearAuthorization();
                }

                return json;
            }
        }

        public Task<string> HttpPostAsync(Uri api, IDictionary<string, string> parameters, bool requiredAuthorized = true)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return HttpPostAsync(api, parameters.ToDictionary(temp => temp.Key, temp => (object)temp.Value), requiredAuthorized);
        }

        public async Task<Status> ShareAsync(string status)
        {
            if (status == null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            var parameters = new Dictionary<string, string>()
            {
                ["status"] = status
            };
            var json = await HttpPostAsync("https://api.weibo.com/2/statuses/share.json", parameters);
            return JsonConvert.DeserializeObject<Status>(json);
        }

        public async Task<Status> ShareAsync(string status, byte[] pic)
        {
            if (status == null)
            {
                throw new ArgumentNullException(nameof(status));
            }
            if (pic == null)
            {
                throw new ArgumentNullException(nameof(pic));
            }

            var parameters = new Dictionary<string, object>()
            {
                ["status"] = status,
                ["pic"] = pic
            };
            var json = await HttpPostAsync("https://api.weibo.com/2/statuses/share.json", parameters);
            return JsonConvert.DeserializeObject<Status>(json);
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

        public async Task<Status> UploadAsync(string status, byte[] pic)
        {
            if (status == null)
            {
                throw new ArgumentNullException(nameof(status));
            }
            if (pic == null)
            {
                throw new ArgumentNullException(nameof(pic));
            }

            var parameters = new Dictionary<string, object>()
            {
                ["status"] = status,
                ["pic"] = pic
            };
            var json = await HttpPostAsync("https://upload.api.weibo.com/2/statuses/upload.json", parameters);
            return JsonConvert.DeserializeObject<Status>(json);
        }

        protected static string ToUriQuery(IDictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return string.Join("&", from temp in dictionary
                                    select WebUtility.UrlEncode(temp.Key) + "=" + WebUtility.UrlEncode(temp.Value));
        }

        private IDictionary<string, TValue> PrepareParameters<TValue>(IDictionary<string, TValue> parameters) where TValue : class
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.ContainsKey("client_id") == false)
            {
                parameters["client_id"] = AppKey as TValue;
            }
            if (parameters.ContainsKey("client_secret") == false)
            {
                parameters["client_secret"] = AppSecret as TValue;
            }
            if (parameters.ContainsKey("redirect_uri") == false)
            {
                parameters["redirect_uri"] = RedirectUri as TValue;
            }
            if (parameters.ContainsKey("code") == false)
            {
                parameters["access_token"] = AccessToken as TValue;
            }
            return parameters;
        }
    }
}