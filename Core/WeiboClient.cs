using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using SSCMS.Login.Models;

namespace SSCMS.Login.Core
{
    //http://open.weibo.com/wiki/Connect/login

    public class WeiboClient
    {
        public string AppKey { get; }
        public string AppSecret { get; }
        public string RedirectUrl { get; }

        public WeiboClient(string appKey, string appSecret, string redirectUrl)
        {
            AppKey = appKey;
            AppSecret = appSecret;
            RedirectUrl = ApiUtils.GetAuthRedirectUrl(OAuthType.Weibo, redirectUrl);
        }

        public string GetAuthorizationUrl()
        {
            return
                $"https://api.weibo.com/oauth2/authorize?client_id={AppKey}&response_type=code&redirect_uri={HttpUtility.UrlEncode(RedirectUrl)}";
        }

        private KeyValuePair<string, string> GetAccessTokenAndUId(string code)
        {
            var dict = new Dictionary<string, string>
            {
                {"client_id", AppKey},
                {"client_secret", AppSecret},
                {"grant_type", "authorization_code"},
                {"code", code},
                {"redirect_uri", RedirectUrl}
            };
            var response = HttpPost("https://api.weibo.com/oauth2/access_token", dict);
            var responseText = response.Content.ReadAsStringAsync().Result;

            var result = JObject.Parse(responseText);
            var accessToken = result.Value<string>("access_token");
            var uid = result.Value<string>("uid");

            return new KeyValuePair<string, string>(accessToken, uid);
        }

        public async Task<WeiboUserInfo> GetUserInfoAsync(string code)
        {
            var userInfo = new WeiboUserInfo();
            var pair = GetAccessTokenAndUId(code);
            userInfo.UnionId = pair.Value;
            var url = $"https://api.weibo.com/2/users/show.json?access_token={pair.Key}&uid={pair.Value}";
            var result = await LoginManager.GetStringAsync(url);

            var data = JObject.Parse(result);
            userInfo.Name = data["name"].Value<string>();
            userInfo.ScreenName = data["screen_name"].Value<string>();
            userInfo.AvatarLarge = data["avatar_large"].Value<string>();

            return userInfo;
        }

        public HttpResponseMessage HttpPost(string api, Dictionary<string, string> dict)
        {
            return HttpPostAsync(api, dict).Result;
        }

        public virtual Task<HttpResponseMessage> HttpPostAsync(string api, Dictionary<string, string> dict)
        {
            HttpContent httpContent = new FormUrlEncodedContent(dict);

            var handler = new HttpClientHandler();
            var http = new HttpClient(handler);

            return http.PostAsync(api, httpContent);
        }
    }
}
