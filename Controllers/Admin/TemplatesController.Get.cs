using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Core;

namespace SSCMS.Login.Controllers.Admin
{
    public partial class TemplatesController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<ListResult>> Get([FromQuery] ListRequest request)
        {
            if (!await _authManager.HasAppPermissionsAsync(LoginManager.PermissionsTemplates))
                return Unauthorized();

            var templateInfoList = _loginManager.GetTemplateInfoList(request.Type);

            return new ListResult
            {
                TemplateInfoList = templateInfoList
            };
        }
    }
}
