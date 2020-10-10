using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Core;

namespace SSCMS.Login.Controllers
{
    public partial class AuthController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<RedirectResult>> GetAuth([FromRoute] string type, [FromQuery] GetAuthRequest request)
        {
            var oAuthType = OAuthType.Parse(type);
            var redirectUrl = request.RedirectUrl;
            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = ApiUtils.GetHomeUrl();
            }

            var url = string.Empty;

            if (oAuthType == OAuthType.Weixin)
            {
                var settings = await _loginManager.GetWeixinSettingsAsync();
                var client = new WeixinClient(settings.WeixinAppId, settings.WeixinAppSecret, redirectUrl);
                url = client.GetAuthorizationUrl();
            }
            else if (oAuthType == OAuthType.Qq)
            {
                var settings = await _loginManager.GetQqSettingsAsync();
                var client = new QqClient(settings.QqAppId, settings.QqAppKey, redirectUrl);
                url = client.GetAuthorizationUrl();
            }
            else if (oAuthType == OAuthType.Weibo)
            {
                var settings = await _loginManager.GetWeiboSettingsAsync();
                var client = new WeiboClient(settings.WeiboAppKey, settings.WeiboAppSecret, redirectUrl);
                url = client.GetAuthorizationUrl();
            }

            if (!string.IsNullOrEmpty(url))
            {
                return Redirect(url);
            }

            return BadRequest("类型不正确");
        }
    }
}
