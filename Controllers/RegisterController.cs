using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Configuration;
using SSCMS.Models;
using SSCMS.Repositories;

namespace SSCMS.Login.Controllers
{
    [Route("api/login/register")]
    public partial class RegisterController : ControllerBase
    {
        private const string Route = "";

        private readonly IConfigRepository _configRepository;
        private readonly ITableStyleRepository _tableStyleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IStatRepository _statRepository;

        public RegisterController(IConfigRepository configRepository, ITableStyleRepository tableStyleRepository, IUserRepository userRepository, IUserGroupRepository userGroupRepository, IStatRepository statRepository)
        {
            _configRepository = configRepository;
            _tableStyleRepository = tableStyleRepository;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _statRepository = statRepository;
        }

        public class GetResult
        {
            public bool IsUserRegistrationGroup { get; set; }
            public bool IsHomeAgreement { get; set; }
            public string HomeAgreementHtml { get; set; }
            public IEnumerable<InputStyle> Styles { get; set; }
            public IEnumerable<UserGroup> Groups { get; set; }
        }
    }
}
