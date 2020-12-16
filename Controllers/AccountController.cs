using Microsoft.AspNetCore.Mvc;
using SSCMS.Models;
using SSCMS.Repositories;
using SSCMS.Services;

namespace SSCMS.Login.Controllers
{
    [Route("api/login/account")]
    public partial class AccountController : ControllerBase
    {
        private const string Route = "";

        private readonly IAuthManager _authManager;
        private readonly IUserRepository _userRepository;
        private readonly IStatRepository _statRepository;
        private readonly ILogRepository _logRepository;

        public AccountController(IAuthManager authManager, IUserRepository userRepository, IStatRepository statRepository, ILogRepository logRepository)
        {
            _authManager = authManager;
            _userRepository = userRepository;
            _statRepository = statRepository;
            _logRepository = logRepository;
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
