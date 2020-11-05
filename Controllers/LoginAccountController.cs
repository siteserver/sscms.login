using Microsoft.AspNetCore.Mvc;
using SSCMS.Models;
using SSCMS.Repositories;
using SSCMS.Services;

namespace SSCMS.Login.Controllers
{
    [Route("api/login/login")]
    public partial class LoginAccountController : ControllerBase
    {
        private const string Route = "";

        private readonly IAuthManager _authManager;
        private readonly IUserRepository _userRepository;

        public LoginAccountController(IAuthManager authManager, IUserRepository userRepository)
        {
            _authManager = authManager;
            _userRepository = userRepository;
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
