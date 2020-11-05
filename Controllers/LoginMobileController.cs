using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Abstractions;
using SSCMS.Login.Core;
using SSCMS.Models;
using SSCMS.Repositories;
using SSCMS.Services;

namespace SSCMS.Login.Controllers
{
    [Route("api/login/login")]
    public partial class LoginMobileController : ControllerBase
    {
        private const string Route = "";
        private const string RouteSendSms = "actions/sendSms";

        private readonly IAuthManager _authManager;
        private readonly IUserRepository _userRepository;
        private readonly ISmsManager _smsManager;
        private readonly ICacheManager _cacheManager;

        public LoginMobileController(IAuthManager authManager, IUserRepository userRepository, ISmsManager smsManager, ICacheManager cacheManager)
        {
            _authManager = authManager;
            _userRepository = userRepository;
            _smsManager = smsManager;
            _cacheManager = cacheManager;
        }

        public class GetResult
        {
            public User User { get; set; }
            public string Token { get; set; }
        }

        public class SendSmsRequest
        {
            public string Mobile { get; set; }
        }

        public class SubmitRequest
        {
            public string Mobile { get; set; }
            public string Code { get; set; }
        }

        private string GetSmsCodeCacheKey(string mobile)
        {
            return CacheUtils.GetCacheKey(nameof(LoginMobileController), nameof(Administrator), mobile);
        }
    }
}
