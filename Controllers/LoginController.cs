using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Configuration;
using SSCMS.Login.Abstractions;
using SSCMS.Models;
using SSCMS.Repositories;
using SSCMS.Services;

namespace SSCMS.Login.Controllers
{
    [Route("api/login/login")]
    public partial class LoginController : ControllerBase
    {
        private const string Route = "";

        private readonly IAuthManager _authManager;
        private readonly IUserRepository _userRepository;
        private readonly ILoginManager _loginManager;

        public LoginController(IAuthManager authManager, IUserRepository userRepository, ILoginManager loginManager)
        {
            _authManager = authManager;
            _userRepository = userRepository;
            _loginManager = loginManager;
        }

        public class GetResult
        {
            public User User { get; set; }
            public string Token { get; set; }
        }

        public class SubmitRequest
        {
            public string Account { get; set; }
            public string Password { get; set; }
        }
    }
}
