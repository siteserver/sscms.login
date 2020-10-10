using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;
using SSCMS.Login.Core;
using SSCMS.Login.Models;

namespace SSCMS.Login.Controllers.Admin
{
    public partial class WeiXinController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<StringResult>> Submit([FromBody] WeixinSettings request)
        {
            if (!await _authManager.HasAppPermissionsAsync(LoginManager.PermissionsLoginWeixin))
            {
                return Unauthorized();
            }

            await _loginManager.SetWeixinSettingsAsync(request);
            var url = string.Empty;
            if (request.IsWeixin)
            {
                url = ApiUtils.GetAuthUrl(OAuthType.Weixin, ApiUtils.GetHomeUrl());
            }

            return new StringResult
            {
                Value = url
            };
        }
    }
}
