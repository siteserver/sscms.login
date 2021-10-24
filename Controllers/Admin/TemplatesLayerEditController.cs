using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Configuration;
using SSCMS.Dto;
using SSCMS.Login.Abstractions;
using SSCMS.Login.Models;
using SSCMS.Services;

namespace SSCMS.Login.Controllers.Admin
{
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class TemplatesLayerEditController : ControllerBase
    {
        private const string Route = "login/templatesLayerEdit";
        private const string RouteUpdate = "login/templatesLayerEdit/actions/update";

        private readonly IAuthManager _authManager;
        private readonly ILoginManager _loginManager;

        public TemplatesLayerEditController(IAuthManager authManager, ILoginManager loginManager)
        {
            _authManager = authManager;
            _loginManager = loginManager;
        }

        public class GetRequest
        {
            public string Name { get; set; }
        }

        public class GetResult
        {
            public TemplateInfo TemplateInfo { get; set; }
        }

        public class CloneRequest
        {
            public string Type { get; set; }
            public string OriginalName { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string TemplateHtml { get; set; }
        }

        public class UpdateRequest
        {
            public string Type { get; set; }
            public string OriginalName { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
