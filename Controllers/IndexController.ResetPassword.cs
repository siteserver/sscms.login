using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;
using SSCMS.Utils;

namespace SSCMS.Login.Controllers
{
    public partial class IndexController
    {
        [HttpPost, Route(RouteResetPassword)]
        public async Task<ActionResult<BoolResult>> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!_authManager.IsUser)
            {
                return this.Error("用户未登录");
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return this.Error("确认密码与新密码不一致，请重新输入");
            }

            var (user, _, _) = await _userRepository.ValidateAsync(request.Account, request.Password, false);

            if (string.IsNullOrEmpty(request.Password) || user == null)
            {
                return this.Error("原密码输入错误，请重新输入");
            }

            if (request.Password == request.NewPassword)
            {
                return this.Error("新密码不能与原密码一致，请重新输入");
            }

            var (success, errorMessage) = await _userRepository.ChangePasswordAsync(user.Id, request.NewPassword);
            if (!success)
            {
                return this.Error(errorMessage);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
