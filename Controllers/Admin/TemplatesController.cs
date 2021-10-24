using System.Collections.Generic;
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
    public partial class TemplatesController : ControllerBase
    {
        private const string Route = "login/templates";
        private const string RouteDelete = "login/templates/actions/delete";

        private readonly IAuthManager _authManager;
        private readonly ILoginManager _loginManager;

        public TemplatesController(IAuthManager authManager, ILoginManager loginManager)
        {
            _authManager = authManager;
            _loginManager = loginManager;
        }

        public class ListRequest
        {
            public string Type { get; set; }
        }

        public class ListResult
        {
            public List<TemplateInfo> TemplateInfoList { get; set; }
        }

        public class DeleteRequest
        {
            public string Type { get; set; }
            public string Name { get; set; }
        }
    }
}
