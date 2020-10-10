using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;
using SSCMS.Models;
using SSCMS.Utils;

namespace SSCMS.Login.Controllers
{
    public partial class ActionsController
    {
        [HttpPost, Route(RouteRegister)]
        public async Task<ActionResult<BoolResult>> Register([FromBody] RegisterRequest request)
        {
            var user = new User
            {
                UserName = request.UserName,
                DisplayName = request.DisplayName,
                Email = request.Email,
                Mobile = request.Mobile
            };

            var (newUser, errorMessage) =
                await _userRepository.InsertAsync(user, request.Password, PageUtils.GetIpAddress(Request));
            if (newUser == null)
            {
                return BadRequest(errorMessage);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
