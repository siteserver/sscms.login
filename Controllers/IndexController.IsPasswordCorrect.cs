using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SSCMS.Login.Controllers
{
    public partial class IndexController
    {
        [HttpPost, Route(RouteIsPasswordCorrect)]
        public async Task<ActionResult<IsPasswordCorrectResult>> IsPasswordCorrect(
            [FromBody] IsPasswordCorrectRequest request)
        {
            var (isCorrect, errorMessage) = await _userRepository.IsPasswordCorrectAsync(request.Password);

            return new IsPasswordCorrectResult
            {
                IsCorrect = isCorrect,
                ErrorMessage = errorMessage
            };
        }
    }
}
