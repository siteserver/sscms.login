using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Utils;

namespace SSCMS.Login.Controllers
{
    public partial class LoginMobileController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<GetResult>> Submit([FromBody] SubmitRequest request)
        {
            var user = await _userRepository.GetByMobileAsync(request.Mobile);

            if (user == null)
            {
                return this.Error("此手机号码未关联用户，请更换手机号码");
            }

            var codeCacheKey = GetSmsCodeCacheKey(request.Mobile);
            var code = _cacheManager.Get<int>(codeCacheKey);
            if (code == 0 || TranslateUtils.ToInt(request.Code) != code)
            {
                return this.Error("输入的验证码有误或验证码已超时，请重试");
            }

            user.CountOfFailedLogin += 1;
            user.LastActivityDate = DateTime.Now;
            await _userRepository.UpdateAsync(user);

            await _userRepository.UpdateLastActivityDateAndCountOfLoginAsync(user);

            var token = _authManager.AuthenticateUser(user, true);

            return new GetResult
            {
                User = user,
                Token = token
            };
        }
    }
}
