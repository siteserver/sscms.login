using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;
using SSCMS.Login.Core;

namespace SSCMS.Login.Controllers.Admin
{
    public partial class TemplateHtmlController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] SubmitRequest request)
        {
            if (!await _authManager.HasAppPermissionsAsync(LoginManager.PermissionsTemplates))
                return Unauthorized();

            var templateInfo = _loginManager.GetTemplateInfo(request.Name);

            _loginManager.SetTemplateHtml(templateInfo, request.TemplateHtml);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
