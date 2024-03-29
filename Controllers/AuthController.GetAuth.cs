﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Core;
using SSCMS.Utils;

namespace SSCMS.Login.Controllers
{
    public partial class AuthController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult> GetAuth([FromRoute] string type, [FromQuery] GetAuthRequest request)
        {
            var oAuthType = OAuthType.Parse(type);
            var host = ApiUtils.GetHost(Request);
            var redirectUrl = request.RedirectUrl;
            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = ApiUtils.GetHomeUrl();
            } 

            var url = string.Empty;

            if (oAuthType == OAuthType.Weixin)
            {
                var settings = await _loginManager.GetWeixinSettingsAsync();
                var client = new WeixinClient(settings.WeixinAppId, settings.WeixinAppSecret, host, redirectUrl);
                url = client.GetAuthorizationUrl();
            }
            else if (oAuthType == OAuthType.Qq)
            {
                var settings = await _loginManager.GetQqSettingsAsync();
                var client = new QqClient(settings.QqAppId, settings.QqAppKey, host, redirectUrl);
                url = client.GetAuthorizationUrl();
            }
            else if (oAuthType == OAuthType.Weibo)
            {
                var settings = await _loginManager.GetWeiboSettingsAsync();
                var client = new WeiboClient(settings.WeiboAppKey, settings.WeiboAppSecret, host, redirectUrl);
                url = client.GetAuthorizationUrl();
            }

            if (!string.IsNullOrEmpty(url))
            {
                return Redirect(url);
            }

            return this.Error("类型不正确");
        }
    }
}
