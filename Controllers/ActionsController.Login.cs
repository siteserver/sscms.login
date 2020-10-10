using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;

namespace SSCMS.Login.Controllers
{
    public partial class ActionsController
    {
        [HttpPost, Route(RouteLogin)]
        public async Task<ActionResult<StringResult>> Login([FromBody] LoginRequest request)
        {
            var (user, userName, errorMessage) =
                await _userRepository.ValidateAsync(request.Account, request.Password, true);

            if (user == null)
            {
                user = await _userRepository.GetByUserNameAsync(userName);
                if (user != null)
                {
                    user.CountOfFailedLogin += 1;
                    user.LastActivityDate = DateTime.Now;
                    await _userRepository.UpdateAsync(user);
                }

                return BadRequest(errorMessage);
            }

            await _userRepository.UpdateLastActivityDateAndCountOfLoginAsync(user);

            var token = _authManager.AuthenticateUser(user, true);

            return new StringResult
            {
                Value = token
            };
        }
    }
}
