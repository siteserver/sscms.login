using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Login.Core;
using SSCMS.Models;

namespace SSCMS.Login.Controllers
{
    public partial class IndexController
    {
        [HttpPost, Route(RouteIsCodeCorrect)]
        public async Task<ActionResult<IsCodeCorrectResult>> IsCodeCorrect([FromBody] IsCodeCorrectRequest request)
        {
            var cacheKey = CacheUtils.GetCacheKey(nameof(IndexController), nameof(Administrator), request.Mobile);
            var dbCode = _cacheManager.Get<int>(cacheKey);

            var isCorrect = !(dbCode == 0 || request.Code != dbCode.ToString());

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
