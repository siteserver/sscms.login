﻿using Microsoft.AspNetCore.Mvc;
using SSCMS.Repositories;
using SSCMS.Services;

namespace SSCMS.Login.Controllers
{
    [Route("api/login/actions")]
    public partial class ActionsController : ControllerBase
    {
        private const string RouteRegister = "register";
        private const string RouteLogin = "login";
        private const string RouteResetPassword = "resetpassword";
        private const string RouteEdit = "edit";
        private const string RouteIsMobileExists = "ismobileexists";
        private const string RouteIsPasswordCorrect = "ispasswordcorrect";
        private const string RouteIsCodeCorrect = "iscodecorrect";

        private readonly IAuthManager _authManager;
        private readonly IUserRepository _userRepository;
        private readonly ILogRepository _logRepository;

        public ActionsController(IAuthManager authManager, IUserRepository userRepository, ILogRepository logRepository)
        {
            _authManager = authManager;
            _userRepository = userRepository;
            _logRepository = logRepository;
        }

        private static string GetSendSmsCacheKey(string mobile)
        {
            return $"SS.Home.Api.ActionsPost.SendSms.{mobile}.Code";
        }

        public class EditRequest
        {
            public string AvatarUrl { get; set; }
            public string DisplayName { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
        }

        public class IsCodeCorrectRequest
        {
            public string Mobile { get; set; }
            public string Code { get; set; }
        }

        public class IsCodeCorrectResult
        {
            public bool IsCorrect { get; set; }
            public string Token { get; set; }
        }

        public class IsMobileExistsRequest
        {
            public string Mobile { get; set; }
        }

        public class IsPasswordCorrectRequest
        {
            public string Password { get; set; }
        }

        public class IsPasswordCorrectResult
        {
            public bool IsCorrect { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class LoginRequest
        {
            public string Account { get; set; }
            public string Password { get; set; }
        }

        public class RegisterRequest
        {
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public string Password { get; set; }
        }

        public class ResetPasswordRequest
        {
            public string Account { get; set; }
            public string Password { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }
    }
}
