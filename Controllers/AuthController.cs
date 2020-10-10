using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Abstractions;
using SSCMS.Repositories;
using SSCMS.Services;

namespace SSCMS.Login.Controllers
{
    [Route("api/login/auth")]
    public partial class AuthController : ControllerBase
    {
        private const string Route = "{type}";
        private const string RouteRedirect = "{type}/redirect";

        private readonly IAuthManager _authManager;
        private readonly IUserRepository _userRepository;
        private readonly IOAuthRepository _oAuthRepository;
        private readonly ILoginManager _loginManager;

        public AuthController(IAuthManager authManager, IUserRepository userRepository, IOAuthRepository oAuthRepository, ILoginManager loginManager)
        {
            _authManager = authManager;
            _userRepository = userRepository;
            _oAuthRepository = oAuthRepository;
            _loginManager = loginManager;
        }

        public class GetAuthRequest
        {
            public string RedirectUrl { get; set; }
        }

        public class GetRedirectRequest
        {
            public string RedirectUrl { get; set; }
            public string Code { get; set; }
        }

        public class GetRedirectResult
        {
            public string RedirectUrl { get; set; }
            public string Token { get; set; }
        }
    }
}
