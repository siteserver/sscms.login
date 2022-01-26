using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using SSCMS.Login.Models;

namespace SSCMS.Login.Core
{
    //https://open.weixin.qq.com/cgi-bin/showdocument?action=dir_list&t=resource/res_list&verify=1&id=open1419316505&token=&lang=zh_CN

    public class WeixinClient
    {
        public string AppId { get; }
        public string AppSecret { get; }
        public string RedirectUrl { get; }

        public WeixinClient(string appId, string appSecret, string host, string redirectUrl)
        {
            AppId = appId;
            AppSecret = appSecret;
            RedirectUrl = ApiUtils.GetAuthRedirectUrl(host, OAuthType.Weixin, redirectUrl);
        }

        public string GetAuthorizationUrl()
        {
            return
                $"https://open.weixin.qq.com/connect/qrconnect?appid={AppId}&redirect_uri={HttpUtility.UrlEncode(RedirectUrl)}&response_type=code&scope=snsapi_login&state=STATE#wechat_redirect";
        }

        private async Task<KeyValuePair<string, string>> GetAccessTokenAndOpenIdAsync(string code)
        {
            var url =
                $"https://api.weixin.qq.com/sns/oauth2/access_token?appid={AppId}&secret={AppSecret}&code={code}&grant_type=authorization_code";

            var result = await LoginManager.GetStringAsync(url);

            if (result.Contains("errmsg"))
            {
                throw new Exception(result);
            }

            var data = JObject.Parse(result);

            var accessToken = data["access_token"].Value<string>();
            var openId = data["openid"].Value<string>();

            return new KeyValuePair<string, string>(accessToken, openId);
        }

        public async Task<WeixinUserInfo> GetUserInfoAsync(string code)
        {
            var userInfo = new WeixinUserInfo();

            var pair = await GetAccessTokenAndOpenIdAsync(code);
            var url = $"https://api.weixin.qq.com/sns/userinfo?access_token={pair.Key}&openid={pair.Value}";
            var result = await LoginManager.GetStringAsync(url);

            if (result.Contains("errmsg"))
            {
                throw new Exception(result);
            }

            var data = JObject.Parse(result);
            userInfo.Nickname = data["nickname"].Value<string>();
            userInfo.HeadImgUrl = data["headimgurl"].Value<string>();
            userInfo.UnionId = data["unionid"].Value<string>();

            return userInfo;
        }
    }
}
