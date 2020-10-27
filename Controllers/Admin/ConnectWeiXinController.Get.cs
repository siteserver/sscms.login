using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Core;

namespace SSCMS.Login.Controllers.Admin
{
    public partial class ConnectWeiXinController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            if (!await _authManager.HasAppPermissionsAsync(LoginManager.PermissionsLoginWeixin))
            {
                return Unauthorized();
            }

            var settings = await _loginManager.GetWeixinSettingsAsync();
            var url = string.Empty;
            if (settings.IsWeixin)
            {
                url = ApiUtils.GetAuthUrl(OAuthType.Weixin, ApiUtils.GetHomeUrl());
            }

            return new GetResult
            {
                Settings = settings,
                Url = url
            };
        }
    }
}
