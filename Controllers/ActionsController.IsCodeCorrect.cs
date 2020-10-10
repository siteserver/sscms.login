using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Core;

namespace SSCMS.Login.Controllers
{
    public partial class ActionsController
    {
        [HttpPost, Route(RouteIsCodeCorrect)]
        public async Task<ActionResult<IsCodeCorrectResult>> IsCodeCorrect([FromBody] IsCodeCorrectRequest request)
        {
            var dbCode = CacheUtils.Get<string>(GetSendSmsCacheKey(request.Mobile));

            var isCorrect = request.Code == dbCode;
            var token = string.Empty;
            if (isCorrect)
            {
                var user = await _userRepository.GetByMobileAsync(request.Mobile);
                if (user != null)
                {
                    token = _authManager.AuthenticateUser(user, true);
                }
            }

            return new IsCodeCorrectResult
            {
                IsCorrect = isCorrect,
                Token = token
            };
        }
    }
}
