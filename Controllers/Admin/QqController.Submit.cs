using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;
using SSCMS.Login.Core;
using SSCMS.Login.Models;

namespace SSCMS.Login.Controllers.Admin
{
    public partial class QqController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<StringResult>> Submit([FromBody] QqSettings request)
        {
            if (!await _authManager.HasAppPermissionsAsync(LoginManager.PermissionsLoginQq))
            {
                return Unauthorized();
            }

            await _loginManager.SetQqSettingsAsync(request);

            var url = string.Empty;
            if (request.IsQq)
            {
                url = ApiUtils.GetAuthUrl(OAuthType.Qq, ApiUtils.GetHomeUrl());
            }

            return new StringResult
            {
                Value = url
            };
        }
    }
}
