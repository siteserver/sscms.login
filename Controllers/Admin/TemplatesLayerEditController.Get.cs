using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Core;
using SSCMS.Login.Models;

namespace SSCMS.Login.Controllers.Admin
{
    public partial class TemplatesLayerEditController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasAppPermissionsAsync(LoginManager.PermissionsTemplates))
                return Unauthorized();

            var templateInfo = _loginManager.GetTemplateInfo(request.Name);

            if (!string.IsNullOrEmpty(templateInfo.Publisher))
            {
                templateInfo = new TemplateInfo();
            }

            return new GetResult
            {
                TemplateInfo = templateInfo
            };
        }
    }
}
