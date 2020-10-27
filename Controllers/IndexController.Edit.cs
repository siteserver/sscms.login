using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Models;
using SSCMS.Utils;

namespace SSCMS.Login.Controllers
{
    public partial class IndexController
    {
        [HttpPost, Route(RouteEdit)]
        public async Task<ActionResult<User>> Edit([FromBody] EditRequest request)
        {
            if (!_authManager.IsUser)
            {
                return this.Error("用户认证失败");
            }

            var user = await _authManager.GetUserAsync();

            if (user == null)
            {
                return this.Error("用户认证失败");
            }

            user.AvatarUrl = request.AvatarUrl;
            user.DisplayName = request.DisplayName;

            if (!string.IsNullOrEmpty(request.Mobile))
            {
                if (request.Mobile != user.Mobile)
                {
                    var exists = await _userRepository.IsMobileExistsAsync(request.Mobile);
                    if (!exists)
                    {
                        user.Mobile = request.Mobile;
                        await _logRepository.AddUserLogAsync(user, "修改手机号码", request.Mobile);
                    }
                    else
                    {
                        return this.Error("此手机号码已注册，请更换手机号码");
                    }
                }
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                if (request.Email != user.Email)
                {
                    var exists = await _userRepository.IsEmailExistsAsync(request.Email);
                    if (!exists)
                    {
                        user.Email = request.Email;
                        await _logRepository.AddUserLogAsync(user, "修改邮箱", request.Email);
                    }
                    else
                    {
                        return this.Error("此邮箱已注册，请更换邮箱");
                    }
                }
            }

            await _userRepository.UpdateAsync(user);

            return user;
        }
    }
}
