using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Configuration;
using SSCMS.Enums;
using SSCMS.Utils;

namespace SSCMS.Login.Controllers
{
    public partial class AccountController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<GetResult>> Submit([FromBody] SubmitRequest request)
        {
            var (user, userName, errorMessage) =
                await _userRepository.ValidateAsync(request.Account, request.Password, false);

            if (user == null)
            {
                user = await _userRepository.GetByUserNameAsync(userName);
                if (user != null)
                {
                    user.CountOfFailedLogin += 1;
                    user.LastActivityDate = DateTime.Now;
                    await _userRepository.UpdateAsync(user);
                }

                return this.Error(errorMessage);
            }

            await _userRepository.UpdateLastActivityDateAndCountOfLoginAsync(user);
            await _statRepository.AddCountAsync(StatType.UserLogin);
            await _logRepository.AddUserLogAsync(user, Constants.ActionsLoginSuccess);

            var token = _authManager.AuthenticateUser(user, true);

            return new GetResult
            {
                User = user,
                Token = token
            };
        }
    }
}
