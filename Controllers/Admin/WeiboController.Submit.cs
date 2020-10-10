using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;
using SSCMS.Login.Core;
using SSCMS.Login.Models;

namespace SSCMS.Login.Controllers.Admin
{
    public partial class WeiboController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<StringResult>> Submit([FromBody] WeiboSettings request)
        {
            if (!await _authManager.HasAppPermissionsAsync(LoginManager.PermissionsLoginWeibo))
            {
                return Unauthorized();
            }

            await _loginManager.SetWeiboSettingsAsync(request);

            var url = string.Empty;
            if (request.IsWeibo)
            {
                url = ApiUtils.GetAuthUrl(OAuthType.Weibo, ApiUtils.GetHomeUrl());
            }

            return new StringResult
            {
                Value = url
            };
        }
    }
}
