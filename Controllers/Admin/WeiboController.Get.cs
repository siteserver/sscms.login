using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Core;

namespace SSCMS.Login.Controllers.Admin
{
    public partial class WeiboController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            if (!await _authManager.HasAppPermissionsAsync(LoginManager.PermissionsLoginWeibo))
            {
                return Unauthorized();
            }

            var settings = await _loginManager.GetWeiboSettingsAsync();
            var url = string.Empty;
            if (settings.IsWeibo)
            {
                url = ApiUtils.GetAuthUrl(OAuthType.Weibo, ApiUtils.GetHomeUrl());
            }

            return new GetResult
            {
                Settings = settings,
                Url = url
            };
        }
    }
}
