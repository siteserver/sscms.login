using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;

namespace SSCMS.Login.Controllers
{
    public partial class ActionsController
    {
        [HttpPost, Route(RouteIsMobileExists)]
        public async Task<ActionResult<BoolResult>> IsMobileExists([FromBody] IsMobileExistsRequest request)
        {
            return new BoolResult
            {
                Value = await _userRepository.IsMobileExistsAsync(request.Mobile)
            };
        }
    }
}
