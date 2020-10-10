using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Core;

namespace SSCMS.Login.Controllers.Admin
{
    public partial class QqController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            if (!await _authManager.HasAppPermissionsAsync(LoginManager.PermissionsLoginQq))
            {
                return Unauthorized();
            }

            var settings = await _loginManager.GetQqSettingsAsync();
            var url = string.Empty;
            if (settings.IsQq)
            {
                url = ApiUtils.GetAuthUrl(OAuthType.Qq, ApiUtils.GetHomeUrl());
            }

            return new GetResult
            {
                Settings = settings,
                Url = url
            };
        }
    }
}
