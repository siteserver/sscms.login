using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;
using SSCMS.Login.Core;
using SSCMS.Login.Models;
using SSCMS.Utils;

namespace SSCMS.Login.Controllers.Admin
{
    public partial class TemplatesLayerEditController
    {
        [HttpPost, Route(RouteUpdate)]
        public async Task<ActionResult<BoolResult>> Update([FromBody] UpdateRequest request)
        {
            if (!await _authManager.HasAppPermissionsAsync(LoginManager.PermissionsTemplates))
                return Unauthorized();

            if (StringUtils.EqualsIgnoreCase(request.OriginalName, request.Name))
            {
                var templateInfoList = _loginManager.GetTemplateInfoList(request.Type);
                var originalTemplateInfo = templateInfoList.First(x => StringUtils.EqualsIgnoreCase(request.OriginalName, x.Name));

                originalTemplateInfo.Name = request.Name;
                originalTemplateInfo.Description = request.Description;
                _loginManager.Edit(originalTemplateInfo);
            }
            else
            {
                var templateInfoList = _loginManager.GetTemplateInfoList(request.Type);
                var originalTemplateInfo = templateInfoList.First(x => StringUtils.EqualsIgnoreCase(request.OriginalName, x.Name));

                if (templateInfoList.Any(x => StringUtils.EqualsIgnoreCase(request.Name, x.Name)))
                {
                    return this.Error($"标识为 {request.Name} 的模板已存在，请更换模板标识！");
                }

                var templateInfo = new TemplateInfo
                {
                    Name = request.Name,
                    Type = request.Type,
                    Main = originalTemplateInfo.Main,
                    Publisher = string.Empty,
                    Description = request.Description,
                    Icon = originalTemplateInfo.Icon
                };
                templateInfoList.Add(templateInfo);

                _loginManager.Clone(request.OriginalName, templateInfo);

                _loginManager.DeleteTemplate(request.OriginalName);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
