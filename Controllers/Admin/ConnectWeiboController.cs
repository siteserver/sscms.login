using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Configuration;
using SSCMS.Login.Abstractions;
using SSCMS.Login.Core;
using SSCMS.Login.Models;
using SSCMS.Services;

namespace SSCMS.Login.Controllers.Admin
{
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ConnectWeiboController : ControllerBase
    {
        private const string Route = "login/connectWeibo";

        private readonly IAuthManager _authManager;
        private readonly ILoginManager _loginManager;

        public ConnectWeiboController(IAuthManager authManager, ILoginManager loginManager)
        {
            _authManager = authManager;
            _loginManager = loginManager;
        }

        public class GetResult
        {
            public WeiboSettings Settings { get; set; }
            public string Url { get; set; }
        }
    }
}
