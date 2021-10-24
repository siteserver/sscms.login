using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Core;

namespace SSCMS.Login.Controllers.Admin
{
    public partial class TemplatesController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<ListResult>> Delete([FromBody] DeleteRequest request)
        {
            if (!await _authManager.HasAppPermissionsAsync(LoginManager.PermissionsTemplates))
                return Unauthorized();

            _loginManager.DeleteTemplate(request.Name);

            return new ListResult
            {
                TemplateInfoList = _loginManager.GetTemplateInfoList(request.Type)
            };
        }
    }
}
