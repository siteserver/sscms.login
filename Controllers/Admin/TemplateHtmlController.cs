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
    public partial class TemplateHtmlController : ControllerBase
    {
        private const string Route = "login/templateHtml";

        private readonly IAuthManager _authManager;
        private readonly ILoginManager _loginManager;

        public TemplateHtmlController(IAuthManager authManager, ILoginManager loginManager)
        {
            _authManager = authManager;
            _loginManager = loginManager;
        }

        public class GetRequest
        {
            public string Type { get; set; }
            public string Name { get; set; }
        }

        public class GetResult
        {
            public TemplateInfo TemplateInfo { get; set; }
            public string TemplateHtml { get; set; }
            public bool IsSystem { get; set; }
        }

        public class SubmitRequest
        {
            public string Name { get; set; }
            public string TemplateHtml { get; set; }
        }
    }
}
